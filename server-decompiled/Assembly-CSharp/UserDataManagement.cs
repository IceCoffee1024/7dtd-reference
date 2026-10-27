using System;
using System.IO;
using Platform;
using WorldGenerationEngineFinal;

// Token: 0x02001517 RID: 5399
public static class UserDataManagement
{
	// Token: 0x0600A8E8 RID: 43240 RVA: 0x003EDF8C File Offset: 0x003EC18C
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

	// Token: 0x0600A8E9 RID: 43241 RVA: 0x003EE074 File Offset: 0x003EC274
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

	// Token: 0x0600A8EA RID: 43242 RVA: 0x003EE110 File Offset: 0x003EC310
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

	// Token: 0x0600A8EB RID: 43243 RVA: 0x003EE1AC File Offset: 0x003EC3AC
	public static string GetSavePathTarget(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType storageTarget)
	{
		SaveInfoProvider.WorldEntryInfo worldEntry = saveInfo.WorldEntry;
		if (worldEntry.Type == SaveInfoProvider.RemoteWorldsType)
		{
			return GameIO.GetSaveGameLocalDir(storageTarget, global::System.IO.Path.GetFileName(saveInfo.SaveDir));
		}
		return GameIO.GetSaveGameDir(worldEntry.Name, saveInfo.Name, storageTarget);
	}

	// Token: 0x0600A8EC RID: 43244 RVA: 0x003EE1F8 File Offset: 0x003EC3F8
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

	// Token: 0x0600A8ED RID: 43245 RVA: 0x003EE29C File Offset: 0x003EC49C
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

	// Token: 0x02001518 RID: 5400
	public enum Result
	{
		// Token: 0x04007D66 RID: 32102
		Success,
		// Token: 0x04007D67 RID: 32103
		TargetAlreadyExists,
		// Token: 0x04007D68 RID: 32104
		Exception,
		// Token: 0x04007D69 RID: 32105
		FailedToMoveSaves
	}

	// Token: 0x02001519 RID: 5401
	public struct SaveMoveResultInfo
	{
		// Token: 0x04007D6A RID: 32106
		public UserDataManagement.Result Result;

		// Token: 0x04007D6B RID: 32107
		public string NewSaveDir;
	}

	// Token: 0x0200151A RID: 5402
	public struct SaveCopyResultInfo
	{
		// Token: 0x04007D6C RID: 32108
		public UserDataManagement.Result Result;

		// Token: 0x04007D6D RID: 32109
		public string NewSaveDir;
	}

	// Token: 0x0200151B RID: 5403
	public struct WorldMove
	{
		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x0600A8EE RID: 43246 RVA: 0x003EE354 File Offset: 0x003EC554
		public bool IsReady
		{
			get
			{
				return this.worldInfo != null;
			}
		}

		// Token: 0x0600A8EF RID: 43247 RVA: 0x003EE35F File Offset: 0x003EC55F
		public WorldMove(SaveInfoProvider.WorldEntryInfo worldInfo, UserDataStorageType moveToStorageType)
		{
			if (!worldInfo.Moveable)
			{
				throw new Exception("Cannot create world move op for non-moveable world");
			}
			this.worldInfo = worldInfo;
			this.moveToStorageType = moveToStorageType;
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x0600A8F0 RID: 43248 RVA: 0x003EE384 File Offset: 0x003EC584
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

		// Token: 0x0600A8F1 RID: 43249 RVA: 0x003EE3FC File Offset: 0x003EC5FC
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

		// Token: 0x04007D6E RID: 32110
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007D6F RID: 32111
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x0200151C RID: 5404
	public struct GameSaveMove
	{
		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x0600A8F2 RID: 43250 RVA: 0x003EE4E8 File Offset: 0x003EC6E8
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x0600A8F3 RID: 43251 RVA: 0x003EE4F3 File Offset: 0x003EC6F3
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.moveToStorageType);
			}
		}

		// Token: 0x0600A8F4 RID: 43252 RVA: 0x003EE506 File Offset: 0x003EC706
		public GameSaveMove(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType moveToStorageType)
		{
			this = new UserDataManagement.GameSaveMove(saveInfo, saveInfo.Name, moveToStorageType);
		}

		// Token: 0x0600A8F5 RID: 43253 RVA: 0x003EE518 File Offset: 0x003EC718
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

		// Token: 0x0600A8F6 RID: 43254 RVA: 0x003EE578 File Offset: 0x003EC778
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

		// Token: 0x04007D70 RID: 32112
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007D71 RID: 32113
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007D72 RID: 32114
		public readonly string targetName;

		// Token: 0x04007D73 RID: 32115
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x0200151D RID: 5405
	public struct GameSaveCopy
	{
		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x0600A8F7 RID: 43255 RVA: 0x003EE64B File Offset: 0x003EC84B
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x0600A8F8 RID: 43256 RVA: 0x003EE656 File Offset: 0x003EC856
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.storageTarget);
			}
		}

		// Token: 0x0600A8F9 RID: 43257 RVA: 0x003EE66C File Offset: 0x003EC86C
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

		// Token: 0x0600A8FA RID: 43258 RVA: 0x003EE6BC File Offset: 0x003EC8BC
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

		// Token: 0x04007D74 RID: 32116
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007D75 RID: 32117
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007D76 RID: 32118
		public readonly string targetName;

		// Token: 0x04007D77 RID: 32119
		public readonly UserDataStorageType storageTarget;
	}
}
