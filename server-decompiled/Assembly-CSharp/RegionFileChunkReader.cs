using System;
using System.IO;
using Noemax.GZip;

// Token: 0x02000B91 RID: 2961
public class RegionFileChunkReader
{
	// Token: 0x060059A4 RID: 22948 RVA: 0x00224554 File Offset: 0x00222754
	public RegionFileChunkReader(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
		this.loadChunkMemoryStream = new MemoryStream(65536);
		this.loadChunkReader = new PooledBinaryReader();
		this.loadChunkReader.SetBaseStream(this.loadChunkMemoryStream);
	}

	// Token: 0x060059A5 RID: 22949 RVA: 0x002245B8 File Offset: 0x002227B8
	public PooledBinaryReader readIntoLoadStream(string _dir, int chunkX, int chunkZ, string ext, out uint version)
	{
		Stream inputStream = this.regionFileAccess.GetInputStream(_dir, chunkX, chunkZ, ext);
		if (inputStream == null)
		{
			version = 0U;
			return null;
		}
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			pooledBinaryReader.SetBaseStream(inputStream);
			for (int i = 0; i < this.magicBytes.Length; i++)
			{
				this.magicBytes[i] = pooledBinaryReader.ReadByte();
			}
			if (this.magicBytes[0] != 116 || this.magicBytes[1] != 116 || this.magicBytes[2] != 99 || this.magicBytes[3] != 0)
			{
				throw new Exception("Wrong chunk header!");
			}
			version = pooledBinaryReader.ReadUInt32();
		}
		if (this.zipLoadStream == null || this.innerLoadStream != inputStream)
		{
			if (this.zipLoadStream != null)
			{
				Log.Warning("RFM.Load: Creating new DeflateStream, underlying Stream changed");
			}
			this.zipLoadStream = new DeflateInputStream(inputStream, true);
			this.innerLoadStream = inputStream;
		}
		this.zipLoadStream.Restart();
		Stream stream = this.zipLoadStream;
		this.loadChunkMemoryStream.SetLength(0L);
		StreamUtils.StreamCopy(stream, this.loadChunkMemoryStream, this.loadBuffer, true);
		this.loadChunkMemoryStream.Position = 0L;
		inputStream.Close();
		return this.loadChunkReader;
	}

	// Token: 0x060059A6 RID: 22950 RVA: 0x002246F4 File Offset: 0x002228F4
	public void WriteBackup(string dir, int chunkX, int chunkZ)
	{
		using (Stream stream = SdFile.OpenWrite(string.Concat(new string[]
		{
			dir,
			"/error_backup_",
			chunkX.ToString(),
			"_",
			chunkZ.ToString(),
			".comp.bak"
		})))
		{
			this.innerLoadStream.Position = 0L;
			StreamUtils.StreamCopy(this.innerLoadStream, stream, null, true);
		}
		using (Stream stream2 = SdFile.OpenWrite(string.Concat(new string[]
		{
			dir,
			"/error_backup_",
			chunkX.ToString(),
			"_",
			chunkZ.ToString(),
			".uncomp.bak"
		})))
		{
			this.loadChunkMemoryStream.Position = 0L;
			StreamUtils.StreamCopy(this.loadChunkMemoryStream, stream2, null, true);
		}
	}

	// Token: 0x0400457B RID: 17787
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x0400457C RID: 17788
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream innerLoadStream;

	// Token: 0x0400457D RID: 17789
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateInputStream zipLoadStream;

	// Token: 0x0400457E RID: 17790
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream loadChunkMemoryStream;

	// Token: 0x0400457F RID: 17791
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryReader loadChunkReader;

	// Token: 0x04004580 RID: 17792
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] loadBuffer = new byte[4096];

	// Token: 0x04004581 RID: 17793
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] magicBytes = new byte[4];
}
