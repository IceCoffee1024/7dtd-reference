using System;

// Token: 0x02000C50 RID: 3152
public class ThreadInfoParam
{
	// Token: 0x06005F65 RID: 24421 RVA: 0x0025E850 File Offset: 0x0025CA50
	public ThreadInfoParam()
	{
		this.IsBigCapacity = false;
		this.ThreadContListA = new ThreadContainer[this.TmpArraySizeFristDim * 20];
		this.ForwardChunkToDeleteIdA = new DistantChunk[this.TmpArraySizeFristDim];
		this.BackwardChunkToDeleteIdA = new DistantChunk[this.TmpArraySizeFristDim * 2];
		this.ForwardChunkSeamToAdjust = new DistantChunk[this.TmpArraySizeFristDim][];
		this.BackwardChunkSeamToAdjust = new DistantChunk[this.TmpArraySizeFristDim][];
		this.ForwardEdgeId = new int[this.TmpArraySizeFristDim][];
		this.BackwardEdgeId = new int[this.TmpArraySizeFristDim][];
		this.SDLengthForwardChunkSeamToAdjust = new int[this.TmpArraySizeFristDim];
		this.SDLengthBackwardChunkSeamToAdjust = new int[this.TmpArraySizeFristDim];
		for (int i = 0; i < this.TmpArraySizeFristDim; i++)
		{
			this.ForwardChunkSeamToAdjust[i] = new DistantChunk[this.TmpArraySizeSecondDim];
			this.BackwardChunkSeamToAdjust[i] = new DistantChunk[this.TmpArraySizeSecondDim];
			this.ForwardEdgeId[i] = new int[this.TmpArraySizeSecondDim];
			this.BackwardEdgeId[i] = new int[this.TmpArraySizeSecondDim];
		}
		this.FDLengthForwardChunkSeamToAdjust = 0;
		this.FDLengthBackwardChunkSeamToAdjust = 0;
		this.CntForwardChunkSeamToAdjust = 0;
		this.CntBackwardChunkSeamToAdjust = 0;
		this.ResLevel = 0;
		this.OutId = 0;
		this.IsThreadDone = false;
		this.IsCoroutineDone = false;
	}

	// Token: 0x06005F66 RID: 24422 RVA: 0x0025E9B0 File Offset: 0x0025CBB0
	public ThreadInfoParam(DistantChunkMap _CMap, int _ResLevel, int _OutId, bool _IsBigCapacity)
	{
		this.ThreadContListA = new ThreadContainer[this.TmpArraySizeFristDim * 20];
		this.ForwardChunkToDeleteIdA = new DistantChunk[this.TmpArraySizeFristDim];
		this.BackwardChunkToDeleteIdA = new DistantChunk[this.TmpArraySizeFristDim * 2];
		this.ForwardChunkSeamToAdjust = new DistantChunk[this.TmpArraySizeFristDim][];
		this.BackwardChunkSeamToAdjust = new DistantChunk[this.TmpArraySizeFristDim][];
		this.ForwardEdgeId = new int[this.TmpArraySizeFristDim][];
		this.BackwardEdgeId = new int[this.TmpArraySizeFristDim][];
		this.SDLengthForwardChunkSeamToAdjust = new int[this.TmpArraySizeFristDim];
		this.SDLengthBackwardChunkSeamToAdjust = new int[this.TmpArraySizeFristDim];
		for (int i = 0; i < this.TmpArraySizeFristDim; i++)
		{
			this.ForwardChunkSeamToAdjust[i] = new DistantChunk[this.TmpArraySizeSecondDim];
			this.BackwardChunkSeamToAdjust[i] = new DistantChunk[this.TmpArraySizeSecondDim];
			this.ForwardEdgeId[i] = new int[this.TmpArraySizeSecondDim];
			this.BackwardEdgeId[i] = new int[this.TmpArraySizeSecondDim];
			this.SDLengthForwardChunkSeamToAdjust[i] = 0;
			this.SDLengthBackwardChunkSeamToAdjust[i] = 0;
		}
		this.Init(_CMap, _ResLevel, _OutId, _IsBigCapacity);
	}

