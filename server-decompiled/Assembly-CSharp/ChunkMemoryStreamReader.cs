using System;
using System.IO;

// Token: 0x02000B79 RID: 2937
[PublicizedFrom(EAccessModifier.Internal)]
public class ChunkMemoryStreamReader : MemoryStream
{
	// Token: 0x060058FC RID: 22780 RVA: 0x00220F3B File Offset: 0x0021F13B
	public ChunkMemoryStreamReader()
		: this(new byte[512000])
	{
	}

	// Token: 0x060058FD RID: 22781 RVA: 0x00220F4D File Offset: 0x0021F14D
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkMemoryStreamReader(byte[] _buffer)
		: base(_buffer)
	{
	}

	// Token: 0x060058FE RID: 22782 RVA: 0x00220F56 File Offset: 0x0021F156
	public override void Close()
	{
		this.Seek(0L, SeekOrigin.Begin);
	}
}
