using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A42 RID: 2626
public static class SaveDataUtils
{
	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x06004F61 RID: 20321 RVA: 0x001E1D4B File Offset: 0x001DFF4B
	// (set) Token: 0x06004F62 RID: 20322 RVA: 0x001E1D52 File Offset: 0x001DFF52
	public static ISaveDataManager SaveDataManager
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x06004F63 RID: 20323 RVA: 0x001E1D5A File Offset: 0x001DFF5A
	// (set) Token: 0x06004F64 RID: 20324 RVA: 0x001E1D61 File Offset: 0x001DFF61
	public static ISaveDataPrefs SaveDataPrefs
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x06004F65 RID: 20325 RVA: 0x001E1D69 File Offset: 0x001DFF69
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

	// Token: 0x06004F66 RID: 20326 RVA: 0x001E1D71 File Offset: 0x001DFF71
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

	// Token: 0x06004F67 RID: 20327 RVA: 0x001E1D8C File Offset: 0x001DFF8C
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

	// Token: 0x06004F68 RID: 20328 RVA: 0x001E1DBF File Offset: 0x001DFFBF
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void SetSaveDataPrefsOverride(ISaveDataPrefs saveDataPrefsOverride)
	{
		SaveDataUtils.SaveDataPrefs = saveDataPrefsOverride;
	}

	// Token: 0x06004F69 RID: 20329 RVA: 0x001E1DC7 File Offset: 0x001DFFC7
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ClearSaveDataPrefsOverride()
	{
		SaveDataUtils.SaveDataPrefs = SaveDataUtils.s_saveDataPrefs;
	}

	// Token: 0x06004F6A RID: 20330 RVA: 0x001E1DD4 File Offset: 0x001DFFD4
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

	// Token: 0x06004F6B RID: 20331 RVA: 0x001E1E08 File Offset: 0x001E0008
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

	// Token: 0x06004F6C RID: 20332 RVA: 0x001E1E76 File Offset: 0x001E0076
	public static SaveDataManagedPath GetBackupPath(SaveDataManagedPath restorePath)
	{
		return new SaveDataManagedPath(restorePath.PathRelativeToRoot + ".bup");
	}

	// Token: 0x06004F6D RID: 20333 RVA: 0x001E1E90 File Offset: 0x001E0090
	public static SaveDataManagedPath GetRestorePath(SaveDataManagedPath backupPath)
	{
		string pathRelativeToRoot = backupPath.PathRelativeToRoot;
		if (!pathRelativeToRoot.EndsWith(".bup"))
		{
			throw new ArgumentException(string.Format("Expected \"{0}\" to end with \"{1}\".", backupPath, ".bup"));
		}
		return new SaveDataManagedPath(pathRelativeToRoot.AsSpan(0, pathRelativeToRoot.Length - ".bup".Length));
	}

	// Token: 0x06004F6E RID: 20334 RVA: 0x001E1EEC File Offset: 0x001E00EC
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

	// Token: 0x06004F6F RID: 20335 RVA: 0x001E1FAC File Offset: 0x001E01AC
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

	// Token: 0x04003D0E RID: 15630
	public const string BACKUP_FILE_EXTENSION = "bup";

	// Token: 0x04003D0F RID: 15631
	public const string BACKUP_FILE_EXTENSION_WITH_DOT = ".bup";

	// Token: 0x04003D10 RID: 15632
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initStatic;

	// Token: 0x04003D12 RID: 15634
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataManager s_saveDataManager;

	// Token: 0x04003D13 RID: 15635
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_isManagementEnabled = false;

	// Token: 0x04003D14 RID: 15636
	public static string s_saveDataRootPathPrefix;

	// Token: 0x04003D16 RID: 15638
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataPrefs s_saveDataPrefs = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x04003D17 RID: 15639
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegex = new Regex("$^", RegexOptions.Compiled);

	// Token: 0x04003D18 RID: 15640
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegexWithoutGroups = new Regex("$^", RegexOptions.Compiled);
}
