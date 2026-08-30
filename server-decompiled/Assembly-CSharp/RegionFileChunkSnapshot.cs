using System;

// Token: 0x02000B92 RID: 2962
public class RegionFileChunkSnapshot : IRegionFileChunkSnapshot, IMemoryPoolableObject
{
	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x060059A7 RID: 22951 RVA: 0x002247F0 File Offset: 0x002229F0
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

	// Token: 0x060059A8 RID: 22952 RVA: 0x00224804 File Offset: 0x00222A04
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

	// Token: 0x060059A9 RID: 22953 RVA: 0x0022493C File Offset: 0x00222B3C
	public void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ)
	{
		if (this.stream != null)
		{
			writer.WriteStreamCompressed(dir, chunkX, chunkZ, "7rg", this.stream);
		}
	}

	// Token: 0x060059AA RID: 22954 RVA: 0x0022495B File Offset: 0x00222B5B
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x060059AB RID: 22955 RVA: 0x00224963 File Offset: 0x00222B63
	public void Reset()
	{
		if (this.stream != null)
		{
			MemoryPools.poolMS.FreeSync(this.stream);
			this.stream = null;
		}
	}

	// Token: 0x04004582 RID: 17794
	public const string EXT = "7rg";

	// Token: 0x04004583 RID: 17795
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream stream;
}
