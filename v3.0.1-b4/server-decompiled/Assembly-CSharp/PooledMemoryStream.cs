using System;
using System.IO;

// Token: 0x02000B89 RID: 2953
public class PooledMemoryStream : MemoryStream, IMemoryPoolableObject
{
	// Token: 0x0600593D RID: 22845 RVA: 0x00222008 File Offset: 0x00220208
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledMemoryStream()
	{
	}

	// Token: 0x0600593E RID: 22846 RVA: 0x00222030 File Offset: 0x00220230
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x0600593F RID: 22847 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x040044F1 RID: 17649
	public static int InstanceCount;
}