	// Token: 0x06005F67 RID: 24423 RVA: 0x0025EAF0 File Offset: 0x0025CCF0
	public void Init(DistantChunkMap _CMap, int _ResLevel, int _OutId, bool _IsBigCapacity)
	{
		this.IsBigCapacity = _IsBigCapacity;
		this.CntThreadContList = 0;
		this.LengthThreadContList = 0;
		this.CntForwardChunkToDeleteId = 0;
		this.LengthForwardChunkToDeleteId = 0;
		this.CntBackwardChunkToDeleteId = 0;
		this.LengthBackwardChunkToDeleteId = 0;
		for (int i = 0; i < this.TmpArraySizeFristDim; i++)
		{
			this.SDLengthForwardChunkSeamToAdjust[i] = 0;
			this.SDLengthBackwardChunkSeamToAdjust[i] = 0;
		}
		this.CntForwardChunkSeamToAdjust = 0;
		this.CntBackwardChunkSeamToAdjust = 0;
		this.FDLengthForwardChunkSeamToAdjust = 0;
		this.FDLengthBackwardChunkSeamToAdjust = 0;
		this.ResLevel = _ResLevel;
		this.OutId = _OutId;
		this.IsThreadDone = false;
		this.IsCoroutineDone = false;
	}

	// Token: 0x06005F68 RID: 24424 RVA: 0x0025EB8C File Offset: 0x0025CD8C
	public void ClearAll(ThreadContainerPool TmpThContPool = null)
	{
		if (TmpThContPool != null)
		{
			while (this.CntThreadContList < this.LengthThreadContList)
			{
				TmpThContPool.ReturnObject(this.ThreadContListA[this.CntThreadContList], true);
				this.ThreadContListA[this.CntThreadContList] = null;
				this.CntThreadContList++;
			}
		}
		this.IsThreadDone = true;
		this.IsCoroutineDone = true;
	}

	// Token: 0x04004AF0 RID: 19184
	public ThreadContainer[] ThreadContListA;

	// Token: 0x04004AF1 RID: 19185
	public int CntThreadContList;

	// Token: 0x04004AF2 RID: 19186
	public int LengthThreadContList;

	// Token: 0x04004AF3 RID: 19187
	public int ResLevel;

	// Token: 0x04004AF4 RID: 19188
	public int OutId;

	// Token: 0x04004AF5 RID: 19189
	public DistantChunk[] ForwardChunkToDeleteIdA;

	// Token: 0x04004AF6 RID: 19190
	public DistantChunk[] BackwardChunkToDeleteIdA;

	// Token: 0x04004AF7 RID: 19191
	public int CntForwardChunkToDeleteId;

	// Token: 0x04004AF8 RID: 19192
	public int LengthForwardChunkToDeleteId;

	// Token: 0x04004AF9 RID: 19193
	public int CntBackwardChunkToDeleteId;

	// Token: 0x04004AFA RID: 19194
	public int LengthBackwardChunkToDeleteId;

	// Token: 0x04004AFB RID: 19195
	public DistantChunk[][] ForwardChunkSeamToAdjust;

	// Token: 0x04004AFC RID: 19196
	public DistantChunk[][] BackwardChunkSeamToAdjust;

	// Token: 0x04004AFD RID: 19197
	public int[][] ForwardEdgeId;

	// Token: 0x04004AFE RID: 19198
	public int[][] BackwardEdgeId;

	// Token: 0x04004AFF RID: 19199
	public int CntForwardChunkSeamToAdjust;

	// Token: 0x04004B00 RID: 19200
	public int CntBackwardChunkSeamToAdjust;

	// Token: 0x04004B01 RID: 19201
	public int FDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004B02 RID: 19202
	public int FDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004B03 RID: 19203
	public int[] SDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004B04 RID: 19204
	public int[] SDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004B05 RID: 19205
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeFristDim = 150;

	// Token: 0x04004B06 RID: 19206
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeSecondDim = 64;

	// Token: 0x04004B07 RID: 19207
	public bool IsThreadDone;

	// Token: 0x04004B08 RID: 19208
	public bool IsCoroutineDone;

	// Token: 0x04004B09 RID: 19209
	public bool IsBigCapacity;

	// Token: 0x04004B0A RID: 19210
	public bool IsAsynchronous;
}
