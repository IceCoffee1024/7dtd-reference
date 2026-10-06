using System;

// Token: 0x02000BA4 RID: 2980
public class RegionFileChunkSnapshot : IRegionFileChunkSnapshot, IMemoryPoolableObject
{
	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x060059C1 RID: 22977 RVA: 0x00227A80 File Offset: 0x00225C80
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

	// Token: 0x060059C2 RID: 22978 RVA: 0x00227A94 File Offset: 0x00225C94
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

	// Token: 0x060059C3 RID: 22979 RVA: 0x00227BCC File Offset: 0x00225DCC
	public void Write(RegionFileChunkWriter writer, string dir, int chunkX, int chunkZ)
	{
		if (this.stream != null)
		{
			writer.WriteStreamCompressed(dir, chunkX, chunkZ, "7rg", this.stream);
		}
	}

	// Token: 0x060059C4 RID: 22980 RVA: 0x00227BEB File Offset: 0x00225DEB
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x060059C5 RID: 22981 RVA: 0x00227BF3 File Offset: 0x00225DF3
	public void Reset()
	{
		if (this.stream != null)
		{
			MemoryPools.poolMS.FreeSync(this.stream);
			this.stream = null;
		}
	}

	// Token: 0x040045C0 RID: 17856
	public const string EXT = "7rg";

	// Token: 0x040045C1 RID: 17857
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream stream;
}
