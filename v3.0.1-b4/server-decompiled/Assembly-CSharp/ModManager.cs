using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x0200073D RID: 1853
public static class ModManager
{
	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06003741 RID: 14145 RVA: 0x0016C1C8 File Offset: 0x0016A3C8
	public static string ModsBasePath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameIO.GetDeviceLocalUserGameDataDir() + "/Mods";
		}
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void initModManager()
	{
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x0016C1DC File Offset: 0x0016A3DC
	public static void LoadMods()
	{
		ModManager.initModManager();
		bool flag = ModManager.loadModsFromFolder(ModManager.ModsBasePath);
		bool flag2 = GameIO.PathsEquals(ModManager.ModsBasePath, ModManager.ModsBasePathLegacy, true) || ModManager.loadModsFromFolder(ModManager.ModsBasePathLegacy);
		if (!flag && !flag2)
		{
			Log.Out("[MODS] No mods folder found");
			return;
		}
		int num = ModManager.loadedMods.list.FindIndex((Mod _mod) => _mod.Name == "TFP_Harmony");
		if (num >= 0)
		{
			Mod mod = ModManager.loadedMods.list[num];
			ModManager.loadedMods.list.RemoveAt(num);
			ModManager.loadedMods.list.Insert(0, mod);
		}
		Log.Out("[MODS] Initializing mod code");
		foreach (Mod mod2 in ModManager.loadedMods.list)
		{
			mod2.InitModCode();
		}
		Log.Out("[MODS] Loading done");
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x0016C2EC File Offset: 0x0016A4EC
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool loadModsFromFolder(string _folder)
	{
		if (!SdDirectory.Exists(_folder))
		{
			return false;
		}
		Log.Out("[MODS] Start loading from: '" + _folder + "'");
		string[] directories = SdDirectory.GetDirectories(_folder);
		Array.Sort<string>(directories);
		foreach (string text in directories)
		{
			Log.Out("[MODS]   Trying to load from folder: '" + Path.GetFileName(text) + "'");
			try
			{
				Mod mod = Mod.LoadDefinitionFromFolder(text);
				if (mod != null)
				{
					if (!mod.LoadMod())
					{
						ModManager.failedMods.Add(mod);
					}
					else
					{
						ModManager.loadedMods.Add(mod.Name, mod);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("[MODS]     Failed loading mod from folder: '" + Path.GetFileName(text) + "'");
				Log.Exception(ex);
			}
		}
		return true;
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x0016C3B8 File Offset: 0x0016A5B8
	public static bool ModLoaded(string _modName)
	{
		return ModManager.loadedMods.dict.ContainsKey(_modName);
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x0016C3CA File Offset: 0x0016A5CA
	public static Mod GetMod(string _modName, bool _onlyLoaded = false)
	{
		if (!ModManager.ModLoaded(_modName))
		{
			return null;
		}
		return ModManager.loadedMods.dict[_modName];
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x0016C3E6 File Offset: 0x0016A5E6
	public static List<Mod> GetLoadedMods()
	{
		return ModManager.loadedMods.list;
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x0016C3F4 File Offset: 0x0016A5F4
	public static List<Mod> GetFailedMods(Mod.EModLoadState? _failureReason = null)
	{
		if (_failureReason == null)
		{
			return ModManager.failedMods;
		}
		List<Mod> list = new List<Mod>();
		foreach (Mod mod in ModManager.failedMods)
		{
			if (mod.LoadState == _failureReason.Value)
			{
				list.Add(mod);
			}
		}
		return list;
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x0016C46C File Offset: 0x0016A66C
	public static List<Assembly> GetLoadedAssemblies()
	{
		List<Assembly> list = new List<Assembly>();
		for (int i = 0; i < ModManager.loadedMods.Count; i++)
		{
			Mod mod = ModManager.loadedMods.list[i];
			list.AddRange(mod.AllAssemblies);
		}
		return list;
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x0016C4B4 File Offset: 0x0016A6B4
	public static Mod GetModForAssembly(Assembly _asm)
	{
		for (int i = 0; i < ModManager.loadedMods.Count; i++)
		{
			Mod mod = ModManager.loadedMods.list[i];
			if (mod.ContainsAssembly(_asm))
			{
				return mod;
			}
		}
		return null;
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x0016C4F4 File Offset: 0x0016A6F4
	public static bool AnyConfigModActive()
	{
		for (int i = 0; i < ModManager.loadedMods.Count; i++)
		{
			if (ModManager.loadedMods.list[i].GameConfigMod)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x0016C530 File Offset: 0x0016A730
	public static bool TryPatchModPathString(string _pathString, out string _modPath)
	{
		_modPath = null;
		if (_pathString.IndexOf('@') < 0)
		{
			return false;
		}
		int num = _pathString.IndexOf("@modfolder(", StringComparison.OrdinalIgnoreCase);
		if (num < 0)
		{
			return false;
		}
		int num2 = _pathString.IndexOf("):", StringComparison.Ordinal);
		int num3 = num + "@modfolder(".Length;
		string text = _pathString.Substring(num3, num2 - num3);
		string text2 = _pathString.Substring(0, num);
		int num4 = num2 + 2;
		while (_pathString[num4] == '/')
		{
			num4++;
		}
		string text3 = _pathString.Substring(num4);
		Mod mod = ModManager.GetMod(text, true);
		if (mod != null)
		{
			_pathString = text2 + mod.Path + "/" + text3;
			_modPath = _pathString;
			return true;
		}
		Log.Error("[MODS] Mod reference for a mod that is not loaded: '" + text + "'");
		return false;
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x0016C5F4 File Offset: 0x0016A7F4
	public static string PatchModPathString(string _pathString)
	{
		string text;
		ModManager.TryPatchModPathString(_pathString, out text);
		return text;
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x0016C60B File Offset: 0x0016A80B
	public static IEnumerator LoadPatchStuff(bool _isLoadingInGame)
	{
		yield return ModManager.LoadUiAtlases(_isLoadingInGame);
		yield return ModManager.LoadLocalizations(_isLoadingInGame);
		yield break;
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x0016C61A File Offset: 0x0016A81A
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadUiAtlases(bool _isLoadingInGame)
	{
		if (GameManager.IsDedicatedServer)
		{
			yield break;
		}
		if (_isLoadingInGame)
		{
			yield break;
		}
		int num;
		for (int i = 0; i < ModManager.loadedMods.Count; i = num + 1)
		{
			Mod mod = ModManager.loadedMods.list[i];
			string text = mod.Path + "/UIAtlases";
			if (SdDirectory.Exists(text))
			{
				string[] array = null;
				try
				{
					array = SdDirectory.GetDirectories(text);
				}
				catch (Exception ex)
				{
					Log.Exception(ex);
				}
				if (array != null)
				{
					string[] array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						string text2 = array2[j];
						string fileName = Path.GetFileName(text2);
						ModManager.AtlasManagerEntry ame;
						if (!ModManager.atlasManagers.TryGetValue(fileName, out ame))
						{
							Log.Out(string.Concat(new string[] { "[MODS] Creating new atlas '", fileName, "' for mod '", mod.Name, "'" }));
							ModManager.RegisterAtlasManager(MultiSourceAtlasManager.Create(ModManager.atlasesParentGo, fileName), true, ModManager.defaultShader, null);
							ame = ModManager.atlasManagers[fileName];
						}
						yield return UIAtlasFromFolder.CreateUiAtlasFromFolder(text2, ame.Shader, delegate(UIAtlas _atlas)
						{
							_atlas.transform.parent = ame.Manager.transform;
							ame.Manager.AddAtlas(_atlas, _atlas.gameObject, _isLoadingInGame);
							Action<INGUIAtlas, bool> onNewAtlasLoaded = ame.OnNewAtlasLoaded;
							if (onNewAtlasLoaded == null)
							{
								return;
							}
							onNewAtlasLoaded(_atlas, _isLoadingInGame);
						});
					}
					array2 = null;
					mod = null;
				}
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0016C629 File Offset: 0x0016A829
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadLocalizations(bool _isLoadingInGame)
	{
		if (_isLoadingInGame)
		{
			yield break;
		}
		for (int i = 0; i < ModManager.loadedMods.Count; i++)
		{
			Mod mod = ModManager.loadedMods.list[i];
			string text = mod.Path + "/Config";
			if (SdDirectory.Exists(text))
			{
				try
				{
					Localization.LoadPatchDictionaries(mod.Name, text, _isLoadingInGame);
				}
				catch (Exception ex)
				{
					Log.Error("[MODS] Failed loading localization from mod: '" + mod.Name + "'");
					Log.Exception(ex);
				}
			}
		}
		Localization.WriteCsv();
		yield break;
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x0016C638 File Offset: 0x0016A838
	public static void ModAtlasesDefaults(GameObject _parentGo, Shader _defaultShader)
	{
		ModManager.atlasesParentGo = _parentGo;
		ModManager.defaultShader = _defaultShader;
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x0016C646 File Offset: 0x0016A846
	public static void RegisterAtlasManager(MultiSourceAtlasManager _atlasManager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded = null)
	{
		ModManager.atlasManagers.Add(_atlasManager.name, new ModManager.AtlasManagerEntry(_atlasManager, _createdByMod, _shader, _onNewAtlasLoaded));
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x0016C664 File Offset: 0x0016A864
	public static MultiSourceAtlasManager GetAtlasManager(string _name)
	{
		ModManager.AtlasManagerEntry atlasManagerEntry;
		if (!ModManager.atlasManagers.TryGetValue(_name, out atlasManagerEntry))
		{
			return null;
		}
		return atlasManagerEntry.Manager;
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x0016C688 File Offset: 0x0016A888
	public static void GameEnded()
	{
		foreach (KeyValuePair<string, ModManager.AtlasManagerEntry> keyValuePair in ModManager.atlasManagers)
		{
			keyValuePair.Value.Manager.CleanupAfterGame();
		}
		Localization.ReloadBaseLocalization();
		Localization.UnloadUnusedLanguages();
		ThreadManager.RunCoroutineSync(ModManager.LoadLocalizations(false));
	}

	// Token: 0x04002D25 RID: 11557
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string ModsBasePathLegacy = ((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXServer) ? (Application.dataPath + "/../../Mods") : (Application.dataPath + "/../Mods"));

	// Token: 0x04002D26 RID: 11558
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly DictionaryList<string, Mod> loadedMods = new DictionaryList<string, Mod>();

	// Token: 0x04002D27 RID: 11559
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Mod> failedMods = new List<Mod>();

	// Token: 0x04002D28 RID: 11560
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ModManager.AtlasManagerEntry> atlasManagers = new CaseInsensitiveStringDictionary<ModManager.AtlasManagerEntry>();

	// Token: 0x04002D29 RID: 11561
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject atlasesParentGo;

	// Token: 0x04002D2A RID: 11562
	[PublicizedFrom(EAccessModifier.Private)]
	public static Shader defaultShader;

	// Token: 0x0200073E RID: 1854
	[PublicizedFrom(EAccessModifier.Private)]
	public class AtlasManagerEntry
	{
		// Token: 0x06003756 RID: 14166 RVA: 0x0016C75D File Offset: 0x0016A95D
		public AtlasManagerEntry(MultiSourceAtlasManager _manager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded)
		{
			this.Manager = _manager;
			this.CreatedByMod = _createdByMod;
			this.Shader = _shader;
			this.OnNewAtlasLoaded = _onNewAtlasLoaded;
		}

		// Token: 0x04002D2B RID: 11563
		public readonly MultiSourceAtlasManager Manager;

		// Token: 0x04002D2C RID: 11564
		public readonly bool CreatedByMod;

		// Token: 0x04002D2D RID: 11565
		public readonly Shader Shader;

		// Token: 0x04002D2E RID: 11566
		public readonly Action<INGUIAtlas, bool> OnNewAtlasLoaded;
	}
}
