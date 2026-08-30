using System;

// Token: 0x02000C25 RID: 3109
public class ThreadContainer
{
	// Token: 0x06005EB5 RID: 24245 RVA: 0x00257CA4 File Offset: 0x00255EA4
	public ThreadContainer(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.Init(_TerExt, _DChunk, _BMesh, _WasReset);
	}

	// Token: 0x06005EB6 RID: 24246 RVA: 0x00257CBE File Offset: 0x00255EBE
	public ThreadContainer()
	{
		this.TerExt = null;
		this.DChunk = null;
		this.BMesh = null;
		this.WasReset = false;
	}

	// Token: 0x06005EB7 RID: 24247 RVA: 0x00257CE9 File Offset: 0x00255EE9
	public void Init(DistantTerrain _TerExt, DistantChunk _DChunk, DistantChunkBasicMesh _BMesh, bool _WasReset)
	{
		this.TerExt = _TerExt;
		this.DChunk = _DChunk;
		this.BMesh = _BMesh;
		this.WasReset = _WasReset;
	}

	// Token: 0x06005EB8 RID: 24248 RVA: 0x00257D08 File Offset: 0x00255F08
	public void ThreadExtraWork()
	{
		this.TerExt.ThreadExtraWork(this.DChunk, this.BMesh, this.WasReset);
	}

	// Token: 0x06005EB9 RID: 24249 RVA: 0x00257D27 File Offset: 0x00255F27
	public void MainExtraWork()
	{
		this.TerExt.MainExtraWork(this.DChunk, this.BMesh);
	}

	// Token: 0x06005EBA RID: 24250 RVA: 0x00257D40 File Offset: 0x00255F40
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

	// Token: 0x04004A14 RID: 18964
	public int DEBUG_TCId = -1;

	// Token: 0x04004A15 RID: 18965
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantTerrain TerExt;

	// Token: 0x04004A16 RID: 18966
	public DistantChunk DChunk;

	// Token: 0x04004A17 RID: 18967
	public DistantChunkBasicMesh BMesh;

	// Token: 0x04004A18 RID: 18968
	public bool WasReset;
}
