using System;

// Token: 0x02000C54 RID: 3156
public class ThreadContainer
{
	// Token: 0x06005F7D RID: 24445 RVA: 0x0025F378 File Offset: 0x0025D578
	public ThreadContainer(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.Init(_TerExt, _DChunk, _BMesh, _WasReset);
	}

	// Token: 0x06005F7E RID: 24446 RVA: 0x0025F392 File Offset: 0x0025D592
	public ThreadContainer()
	{
		this.TerExt = null;
		this.DChunk = null;
		this.BMesh = null;
		this.WasReset = false;
	}

	// Token: 0x06005F7F RID: 24447 RVA: 0x0025F3BD File Offset: 0x0025D5BD
	public void Init(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.TerExt = _TerExt;
		this.DChunk = _DChunk;
		this.BMesh = _BMesh;
		this.WasReset = _WasReset;
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x0025F3DC File Offset: 0x0025D5DC
	public void ThreadExtraWork()
	{
		this.TerExt.ThreadExtraWork(this.DChunk, this.BMesh, this.WasReset);
	}

	// Token: 0x06005F81 RID: 24449 RVA: 0x0025F3FB File Offset: 0x0025D5FB
	public void MainExtraWork()
	{
		this.TerExt.MainExtraWork(this.DChunk, this.BMesh);
	}

	// Token: 0x06005F82 RID: 24450 RVA: 0x0025F414 File Offset: 0x0025D614
	public void Clear(bool IsClearItem)
	{
		if (IsClearItem)
		{
			this.TerExt = null;
			this.DChunk = null;
			this.BMesh = null;
			this.WasReset = false;
		}
	}

	// Token: 0x04004B15 RID: 19221
	public int DEBUG_TCId = -1;

	// Token: 0x04004B16 RID: 19222
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantTerrain TerExt;

	// Token: 0x04004B17 RID: 19223
	public DistantChunk DChunk;

	// Token: 0x04004B18 RID: 19224
	public DistantChunkBasicMesh BMesh;

	// Token: 0x04004B19 RID: 19225
	public bool WasReset;
}
