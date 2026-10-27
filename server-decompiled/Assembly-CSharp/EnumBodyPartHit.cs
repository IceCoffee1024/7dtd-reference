using System;

// Token: 0x0200046F RID: 1135
[Flags]
public enum EnumBodyPartHit
{
	// Token: 0x040017BF RID: 6079
	None = 0,
	// Token: 0x040017C0 RID: 6080
	Torso = 1,
	// Token: 0x040017C1 RID: 6081
	Head = 2,
	// Token: 0x040017C2 RID: 6082
	LeftUpperArm = 4,
	// Token: 0x040017C3 RID: 6083
	RightUpperArm = 8,
	// Token: 0x040017C4 RID: 6084
	LeftUpperLeg = 16,
	// Token: 0x040017C5 RID: 6085
	RightUpperLeg = 32,
	// Token: 0x040017C6 RID: 6086
	LeftLowerArm = 64,
	// Token: 0x040017C7 RID: 6087
	RightLowerArm = 128,
	// Token: 0x040017C8 RID: 6088
	LeftLowerLeg = 256,
	// Token: 0x040017C9 RID: 6089
	RightLowerLeg = 512,
	// Token: 0x040017CA RID: 6090
	Special = 1024,
	// Token: 0x040017CB RID: 6091
	UpperArms = 12,
	// Token: 0x040017CC RID: 6092
	LowerArms = 192,
	// Token: 0x040017CD RID: 6093
	Arms = 204,
	// Token: 0x040017CE RID: 6094
	UpperLegs = 48,
	// Token: 0x040017CF RID: 6095
	LowerLegs = 768,
	// Token: 0x040017D0 RID: 6096
	Legs = 816,
	// Token: 0x040017D1 RID: 6097
	BitsUsed = 11
}
