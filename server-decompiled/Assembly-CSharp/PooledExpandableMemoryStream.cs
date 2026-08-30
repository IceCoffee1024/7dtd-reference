using System;
using System.IO;

// Token: 0x02001482 RID: 5250
public class PooledExpandableMemoryStream : MemoryStream, IMemoryPoolableObject, IDisposable
{
	// Token: 0x0600A4F3 RID: 42227 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A4F4 RID: 42228 RVA: 0x00224D4C File Offset: 0x00222F4C
	public void Reset()
	{
		this.SetLength(0L);
	}

	// Token: 0x0600A4F5 RID: 42229 RVA: 0x003D6A28 File Offset: 0x003D4C28
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A4F6 RID: 42230 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A4F7 RID: 42231 RVA: 0x003D6A30 File Offset: 0x003D4C30
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolMemoryStream.FreeSync(this);
	}
}
