using System;

// Token: 0x02000490 RID: 1168
[Flags]
public enum EntityFlags : uint
{
	// Token: 0x0400199A RID: 6554
	None = 0U,
	// Token: 0x0400199B RID: 6555
	Player = 1U,
	// Token: 0x0400199C RID: 6556
	Zombie = 2U,
	// Token: 0x0400199D RID: 6557
	Animal = 4U,
	// Token: 0x0400199E RID: 6558
	Bandit = 8U,
	// Token: 0x0400199F RID: 6559
	Edible = 16U,
	// Token: 0x040019A0 RID: 6560
	Timid = 32U,
	// Token: 0x040019A1 RID: 6561
	All = 4294967295U,
	// Token: 0x040019A2 RID: 6562
	AIHearing = 14U,
	// Token: 0x040019A3 RID: 6563
	AISmelling = 6U
}
