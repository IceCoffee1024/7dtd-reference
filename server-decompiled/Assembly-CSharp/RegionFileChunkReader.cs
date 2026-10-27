using System;
using System.IO;
using Noemax.GZip;

// Token: 0x02000BAE RID: 2990
public class RegionFileChunkReader
{
	// Token: 0x060059E3 RID: 23011 RVA: 0x00228D40 File Offset: 0x00226F40
	public RegionFileChunkReader(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
		this.loadChunkMemoryStream = new MemoryStream(65536);
		this.loadChunkReader = new PooledBinaryReader();
		this.loadChunkReader.SetBaseStream(this.loadChunkMemoryStream);
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x00228DA4 File Offset: 0x00226FA4
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

	// Token: 0x060059E5 RID: 23013 RVA: 0x00228EE0 File Offset: 0x002270E0
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

	// Token: 0x040045DE RID: 17886
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040045DF RID: 17887
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream innerLoadStream;

	// Token: 0x040045E0 RID: 17888
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateInputStream zipLoadStream;

	// Token: 0x040045E1 RID: 17889
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream loadChunkMemoryStream;

	// Token: 0x040045E2 RID: 17890
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledBinaryReader loadChunkReader;

	// Token: 0x040045E3 RID: 17891
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] loadBuffer = new byte[4096];

	// Token: 0x040045E4 RID: 17892
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] magicBytes = new byte[4];
}
