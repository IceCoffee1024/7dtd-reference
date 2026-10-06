using System;
using System.IO;
using Noemax.GZip;

// Token: 0x02000BA3 RID: 2979
public class RegionFileChunkReader
{
	// Token: 0x060059BE RID: 22974 RVA: 0x002277E4 File Offset: 0x002259E4
	public RegionFileChunkReader(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
		this.loadChunkMemoryStream = new MemoryStream(65536);
		this.loadChunkReader = new PooledBinaryReader();
		this.loadChunkReader.SetBaseStream(this.loadChunkMemoryStream);
	}

	// Token: 0x060059BF RID: 22975 RVA: 0x00227848 File Offset: 0x00225A48
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

	// Token: 0x060059C0 RID: 22976 RVA: 0x00227984 File Offset: 0x00225B84
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

	// Token: 0x040045B9 RID: 17849
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040045BA RID: 17850
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream innerLoadStream;

	// Token: 0x040045BB RID: 17851
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateInputStream zipLoadStream;

	// Token: 0x040045BC RID: 17852
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream loadChunkMemoryStream;

	// Token: 0x040045BD RID: 17853
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryReader loadChunkReader;

	// Token: 0x040045BE RID: 17854
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] loadBuffer = new byte[4096];

	// Token: 0x040045BF RID: 17855
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] magicBytes = new byte[4];
}
