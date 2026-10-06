using System;
using System.IO;

// Token: 0x02000BAD RID: 2989
public class PooledMemoryStream : MemoryStream, IMemoryPoolableObject
{
	// Token: 0x060059DF RID: 23007 RVA: 0x00227FB4 File Offset: 0x002261B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledMemoryStream()
	{
	}

	// Token: 0x060059E0 RID: 23008 RVA: 0x00227FDC File Offset: 0x002261DC
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x060059E1 RID: 23009 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x040045CA RID: 17866
	public static int InstanceCount;
}
