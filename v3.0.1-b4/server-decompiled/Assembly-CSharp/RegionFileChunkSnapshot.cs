using System;

// Token: 0x02000B80 RID: 2944
public class RegionFileChunkSnapshot : IRegionFileChunkSnapshot, IMemoryPoolableObject
{
	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x0600591F RID: 22815 RVA: 0x00221AD4 File Offset: 0x0021FCD4
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

	// Token: 0x06005920 RID: 22816 RVA: 0x00221AE8 File Offset: 0x0021FCE8
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

	// Token: 0x06005921 RID: 22817 RVA: 0x00221C20 File Offset: 0x0021FE20
	public void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ)
	{
		if (this.stream != null)
		{
			writer.WriteStreamCompressed(dir, chunkX, chunkZ, "7rg", this.stream);
		}
	}

	// Token: 0x06005922 RID: 22818 RVA: 0x00221C3F File Offset: 0x0021FE3F
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x06005923 RID: 22819 RVA: 0x00221C47 File Offset: 0x0021FE47
	public void Reset()
	{
		if (this.stream != null)
		{
			MemoryPools.poolMS.FreeSync(this.stream);
			this.stream = null;
		}
	}

	// Token: 0x040044E7 RID: 17639
	public const string EXT = "7rg";

	// Token: 0x040044E8 RID: 17640
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream stream;
}
