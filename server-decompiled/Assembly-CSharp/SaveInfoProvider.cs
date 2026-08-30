using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Platform;
using Platform.XBL.Save.Storage;
using UnityEngine;

// Token: 0x02001498 RID: 5272
public class SaveInfoProvider
{
	// Token: 0x1700136C RID: 4972
	// (get) Token: 0x0600A562 RID: 42338 RVA: 0x003D87D4 File Offset: 0x003D69D4
	public static SaveInfoProvider Instance
	{
		get
		{
			if (SaveInfoProvider.instance == null)
			{
				SaveInfoProvider.instance = new SaveInfoProvider();
			}
			return SaveInfoProvider.instance;
		}
	}

	// Token: 0x1700136D RID: 4973
	// (get) Token: 0x0600A563 RID: 42339 RVA: 0x003D87EC File Offset: 0x003D69EC
	public static bool DataLimitEnabled
	{
		get
		{
			return SaveDataUtils.SaveDataManager.ShouldLimitSize();
		}
	}

	// Token: 0x0600A564 RID: 42340 RVA: 0x003D87F8 File Offset: 0x003D69F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string worldName, string fullPath)
	{
		return worldName.ToLowerInvariant() + "_" + GameIO.GetNormalizedPath(fullPath);
	}

	// Token: 0x0600A565 RID: 42341 RVA: 0x003D8810 File Offset: 0x003D6A10
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string key)
	{
		return key.ToLowerInvariant();
	}

	// Token: 0x0600A566 RID: 42342 RVA: 0x003D8818 File Offset: 0x003D6A18
	public static string GetWorldEntryKey(string worldName, UserDataStorageType userStorageType)
	{
		if (string.IsNullOrEmpty(worldName))
		{
			return null;
		}
		PathAbstractions.SearchDefinition worldsSearchPaths = PathAbstractions.WorldsSearchPaths;
		UserDataStorageType? userDataStorageType = new UserDataStorageType?(userStorageType);
		PathAbstractions.AbstractedLocation location = worldsSearchPaths.GetLocation(worldName, null, userDataStorageType);
		if (location.Type == PathAbstractions.EAbstractedLocationType.None)
		{
			return SaveInfoProvider.GetWorldEntryKey(worldName);
		}
		return SaveInfoProvider.GetWorldEntryKey(worldName, location.FullPath);
	}

	// Token: 0x0600A567 RID: 42343 RVA: 0x003D886A File Offset: 0x003D6A6A
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetLocalSaveEntryKey(string worldName, string saveName, UserDataStorageType saveStorage)
	{
		return string.Format("{0}/{1}_{2}", worldName, saveName, saveStorage).ToLowerInvariant();
	}

	// Token: 0x1700136E RID: 4974
	// (get) Token: 0x0600A568 RID: 42344 RVA: 0x003D8883 File Offset: 0x003D6A83
	public IReadOnlyCollection<SaveInfoProvider.WorldEntryInfo> WorldEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.worldEntryInfos;
		}
	}

	// Token: 0x1700136F RID: 4975
	// (get) Token: 0x0600A569 RID: 42345 RVA: 0x003D8891 File Offset: 0x003D6A91
	public IReadOnlyCollection<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.saveEntryInfos;
		}
	}

	// Token: 0x17001370 RID: 4976
	// (get) Token: 0x0600A56A RID: 42346 RVA: 0x003D889F File Offset: 0x003D6A9F
	public long TotalUsedBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalUsedBytes;
		}
	}

	// Token: 0x17001371 RID: 4977
	// (get) Token: 0x0600A56B RID: 42347 RVA: 0x003D88AD File Offset: 0x003D6AAD
	public long TotalAllowanceBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes;
		}
	}

	// Token: 0x17001372 RID: 4978
	// (get) Token: 0x0600A56C RID: 42348 RVA: 0x003D88BB File Offset: 0x003D6ABB
	public long TotalAvailableBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes - this.totalUsedBytes;
		}
	}

	// Token: 0x0600A56D RID: 42349 RVA: 0x003D88D0 File Offset: 0x003D6AD0
	public void SetDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x0600A56E RID: 42350 RVA: 0x003D88DC File Offset: 0x003D6ADC
	[PublicizedFrom(EAccessModifier.Private)]
	public static long GetPlatformReservedSizeBytes(SaveDataSizes sizes)
	{
		if (PlatformManager.NativePlatform.PlatformIdentifier == EPlatformIdentifier.XBL)
		{
			long num = ((LaunchPrefs.GameCoreSaveStorageProvider.Value == SaveStorageProvider.Files) ? 32L : 16L);
			return 5242880L + num * 1048576L;
		}
		return 5242880L;
	}

	// Token: 0x0600A56F RID: 42351 RVA: 0x003D8924 File Offset: 0x003D6B24
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshIfDirty()
	{
		if (!this.isDirty)
		{
			return;
		}
		this.isDirty = false;
		this.localSavesSum = 0L;
		this.remoteSavesSum = 0L;
		this.worldsSum = 0L;
		this.worldEntryInfosByWorldKey.Clear();
		this.deletedWorlds.Clear();
		this.conflictedWorlds.Clear();
		this.saveEntryInfosBySaveKey.Clear();
		this.remoteSaveEntryInfosByGuid.Clear();
		this.worldEntryInfos.Clear();
		this.saveEntryInfos.Clear();
		this.ProcessLocalWorlds();
		this.ProcessLocalWorldSaves();
		this.ProcessRemoteWorldSaves();
		this.worldEntryInfos.AddRange(this.worldEntryInfosByWorldKey.Values);
		this.worldEntryInfos.AddRange(this.deletedWorlds.Values);
		this.worldEntryInfos.AddRange(this.conflictedWorlds.Values);
		this.totalUsedBytes = this.localSavesSum + this.remoteSavesSum + this.worldsSum;
		long num = 0L;
		this.worldEntryInfos.Sort();
		foreach (SaveInfoProvider.WorldEntryInfo worldEntryInfo in this.worldEntryInfos)
		{
			worldEntryInfo.BarStartOffset = num;
			if (worldEntryInfo.UsesDataLimit)
			{
				num += worldEntryInfo.WorldDataSize;
			}
			worldEntryInfo.SaveEntryInfos.Sort();
			foreach (SaveInfoProvider.SaveEntryInfo saveEntryInfo in worldEntryInfo.SaveEntryInfos)
			{
				saveEntryInfo.PlayerEntryInfos.Sort();
				if (saveEntryInfo.SizeInfo.UsesDataLimit)
				{
					saveEntryInfo.BarStartOffset = num;
					num += saveEntryInfo.SizeInfo.ReportedSize;
					long num2 = num;
					for (int i = saveEntryInfo.PlayerEntryInfos.Count - 1; i >= 0; i--)
					{
						SaveInfoProvider.PlayerEntryInfo playerEntryInfo = saveEntryInfo.PlayerEntryInfos[i];
						num2 -= playerEntryInfo.Size;
						playerEntryInfo.BarStartOffset = num2;
					}
				}
			}
		}
		if (SaveDataUtils.SaveDataManager.ShouldLimitSize())
		{
			SaveDataUtils.SaveDataManager.UpdateSizes();
			SaveDataSizes sizes = SaveDataUtils.SaveDataManager.GetSizes();
			long platformReservedSizeBytes = SaveInfoProvider.GetPlatformReservedSizeBytes(sizes);
			this.totalAllowanceBytes = sizes.Total - platformReservedSizeBytes;
			return;
		}
		this.totalAllowanceBytes = -1L;
	}

	// Token: 0x0600A570 RID: 42352 RVA: 0x003D8B84 File Offset: 0x003D6D84
	public void ClearResources()
	{
		this.worldEntryInfosByWorldKey.Clear();
		this.deletedWorlds.Clear();
		this.conflictedWorlds.Clear();
		this.saveEntryInfosBySaveKey.Clear();
		this.remoteSaveEntryInfosByGuid.Clear();
		this.worldEntryInfos.Clear();
		this.saveEntryInfos.Clear();
		this.protectedDirectories.Clear();
		this.isDirty = true;
	}

	// Token: 0x0600A571 RID: 42353 RVA: 0x003D8BF0 File Offset: 0x003D6DF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessLocalWorlds()
	{
		foreach (PathAbstractions.AbstractedLocation abstractedLocation in PathAbstractions.WorldsSearchPaths.GetAvailablePathsList(null, false, null))
		{
			PathAbstractions.EAbstractedLocationType type = abstractedLocation.Type;
			string text;
			if (type != PathAbstractions.EAbstractedLocationType.Mods)
			{
				if (type != PathAbstractions.EAbstractedLocationType.GameData)
				{
					text = Localization.Get("xuiDmGenerated", false, null);
				}
				else
				{
					text = Localization.Get("xuiDmBuiltIn", false, null);
				}
			}
			else
			{
				text = Localization.Get("xuiDmMod", false, null) + ": " + abstractedLocation.ContainingMod.Name;
			}
			string text2 = text;
			bool flag = GameIO.IsWorldGenerated(abstractedLocation.Name, abstractedLocation.StorageType);
			SaveInfoProvider.WorldEntryInfo worldEntryInfo = new SaveInfoProvider.WorldEntryInfo
			{
				WorldKey = SaveInfoProvider.GetWorldEntryKey(abstractedLocation.Name, abstractedLocation.FullPath),
				Name = abstractedLocation.Name,
				Type = text2,
				Location = abstractedLocation,
				Deletable = flag,
				Moveable = flag,
				WorldDataSize = GameIO.GetDirectorySize(abstractedLocation.FullPath, true),
				Version = null,
				HideIfEmpty = (!flag || SaveInfoProvider.HideableWorlds.ContainsCaseInsensitive(abstractedLocation.Name))
			};
			if (worldEntryInfo.UsesDataLimit)
			{
				this.worldsSum += worldEntryInfo.WorldDataSize;
			}
			GameUtils.WorldInfo worldInfo = GameUtils.WorldInfo.LoadWorldInfo(abstractedLocation);
			if (worldInfo != null)
			{
				worldEntryInfo.Version = worldInfo.GameVersionCreated;
				worldEntryInfo.WorldSize = new Vector2i?(worldInfo.WorldSize);
			}
			this.worldEntryInfosByWorldKey[worldEntryInfo.WorldKey] = worldEntryInfo;
		}
	}

	// Token: 0x0600A572 RID: 42354 RVA: 0x003D8DA8 File Offset: 0x003D6FA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessLocalWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessLocalWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessLocalWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A573 RID: 42355 RVA: 0x003D8DCC File Offset: 0x003D6FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessLocalWorldSaves(UserDataStorageType storageType)
	{
		string saveGameRootDir = GameIO.GetSaveGameRootDir(storageType);
		if (SdDirectory.Exists(saveGameRootDir))
		{
			SdFileSystemInfo[] array = new SdDirectoryInfo(saveGameRootDir).GetDirectories();
			foreach (SdDirectoryInfo sdDirectoryInfo in array)
			{
				if (SdDirectory.Exists(sdDirectoryInfo.FullName))
				{
					SdDirectoryInfo sdDirectoryInfo2 = new SdDirectoryInfo(sdDirectoryInfo.FullName);
					SdFileSystemInfo[] array2 = sdDirectoryInfo2.GetDirectories();
					SdFileSystemInfo[] array3 = array2;
					if (array3.Length == 0 && sdDirectoryInfo2.GetFiles().Length == 0)
					{
						SdDirectory.Delete(sdDirectoryInfo.FullName);
					}
					else
					{
						string name = sdDirectoryInfo.Name;
						foreach (SdDirectoryInfo sdDirectoryInfo3 in array3)
						{
							SaveInfoProvider.WorldEntryInfo worldEntryInfo = null;
							int num = 0;
							int num2 = 0;
							UserDataStorageType userDataStorageType = UserDataStorageType.DeviceLocal;
							foreach (SaveInfoProvider.WorldEntryInfo worldEntryInfo2 in this.worldEntryInfosByWorldKey.Values)
							{
								if (!worldEntryInfo2.Type.Equals(SaveInfoProvider.RemoteWorldsType) && worldEntryInfo2.Name.Equals(name))
								{
									num++;
									if (worldEntryInfo2.Location.Type == PathAbstractions.EAbstractedLocationType.UserDataPath)
									{
										num2++;
										userDataStorageType = worldEntryInfo2.Location.StorageType;
									}
									worldEntryInfo = worldEntryInfo2;
								}
							}
							if (num > 1)
							{
								string worldEntryKey = SaveInfoProvider.GetWorldEntryKey(name + "_" + SaveInfoProvider.ConflictedWorldsType);
								if (!this.conflictedWorlds.TryGetValue(worldEntryKey, out worldEntryInfo))
								{
									PathAbstractions.AbstractedLocation abstractedLocation = PathAbstractions.AbstractedLocation.None;
									if (num2 <= 1)
									{
										PathAbstractions.SearchDefinition worldsSearchPaths = PathAbstractions.WorldsSearchPaths;
										string text = name;
										UserDataStorageType? userDataStorageType2 = new UserDataStorageType?(userDataStorageType);
										abstractedLocation = worldsSearchPaths.GetLocation(text, null, userDataStorageType2);
									}
									worldEntryInfo = new SaveInfoProvider.WorldEntryInfo
									{
										WorldKey = worldEntryKey,
										Name = name,
										Type = SaveInfoProvider.ConflictedWorldsType,
										Location = abstractedLocation,
										Deletable = false,
										Moveable = false,
										WorldDataSize = 0L,
										Version = null,
										HideIfEmpty = true
									};
									this.conflictedWorlds.Add(worldEntryInfo.WorldKey, worldEntryInfo);
								}
							}
							if (worldEntryInfo == null)
							{
								string worldEntryKey2 = SaveInfoProvider.GetWorldEntryKey(sdDirectoryInfo.Name + "_" + SaveInfoProvider.DeletedWorldsType);
								if (!this.deletedWorlds.TryGetValue(worldEntryKey2, out worldEntryInfo))
								{
									worldEntryInfo = new SaveInfoProvider.WorldEntryInfo
									{
										WorldKey = worldEntryKey2,
										Name = sdDirectoryInfo.Name,
										Type = SaveInfoProvider.DeletedWorldsType,
										Location = PathAbstractions.AbstractedLocation.None,
										Deletable = false,
										Moveable = false,
										WorldDataSize = 0L,
										Version = null,
										HideIfEmpty = true
									};
									this.deletedWorlds.Add(worldEntryInfo.WorldKey, worldEntryInfo);
								}
							}
							this.ProcessSaveEntry(worldEntryInfo, sdDirectoryInfo3, storageType);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600A574 RID: 42356 RVA: 0x003D90A0 File Offset: 0x003D72A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessSaveEntry(SaveInfoProvider.WorldEntryInfo worldEntryInfo, SdDirectoryInfo curSaveFolder, UserDataStorageType storageType)
	{
		string text = curSaveFolder.FullName + "/main.ttw";
		SaveInfoProvider.SaveEntryInfo saveEntryInfo = new SaveInfoProvider.SaveEntryInfo
		{
			Name = curSaveFolder.Name,
			WorldEntry = worldEntryInfo,
			SaveDir = curSaveFolder.FullName,
			StorageType = storageType,
			Version = null
		};
		saveEntryInfo.SizeInfo.BytesOnDisk = GameIO.GetDirectorySize(curSaveFolder, true);
		saveEntryInfo.SizeInfo.IsArchived = SaveInfoProvider.DataLimitEnabled && SdFile.Exists(Path.Combine(curSaveFolder.FullName, "archived.flag"));
		saveEntryInfo.SizeInfo.StorageType = storageType;
		if (SdFile.Exists(text))
		{
			saveEntryInfo.LastSaved = SdFile.GetLastWriteTime(text);
			try
			{
				WorldState worldState = new WorldState();
				worldState.Load(text, false, false, false);
				saveEntryInfo.Version = worldState.gameVersion;
				saveEntryInfo.SizeInfo.BytesReserved = worldState.saveDataLimit;
				if (SaveInfoProvider.DataLimitEnabled && saveEntryInfo.SizeInfo.BytesReserved != -1L && saveEntryInfo.SizeInfo.BytesOnDisk > saveEntryInfo.SizeInfo.BytesReserved)
				{
					Debug.LogError(string.Format("Directory size of save \"{0}\" exceeds serialized save data limit of {1}.", curSaveFolder.FullName, worldState.saveDataLimit));
				}
				goto IL_0181;
			}
			catch (Exception ex)
			{
				Log.Warning("Error reading header of level '" + text + "'. Msg: " + ex.Message);
				goto IL_0181;
			}
		}
		if (curSaveFolder.Name != "WorldEditor" && curSaveFolder.Name != "PrefabEditor")
		{
			Log.Warning(string.Format("Could not find main ttw file for save in directory: {0}", curSaveFolder));
		}
		saveEntryInfo.LastSaved = curSaveFolder.LastWriteTime;
		IL_0181:
		this.saveEntryInfos.Add(saveEntryInfo);
		string localSaveEntryKey = SaveInfoProvider.GetLocalSaveEntryKey(saveEntryInfo.WorldEntry.Name, saveEntryInfo.Name, saveEntryInfo.StorageType);
		this.saveEntryInfosBySaveKey[localSaveEntryKey] = saveEntryInfo;
		worldEntryInfo.SaveEntryInfos.Add(saveEntryInfo);
		worldEntryInfo.SaveDataCount++;
		worldEntryInfo.SaveDataSizeTotal += saveEntryInfo.SizeInfo.ReportedSize;
		if (saveEntryInfo.SizeInfo.UsesDataLimit)
		{
			worldEntryInfo.SaveDataSizeForLimit += saveEntryInfo.SizeInfo.ReportedSize;
			this.localSavesSum += saveEntryInfo.SizeInfo.ReportedSize;
		}
		this.ProcessPlayerEntries(saveEntryInfo);
	}

	// Token: 0x0600A575 RID: 42357 RVA: 0x003D92EC File Offset: 0x003D74EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessPlayerEntries(SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		string text = saveEntryInfo.SaveDir + "/Player";
		if (!SdDirectory.Exists(text))
		{
			return;
		}
		this.fileNameKeysToPlayerInfos.Clear();
		foreach (SdFileInfo sdFileInfo in new SdDirectoryInfo(text).GetFiles())
		{
			int num;
			string text2;
			string text3;
			if ((num = sdFileInfo.Name.IndexOf('.')) != -1)
			{
				text2 = sdFileInfo.Name.Substring(0, num);
				int num2 = sdFileInfo.Name.LastIndexOf('.') + 1;
				text3 = sdFileInfo.Name.Substring(num2, sdFileInfo.Name.Length - num2);
			}
			else
			{
				Debug.LogError("Encountered player save file with no extension.");
				text2 = sdFileInfo.Name;
				text3 = string.Empty;
			}
			DateTime lastWriteTime = SdFile.GetLastWriteTime(sdFileInfo.FullName);
			SaveInfoProvider.PlayerEntryInfo playerEntryInfo;
			if (!this.fileNameKeysToPlayerInfos.TryGetValue(text2, out playerEntryInfo))
			{
				playerEntryInfo = new SaveInfoProvider.PlayerEntryInfo
				{
					Id = text2,
					LastPlayed = lastWriteTime,
					SaveEntry = saveEntryInfo
				};
				PlatformUserIdentifierAbs platformUserIdentifierAbs;
				if (PlatformUserIdentifierAbs.TryFromCombinedString(text2, out platformUserIdentifierAbs))
				{
					playerEntryInfo.PrimaryUserId = platformUserIdentifierAbs;
					playerEntryInfo.CachedName = platformUserIdentifierAbs.ReadablePlatformUserIdentifier;
				}
				else
				{
					Log.Error("Could not associate player save file \"" + sdFileInfo.FullName + "\" with a player id. Combined id string: " + text2);
					playerEntryInfo.CachedName = text2;
				}
				saveEntryInfo.PlayerEntryInfos.Add(playerEntryInfo);
			}
			else if (lastWriteTime > playerEntryInfo.LastPlayed)
			{
				playerEntryInfo.LastPlayed = lastWriteTime;
			}
			playerEntryInfo.Size += sdFileInfo.Length;
			PlayerMetaInfo playerMetaInfo;
			if (string.Equals(text3, "meta", StringComparison.InvariantCultureIgnoreCase) && PlayerMetaInfo.TryRead(sdFileInfo.FullName, out playerMetaInfo))
			{
				if (playerMetaInfo.NativeId != null)
				{
					playerEntryInfo.NativeUserId = playerMetaInfo.NativeId;
				}
				if (playerMetaInfo.Name != null)
				{
					playerEntryInfo.CachedName = playerMetaInfo.Name;
				}
				playerEntryInfo.PlayerLevel = playerMetaInfo.Level;
				playerEntryInfo.DistanceWalked = playerMetaInfo.DistanceWalked;
			}
			this.fileNameKeysToPlayerInfos[text2] = playerEntryInfo;
		}
		foreach (KeyValuePair<string, SaveInfoProvider.PlayerEntryInfo> keyValuePair in this.fileNameKeysToPlayerInfos)
		{
			long directorySize = GameIO.GetDirectorySize(new SdDirectoryInfo(Path.Combine(text, keyValuePair.Key)), true);
			keyValuePair.Value.Size += directorySize;
		}
		this.fileNameKeysToPlayerInfos.Clear();
	}

	// Token: 0x0600A576 RID: 42358 RVA: 0x003D9564 File Offset: 0x003D7764
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessRemoteWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessRemoteWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessRemoteWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A577 RID: 42359 RVA: 0x003D9588 File Offset: 0x003D7788
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessRemoteWorldSaves(UserDataStorageType storageType)
	{
		string saveGameLocalRootDir = GameIO.GetSaveGameLocalRootDir(storageType);
		if (!SdDirectory.Exists(saveGameLocalRootDir))
		{
			return;
		}
		SdFileSystemInfo[] array = new SdDirectoryInfo(saveGameLocalRootDir).GetDirectories();
		foreach (SdDirectoryInfo sdDirectoryInfo in array)
		{
			string text = sdDirectoryInfo.FullName + "/RemoteWorldInfo.xml";
			SaveInfoProvider.SaveEntryInfo saveEntryInfo = new SaveInfoProvider.SaveEntryInfo
			{
				SaveDir = sdDirectoryInfo.FullName,
				StorageType = storageType
			};
			saveEntryInfo.SizeInfo.BytesOnDisk = GameIO.GetDirectorySize(sdDirectoryInfo, true);
			saveEntryInfo.SizeInfo.IsArchived = SaveInfoProvider.DataLimitEnabled && SdFile.Exists(Path.Combine(sdDirectoryInfo.FullName, "archived.flag"));
			saveEntryInfo.SizeInfo.StorageType = storageType;
			PathAbstractions.AbstractedLocation abstractedLocation = PathAbstractions.Contextual.FindDownloadedRemoteWorld(sdDirectoryInfo.FullName);
			RemoteWorldInfo remoteWorldInfo;
			SaveInfoProvider.WorldEntryInfo worldEntryInfo;
			if (RemoteWorldInfo.TryRead(text, out remoteWorldInfo))
			{
				string text2 = ((abstractedLocation.Type != PathAbstractions.EAbstractedLocationType.None) ? SaveInfoProvider.GetWorldEntryKey(remoteWorldInfo.WorldName, abstractedLocation.FullPath) : SaveInfoProvider.GetWorldEntryKey(remoteWorldInfo.WorldName));
				if (!this.worldEntryInfosByWorldKey.TryGetValue(text2, out worldEntryInfo))
				{
					worldEntryInfo = new SaveInfoProvider.WorldEntryInfo
					{
						WorldKey = text2,
						Name = remoteWorldInfo.WorldName,
						Type = SaveInfoProvider.RemoteWorldsType,
						Location = abstractedLocation,
						Deletable = false,
						Moveable = false,
						WorldDataSize = 0L,
						Version = remoteWorldInfo.GameVersion
					};
					this.worldEntryInfosByWorldKey[text2] = worldEntryInfo;
				}
				saveEntryInfo.SizeInfo.BytesReserved = remoteWorldInfo.SaveSize;
				if (SaveInfoProvider.DataLimitEnabled && saveEntryInfo.SizeInfo.BytesReserved != -1L && saveEntryInfo.SizeInfo.BytesOnDisk > saveEntryInfo.SizeInfo.BytesReserved)
				{
					Debug.LogError(string.Format("Directory size of save \"{0}\" exceeds serialized save data size of {1}.", sdDirectoryInfo.FullName, remoteWorldInfo.SaveSize));
				}
				saveEntryInfo.Name = remoteWorldInfo.GameName;
				saveEntryInfo.WorldEntry = worldEntryInfo;
				saveEntryInfo.LastSaved = SdFile.GetLastWriteTime(text);
				saveEntryInfo.Version = remoteWorldInfo.GameVersion;
			}
			else
			{
				string worldEntryKey = SaveInfoProvider.GetWorldEntryKey(SaveInfoProvider.RemoteWorldsLabel);
				if (!this.worldEntryInfosByWorldKey.TryGetValue(worldEntryKey, out worldEntryInfo))
				{
					worldEntryInfo = new SaveInfoProvider.WorldEntryInfo
					{
						WorldKey = worldEntryKey,
						Name = SaveInfoProvider.RemoteWorldsLabel,
						Type = SaveInfoProvider.RemoteWorldsType,
						Location = abstractedLocation,
						Deletable = false,
						Moveable = false,
						WorldDataSize = 0L,
						Version = null
					};
					this.worldEntryInfosByWorldKey[worldEntryKey] = worldEntryInfo;
				}
				saveEntryInfo.Name = sdDirectoryInfo.Name;
				saveEntryInfo.WorldEntry = worldEntryInfo;
				saveEntryInfo.LastSaved = sdDirectoryInfo.LastWriteTime;
				saveEntryInfo.Version = null;
			}
			this.saveEntryInfos.Add(saveEntryInfo);
			this.remoteSaveEntryInfosByGuid[sdDirectoryInfo.Name] = saveEntryInfo;
			worldEntryInfo.SaveEntryInfos.Add(saveEntryInfo);
			worldEntryInfo.SaveDataCount++;
			worldEntryInfo.SaveDataSizeTotal += saveEntryInfo.SizeInfo.ReportedSize;
			if (saveEntryInfo.SizeInfo.UsesDataLimit)
			{
				worldEntryInfo.SaveDataSizeForLimit += saveEntryInfo.SizeInfo.ReportedSize;
				this.remoteSavesSum += saveEntryInfo.SizeInfo.ReportedSize;
			}
		}
	}

	// Token: 0x0600A578 RID: 42360 RVA: 0x003D98D8 File Offset: 0x003D7AD8
	public bool TryGetLocalSaveEntry(string worldName, string saveName, UserDataStorageType storage, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		string localSaveEntryKey = SaveInfoProvider.GetLocalSaveEntryKey(worldName, saveName, storage);
		return this.saveEntryInfosBySaveKey.TryGetValue(localSaveEntryKey, out saveEntryInfo);
	}

	// Token: 0x0600A579 RID: 42361 RVA: 0x003D9902 File Offset: 0x003D7B02
	public bool TryGetRemoteSaveEntry(string guid, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		return this.remoteSaveEntryInfosByGuid.TryGetValue(guid, out saveEntryInfo);
	}

	// Token: 0x0600A57A RID: 42362 RVA: 0x003D9917 File Offset: 0x003D7B17
	[PublicizedFrom(EAccessModifier.Private)]
	public string NormalizePath(string path)
	{
		return Path.GetFullPath(path);
	}

	// Token: 0x0600A57B RID: 42363 RVA: 0x003D991F File Offset: 0x003D7B1F
	public void SetDirectoryProtected(string path, bool isProtected)
	{
		if (isProtected)
		{
			this.protectedDirectories.Add(this.NormalizePath(path));
			return;
		}
		this.protectedDirectories.Remove(this.NormalizePath(path));
	}

	// Token: 0x0600A57C RID: 42364 RVA: 0x003D994B File Offset: 0x003D7B4B
	public bool IsDirectoryProtected(string path)
	{
		return this.protectedDirectories.Contains(this.NormalizePath(path));
	}

	// Token: 0x04007B2C RID: 31532
	[PublicizedFrom(EAccessModifier.Private)]
	public const long BytesPerMB = 1048576L;

	// Token: 0x04007B2D RID: 31533
	public static readonly string RemoteWorldsLabel = "[" + Localization.Get("xuiDmRemoteWorlds", false, null) + "] ";

	// Token: 0x04007B2E RID: 31534
	public static readonly string RemoteWorldsType = Localization.Get("xuiDmRemote", false, null);

	// Token: 0x04007B2F RID: 31535
	public static readonly string DeletedWorldsType = Localization.Get("xuiDmDeleted", false, null);

	// Token: 0x04007B30 RID: 31536
	public static readonly string ConflictedWorldsType = Localization.Get("xuiDmConflicted", false, null);

	// Token: 0x04007B31 RID: 31537
	public static readonly List<string> HideableWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x04007B32 RID: 31538
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveInfoProvider instance;

	// Token: 0x04007B33 RID: 31539
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> worldEntryInfosByWorldKey = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007B34 RID: 31540
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> deletedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007B35 RID: 31541
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> conflictedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007B36 RID: 31542
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> saveEntryInfosBySaveKey = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007B37 RID: 31543
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> remoteSaveEntryInfosByGuid = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007B38 RID: 31544
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.WorldEntryInfo> worldEntryInfos = new List<SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007B39 RID: 31545
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.SaveEntryInfo> saveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007B3A RID: 31546
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<string> protectedDirectories = new HashSet<string>();

	// Token: 0x04007B3B RID: 31547
	[PublicizedFrom(EAccessModifier.Private)]
	public long localSavesSum;

	// Token: 0x04007B3C RID: 31548
	[PublicizedFrom(EAccessModifier.Private)]
	public long remoteSavesSum;

	// Token: 0x04007B3D RID: 31549
	[PublicizedFrom(EAccessModifier.Private)]
	public long worldsSum;

	// Token: 0x04007B3E RID: 31550
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalUsedBytes;

	// Token: 0x04007B3F RID: 31551
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalAllowanceBytes;

	// Token: 0x04007B40 RID: 31552
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty = true;

	// Token: 0x04007B41 RID: 31553
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.PlayerEntryInfo> fileNameKeysToPlayerInfos = new Dictionary<string, SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x02001499 RID: 5273
	public struct SaveSizeInfo
	{
		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x0600A57F RID: 42367 RVA: 0x003D9A60 File Offset: 0x003D7C60
		public long ReportedSize
		{
			get
			{
				if (!this.IsArchived && this.UsesDataLimit)
				{
					return this.MaxSize;
				}
				return this.BytesOnDisk;
			}
		}

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x0600A580 RID: 42368 RVA: 0x003D9A7F File Offset: 0x003D7C7F
		public long MaxSize
		{
			get
			{
				return Math.Max(this.BytesOnDisk, this.BytesReserved);
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x0600A581 RID: 42369 RVA: 0x003D9A92 File Offset: 0x003D7C92
		public bool UsesDataLimit
		{
			get
			{
				return this.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x0600A582 RID: 42370 RVA: 0x003D9A9F File Offset: 0x003D7C9F
		public bool Archivable
		{
			get
			{
				return this.UsesDataLimit && this.BytesReserved >= this.BytesOnDisk;
			}
		}

		// Token: 0x04007B42 RID: 31554
		public UserDataStorageType StorageType;

		// Token: 0x04007B43 RID: 31555
		public long BytesOnDisk;

		// Token: 0x04007B44 RID: 31556
		public long BytesReserved;

		// Token: 0x04007B45 RID: 31557
		public bool IsArchived;
	}

	// Token: 0x0200149A RID: 5274
	public class WorldEntryInfo : IComparable
	{
		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x0600A583 RID: 42371 RVA: 0x003D9ABC File Offset: 0x003D7CBC
		public bool UsesDataLimit
		{
			get
			{
				return this.Location.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x0600A584 RID: 42372 RVA: 0x003D9AD0 File Offset: 0x003D7CD0
		public string DisplayName
		{
			get
			{
				if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.Location.Type == PathAbstractions.EAbstractedLocationType.UserDataPath)
				{
					return this.Name + " [808080][i](" + this.Location.StorageType.LocalizedName() + ")[/i][-]";
				}
				return this.Name;
			}
		}

		// Token: 0x0600A585 RID: 42373 RVA: 0x003D9B28 File Offset: 0x003D7D28
		public bool ShouldBeMovedWithSave(UserDataStorageType targetSaveStorage)
		{
			return targetSaveStorage.UsesDataLimit() && this.Moveable && targetSaveStorage != this.Location.StorageType;
		}

		// Token: 0x0600A586 RID: 42374 RVA: 0x003D9B50 File Offset: 0x003D7D50
		public int CompareTo(object obj)
		{
			SaveInfoProvider.WorldEntryInfo worldEntryInfo = obj as SaveInfoProvider.WorldEntryInfo;
			if (worldEntryInfo != null)
			{
				return string.Compare(this.WorldKey, worldEntryInfo.WorldKey, StringComparison.OrdinalIgnoreCase);
			}
			return 1;
		}

		// Token: 0x04007B46 RID: 31558
		public string WorldKey;

		// Token: 0x04007B47 RID: 31559
		public string Name;

		// Token: 0x04007B48 RID: 31560
		public string Type;

		// Token: 0x04007B49 RID: 31561
		public PathAbstractions.AbstractedLocation Location;

		// Token: 0x04007B4A RID: 31562
		public bool Deletable;

		// Token: 0x04007B4B RID: 31563
		public bool Moveable;

		// Token: 0x04007B4C RID: 31564
		public long WorldDataSize;

		// Token: 0x04007B4D RID: 31565
		public VersionInformation Version;

		// Token: 0x04007B4E RID: 31566
		public Vector2i? WorldSize;

		// Token: 0x04007B4F RID: 31567
		public long SaveDataSizeForLimit;

		// Token: 0x04007B50 RID: 31568
		public long SaveDataSizeTotal;

		// Token: 0x04007B51 RID: 31569
		public int SaveDataCount;

		// Token: 0x04007B52 RID: 31570
		public long BarStartOffset;

		// Token: 0x04007B53 RID: 31571
		public bool HideIfEmpty;

		// Token: 0x04007B54 RID: 31572
		public readonly List<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();
	}

	// Token: 0x0200149B RID: 5275
	public class SaveEntryInfo : IComparable
	{
		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x0600A588 RID: 42376 RVA: 0x003D9B8E File Offset: 0x003D7D8E
		public string DisplayName
		{
			get
			{
				if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional)
				{
					return this.Name + " [808080][i](" + this.StorageType.LocalizedName() + ")[/i][-]";
				}
				return this.Name;
			}
		}

		// Token: 0x0600A589 RID: 42377 RVA: 0x003D9BC8 File Offset: 0x003D7DC8
		public int CompareTo(object obj)
		{
			SaveInfoProvider.SaveEntryInfo saveEntryInfo = obj as SaveInfoProvider.SaveEntryInfo;
			if (saveEntryInfo == null)
			{
				return 1;
			}
			int num = saveEntryInfo.LastSaved.CompareTo(this.LastSaved);
			if (num == 0)
			{
				return saveEntryInfo.Name.CompareTo(this.Name);
			}
			return num;
		}

		// Token: 0x04007B55 RID: 31573
		public string Name;

		// Token: 0x04007B56 RID: 31574
		public UserDataStorageType StorageType;

		// Token: 0x04007B57 RID: 31575
		public string SaveDir;

		// Token: 0x04007B58 RID: 31576
		public SaveInfoProvider.SaveSizeInfo SizeInfo;

		// Token: 0x04007B59 RID: 31577
		public long BarStartOffset;

		// Token: 0x04007B5A RID: 31578
		public SaveInfoProvider.WorldEntryInfo WorldEntry;

		// Token: 0x04007B5B RID: 31579
		public DateTime LastSaved;

		// Token: 0x04007B5C RID: 31580
		public VersionInformation Version;

		// Token: 0x04007B5D RID: 31581
		public readonly List<SaveInfoProvider.PlayerEntryInfo> PlayerEntryInfos = new List<SaveInfoProvider.PlayerEntryInfo>();
	}

	// Token: 0x0200149C RID: 5276
	public class PlayerEntryInfo : IComparable
	{
		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x0600A58B RID: 42379 RVA: 0x003D9C1C File Offset: 0x003D7E1C
		public string PlatformName
		{
			get
			{
				PlatformUserIdentifierAbs platformUserIdentifierAbs = this.NativeUserId ?? this.PrimaryUserId;
				return ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.PlatformIdentifierString : null) ?? "-";
			}
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x0600A58C RID: 42380 RVA: 0x003D9C44 File Offset: 0x003D7E44
		public IPlatformUserData PlatformUserData
		{
			get
			{
				if (this.platformUserData == null && this.PrimaryUserId != null)
				{
					IPlatformUserData orCreate = PlatformUserManager.GetOrCreate(this.PrimaryUserId);
					if (this.NativeUserId != null)
					{
						orCreate.NativeId = this.NativeUserId;
					}
					this.platformUserData = orCreate;
				}
				return this.platformUserData;
			}
		}

		// Token: 0x0600A58D RID: 42381 RVA: 0x003D9C90 File Offset: 0x003D7E90
		public int CompareTo(object obj)
		{
			SaveInfoProvider.PlayerEntryInfo playerEntryInfo = obj as SaveInfoProvider.PlayerEntryInfo;
			if (playerEntryInfo == null)
			{
				return 1;
			}
			int num = playerEntryInfo.LastPlayed.CompareTo(this.LastPlayed);
			if (num == 0)
			{
				return playerEntryInfo.CachedName.CompareTo(this.CachedName);
			}
			return num;
		}

		// Token: 0x04007B5E RID: 31582
		public string Id;

		// Token: 0x04007B5F RID: 31583
		public string CachedName;

		// Token: 0x04007B60 RID: 31584
		public PlatformUserIdentifierAbs PrimaryUserId;

		// Token: 0x04007B61 RID: 31585
		public PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x04007B62 RID: 31586
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformUserData platformUserData;

		// Token: 0x04007B63 RID: 31587
		public long Size;

		// Token: 0x04007B64 RID: 31588
		public long BarStartOffset;

		// Token: 0x04007B65 RID: 31589
		public SaveInfoProvider.SaveEntryInfo SaveEntry;

		// Token: 0x04007B66 RID: 31590
		public DateTime LastPlayed;

		// Token: 0x04007B67 RID: 31591
		public int PlayerLevel;

		// Token: 0x04007B68 RID: 31592
		public float DistanceWalked;
	}

	// Token: 0x0200149D RID: 5277
	public class PlayerEntryInfoPlatformDataResolver
	{
		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x0600A58F RID: 42383 RVA: 0x003D9CD1 File Offset: 0x003D7ED1
		// (set) Token: 0x0600A590 RID: 42384 RVA: 0x003D9CD9 File Offset: 0x003D7ED9
		public bool IsComplete
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600A591 RID: 42385 RVA: 0x003D9CE2 File Offset: 0x003D7EE2
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerEntryInfoPlatformDataResolver(List<SaveInfoProvider.PlayerEntryInfo> playerEntries)
		{
			this.pendingPlayerEntries = playerEntries;
		}

		// Token: 0x0600A592 RID: 42386 RVA: 0x003D9CF4 File Offset: 0x003D7EF4
		public static SaveInfoProvider.PlayerEntryInfoPlatformDataResolver StartNew(IEnumerable<SaveInfoProvider.PlayerEntryInfo> playerEntries)
		{
			List<SaveInfoProvider.PlayerEntryInfo> list = new List<SaveInfoProvider.PlayerEntryInfo>();
			List<IPlatformUserData> list2 = null;
			foreach (SaveInfoProvider.PlayerEntryInfo playerEntryInfo in playerEntries)
			{
				list.Add(playerEntryInfo);
				IPlatformUserData platformUserData = playerEntryInfo.PlatformUserData;
				if (platformUserData != null)
				{
					if (list2 == null)
					{
						list2 = new List<IPlatformUserData>();
					}
					list2.Add(platformUserData);
				}
			}
			SaveInfoProvider.PlayerEntryInfoPlatformDataResolver playerEntryInfoPlatformDataResolver = new SaveInfoProvider.PlayerEntryInfoPlatformDataResolver(list);
			if (list2 == null)
			{
				playerEntryInfoPlatformDataResolver.IsComplete = true;
				return playerEntryInfoPlatformDataResolver;
			}
			if (!PlatformUserManager.AreUsersPendingResolve(list2))
			{
				playerEntryInfoPlatformDataResolver.IsComplete = true;
				return playerEntryInfoPlatformDataResolver;
			}
			ThreadManager.StartCoroutine(playerEntryInfoPlatformDataResolver.ResolveUserData(list2));
			return playerEntryInfoPlatformDataResolver;
		}

		// Token: 0x0600A593 RID: 42387 RVA: 0x003D9D98 File Offset: 0x003D7F98
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator ResolveUserData(List<IPlatformUserData> resolvingPlatformData)
		{
			yield return PlatformUserManager.ResolveUsersDetailsCoroutine(resolvingPlatformData);
			yield return PlatformUserManager.ResolveUserBlocksCoroutine(resolvingPlatformData);
			this.IsComplete = true;
			yield break;
		}

		// Token: 0x04007B69 RID: 31593
		public readonly List<SaveInfoProvider.PlayerEntryInfo> pendingPlayerEntries;
	}
}
