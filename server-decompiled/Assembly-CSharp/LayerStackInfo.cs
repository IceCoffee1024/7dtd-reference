using System;

// Token: 0x02000A9F RID: 2719
public struct LayerStackInfo
{
	// Token: 0x060051A7 RID: 20903 RVA: 0x001F43DA File Offset: 0x001F25DA
	public LayerStackInfo(int startCompStackIndex, int startUVStackIndex)
	{
		this.GroupDepth = 0;
		this.MaxCompStackIndex = startCompStackIndex;
		this.MaxUVStackIndex = startUVStackIndex;
	}

	// Token: 0x04003F10 RID: 16144
	public int GroupDepth;

	// Token: 0x04003F11 RID: 16145
	public int MaxCompStackIndex;

	// Token: 0x04003F12 RID: 16146
	public int MaxUVStackIndex;
}
