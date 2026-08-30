using System;
using System.IO;

// Token: 0x02001467 RID: 5223
public class PooledExpandableMemoryStream : MemoryStream, IMemoryPoolableObject, IDisposable
{
	// Token: 0x0600A3F6 RID: 41974 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A3F7 RID: 41975 RVA: 0x00222030 File Offset: 0x00220230
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x0600A3F8 RID: 41976 RVA: 0x003D16E4 File Offset: 0x003CF8E4
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A3F9 RID: 41977 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A3FA RID: 41978 RVA: 0x003D16EC File Offset: 0x003CF8EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolMemoryStream.FreeSync(this);
	}
}
