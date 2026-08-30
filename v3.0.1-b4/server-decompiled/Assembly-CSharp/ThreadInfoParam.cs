using System;

// Token: 0x02000C21 RID: 3105
public class ThreadInfoParam
{
	// Token: 0x06005E9D RID: 24221 RVA: 0x0025717C File Offset: 0x0025537C
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

	// Token: 0x06005E9E RID: 24222 RVA: 0x002572DC File Offset: 0x002554DC
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

	// Token: 0x06005E9F RID: 24223 RVA: 0x0025741C File Offset: 0x0025561C
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

	// Token: 0x06005EA0 RID: 24224 RVA: 0x002574B8 File Offset: 0x002556B8
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

	// Token: 0x040049EF RID: 18927
	public ThreadContainer[] ThreadContListA;

	// Token: 0x040049F0 RID: 18928
	public int CntThreadContList;

	// Token: 0x040049F1 RID: 18929
	public int LengthThreadContList;

	// Token: 0x040049F2 RID: 18930
	public int ResLevel;

	// Token: 0x040049F3 RID: 18931
	public int OutId;

	// Token: 0x040049F4 RID: 18932
	public DistantChunk[] ForwardChunkToDeleteIdA;

	// Token: 0x040049F5 RID: 18933
	public DistantChunk[] BackwardChunkToDeleteIdA;

	// Token: 0x040049F6 RID: 18934
	public int CntForwardChunkToDeleteId;

	// Token: 0x040049F7 RID: 18935
	public int LengthForwardChunkToDeleteId;

	// Token: 0x040049F8 RID: 18936
	public int CntBackwardChunkToDeleteId;

	// Token: 0x040049F9 RID: 18937
	public int LengthBackwardChunkToDeleteId;

	// Token: 0x040049FA RID: 18938
	public DistantChunk[][] ForwardChunkSeamToAdjust;

	// Token: 0x040049FB RID: 18939
	public DistantChunk[][] BackwardChunkSeamToAdjust;

	// Token: 0x040049FC RID: 18940
	public int[][] ForwardEdgeId;

	// Token: 0x040049FD RID: 18941
	public int[][] BackwardEdgeId;

	// Token: 0x040049FE RID: 18942
	public int CntForwardChunkSeamToAdjust;

	// Token: 0x040049FF RID: 18943
	public int CntBackwardChunkSeamToAdjust;

	// Token: 0x04004A00 RID: 18944
	public int FDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004A01 RID: 18945
	public int FDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004A02 RID: 18946
	public int[] SDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004A03 RID: 18947
	public int[] SDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004A04 RID: 18948
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeFristDim = 150;

	// Token: 0x04004A05 RID: 18949
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeSecondDim = 64;

	// Token: 0x04004A06 RID: 18950
	public bool IsThreadDone;

	// Token: 0x04004A07 RID: 18951
	public bool IsCoroutineDone;

	// Token: 0x04004A08 RID: 18952
	public bool IsBigCapacity;

	// Token: 0x04004A09 RID: 18953
	public bool IsAsynchronous;
}
