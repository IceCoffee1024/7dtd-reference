using System;

// Token: 0x020013DB RID: 5083
[Flags]
public enum BlockFaceFlag
{
	// Token: 0x040078F3 RID: 30963
	None = 0,
	// Token: 0x040078F4 RID: 30964
	Top = 1,
	// Token: 0x040078F5 RID: 30965
	Bottom = 2,
	// Token: 0x040078F6 RID: 30966
	North = 4,
	// Token: 0x040078F7 RID: 30967
	West = 8,
	// Token: 0x040078F8 RID: 30968
	South = 16,
	// Token: 0x040078F9 RID: 30969
	East = 32,
	// Token: 0x040078FA RID: 30970
	All = 63,
	// Token: 0x040078FB RID: 30971
	Solid = 63,
	// Token: 0x040078FC RID: 30972
	Axials = 60
}
