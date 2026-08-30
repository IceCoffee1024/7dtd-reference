using System;
using System.IO;
using Platform;
using WorldGenerationEngineFinal;

// Token: 0x020014C8 RID: 5320
public static class UserDataManagement
{
	// Token: 0x0600A6E0 RID: 42720 RVA: 0x003E19F4 File Offset: 0x003DFBF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static UserDataManagement.Result MoveWorld(SaveInfoProvider.WorldEntryInfo worldInfo, UserDataStorageType moveToStorageType)
	{
		string worldPath = WorldBuilder.GetWorldPath(moveToStorageType, worldInfo.Name);
		if (SdDirectory.Exists(worldPath))
		{
			Log.Error(string.Concat(new string[]
			{
				"Could not move world from ",
				worldInfo.Location.FullPath,
				" to ",
				worldPath,
				". Directory already exists"
			}));
			return UserDataManagement.Result.TargetAlreadyExists;
		}
		Log.Out(string.Concat(new string[]
		{
			"Moving world ",
			worldInfo.Name,
			" from ",
			worldInfo.Location.FullPath,
			" to ",
			worldPath
		}));
		try
		{
			GameIO.SafeDirectoryMove(worldInfo.Location.FullPath, worldPath);
		}
		catch (Exception ex)
		{
			Log.Error("Could not move world: " + ex.Message);
			Log.Exception(ex);
			return UserDataManagement.Result.Exception;
		}
		return UserDataManagement.Result.Success;
	}

	// Token: 0x0600A6E1 RID: 42721 RVA: 0x003E1ADC File Offset: 0x003DFCDC
	[PublicizedFrom(EAccessModifier.Private)]
	public static UserDataManagement.Result MoveSave(string sourceDir, string targetDir)
	{
		if (SdDirectory.Exists(targetDir))
		{
			Log.Error(string.Concat(new string[] { "Could not move save from ", sourceDir, " to ", targetDir, ". Directory already exists" }));
			return UserDataManagement.Result.TargetAlreadyExists;
		}
		Log.Out("Moving save from " + sourceDir + " to " + targetDir);
		try
		{
			GameIO.SafeDirectoryMove(sourceDir, targetDir);
		}
		catch (Exception ex)
		{
			Log.Error("Could not move save: " + ex.Message);
			Log.Exception(ex);
			return UserDataManagement.Result.Exception;
		}
		return UserDataManagement.Result.Success;
	}

	// Token: 0x0600A6E2 RID: 42722 RVA: 0x003E1B78 File Offset: 0x003DFD78
	[PublicizedFrom(EAccessModifier.Private)]
	public static UserDataManagement.Result CopySave(string sourceDir, string targetDir)
	{
		if (SdDirectory.Exists(targetDir))
		{
			Log.Error(string.Concat(new string[] { "Could not copy save from ", sourceDir, " to ", targetDir, ". Directory already exists" }));
			return UserDataManagement.Result.TargetAlreadyExists;
		}
		Log.Out("Copying save from " + sourceDir + " to " + targetDir);
		try
		{
			GameIO.SafeDirectoryCopy(sourceDir, targetDir);
		}
		catch (Exception ex)
		{
			Log.Error("Could not copy save: " + ex.Message);
			Log.Exception(ex);
			return UserDataManagement.Result.Exception;
		}
		return UserDataManagement.Result.Success;
	}

	// Token: 0x0600A6E3 RID: 42723 RVA: 0x003E1C14 File Offset: 0x003DFE14
	public static string GetSavePathTarget(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType storageTarget)
	{
		SaveInfoProvider.WorldEntryInfo worldEntry = saveInfo.WorldEntry;
		if (worldEntry.Type == SaveInfoProvider.RemoteWorldsType)
		{
			return GameIO.GetSaveGameLocalDir(storageTarget, global::System.IO.Path.GetFileName(saveInfo.SaveDir));
		}
		return GameIO.GetSaveGameDir(worldEntry.Name, saveInfo.Name, storageTarget);
	}

