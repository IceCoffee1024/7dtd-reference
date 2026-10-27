using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine;

// Token: 0x02001434 RID: 5172
public static class GameIO
{
	// Token: 0x0600A1DA RID: 41434 RVA: 0x003CD11F File Offset: 0x003CB31F
	public static SdFileInfo[] GetDirectory(string _path, string _pattern)
	{
		if (!SdDirectory.Exists(_path))
		{
			return Array.Empty<SdFileInfo>();
		}
		return new SdDirectoryInfo(_path).GetFiles(_pattern);
	}

	// Token: 0x0600A1DB RID: 41435 RVA: 0x003CD13C File Offset: 0x003CB33C
	public static long FileSize(string _filePath)
	{
		SdFileInfo sdFileInfo = new SdFileInfo(_filePath);
		if (!sdFileInfo.Exists)
		{
			return -1L;
		}
		return sdFileInfo.Length;
	}

	// Token: 0x0600A1DC RID: 41436 RVA: 0x003CD161 File Offset: 0x003CB361
	public static string GetNormalizedPath(string _path)
	{
		return Path.GetFullPath(_path).TrimEnd(GameIO.pathTrimCharacters);
	}

	// Token: 0x0600A1DD RID: 41437 RVA: 0x003CD173 File Offset: 0x003CB373
	public static bool PathsEquals(string _path1, string _path2, bool _ignoreCase)
	{
		return string.Equals(GameIO.GetNormalizedPath(_path1), GameIO.GetNormalizedPath(_path2), _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
	}

	// Token: 0x0600A1DE RID: 41438 RVA: 0x003CD190 File Offset: 0x003CB390
	public static string GetFileExtension(string _filename)
	{
		int num;
		if (_filename.Length > 4 && (num = _filename.LastIndexOf('.')) > 0)
		{
			return _filename.Substring(num);
		}
		return _filename;
	}

	// Token: 0x0600A1DF RID: 41439 RVA: 0x003CD1BC File Offset: 0x003CB3BC
	public static string RemoveFileExtension(string _filename)
	{
		int num;
		if (_filename.Length > 4 && (num = _filename.LastIndexOf('.')) > 0)
		{
			return _filename.Substring(0, num);
		}
		return _filename;
	}

	// Token: 0x0600A1E0 RID: 41440 RVA: 0x003CD1E9 File Offset: 0x003CB3E9
	public static string RemoveExtension(string _filename, string _extension)
	{
		if (_filename.Length > _extension.Length && _filename.EndsWith(_extension, StringComparison.InvariantCultureIgnoreCase))
		{
			return _filename.Substring(0, _filename.Length - _extension.Length);
		}
		return _filename;
	}

	// Token: 0x0600A1E1 RID: 41441 RVA: 0x003CD21C File Offset: 0x003CB41C
	public static string GetFilenameFromPath(string _filepath)
	{
		int num = _filepath.LastIndexOfAny(GameIO.ResourcePathSeparators);
		if (num >= 0 && num < _filepath.Length)
		{
			_filepath = _filepath.Substring(num + 1);
		}
		return _filepath;
	}

	// Token: 0x0600A1E2 RID: 41442 RVA: 0x003CD250 File Offset: 0x003CB450
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

	// Token: 0x0600A1E3 RID: 41443 RVA: 0x003CD2B0 File Offset: 0x003CB4B0
	public static string GetDirectoryFromPath(string _filepath)
	{
		int num = _filepath.LastIndexOf('/');
		if (num > 0 && num < _filepath.Length)
		{
			_filepath = _filepath.Substring(0, num);
		}
		return _filepath;
	}

	// Token: 0x0600A1E4 RID: 41444 RVA: 0x003CD2DE File Offset: 0x003CB4DE
	public static long GetDirectorySize(string _filepath, bool recursive = true)
	{
		return GameIO.GetDirectorySize(new SdDirectoryInfo(_filepath), recursive);
	}

	// Token: 0x0600A1E5 RID: 41445 RVA: 0x003CD2EC File Offset: 0x003CB4EC
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

	// Token: 0x0600A1E6 RID: 41446 RVA: 0x003CD357 File Offset: 0x003CB557
	public static string GetGameDir(string _relDir)
	{
		return GameIO.GetApplicationPath() + "/" + _relDir;
	}

	// Token: 0x0600A1E7 RID: 41447 RVA: 0x003CD36C File Offset: 0x003CB56C
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

	// Token: 0x0600A1E8 RID: 41448 RVA: 0x003CD3CF File Offset: 0x003CB5CF
	public static string GetGamePath()
	{
		if (GameIO.m_UnityRuntimePlatform != RuntimePlatform.OSXPlayer && GameIO.m_UnityRuntimePlatform != RuntimePlatform.OSXServer)
		{
			return GameIO.m_UnityDataPath + "/..";
		}
		return GameIO.m_UnityDataPath + "/../..";
	}

	// Token: 0x0600A1E9 RID: 41449 RVA: 0x003CD401 File Offset: 0x003CB601
	public static string GetGameExecutablePath()
	{
		return GameIO.GetGamePath() + "/" + GameIO.GetGameExecutableName();
	}

	// Token: 0x0600A1EA RID: 41450 RVA: 0x003CD418 File Offset: 0x003CB618
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

	// Token: 0x0600A1EB RID: 41451 RVA: 0x003CD552 File Offset: 0x003CB752
	public static string GetLauncherExecutablePath()
	{
		return GameIO.GetGamePath() + "/" + GameIO.GetLauncherExecutableName();
	}

	// Token: 0x0600A1EC RID: 41452 RVA: 0x003CD568 File Offset: 0x003CB768
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

	// Token: 0x0600A1ED RID: 41453 RVA: 0x003CD6A4 File Offset: 0x003CB8A4
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

	// Token: 0x0600A1EE RID: 41454 RVA: 0x003CD710 File Offset: 0x003CB910
	public static string GetApplicationTempPath()
	{
		return PlatformApplicationManager.Application.temporaryCachePath;
	}

	// Token: 0x0600A1EF RID: 41455 RVA: 0x003CD71C File Offset: 0x003CB91C
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

	// Token: 0x0600A1F0 RID: 41456 RVA: 0x003CD73C File Offset: 0x003CB93C
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

	// Token: 0x0600A1F1 RID: 41457 RVA: 0x003CD7FD File Offset: 0x003CB9FD
	public static string GetDefaultPersistentDataPath()
	{
		return GameIO.GetDocumentPath() + "/" + "7 Days To Die".Replace(" ", "");
	}

	// Token: 0x0600A1F2 RID: 41458 RVA: 0x003CD824 File Offset: 0x003CBA24
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

	// Token: 0x0600A1F3 RID: 41459 RVA: 0x003CD8D1 File Offset: 0x003CBAD1
	public static bool IsRoamingUserDataPath(string _path)
	{
		if (GameIO.m_isRoamingPathRegex == null)
		{
			throw new Exception("IsRoamingUserDataPath used before InitializeUserDataPaths");
		}
		return GameIO.m_isRoamingPathRegex.IsMatch(_path);
	}

	// Token: 0x0600A1F4 RID: 41460 RVA: 0x003CD8F0 File Offset: 0x003CBAF0
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetCachedUserDataPath(UserDataStorageType _storage, string _folder = "")
	{
		if (GameIO.m_cachedUserDataPaths == null)
		{
			throw new Exception("GetCachedUserDataPath used before InitializeUserDataPaths");
		}
		return GameIO.m_cachedUserDataPaths[new ValueTuple<UserDataStorageType, string>(_storage, _folder)];
	}

	// Token: 0x0600A1F5 RID: 41461 RVA: 0x003CD918 File Offset: 0x003CBB18
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

	// Token: 0x0600A1F6 RID: 41462 RVA: 0x003CD95C File Offset: 0x003CBB5C
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

	// Token: 0x0600A1F7 RID: 41463 RVA: 0x003CD9C2 File Offset: 0x003CBBC2
	public static string GetDeviceLocalUserGameDataDir()
	{
		return GameIO.GetUserGameDataPath(UserDataStorageType.DeviceLocal, "");
	}

	// Token: 0x0600A1F8 RID: 41464 RVA: 0x003CD9CF File Offset: 0x003CBBCF
	public static string GetRoamingUserGameDataDir()
	{
		return GameIO.GetUserGameDataPath(UserDataStorageType.Roaming, "");
	}

	// Token: 0x0600A1F9 RID: 41465 RVA: 0x003CD9DC File Offset: 0x003CBBDC
	public static string GetUserGameDataDir()
	{
		return GameIO.GetDefaultUserGameDataPath("");
	}

	// Token: 0x0600A1FA RID: 41466 RVA: 0x003CD9E8 File Offset: 0x003CBBE8
	public static string GetUserGameDataDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "");
	}

	// Token: 0x0600A1FB RID: 41467 RVA: 0x003CD9F5 File Offset: 0x003CBBF5
	public static string GetSaveGameRootDir()
	{
		return GameIO.GetDefaultUserGameDataPath("Saves");
	}

	// Token: 0x0600A1FC RID: 41468 RVA: 0x003CDA01 File Offset: 0x003CBC01
	public static string GetSaveGameRootDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "Saves");
	}

	// Token: 0x0600A1FD RID: 41469 RVA: 0x003CDA0E File Offset: 0x003CBC0E
	public static string GetSaveGameDir(string _worldName, UserDataStorageType _storage)
	{
		return GameIO.GetSaveGameRootDir(_storage) + "/" + _worldName;
	}

	// Token: 0x0600A1FE RID: 41470 RVA: 0x003CDA21 File Offset: 0x003CBC21
	public static string GetSaveGameDir(string _worldName, string _gameName, UserDataStorageType _storage)
	{
		return GameIO.GetSaveGameDir(_worldName, _storage) + "/" + _gameName;
	}

	// Token: 0x0600A1FF RID: 41471 RVA: 0x003CDA35 File Offset: 0x003CBC35
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

	// Token: 0x0600A200 RID: 41472 RVA: 0x003CDA45 File Offset: 0x003CBC45
	public static string GetSaveGameDir()
	{
		return GameIO.GetSaveGameDir(GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameName), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType));
	}

	// Token: 0x0600A201 RID: 41473 RVA: 0x003CDA64 File Offset: 0x003CBC64
	public static string GetSaveGameLocalRootDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "SavesLocal");
	}

	// Token: 0x0600A202 RID: 41474 RVA: 0x003CDA74 File Offset: 0x003CBC74
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

	// Token: 0x0600A203 RID: 41475 RVA: 0x003CDAD8 File Offset: 0x003CBCD8
	public static string GetSaveGameLocalDir()
	{
		string @string = GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
		if (string.IsNullOrEmpty(@string))
		{
			throw new Exception("Accessing GetSaveGameLocalDir while GameGuidClient is not yet set!");
		}
		return GameIO.GetSaveGameLocalDir((UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType), @string);
	}

	// Token: 0x0600A204 RID: 41476 RVA: 0x003CDB13 File Offset: 0x003CBD13
	public static string GetSaveGameLocalDir(UserDataStorageType _storage, string _guid)
	{
		return GameIO.GetSaveGameLocalRootDir(_storage) + "/" + _guid;
	}

	// Token: 0x0600A205 RID: 41477 RVA: 0x003CDB26 File Offset: 0x003CBD26
	public static string GetPlayerDataDir()
	{
		return Path.Combine(GameIO.GetSaveGameDir(), "Player");
	}

	// Token: 0x0600A206 RID: 41478 RVA: 0x003CDB37 File Offset: 0x003CBD37
	public static string GetPlayerDataLocalDir()
	{
		return Path.Combine(GameIO.GetSaveGameLocalDir(), "Player");
	}

	// Token: 0x0600A207 RID: 41479 RVA: 0x003CDB48 File Offset: 0x003CBD48
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

	// Token: 0x0600A208 RID: 41480 RVA: 0x003CDB8F File Offset: 0x003CBD8F
	public static string GetSaveGameRegionDir()
	{
		return Path.Combine(GameIO.GetSaveGameDir(), "Region");
	}

	// Token: 0x0600A209 RID: 41481 RVA: 0x003CDBA0 File Offset: 0x003CBDA0
	public static bool IsWorldGenerated(string _worldName, UserDataStorageType _storage)
	{
		return SdDirectory.Exists(Path.Combine(GameIO.GetUserGameDataDir(_storage), "GeneratedWorlds", _worldName));
	}

	// Token: 0x0600A20A RID: 41482 RVA: 0x003CDBB8 File Offset: 0x003CBDB8
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

	// Token: 0x0600A20B RID: 41483 RVA: 0x003CDD10 File Offset: 0x003CBF10
	public static string MakeAbsolutePath(string _path)
	{
		if (GameIO.IsAbsolutePath(_path))
		{
			return _path;
		}
		return GameIO.GetGamePath() + "/" + _path;
	}

	// Token: 0x0600A20C RID: 41484 RVA: 0x003CDD2C File Offset: 0x003CBF2C
	public static string GetOsStylePath(string _path)
	{
		if (GameIO.m_UnityRuntimePlatform != RuntimePlatform.WindowsPlayer && GameIO.m_UnityRuntimePlatform != RuntimePlatform.WindowsServer)
		{
			return _path.Replace("\\", "/");
		}
		return _path.Replace("/", "\\");
	}

	// Token: 0x0600A20D RID: 41485 RVA: 0x003CDD60 File Offset: 0x003CBF60
	public static void CopyDirectory(string _sourceDirectory, string _targetDirectory)
	{
		SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(_sourceDirectory);
		SdDirectoryInfo sdDirectoryInfo2 = new SdDirectoryInfo(_targetDirectory);
		GameIO.CopyAll(sdDirectoryInfo, sdDirectoryInfo2);
	}

	// Token: 0x0600A20E RID: 41486 RVA: 0x003CDD80 File Offset: 0x003CBF80
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

	// Token: 0x0600A20F RID: 41487 RVA: 0x003CDE00 File Offset: 0x003CC000
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

	// Token: 0x0600A210 RID: 41488 RVA: 0x003CDE64 File Offset: 0x003CC064
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

	// Token: 0x0600A211 RID: 41489 RVA: 0x003CDEA8 File Offset: 0x003CC0A8
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

	// Token: 0x0600A212 RID: 41490 RVA: 0x003CDF24 File Offset: 0x003CC124
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

	// Token: 0x0600A213 RID: 41491 RVA: 0x003CDFC4 File Offset: 0x003CC1C4
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

	// Token: 0x0600A214 RID: 41492 RVA: 0x003CE034 File Offset: 0x003CC234
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

	// Token: 0x0600A215 RID: 41493 RVA: 0x003CE09C File Offset: 0x003CC29C
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

	// Token: 0x0600A216 RID: 41494 RVA: 0x003CE0E4 File Offset: 0x003CC2E4
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

	// Token: 0x0600A217 RID: 41495 RVA: 0x003CE198 File Offset: 0x003CC398
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

	// Token: 0x0600A218 RID: 41496 RVA: 0x003CE1EC File Offset: 0x003CC3EC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <InitializeUserDataPaths>g__PopulateCache|36_0(UserDataStorageType _storage, string _rootPath)
	{
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, ""), _rootPath);
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, "Saves"), Path.Combine(_rootPath, "Saves"));
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, "SavesLocal"), Path.Combine(_rootPath, "SavesLocal"));
	}

	// Token: 0x0600A219 RID: 41497 RVA: 0x003CE250 File Offset: 0x003CC450
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

	// Token: 0x040079F0 RID: 31216
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_ApplicationScratchPath;

	// Token: 0x040079F1 RID: 31217
	[PublicizedFrom(EAccessModifier.Private)]
	public const string XB1ScratchPath = "D:";

	// Token: 0x040079F2 RID: 31218
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PS4ScratchPath = "/hostapp";

	// Token: 0x040079F3 RID: 31219
	[PublicizedFrom(EAccessModifier.Private)]
	public static RuntimePlatform m_UnityRuntimePlatform = Application.platform;

	// Token: 0x040079F4 RID: 31220
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_UnityDataPath = Application.dataPath;

	// Token: 0x040079F5 RID: 31221
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_ApplicationPath;

	// Token: 0x040079F6 RID: 31222
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] pathTrimCharacters = new char[] { '/', '\\' };

	// Token: 0x040079F7 RID: 31223
	public static readonly char[] ResourcePathSeparators = new char[] { '/', '\\', '?' };

	// Token: 0x040079F8 RID: 31224
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_lastDeviceLocalRoot;

	// Token: 0x040079F9 RID: 31225
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<ValueTuple<UserDataStorageType, string>, string> m_cachedUserDataPaths;

	// Token: 0x040079FA RID: 31226
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex m_isRoamingPathRegex;

	// Token: 0x040079FB RID: 31227
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex linuxOsReleaseMatcher = new Regex("^ID=['\"]?([^'\"]+)['\"]?$");

	// Token: 0x02001435 RID: 5173
	// (Invoke) Token: 0x0600A21B RID: 41499
	public delegate void FoundSave(UserDataStorageType storage, string saveName, string worldName, DateTime lastSaved, WorldState worldState, bool isArchived);
}
