using System;
using System.IO;

// Token: 0x02000B9B RID: 2971
public class PooledMemoryStream : MemoryStream, IMemoryPoolableObject
{
	// Token: 0x060059C5 RID: 22981 RVA: 0x00224D24 File Offset: 0x00222F24
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledMemoryStream()
	{
	}

	// Token: 0x060059C6 RID: 22982 RVA: 0x00224D4C File Offset: 0x00222F4C
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x060059C7 RID: 22983 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0400458C RID: 17804
	public static int InstanceCount;
}
