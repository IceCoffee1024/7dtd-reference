using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Platform;
using Platform.XBL.Save.Storage;
using UnityEngine;

// Token: 0x020014CA RID: 5322
public class SaveInfoProvider
{
	// Token: 0x170013BD RID: 5053
	// (get) Token: 0x0600A668 RID: 42600 RVA: 0x003DF864 File Offset: 0x003DDA64
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

	// Token: 0x170013BE RID: 5054
	// (get) Token: 0x0600A669 RID: 42601 RVA: 0x003DF87C File Offset: 0x003DDA7C
	public static bool DataLimitEnabled
	{
		get
		{
			return SaveDataUtils.SaveDataManager.ShouldLimitSize();
		}
	}

	// Token: 0x0600A66A RID: 42602 RVA: 0x003DF888 File Offset: 0x003DDA88
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string worldName, string fullPath)
	{
		return worldName.ToLowerInvariant() + "_" + GameIO.GetNormalizedPath(fullPath);
	}

	// Token: 0x0600A66B RID: 42603 RVA: 0x003DF8A0 File Offset: 0x003DDAA0
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string key)
	{
		return key.ToLowerInvariant();
	}

	// Token: 0x0600A66C RID: 42604 RVA: 0x003DF8A8 File Offset: 0x003DDAA8
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

	// Token: 0x0600A66D RID: 42605 RVA: 0x003DF8FA File Offset: 0x003DDAFA
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetLocalSaveEntryKey(string worldName, string saveName, UserDataStorageType saveStorage)
	{
		return string.Format("{0}/{1}_{2}", worldName, saveName, saveStorage).ToLowerInvariant();
	}

	// Token: 0x170013BF RID: 5055
	// (get) Token: 0x0600A66E RID: 42606 RVA: 0x003DF913 File Offset: 0x003DDB13
	public IReadOnlyCollection<SaveInfoProvider.WorldEntryInfo> WorldEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.worldEntryInfos;
		}
	}

	// Token: 0x170013C0 RID: 5056
	// (get) Token: 0x0600A66F RID: 42607 RVA: 0x003DF921 File Offset: 0x003DDB21
	public IReadOnlyCollection<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.saveEntryInfos;
		}
	}

	// Token: 0x170013C1 RID: 5057
	// (get) Token: 0x0600A670 RID: 42608 RVA: 0x003DF92F File Offset: 0x003DDB2F
	public long TotalUsedBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalUsedBytes;
		}
	}

	// Token: 0x170013C2 RID: 5058
	// (get) Token: 0x0600A671 RID: 42609 RVA: 0x003DF93D File Offset: 0x003DDB3D
	public long TotalAllowanceBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes;
		}
	}

	// Token: 0x170013C3 RID: 5059
	// (get) Token: 0x0600A672 RID: 42610 RVA: 0x003DF94B File Offset: 0x003DDB4B
	public long TotalAvailableBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes - this.totalUsedBytes;
		}
	}

	// Token: 0x0600A673 RID: 42611 RVA: 0x003DF960 File Offset: 0x003DDB60
	public void SetDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x0600A674 RID: 42612 RVA: 0x003DF96C File Offset: 0x003DDB6C
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

	// Token: 0x0600A675 RID: 42613 RVA: 0x003DF9B4 File Offset: 0x003DDBB4
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

	// Token: 0x0600A676 RID: 42614 RVA: 0x003DFC14 File Offset: 0x003DDE14
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

	// Token: 0x0600A677 RID: 42615 RVA: 0x003DFC80 File Offset: 0x003DDE80
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

	// Token: 0x0600A678 RID: 42616 RVA: 0x003DFE38 File Offset: 0x003DE038
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessLocalWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessLocalWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessLocalWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A679 RID: 42617 RVA: 0x003DFE5C File Offset: 0x003DE05C
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

	// Token: 0x0600A67A RID: 42618 RVA: 0x003E0130 File Offset: 0x003DE330
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

	// Token: 0x0600A67B RID: 42619 RVA: 0x003E037C File Offset: 0x003DE57C
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

	// Token: 0x0600A67C RID: 42620 RVA: 0x003E05F4 File Offset: 0x003DE7F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessRemoteWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessRemoteWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessRemoteWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A67D RID: 42621 RVA: 0x003E0618 File Offset: 0x003DE818
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

	// Token: 0x0600A67E RID: 42622 RVA: 0x003E0968 File Offset: 0x003DEB68
	public bool TryGetLocalSaveEntry(string worldName, string saveName, UserDataStorageType storage, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		string localSaveEntryKey = SaveInfoProvider.GetLocalSaveEntryKey(worldName, saveName, storage);
		return this.saveEntryInfosBySaveKey.TryGetValue(localSaveEntryKey, out saveEntryInfo);
	}

	// Token: 0x0600A67F RID: 42623 RVA: 0x003E0992 File Offset: 0x003DEB92
	public bool TryGetRemoteSaveEntry(string guid, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		return this.remoteSaveEntryInfosByGuid.TryGetValue(guid, out saveEntryInfo);
	}

	// Token: 0x0600A680 RID: 42624 RVA: 0x003E09A7 File Offset: 0x003DEBA7
	[PublicizedFrom(EAccessModifier.Private)]
	public string NormalizePath(string path)
	{
		return Path.GetFullPath(path);
	}

	// Token: 0x0600A681 RID: 42625 RVA: 0x003E09AF File Offset: 0x003DEBAF
	public void SetDirectoryProtected(string path, bool isProtected)
	{
		if (isProtected)
		{
			this.protectedDirectories.Add(this.NormalizePath(path));
			return;
		}
		this.protectedDirectories.Remove(this.NormalizePath(path));
	}

	// Token: 0x0600A682 RID: 42626 RVA: 0x003E09DB File Offset: 0x003DEBDB
	public bool IsDirectoryProtected(string path)
	{
		return this.protectedDirectories.Contains(this.NormalizePath(path));
	}

	// Token: 0x04007BE8 RID: 31720
	[PublicizedFrom(EAccessModifier.Private)]
	public const long BytesPerMB = 1048576L;

	// Token: 0x04007BE9 RID: 31721
	public static readonly string RemoteWorldsLabel = "[" + Localization.Get("xuiDmRemoteWorlds", false, null) + "] ";

	// Token: 0x04007BEA RID: 31722
	public static readonly string RemoteWorldsType = Localization.Get("xuiDmRemote", false, null);

	// Token: 0x04007BEB RID: 31723
	public static readonly string DeletedWorldsType = Localization.Get("xuiDmDeleted", false, null);

	// Token: 0x04007BEC RID: 31724
	public static readonly string ConflictedWorldsType = Localization.Get("xuiDmConflicted", false, null);

	// Token: 0x04007BED RID: 31725
	public static readonly List<string> HideableWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x04007BEE RID: 31726
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveInfoProvider instance;

	// Token: 0x04007BEF RID: 31727
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> worldEntryInfosByWorldKey = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BF0 RID: 31728
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> deletedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BF1 RID: 31729
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> conflictedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BF2 RID: 31730
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> saveEntryInfosBySaveKey = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007BF3 RID: 31731
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> remoteSaveEntryInfosByGuid = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007BF4 RID: 31732
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.WorldEntryInfo> worldEntryInfos = new List<SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007BF5 RID: 31733
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.SaveEntryInfo> saveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007BF6 RID: 31734
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<string> protectedDirectories = new HashSet<string>();

	// Token: 0x04007BF7 RID: 31735
	[PublicizedFrom(EAccessModifier.Private)]
	public long localSavesSum;

	// Token: 0x04007BF8 RID: 31736
	[PublicizedFrom(EAccessModifier.Private)]
	public long remoteSavesSum;

	// Token: 0x04007BF9 RID: 31737
	[PublicizedFrom(EAccessModifier.Private)]
	public long worldsSum;

	// Token: 0x04007BFA RID: 31738
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalUsedBytes;

	// Token: 0x04007BFB RID: 31739
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalAllowanceBytes;

	// Token: 0x04007BFC RID: 31740
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty = true;

	// Token: 0x04007BFD RID: 31741
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.PlayerEntryInfo> fileNameKeysToPlayerInfos = new Dictionary<string, SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x020014CB RID: 5323
	public struct SaveSizeInfo
	{
		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x0600A685 RID: 42629 RVA: 0x003E0AF0 File Offset: 0x003DECF0
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

		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x0600A686 RID: 42630 RVA: 0x003E0B0F File Offset: 0x003DED0F
		public long MaxSize
		{
			get
			{
				return Math.Max(this.BytesOnDisk, this.BytesReserved);
			}
		}

		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x0600A687 RID: 42631 RVA: 0x003E0B22 File Offset: 0x003DED22
		public bool UsesDataLimit
		{
			get
			{
				return this.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x0600A688 RID: 42632 RVA: 0x003E0B2F File Offset: 0x003DED2F
		public bool Archivable
		{
			get
			{
				return this.UsesDataLimit && this.BytesReserved >= this.BytesOnDisk;
			}
		}

		// Token: 0x04007BFE RID: 31742
		public UserDataStorageType StorageType;

		// Token: 0x04007BFF RID: 31743
		public long BytesOnDisk;

		// Token: 0x04007C00 RID: 31744
		public long BytesReserved;

		// Token: 0x04007C01 RID: 31745
		public bool IsArchived;
	}

	// Token: 0x020014CC RID: 5324
	public class WorldEntryInfo : IComparable
	{
		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x0600A689 RID: 42633 RVA: 0x003E0B4C File Offset: 0x003DED4C
		public bool UsesDataLimit
		{
			get
			{
				return this.Location.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x0600A68A RID: 42634 RVA: 0x003E0B60 File Offset: 0x003DED60
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

		// Token: 0x0600A68B RID: 42635 RVA: 0x003E0BB8 File Offset: 0x003DEDB8
		public bool ShouldBeMovedWithSave(UserDataStorageType targetSaveStorage)
		{
			return targetSaveStorage.UsesDataLimit() && this.Moveable && targetSaveStorage != this.Location.StorageType;
		}

		// Token: 0x0600A68C RID: 42636 RVA: 0x003E0BE0 File Offset: 0x003DEDE0
		public int CompareTo(object obj)
		{
			SaveInfoProvider.WorldEntryInfo worldEntryInfo = obj as SaveInfoProvider.WorldEntryInfo;
			if (worldEntryInfo != null)
			{
				return string.Compare(this.WorldKey, worldEntryInfo.WorldKey, StringComparison.OrdinalIgnoreCase);
			}
			return 1;
		}

		// Token: 0x04007C02 RID: 31746
		public string WorldKey;

		// Token: 0x04007C03 RID: 31747
		public string Name;

		// Token: 0x04007C04 RID: 31748
		public string Type;

		// Token: 0x04007C05 RID: 31749
		public PathAbstractions.AbstractedLocation Location;

		// Token: 0x04007C06 RID: 31750
		public bool Deletable;

		// Token: 0x04007C07 RID: 31751
		public bool Moveable;

		// Token: 0x04007C08 RID: 31752
		public long WorldDataSize;

		// Token: 0x04007C09 RID: 31753
		public VersionInformation Version;

		// Token: 0x04007C0A RID: 31754
		public Vector2i? WorldSize;

		// Token: 0x04007C0B RID: 31755
		public long SaveDataSizeForLimit;

		// Token: 0x04007C0C RID: 31756
		public long SaveDataSizeTotal;

		// Token: 0x04007C0D RID: 31757
		public int SaveDataCount;

		// Token: 0x04007C0E RID: 31758
		public long BarStartOffset;

		// Token: 0x04007C0F RID: 31759
		public bool HideIfEmpty;

		// Token: 0x04007C10 RID: 31760
		public readonly List<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();
	}

	// Token: 0x020014CD RID: 5325
	public class SaveEntryInfo : IComparable
	{
		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x0600A68E RID: 42638 RVA: 0x003E0C1E File Offset: 0x003DEE1E
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

		// Token: 0x0600A68F RID: 42639 RVA: 0x003E0C58 File Offset: 0x003DEE58
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

		// Token: 0x04007C11 RID: 31761
		public string Name;

		// Token: 0x04007C12 RID: 31762
		public UserDataStorageType StorageType;

		// Token: 0x04007C13 RID: 31763
		public string SaveDir;

		// Token: 0x04007C14 RID: 31764
		public SaveInfoProvider.SaveSizeInfo SizeInfo;

		// Token: 0x04007C15 RID: 31765
		public long BarStartOffset;

		// Token: 0x04007C16 RID: 31766
		public SaveInfoProvider.WorldEntryInfo WorldEntry;

		// Token: 0x04007C17 RID: 31767
		public DateTime LastSaved;

		// Token: 0x04007C18 RID: 31768
		public VersionInformation Version;

		// Token: 0x04007C19 RID: 31769
		public readonly List<SaveInfoProvider.PlayerEntryInfo> PlayerEntryInfos = new List<SaveInfoProvider.PlayerEntryInfo>();
	}

	// Token: 0x020014CE RID: 5326
	public class PlayerEntryInfo : IComparable
	{
		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x0600A691 RID: 42641 RVA: 0x003E0CAC File Offset: 0x003DEEAC
		public string PlatformName
		{
			get
			{
				PlatformUserIdentifierAbs platformUserIdentifierAbs = this.NativeUserId ?? this.PrimaryUserId;
				return ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.PlatformIdentifierString : null) ?? "-";
			}
		}

		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x0600A692 RID: 42642 RVA: 0x003E0CD4 File Offset: 0x003DEED4
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

		// Token: 0x0600A693 RID: 42643 RVA: 0x003E0D20 File Offset: 0x003DEF20
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

		// Token: 0x04007C1A RID: 31770
		public string Id;

		// Token: 0x04007C1B RID: 31771
		public string CachedName;

		// Token: 0x04007C1C RID: 31772
		public PlatformUserIdentifierAbs PrimaryUserId;

		// Token: 0x04007C1D RID: 31773
		public PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x04007C1E RID: 31774
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformUserData platformUserData;

		// Token: 0x04007C1F RID: 31775
		public long Size;

		// Token: 0x04007C20 RID: 31776
		public long BarStartOffset;

		// Token: 0x04007C21 RID: 31777
		public SaveInfoProvider.SaveEntryInfo SaveEntry;

		// Token: 0x04007C22 RID: 31778
		public DateTime LastPlayed;

		// Token: 0x04007C23 RID: 31779
		public int PlayerLevel;

		// Token: 0x04007C24 RID: 31780
		public float DistanceWalked;
	}

	// Token: 0x020014CF RID: 5327
	public class PlayerEntryInfoPlatformDataResolver
	{
		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x0600A695 RID: 42645 RVA: 0x003E0D61 File Offset: 0x003DEF61
		// (set) Token: 0x0600A696 RID: 42646 RVA: 0x003E0D69 File Offset: 0x003DEF69
		public bool IsComplete
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600A697 RID: 42647 RVA: 0x003E0D72 File Offset: 0x003DEF72
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerEntryInfoPlatformDataResolver(List<SaveInfoProvider.PlayerEntryInfo> playerEntries)
		{
			this.pendingPlayerEntries = playerEntries;
		}

		// Token: 0x0600A698 RID: 42648 RVA: 0x003E0D84 File Offset: 0x003DEF84
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

		// Token: 0x0600A699 RID: 42649 RVA: 0x003E0E28 File Offset: 0x003DF028
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator ResolveUserData(List<IPlatformUserData> resolvingPlatformData)
		{
			yield return PlatformUserManager.ResolveUsersDetailsCoroutine(resolvingPlatformData);
			yield return PlatformUserManager.ResolveUserBlocksCoroutine(resolvingPlatformData);
			this.IsComplete = true;
			yield break;
		}

		// Token: 0x04007C25 RID: 31781
		public readonly List<SaveInfoProvider.PlayerEntryInfo> pendingPlayerEntries;
	}
}
