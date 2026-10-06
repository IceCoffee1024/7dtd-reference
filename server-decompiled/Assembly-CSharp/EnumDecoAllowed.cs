using System;

// Token: 0x02000C32 RID: 3122
[Flags]
public enum EnumDecoAllowed : byte
{
	// Token: 0x040049DA RID: 18906
	Everything = 0,
	// Token: 0x040049DB RID: 18907
	SlopeLo = 1,
	// Token: 0x040049DC RID: 18908
	SlopeHi = 2,
	// Token: 0x040049DD RID: 18909
	SizeLo = 4,
	// Token: 0x040049DE RID: 18910
	SizeHi = 8,
	// Token: 0x040049DF RID: 18911
	StreetOnly = 16,
	// Token: 0x040049E0 RID: 18912
	Nothing = 255
}
