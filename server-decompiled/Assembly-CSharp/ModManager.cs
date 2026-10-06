using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x02000749 RID: 1865
public static class ModManager
{
	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x060037DA RID: 14298 RVA: 0x0016ED48 File Offset: 0x0016CF48
	public static string ModsBasePath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameIO.GetDeviceLocalUserGameDataDir() + "/Mods";
		}
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void initModManager()
	{
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x0016ED5C File Offset: 0x0016CF5C
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

	// Token: 0x060037DD RID: 14301 RVA: 0x0016EE6C File Offset: 0x0016D06C
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

	// Token: 0x060037DE RID: 14302 RVA: 0x0016EF38 File Offset: 0x0016D138
	public static bool ModLoaded(string _modName)
	{
		return ModManager.loadedMods.dict.ContainsKey(_modName);
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x0016EF4A File Offset: 0x0016D14A
	public static Mod GetMod(string _modName, bool _onlyLoaded = false)
	{
		if (!ModManager.ModLoaded(_modName))
		{
			return null;
		}
		return ModManager.loadedMods.dict[_modName];
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x0016EF66 File Offset: 0x0016D166
	public static List<Mod> GetLoadedMods()
	{
		return ModManager.loadedMods.list;
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x0016EF74 File Offset: 0x0016D174
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

	// Token: 0x060037E2 RID: 14306 RVA: 0x0016EFEC File Offset: 0x0016D1EC
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

	// Token: 0x060037E3 RID: 14307 RVA: 0x0016F034 File Offset: 0x0016D234
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

	// Token: 0x060037E4 RID: 14308 RVA: 0x0016F074 File Offset: 0x0016D274
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

	// Token: 0x060037E5 RID: 14309 RVA: 0x0016F0B0 File Offset: 0x0016D2B0
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

	// Token: 0x060037E6 RID: 14310 RVA: 0x0016F174 File Offset: 0x0016D374
	public static string PatchModPathString(string _pathString)
	{
		string text;
		ModManager.TryPatchModPathString(_pathString, out text);
		return text;
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x0016F18B File Offset: 0x0016D38B
	public static IEnumerator LoadPatchStuff(bool _isLoadingInGame)
	{
		yield return ModManager.LoadUiAtlases(_isLoadingInGame);
		yield return ModManager.LoadLocalizations(_isLoadingInGame);
		yield break;
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x0016F19A File Offset: 0x0016D39A
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

	// Token: 0x060037E9 RID: 14313 RVA: 0x0016F1A9 File Offset: 0x0016D3A9
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

	// Token: 0x060037EA RID: 14314 RVA: 0x0016F1B8 File Offset: 0x0016D3B8
	public static void ModAtlasesDefaults(GameObject _parentGo, Shader _defaultShader)
	{
		ModManager.atlasesParentGo = _parentGo;
		ModManager.defaultShader = _defaultShader;
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x0016F1C6 File Offset: 0x0016D3C6
	public static void RegisterAtlasManager(MultiSourceAtlasManager _atlasManager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded = null)
	{
		ModManager.atlasManagers.Add(_atlasManager.name, new ModManager.AtlasManagerEntry(_atlasManager, _createdByMod, _shader, _onNewAtlasLoaded));
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x0016F1E4 File Offset: 0x0016D3E4
	public static MultiSourceAtlasManager GetAtlasManager(string _name)
	{
		ModManager.AtlasManagerEntry atlasManagerEntry;
		if (!ModManager.atlasManagers.TryGetValue(_name, out atlasManagerEntry))
		{
			return null;
		}
		return atlasManagerEntry.Manager;
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x0016F208 File Offset: 0x0016D408
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

	// Token: 0x04002D96 RID: 11670
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string ModsBasePathLegacy = ((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXServer) ? (Application.dataPath + "/../../Mods") : (Application.dataPath + "/../Mods"));

	// Token: 0x04002D97 RID: 11671
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly DictionaryList<string, Mod> loadedMods = new DictionaryList<string, Mod>();

	// Token: 0x04002D98 RID: 11672
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Mod> failedMods = new List<Mod>();

	// Token: 0x04002D99 RID: 11673
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ModManager.AtlasManagerEntry> atlasManagers = new CaseInsensitiveStringDictionary<ModManager.AtlasManagerEntry>();

	// Token: 0x04002D9A RID: 11674
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject atlasesParentGo;

	// Token: 0x04002D9B RID: 11675
	[PublicizedFrom(EAccessModifier.Private)]
	public static Shader defaultShader;

	// Token: 0x0200074A RID: 1866
	[PublicizedFrom(EAccessModifier.Private)]
	public class AtlasManagerEntry
	{
		// Token: 0x060037EF RID: 14319 RVA: 0x0016F2DD File Offset: 0x0016D4DD
		public AtlasManagerEntry(MultiSourceAtlasManager _manager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded)
		{
			this.Manager = _manager;
			this.CreatedByMod = _createdByMod;
			this.Shader = _shader;
			this.OnNewAtlasLoaded = _onNewAtlasLoaded;
		}

		// Token: 0x04002D9C RID: 11676
		public readonly MultiSourceAtlasManager Manager;

		// Token: 0x04002D9D RID: 11677
		public readonly bool CreatedByMod;

		// Token: 0x04002D9E RID: 11678
		public readonly Shader Shader;

		// Token: 0x04002D9F RID: 11679
		public readonly Action<INGUIAtlas, bool> OnNewAtlasLoaded;
	}
}
