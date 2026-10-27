using System;
using System.IO;

// Token: 0x02000BA8 RID: 2984
[PublicizedFrom(EAccessModifier.Internal)]
public class ChunkMemoryStreamReader : MemoryStream
{
	// Token: 0x060059C3 RID: 22979 RVA: 0x0022843F File Offset: 0x0022663F
	public ChunkMemoryStreamReader()
		: this(new byte[512000])
	{
	}

	// Token: 0x060059C4 RID: 22980 RVA: 0x00228451 File Offset: 0x00226651
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkMemoryStreamReader(byte[] _buffer)
		: base(_buffer)
	{
	}

	// Token: 0x060059C5 RID: 22981 RVA: 0x0022845A File Offset: 0x0022665A
	public override void Close()
	{
		this.Seek(0L, SeekOrigin.Begin);
	}
}
