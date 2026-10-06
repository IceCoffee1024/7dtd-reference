using System;

// Token: 0x02000CE8 RID: 3304
public struct ScopedChunkWriteAccess : IDisposable
{
	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x060064D3 RID: 25811 RVA: 0x0027A824 File Offset: 0x00278A24
	// (set) Token: 0x060064D4 RID: 25812 RVA: 0x0027A82C File Offset: 0x00278A2C
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064D5 RID: 25813 RVA: 0x0027A835 File Offset: 0x00278A35
	public ScopedChunkWriteAccess(Chunk chunk)
	{
		this.Chunk = chunk;
		Chunk chunk2 = this.Chunk;
		if (chunk2 == null)
		{
			return;
		}
		chunk2.EnterWriteLock();
	}

	// Token: 0x060064D6 RID: 25814 RVA: 0x0027A84E File Offset: 0x00278A4E
	public void Dispose()
	{
		Chunk chunk = this.Chunk;
		if (chunk == null)
		{
			return;
		}
		chunk.ExitWriteLock();
	}
}
