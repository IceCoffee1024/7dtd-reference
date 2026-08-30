using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02000A31 RID: 2609
public static class SaveDataUtils
{
	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x06004EDB RID: 20187 RVA: 0x001DF2B3 File Offset: 0x001DD4B3
	// (set) Token: 0x06004EDC RID: 20188 RVA: 0x001DF2BA File Offset: 0x001DD4BA
	public static ISaveDataManager SaveDataManager
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x06004EDD RID: 20189 RVA: 0x001DF2C2 File Offset: 0x001DD4C2
	// (set) Token: 0x06004EDE RID: 20190 RVA: 0x001DF2C9 File Offset: 0x001DD4C9
	public static ISaveDataPrefs SaveDataPrefs
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x06004EDF RID: 20191 RVA: 0x001DF2D1 File Offset: 0x001DD4D1
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

	// Token: 0x06004EE0 RID: 20192 RVA: 0x001DF2D9 File Offset: 0x001DD4D9
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

	// Token: 0x06004EE1 RID: 20193 RVA: 0x001DF2F4 File Offset: 0x001DD4F4
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

	// Token: 0x06004EE2 RID: 20194 RVA: 0x001DF327 File Offset: 0x001DD527
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void SetSaveDataPrefsOverride(ISaveDataPrefs saveDataPrefsOverride)
	{
		SaveDataUtils.SaveDataPrefs = saveDataPrefsOverride;
	}

	// Token: 0x06004EE3 RID: 20195 RVA: 0x001DF32F File Offset: 0x001DD52F
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void ClearSaveDataPrefsOverride()
	{
		SaveDataUtils.SaveDataPrefs = SaveDataUtils.s_saveDataPrefs;
	}

	// Token: 0x06004EE4 RID: 20196 RVA: 0x001DF33C File Offset: 0x001DD53C
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

	// Token: 0x06004EE5 RID: 20197 RVA: 0x001DF370 File Offset: 0x001DD570
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

	// Token: 0x06004EE6 RID: 20198 RVA: 0x001DF3DE File Offset: 0x001DD5DE
	public static SaveDataManagedPath GetBackupPath(SaveDataManagedPath restorePath)
	{
		return new SaveDataManagedPath(restorePath.PathRelativeToRoot + ".bup");
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x001DF3F8 File Offset: 0x001DD5F8
	public static SaveDataManagedPath GetRestorePath(SaveDataManagedPath backupPath)
	{
		string pathRelativeToRoot = backupPath.PathRelativeToRoot;
		if (!pathRelativeToRoot.EndsWith(".bup"))
		{
			throw new ArgumentException(string.Format("Expected \"{0}\" to end with \"{1}\".", backupPath, ".bup"));
		}
		return new SaveDataManagedPath(pathRelativeToRoot.AsSpan(0, pathRelativeToRoot.Length - ".bup".Length));
	}

	// Token: 0x06004EE8 RID: 20200 RVA: 0x001DF454 File Offset: 0x001DD654
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

	// Token: 0x06004EE9 RID: 20201 RVA: 0x001DF514 File Offset: 0x001DD714
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

	// Token: 0x04003C7D RID: 15485
	public const string BACKUP_FILE_EXTENSION = "bup";

	// Token: 0x04003C7E RID: 15486
	public const string BACKUP_FILE_EXTENSION_WITH_DOT = ".bup";

	// Token: 0x04003C7F RID: 15487
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_initStatic;

	// Token: 0x04003C81 RID: 15489
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataManager s_saveDataManager;

	// Token: 0x04003C82 RID: 15490
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool s_isManagementEnabled = false;

	// Token: 0x04003C83 RID: 15491
	public static string s_saveDataRootPathPrefix;

	// Token: 0x04003C85 RID: 15493
	[PublicizedFrom(EAccessModifier.Private)]
	public static ISaveDataPrefs s_saveDataPrefs = SaveDataPrefsUninitialized.INSTANCE;

	// Token: 0x04003C86 RID: 15494
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegex = new Regex("$^", RegexOptions.Compiled);

	// Token: 0x04003C87 RID: 15495
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex s_managedPathRegexWithoutGroups = new Regex("$^", RegexOptions.Compiled);
}
