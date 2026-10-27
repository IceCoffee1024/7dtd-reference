using System;

// Token: 0x02000492 RID: 1170
[Flags]
public enum EntityFlags : uint
{
	// Token: 0x040019AA RID: 6570
	None = 0U,
	// Token: 0x040019AB RID: 6571
	Player = 1U,
	// Token: 0x040019AC RID: 6572
	Zombie = 2U,
	// Token: 0x040019AD RID: 6573
	Animal = 4U,
	// Token: 0x040019AE RID: 6574
	Bandit = 8U,
	// Token: 0x040019AF RID: 6575
	Edible = 16U,
	// Token: 0x040019B0 RID: 6576
	Timid = 32U,
	// Token: 0x040019B1 RID: 6577
	All = 4294967295U,
	// Token: 0x040019B2 RID: 6578
	AIHearing = 14U,
	// Token: 0x040019B3 RID: 6579
	AISmelling = 6U
}
