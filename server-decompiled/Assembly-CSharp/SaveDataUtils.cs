using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A56 RID: 2646
public static class SaveDataUtils
{
	// Token: 0x170008A0 RID: 2208
	// (get) Token: 0x06004F54 RID: 20308 RVA: 0x001E40E3 File Offset: 0x001E22E3
	// (set) Token: 0x06004F55 RID: 20309 RVA: 0x001E40EA File Offset: 0x001E22EA
	public static ISaveDataManager SaveDataManager
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008A1 RID: 2209
	// (get) Token: 0x06004F56 RID: 20310 RVA: 0x001E40F2 File Offset: 0x001E22F2
	// (set) Token: 0x06004F57 RID: 20311 RVA: 0x001E40F9 File Offset: 0x001E22F9
	public static ISaveDataPrefs SaveDataPrefs
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x06004F58 RID: 20312 RVA: 0x001E4101 File Offset: 0x001E2301
	public static IEnumerator InitStaticCoroutine()
	{
		if (SaveDataUtils.s_initStatic)
		{
			yield break;
		}
		SaveDataUtils.s_initStatic = true;
		Log.Out("[SaveDataUtils] InitStatic Begin");
		SaveDataUtils.UpdatePaths();
		IPlatformSaveGameProvider platformSaveGameProvider = PlatformManager.MultiPlatform.SaveGameProvider;
		SaveDataUtils.s_isManagementEnabled = platformSaveGameProvider != null;
		if (SaveDataUtils.s_isManagementEnabled)
		{
			while (!platformSaveGameProvider.Status.IsTerminal())
			{
				yield return null;
			}
		}
		if (SaveDataUtils.s_isManagementEnabled)
		{
			SaveDataUtils.s_saveDataManager = new SaveDataManager(platformSaveGameProvider);
		}
		else
		{
			SaveDataUtils.s_saveDataManager = SaveDataManager_Placeholder.Instance;
		}
		SaveDataUtils.SaveDataManager = SaveDataUtils.s_saveDataManager;
		SaveDataUtils.SaveDataManager.Init();
		SdDirectory.CreateDirectory(GameIO.GetUserGameDataDir());
		Log.Out("[SaveDataUtils] UserGameDataDir: " + GameIO.GetUserGameDataDir());
		if (LaunchPrefs.PlayerPrefsFile.Value)
		{
			Log.Out("[SaveDataUtils] SdPlayerPrefs -> SdFile");
			SaveDataUtils.SaveDataPrefs = (SaveDataUtils.s_saveDataPrefs = new SaveDataPrefsFile(GameIO.GetNormalizedPath(Path.Join(GameIO.GetUserGameDataDir(), "prefs.cfg"))));
		}
		else
		{
			Log.Out("[SaveDataUtils] SdPlayerPrefs -> Unity");
			SaveDataUtils.SaveDataPrefs = (SaveDataUtils.s_saveDataPrefs = SaveDataPrefsUnity.INSTANCE);
		}
		Log.Out("[SaveDataUtils] InitStatic Complete");
		yield break;
	}

	// Token: 0x06004F59 RID: 20313 RVA: 0x001E4109 File Offset: 0x001E2309
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void SetSaveDataManagerOverride(ISaveDataManager saveDataManagerOverride)
	{
		if (saveDataManagerOverride == SaveDataUtils.s_saveDataManager)
		{
			Log.Error("SetSaveDataManagerOverride failed: Cannot override default Save Data Manager with itself.");
			return;
		}
		SaveDataUtils.SaveDataManager = saveDataManagerOverride;
	}

	// Token: 0x06004F5A RID: 20314 RVA: 0x001E4124 File Offset: 0x001E2324
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ClearSaveDataManagerOverride()
	{
		if (SaveDataUtils.SaveDataManager == SaveDataUtils.s_saveDataManager)
		{
			Log.Error("ClearSaveDataManagerOverride failed: Save Data Manager override was not set or has already been cleared.");
			return;
		}
		SaveDataUtils.SaveDataManager.Cleanup();
		SaveDataUtils.SaveDataManager = null;
		SaveDataUtils.SaveDataManager = SaveDataUtils.s_saveDataManager;
	}

	// Token: 0x06004F5B RID: 20315 RVA: 0x001E4157 File Offset: 0x001E2357
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void SetSaveDataPrefsOverride(ISaveDataPrefs saveDataPrefsOverride)
	{
		SaveDataUtils.SaveDataPrefs = saveDataPrefsOverride;
	}

	// Token: 0x06004F5C RID: 20316 RVA: 0x001E415F File Offset: 0x001E235F
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ClearSaveDataPrefsOverride()
	{
		SaveDataUtils.SaveDataPrefs = SaveDataUtils.s_saveDataPrefs;
	}

