using System;

// Token: 0x0200046D RID: 1133
[Flags]
public enum EnumBodyPartHit
{
	// Token: 0x040017AF RID: 6063
	None = 0,
	// Token: 0x040017B0 RID: 6064
	Torso = 1,
	// Token: 0x040017B1 RID: 6065
	Head = 2,
	// Token: 0x040017B2 RID: 6066
	LeftUpperArm = 4,
	// Token: 0x040017B3 RID: 6067
	RightUpperArm = 8,
	// Token: 0x040017B4 RID: 6068
	LeftUpperLeg = 16,
	// Token: 0x040017B5 RID: 6069
	RightUpperLeg = 32,
	// Token: 0x040017B6 RID: 6070
	LeftLowerArm = 64,
	// Token: 0x040017B7 RID: 6071
	RightLowerArm = 128,
	// Token: 0x040017B8 RID: 6072
	LeftLowerLeg = 256,
	// Token: 0x040017B9 RID: 6073
	RightLowerLeg = 512,
	// Token: 0x040017BA RID: 6074
	Special = 1024,
	// Token: 0x040017BB RID: 6075
	UpperArms = 12,
	// Token: 0x040017BC RID: 6076
	LowerArms = 192,
	// Token: 0x040017BD RID: 6077
	Arms = 204,
	// Token: 0x040017BE RID: 6078
	UpperLegs = 48,
	// Token: 0x040017BF RID: 6079
	LowerLegs = 768,
	// Token: 0x040017C0 RID: 6080
	Legs = 816,
	// Token: 0x040017C1 RID: 6081
	BitsUsed = 11
}
