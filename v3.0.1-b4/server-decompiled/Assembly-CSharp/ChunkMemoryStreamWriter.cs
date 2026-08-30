using System;
using System.IO;

// Token: 0x02000B78 RID: 2936
[PublicizedFrom(EAccessModifier.Internal)]
public class ChunkMemoryStreamWriter : MemoryStream
{
	// Token: 0x060058F8 RID: 22776 RVA: 0x00220EB6 File Offset: 0x0021F0B6
	public ChunkMemoryStreamWriter()
		: this(new byte[512000])
	{
	}

	// Token: 0x060058F9 RID: 22777 RVA: 0x00220EC8 File Offset: 0x0021F0C8
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkMemoryStreamWriter(byte[] _buffer)
		: base(_buffer)
	{
		this.buffer = _buffer;
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x00220ED8 File Offset: 0x0021F0D8
	public void Init(RegionFileAccessMultipleChunks _regionFileAccess, string _dir, int _x, int _z, string _ext)
	{
		this.dir = _dir;
		this.chunkX = _x;
		this.chunkZ = _z;
		this.ext = _ext;
		this.regionFileAccess = _regionFileAccess;
		this.Seek(0L, SeekOrigin.Begin);
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x00220F09 File Offset: 0x0021F109
	public override void Close()
	{
		this.regionFileAccess.Write(this.dir, this.chunkX, this.chunkZ, this.ext, this.buffer, (int)this.Position);
	}

	// Token: 0x040044D5 RID: 17621
	[PublicizedFrom(EAccessModifier.Private)]
	public string dir;

	// Token: 0x040044D6 RID: 17622
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x040044D7 RID: 17623
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;

	// Token: 0x040044D8 RID: 17624
	[PublicizedFrom(EAccessModifier.Private)]
	public string ext;

	// Token: 0x040044D9 RID: 17625
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessMultipleChunks regionFileAccess;

	// Token: 0x040044DA RID: 17626
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] buffer;
}
