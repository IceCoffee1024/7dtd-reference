using System;
using System.IO;

// Token: 0x02000B8B RID: 2955
[PublicizedFrom(EAccessModifier.Internal)]
public class ChunkMemoryStreamReader : MemoryStream
{
	// Token: 0x06005984 RID: 22916 RVA: 0x00223C57 File Offset: 0x00221E57
	public ChunkMemoryStreamReader()
		: this(new byte[512000])
	{
	}

	// Token: 0x06005985 RID: 22917 RVA: 0x00223C69 File Offset: 0x00221E69
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkMemoryStreamReader(byte[] _buffer)
		: base(_buffer)
	{
	}

	// Token: 0x06005986 RID: 22918 RVA: 0x00223C72 File Offset: 0x00221E72
	public override void Close()
	{
		this.Seek(0L, SeekOrigin.Begin);
	}
}
