using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Platform;
using UnityEngine;

// Token: 0x02001422 RID: 5154
public static class GameIO
{
	// Token: 0x0600A182 RID: 41346 RVA: 0x003CAD1B File Offset: 0x003C8F1B
	public static SdFileInfo[] GetDirectory(string _path, string _pattern)
	{
		if (!SdDirectory.Exists(_path))
		{
			return Array.Empty<SdFileInfo>();
		}
		return new SdDirectoryInfo(_path).GetFiles(_pattern);
	}

	// Token: 0x0600A183 RID: 41347 RVA: 0x003CAD38 File Offset: 0x003C8F38
	public static long FileSize(string _filePath)
	{
		SdFileInfo sdFileInfo = new SdFileInfo(_filePath);
		if (!sdFileInfo.Exists)
		{
			return -1L;
		}
		return sdFileInfo.Length;
	}

	// Token: 0x0600A184 RID: 41348 RVA: 0x003CAD5D File Offset: 0x003C8F5D
	public static string GetNormalizedPath(string _path)
	{
		return Path.GetFullPath(_path).TrimEnd(GameIO.pathTrimCharacters);
	}

	// Token: 0x0600A185 RID: 41349 RVA: 0x003CAD6F File Offset: 0x003C8F6F
	public static bool PathsEquals(string _path1, string _path2, bool _ignoreCase)
	{
		return string.Equals(GameIO.GetNormalizedPath(_path1), GameIO.GetNormalizedPath(_path2), _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
	}

	// Token: 0x0600A186 RID: 41350 RVA: 0x003CAD8C File Offset: 0x003C8F8C
	public static string GetFileExtension(string _filename)
	{
		int num;
		if (_filename.Length > 4 && (num = _filename.LastIndexOf('.')) > 0)
		{
			return _filename.Substring(num);
		}
		return _filename;
	}

	// Token: 0x0600A187 RID: 41351 RVA: 0x003CADB8 File Offset: 0x003C8FB8
	public static string RemoveFileExtension(string _filename)
	{
		int num;
		if (_filename.Length > 4 && (num = _filename.LastIndexOf('.')) > 0)
		{
			return _filename.Substring(0, num);
		}
		return _filename;
	}

	// Token: 0x0600A188 RID: 41352 RVA: 0x003CADE5 File Offset: 0x003C8FE5
	public static string RemoveExtension(string _filename, string _extension)
	{
		if (_filename.Length > _extension.Length && _filename.EndsWith(_extension, StringComparison.InvariantCultureIgnoreCase))
		{
			return _filename.Substring(0, _filename.Length - _extension.Length);
		}
		return _filename;
	}

	// Token: 0x0600A189 RID: 41353 RVA: 0x003CAE18 File Offset: 0x003C9018
	public static string GetFilenameFromPath(string _filepath)
	{
		int num = _filepath.LastIndexOfAny(GameIO.ResourcePathSeparators);
		if (num >= 0 && num < _filepath.Length)
		{
			_filepath = _filepath.Substring(num + 1);
		}
		return _filepath;
	}

	// Token: 0x0600A18A RID: 41354 RVA: 0x003CAE4C File Offset: 0x003C904C
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

	// Token: 0x0600A18B RID: 41355 RVA: 0x003CAEAC File Offset: 0x003C90AC
	public static string GetDirectoryFromPath(string _filepath)
	{
		int num = _filepath.LastIndexOf('/');
		if (num > 0 && num < _filepath.Length)
		{
			_filepath = _filepath.Substring(0, num);
		}
		return _filepath;
	}

	// Token: 0x0600A18C RID: 41356 RVA: 0x003CAEDA File Offset: 0x003C90DA
	public static long GetDirectorySize(string _filepath, bool recursive = true)
	{
		return GameIO.GetDirectorySize(new SdDirectoryInfo(_filepath), recursive);
	}

	// Token: 0x0600A18D RID: 41357 RVA: 0x003CAEE8 File Offset: 0x003C90E8
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

	// Token: 0x0600A18E RID: 41358 RVA: 0x003CAF53 File Offset: 0x003C9153
	public static string GetGameDir(string _relDir)
	{
		return GameIO.GetApplicationPath() + "/" + _relDir;
	}

	// Token: 0x0600A18F RID: 41359 RVA: 0x003CAF68 File Offset: 0x003C9168
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

	// Token: 0x0600A190 RID: 41360 RVA: 0x003CAFCB File Offset: 0x003C91CB
	public static string GetGamePath()
	{
		if (GameIO.m_UnityRuntimePlatform != RuntimePlatform.OSXPlayer && GameIO.m_UnityRuntimePlatform != RuntimePlatform.OSXServer)
		{
			return GameIO.m_UnityDataPath + "/..";
		}
		return GameIO.m_UnityDataPath + "/../..";
	}

	// Token: 0x0600A191 RID: 41361 RVA: 0x003CAFFD File Offset: 0x003C91FD
	public static string GetGameExecutablePath()
	{
		return GameIO.GetGamePath() + "/" + GameIO.GetGameExecutableName();
	}

	// Token: 0x0600A192 RID: 41362 RVA: 0x003CB014 File Offset: 0x003C9214
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

	// Token: 0x0600A193 RID: 41363 RVA: 0x003CB14E File Offset: 0x003C934E
	public static string GetLauncherExecutablePath()
	{
		return GameIO.GetGamePath() + "/" + GameIO.GetLauncherExecutableName();
	}

	// Token: 0x0600A194 RID: 41364 RVA: 0x003CB164 File Offset: 0x003C9364
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

	// Token: 0x0600A195 RID: 41365 RVA: 0x003CB2A0 File Offset: 0x003C94A0
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

	// Token: 0x0600A196 RID: 41366 RVA: 0x003CB30C File Offset: 0x003C950C
	public static string GetApplicationTempPath()
	{
		return PlatformApplicationManager.Application.temporaryCachePath;
	}

	// Token: 0x0600A197 RID: 41367 RVA: 0x003CB318 File Offset: 0x003C9518
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

	// Token: 0x0600A198 RID: 41368 RVA: 0x003CB338 File Offset: 0x003C9538
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

	// Token: 0x0600A199 RID: 41369 RVA: 0x003CB3F9 File Offset: 0x003C95F9
	public static string GetDefaultPersistentDataPath()
	{
		return GameIO.GetDocumentPath() + "/" + "7 Days To Die".Replace(" ", "");
	}

	// Token: 0x0600A19A RID: 41370 RVA: 0x003CB420 File Offset: 0x003C9620
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

	// Token: 0x0600A19B RID: 41371 RVA: 0x003CB4CD File Offset: 0x003C96CD
	public static bool IsRoamingUserDataPath(string _path)
	{
		if (GameIO.m_isRoamingPathRegex == null)
		{
			throw new Exception("IsRoamingUserDataPath used before InitializeUserDataPaths");
		}
		return GameIO.m_isRoamingPathRegex.IsMatch(_path);
	}

	// Token: 0x0600A19C RID: 41372 RVA: 0x003CB4EC File Offset: 0x003C96EC
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetCachedUserDataPath(UserDataStorageType _storage, string _folder = "")
	{
		if (GameIO.m_cachedUserDataPaths == null)
		{
			throw new Exception("GetCachedUserDataPath used before InitializeUserDataPaths");
		}
		return GameIO.m_cachedUserDataPaths[new ValueTuple<UserDataStorageType, string>(_storage, _folder)];
	}

	// Token: 0x0600A19D RID: 41373 RVA: 0x003CB514 File Offset: 0x003C9714
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

	// Token: 0x0600A19E RID: 41374 RVA: 0x003CB558 File Offset: 0x003C9758
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

	// Token: 0x0600A19F RID: 41375 RVA: 0x003CB5BE File Offset: 0x003C97BE
	public static string GetDeviceLocalUserGameDataDir()
	{
		return GameIO.GetUserGameDataPath(UserDataStorageType.DeviceLocal, "");
	}

	// Token: 0x0600A1A0 RID: 41376 RVA: 0x003CB5CB File Offset: 0x003C97CB
	public static string GetRoamingUserGameDataDir()
	{
		return GameIO.GetUserGameDataPath(UserDataStorageType.Roaming, "");
	}

	// Token: 0x0600A1A1 RID: 41377 RVA: 0x003CB5D8 File Offset: 0x003C97D8
	public static string GetUserGameDataDir()
	{
		return GameIO.GetDefaultUserGameDataPath("");
	}

	// Token: 0x0600A1A2 RID: 41378 RVA: 0x003CB5E4 File Offset: 0x003C97E4
	public static string GetUserGameDataDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "");
	}

	// Token: 0x0600A1A3 RID: 41379 RVA: 0x003CB5F1 File Offset: 0x003C97F1
	public static string GetSaveGameRootDir()
	{
		return GameIO.GetDefaultUserGameDataPath("Saves");
	}

	// Token: 0x0600A1A4 RID: 41380 RVA: 0x003CB5FD File Offset: 0x003C97FD
	public static string GetSaveGameRootDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "Saves");
	}

	// Token: 0x0600A1A5 RID: 41381 RVA: 0x003CB60A File Offset: 0x003C980A
	public static string GetSaveGameDir(string _worldName, UserDataStorageType _storage)
	{
		return GameIO.GetSaveGameRootDir(_storage) + "/" + _worldName;
	}

	// Token: 0x0600A1A6 RID: 41382 RVA: 0x003CB61D File Offset: 0x003C981D
	public static string GetSaveGameDir(string _worldName, string _gameName, UserDataStorageType _storage)
	{
		return GameIO.GetSaveGameDir(_worldName, _storage) + "/" + _gameName;
	}

	// Token: 0x0600A1A7 RID: 41383 RVA: 0x003CB631 File Offset: 0x003C9831
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

	// Token: 0x0600A1A8 RID: 41384 RVA: 0x003CB641 File Offset: 0x003C9841
	public static string GetSaveGameDir()
	{
		return GameIO.GetSaveGameDir(GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameName), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType));
	}

	// Token: 0x0600A1A9 RID: 41385 RVA: 0x003CB660 File Offset: 0x003C9860
	public static string GetSaveGameLocalRootDir(UserDataStorageType _storage)
	{
		return GameIO.GetUserGameDataPath(_storage, "SavesLocal");
	}

	// Token: 0x0600A1AA RID: 41386 RVA: 0x003CB670 File Offset: 0x003C9870
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

	// Token: 0x0600A1AB RID: 41387 RVA: 0x003CB6D4 File Offset: 0x003C98D4
	public static string GetSaveGameLocalDir()
	{
		string @string = GamePrefs.GetString(EnumGamePrefs.GameGuidClient);
		if (string.IsNullOrEmpty(@string))
		{
			throw new Exception("Accessing GetSaveGameLocalDir while GameGuidClient is not yet set!");
		}
		return GameIO.GetSaveGameLocalDir((UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType), @string);
	}

	// Token: 0x0600A1AC RID: 41388 RVA: 0x003CB70F File Offset: 0x003C990F
	public static string GetSaveGameLocalDir(UserDataStorageType _storage, string _guid)
	{
		return GameIO.GetSaveGameLocalRootDir(_storage) + "/" + _guid;
	}

	// Token: 0x0600A1AD RID: 41389 RVA: 0x003CB722 File Offset: 0x003C9922
	public static string GetPlayerDataDir()
	{
		return Path.Combine(GameIO.GetSaveGameDir(), "Player");
	}

	// Token: 0x0600A1AE RID: 41390 RVA: 0x003CB733 File Offset: 0x003C9933
	public static string GetPlayerDataLocalDir()
	{
		return Path.Combine(GameIO.GetSaveGameLocalDir(), "Player");
	}

	// Token: 0x0600A1AF RID: 41391 RVA: 0x003CB744 File Offset: 0x003C9944
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

	// Token: 0x0600A1B0 RID: 41392 RVA: 0x003CB78B File Offset: 0x003C998B
	public static string GetSaveGameRegionDir()
	{
		return Path.Combine(GameIO.GetSaveGameDir(), "Region");
	}

	// Token: 0x0600A1B1 RID: 41393 RVA: 0x003CB79C File Offset: 0x003C999C
	public static bool IsWorldGenerated(string _worldName, UserDataStorageType _storage)
	{
		return SdDirectory.Exists(Path.Combine(GameIO.GetUserGameDataDir(_storage), "GeneratedWorlds", _worldName));
	}

	// Token: 0x0600A1B2 RID: 41394 RVA: 0x003CB7B4 File Offset: 0x003C99B4
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

	// Token: 0x0600A1B3 RID: 41395 RVA: 0x003CB90C File Offset: 0x003C9B0C
	public static string MakeAbsolutePath(string _path)
	{
		if (GameIO.IsAbsolutePath(_path))
		{
			return _path;
		}
		return GameIO.GetGamePath() + "/" + _path;
	}

	// Token: 0x0600A1B4 RID: 41396 RVA: 0x003CB928 File Offset: 0x003C9B28
	public static string GetOsStylePath(string _path)
	{
		if (GameIO.m_UnityRuntimePlatform != RuntimePlatform.WindowsPlayer && GameIO.m_UnityRuntimePlatform != RuntimePlatform.WindowsServer)
		{
			return _path.Replace("\\", "/");
		}
		return _path.Replace("/", "\\");
	}

	// Token: 0x0600A1B5 RID: 41397 RVA: 0x003CB95C File Offset: 0x003C9B5C
	public static void CopyDirectory(string _sourceDirectory, string _targetDirectory)
	{
		SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(_sourceDirectory);
		SdDirectoryInfo sdDirectoryInfo2 = new SdDirectoryInfo(_targetDirectory);
		GameIO.CopyAll(sdDirectoryInfo, sdDirectoryInfo2);
	}

	// Token: 0x0600A1B6 RID: 41398 RVA: 0x003CB97C File Offset: 0x003C9B7C
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

	// Token: 0x0600A1B7 RID: 41399 RVA: 0x003CB9FC File Offset: 0x003C9BFC
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

	// Token: 0x0600A1B8 RID: 41400 RVA: 0x003CBA60 File Offset: 0x003C9C60
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

	// Token: 0x0600A1B9 RID: 41401 RVA: 0x003CBAA4 File Offset: 0x003C9CA4
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

	// Token: 0x0600A1BA RID: 41402 RVA: 0x003CBB20 File Offset: 0x003C9D20
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

	// Token: 0x0600A1BB RID: 41403 RVA: 0x003CBBC0 File Offset: 0x003C9DC0
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

	// Token: 0x0600A1BC RID: 41404 RVA: 0x003CBC30 File Offset: 0x003C9E30
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

	// Token: 0x0600A1BD RID: 41405 RVA: 0x003CBC98 File Offset: 0x003C9E98
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

	// Token: 0x0600A1BE RID: 41406 RVA: 0x003CBCE0 File Offset: 0x003C9EE0
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

	// Token: 0x0600A1BF RID: 41407 RVA: 0x003CBD94 File Offset: 0x003C9F94
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

	// Token: 0x0600A1C0 RID: 41408 RVA: 0x003CBDE8 File Offset: 0x003C9FE8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <InitializeUserDataPaths>g__PopulateCache|36_0(UserDataStorageType _storage, string _rootPath)
	{
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, ""), _rootPath);
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, "Saves"), Path.Combine(_rootPath, "Saves"));
		GameIO.m_cachedUserDataPaths.Add(new ValueTuple<UserDataStorageType, string>(_storage, "SavesLocal"), Path.Combine(_rootPath, "SavesLocal"));
	}

	// Token: 0x0600A1C1 RID: 41409 RVA: 0x003CBE4C File Offset: 0x003CA04C
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

	// Token: 0x040079AF RID: 31151
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_ApplicationScratchPath;

	// Token: 0x040079B0 RID: 31152
	[PublicizedFrom(EAccessModifier.Private)]
	public const string XB1ScratchPath = "D:";

	// Token: 0x040079B1 RID: 31153
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PS4ScratchPath = "/hostapp";

	// Token: 0x040079B2 RID: 31154
	[PublicizedFrom(EAccessModifier.Private)]
	public static RuntimePlatform m_UnityRuntimePlatform = Application.platform;

	// Token: 0x040079B3 RID: 31155
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_UnityDataPath = Application.dataPath;

	// Token: 0x040079B4 RID: 31156
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_ApplicationPath;

	// Token: 0x040079B5 RID: 31157
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] pathTrimCharacters = new char[] { '/', '\\' };

	// Token: 0x040079B6 RID: 31158
	public static readonly char[] ResourcePathSeparators = new char[] { '/', '\\', '?' };

	// Token: 0x040079B7 RID: 31159
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_lastDeviceLocalRoot;

	// Token: 0x040079B8 RID: 31160
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<ValueTuple<UserDataStorageType, string>, string> m_cachedUserDataPaths;

	// Token: 0x040079B9 RID: 31161
	[PublicizedFrom(EAccessModifier.Private)]
	public static Regex m_isRoamingPathRegex;

	// Token: 0x040079BA RID: 31162
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex linuxOsReleaseMatcher = new Regex("^ID=['\"]?([^'\"]+)['\"]?$");

	// Token: 0x02001423 RID: 5155
	// (Invoke) Token: 0x0600A1C3 RID: 41411
	public delegate void FoundSave(UserDataStorageType storage, string saveName, string worldName, DateTime lastSaved, WorldState worldState, bool isArchived);
}
