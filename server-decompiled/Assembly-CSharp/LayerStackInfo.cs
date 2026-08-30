using System;

// Token: 0x02000A90 RID: 2704
public struct LayerStackInfo
{
	// Token: 0x060051B2 RID: 20914 RVA: 0x001F23AE File Offset: 0x001F05AE
	public LayerStackInfo(int startCompStackIndex, int startUVStackIndex)
	{
		this.GroupDepth = 0;
		this.MaxCompStackIndex = startCompStackIndex;
		this.MaxUVStackIndex = startUVStackIndex;
	}

	// Token: 0x04003EE5 RID: 16101
	public int GroupDepth;

	// Token: 0x04003EE6 RID: 16102
	public int MaxCompStackIndex;

	// Token: 0x04003EE7 RID: 16103
	public int MaxUVStackIndex;
}
