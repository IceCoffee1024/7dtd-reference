using System;

// Token: 0x02000AA9 RID: 2729
public struct LayerStackInfo
{
	// Token: 0x060051CB RID: 20939 RVA: 0x001F54B6 File Offset: 0x001F36B6
	public LayerStackInfo(int startCompStackIndex, int startUVStackIndex)
	{
		this.GroupDepth = 0;
		this.MaxCompStackIndex = startCompStackIndex;
		this.MaxUVStackIndex = startUVStackIndex;
	}

	// Token: 0x04003F31 RID: 16177
	public int GroupDepth;

	// Token: 0x04003F32 RID: 16178
	public int MaxCompStackIndex;

	// Token: 0x04003F33 RID: 16179
	public int MaxUVStackIndex;
}
