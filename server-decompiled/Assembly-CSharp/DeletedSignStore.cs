using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A7F RID: 2687
public class DeletedSignStore
{
	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x060050C9 RID: 20681 RVA: 0x001F0453 File Offset: 0x001EE653
	public bool Overflowed
	{
		get
		{
			return this.overflowed;
		}
	}

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x060050CA RID: 20682 RVA: 0x001F045B File Offset: 0x001EE65B
	public int Count
	{
		get
		{
			return this.deletedSignIds.Count;
		}
	}

	// Token: 0x060050CB RID: 20683 RVA: 0x001F0468 File Offset: 0x001EE668
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetStorePath()
	{
		return GameIO.GetSaveGameDir() + "/deletedSigns.dat";
	}

	// Token: 0x060050CC RID: 20684 RVA: 0x001F0479 File Offset: 0x001EE679
	public bool IsDeleted(GlobalSignId signId)
	{
		if (this.overflowed)
		{
			return SignDataManager.IsPlayerLibraryId(signId.libraryId);
		}
		return this.deletedSignIds.Contains(signId);
	}

	// Token: 0x060050CD RID: 20685 RVA: 0x001F049C File Offset: 0x001EE69C
	public void Add(GlobalSignId signId)
	{
		if (this.overflowed)
		{
			return;
		}
		if (!this.deletedSignIds.Add(signId))
		{
			return;
		}
		this.dirty = true;
		if (this.deletedSignIds.Count >= 10000)
		{
			this.overflowed = true;
			this.deletedSignIds.Clear();
			Log.Warning(string.Format("[DeletedSignStore] Reached the cap of {0} deleted sign records. From now on any missing player-made ", 10000) + "signs will be treated as deliberately deleted and silently replaced with the default sign instead of logging an error.");
		}
	}

	// Token: 0x060050CE RID: 20686 RVA: 0x001F050F File Offset: 0x001EE70F
	public void Remove(GlobalSignId signId)
	{
		if (this.deletedSignIds.Remove(signId))
		{
			this.dirty = true;
		}
	}

