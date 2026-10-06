using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A75 RID: 2677
public class DeletedSignStore
{
	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x060050A5 RID: 20645 RVA: 0x001EF377 File Offset: 0x001ED577
	public bool Overflowed
	{
		get
		{
			return this.overflowed;
		}
	}

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x060050A6 RID: 20646 RVA: 0x001EF37F File Offset: 0x001ED57F
	public int Count
	{
		get
		{
			return this.deletedSignIds.Count;
		}
	}

	// Token: 0x060050A7 RID: 20647 RVA: 0x001EF38C File Offset: 0x001ED58C
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetStorePath()
	{
		return GameIO.GetSaveGameDir() + "/deletedSigns.dat";
	}

	// Token: 0x060050A8 RID: 20648 RVA: 0x001EF39D File Offset: 0x001ED59D
	public bool IsDeleted(GlobalSignId signId)
	{
		if (this.overflowed)
		{
			return SignDataManager.IsPlayerLibraryId(signId.libraryId);
		}
		return this.deletedSignIds.Contains(signId);
	}

	// Token: 0x060050A9 RID: 20649 RVA: 0x001EF3C0 File Offset: 0x001ED5C0
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

	// Token: 0x060050AA RID: 20650 RVA: 0x001EF433 File Offset: 0x001ED633
	public void Remove(GlobalSignId signId)
	{
		if (this.deletedSignIds.Remove(signId))
		{
			this.dirty = true;
		}
	}

	// Token: 0x060050AB RID: 20651 RVA: 0x001EF44C File Offset: 0x001ED64C
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

	// Token: 0x060050AC RID: 20652 RVA: 0x001EF51C File Offset: 0x001ED71C
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

	// Token: 0x060050AD RID: 20653 RVA: 0x001EF5A7 File Offset: 0x001ED7A7
	public void Update()
	{
		this.CheckCompletedWrite();
		if (this.dirty && this.writeTask == null)
		{
			this.BeginWrite(false);
		}
	}

	// Token: 0x060050AE RID: 20654 RVA: 0x001EF5C6 File Offset: 0x001ED7C6
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

	// Token: 0x060050AF RID: 20655 RVA: 0x001EF5FC File Offset: 0x001ED7FC
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

	// Token: 0x060050B0 RID: 20656 RVA: 0x001EF63C File Offset: 0x001ED83C
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

	// Token: 0x060050B1 RID: 20657 RVA: 0x001EF6CC File Offset: 0x001ED8CC
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

	// Token: 0x060050B2 RID: 20658 RVA: 0x001EF774 File Offset: 0x001ED974
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

	// Token: 0x060050B3 RID: 20659 RVA: 0x001EF7CC File Offset: 0x001ED9CC
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

	// Token: 0x060050B4 RID: 20660 RVA: 0x001EF80C File Offset: 0x001EDA0C
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

	// Token: 0x04003E2F RID: 15919
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint kMagic = 1313297220U;

	// Token: 0x04003E30 RID: 15920
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte kFlagOverflowed = 1;

	// Token: 0x04003E31 RID: 15921
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLogTag = "[DeletedSignStore]";

	// Token: 0x04003E32 RID: 15922
	public const int CurrentVersion = 1;

	// Token: 0x04003E33 RID: 15923
	public const int MinReadableVersion = 1;

	// Token: 0x04003E34 RID: 15924
	public const int cMaxDeletedSigns = 10000;

	// Token: 0x04003E35 RID: 15925
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cFileName = "deletedSigns.dat";

	// Token: 0x04003E36 RID: 15926
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cBackupExt = ".bak";

	// Token: 0x04003E37 RID: 15927
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<GlobalSignId> deletedSignIds = new HashSet<GlobalSignId>();

	// Token: 0x04003E38 RID: 15928
	[PublicizedFrom(EAccessModifier.Private)]
	public bool overflowed;

	// Token: 0x04003E39 RID: 15929
	[PublicizedFrom(EAccessModifier.Private)]
	public bool dirty;

	// Token: 0x04003E3A RID: 15930
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.TaskInfo writeTask;
}
