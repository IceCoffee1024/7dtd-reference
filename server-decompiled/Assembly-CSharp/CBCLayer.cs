using System;

// Token: 0x02000B27 RID: 2855
public class CBCLayer : IMemoryPoolableObject
{
	// Token: 0x06005550 RID: 21840 RVA: 0x0020CF54 File Offset: 0x0020B154
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~CBCLayer()
	{
	}

	// Token: 0x06005551 RID: 21841 RVA: 0x000027FC File Offset: 0x000009FC
	public void Reset()
	{
	}

	// Token: 0x06005552 RID: 21842 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06005553 RID: 21843 RVA: 0x0020CF7C File Offset: 0x0020B17C
	public void CopyFrom(CBCLayer _other)
	{
		Array.Copy(_other.data, this.data, this.data.Length);
	}

	// Token: 0x04004287 RID: 17031
	public readonly byte[] data = new byte[1024];

	// Token: 0x04004288 RID: 17032
	public static int InstanceCount;
}
