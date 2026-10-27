using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x02000751 RID: 1873
public static class ModManager
{
	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x060037F1 RID: 14321 RVA: 0x0016FCE0 File Offset: 0x0016DEE0
	public static string ModsBasePath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameIO.GetDeviceLocalUserGameDataDir() + "/Mods";
		}
	}

	// Token: 0x060037F2 RID: 14322 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void initModManager()
	{
	}

	// Token: 0x060037F3 RID: 14323 RVA: 0x0016FCF4 File Offset: 0x0016DEF4
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

	// Token: 0x060037F4 RID: 14324 RVA: 0x0016FE04 File Offset: 0x0016E004
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

	// Token: 0x060037F5 RID: 14325 RVA: 0x0016FED0 File Offset: 0x0016E0D0
	public static bool ModLoaded(string _modName)
	{
		return ModManager.loadedMods.dict.ContainsKey(_modName);
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x0016FEE2 File Offset: 0x0016E0E2
	public static Mod GetMod(string _modName, bool _onlyLoaded = false)
	{
		if (!ModManager.ModLoaded(_modName))
		{
			return null;
		}
		return ModManager.loadedMods.dict[_modName];
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x0016FEFE File Offset: 0x0016E0FE
	public static List<Mod> GetLoadedMods()
	{
		return ModManager.loadedMods.list;
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x0016FF0C File Offset: 0x0016E10C
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

	// Token: 0x060037F9 RID: 14329 RVA: 0x0016FF84 File Offset: 0x0016E184
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

	// Token: 0x060037FA RID: 14330 RVA: 0x0016FFCC File Offset: 0x0016E1CC
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

	// Token: 0x060037FB RID: 14331 RVA: 0x0017000C File Offset: 0x0016E20C
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

	// Token: 0x060037FC RID: 14332 RVA: 0x00170048 File Offset: 0x0016E248
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

	// Token: 0x060037FD RID: 14333 RVA: 0x0017010C File Offset: 0x0016E30C
	public static string PatchModPathString(string _pathString)
	{
		string text;
		ModManager.TryPatchModPathString(_pathString, out text);
		return text;
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x00170123 File Offset: 0x0016E323
	public static IEnumerator LoadPatchStuff(bool _isLoadingInGame)
	{
		yield return ModManager.LoadUiAtlases(_isLoadingInGame);
		yield return ModManager.LoadLocalizations(_isLoadingInGame);
		yield break;
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x00170132 File Offset: 0x0016E332
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

	// Token: 0x06003800 RID: 14336 RVA: 0x00170141 File Offset: 0x0016E341
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

	// Token: 0x06003801 RID: 14337 RVA: 0x00170150 File Offset: 0x0016E350
	public static void ModAtlasesDefaults(GameObject _parentGo, Shader _defaultShader)
	{
		ModManager.atlasesParentGo = _parentGo;
		ModManager.defaultShader = _defaultShader;
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x0017015E File Offset: 0x0016E35E
	public static void RegisterAtlasManager(MultiSourceAtlasManager _atlasManager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded = null)
	{
		ModManager.atlasManagers.Add(_atlasManager.name, new ModManager.AtlasManagerEntry(_atlasManager, _createdByMod, _shader, _onNewAtlasLoaded));
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x0017017C File Offset: 0x0016E37C
	public static MultiSourceAtlasManager GetAtlasManager(string _name)
	{
		ModManager.AtlasManagerEntry atlasManagerEntry;
		if (!ModManager.atlasManagers.TryGetValue(_name, out atlasManagerEntry))
		{
			return null;
		}
		return atlasManagerEntry.Manager;
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x001701A0 File Offset: 0x0016E3A0
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

	// Token: 0x04002DB2 RID: 11698
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string ModsBasePathLegacy = ((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXServer) ? (Application.dataPath + "/../../Mods") : (Application.dataPath + "/../Mods"));

	// Token: 0x04002DB3 RID: 11699
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly DictionaryList<string, Mod> loadedMods = new DictionaryList<string, Mod>();

	// Token: 0x04002DB4 RID: 11700
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Mod> failedMods = new List<Mod>();

	// Token: 0x04002DB5 RID: 11701
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ModManager.AtlasManagerEntry> atlasManagers = new CaseInsensitiveStringDictionary<ModManager.AtlasManagerEntry>();

	// Token: 0x04002DB6 RID: 11702
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject atlasesParentGo;

	// Token: 0x04002DB7 RID: 11703
	[PublicizedFrom(EAccessModifier.Private)]
	public static Shader defaultShader;

	// Token: 0x02000752 RID: 1874
	[PublicizedFrom(EAccessModifier.Private)]
	public class AtlasManagerEntry
	{
		// Token: 0x06003806 RID: 14342 RVA: 0x00170275 File Offset: 0x0016E475
		public AtlasManagerEntry(MultiSourceAtlasManager _manager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded)
		{
			this.Manager = _manager;
			this.CreatedByMod = _createdByMod;
			this.Shader = _shader;
			this.OnNewAtlasLoaded = _onNewAtlasLoaded;
		}

		// Token: 0x04002DB8 RID: 11704
		public readonly MultiSourceAtlasManager Manager;

		// Token: 0x04002DB9 RID: 11705
		public readonly bool CreatedByMod;

		// Token: 0x04002DBA RID: 11706
		public readonly Shader Shader;

		// Token: 0x04002DBB RID: 11707
		public readonly Action<INGUIAtlas, bool> OnNewAtlasLoaded;
	}
}
