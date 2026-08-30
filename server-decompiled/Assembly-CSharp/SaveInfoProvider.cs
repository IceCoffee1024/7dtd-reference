using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Platform;
using Platform.XBL.Save.Storage;
using UnityEngine;

// Token: 0x0200147D RID: 5245
public class SaveInfoProvider
{
	// Token: 0x17001333 RID: 4915
	// (get) Token: 0x0600A465 RID: 42085 RVA: 0x003D3490 File Offset: 0x003D1690
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

	// Token: 0x17001334 RID: 4916
	// (get) Token: 0x0600A466 RID: 42086 RVA: 0x003D34A8 File Offset: 0x003D16A8
	public static bool DataLimitEnabled
	{
		get
		{
			return SaveDataUtils.SaveDataManager.ShouldLimitSize();
		}
	}

	// Token: 0x0600A467 RID: 42087 RVA: 0x003D34B4 File Offset: 0x003D16B4
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string worldName, string fullPath)
	{
		return worldName.ToLowerInvariant() + "_" + GameIO.GetNormalizedPath(fullPath);
	}

	// Token: 0x0600A468 RID: 42088 RVA: 0x003D34CC File Offset: 0x003D16CC
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetWorldEntryKey(string key)
	{
		return key.ToLowerInvariant();
	}

	// Token: 0x0600A469 RID: 42089 RVA: 0x003D34D4 File Offset: 0x003D16D4
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

	// Token: 0x0600A46A RID: 42090 RVA: 0x003D3526 File Offset: 0x003D1726
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetLocalSaveEntryKey(string worldName, string saveName, UserDataStorageType saveStorage)
	{
		return string.Format("{0}/{1}_{2}", worldName, saveName, saveStorage).ToLowerInvariant();
	}

	// Token: 0x17001335 RID: 4917
	// (get) Token: 0x0600A46B RID: 42091 RVA: 0x003D353F File Offset: 0x003D173F
	public IReadOnlyCollection<SaveInfoProvider.WorldEntryInfo> WorldEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.worldEntryInfos;
		}
	}

	// Token: 0x17001336 RID: 4918
	// (get) Token: 0x0600A46C RID: 42092 RVA: 0x003D354D File Offset: 0x003D174D
	public IReadOnlyCollection<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos
	{
		get
		{
			this.RefreshIfDirty();
			return this.saveEntryInfos;
		}
	}

	// Token: 0x17001337 RID: 4919
	// (get) Token: 0x0600A46D RID: 42093 RVA: 0x003D355B File Offset: 0x003D175B
	public long TotalUsedBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalUsedBytes;
		}
	}

	// Token: 0x17001338 RID: 4920
	// (get) Token: 0x0600A46E RID: 42094 RVA: 0x003D3569 File Offset: 0x003D1769
	public long TotalAllowanceBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes;
		}
	}

	// Token: 0x17001339 RID: 4921
	// (get) Token: 0x0600A46F RID: 42095 RVA: 0x003D3577 File Offset: 0x003D1777
	public long TotalAvailableBytes
	{
		get
		{
			this.RefreshIfDirty();
			return this.totalAllowanceBytes - this.totalUsedBytes;
		}
	}

	// Token: 0x0600A470 RID: 42096 RVA: 0x003D358C File Offset: 0x003D178C
	public void SetDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x0600A471 RID: 42097 RVA: 0x003D3598 File Offset: 0x003D1798
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

	// Token: 0x0600A472 RID: 42098 RVA: 0x003D35E0 File Offset: 0x003D17E0
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

	// Token: 0x0600A473 RID: 42099 RVA: 0x003D3840 File Offset: 0x003D1A40
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

	// Token: 0x0600A474 RID: 42100 RVA: 0x003D38AC File Offset: 0x003D1AAC
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

	// Token: 0x0600A475 RID: 42101 RVA: 0x003D3A64 File Offset: 0x003D1C64
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessLocalWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessLocalWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessLocalWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A476 RID: 42102 RVA: 0x003D3A88 File Offset: 0x003D1C88
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

	// Token: 0x0600A477 RID: 42103 RVA: 0x003D3D5C File Offset: 0x003D1F5C
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

	// Token: 0x0600A478 RID: 42104 RVA: 0x003D3FA8 File Offset: 0x003D21A8
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

	// Token: 0x0600A479 RID: 42105 RVA: 0x003D4220 File Offset: 0x003D2420
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessRemoteWorldSaves()
	{
		if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
		{
			this.ProcessRemoteWorldSaves(UserDataStorageType.Roaming);
		}
		this.ProcessRemoteWorldSaves(UserDataStorageType.DeviceLocal);
	}

	// Token: 0x0600A47A RID: 42106 RVA: 0x003D4244 File Offset: 0x003D2444
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

	// Token: 0x0600A47B RID: 42107 RVA: 0x003D4594 File Offset: 0x003D2794
	public bool TryGetLocalSaveEntry(string worldName, string saveName, UserDataStorageType storage, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		string localSaveEntryKey = SaveInfoProvider.GetLocalSaveEntryKey(worldName, saveName, storage);
		return this.saveEntryInfosBySaveKey.TryGetValue(localSaveEntryKey, out saveEntryInfo);
	}

	// Token: 0x0600A47C RID: 42108 RVA: 0x003D45BE File Offset: 0x003D27BE
	public bool TryGetRemoteSaveEntry(string guid, out SaveInfoProvider.SaveEntryInfo saveEntryInfo)
	{
		this.RefreshIfDirty();
		return this.remoteSaveEntryInfosByGuid.TryGetValue(guid, out saveEntryInfo);
	}

	// Token: 0x0600A47D RID: 42109 RVA: 0x003D45D3 File Offset: 0x003D27D3
	[PublicizedFrom(EAccessModifier.Private)]
	public string NormalizePath(string path)
	{
		return Path.GetFullPath(path);
	}

	// Token: 0x0600A47E RID: 42110 RVA: 0x003D45DB File Offset: 0x003D27DB
	public void SetDirectoryProtected(string path, bool isProtected)
	{
		if (isProtected)
		{
			this.protectedDirectories.Add(this.NormalizePath(path));
			return;
		}
		this.protectedDirectories.Remove(this.NormalizePath(path));
	}

	// Token: 0x0600A47F RID: 42111 RVA: 0x003D4607 File Offset: 0x003D2807
	public bool IsDirectoryProtected(string path)
	{
		return this.protectedDirectories.Contains(this.NormalizePath(path));
	}

	// Token: 0x04007A53 RID: 31315
	[PublicizedFrom(EAccessModifier.Private)]
	public const long BytesPerMB = 1048576L;

	// Token: 0x04007A54 RID: 31316
	public static readonly string RemoteWorldsLabel = "[" + Localization.Get("xuiDmRemoteWorlds", false, null) + "] ";

	// Token: 0x04007A55 RID: 31317
	public static readonly string RemoteWorldsType = Localization.Get("xuiDmRemote", false, null);

	// Token: 0x04007A56 RID: 31318
	public static readonly string DeletedWorldsType = Localization.Get("xuiDmDeleted", false, null);

	// Token: 0x04007A57 RID: 31319
	public static readonly string ConflictedWorldsType = Localization.Get("xuiDmConflicted", false, null);

	// Token: 0x04007A58 RID: 31320
	public static readonly List<string> HideableWorlds = new List<string> { "Empty", "Playtesting" };

	// Token: 0x04007A59 RID: 31321
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveInfoProvider instance;

	// Token: 0x04007A5A RID: 31322
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> worldEntryInfosByWorldKey = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007A5B RID: 31323
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> deletedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007A5C RID: 31324
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.WorldEntryInfo> conflictedWorlds = new Dictionary<string, SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007A5D RID: 31325
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> saveEntryInfosBySaveKey = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007A5E RID: 31326
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.SaveEntryInfo> remoteSaveEntryInfosByGuid = new Dictionary<string, SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007A5F RID: 31327
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.WorldEntryInfo> worldEntryInfos = new List<SaveInfoProvider.WorldEntryInfo>();

	// Token: 0x04007A60 RID: 31328
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SaveInfoProvider.SaveEntryInfo> saveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();

	// Token: 0x04007A61 RID: 31329
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<string> protectedDirectories = new HashSet<string>();

	// Token: 0x04007A62 RID: 31330
	[PublicizedFrom(EAccessModifier.Private)]
	public long localSavesSum;

	// Token: 0x04007A63 RID: 31331
	[PublicizedFrom(EAccessModifier.Private)]
	public long remoteSavesSum;

	// Token: 0x04007A64 RID: 31332
	[PublicizedFrom(EAccessModifier.Private)]
	public long worldsSum;

	// Token: 0x04007A65 RID: 31333
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalUsedBytes;

	// Token: 0x04007A66 RID: 31334
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalAllowanceBytes;

	// Token: 0x04007A67 RID: 31335
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty = true;

	// Token: 0x04007A68 RID: 31336
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, SaveInfoProvider.PlayerEntryInfo> fileNameKeysToPlayerInfos = new Dictionary<string, SaveInfoProvider.PlayerEntryInfo>();

	// Token: 0x0200147E RID: 5246
	public struct SaveSizeInfo
	{
		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x0600A482 RID: 42114 RVA: 0x003D471C File Offset: 0x003D291C
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

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x0600A483 RID: 42115 RVA: 0x003D473B File Offset: 0x003D293B
		public long MaxSize
		{
			get
			{
				return Math.Max(this.BytesOnDisk, this.BytesReserved);
			}
		}

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x0600A484 RID: 42116 RVA: 0x003D474E File Offset: 0x003D294E
		public bool UsesDataLimit
		{
			get
			{
				return this.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x0600A485 RID: 42117 RVA: 0x003D475B File Offset: 0x003D295B
		public bool Archivable
		{
			get
			{
				return this.UsesDataLimit && this.BytesReserved >= this.BytesOnDisk;
			}
		}

		// Token: 0x04007A69 RID: 31337
		public UserDataStorageType StorageType;

		// Token: 0x04007A6A RID: 31338
		public long BytesOnDisk;

		// Token: 0x04007A6B RID: 31339
		public long BytesReserved;

		// Token: 0x04007A6C RID: 31340
		public bool IsArchived;
	}

	// Token: 0x0200147F RID: 5247
	public class WorldEntryInfo : IComparable
	{
		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x0600A486 RID: 42118 RVA: 0x003D4778 File Offset: 0x003D2978
		public bool UsesDataLimit
		{
			get
			{
				return this.Location.StorageType.UsesDataLimit();
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x0600A487 RID: 42119 RVA: 0x003D478C File Offset: 0x003D298C
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

		// Token: 0x0600A488 RID: 42120 RVA: 0x003D47E4 File Offset: 0x003D29E4
		public bool ShouldBeMovedWithSave(UserDataStorageType targetSaveStorage)
		{
			return targetSaveStorage.UsesDataLimit() && this.Moveable && targetSaveStorage != this.Location.StorageType;
		}

		// Token: 0x0600A489 RID: 42121 RVA: 0x003D480C File Offset: 0x003D2A0C
		public int CompareTo(object obj)
		{
			SaveInfoProvider.WorldEntryInfo worldEntryInfo = obj as SaveInfoProvider.WorldEntryInfo;
			if (worldEntryInfo != null)
			{
				return string.Compare(this.WorldKey, worldEntryInfo.WorldKey, StringComparison.OrdinalIgnoreCase);
			}
			return 1;
		}

		// Token: 0x04007A6D RID: 31341
		public string WorldKey;

		// Token: 0x04007A6E RID: 31342
		public string Name;

		// Token: 0x04007A6F RID: 31343
		public string Type;

		// Token: 0x04007A70 RID: 31344
		public PathAbstractions.AbstractedLocation Location;

		// Token: 0x04007A71 RID: 31345
		public bool Deletable;

		// Token: 0x04007A72 RID: 31346
		public bool Moveable;

		// Token: 0x04007A73 RID: 31347
		public long WorldDataSize;

		// Token: 0x04007A74 RID: 31348
		public VersionInformation Version;

		// Token: 0x04007A75 RID: 31349
		public Vector2i? WorldSize;

		// Token: 0x04007A76 RID: 31350
		public long SaveDataSizeForLimit;

		// Token: 0x04007A77 RID: 31351
		public long SaveDataSizeTotal;

		// Token: 0x04007A78 RID: 31352
		public int SaveDataCount;

		// Token: 0x04007A79 RID: 31353
		public long BarStartOffset;

		// Token: 0x04007A7A RID: 31354
		public bool HideIfEmpty;

		// Token: 0x04007A7B RID: 31355
		public readonly List<SaveInfoProvider.SaveEntryInfo> SaveEntryInfos = new List<SaveInfoProvider.SaveEntryInfo>();
	}

	// Token: 0x02001480 RID: 5248
	public class SaveEntryInfo : IComparable
	{
		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x0600A48B RID: 42123 RVA: 0x003D484A File Offset: 0x003D2A4A
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

		// Token: 0x0600A48C RID: 42124 RVA: 0x003D4884 File Offset: 0x003D2A84
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

		// Token: 0x04007A7C RID: 31356
		public string Name;

		// Token: 0x04007A7D RID: 31357
		public UserDataStorageType StorageType;

		// Token: 0x04007A7E RID: 31358
		public string SaveDir;

		// Token: 0x04007A7F RID: 31359
		public SaveInfoProvider.SaveSizeInfo SizeInfo;

		// Token: 0x04007A80 RID: 31360
		public long BarStartOffset;

		// Token: 0x04007A81 RID: 31361
		public SaveInfoProvider.WorldEntryInfo WorldEntry;

		// Token: 0x04007A82 RID: 31362
		public DateTime LastSaved;

		// Token: 0x04007A83 RID: 31363
		public VersionInformation Version;

		// Token: 0x04007A84 RID: 31364
		public readonly List<SaveInfoProvider.PlayerEntryInfo> PlayerEntryInfos = new List<SaveInfoProvider.PlayerEntryInfo>();
	}

	// Token: 0x02001481 RID: 5249
	public class PlayerEntryInfo : IComparable
	{
		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x0600A48E RID: 42126 RVA: 0x003D48D8 File Offset: 0x003D2AD8
		public string PlatformName
		{
			get
			{
				PlatformUserIdentifierAbs platformUserIdentifierAbs = this.NativeUserId ?? this.PrimaryUserId;
				return ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.PlatformIdentifierString : null) ?? "-";
			}
		}

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x0600A48F RID: 42127 RVA: 0x003D4900 File Offset: 0x003D2B00
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

		// Token: 0x0600A490 RID: 42128 RVA: 0x003D494C File Offset: 0x003D2B4C
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

		// Token: 0x04007A85 RID: 31365
		public string Id;

		// Token: 0x04007A86 RID: 31366
		public string CachedName;

		// Token: 0x04007A87 RID: 31367
		public PlatformUserIdentifierAbs PrimaryUserId;

		// Token: 0x04007A88 RID: 31368
		public PlatformUserIdentifierAbs NativeUserId;

		// Token: 0x04007A89 RID: 31369
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatformUserData platformUserData;

		// Token: 0x04007A8A RID: 31370
		public long Size;

		// Token: 0x04007A8B RID: 31371
		public long BarStartOffset;

		// Token: 0x04007A8C RID: 31372
		public SaveInfoProvider.SaveEntryInfo SaveEntry;

		// Token: 0x04007A8D RID: 31373
		public DateTime LastPlayed;

		// Token: 0x04007A8E RID: 31374
		public int PlayerLevel;

		// Token: 0x04007A8F RID: 31375
		public float DistanceWalked;
	}

	// Token: 0x02001482 RID: 5250
	public class PlayerEntryInfoPlatformDataResolver
	{
		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x0600A492 RID: 42130 RVA: 0x003D498D File Offset: 0x003D2B8D
		// (set) Token: 0x0600A493 RID: 42131 RVA: 0x003D4995 File Offset: 0x003D2B95
		public bool IsComplete
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600A494 RID: 42132 RVA: 0x003D499E File Offset: 0x003D2B9E
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerEntryInfoPlatformDataResolver(List<SaveInfoProvider.PlayerEntryInfo> playerEntries)
		{
			this.pendingPlayerEntries = playerEntries;
		}

		// Token: 0x0600A495 RID: 42133 RVA: 0x003D49B0 File Offset: 0x003D2BB0
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

		// Token: 0x0600A496 RID: 42134 RVA: 0x003D4A54 File Offset: 0x003D2C54
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator ResolveUserData(List<IPlatformUserData> resolvingPlatformData)
		{
			yield return PlatformUserManager.ResolveUsersDetailsCoroutine(resolvingPlatformData);
			yield return PlatformUserManager.ResolveUserBlocksCoroutine(resolvingPlatformData);
			this.IsComplete = true;
			yield break;
		}

		// Token: 0x04007A90 RID: 31376
		public readonly List<SaveInfoProvider.PlayerEntryInfo> pendingPlayerEntries;
	}
}
