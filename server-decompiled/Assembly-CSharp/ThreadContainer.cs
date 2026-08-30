using System;

// Token: 0x02000C37 RID: 3127
public class ThreadContainer
{
	// Token: 0x06005F3F RID: 24383 RVA: 0x0025AB54 File Offset: 0x00258D54
	public ThreadContainer(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.Init(_TerExt, _DChunk, _BMesh, _WasReset);
	}

	// Token: 0x06005F40 RID: 24384 RVA: 0x0025AB6E File Offset: 0x00258D6E
	public ThreadContainer()
	{
		this.TerExt = null;
		this.DChunk = null;
		this.BMesh = null;
		this.WasReset = false;
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x0025AB99 File Offset: 0x00258D99
	public void Init(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.TerExt = _TerExt;
		this.DChunk = _DChunk;
		this.BMesh = _BMesh;
		this.WasReset = _WasReset;
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x0025ABB8 File Offset: 0x00258DB8
	public void ThreadExtraWork()
	{
		this.TerExt.ThreadExtraWork(this.DChunk, this.BMesh, this.WasReset);
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x0025ABD7 File Offset: 0x00258DD7
	public void MainExtraWork()
	{
		this.TerExt.MainExtraWork(this.DChunk, this.BMesh);
	}

	// Token: 0x06005F44 RID: 24388 RVA: 0x0025ABF0 File Offset: 0x00258DF0
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

	// Token: 0x04004AB1 RID: 19121
	public int DEBUG_TCId = -1;

	// Token: 0x04004AB2 RID: 19122
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantTerrain TerExt;

	// Token: 0x04004AB3 RID: 19123
	public DistantChunk DChunk;

	// Token: 0x04004AB4 RID: 19124
	public DistantChunkBasicMesh BMesh;

	// Token: 0x04004AB5 RID: 19125
	public bool WasReset;
}
