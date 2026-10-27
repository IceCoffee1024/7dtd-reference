using System;

// Token: 0x02000C80 RID: 3200
public enum TileEntityType : byte
{
	// Token: 0x04004C07 RID: 19463
	None,
	// Token: 0x04004C08 RID: 19464
	Collector = 3,
	// Token: 0x04004C09 RID: 19465
	VendingMachine = 7,
	// Token: 0x04004C0A RID: 19466
	Forge,
	// Token: 0x04004C0B RID: 19467
	Campfire,
	// Token: 0x04004C0C RID: 19468
	Workstation = 12,
	// Token: 0x04004C0D RID: 19469
	Powered = 15,
	// Token: 0x04004C0E RID: 19470
	PowerSource,
	// Token: 0x04004C0F RID: 19471
	PowerRangeTrap,
	// Token: 0x04004C10 RID: 19472
	Light,
	// Token: 0x04004C11 RID: 19473
	Trigger,
	// Token: 0x04004C12 RID: 19474
	Sleeper,
	// Token: 0x04004C13 RID: 19475
	PowerMeleeTrap,
	// Token: 0x04004C14 RID: 19476
	Composite = 25,
	// Token: 0x04004C15 RID: 19477
	Taskboard = 27,
	// Token: 0x04004C16 RID: 19478
	LandClaim = 4,
	// Token: 0x04004C17 RID: 19479
	Loot,
	// Token: 0x04004C18 RID: 19480
	Trader,
	// Token: 0x04004C19 RID: 19481
	SecureLoot = 10,
	// Token: 0x04004C1A RID: 19482
	SecureDoor,
	// Token: 0x04004C1B RID: 19483
	Sign = 13,
	// Token: 0x04004C1C RID: 19484
	GoreBlock,
	// Token: 0x04004C1D RID: 19485
	SecureLootSigned = 22
}
