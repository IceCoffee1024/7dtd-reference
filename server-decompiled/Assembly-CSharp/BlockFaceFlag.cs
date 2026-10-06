using System;

// Token: 0x020013C9 RID: 5065
[Flags]
public enum BlockFaceFlag
{
	// Token: 0x040078B2 RID: 30898
	None = 0,
	// Token: 0x040078B3 RID: 30899
	Top = 1,
	// Token: 0x040078B4 RID: 30900
	Bottom = 2,
	// Token: 0x040078B5 RID: 30901
	North = 4,
	// Token: 0x040078B6 RID: 30902
	West = 8,
	// Token: 0x040078B7 RID: 30903
	South = 16,
	// Token: 0x040078B8 RID: 30904
	East = 32,
	// Token: 0x040078B9 RID: 30905
	All = 63,
	// Token: 0x040078BA RID: 30906
	Solid = 63,
	// Token: 0x040078BB RID: 30907
	Axials = 60
}
