using System;

// Token: 0x02000BA5 RID: 2981
public class ChunkSnapshotUtil : IRegionFileChunkSnapshotUtil
{
	// Token: 0x060059C7 RID: 22983 RVA: 0x00227C14 File Offset: 0x00225E14
	public ChunkSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
		this.chunkReader = new RegionFileChunkReader(regionFileAccess);
		this.chunkWriter = new RegionFileChunkWriter(regionFileAccess);
	}

	// Token: 0x060059C8 RID: 22984 RVA: 0x00227C4B File Offset: 0x00225E4B
	public IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged)
	{
		RegionFileChunkSnapshot regionFileChunkSnapshot = this.poolSnapshots.AllocSync(true);
		regionFileChunkSnapshot.Update(chunk, saveIfUnchanged);
		return regionFileChunkSnapshot;
	}

	// Token: 0x060059C9 RID: 22985 RVA: 0x00227C61 File Offset: 0x00225E61
	public void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ)
	{
		snapshot.Write(this.chunkWriter, dir, chunkX, chunkZ);
	}

	// Token: 0x060059CA RID: 22986 RVA: 0x00227C74 File Offset: 0x00225E74
	public Chunk LoadChunk(string dir, long key)
	{
		int num = WorldChunkCache.extractX(key);
		int num2 = WorldChunkCache.extractZ(key);
		try
		{
			uint num3;
			PooledBinaryReader pooledBinaryReader = this.chunkReader.readIntoLoadStream(dir, num, num2, "7rg", out num3);
			if (pooledBinaryReader == null)
			{
				return null;
			}
			Chunk chunk = MemoryPools.PoolChunks.AllocSync(true);
			chunk.load(pooledBinaryReader, num3);
			chunk.NeedsRegeneration = true;
			return chunk;
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[]
			{
				"EXCEPTION: In load chunk (chunkX=",
				num.ToString(),
				" chunkZ=",
				num2.ToString(),
				")"
			}));
			Log.Exception(ex);
			try
			{
				this.chunkReader.WriteBackup(dir, num, num2);
			}
			catch (Exception ex2)
			{
				Log.Error("Error backing up data:");
				Log.Exception(ex2);
			}
		}
		try
		{
			this.regionFileAccess.Remove(dir, num, num2);
		}
		catch (Exception ex3)
		{
			Log.Error(string.Concat(new string[]
			{
				"In remove chunk (chunkX=",
				num.ToString(),
				" chunkZ=",
				num2.ToString(),
				"):",
				ex3.Message
			}));
		}
		return null;
	}

	// Token: 0x060059CB RID: 22987 RVA: 0x00227DC0 File Offset: 0x00225FC0
	public void Free(IRegionFileChunkSnapshot iSnapshot)
	{
		if (iSnapshot == null)
		{
			return;
		}
		RegionFileChunkSnapshot regionFileChunkSnapshot = iSnapshot as RegionFileChunkSnapshot;
		if (regionFileChunkSnapshot != null)
		{
			this.poolSnapshots.FreeSync(regionFileChunkSnapshot);
			return;
		}
		Log.Error("Attempting to free snapshot of wrong type. Expected: RegionFileChunkSnapshot, Actual: " + iSnapshot.GetType().Name);
	}

	// Token: 0x060059CC RID: 22988 RVA: 0x00227E02 File Offset: 0x00226002
	public void Cleanup()
	{
		this.poolSnapshots.Cleanup();
	}

	// Token: 0x040045C2 RID: 17858
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040045C3 RID: 17859
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkReader chunkReader;

	// Token: 0x040045C4 RID: 17860
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkWriter chunkWriter;

	// Token: 0x040045C5 RID: 17861
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<RegionFileChunkSnapshot> poolSnapshots = new MemoryPooledObject<RegionFileChunkSnapshot>(255);
}
