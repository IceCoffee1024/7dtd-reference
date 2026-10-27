using System;

// Token: 0x02000BB0 RID: 2992
public class ChunkSnapshotUtil : IRegionFileChunkSnapshotUtil
{
	// Token: 0x060059EC RID: 23020 RVA: 0x00229170 File Offset: 0x00227370
	public ChunkSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
		this.chunkReader = new RegionFileChunkReader(regionFileAccess);
		this.chunkWriter = new RegionFileChunkWriter(regionFileAccess);
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x002291A7 File Offset: 0x002273A7
	public IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged)
	{
		RegionFileChunkSnapshot regionFileChunkSnapshot = this.poolSnapshots.AllocSync(true);
		regionFileChunkSnapshot.Update(chunk, saveIfUnchanged);
		return regionFileChunkSnapshot;
	}

	// Token: 0x060059EE RID: 23022 RVA: 0x002291BD File Offset: 0x002273BD
	public void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ)
	{
		snapshot.Write(this.chunkWriter, dir, chunkX, chunkZ);
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x002291D0 File Offset: 0x002273D0
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

	// Token: 0x060059F0 RID: 23024 RVA: 0x0022931C File Offset: 0x0022751C
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

	// Token: 0x060059F1 RID: 23025 RVA: 0x0022935E File Offset: 0x0022755E
	public void Cleanup()
	{
		this.poolSnapshots.Cleanup();
	}

	// Token: 0x040045E7 RID: 17895
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040045E8 RID: 17896
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkReader chunkReader;

	// Token: 0x040045E9 RID: 17897
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkWriter chunkWriter;

	// Token: 0x040045EA RID: 17898
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<RegionFileChunkSnapshot> poolSnapshots = new MemoryPooledObject<RegionFileChunkSnapshot>(255);
}
