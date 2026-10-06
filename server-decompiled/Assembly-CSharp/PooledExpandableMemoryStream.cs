using System;
using System.IO;

// Token: 0x020014A2 RID: 5282
public class PooledExpandableMemoryStream : MemoryStream, IMemoryPoolableObject, IDisposable
{
	// Token: 0x0600A5A1 RID: 42401 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A5A2 RID: 42402 RVA: 0x00227FDC File Offset: 0x002261DC
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x0600A5A3 RID: 42403 RVA: 0x003DB6B4 File Offset: 0x003D98B4
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A5A4 RID: 42404 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A5A5 RID: 42405 RVA: 0x003DB6BC File Offset: 0x003D98BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolMemoryStream.FreeSync(this);
	}
}
