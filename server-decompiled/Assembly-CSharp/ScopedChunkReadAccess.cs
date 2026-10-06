using System;

// Token: 0x02000CE7 RID: 3303
public struct ScopedChunkReadAccess : IDisposable
{
	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x060064CF RID: 25807 RVA: 0x0027A7E8 File Offset: 0x002789E8
	// (set) Token: 0x060064D0 RID: 25808 RVA: 0x0027A7F0 File Offset: 0x002789F0
	public Chunk Chunk
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060064D1 RID: 25809 RVA: 0x0027A7F9 File Offset: 0x002789F9
	public ScopedChunkReadAccess(Chunk chunk)
	{
		this.Chunk = chunk;
		Chunk chunk2 = this.Chunk;
		if (chunk2 == null)
		{
			return;
		}
		chunk2.EnterReadLock();
	}

	// Token: 0x060064D2 RID: 25810 RVA: 0x0027A812 File Offset: 0x00278A12
	public void Dispose()
	{
		Chunk chunk = this.Chunk;
		if (chunk == null)
		{
			return;
		}
		chunk.ExitReadLock();
	}
}
