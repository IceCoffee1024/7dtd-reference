using System;
using System.IO;
using Platform;
using WorldGenerationEngineFinal;

// Token: 0x020014E3 RID: 5347
public static class UserDataManagement
{
	// Token: 0x0600A7DD RID: 42973 RVA: 0x003E6D34 File Offset: 0x003E4F34
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

	// Token: 0x0600A7DE RID: 42974 RVA: 0x003E6E1C File Offset: 0x003E501C
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

	// Token: 0x0600A7DF RID: 42975 RVA: 0x003E6EB8 File Offset: 0x003E50B8
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

	// Token: 0x0600A7E0 RID: 42976 RVA: 0x003E6F54 File Offset: 0x003E5154
	public static string GetSavePathTarget(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType storageTarget)
	{
		SaveInfoProvider.WorldEntryInfo worldEntry = saveInfo.WorldEntry;
		if (worldEntry.Type == SaveInfoProvider.RemoteWorldsType)
		{
			return GameIO.GetSaveGameLocalDir(storageTarget, global::System.IO.Path.GetFileName(saveInfo.SaveDir));
		}
		return GameIO.GetSaveGameDir(worldEntry.Name, saveInfo.Name, storageTarget);
	}

	// Token: 0x0600A7E1 RID: 42977 RVA: 0x003E6FA0 File Offset: 0x003E51A0
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

	// Token: 0x0600A7E2 RID: 42978 RVA: 0x003E7044 File Offset: 0x003E5244
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

	// Token: 0x020014E4 RID: 5348
	public enum Result
	{
		// Token: 0x04007CA2 RID: 31906
		Success,
		// Token: 0x04007CA3 RID: 31907
		TargetAlreadyExists,
		// Token: 0x04007CA4 RID: 31908
		Exception,
		// Token: 0x04007CA5 RID: 31909
		FailedToMoveSaves
	}

	// Token: 0x020014E5 RID: 5349
	public struct SaveMoveResultInfo
	{
		// Token: 0x04007CA6 RID: 31910
		public UserDataManagement.Result Result;

		// Token: 0x04007CA7 RID: 31911
		public string NewSaveDir;
	}

	// Token: 0x020014E6 RID: 5350
	public struct SaveCopyResultInfo
	{
		// Token: 0x04007CA8 RID: 31912
		public UserDataManagement.Result Result;

		// Token: 0x04007CA9 RID: 31913
		public string NewSaveDir;
	}

	// Token: 0x020014E7 RID: 5351
	public struct WorldMove
	{
		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x0600A7E3 RID: 42979 RVA: 0x003E70FC File Offset: 0x003E52FC
		public bool IsReady
		{
			get
			{
				return this.worldInfo != null;
			}
		}

		// Token: 0x0600A7E4 RID: 42980 RVA: 0x003E7107 File Offset: 0x003E5307
		public WorldMove(SaveInfoProvider.WorldEntryInfo worldInfo, UserDataStorageType moveToStorageType)
		{
			if (!worldInfo.Moveable)
			{
				throw new Exception("Cannot create world move op for non-moveable world");
			}
			this.worldInfo = worldInfo;
			this.moveToStorageType = moveToStorageType;
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x0600A7E5 RID: 42981 RVA: 0x003E712C File Offset: 0x003E532C
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

		// Token: 0x0600A7E6 RID: 42982 RVA: 0x003E71A4 File Offset: 0x003E53A4
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

		// Token: 0x04007CAA RID: 31914
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007CAB RID: 31915
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x020014E8 RID: 5352
	public struct GameSaveMove
	{
		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x0600A7E7 RID: 42983 RVA: 0x003E7290 File Offset: 0x003E5490
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x0600A7E8 RID: 42984 RVA: 0x003E729B File Offset: 0x003E549B
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.moveToStorageType);
			}
		}

		// Token: 0x0600A7E9 RID: 42985 RVA: 0x003E72AE File Offset: 0x003E54AE
		public GameSaveMove(SaveInfoProvider.SaveEntryInfo saveInfo, UserDataStorageType moveToStorageType)
		{
			this = new UserDataManagement.GameSaveMove(saveInfo, saveInfo.Name, moveToStorageType);
		}

		// Token: 0x0600A7EA RID: 42986 RVA: 0x003E72C0 File Offset: 0x003E54C0
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

		// Token: 0x0600A7EB RID: 42987 RVA: 0x003E7320 File Offset: 0x003E5520
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

		// Token: 0x04007CAC RID: 31916
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007CAD RID: 31917
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007CAE RID: 31918
		public readonly string targetName;

		// Token: 0x04007CAF RID: 31919
		public readonly UserDataStorageType moveToStorageType;
	}

	// Token: 0x020014E9 RID: 5353
	public struct GameSaveCopy
	{
		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x0600A7EC RID: 42988 RVA: 0x003E73F3 File Offset: 0x003E55F3
		public bool IsReady
		{
			get
			{
				return this.saveInfo != null;
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x0600A7ED RID: 42989 RVA: 0x003E73FE File Offset: 0x003E55FE
		public bool WorldRequiresMoving
		{
			get
			{
				return this.worldInfo.ShouldBeMovedWithSave(this.storageTarget);
			}
		}

		// Token: 0x0600A7EE RID: 42990 RVA: 0x003E7414 File Offset: 0x003E5614
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

		// Token: 0x0600A7EF RID: 42991 RVA: 0x003E7464 File Offset: 0x003E5664
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

		// Token: 0x04007CB0 RID: 31920
		public readonly SaveInfoProvider.SaveEntryInfo saveInfo;

		// Token: 0x04007CB1 RID: 31921
		public readonly SaveInfoProvider.WorldEntryInfo worldInfo;

		// Token: 0x04007CB2 RID: 31922
		public readonly string targetName;

		// Token: 0x04007CB3 RID: 31923
		public readonly UserDataStorageType storageTarget;
	}
}
