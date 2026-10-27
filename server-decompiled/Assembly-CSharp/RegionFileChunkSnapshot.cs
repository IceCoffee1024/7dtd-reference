using System;

// Token: 0x02000BAF RID: 2991
public class RegionFileChunkSnapshot : IRegionFileChunkSnapshot, IMemoryPoolableObject
{
	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x060059E6 RID: 23014 RVA: 0x00228FDC File Offset: 0x002271DC
	public long Size
	{
		get
		{
			PooledMemoryStream pooledMemoryStream = this.stream;
			if (pooledMemoryStream == null)
			{
				return 0L;
			}
			return pooledMemoryStream.Length;
		}
	}

	// Token: 0x060059E7 RID: 23015 RVA: 0x00228FF0 File Offset: 0x002271F0
	public void Update(Chunk chunk, bool saveIfUnchanged)
	{
		if (saveIfUnchanged || chunk.NeedsSaving)
		{
			if (this.stream != null)
			{
				this.stream.SetLength(0L);
			}
			else
			{
				this.stream = MemoryPools.poolMS.AllocSync(true);
			}
			try
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(this.stream);
					pooledBinaryWriter.Write(116);
					pooledBinaryWriter.Write(116);
					pooledBinaryWriter.Write(99);
					pooledBinaryWriter.Write(0);
					pooledBinaryWriter.Write(47U);
					chunk.save(pooledBinaryWriter);
					this.stream.Position = 0L;
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new string[]
				{
					"Error writing blocks to stream (chunkX=",
					chunk.X.ToString(),
					" chunkZ=",
					chunk.Z.ToString(),
					"): ",
					ex.Message,
					"\nStackTrace: ",
					ex.StackTrace
				}));
				MemoryPools.poolMS.FreeSync(this.stream);
			}
		}
	}

	// Token: 0x060059E8 RID: 23016 RVA: 0x00229128 File Offset: 0x00227328
	public void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ)
	{
		if (this.stream != null)
		{
			writer.WriteStreamCompressed(dir, chunkX, chunkZ, "7rg", this.stream);
		}
	}

	// Token: 0x060059E9 RID: 23017 RVA: 0x00229147 File Offset: 0x00227347
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x060059EA RID: 23018 RVA: 0x0022914F File Offset: 0x0022734F
	public void Reset()
	{
		if (this.stream != null)
		{
			MemoryPools.poolMS.FreeSync(this.stream);
			this.stream = null;
		}
	}

	// Token: 0x040045E5 RID: 17893
	public const string EXT = "7rg";

	// Token: 0x040045E6 RID: 17894
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream stream;
}
