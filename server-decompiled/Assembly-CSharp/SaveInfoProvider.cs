using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Platform;
using Platform.XBL.Save.Storage;
using UnityEngine;

// Token: 0x020014B8 RID: 5304
public class SaveInfoProvider
{
	// Token: 0x170013B1 RID: 5041
	// (get) Token: 0x0600A610 RID: 42512 RVA: 0x003DD460 File Offset: 0x003DB660
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

	// Token: 0x170013B2 RID: 5042
	// (get) Token: 0x0600A611 RID: 42513 RVA: 0x003DD478 File Offset: 0x003DB678
	public static bool DataLimitEnabled
	{
		get
		{
			return SaveDataUtils.SaveDataManager.ShouldLimitSize();
		}
	}

	// Token: 0x0600A612 RID: 42514 RVA: 0x003DD484 File Offset: 0x003DB684
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string worldName, string fullPath)
	{
		return worldName.ToLowerInvariant() + "_" + GameIO.GetNormalizedPath(fullPath);
	}

	// Token: 0x0600A613 RID: 42515 RVA: 0x003DD49C File Offset: 0x003DB69C
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string key)
	{
		return key.ToLowerInvariant();
	}

	// Token: 0x0600A614 RID: 42516 RVA: 0x003DD4A4 File Offset: 0x003DB6A4
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

	// Token: 0x0600A615 RID: 42517 RVA: 0x003DD4F6 File Offset: 0x003DB6F6
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetLocalSaveEntryKey(string worldName, string saveName, UserDataStorageType saveStorage)
	{
		return string.Format("{0}/{1}_{2}", worldName, saveName, saveStorage).ToLowerInvariant();
	}

	// Token: 0x170013B3 RID: 5043
	// (get) Token: 0x0600A616 RID: 42518 RVA: 0x003DD50F File Offset: 0x003DB70F
	public IReadOnlyCollection<SaveInfoProvider.WorldEntryInfo> WorldEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.worldEntryInfos;
		}
	}

	// Token: 0x170013B4 RID: 5044
	// (get) Token: 0x0600A617 RID: 42519 RVA: 0x003DD51D File Offset: 0x003DB71D
	public IReadOnlyCollection<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.saveEntryInfos;
		}
	}

	// Token: 0x170013B5 RID: 5045
	// (get) Token: 0x0600A618 RID: 42520 RVA: 0x003DD52B File Offset: 0x003DB72B
	public long TotalUsedBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalUsedBytes;
		}
	}

	// Token: 0x170013B6 RID: 5046
	// (get) Token: 0x0600A619 RID: 42521 RVA: 0x003DD539 File Offset: 0x003DB739
	public long TotalAllowanceBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes;
		}
	}

	// Token: 0x170013B7 RID: 5047
	// (get) Token: 0x0600A61A RID: 42522 RVA: 0x003DD547 File Offset: 0x003DB747
	public long TotalAvailableBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes - this.totalUsedBytes;
		}
	}

	// Token: 0x0600A61B RID: 42523 RVA: 0x003DD55C File Offset: 0x003DB75C
	public void SetDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x0600A61C RID: 42524 RVA: 0x003DD568 File Offset: 0x003DB768
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

	// Token: 0x0600A61D RID: 42525 RVA: 0x003DD5B0 File Offset: 0x003DB7B0
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

	// Token: 0x0600A61E RID: 42526 RVA: 0x003DD810 File Offset: 0x003DBA10
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

	// Token: 0x0600A61F RID: 42527 RVA: 0x003DD87C File Offset: 0x003DBA7C
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

	// Token: 0x0600A620 RID: 42528 RVA: 0x003DDA34 File Offset: 0x003DBC34
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessLocalWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessLocalWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessLocalWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A621 RID: 42529 RVA: 0x003DDA58 File Offset: 0x003DBC58
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

	// Token: 0x0600A622 RID: 42530 RVA: 0x003DDD2C File Offset: 0x003DBF2C
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

	// Token: 0x0600A623 RID: 42531 RVA: 0x003DDF78 File Offset: 0x003DC178
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

	// Token: 0x0600A624 RID: 42532 RVA: 0x003DE1F0 File Offset: 0x003DC3F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessRemoteWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessRemoteWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessRemoteWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A625 RID: 42533 RVA: 0x003DE214 File Offset: 0x003DC414
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

	// Token: 0x0600A626 RID: 42534 RVA: 0x003DE564 File Offset: 0x003DC764
	public bool TryGetLocalSaveEntry(string worldName, string saveName, UserDataStorageType storage, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		string localSaveEntryKey = SaveInfoProvider.GetLocalSaveEntryKey(worldName, saveName, storage);
		return this.saveEntryInfosBySaveKey.TryGetValue(localSaveEntryKey, out saveEntryInfo);
	}

	// Token: 0x0600A627 RID: 42535 RVA: 0x003DE58E File Offset: 0x003DC78E
	public bool TryGetRemoteSaveEntry(string guid, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		return this.remoteSaveEntryInfosByGuid.TryGetValue(guid, out saveEntryInfo);
	}

	// Token: 0x0600A628 RID: 42536 RVA: 0x003DE5A3 File Offset: 0x003DC7A3
	[PublicizedFrom(EAccessModifier.Private)]
	public string NormalizePath(string path)
	{
		return Path.GetFullPath(path);
	}

	// Token: 0x0600A629 RID: 42537 RVA: 0x003DE5AB File Offset: 0x003DC7AB
	public void SetDirectoryProtected(string path, bool isProtected)
	{
		if (isProtected)
		{
			this.protectedDirectories.Add(this.NormalizePath(path));
			return;
		}
		this.protectedDirectories.Remove(this.NormalizePath(path));
	}

	// Token: 0x0600A62A RID: 42538 RVA: 0x003DE5D7 File Offset: 0x003DC7D7
	public bool IsDirectoryProtected(string path)
	{
		return this.protectedDirectories.Contains(this.NormalizePath(path));
	}

	// Token: 0x04007BA7 RID: 31655
	[PublicizedFrom(EAccessModifier.Private)]
	public const long BytesPerMB = 1048576L;

	// Token: 0x04007BA8 RID: 31656
	public static readonly string RemoteWorldsLabel = "[" + Localization.Get("xuiDmRemoteWorlds", false, null) + "] ";

	// Token: 0x04007BA9 RID: 31657
	public static readonly string RemoteWorldsType = Localization.Get("xuiDmRemote", false, null);

	// Token: 0x04007BAA RID: 31658
	public static readonly string DeletedWorldsType = Localization.Get("xuiDmDeleted", false, null);

	// Token: 0x04007BAB RID: 31659
	public static readonly string ConflictedWorldsType = Localization.Get("xuiDmConflicted", false, null);

	// Token: 0x04007BAC RID: 31660
	public static readonly List<string> HideableWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x04007BAD RID: 31661
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveInfoProvider instance;

	// Token: 0x04007BAE RID: 31662
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> worldEntryInfosByWorldKey = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BAF RID: 31663
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> deletedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BB0 RID: 31664
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> conflictedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BB1 RID: 31665
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> saveEntryInfosBySaveKey = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007BB2 RID: 31666
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> remoteSaveEntryInfosByGuid = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007BB3 RID: 31667
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.WorldEntryInfo> worldEntryInfos = new List<SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BB4 RID: 31668
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.SaveEntryInfo> saveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007BB5 RID: 31669
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<string> protectedDirectories = new HashSet<string>();

	// Token: 0x04007BB6 RID: 31670
	[PublicizedFrom(EAccessModifier.Private)]
	public long localSavesSum;

	// Token: 0x04007BB7 RID: 31671
	[PublicizedFrom(EAccessModifier.Private)]
	public long remoteSavesSum;

	// Token: 0x04007BB8 RID: 31672
	[PublicizedFrom(EAccessModifier.Private)]
	public long worldsSum;

	// Token: 0x04007BB9 RID: 31673
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalUsedBytes;

	// Token: 0x04007BBA RID: 31674
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalAllowanceBytes;

	// Token: 0x04007BBB RID: 31675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty = true;

	// Token: 0x04007BBC RID: 31676
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.PlayerEntryInfo> fileNameKeysToPlayerInfos = new Dictionary<string, SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x020014B9 RID: 5305
	public struct SaveSizeInfo
	{
		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x0600A62D RID: 42541 RVA: 0x003DE6EC File Offset: 0x003DC8EC
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

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x0600A62E RID: 42542 RVA: 0x003DE70B File Offset: 0x003DC90B
		public long MaxSize
		{
			get
			{
				return Math.Max(this.BytesOnDisk, this.BytesReserved);
			}
		}

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x0600A62F RID: 42543 RVA: 0x003DE71E File Offset: 0x003DC91E
		public bool UsesDataLimit
		{
			get
			{
				return this.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x0600A630 RID: 42544 RVA: 0x003DE72B File Offset: 0x003DC92B
		public bool Archivable
		{
			get
			{
				return this.UsesDataLimit && this.BytesReserved >= this.BytesOnDisk;
			}
		}

		// Token: 0x04007BBD RID: 31677
		public UserDataStorageType StorageType;

		// Token: 0x04007BBE RID: 31678
		public long BytesOnDisk;

		// Token: 0x04007BBF RID: 31679
		public long BytesReserved;

		// Token: 0x04007BC0 RID: 31680
		public bool IsArchived;
	}

	// Token: 0x020014BA RID: 5306
	public class WorldEntryInfo : IComparable
	{
		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x0600A631 RID: 42545 RVA: 0x003DE748 File Offset: 0x003DC948
		public bool UsesDataLimit
		{
			get
			{
				return this.Location.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x0600A632 RID: 42546 RVA: 0x003DE75C File Offset: 0x003DC95C
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

		// Token: 0x0600A633 RID: 42547 RVA: 0x003DE7B4 File Offset: 0x003DC9B4
		public bool ShouldBeMovedWithSave(UserDataStorageType targetSaveStorage)
		{
			return targetSaveStorage.UsesDataLimit() && this.Moveable && targetSaveStorage != this.Location.StorageType;
		}

		// Token: 0x0600A634 RID: 42548 RVA: 0x003DE7DC File Offset: 0x003DC9DC
		public int CompareTo(object obj)
		{
			SaveInfoProvider.WorldEntryInfo worldEntryInfo = obj as SaveInfoProvider.WorldEntryInfo;
			if (worldEntryInfo != null)
			{
				return string.Compare(this.WorldKey, worldEntryInfo.WorldKey, StringComparison.OrdinalIgnoreCase);
			}
			return 1;
		}

		// Token: 0x04007BC1 RID: 31681
		public string WorldKey;

		// Token: 0x04007BC2 RID: 31682
		public string Name;

		// Token: 0x04007BC3 RID: 31683
		public string Type;

		// Token: 0x04007BC4 RID: 31684
		public PathAbstractions.AbstractedLocation Location;

		// Token: 0x04007BC5 RID: 31685
		public bool Deletable;

		// Token: 0x04007BC6 RID: 31686
		public bool Moveable;

		// Token: 0x04007BC7 RID: 31687
		public long WorldDataSize;

		// Token: 0x04007BC8 RID: 31688
		public VersionInformation Version;

		// Token: 0x04007BC9 RID: 31689
		public Vector2i? WorldSize;

		// Token: 0x04007BCA RID: 31690
		public long SaveDataSizeForLimit;

		// Token: 0x04007BCB RID: 31691
		public long SaveDataSizeTotal;

		// Token: 0x04007BCC RID: 31692
		public int SaveDataCount;

		// Token: 0x04007BCD RID: 31693
		public long BarStartOffset;

		// Token: 0x04007BCE RID: 31694
		public bool HideIfEmpty;

		// Token: 0x04007BCF RID: 31695
		public readonly List<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();
	}

	// Token: 0x020014BB RID: 5307
	public class SaveEntryInfo : IComparable
	{
		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x0600A636 RID: 42550 RVA: 0x003DE81A File Offset: 0x003DCA1A
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

		// Token: 0x0600A637 RID: 42551 RVA: 0x003DE854 File Offset: 0x003DCA54
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

		// Token: 0x04007BD0 RID: 31696
		public string Name;

		// Token: 0x04007BD1 RID: 31697
		public UserDataStorageType StorageType;

		// Token: 0x04007BD2 RID: 31698
		public string SaveDir;

		// Token: 0x04007BD3 RID: 31699
		public SaveInfoProvider.SaveSizeInfo SizeInfo;

		// Token: 0x04007BD4 RID: 31700
		public long BarStartOffset;

		// Token: 0x04007BD5 RID: 31701
		public SaveInfoProvider.WorldEntryInfo WorldEntry;

		// Token: 0x04007BD6 RID: 31702
		public DateTime LastSaved;

		// Token: 0x04007BD7 RID: 31703
		public VersionInformation Version;

		// Token: 0x04007BD8 RID: 31704
		public readonly List<SaveInfoProvider.PlayerEntryInfo> PlayerEntryInfos = new List<SaveInfoProvider.PlayerEntryInfo>();
	}

	// Token: 0x020014BC RID: 5308
	public class PlayerEntryInfo : IComparable
	{
		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x0600A639 RID: 42553 RVA: 0x003DE8A8 File Offset: 0x003DCAA8
		public string PlatformName
		{
			get
			{
				PlatformUserIdentifierAbs platformUserIdentifierAbs = this.NativeUserId ?? this.PrimaryUserId;
				return ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.PlatformIdentifierString : null) ?? "-";
			}
		}

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x0600A63A RID: 42554 RVA: 0x003DE8D0 File Offset: 0x003DCAD0
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

		// Token: 0x0600A63B RID: 42555 RVA: 0x003DE91C File Offset: 0x003DCB1C
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

		// Token: 0x04007BD9 RID: 31705
		public string Id;

		// Token: 0x04007BDA RID: 31706
		public string CachedName;

		// Token: 0x04007BDB RID: 31707
		public PlatformUserIdentifierAbs PrimaryUserId;

		// Token: 0x04007BDC RID: 31708
		public PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x04007BDD RID: 31709
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformUserData platformUserData;

		// Token: 0x04007BDE RID: 31710
		public long Size;

		// Token: 0x04007BDF RID: 31711
		public long BarStartOffset;

		// Token: 0x04007BE0 RID: 31712
		public SaveInfoProvider.SaveEntryInfo SaveEntry;

		// Token: 0x04007BE1 RID: 31713
		public DateTime LastPlayed;

		// Token: 0x04007BE2 RID: 31714
		public int PlayerLevel;

		// Token: 0x04007BE3 RID: 31715
		public float DistanceWalked;
	}

	// Token: 0x020014BD RID: 5309
	public class PlayerEntryInfoPlatformDataResolver
	{
		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x0600A63D RID: 42557 RVA: 0x003DE95D File Offset: 0x003DCB5D
		// (set) Token: 0x0600A63E RID: 42558 RVA: 0x003DE965 File Offset: 0x003DCB65
		public bool IsComplete
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600A63F RID: 42559 RVA: 0x003DE96E File Offset: 0x003DCB6E
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerEntryInfoPlatformDataResolver(List<SaveInfoProvider.PlayerEntryInfo> playerEntries)
		{
			this.pendingPlayerEntries = playerEntries;
		}

		// Token: 0x0600A640 RID: 42560 RVA: 0x003DE980 File Offset: 0x003DCB80
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

		// Token: 0x0600A641 RID: 42561 RVA: 0x003DEA24 File Offset: 0x003DCC24
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator ResolveUserData(List<IPlatformUserData> resolvingPlatformData)
		{
			yield return PlatformUserManager.ResolveUsersDetailsCoroutine(resolvingPlatformData);
			yield return PlatformUserManager.ResolveUserBlocksCoroutine(resolvingPlatformData);
			this.IsComplete = true;
			yield break;
		}

		// Token: 0x04007BE4 RID: 31716
		public readonly List<SaveInfoProvider.PlayerEntryInfo> pendingPlayerEntries;
	}
}
