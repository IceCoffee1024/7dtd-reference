using System;

// Token: 0x02000B81 RID: 2945
public class ChunkSnapshotUtil : IRegionFileChunkSnapshotUtil
{
	// Token: 0x06005925 RID: 22821 RVA: 0x00221C68 File Offset: 0x0021FE68
	public ChunkSnapshotUtil(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
		this.chunkReader = new RegionFileChunkReader(regionFileAccess);
		this.chunkWriter = new RegionFileChunkWriter(regionFileAccess);
	}

	// Token: 0x06005926 RID: 22822 RVA: 0x00221C9F File Offset: 0x0021FE9F
	public IRegionFileChunkSnapshot TakeSnapshot(Chunk chunk, bool saveIfUnchanged)
	{
		RegionFileChunkSnapshot regionFileChunkSnapshot = this.poolSnapshots.AllocSync(true);
		regionFileChunkSnapshot.Update(chunk, saveIfUnchanged);
		return regionFileChunkSnapshot;
	}

	// Token: 0x06005927 RID: 22823 RVA: 0x00221CB5 File Offset: 0x0021FEB5
	public void WriteSnapshot(IRegionFileChunkSnapshot snapshot, string dir, int chunkX, int chunkZ)
	{
		snapshot.Write(this.chunkWriter, dir, chunkX, chunkZ);
	}

	// Token: 0x06005928 RID: 22824 RVA: 0x00221CC8 File Offset: 0x0021FEC8
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

	// Token: 0x06005929 RID: 22825 RVA: 0x00221E14 File Offset: 0x00220014
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

	// Token: 0x0600592A RID: 22826 RVA: 0x00221E56 File Offset: 0x00220056
	public void Cleanup()
	{
		this.poolSnapshots.Cleanup();
	}

	// Token: 0x040044E9 RID: 17641
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040044EA RID: 17642
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkReader chunkReader;

	// Token: 0x040044EB RID: 17643
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileChunkWriter chunkWriter;

	// Token: 0x040044EC RID: 17644
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<RegionFileChunkSnapshot> poolSnapshots = new MemoryPooledObject<RegionFileChunkSnapshot>(255);
}
