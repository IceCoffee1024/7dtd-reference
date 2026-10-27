using System;
using System.IO;

// Token: 0x02000BA7 RID: 2983
[PublicizedFrom(EAccessModifier.Internal)]
public class ChunkMemoryStreamWriter : MemoryStream
{
	// Token: 0x060059BF RID: 22975 RVA: 0x002283BA File Offset: 0x002265BA
	public ChunkMemoryStreamWriter()
		: this(new byte[512000])
	{
	}

	// Token: 0x060059C0 RID: 22976 RVA: 0x002283CC File Offset: 0x002265CC
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkMemoryStreamWriter(byte[] _buffer)
		: base(_buffer)
	{
		this.buffer = _buffer;
	}

	// Token: 0x060059C1 RID: 22977 RVA: 0x002283DC File Offset: 0x002265DC
	public void Init(RegionFileAccessMultipleChunks _regionFileAccess, string _dir, int _x, int _z, string _ext)
	{
		this.dir = _dir;
		this.chunkX = _x;
		this.chunkZ = _z;
		this.ext = _ext;
		this.regionFileAccess = _regionFileAccess;
		this.Seek(0L, SeekOrigin.Begin);
	}

	// Token: 0x060059C2 RID: 22978 RVA: 0x0022840D File Offset: 0x0022660D
	public override void Close()
	{
		this.regionFileAccess.Write(this.dir, this.chunkX, this.chunkZ, this.ext, this.buffer, (int)this.Position);
	}

	// Token: 0x040045D3 RID: 17875
	[PublicizedFrom(EAccessModifier.Private)]
	public string dir;

	// Token: 0x040045D4 RID: 17876
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x040045D5 RID: 17877
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;

	// Token: 0x040045D6 RID: 17878
	[PublicizedFrom(EAccessModifier.Private)]
	public string ext;

	// Token: 0x040045D7 RID: 17879
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessMultipleChunks regionFileAccess;

	// Token: 0x040045D8 RID: 17880
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] buffer;
}
