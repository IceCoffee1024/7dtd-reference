using System;
using System.IO;

// Token: 0x020014B4 RID: 5300
public class PooledExpandableMemoryStream : MemoryStream, IMemoryPoolableObject, IDisposable
{
	// Token: 0x0600A5F9 RID: 42489 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A5FA RID: 42490 RVA: 0x00229538 File Offset: 0x00227738
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x0600A5FB RID: 42491 RVA: 0x003DDAB8 File Offset: 0x003DBCB8
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A5FC RID: 42492 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A5FD RID: 42493 RVA: 0x003DDAC0 File Offset: 0x003DBCC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolMemoryStream.FreeSync(this);
	}
}
