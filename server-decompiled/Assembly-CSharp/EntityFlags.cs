using System;

// Token: 0x0200048A RID: 1162
[Flags]
public enum EntityFlags : uint
{
	// Token: 0x0400198F RID: 6543
	None = 0U,
	// Token: 0x04001990 RID: 6544
	Player = 1U,
	// Token: 0x04001991 RID: 6545
	Zombie = 2U,
	// Token: 0x04001992 RID: 6546
	Animal = 4U,
	// Token: 0x04001993 RID: 6547
	Bandit = 8U,
	// Token: 0x04001994 RID: 6548
	Edible = 16U,
	// Token: 0x04001995 RID: 6549
	All = 4294967295U,
	// Token: 0x04001996 RID: 6550
	AIHearing = 14U,
	// Token: 0x04001997 RID: 6551
	AISmelling = 6U
}