	// Token: 0x060050CF RID: 20687 RVA: 0x001F0528 File Offset: 0x001EE728
	public void PruneExistingSigns(SignDataManager manager)
	{
		List<GlobalSignId> list = null;
		foreach (GlobalSignId globalSignId in this.deletedSignIds)
		{
			SignData signData;
			if (manager.TryGetSignData(globalSignId, out signData))
			{
				if (list == null)
				{
					list = new List<GlobalSignId>();
				}
				list.Add(globalSignId);
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (GlobalSignId globalSignId2 in list)
		{
			this.deletedSignIds.Remove(globalSignId2);
			this.dirty = true;
			Log.Warning(string.Format("[DeletedSignStore] Dropped a stale deletion record for sign '{0}'; the sign exists.", globalSignId2));
		}
	}

	// Token: 0x060050D0 RID: 20688 RVA: 0x001F05F8 File Offset: 0x001EE7F8
	public void Load()
	{
		this.deletedSignIds.Clear();
		this.overflowed = false;
		this.dirty = false;
		string storePath = DeletedSignStore.GetStorePath();
		if (this.TryReadFile(storePath))
		{
			return;
		}
		string text = storePath + ".bak";
		if (SdFile.Exists(text))
		{
			Log.Warning(string.Concat(new string[] { "[DeletedSignStore] Could not use '", storePath, "'; falling back to backup '", text, "'." }));
			if (this.TryReadFile(text))
			{
				this.dirty = true;
				return;
			}
		}
	}

	// Token: 0x060050D1 RID: 20689 RVA: 0x001F0683 File Offset: 0x001EE883
	public void Update()
	{
		this.CheckCompletedWrite();
		if (this.dirty && this.writeTask == null)
		{
			this.BeginWrite(false);
		}
	}

	// Token: 0x060050D2 RID: 20690 RVA: 0x001F06A2 File Offset: 0x001EE8A2
	public void Flush()
	{
		if (this.writeTask != null)
		{
			this.writeTask.WaitForEnd();
			ThreadManager.ReleaseTaskInfo(this.writeTask);
			this.writeTask = null;
		}
		if (this.dirty)
		{
			this.BeginWrite(true);
		}
	}

	// Token: 0x060050D3 RID: 20691 RVA: 0x001F06D8 File Offset: 0x001EE8D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckCompletedWrite()
	{
		if (this.writeTask == null)
		{
			return;
		}
		if (this.writeTask.evStopped == null || this.writeTask.evStopped.WaitOne(0))
		{
			ThreadManager.ReleaseTaskInfo(this.writeTask);
			this.writeTask = null;
		}
	}

	// Token: 0x060050D4 RID: 20692 RVA: 0x001F0718 File Offset: 0x001EE918
	[PublicizedFrom(EAccessModifier.Private)]
	public void BeginWrite(bool waitForCompletion)
	{
		this.dirty = false;
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		try
		{
			this.BuildImage(pooledExpandableMemoryStream);
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[DeletedSignStore] Failed to build store image: {0}", ex));
			MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
			return;
		}
		if (waitForCompletion)
		{
			DeletedSignStore.WriteImage(pooledExpandableMemoryStream);
			return;
		}
		this.writeTask = ThreadManager.AddSingleTask(delegate(ThreadManager.TaskInfo _taskInfo)
		{
			DeletedSignStore.WriteImage((PooledExpandableMemoryStream)_taskInfo.parameter);
		}, pooledExpandableMemoryStream, null, true);
	}

	// Token: 0x060050D5 RID: 20693 RVA: 0x001F07A8 File Offset: 0x001EE9A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildImage(PooledExpandableMemoryStream image)
	{
		SignStoreIO.BeginImage(image);
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(image);
			foreach (GlobalSignId globalSignId in this.deletedSignIds)
			{
				globalSignId.ToStream(pooledBinaryWriter);
			}
		}
		SignStoreIO.FinalizeImage(image, 1313297220U, 1, this.deletedSignIds.Count, this.overflowed ? 1 : 0);
	}

	// Token: 0x060050D6 RID: 20694 RVA: 0x001F0850 File Offset: 0x001EEA50
	[PublicizedFrom(EAccessModifier.Private)]
	public static void WriteImage(PooledExpandableMemoryStream image)
	{
		try
		{
			SignStoreIO.WriteWithBackup(DeletedSignStore.GetStorePath(), image);
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[DeletedSignStore] Failed to write store: {0}", ex));
		}
		finally
		{
			MemoryPools.poolMemoryStream.FreeSync(image);
		}
	}

	// Token: 0x060050D7 RID: 20695 RVA: 0x001F08A8 File Offset: 0x001EEAA8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryReadFile(string path)
	{
		byte[] array;
		if (!SignStoreIO.TryReadAllBytes("[DeletedSignStore]", path, out array))
		{
			return false;
		}
		if (!this.TryParseImage(path, array))
		{
			this.deletedSignIds.Clear();
			this.overflowed = false;
			return false;
		}
		return true;
	}

	// Token: 0x060050D8 RID: 20696 RVA: 0x001F08E8 File Offset: 0x001EEAE8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryParseImage(string path, byte[] image)
	{
		int num;
		int num2;
		byte b;
		if (!SignStoreIO.TryValidateHeader("[DeletedSignStore]", path, image, 1313297220U, 1, 1, out num, out num2, out b))
		{
			return false;
		}
		try
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				using (MemoryStream memoryStream = new MemoryStream(image))
				{
					memoryStream.Position = 32L;
					pooledBinaryReader.SetBaseStream(memoryStream);
					for (int i = 0; i < num2; i++)
					{
						this.deletedSignIds.Add(GlobalSignId.FromStream(pooledBinaryReader));
					}
					if (memoryStream.Position != (long)image.Length)
					{
						Log.Warning(string.Format("{0} Ignoring '{1}': {2} trailing byte(s) after {3} record(s).", new object[]
						{
							"[DeletedSignStore]",
							path,
							(long)image.Length - memoryStream.Position,
							num2
						}));
						return false;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning("[DeletedSignStore] Ignoring '" + path + "': " + ex.Message);
			return false;
		}
		this.overflowed = (b & 1) > 0;
		if (this.overflowed && this.deletedSignIds.Count > 0)
		{
			Log.Warning("[DeletedSignStore] Unexpected case whilst parsing '" + path + "': overflowed flag has been set but the store still contains entries. Clearing entries now.");
			this.deletedSignIds.Clear();
		}
		return true;
	}

	// Token: 0x04003E50 RID: 15952
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint kMagic = 1313297220U;

	// Token: 0x04003E51 RID: 15953
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte kFlagOverflowed = 1;

	// Token: 0x04003E52 RID: 15954
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLogTag = "[DeletedSignStore]";

	// Token: 0x04003E53 RID: 15955
	public const int CurrentVersion = 1;

	// Token: 0x04003E54 RID: 15956
	public const int MinReadableVersion = 1;

	// Token: 0x04003E55 RID: 15957
	public const int cMaxDeletedSigns = 10000;

	// Token: 0x04003E56 RID: 15958
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cFileName = "deletedSigns.dat";

	// Token: 0x04003E57 RID: 15959
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cBackupExt = ".bak";

	// Token: 0x04003E58 RID: 15960
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<GlobalSignId> deletedSignIds = new HashSet<GlobalSignId>();

	// Token: 0x04003E59 RID: 15961
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overflowed;

	// Token: 0x04003E5A RID: 15962
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dirty;

	// Token: 0x04003E5B RID: 15963
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.TaskInfo writeTask;
}
