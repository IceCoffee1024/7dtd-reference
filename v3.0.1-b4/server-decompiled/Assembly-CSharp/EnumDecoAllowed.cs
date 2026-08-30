using System;

// Token: 0x02000C0E RID: 3086
[Flags]
public enum EnumDecoAllowed : byte
{
	// Token: 0x040048FE RID: 18686
	Everything = 0,
	// Token: 0x040048FF RID: 18687
	SlopeLo = 1,
	// Token: 0x04004900 RID: 18688
	SlopeHi = 2,
	// Token: 0x04004901 RID: 18689
	SizeLo = 4,
	// Token: 0x04004902 RID: 18690
	SizeHi = 8,
	// Token: 0x04004903 RID: 18691
	StreetOnly = 16,
	// Token: 0x04004904 RID: 18692
	Nothing = 255
}
