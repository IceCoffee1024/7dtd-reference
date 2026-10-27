using System;

// Token: 0x02000C3D RID: 3133
[Flags]
public enum EnumDecoAllowed : byte
{
	// Token: 0x040049FF RID: 18943
	Everything = 0,
	// Token: 0x04004A00 RID: 18944
	SlopeLo = 1,
	// Token: 0x04004A01 RID: 18945
	SlopeHi = 2,
	// Token: 0x04004A02 RID: 18946
	SizeLo = 4,
	// Token: 0x04004A03 RID: 18947
	SizeHi = 8,
	// Token: 0x04004A04 RID: 18948
	StreetOnly = 16,
	// Token: 0x04004A05 RID: 18949
	Nothing = 255
}
