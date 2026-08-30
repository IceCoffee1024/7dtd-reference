using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine;

// Token: 0x020013E7 RID: 5095
public static class GameIO
{
	// Token: 0x06009FDA RID: 40922 RVA: 0x003C0E3B File Offset: 0x003BF03B
	public static SdFileInfo[] GetDirectory(string _path, string _pattern)
	{
		if (!SdDirectory.Exists(_path))
		{
			return Array.Empty<SdFileInfo>();
		}
		return new SdDirectoryInfo(_path).GetFiles(_pattern);
	}

	// Token: 0x06009FDB RID: 40923 RVA: 0x003C0E58 File Offset: 0x003BF058
	public static long FileSize(string _filePath)
	{
		SdFileInfo sdFileInfo = new SdFileInfo(_filePath);
		if (!sdFileInfo.Exists)
		{
			return -1L;
		}
		return sdFileInfo.Length;
	}

	// Token: 0x06009FDC RID: 40924 RVA: 0x003C0E7D File Offset: 0x003BF07D
	public static string GetNormalizedPath(string _path)
	{
		return Path.GetFullPath(_path).TrimEnd(GameIO.pathTrimCharacters);
	}

	// Token: 0x06009FDD RID: 40925 RVA: 0x003C0E8F File Offset: 0x003BF08F
	public static bool PathsEquals(string _path1, string _path2, bool _ignoreCase)
	{
		return string.Equals(GameIO.GetNormalizedPath(_path1), GameIO.GetNormalizedPath(_path2), _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
	}

	// Token: 0x06009FDE RID: 40926 RVA: 0x003C0EAC File Offset: 0x003BF0AC
	public static string GetFileExtension(string _filename)
	{
		int num;
		if (_filename.Length > 4 && (num = _filename.LastIndexOf('.')) > 0)
		{
			return _filename.Substring(num);
		}
		return _filename;
	}

	// Token: 0x06009FDF RID: 40927 RVA: 0x003C0ED8 File Offset: 0x003BF0D8
	public static string RemoveFileExtension(string _filename)
	{
		int num;
		if (_filename.Length > 4 && (num = _filename.LastIndexOf('.')) > 0)
		{
			return _filename.Substring(0, num);
		}
		return _filename;
	}

	// Token: 0x06009FE0 RID: 40928 RVA: 0x003C0F05 File Offset: 0x003BF105
	public static string RemoveExtension(string _filename, string _extension)
	{
		if (_filename.Length > _extension.Length && _filename.EndsWith(_extension, StringComparison.InvariantCultureIgnoreCase))
		{
			return _filename.Substring(0, _filename.Length - _extension.Length);
		}
		return _filename;
	}

	// Token: 0x06009FE1 RID: 40929 RVA: 0x003C0F38 File Offset: 0x003BF138
	public static string GetFilenameFromPath(string _filepath)
	{
		int num = _filepath.LastIndexOfAny(GameIO.ResourcePathSeparators);
		if (num >= 0 && num < _filepath.Length)
		{
			_filepath = _filepath.Substring(num + 1);
		}
		return _filepath;
	}

	// Token: 0x06009FE2 RID: 40930 RVA: 0x003C0F6C File Offset: 0x003BF16C
	public static string GetFilenameFromPathWithoutExtension(string _filepath)
	{
		int num = _filepath.LastIndexOfAny(GameIO.ResourcePathSeparators);
		int num2 = _filepath.LastIndexOf('.');
		if (num >= 0 && num2 < num)
		{
			num2 = -1;
		}
		if (num >= 0 && num2 >= 0)
		{
			return _filepath.Substring(num + 1, num2 - num - 1);
		}
		if (num >= 0)
		{
			return _filepath.Substring(num + 1);
		}
		if (num2 >= 0)
		{
			return _filepath.Substring(0, num2);
		}
		return _filepath;
	}

	// Token: 0x06009FE3 RID: 40931 RVA: 0x003C0FCC File Offset: 0x003BF1CC
	public static string GetDirectoryFromPath(string _filepath)
	{
		int num = _filepath.LastIndexOf('/');
		if (num > 0 && num < _filepath.Length)
		{
			_filepath = _filepath.Substring(0, num);
		}
		return _filepath;
	}

	// Token: 0x06009FE4 RID: 40932 RVA: 0x003C0FFA File Offset: 0x003BF1FA
	public static long GetDirectorySize(string _filepath, bool recursive = true)
	{
		return GameIO.GetDirectorySize(new SdDirectoryInfo(_filepath), recursive);
	}

	// Token: 0x06009FE5 RID: 40933 RVA: 0x003C1008 File Offset: 0x003BF208
	public static long GetDirectorySize(SdDirectoryInfo directoryInfo, bool recursive = true)
	{
		long num = 0L;
		if (directoryInfo == null || !directoryInfo.Exists)
		{
			return num;
		}
		foreach (SdFileInfo sdFileInfo in directoryInfo.GetFiles())
		{
			num += sdFileInfo.Length;
		}
		if (recursive)
		{
			foreach (SdDirectoryInfo sdDirectoryInfo in directoryInfo.GetDirectories())
			{
				num += GameIO.GetDirectorySize(sdDirectoryInfo, recursive);
			}
		}
		return num;
	}

	// Token: 0x06009FE6 RID: 40934 RVA: 0x003C1073 File Offset: 0x003BF273
	public static string GetGameDir(string _relDir)
	{
		return GameIO.GetApplicationPath() + "/" + _relDir;
	}

	// Token: 0x06009FE7 RID: 40935 RVA: 0x003C1088 File Offset: 0x003BF288
	public static string GetApplicationPath()
	{
		if (GameIO.m_ApplicationPath == null)
		{
			string text = GameIO.m_UnityDataPath;
			RuntimePlatform unityRuntimePlatform = GameIO.m_UnityRuntimePlatform;
			if (unityRuntimePlatform <= RuntimePlatform.PS4)
			{
				if (unityRuntimePlatform - RuntimePlatform.OSXPlayer > 1)
				{
					if (unityRuntimePlatform != RuntimePlatform.PS4)
					{
						goto IL_003F;
					}
					goto IL_004B;
				}
			}
			else
			{
				if (unityRuntimePlatform == RuntimePlatform.PS5)
				{
					goto IL_004B;
				}
				if (unityRuntimePlatform - RuntimePlatform.WindowsServer > 1)
				{
					goto IL_003F;
				}
			}
			text += "/..";
			goto IL_004B;
			IL_003F:
			text += "/..";
			IL_004B:
			GameIO.m_ApplicationPath = text;
		}
		return GameIO.m_ApplicationPath;
	}

	// Token: 0x06009FE8 RID: 40936 RVA: 0x003C10EB File Offset: 0x003BF2EB
	public static string GetGamePath()
	{
		if (GameIO.m_UnityRuntimePlatform != RuntimePlatform.OSXPlayer && GameIO.m_UnityRuntimePlatform != RuntimePlatform.OSXServer)
		{
			return GameIO.m_UnityDataPath + "/..";
		}
		return GameIO.m_UnityDataPath + "/../..";
	}

	// Token: 0x06009FE9 RID: 40937 RVA: 0x003C111D File Offset: 0x003BF31D
	public static string GetGameExecutablePath()
	{
		return GameIO.GetGamePath() + "/" + GameIO.GetGameExecutableName();
	}

	// Token: 0x06009FEA RID: 40938 RVA: 0x003C1134 File Offset: 0x003BF334
	public static string GetGameExecutableName()
	{
		RuntimePlatform unityRuntimePlatform = GameIO.m_UnityRuntimePlatform;
		if (unityRuntimePlatform <= RuntimePlatform.LinuxPlayer)
		{
			switch (unityRuntimePlatform)
			{
			case RuntimePlatform.OSXEditor:
				return "7DaysToDie.app";
			case RuntimePlatform.OSXPlayer:
				return "7DaysToDie.app";
			case RuntimePlatform.WindowsPlayer:
				return "7DaysToDie.exe";
			default:
				if (unityRuntimePlatform == RuntimePlatform.WindowsEditor)
				{
					return "7DaysToDie.exe";
				}
				if (unityRuntimePlatform == RuntimePlatform.LinuxPlayer)
				{
					return "7DaysToDie.x86_64";
				}
				break;
			}
		}
		else
		{
			if (unityRuntimePlatform == RuntimePlatform.LinuxEditor)
			{
				return "7DaysToDie.x86_64";
			}
			if (unityRuntimePlatform == RuntimePlatform.XboxOne)
			{
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			}
			switch (unityRuntimePlatform)
			{
			case RuntimePlatform.GameCoreXboxSeries:
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			case RuntimePlatform.GameCoreXboxOne:
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			case RuntimePlatform.PS5:
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			}
		}
		throw new ArgumentOutOfRangeException("m_UnityRuntimePlatform");
	}

	// Token: 0x06009FEB RID: 40939 RVA: 0x003C126E File Offset: 0x003BF46E
	public static string GetLauncherExecutablePath()
	{
		return GameIO.GetGamePath() + "/" + GameIO.GetLauncherExecutableName();
	}

	// Token: 0x06009FEC RID: 40940 RVA: 0x003C1284 File Offset: 0x003BF484
	public static string GetLauncherExecutableName()
	{
		RuntimePlatform unityRuntimePlatform = GameIO.m_UnityRuntimePlatform;
		if (unityRuntimePlatform <= RuntimePlatform.LinuxPlayer)
		{
			switch (unityRuntimePlatform)
			{
			case RuntimePlatform.OSXEditor:
				return "7dLauncher.app";
			case RuntimePlatform.OSXPlayer:
				return "7dLauncher.app";
			case RuntimePlatform.WindowsPlayer:
				return "7dLauncher.exe";
			default:
				if (unityRuntimePlatform == RuntimePlatform.WindowsEditor)
				{
					return "7dLauncher.exe";
				}
				if (unityRuntimePlatform == RuntimePlatform.LinuxPlayer)
				{
					return "7DaysToDie.sh";
				}
				break;
			}
		}
		else
		{
			if (unityRuntimePlatform == RuntimePlatform.LinuxEditor)
			{
				return "7DaysToDie.sh";
			}
			if (unityRuntimePlatform == RuntimePlatform.XboxOne)
			{
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			}
			switch (unityRuntimePlatform)
			{
			case RuntimePlatform.GameCoreXboxSeries:
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			case RuntimePlatform.GameCoreXboxOne:
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			case RuntimePlatform.PS5:
				throw new ArgumentException("Platform " + GameIO.m_UnityRuntimePlatform.ToStringCached<RuntimePlatform>() + " currently not supported", "m_UnityRuntimePlatform");
			}
		}
		throw new ArgumentOutOfRangeException("m_UnityRuntimePlatform");
	}

	// Token: 0x06009FED RID: 40941 RVA: 0x003C13C0 File Offset: 0x003BF5C0
	public static string GetApplicationScratchPath()
	{
		if (GameIO.m_ApplicationScratchPath == null)
		{
			RuntimePlatform unityRuntimePlatform = GameIO.m_UnityRuntimePlatform;
			if (unityRuntimePlatform <= RuntimePlatform.XboxOne)
			{
				if (unityRuntimePlatform != RuntimePlatform.PS4)
				{
					if (unityRuntimePlatform != RuntimePlatform.XboxOne)
					{
						goto IL_0050;
					}
					GameIO.m_ApplicationScratchPath = "D:";
					goto IL_005A;
				}
			}
			else
			{
				if (unityRuntimePlatform - RuntimePlatform.GameCoreXboxSeries <= 1)
				{
					GameIO.m_ApplicationScratchPath = "D:";
					goto IL_005A;
				}
				if (unityRuntimePlatform != RuntimePlatform.PS5)
				{
					goto IL_0050;
				}
			}
			GameIO.m_ApplicationScratchPath = "/hostapp";
			goto IL_005A;
			IL_0050:
			GameIO.m_ApplicationScratchPath = GameIO.GetApplicationPath();
		}
		IL_005A:
		return GameIO.m_ApplicationScratchPath;
	}

	// Token: 0x06009FEE RID: 40942 RVA: 0x003C142C File Offset: 0x003BF62C
	public static string GetApplicationTempPath()
	{
		return PlatformApplicationManager.Application.temporaryCachePath;
	}

	// Token: 0x06009FEF RID: 40943 RVA: 0x003C1438 File Offset: 0x003BF638
	public static IEnumerator PrecacheFile(string _path, int _doYieldEveryMs = -1, Action<float, long, long> _statusUpdateHandler = null)
	{
		if (!SdFile.Exists(_path))
		{
			Log.Error("File does not exist: " + _path);
			yield break;
		}
		Stream fs;
		try
		{
			fs = SdFile.OpenRead(_path);
		}
		catch (Exception ex)
		{
			Log.Error("Precaching file failed");
			Log.Exception(ex);
			yield break;
		}
		byte[] buf = new byte[16384];
		MicroStopwatch msw = new MicroStopwatch();
		int num;
		do
		{
			if (_doYieldEveryMs > 0 && msw.ElapsedMilliseconds >= (long)_doYieldEveryMs)
			{
				if (_statusUpdateHandler != null)
				{
					_statusUpdateHandler((float)fs.Position / (float)fs.Length, fs.Position, fs.Length);
				}
				yield return null;
				msw.ResetAndRestart();
			}
			try
			{
				num = fs.Read(buf, 0, buf.Length);
			}
			catch (Exception ex2)
			{
				Log.Error("Precaching file failed");
				Log.Exception(ex2);
				try
				{
					fs.Dispose();
				}
				catch (Exception ex3)
				{
					Log.Error("Failed disposing filestream");
					Log.Exception(ex3);
				}
				yield break;
			}
		}
		while (num > 0);
		fs.Dispose();
		yield break;
	}

	// Token: 0x06009FF0 RID: 40944 RVA: 0x003C1458 File Offset: 0x003BF658
	public static string GetDocumentPath()
	{
		RuntimePlatform unityRuntimePlatform = GameIO.m_UnityRuntimePlatform;
		if (unityRuntimePlatform <= RuntimePlatform.LinuxPlayer)
		{
			if (unityRuntimePlatform <= RuntimePlatform.WindowsPlayer)
			{
				if (unityRuntimePlatform <= RuntimePlatform.OSXPlayer)
				{
					goto IL_0083;
				}
				if (unityRuntimePlatform != RuntimePlatform.WindowsPlayer)
				{
					goto IL_00B3;
				}
				goto IL_007B;
			}
			else
			{
				if (unityRuntimePlatform == RuntimePlatform.WindowsEditor)
				{
					goto IL_007B;
				}
				if (unityRuntimePlatform != RuntimePlatform.LinuxPlayer)
				{
					goto IL_00B3;
				}
			}
		}
		else if (unityRuntimePlatform <= RuntimePlatform.PS4)
		{
			if (unityRuntimePlatform != RuntimePlatform.LinuxEditor)
			{
				if (unityRuntimePlatform != RuntimePlatform.PS4)
				{
					goto IL_00B3;
				}
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			}
		}
		else
		{
			if (unityRuntimePlatform == RuntimePlatform.XboxOne)
			{
				global::UnityEngine.Debug.LogWarning("XboxOne: Platform Document Path is currently not set");
				return null;
			}
			switch (unityRuntimePlatform)
			{
			case RuntimePlatform.GameCoreXboxSeries:
			case RuntimePlatform.GameCoreXboxOne:
				return Application.persistentDataPath;
			case RuntimePlatform.PS5:
				return "/download0";
			case RuntimePlatform.EmbeddedLinuxArm64:
			case RuntimePlatform.EmbeddedLinuxArm32:
			case RuntimePlatform.EmbeddedLinuxX64:
			case RuntimePlatform.EmbeddedLinuxX86:
				goto IL_00B3;
			case RuntimePlatform.LinuxServer:
				break;
			case RuntimePlatform.WindowsServer:
				goto IL_007B;
			case RuntimePlatform.OSXServer:
				goto IL_0083;
			default:
				goto IL_00B3;
			}
		}
		return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		IL_007B:
		return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		IL_0083:
		return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Library/Application Support";
		IL_00B3:
		return null;
	}

	// Token: 0x06009FF1 RID: 40945 RVA: 0x003C1519 File Offset: 0x003BF719
	public static string GetDefaultPersistentDataPath()
	{
		return GameIO.GetDocumentPath() + "/" + "7 Days To Die".Replace(" ", "");
	}

	// Token: 0x06009FF2 RID: 40946 RVA: 0x003C1540 File Offset: 0x003BF740
	public static void InitializeUserDataPaths(string _rootPathOverride = null)
	{
		string text = GameIO.GetDefaultPersistentDataPath();
		if (!string.IsNullOrEmpty(_rootPathOverride))
		{
			text = GameIO.MakeAbsolutePath(_rootPathOverride);
			Log.Out("Overriding default user data path to " + text);
		}
		if (GameIO.m_lastDeviceLocalRoot != null && GameIO.m_lastDeviceLocalRoot.Equals(text))
		{
			return;
		}
		GameIO.m_lastDeviceLocalRoot = text;
		text = Path.GetFullPath(text);
		string text2 = text + "Roaming";
		string text3 = Regex.Replace(text2, "[\\\\/]", "[\\\\/]", RegexOptions.Singleline | RegexOptions.CultureInvariant);
		GameIO.m_isRoamingPathRegex = new Regex("^(?:" + text3 + ")", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
		GameIO.m_cachedUserDataPaths = new Dictionary<ValueTuple<UserDataStorageType, string>, string>();
		GameIO.<InitializeUserDataPaths>g__PopulateCache|36_0(UserDataStorageType.DeviceLocal, text);
		GameIO.<InitializeUserDataPaths>g__PopulateCache|36_0(UserDataStorageType.Roaming, text2);
	}

	// Token: 0x06009FF3 RID: 40947 RVA: 0x003C15ED File Offset: 0x003BF7ED
	public static bool IsRoamingUserDataPath(string _path)
	{
		if (GameIO.m_isRoamingPathRegex == null)
		{
			throw new Exception("IsRoamingUserDataPath used before InitializeUserDataPaths");
		}
		return GameIO.m_isRoamingPathRegex.IsMatch(_path);
	}

	// Token: 0x06009FF4 RID: 40948 RVA: 0x003C160C File Offset: 0x003BF80C
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetCachedUserDataPath(UserDataStorageType _storage, string _folder = "")
	{
		if (GameIO.m_cachedUserDataPaths == null)
		{
			throw new Exception("GetCachedUserDataPath used before InitializeUserDataPaths");
		}
		return GameIO.m_cachedUserDataPaths[new ValueTuple<UserDataStorageType, string>(_storage, _folder)];
	}

	// Token: 0x06009FF5 RID: 40949 RVA: 0x003C1634 File Offset: 0x003BF834
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetDefaultUserGameDataPath(string _folder = "")
	{
		if (PlatformManager.MultiPlatform == null)
		{
			string cachedUserDataPath = GameIO.GetCachedUserDataPath(UserDataStorageType.DeviceLocal, _folder);
			Log.Error("Trying to access default save storage before native platform initialized. Falling back to " + cachedUserDataPath);
			return cachedUserDataPath;
		}
		return GameIO.GetUserGameDataPath(PlatformManager.MultiPlatform.UserDataRoaming.DefaultSaveStorage, _folder);
	}

	// Token: 0x06009FF6 RID: 40950 RVA: 0x003C1678 File Offset: 0x003BF878
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetUserGameDataPath(UserDataStorageType _storage, string _folder = "")
	{
		if (_storage == UserDataStorageType.DeviceLocal)
		{
			return GameIO.GetCachedUserDataPath(_storage, _folder);
		}
		if (_storage != UserDataStorageType.Roaming)
		{
			throw new Exception(string.Format("Unknown user data storage type {0}", _storage));
		}
		if (!PlatformManager.MultiPlatform.UserDataRoaming.IsSupported)
		{
			string cachedUserDataPath = GameIO.GetCachedUserDataPath(UserDataStorageType.DeviceLocal, _folder);
			Log.Error("Platform does not support save roaming. Falling back to " + cachedUserDataPath);
			return cachedUserDataPath;
		}
		return GameIO.GetCachedUserDataPath(_storage, _folder);
	}

	// Token: 0x06009FF7 RID: 40951 RVA: 0x003C16DE File Offset: 0x003BF8DE
	public static string GetDeviceLocalUserGameDataDir()
	{
		return GameIO.GetUserGameDataPath(UserDataStorageType.DeviceLocal, "");
	}

	// Token: 0x06009FF8 RID: 40952 RVA: 0x003C16EB File Offset: 0x003BF8EB
	public static string GetRoamingUserGameDataDir()
	{
		return GameIO.GetUserGameDataPath(UserDataStorageType.Roaming, "");
	}

	// Token: 0x06009FF9 RID: 40953 RVA: 0x003C16F8 File Offset: 0x003BF8F8
	public static string GetUserGameDataDir()
	{
		return GameIO.GetDefaultUserGameDataPath("");
	}

	// Token: 0x06009FFA RID: 40954 RVA: 0x003C1704 File Offset: 0x003BF904
	public static string GetUserGameDataDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "");
	}

