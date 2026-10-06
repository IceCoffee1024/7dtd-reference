using System;
using System.IO;
using Platform;
using WorldGenerationEngineFinal;

// Token: 0x02001505 RID: 5381
public static class UserDataManagement
{
	// Token: 0x0600A890 RID: 43152 RVA: 0x003EBB88 File Offset: 0x003E9D88
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

	// Token: 0x0600A891 RID: 43153 RVA: 0x003EBC70 File Offset: 0x003E9E70
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

	// Token: 0x0600A892 RID: 43154 RVA: 0x003EBD0C File Offset: 0x003E9F0C
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

	// Token: 0x0600A893 RID: 43155 RVA: 0x003EBDA8 File Offset: 0x003E9FA8
	public static string GetSavePathTarget(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType storageTarget)
	{
		SaveInfoProvider.WorldEntryInfo worldEntry = saveInfo.WorldEntry;
		if (worldEntry.Type == SaveInfoProvider.RemoteWorldsType)
		{
			return GameIO.GetSaveGameLocalDir(storageTarget, global::System.IO.Path.GetFileName(saveInfo.SaveDir));
		}
		return GameIO.GetSaveGameDir(worldEntry.Name, saveInfo.Name, storageTarget);
	}

	// Token: 0x0600A894 RID: 43156 RVA: 0x003EBDF4 File Offset: 0x003E9FF4
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

	// Token: 0x0600A895 RID: 43157 RVA: 0x003EBE98 File Offset: 0x003EA098
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

	// Token: 0x02001506 RID: 5382
	public enum Result
	{
		// Token: 0x04007D25 RID: 32037
		Success,
		// Token: 0x04007D26 RID: 32038
		TargetAlreadyExists,
		// Token: 0x04007D27 RID: 32039
		Exception,
		// Token: 0x04007D28 RID: 32040
		FailedToMoveSaves
	}

	// Token: 0x02001507 RID: 5383
	public struct SaveMoveResultInfo
	{
		// Token: 0x04007D29 RID: 32041
		public UserDataManagement.Result Result;

		// Token: 0x04007D2A RID: 32042
		public string NewSaveDir;
	}

	// Token: 0x02001508 RID: 5384
	public struct SaveCopyResultInfo
	{
		// Token: 0x04007D2B RID: 32043
		public UserDataManagement.Result Result;

		// Token: 0x04007D2C RID: 32044
		public string NewSaveDir;
	}

	// Token: 0x02001509 RID: 5385
	public struct WorldMove
	{
		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x0600A896 RID: 43158 RVA: 0x003EBF50 File Offset: 0x003EA150
		public bool IsReady
		{
			get
			{
				return this.worldInfo != null;
			}
		}

		// Token: 0x0600A897 RID: 43159 RVA: 0x003EBF5B File Offset: 0x003EA15B
		public WorldMove(SaveInfoProvider.WorldEntryInfo worldInfo, UserDataStorageType moveToStorageType)
		{
			if (!worldInfo.Moveable)
			{
				throw new Exception("Cannot create world move op for non-moveable world");
			}
			this.worldInfo = worldInfo;
			this.moveToStorageType = moveToStorageType;
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x0600A898 RID: 43160 RVA: 0x003EBF80 File Offset: 0x003EA180
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

		// Token: 0x0600A899 RID: 43161 RVA: 0x003EBFF8 File Offset: 0x003EA1F8
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

		// Token: 0x04007D2D RID: 32045
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007D2E RID: 32046
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x0200150A RID: 5386
	public struct GameSaveMove
	{
		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x0600A89A RID: 43162 RVA: 0x003EC0E4 File Offset: 0x003EA2E4
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x0600A89B RID: 43163 RVA: 0x003EC0EF File Offset: 0x003EA2EF
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.moveToStorageType);
			}
		}

		// Token: 0x0600A89C RID: 43164 RVA: 0x003EC102 File Offset: 0x003EA302
		public GameSaveMove(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType moveToStorageType)
		{
			this = new UserDataManagement.GameSaveMove(saveInfo, saveInfo.Name, moveToStorageType);
		}

		// Token: 0x0600A89D RID: 43165 RVA: 0x003EC114 File Offset: 0x003EA314
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

		// Token: 0x0600A89E RID: 43166 RVA: 0x003EC174 File Offset: 0x003EA374
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

		// Token: 0x04007D2F RID: 32047
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007D30 RID: 32048
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007D31 RID: 32049
		public readonly string targetName;

		// Token: 0x04007D32 RID: 32050
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x0200150B RID: 5387
	public struct GameSaveCopy
	{
		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x0600A89F RID: 43167 RVA: 0x003EC247 File Offset: 0x003EA447
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x0600A8A0 RID: 43168 RVA: 0x003EC252 File Offset: 0x003EA452
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.storageTarget);
			}
		}

		// Token: 0x0600A8A1 RID: 43169 RVA: 0x003EC268 File Offset: 0x003EA468
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

		// Token: 0x0600A8A2 RID: 43170 RVA: 0x003EC2B8 File Offset: 0x003EA4B8
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

		// Token: 0x04007D33 RID: 32051
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007D34 RID: 32052
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007D35 RID: 32053
		public readonly string targetName;

		// Token: 0x04007D36 RID: 32054
		public readonly UserDataStorageType storageTarget;
	}
}
