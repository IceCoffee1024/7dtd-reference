using System;
using System.IO;

// Token: 0x02000B9C RID: 2972
[PublicizedFrom(EAccessModifier.Internal)]
public class ChunkMemoryStreamWriter : MemoryStream
{
	// Token: 0x0600599A RID: 22938 RVA: 0x00226E5E File Offset: 0x0022505E
	public ChunkMemoryStreamWriter()
		: this(new byte[512000])
	{
	}

	// Token: 0x0600599B RID: 22939 RVA: 0x00226E70 File Offset: 0x00225070
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkMemoryStreamWriter(byte[] _buffer)
		: base(_buffer)
	{
		this.buffer = _buffer;
	}

	// Token: 0x0600599C RID: 22940 RVA: 0x00226E80 File Offset: 0x00225080
	public void Init(RegionFileAccessMultipleChunks _regionFileAccess, string _dir, int _x, int _z, string _ext)
	{
		this.dir = _dir;
		this.chunkX = _x;
		this.chunkZ = _z;
		this.ext = _ext;
		this.regionFileAccess = _regionFileAccess;
		this.Seek(0L, SeekOrigin.Begin);
	}

	// Token: 0x0600599D RID: 22941 RVA: 0x00226EB1 File Offset: 0x002250B1
	public override void Close()
	{
		this.regionFileAccess.Write(this.dir, this.chunkX, this.chunkZ, this.ext, this.buffer, (int)this.Position);
	}

	// Token: 0x040045AE RID: 17838
	[PublicizedFrom(EAccessModifier.Private)]
	public string dir;

	// Token: 0x040045AF RID: 17839
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x040045B0 RID: 17840
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;

	// Token: 0x040045B1 RID: 17841
	[PublicizedFrom(EAccessModifier.Private)]
	public string ext;

	// Token: 0x040045B2 RID: 17842
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessMultipleChunks regionFileAccess;

	// Token: 0x040045B3 RID: 17843
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] buffer;
}
