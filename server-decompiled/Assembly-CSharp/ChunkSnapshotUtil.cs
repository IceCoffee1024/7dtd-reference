using System;

// Token: 0x02000B93 RID: 2963
public class ChunkSnapshotUtil : IRegionFileChunkSnapshotUtil
{
	// Token: 0x060059AD RID: 22957 RVA: 0x00224984 File Offset: 0x00222B84
	public ChunkSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
		this.chunkReader = new RegionFileChunkReader(regionFileAccess);
		this.chunkWriter = new RegionFileChunkWriter(regionFileAccess);
	}

	// Token: 0x060059AE RID: 22958 RVA: 0x002249BB File Offset: 0x00222BBB
	public IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged)
	{
		RegionFileChunkSnapshot regionFileChunkSnapshot = this.poolSnapshots.AllocSync(true);
		regionFileChunkSnapshot.Update(chunk, saveIfUnchanged);
		return regionFileChunkSnapshot;
	}

	// Token: 0x060059AF RID: 22959 RVA: 0x002249D1 File Offset: 0x00222BD1
	public void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ)
	{
		snapshot.Write(this.chunkWriter, dir, chunkX, chunkZ);
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x002249E4 File Offset: 0x00222BE4
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

	// Token: 0x060059B1 RID: 22961 RVA: 0x00224B30 File Offset: 0x00222D30
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

	// Token: 0x060059B2 RID: 22962 RVA: 0x00224B72 File Offset: 0x00222D72
	public void Cleanup()
	{
		this.poolSnapshots.Cleanup();
	}

	// Token: 0x04004584 RID: 17796
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x04004585 RID: 17797
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkReader chunkReader;

	// Token: 0x04004586 RID: 17798
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkWriter chunkWriter;

	// Token: 0x04004587 RID: 17799
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<RegionFileChunkSnapshot> poolSnapshots = new MemoryPooledObject<RegionFileChunkSnapshot>(255);
}
