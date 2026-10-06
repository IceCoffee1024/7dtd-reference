using System;

// Token: 0x02000C45 RID: 3141
public class ThreadInfoParam
{
	// Token: 0x06005F40 RID: 24384 RVA: 0x0025D328 File Offset: 0x0025B528
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

	// Token: 0x06005F41 RID: 24385 RVA: 0x0025D488 File Offset: 0x0025B688
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

	// Token: 0x06005F42 RID: 24386 RVA: 0x0025D5C8 File Offset: 0x0025B7C8
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

	// Token: 0x06005F43 RID: 24387 RVA: 0x0025D664 File Offset: 0x0025B864
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

	// Token: 0x04004ACB RID: 19147
	public ThreadContainer[] ThreadContListA;

	// Token: 0x04004ACC RID: 19148
	public int CntThreadContList;

	// Token: 0x04004ACD RID: 19149
	public int LengthThreadContList;

	// Token: 0x04004ACE RID: 19150
	public int ResLevel;

	// Token: 0x04004ACF RID: 19151
	public int OutId;

	// Token: 0x04004AD0 RID: 19152
	public DistantChunk[] ForwardChunkToDeleteIdA;

	// Token: 0x04004AD1 RID: 19153
	public DistantChunk[] BackwardChunkToDeleteIdA;

	// Token: 0x04004AD2 RID: 19154
	public int CntForwardChunkToDeleteId;

	// Token: 0x04004AD3 RID: 19155
	public int LengthForwardChunkToDeleteId;

	// Token: 0x04004AD4 RID: 19156
	public int CntBackwardChunkToDeleteId;

	// Token: 0x04004AD5 RID: 19157
	public int LengthBackwardChunkToDeleteId;

	// Token: 0x04004AD6 RID: 19158
	public DistantChunk[][] ForwardChunkSeamToAdjust;

	// Token: 0x04004AD7 RID: 19159
	public DistantChunk[][] BackwardChunkSeamToAdjust;

	// Token: 0x04004AD8 RID: 19160
	public int[][] ForwardEdgeId;

	// Token: 0x04004AD9 RID: 19161
	public int[][] BackwardEdgeId;

	// Token: 0x04004ADA RID: 19162
	public int CntForwardChunkSeamToAdjust;

	// Token: 0x04004ADB RID: 19163
	public int CntBackwardChunkSeamToAdjust;

	// Token: 0x04004ADC RID: 19164
	public int FDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004ADD RID: 19165
	public int FDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004ADE RID: 19166
	public int[] SDLengthForwardChunkSeamToAdjust;

	// Token: 0x04004ADF RID: 19167
	public int[] SDLengthBackwardChunkSeamToAdjust;

	// Token: 0x04004AE0 RID: 19168
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeFristDim = 150;

	// Token: 0x04004AE1 RID: 19169
	[PublicizedFrom(EAccessModifier.Private)]
	public int TmpArraySizeSecondDim = 64;

	// Token: 0x04004AE2 RID: 19170
	public bool IsThreadDone;

	// Token: 0x04004AE3 RID: 19171
	public bool IsCoroutineDone;

	// Token: 0x04004AE4 RID: 19172
	public bool IsBigCapacity;

	// Token: 0x04004AE5 RID: 19173
	public bool IsAsynchronous;
}
