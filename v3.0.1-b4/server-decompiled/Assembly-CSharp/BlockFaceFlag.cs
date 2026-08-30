using System;

// Token: 0x0200138E RID: 5006
[Flags]
public enum BlockFaceFlag
{
	// Token: 0x0400775E RID: 30558
	None = 0,
	// Token: 0x0400775F RID: 30559
	Top = 1,
	// Token: 0x04007760 RID: 30560
	Bottom = 2,
	// Token: 0x04007761 RID: 30561
	North = 4,
	// Token: 0x04007762 RID: 30562
	West = 8,
	// Token: 0x04007763 RID: 30563
	South = 16,
	// Token: 0x04007764 RID: 30564
	East = 32,
	// Token: 0x04007765 RID: 30565
	All = 63,
	// Token: 0x04007766 RID: 30566
	Solid = 63,
	// Token: 0x04007767 RID: 30567
	Axials = 60
}