	// Token: 0x0600A6E4 RID: 42724 RVA: 0x003E1C60 File Offset: 0x003DFE60
	public static void SetSaveArchived(string saveDir, bool shouldArchive)
	{
		try
		{
			string text = global::System.IO.Path.Combine(saveDir, "archived.flag");
			if (SdFile.Exists(text))
			{
				if (!shouldArchive)
				{
					Log.Out("Unarchiving save by deleting: " + text);
					SdFile.Delete(text);
				}
			}
			else if (shouldArchive)
			{
				Log.Out("Archiving save by creating: " + text);
				using (SdFile.Create(text))
				{
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("Failed to set archived to {0} for save at {1}", shouldArchive, saveDir));
			Log.Exception(ex);
		}
	}

	// Token: 0x0600A6E5 RID: 42725 RVA: 0x003E1D04 File Offset: 0x003DFF04
	public static void SetSaveDataLimit(string saveDir, long reservedBytes)
	{
		try
		{
			string text = global::System.IO.Path.Combine(saveDir, "main.ttw");
			WorldState worldState = new WorldState();
			if (!worldState.Load(text, false, false, false))
			{
				Log.Error(string.Format("Could not update save data limit for {0} to {1}B. WorldState failed to load", saveDir, reservedBytes));
			}
			else if (worldState.saveDataLimit != reservedBytes)
			{
				Log.Out(string.Format("Updating reserved bytes for {0} to {1}", saveDir, reservedBytes));
				worldState.saveDataLimit = reservedBytes;
				if (!worldState.Save(text))
				{
					Log.Error(string.Format("Could not update save data limit for {0} to {1}B. WorldState failed to save", saveDir, reservedBytes));
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("Failed to set data limit to {0} for save at {1}", reservedBytes, saveDir));
			Log.Exception(ex);
		}
	}

	// Token: 0x020014C9 RID: 5321
	public enum Result
	{
		// Token: 0x04007BC9 RID: 31689
		Success,
		// Token: 0x04007BCA RID: 31690
		TargetAlreadyExists,
		// Token: 0x04007BCB RID: 31691
		Exception,
		// Token: 0x04007BCC RID: 31692
		FailedToMoveSaves
	}

	// Token: 0x020014CA RID: 5322
	public struct SaveMoveResultInfo
	{
		// Token: 0x04007BCD RID: 31693
		public UserDataManagement.Result Result;

		// Token: 0x04007BCE RID: 31694
		public string NewSaveDir;
	}

	// Token: 0x020014CB RID: 5323
	public struct SaveCopyResultInfo
	{
		// Token: 0x04007BCF RID: 31695
		public UserDataManagement.Result Result;

		// Token: 0x04007BD0 RID: 31696
		public string NewSaveDir;
	}

	// Token: 0x020014CC RID: 5324
	public struct WorldMove
	{
		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x0600A6E6 RID: 42726 RVA: 0x003E1DBC File Offset: 0x003DFFBC
		public bool IsReady
		{
			get
			{
				return this.worldInfo != null;
			}
		}

		// Token: 0x0600A6E7 RID: 42727 RVA: 0x003E1DC7 File Offset: 0x003DFFC7
		public WorldMove(SaveInfoProvider.WorldEntryInfo worldInfo, UserDataStorageType moveToStorageType)
		{
			if (!worldInfo.Moveable)
			{
				throw new Exception("Cannot create world move op for non-moveable world");
			}
			this.worldInfo = worldInfo;
			this.moveToStorageType = moveToStorageType;
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x0600A6E8 RID: 42728 RVA: 0x003E1DEC File Offset: 0x003DFFEC
		public int CountOfAssociatedSavesInSameStorage
		{
			get
			{
				int num = 0;
				foreach (SaveInfoProvider.SaveEntryInfo saveEntryInfo in SaveInfoProvider.Instance.SaveEntryInfos)
				{
					if (saveEntryInfo.WorldEntry == this.worldInfo && saveEntryInfo.StorageType == this.worldInfo.Location.StorageType)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600A6E9 RID: 42729 RVA: 0x003E1E64 File Offset: 0x003E0064
		public UserDataManagement.Result PerformMove()
		{
			UserDataManagement.Result result = UserDataManagement.MoveWorld(this.worldInfo, this.moveToStorageType);
			if (result == UserDataManagement.Result.Success && !this.moveToStorageType.UsesDataLimit())
			{
				bool flag = true;
				foreach (SaveInfoProvider.SaveEntryInfo saveEntryInfo in SaveInfoProvider.Instance.SaveEntryInfos)
				{
					if (saveEntryInfo.WorldEntry == this.worldInfo && saveEntryInfo.StorageType != this.moveToStorageType)
					{
						string saveGameDir = GameIO.GetSaveGameDir(this.worldInfo.Name, saveEntryInfo.Name, this.moveToStorageType);
						UserDataManagement.Result result2 = UserDataManagement.MoveSave(saveEntryInfo.SaveDir, saveGameDir);
						if (result2 == UserDataManagement.Result.Success)
						{
							UserDataManagement.SetSaveArchived(saveGameDir, false);
						}
						flag &= result2 == UserDataManagement.Result.Success;
					}
				}
				if (!flag)
				{
					result = UserDataManagement.Result.FailedToMoveSaves;
				}
			}
			PathAbstractions.WorldsSearchPaths.InvalidateCache();
			SaveInfoProvider.Instance.SetDirty();
			return result;
		}

		// Token: 0x04007BD1 RID: 31697
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007BD2 RID: 31698
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x020014CD RID: 5325
	public struct GameSaveMove
	{
		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x0600A6EA RID: 42730 RVA: 0x003E1F50 File Offset: 0x003E0150
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x0600A6EB RID: 42731 RVA: 0x003E1F5B File Offset: 0x003E015B
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.moveToStorageType);
			}
		}

		// Token: 0x0600A6EC RID: 42732 RVA: 0x003E1F6E File Offset: 0x003E016E
		public GameSaveMove(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType moveToStorageType)
		{
			this = new UserDataManagement.GameSaveMove(saveInfo, saveInfo.Name, moveToStorageType);
		}

		// Token: 0x0600A6ED RID: 42733 RVA: 0x003E1F80 File Offset: 0x003E0180
		public GameSaveMove(SaveInfoProvider.SaveEntryInfo saveInfo, string targetName, UserDataStorageType moveToStorageType)
		{
			if (saveInfo.WorldEntry.Type == SaveInfoProvider.RemoteWorldsType && targetName != saveInfo.Name)
			{
				throw new ArgumentException("Cannot rename remote world saves");
			}
			this.saveInfo = saveInfo;
			this.worldInfo = saveInfo.WorldEntry;
			this.targetName = targetName;
			this.moveToStorageType = moveToStorageType;
		}

		// Token: 0x0600A6EE RID: 42734 RVA: 0x003E1FE0 File Offset: 0x003E01E0
		public UserDataManagement.SaveMoveResultInfo PerformMove()
		{
			UserDataManagement.SaveMoveResultInfo saveMoveResultInfo;
			if (this.WorldRequiresMoving)
			{
				UserDataManagement.Result result = UserDataManagement.MoveWorld(this.worldInfo, this.moveToStorageType);
				if (result != UserDataManagement.Result.Success)
				{
					saveMoveResultInfo = new UserDataManagement.SaveMoveResultInfo
					{
						Result = result
					};
					return saveMoveResultInfo;
				}
				PathAbstractions.WorldsSearchPaths.InvalidateCache();
				SaveInfoProvider.Instance.SetDirty();
			}
			string text = ((this.targetName != this.saveInfo.Name) ? GameIO.GetSaveGameDir(this.worldInfo.Name, this.targetName, this.moveToStorageType) : UserDataManagement.GetSavePathTarget(this.saveInfo, this.moveToStorageType));
			UserDataManagement.Result result2 = UserDataManagement.MoveSave(this.saveInfo.SaveDir, text);
			SaveInfoProvider.Instance.SetDirty();
			saveMoveResultInfo = new UserDataManagement.SaveMoveResultInfo
			{
				Result = result2,
				NewSaveDir = ((result2 == UserDataManagement.Result.Success) ? text : null)
			};
			return saveMoveResultInfo;
		}

		// Token: 0x04007BD3 RID: 31699
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007BD4 RID: 31700
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007BD5 RID: 31701
		public readonly string targetName;

		// Token: 0x04007BD6 RID: 31702
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x020014CE RID: 5326
	public struct GameSaveCopy
	{
		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x0600A6EF RID: 42735 RVA: 0x003E20B3 File Offset: 0x003E02B3
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x0600A6F0 RID: 42736 RVA: 0x003E20BE File Offset: 0x003E02BE
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.storageTarget);
			}
		}

		// Token: 0x0600A6F1 RID: 42737 RVA: 0x003E20D4 File Offset: 0x003E02D4
		public GameSaveCopy(SaveInfoProvider.SaveEntryInfo saveInfo, string targetName, UserDataStorageType moveToStorageType)
		{
			this.saveInfo = saveInfo;
			if (saveInfo.WorldEntry.Type == SaveInfoProvider.RemoteWorldsType)
			{
				throw new Exception("Copy not permitted for remote world saves");
			}
			this.worldInfo = saveInfo.WorldEntry;
			this.targetName = targetName;
			this.storageTarget = moveToStorageType;
		}

		// Token: 0x0600A6F2 RID: 42738 RVA: 0x003E2124 File Offset: 0x003E0324
		public UserDataManagement.SaveCopyResultInfo PerformCopy()
		{
			UserDataManagement.SaveCopyResultInfo saveCopyResultInfo;
			if (this.WorldRequiresMoving)
			{
				UserDataManagement.Result result = UserDataManagement.MoveWorld(this.worldInfo, this.storageTarget);
				if (result != UserDataManagement.Result.Success)
				{
					saveCopyResultInfo = new UserDataManagement.SaveCopyResultInfo
					{
						Result = result
					};
					return saveCopyResultInfo;
				}
				PathAbstractions.WorldsSearchPaths.InvalidateCache();
				SaveInfoProvider.Instance.SetDirty();
			}
			string saveGameDir = GameIO.GetSaveGameDir(this.worldInfo.Name, this.targetName, this.storageTarget);
			UserDataManagement.Result result2 = UserDataManagement.CopySave(this.saveInfo.SaveDir, saveGameDir);
			SaveInfoProvider.Instance.SetDirty();
			saveCopyResultInfo = new UserDataManagement.SaveCopyResultInfo
			{
				Result = result2,
				NewSaveDir = ((result2 == UserDataManagement.Result.Success) ? saveGameDir : null)
			};
			return saveCopyResultInfo;
		}

		// Token: 0x04007BD7 RID: 31703
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007BD8 RID: 31704
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007BD9 RID: 31705
		public readonly string targetName;

		// Token: 0x04007BDA RID: 31706
		public readonly UserDataStorageType storageTarget;
	}
}
