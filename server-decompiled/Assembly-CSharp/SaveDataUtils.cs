using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A4C RID: 2636
public static class SaveDataUtils
{
	// Token: 0x1700089A RID: 2202
	// (get) Token: 0x06004F30 RID: 20272 RVA: 0x001E2FFB File Offset: 0x001E11FB
	// (set) Token: 0x06004F31 RID: 20273 RVA: 0x001E3002 File Offset: 0x001E1202
	public static ISaveDataManager SaveDataManager
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700089B RID: 2203
	// (get) Token: 0x06004F32 RID: 20274 RVA: 0x001E300A File Offset: 0x001E120A
	// (set) Token: 0x06004F33 RID: 20275 RVA: 0x001E3011 File Offset: 0x001E1211
	public static ISaveDataPrefs SaveDataPrefs
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x06004F34 RID: 20276 RVA: 0x001E3019 File Offset: 0x001E1219
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

	// Token: 0x06004F35 RID: 20277 RVA: 0x001E3021 File Offset: 0x001E1221
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

	// Token: 0x06004F36 RID: 20278 RVA: 0x001E303C File Offset: 0x001E123C
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

	// Token: 0x06004F37 RID: 20279 RVA: 0x001E306F File Offset: 0x001E126F
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void SetSaveDataPrefsOverride(ISaveDataPrefs saveDataPrefsOverride)
	{
		SaveDataUtils.SaveDataPrefs = saveDataPrefsOverride;
	}

	// Token: 0x06004F38 RID: 20280 RVA: 0x001E3077 File Offset: 0x001E1277
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ClearSaveDataPrefsOverride()
	{
		SaveDataUtils.SaveDataPrefs = SaveDataUtils.s_saveDataPrefs;
	}

	// Token: 0x06004F39 RID: 20281 RVA: 0x001E3084 File Offset: 0x001E1284
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

	// Token: 0x06004F3A RID: 20282 RVA: 0x001E30B8 File Offset: 0x001E12B8
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

	// Token: 0x06004F3B RID: 20283 RVA: 0x001E3126 File Offset: 0x001E1326
	public static SaveDataManagedPath GetBackupPath(SaveDataManagedPath restorePath)
	{
		return new SaveDataManagedPath(restorePath.PathRelativeToRoot + ".bup");
	}

	// Token: 0x06004F3C RID: 20284 RVA: 0x001E3140 File Offset: 0x001E1340
	public static SaveDataManagedPath GetRestorePath(SaveDataManagedPath backupPath)
	{
		string pathRelativeToRoot = backupPath.PathRelativeToRoot;
		if (!pathRelativeToRoot.EndsWith(".bup"))
		{
			throw new ArgumentException(string.Format("Expected \"{0}\" to end with \"{1}\".", backupPath, ".bup"));
		}
		return new SaveDataManagedPath(pathRelativeToRoot.AsSpan(0, pathRelativeToRoot.Length - ".bup".Length));
	}

	// Token: 0x06004F3D RID: 20285 RVA: 0x001E319C File Offset: 0x001E139C
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

	// Token: 0x06004F3E RID: 20286 RVA: 0x001E325C File Offset: 0x001E145C
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

	// Token: 0x04003D1B RID: 15643
	public const string BACKUP_FILE_EXTENSION = "bup";

	// Token: 0x04003D1C RID: 15644
	public const string BACKUP_FILE_EXTENSION_WITH_DOT = ".bup";

	// Token: 0x04003D1D RID: 15645
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initStatic;

	// Token: 0x04003D1F RID: 15647
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataManager s_saveDataManager;

	// Token: 0x04003D20 RID: 15648
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_isManagementEnabled = false;

	// Token: 0x04003D21 RID: 15649
	public static string s_saveDataRootPathPrefix;

	// Token: 0x04003D23 RID: 15651
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataPrefs s_saveDataPrefs = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x04003D24 RID: 15652
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegex = new Regex("$^", RegexOptions.Compiled);

	// Token: 0x04003D25 RID: 15653
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegexWithoutGroups = new Regex("$^", RegexOptions.Compiled);
}
