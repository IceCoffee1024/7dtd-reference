using System;

// Token: 0x02000A7F RID: 2687
public struct LayerStackInfo
{
	// Token: 0x0600512C RID: 20780 RVA: 0x001EF916 File Offset: 0x001EDB16
	public LayerStackInfo(int startCompStackIndex, int startUVStackIndex)
	{
		this.GroupDepth = 0;
		this.MaxCompStackIndex = startCompStackIndex;
		this.MaxUVStackIndex = startUVStackIndex;
	}

	// Token: 0x04003E54 RID: 15956
	public int GroupDepth;

	// Token: 0x04003E55 RID: 15957
	public int MaxCompStackIndex;

	// Token: 0x04003E56 RID: 15958
	public int MaxUVStackIndex;
}
