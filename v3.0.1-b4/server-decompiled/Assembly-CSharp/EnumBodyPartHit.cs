using System;

// Token: 0x02000467 RID: 1127
[Flags]
public enum EnumBodyPartHit
{
	// Token: 0x040017A2 RID: 6050
	None = 0,
	// Token: 0x040017A3 RID: 6051
	Torso = 1,
	// Token: 0x040017A4 RID: 6052
	Head = 2,
	// Token: 0x040017A5 RID: 6053
	LeftUpperArm = 4,
	// Token: 0x040017A6 RID: 6054
	RightUpperArm = 8,
	// Token: 0x040017A7 RID: 6055
	LeftUpperLeg = 16,
	// Token: 0x040017A8 RID: 6056
	RightUpperLeg = 32,
	// Token: 0x040017A9 RID: 6057
	LeftLowerArm = 64,
	// Token: 0x040017AA RID: 6058
	RightLowerArm = 128,
	// Token: 0x040017AB RID: 6059
	LeftLowerLeg = 256,
	// Token: 0x040017AC RID: 6060
	RightLowerLeg = 512,
	// Token: 0x040017AD RID: 6061
	Special = 1024,
	// Token: 0x040017AE RID: 6062
	UpperArms = 12,
	// Token: 0x040017AF RID: 6063
	LowerArms = 192,
	// Token: 0x040017B0 RID: 6064
	Arms = 204,
	// Token: 0x040017B1 RID: 6065
	UpperLegs = 48,
	// Token: 0x040017B2 RID: 6066
	LowerLegs = 768,
	// Token: 0x040017B3 RID: 6067
	Legs = 816,
	// Token: 0x040017B4 RID: 6068
	BitsUsed = 11
}
