using System;

// Token: 0x020013A9 RID: 5033
[Flags]
public enum BlockFaceFlag
{
	// Token: 0x04007837 RID: 30775
	None = 0,
	// Token: 0x04007838 RID: 30776
	Top = 1,
	// Token: 0x04007839 RID: 30777
	Bottom = 2,
	// Token: 0x0400783A RID: 30778
	North = 4,
	// Token: 0x0400783B RID: 30779
	West = 8,
	// Token: 0x0400783C RID: 30780
	South = 16,
	// Token: 0x0400783D RID: 30781
	East = 32,
	// Token: 0x0400783E RID: 30782
	All = 63,
	// Token: 0x0400783F RID: 30783
	Solid = 63,
	// Token: 0x04007840 RID: 30784
	Axials = 60
}
