using System;
using System.IO;

// Token: 0x02000B9D RID: 2973
[PublicizedFrom(EAccessModifier.Internal)]
public class ChunkMemoryStreamReader : MemoryStream
{
	// Token: 0x0600599E RID: 22942 RVA: 0x00226EE3 File Offset: 0x002250E3
	public ChunkMemoryStreamReader()
		: this(new byte[512000])
	{
	}

	// Token: 0x0600599F RID: 22943 RVA: 0x00226EF5 File Offset: 0x002250F5
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkMemoryStreamReader(byte[] _buffer)
		: base(_buffer)
	{
	}

	// Token: 0x060059A0 RID: 22944 RVA: 0x00226EFE File Offset: 0x002250FE
	public override void Close()
	{
		this.Seek(0L, SeekOrigin.Begin);
	}
}
