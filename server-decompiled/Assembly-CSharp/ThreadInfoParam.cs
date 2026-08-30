using System;

// Token: 0x02000C33 RID: 3123
public class ThreadInfoParam
{
	// Token: 0x06005F27 RID: 24359 RVA: 0x0025A02C File Offset: 0x0025822C
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

	// Token: 0x06005F28 RID: 24360 RVA: 0x0025A18C File Offset: 0x0025838C
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

	// Token: 0x06005F29 RID: 24361 RVA: 0x0025A2CC File Offset: 0x002584CC
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

	// Token: 0x06005F2A RID: 24362 RVA: 0x0025A368 File Offset: 0x00258568
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

	// Token: 0x04004A8C RID: 19084
	public ThreadContainer[] ThreadContListA;

	// Token: 0x04004A8D RID: 19085
	public int CntThreadContList;

	// Token: 0x04004A8E RID: 19086
	public int LengthThreadContList;

	// Token: 0x04004A8F RID: 19087
	public int ResLevel;

	// Token: 0x04004A90 RID: 19088
	public int OutId;

	// Token: 0x04004A91 RID: 19089
	public DistantChunk[] ForwardChunkToDeleteIdA;

	// Token: 0x04004A92 RID: 19090
	public DistantChunk[] BackwardChunkToDeleteIdA;

	// Token: 0x04004A93 RID: 19091
	public int CntForwardChunkToDeleteId;

	// Token: 0x04004A94 RID: 19092
	public int LengthForwardChunkToDeleteId;

	// Token: 0x04004A95 RID: 19093
	public int CntBackwardChunkToDeleteId;

	// Token: 0x04004A96 RID: 19094
	public int LengthBackwardChunkToDeleteId;

	// Token: 0x04004A97 RID: 19095
	public DistantChunk[][] ForwardChunkSeamToAdjust;

	// Token: 0x04004A98 RID: 19096
	public DistantChunk[][] BackwardChunkSeamToAdjust;

	// Token: 0x04004A99 RID: 19097
	public int[][] ForwardEdgeId;

	// Token: 0x04004A9A RID: 19098
	public int[][] BackwardEdgeId;

	// Token: 0x04004A9B RID: 19099
	public int CntForwardChunkSeamToAdjust;

	// Token: 0x04004A9C RID: 19100
	public int CntBackwardChunkSeamToAdjust;

	// Token: 0x04004A9D RID: 19101
	public int FDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004A9E RID: 19102
	public int FDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004A9F RID: 19103
	public int[] SDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004AA0 RID: 19104
	public int[] SDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004AA1 RID: 19105
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeFristDim = 150;

	// Token: 0x04004AA2 RID: 19106
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeSecondDim = 64;

	// Token: 0x04004AA3 RID: 19107
	public bool IsThreadDone;

	// Token: 0x04004AA4 RID: 19108
	public bool IsCoroutineDone;

	// Token: 0x04004AA5 RID: 19109
	public bool IsBigCapacity;

	// Token: 0x04004AA6 RID: 19110
	public bool IsAsynchronous;
}
