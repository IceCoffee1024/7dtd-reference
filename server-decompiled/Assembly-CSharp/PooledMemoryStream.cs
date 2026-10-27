using System;
using System.IO;

// Token: 0x02000BB8 RID: 3000
public class PooledMemoryStream : MemoryStream, IMemoryPoolableObject
{
	// Token: 0x06005A04 RID: 23044 RVA: 0x00229510 File Offset: 0x00227710
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledMemoryStream()
	{
	}

	// Token: 0x06005A05 RID: 23045 RVA: 0x00229538 File Offset: 0x00227738
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x06005A06 RID: 23046 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x040045EF RID: 17903
	public static int InstanceCount;
}
