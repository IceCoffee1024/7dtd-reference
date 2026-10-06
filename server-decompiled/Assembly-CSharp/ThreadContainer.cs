using System;

// Token: 0x02000C49 RID: 3145
public class ThreadContainer
{
	// Token: 0x06005F58 RID: 24408 RVA: 0x0025DE50 File Offset: 0x0025C050
	public ThreadContainer(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.Init(_TerExt, _DChunk, _BMesh, _WasReset);
	}

	// Token: 0x06005F59 RID: 24409 RVA: 0x0025DE6A File Offset: 0x0025C06A
	public ThreadContainer()
	{
		this.TerExt = null;
		this.DChunk = null;
		this.BMesh = null;
		this.WasReset = false;
	}

	// Token: 0x06005F5A RID: 24410 RVA: 0x0025DE95 File Offset: 0x0025C095
	public void Init(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.TerExt = _TerExt;
		this.DChunk = _DChunk;
		this.BMesh = _BMesh;
		this.WasReset = _WasReset;
	}

	// Token: 0x06005F5B RID: 24411 RVA: 0x0025DEB4 File Offset: 0x0025C0B4
	public void ThreadExtraWork()
	{
		this.TerExt.ThreadExtraWork(this.DChunk, this.BMesh, this.WasReset);
	}

	// Token: 0x06005F5C RID: 24412 RVA: 0x0025DED3 File Offset: 0x0025C0D3
	public void MainExtraWork()
	{
		this.TerExt.MainExtraWork(this.DChunk, this.BMesh);
	}

	// Token: 0x06005F5D RID: 24413 RVA: 0x0025DEEC File Offset: 0x0025C0EC
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

	// Token: 0x04004AF0 RID: 19184
	public int DEBUG_TCId = -1;

	// Token: 0x04004AF1 RID: 19185
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantTerrain TerExt;

	// Token: 0x04004AF2 RID: 19186
	public DistantChunk DChunk;

	// Token: 0x04004AF3 RID: 19187
	public DistantChunkBasicMesh BMesh;

	// Token: 0x04004AF4 RID: 19188
	public bool WasReset;
}
