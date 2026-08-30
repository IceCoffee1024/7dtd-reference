using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x02000747 RID: 1863
public static class ModManager
{
	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06003797 RID: 14231 RVA: 0x0016E3E4 File Offset: 0x0016C5E4
	public static string ModsBasePath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameIO.GetDeviceLocalUserGameDataDir() + "/Mods";
		}
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void initModManager()
	{
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x0016E3F8 File Offset: 0x0016C5F8
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

	// Token: 0x0600379A RID: 14234 RVA: 0x0016E508 File Offset: 0x0016C708
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

	// Token: 0x0600379B RID: 14235 RVA: 0x0016E5D4 File Offset: 0x0016C7D4
	public static bool ModLoaded(string _modName)
	{
		return ModManager.loadedMods.dict.ContainsKey(_modName);
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0016E5E6 File Offset: 0x0016C7E6
	public static Mod GetMod(string _modName, bool _onlyLoaded = false)
	{
		if (!ModManager.ModLoaded(_modName))
		{
			return null;
		}
		return ModManager.loadedMods.dict[_modName];
	}

	// Token: 0x0600379D RID: 14237 RVA: 0x0016E602 File Offset: 0x0016C802
	public static List<Mod> GetLoadedMods()
	{
		return ModManager.loadedMods.list;
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x0016E610 File Offset: 0x0016C810
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

	// Token: 0x0600379F RID: 14239 RVA: 0x0016E688 File Offset: 0x0016C888
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

	// Token: 0x060037A0 RID: 14240 RVA: 0x0016E6D0 File Offset: 0x0016C8D0
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

	// Token: 0x060037A1 RID: 14241 RVA: 0x0016E710 File Offset: 0x0016C910
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

	// Token: 0x060037A2 RID: 14242 RVA: 0x0016E74C File Offset: 0x0016C94C
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

	// Token: 0x060037A3 RID: 14243 RVA: 0x0016E810 File Offset: 0x0016CA10
	public static string PatchModPathString(string _pathString)
	{
		string text;
		ModManager.TryPatchModPathString(_pathString, out text);
		return text;
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x0016E827 File Offset: 0x0016CA27
	public static IEnumerator LoadPatchStuff(bool _isLoadingInGame)
	{
		yield return ModManager.LoadUiAtlases(_isLoadingInGame);
		yield return ModManager.LoadLocalizations(_isLoadingInGame);
		yield break;
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x0016E836 File Offset: 0x0016CA36
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

	// Token: 0x060037A6 RID: 14246 RVA: 0x0016E845 File Offset: 0x0016CA45
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

	// Token: 0x060037A7 RID: 14247 RVA: 0x0016E854 File Offset: 0x0016CA54
	public static void ModAtlasesDefaults(GameObject _parentGo, Shader _defaultShader)
	{
		ModManager.atlasesParentGo = _parentGo;
		ModManager.defaultShader = _defaultShader;
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x0016E862 File Offset: 0x0016CA62
	public static void RegisterAtlasManager(MultiSourceAtlasManager _atlasManager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded = null)
	{
		ModManager.atlasManagers.Add(_atlasManager.name, new ModManager.AtlasManagerEntry(_atlasManager, _createdByMod, _shader, _onNewAtlasLoaded));
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x0016E880 File Offset: 0x0016CA80
	public static MultiSourceAtlasManager GetAtlasManager(string _name)
	{
		ModManager.AtlasManagerEntry atlasManagerEntry;
		if (!ModManager.atlasManagers.TryGetValue(_name, out atlasManagerEntry))
		{
			return null;
		}
		return atlasManagerEntry.Manager;
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x0016E8A4 File Offset: 0x0016CAA4
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

	// Token: 0x04002D9D RID: 11677
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string ModsBasePathLegacy = ((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXServer) ? (Application.dataPath + "/../../Mods") : (Application.dataPath + "/../Mods"));

	// Token: 0x04002D9E RID: 11678
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly DictionaryList<string, Mod> loadedMods = new DictionaryList<string, Mod>();

	// Token: 0x04002D9F RID: 11679
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Mod> failedMods = new List<Mod>();

	// Token: 0x04002DA0 RID: 11680
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, ModManager.AtlasManagerEntry> atlasManagers = new CaseInsensitiveStringDictionary<ModManager.AtlasManagerEntry>();

	// Token: 0x04002DA1 RID: 11681
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject atlasesParentGo;

	// Token: 0x04002DA2 RID: 11682
	[PublicizedFrom(EAccessModifier.Private)]
	public static Shader defaultShader;

	// Token: 0x02000748 RID: 1864
	[PublicizedFrom(EAccessModifier.Private)]
	public class AtlasManagerEntry
	{
		// Token: 0x060037AC RID: 14252 RVA: 0x0016E979 File Offset: 0x0016CB79
		public AtlasManagerEntry(MultiSourceAtlasManager _manager, bool _createdByMod, Shader _shader, Action<INGUIAtlas, bool> _onNewAtlasLoaded)
		{
			this.Manager = _manager;
			this.CreatedByMod = _createdByMod;
			this.Shader = _shader;
			this.OnNewAtlasLoaded = _onNewAtlasLoaded;
		}

		// Token: 0x04002DA3 RID: 11683
		public readonly MultiSourceAtlasManager Manager;

		// Token: 0x04002DA4 RID: 11684
		public readonly bool CreatedByMod;

		// Token: 0x04002DA5 RID: 11685
		public readonly Shader Shader;

		// Token: 0x04002DA6 RID: 11686
		public readonly Action<INGUIAtlas, bool> OnNewAtlasLoaded;
	}
}
