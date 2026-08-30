using System;

// Token: 0x02000AF8 RID: 2808
public class CBCLayer : IMemoryPoolableObject
{
	// Token: 0x0600548A RID: 21642 RVA: 0x00205A4C File Offset: 0x00203C4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~CBCLayer()
	{
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x0600548C RID: 21644 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0600548D RID: 21645 RVA: 0x00205A74 File Offset: 0x00203C74
	public void CopyFrom(CBCLayer _other)
	{
		Array.Copy(_other.data, this.data, this.data.Length);
	}

	// Token: 0x0400418A RID: 16778
	public readonly byte[] data = new byte[1024];

	// Token: 0x0400418B RID: 16779
	public static int InstanceCount;
}
