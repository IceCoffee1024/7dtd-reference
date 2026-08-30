using System;

// Token: 0x02000C52 RID: 3154
public enum TileEntityType : byte
{
	// Token: 0x04004B06 RID: 19206
	None,
	// Token: 0x04004B07 RID: 19207
	Collector = 3,
	// Token: 0x04004B08 RID: 19208
	VendingMachine = 7,
	// Token: 0x04004B09 RID: 19209
	Forge,
	// Token: 0x04004B0A RID: 19210
	Campfire,
	// Token: 0x04004B0B RID: 19211
	Workstation = 12,
	// Token: 0x04004B0C RID: 19212
	Powered = 15,
	// Token: 0x04004B0D RID: 19213
	PowerSource,
	// Token: 0x04004B0E RID: 19214
	PowerRangeTrap,
	// Token: 0x04004B0F RID: 19215
	Light,
	// Token: 0x04004B10 RID: 19216
	Trigger,
	// Token: 0x04004B11 RID: 19217
	Sleeper,
	// Token: 0x04004B12 RID: 19218
	PowerMeleeTrap,
	// Token: 0x04004B13 RID: 19219
	Composite = 25,
	// Token: 0x04004B14 RID: 19220
	Taskboard = 27,
	// Token: 0x04004B15 RID: 19221
	LandClaim = 4,
	// Token: 0x04004B16 RID: 19222
	Loot,
	// Token: 0x04004B17 RID: 19223
	Trader,
	// Token: 0x04004B18 RID: 19224
	SecureLoot = 10,
	// Token: 0x04004B19 RID: 19225
	SecureDoor,
	// Token: 0x04004B1A RID: 19226
	Sign = 13,
	// Token: 0x04004B1B RID: 19227
	GoreBlock,
	// Token: 0x04004B1C RID: 19228
	SecureLootSigned = 22
}