	// Token: 0x06009FFB RID: 40955 RVA: 0x003C1711 File Offset: 0x003BF911
	public static string GetSaveGameRootDir()
	{
		return GameIO.GetDefaultUserGameDataPath("Saves");
	}

	// Token: 0x06009FFC RID: 40956 RVA: 0x003C171D File Offset: 0x003BF91D
	public static string GetSaveGameRootDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "Saves");
	}

	// Token: 0x06009FFD RID: 40957 RVA: 0x003C172A File Offset: 0x003BF92A
	public static string GetSaveGameDir(string _worldName, UserDataStorageType _storage)
	{
		return GameIO.GetSaveGameRootDir(_storage) + "/" + _worldName;
	}

	// Token: 0x06009FFE RID: 40958 RVA: 0x003C173D File Offset: 0x003BF93D
	public static string GetSaveGameDir(string _worldName, string _gameName, UserDataStorageType _storage)
	{
		return GameIO.GetSaveGameDir(_worldName, _storage) + "/" + _gameName;
	}

	// Token: 0x06009FFF RID: 40959 RVA: 0x003C1751 File Offset: 0x003BF951
	public static IEnumerable<string> GetSaveGameDirs(string _worldName)
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.IsSupported)
		{
			yield return GameIO.GetSaveGameDir(_worldName, UserDataStorageType.Roaming);
			if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional)
			{
				yield return GameIO.GetSaveGameDir(_worldName, UserDataStorageType.DeviceLocal);
			}
		}
		else
		{
			yield return GameIO.GetSaveGameDir(_worldName, UserDataStorageType.DeviceLocal);
		}
		yield break;
	}

	// Token: 0x0600A000 RID: 40960 RVA: 0x003C1761 File Offset: 0x003BF961
	public static string GetSaveGameDir()
	{
		return GameIO.GetSaveGameDir(GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameName), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType));
	}

	// Token: 0x0600A001 RID: 40961 RVA: 0x003C1780 File Offset: 0x003BF980
	public static string GetSaveGameLocalRootDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "SavesLocal");
	}

	// Token: 0x0600A002 RID: 40962 RVA: 0x003C1790 File Offset: 0x003BF990
	public static void SetSaveGameLocalGuid(string _guid)
	{
		GamePrefs.Set(EnumGamePrefs.GameGuidClient, _guid);
		if (string.IsNullOrEmpty(_guid))
		{
			return;
		}
		if (!PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, 0);
			return;
		}
		if (SdDirectory.Exists(GameIO.GetSaveGameLocalDir(UserDataStorageType.DeviceLocal, _guid)))
		{
			GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, 0);
			return;
		}
		GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, 1);
	}

	// Token: 0x0600A003 RID: 40963 RVA: 0x003C17F4 File Offset: 0x003BF9F4
	public static string GetSaveGameLocalDir()
	{
		string @string = GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
		if (string.IsNullOrEmpty(@string))
		{
			throw new Exception("Accessing GetSaveGameLocalDir while GameGuidClient is not yet set!");
		}
		return GameIO.GetSaveGameLocalDir((UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType), @string);
	}

	// Token: 0x0600A004 RID: 40964 RVA: 0x003C182F File Offset: 0x003BFA2F
	public static string GetSaveGameLocalDir(UserDataStorageType _storage, string _guid)
	{
		return GameIO.GetSaveGameLocalRootDir(_storage) + "/" + _guid;
	}

	// Token: 0x0600A005 RID: 40965 RVA: 0x003C1842 File Offset: 0x003BFA42
	public static string GetPlayerDataDir()
	{
		return Path.Combine(GameIO.GetSaveGameDir(), "Player");
	}

	// Token: 0x0600A006 RID: 40966 RVA: 0x003C1853 File Offset: 0x003BFA53
	public static string GetPlayerDataLocalDir()
	{
		return Path.Combine(GameIO.GetSaveGameLocalDir(), "Player");
	}

	// Token: 0x0600A007 RID: 40967 RVA: 0x003C1864 File Offset: 0x003BFA64
	public static int GetPlayerSaves(GameIO.FoundSave _foundSave = null, bool includeArchived = false)
	{
		GameIO.<>c__DisplayClass58_0 CS$<>8__locals1;
		CS$<>8__locals1.includeArchived = includeArchived;
		CS$<>8__locals1._foundSave = _foundSave;
		int num = 0;
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			num += GameIO.<GetPlayerSaves>g__SearchSaveDir|58_0(UserDataStorageType.Roaming, ref CS$<>8__locals1);
		}
		return num + GameIO.<GetPlayerSaves>g__SearchSaveDir|58_0(UserDataStorageType.DeviceLocal, ref CS$<>8__locals1);
	}

	// Token: 0x0600A008 RID: 40968 RVA: 0x003C18AB File Offset: 0x003BFAAB
	public static string GetSaveGameRegionDir()
	{
		return Path.Combine(GameIO.GetSaveGameDir(), "Region");
	}

	// Token: 0x0600A009 RID: 40969 RVA: 0x003C18BC File Offset: 0x003BFABC
	public static bool IsWorldGenerated(string _worldName, UserDataStorageType _storage)
	{
		return SdDirectory.Exists(Path.Combine(GameIO.GetUserGameDataDir(_storage), "GeneratedWorlds", _worldName));
	}

	// Token: 0x0600A00A RID: 40970 RVA: 0x003C18D4 File Offset: 0x003BFAD4
	public static bool IsAbsolutePath(string _path)
	{
		RuntimePlatform unityRuntimePlatform = GameIO.m_UnityRuntimePlatform;
		if (unityRuntimePlatform <= RuntimePlatform.WebGLPlayer)
		{
			switch (unityRuntimePlatform)
			{
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.IPhonePlayer:
				break;
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				goto IL_00E4;
			case RuntimePlatform.OSXWebPlayer:
			case RuntimePlatform.OSXDashboardPlayer:
			case RuntimePlatform.WindowsWebPlayer:
			case (RuntimePlatform)6:
				goto IL_013B;
			default:
				switch (unityRuntimePlatform)
				{
				case RuntimePlatform.Android:
				case RuntimePlatform.LinuxPlayer:
				case RuntimePlatform.LinuxEditor:
					break;
				case RuntimePlatform.NaCl:
				case (RuntimePlatform)14:
				case RuntimePlatform.FlashPlayer:
				case RuntimePlatform.WebGLPlayer:
					goto IL_013B;
				default:
					goto IL_013B;
				}
				break;
			}
		}
		else if (unityRuntimePlatform != RuntimePlatform.PS4)
		{
			if (unityRuntimePlatform == RuntimePlatform.XboxOne)
			{
				goto IL_00E4;
			}
			switch (unityRuntimePlatform)
			{
			case RuntimePlatform.tvOS:
			case RuntimePlatform.Switch:
			case RuntimePlatform.Lumin:
			case RuntimePlatform.Stadia:
			case RuntimePlatform.CloudRendering:
			case RuntimePlatform.EmbeddedLinuxArm64:
			case RuntimePlatform.EmbeddedLinuxArm32:
			case RuntimePlatform.EmbeddedLinuxX64:
			case RuntimePlatform.EmbeddedLinuxX86:
				goto IL_013B;
			case RuntimePlatform.GameCoreXboxSeries:
			case RuntimePlatform.GameCoreXboxOne:
			case RuntimePlatform.WindowsServer:
				goto IL_00E4;
			case RuntimePlatform.PS5:
			case RuntimePlatform.LinuxServer:
			case RuntimePlatform.OSXServer:
				break;
			default:
				goto IL_013B;
			}
		}
		return _path[0] == '/' || _path[0] == '\\' || _path.StartsWith("~/") || _path.StartsWith("~\\");
		IL_00E4:
		return _path[1] == ':' && (_path[2] == '/' || _path[2] == '\\') && ((_path[0] >= 'A' && _path[0] <= 'Z') || (_path[0] >= 'a' && _path[0] <= 'z'));
		IL_013B:
		throw new ArgumentOutOfRangeException("_path", _path, "Unsupported platform");
	}

	// Token: 0x0600A00B RID: 40971 RVA: 0x003C1A2C File Offset: 0x003BFC2C
	public static string MakeAbsolutePath(string _path)
	{
		if (GameIO.IsAbsolutePath(_path))
		{
			return _path;
		}
		return GameIO.GetGamePath() + "/" + _path;
	}

	// Token: 0x0600A00C RID: 40972 RVA: 0x003C1A48 File Offset: 0x003BFC48
	public static string GetOsStylePath(string _path)
	{
		if (GameIO.m_UnityRuntimePlatform != RuntimePlatform.WindowsPlayer && GameIO.m_UnityRuntimePlatform != RuntimePlatform.WindowsServer)
		{
			return _path.Replace("\\", "/");
		}
		return _path.Replace("/", "\\");
	}

	// Token: 0x0600A00D RID: 40973 RVA: 0x003C1A7C File Offset: 0x003BFC7C
	public static void CopyDirectory(string _sourceDirectory, string _targetDirectory)
	{
		SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(_sourceDirectory);
		SdDirectoryInfo sdDirectoryInfo2 = new SdDirectoryInfo(_targetDirectory);
		GameIO.CopyAll(sdDirectoryInfo, sdDirectoryInfo2);
	}

	// Token: 0x0600A00E RID: 40974 RVA: 0x003C1A9C File Offset: 0x003BFC9C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CopyAll(SdDirectoryInfo _source, SdDirectoryInfo _target)
	{
		SdDirectory.CreateDirectory(_target.FullName);
		foreach (SdFileInfo sdFileInfo in _source.GetFiles())
		{
			sdFileInfo.CopyTo(Path.Combine(_target.FullName, sdFileInfo.Name), true);
		}
		foreach (SdDirectoryInfo sdDirectoryInfo in _source.GetDirectories())
		{
			SdDirectoryInfo sdDirectoryInfo2 = _target.CreateSubdirectory(sdDirectoryInfo.Name);
			GameIO.CopyAll(sdDirectoryInfo, sdDirectoryInfo2);
		}
	}

	// Token: 0x0600A00F RID: 40975 RVA: 0x003C1B1C File Offset: 0x003BFD1C
	public static void SafeDirectoryCopy(string _sourceDirectory, string _targetDirectory)
	{
		try
		{
			GameIO.CopyDirectory(_sourceDirectory, _targetDirectory);
		}
		catch
		{
			try
			{
				if (SdDirectory.Exists(_targetDirectory))
				{
					SdDirectory.Delete(_targetDirectory, true);
				}
			}
			catch (Exception ex)
			{
				Log.Error("Failed to cleanup target path " + _targetDirectory + " after failed directory move");
				Log.Exception(ex);
			}
			throw;
		}
	}

	// Token: 0x0600A010 RID: 40976 RVA: 0x003C1B80 File Offset: 0x003BFD80
	public static void SafeDirectoryMove(string _sourceDirectory, string _targetDirectory)
	{
		GameIO.SafeDirectoryCopy(_sourceDirectory, _targetDirectory);
		try
		{
			SdDirectory.Delete(_sourceDirectory, true);
		}
		catch (Exception ex)
		{
			Log.Error("Could not remove source directory " + _sourceDirectory);
			Log.Exception(ex);
		}
	}

	// Token: 0x0600A011 RID: 40977 RVA: 0x003C1BC4 File Offset: 0x003BFDC4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OpenExplorerOnLinux(string _path)
	{
		_path = _path.Replace("\\", "/");
		if (SdFile.Exists(_path))
		{
			_path = Path.GetDirectoryName(_path);
		}
		if (_path.IndexOf(' ') >= 0)
		{
			_path = "\"" + _path + "\"";
		}
		try
		{
			Process.Start("xdg-open", _path);
		}
		catch (Exception ex)
		{
			Log.Error("Failed opening file browser:");
			Log.Exception(ex);
		}
	}

	// Token: 0x0600A012 RID: 40978 RVA: 0x003C1C40 File Offset: 0x003BFE40
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OpenExplorerOnMac(string _path)
	{
		_path = _path.Replace("\\", "/");
		bool flag = SdDirectory.Exists(_path);
		if (!_path.StartsWith("\""))
		{
			_path = "\"" + _path;
		}
		if (!_path.EndsWith("\""))
		{
			_path += "\"";
		}
		try
		{
			Process.Start("open", (flag ? "" : "-R ") + _path);
		}
		catch (Exception ex)
		{
			Log.Error("Failed opening Finder:");
			Log.Exception(ex);
		}
	}

	// Token: 0x0600A013 RID: 40979 RVA: 0x003C1CE0 File Offset: 0x003BFEE0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OpenExplorerOnWin(string _path)
	{
		_path = _path.Replace("/", "\\");
		bool flag = SdDirectory.Exists(_path);
		try
		{
			Process.Start("explorer.exe", (flag ? "/root,\"" : "/select,\"") + _path + "\"");
		}
		catch (Exception ex)
		{
			Log.Error("Failed opening Explorer:");
			Log.Exception(ex);
		}
	}

	// Token: 0x0600A014 RID: 40980 RVA: 0x003C1D50 File Offset: 0x003BFF50
	public static void OpenExplorer(string _path)
	{
		RuntimePlatform platform = Application.platform;
		if (platform > RuntimePlatform.WindowsEditor)
		{
			if (platform != RuntimePlatform.LinuxPlayer && platform != RuntimePlatform.LinuxEditor)
			{
				switch (platform)
				{
				case RuntimePlatform.LinuxServer:
					break;
				case RuntimePlatform.WindowsServer:
					goto IL_0039;
				case RuntimePlatform.OSXServer:
					goto IL_0040;
				default:
					goto IL_004E;
				}
			}
			GameIO.OpenExplorerOnLinux(_path);
			return;
		}
		if (platform <= RuntimePlatform.OSXPlayer)
		{
			goto IL_0040;
		}
		if (platform != RuntimePlatform.WindowsPlayer && platform != RuntimePlatform.WindowsEditor)
		{
			goto IL_004E;
		}
		IL_0039:
		GameIO.OpenExplorerOnWin(_path);
		return;
		IL_0040:
		GameIO.OpenExplorerOnMac(_path);
		return;
		IL_004E:
		Log.Error("Failed opening file browser: Unsupported OS");
	}

	// Token: 0x0600A015 RID: 40981 RVA: 0x003C1DB8 File Offset: 0x003BFFB8
	public static string IsRunningAsSnap()
	{
		if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Linux)
		{
			return null;
		}
		string environmentVariable = Environment.GetEnvironmentVariable("SNAP_NAME");
		if (environmentVariable == null)
		{
			Log.Out("Snap detection: Not running as Snap (no SNAP_NAME environment variable)");
			return null;
		}
		Log.Out("Snap detection: Running as Snap (Snap package: '" + environmentVariable + "')");
		return environmentVariable;
	}

	// Token: 0x0600A016 RID: 40982 RVA: 0x003C1E00 File Offset: 0x003C0000
	public static bool IsRunningInSteamRuntime()
	{
		if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Linux)
		{
			return false;
		}
		string text = "/etc/os-release";
		if (!SdFile.Exists(text))
		{
			Log.Out("SteamRuntime detection: Linux OS file " + text + " does not exist");
			return false;
		}
		foreach (string text2 in SdFile.ReadAllLines(text))
		{
			Match match = GameIO.linuxOsReleaseMatcher.Match(text2);
			if (match.Success)
			{
				string value = match.Groups[1].Value;
				bool flag = value.EqualsCaseInsensitive("steamrt");
				Log.Out(string.Format("SteamRuntime detection: OS ID='{0}', is SteamRT={1}", value, flag));
				return flag;
			}
		}
		Log.Out("SteamRuntime detection: No ID line matched");
		return false;
	}

	// Token: 0x0600A017 RID: 40983 RVA: 0x003C1EB4 File Offset: 0x003C00B4
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string GetPostTerminationAccessiblePath()
	{
		string text = GameIO.GetApplicationScratchPath() + "/" + "7 Days To Die".Replace(" ", "");
		if (!text.EndsWith('/') && !text.EndsWith('\\'))
		{
			text += "/";
		}
		return text;
	}

	// Token: 0x0600A018 RID: 40984 RVA: 0x003C1F08 File Offset: 0x003C0108
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <InitializeUserDataPaths>g__PopulateCache|36_0(UserDataStorageType _storage, string _rootPath)
	{
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, ""), _rootPath);
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, "Saves"), Path.Combine(_rootPath, "Saves"));
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, "SavesLocal"), Path.Combine(_rootPath, "SavesLocal"));
	}

	// Token: 0x0600A019 RID: 40985 RVA: 0x003C1F6C File Offset: 0x003C016C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static int <GetPlayerSaves>g__SearchSaveDir|58_0(UserDataStorageType storage, ref GameIO.<>c__DisplayClass58_0 A_1)
	{
		int num = 0;
		string saveGameRootDir = GameIO.GetSaveGameRootDir(storage);
		if (!SdDirectory.Exists(saveGameRootDir))
		{
			return 0;
		}
		SdFileSystemInfo[] array = new SdDirectoryInfo(saveGameRootDir).GetDirectories();
		foreach (SdDirectoryInfo sdDirectoryInfo in array)
		{
			string fullName = sdDirectoryInfo.FullName;
			if (SdDirectory.Exists(fullName))
			{
				SdFileSystemInfo[] array2 = new SdDirectoryInfo(fullName).GetDirectories();
				foreach (SdDirectoryInfo sdDirectoryInfo2 in array2)
				{
					if (!sdDirectoryInfo2.Name.Contains("#"))
					{
						bool flag = SdFile.Exists(Path.Combine(sdDirectoryInfo2.FullName, "archived.flag"));
						if (A_1.includeArchived || !flag)
						{
							string text = sdDirectoryInfo2.FullName + "/main.ttw";
							if (SdFile.Exists(text))
							{
								try
								{
									WorldState worldState = new WorldState();
									worldState.Load(text, false, false, false);
									if (worldState.gameVersion != null)
									{
										GameIO.FoundSave foundSave = A_1._foundSave;
										if (foundSave != null)
										{
											foundSave(storage, sdDirectoryInfo2.Name, sdDirectoryInfo.Name, SdFile.GetLastWriteTime(text), worldState, flag);
										}
										num++;
									}
								}
								catch (Exception ex)
								{
									Log.Warning("Error reading header of level '" + text + "'. Ignoring. Msg: " + ex.Message);
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0400785B RID: 30811
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_ApplicationScratchPath;

	// Token: 0x0400785C RID: 30812
	[PublicizedFrom(EAccessModifier.Private)]
	public const string XB1ScratchPath = "D:";

	// Token: 0x0400785D RID: 30813
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PS4ScratchPath = "/hostapp";

	// Token: 0x0400785E RID: 30814
	[PublicizedFrom(EAccessModifier.Private)]
	public static RuntimePlatform m_UnityRuntimePlatform = Application.platform;

	// Token: 0x0400785F RID: 30815
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_UnityDataPath = Application.dataPath;

	// Token: 0x04007860 RID: 30816
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_ApplicationPath;

	// Token: 0x04007861 RID: 30817
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] pathTrimCharacters = new char[] { '/', '\\' };

	// Token: 0x04007862 RID: 30818
	public static readonly char[] ResourcePathSeparators = new char[] { '/', '\\', '?' };

	// Token: 0x04007863 RID: 30819
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_lastDeviceLocalRoot;

	// Token: 0x04007864 RID: 30820
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<ValueTuple<UserDataStorageType, string>, string> m_cachedUserDataPaths;

	// Token: 0x04007865 RID: 30821
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex m_isRoamingPathRegex;

	// Token: 0x04007866 RID: 30822
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex linuxOsReleaseMatcher = new Regex("^ID=['\"]?([^'\"]+)['\"]?$");

	// Token: 0x020013E8 RID: 5096
	// (Invoke) Token: 0x0600A01B RID: 40987
	public delegate void FoundSave(UserDataStorageType storage, string saveName, string worldName, DateTime lastSaved, WorldState worldState, bool isArchived);
}
