using System;

// Token: 0x02000B1C RID: 2844
public class CBCLayer : IMemoryPoolableObject
{
	// Token: 0x0600552B RID: 21803 RVA: 0x0020B9D8 File Offset: 0x00209BD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~CBCLayer()
	{
	}

	// Token: 0x0600552C RID: 21804 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x0600552D RID: 21805 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0600552E RID: 21806 RVA: 0x0020BA00 File Offset: 0x00209C00
	public void CopyFrom(CBCLayer _other)
	{
		Array.Copy(_other.data, this.data, this.data.Length);
	}

	// Token: 0x04004262 RID: 16994
	public readonly byte[] data = new byte[1024];

	// Token: 0x04004263 RID: 16995
	public static int InstanceCount;
}