	// Token: 0x06004F5D RID: 20317 RVA: 0x001E416C File Offset: 0x001E236C
	public static bool IsManaged(string path)
	{
		if (!SaveDataUtils.s_isManagementEnabled)
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		string normalizedPath = GameIO.GetNormalizedPath(path);
		return SaveDataUtils.s_managedPathRegexWithoutGroups.IsMatch(normalizedPath);
	}

	// Token: 0x06004F5E RID: 20318 RVA: 0x001E41A0 File Offset: 0x001E23A0
	public static bool TryGetManagedPath(string path, out SaveDataManagedPath managedPath)
	{
		if (!SaveDataUtils.s_isManagementEnabled)
		{
			managedPath = null;
			return false;
		}
		if (string.IsNullOrWhiteSpace(path))
		{
			managedPath = null;
			return false;
		}
		string normalizedPath = GameIO.GetNormalizedPath(path);
		Match match = SaveDataUtils.s_managedPathRegex.Match(normalizedPath);
		if (match.Success)
		{
			GroupCollection groups = match.Groups;
			string value = groups[1].Value;
			string value2 = groups[2].Value;
			managedPath = new SaveDataManagedPath(value2);
			return true;
		}
		managedPath = null;
		return false;
	}

	// Token: 0x06004F5F RID: 20319 RVA: 0x001E420E File Offset: 0x001E240E
	public static SaveDataManagedPath GetBackupPath(SaveDataManagedPath restorePath)
	{
		return new SaveDataManagedPath(restorePath.PathRelativeToRoot + ".bup");
	}

	// Token: 0x06004F60 RID: 20320 RVA: 0x001E4228 File Offset: 0x001E2428
	public static SaveDataManagedPath GetRestorePath(SaveDataManagedPath backupPath)
	{
		string pathRelativeToRoot = backupPath.PathRelativeToRoot;
		if (!pathRelativeToRoot.EndsWith(".bup"))
		{
			throw new ArgumentException(string.Format("Expected \"{0}\" to end with \"{1}\".", backupPath, ".bup"));
		}
		return new SaveDataManagedPath(pathRelativeToRoot.AsSpan(0, pathRelativeToRoot.Length - ".bup".Length));
	}

	// Token: 0x06004F61 RID: 20321 RVA: 0x001E4284 File Offset: 0x001E2484
	[PublicizedFrom(EAccessModifier.Private)]
	public static void UpdatePaths()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder.Append("^(?:");
		stringBuilder2.Append("^(?:");
		string normalizedPath = GameIO.GetNormalizedPath(GameIO.GetUserGameDataDir());
		stringBuilder.Append("(?<1>");
		string text = Regex.Escape(normalizedPath);
		stringBuilder.Append(text);
		stringBuilder2.Append(text);
		stringBuilder.Append(')');
		stringBuilder.Append(')');
		stringBuilder2.Append(')');
		stringBuilder.Append("(?:$|[\\\\/](?<2>.*)$)");
		stringBuilder2.Append("(?:$|[\\\\/])");
		SaveDataUtils.s_managedPathRegex = new Regex(stringBuilder.ToString(), RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
		SaveDataUtils.s_managedPathRegexWithoutGroups = new Regex(stringBuilder.ToString(), RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
		SaveDataUtils.s_saveDataRootPathPrefix = normalizedPath;
	}

	// Token: 0x06004F62 RID: 20322 RVA: 0x001E4344 File Offset: 0x001E2544
	public static void Destroy()
	{
		SdPlayerPrefs.Save();
		SaveDataUtils.SaveDataManager.Cleanup();
		SaveDataUtils.SaveDataPrefs = (SaveDataUtils.s_saveDataPrefs = SaveDataPrefsUninitialized.INSTANCE);
		RoamingPrefs.Destroy();
		SaveDataUtils.SaveDataManager = (SaveDataUtils.s_saveDataManager = null);
		IPlatformSaveGameProvider saveGameProvider = PlatformManager.NativePlatform.SaveGameProvider;
		if (saveGameProvider == null)
		{
			return;
		}
		saveGameProvider.Destroy();
	}

	// Token: 0x04003D3C RID: 15676
	public const string BACKUP_FILE_EXTENSION = "bup";

	// Token: 0x04003D3D RID: 15677
	public const string BACKUP_FILE_EXTENSION_WITH_DOT = ".bup";

	// Token: 0x04003D3E RID: 15678
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initStatic;

	// Token: 0x04003D40 RID: 15680
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataManager s_saveDataManager;

	// Token: 0x04003D41 RID: 15681
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_isManagementEnabled = false;

	// Token: 0x04003D42 RID: 15682
	public static string s_saveDataRootPathPrefix;

	// Token: 0x04003D44 RID: 15684
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataPrefs s_saveDataPrefs = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x04003D45 RID: 15685
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegex = new Regex("$^", RegexOptions.Compiled);

	// Token: 0x04003D46 RID: 15686
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegexWithoutGroups = new Regex("$^", RegexOptions.Compiled);
}
