using System;

// Token: 0x02000B8A RID: 2954
[Flags]
public enum ChunkProtectionLevel
{
	// Token: 0x040044F3 RID: 17651
	None = 0,
	// Token: 0x040044F4 RID: 17652
	NearOfflinePlayer = 1,
	// Token: 0x040044F5 RID: 17653
	NearBedroll = 2,
	// Token: 0x040044F6 RID: 17654
	NearSupplyCrate = 4,
	// Token: 0x040044F7 RID: 17655
	NearQuestObjective = 8,
	// Token: 0x040044F8 RID: 17656
	NearDroppedBackpack = 16,
	// Token: 0x040044F9 RID: 17657
	NearVehicle = 32,
	// Token: 0x040044FA RID: 17658
	NearLandClaim = 64,
	// Token: 0x040044FB RID: 17659
	OfflinePlayer = 128,
	// Token: 0x040044FC RID: 17660
	Bedroll = 256,
	// Token: 0x040044FD RID: 17661
	SupplyCrate = 512,
	// Token: 0x040044FE RID: 17662
	QuestObjective = 1024,
	// Token: 0x040044FF RID: 17663
	DroppedBackpack = 2048,
	// Token: 0x04004500 RID: 17664
	Trader = 4096,
	// Token: 0x04004501 RID: 17665
	Drone = 8192,
	// Token: 0x04004502 RID: 17666
	Vehicle = 16384,
	// Token: 0x04004503 RID: 17667
	LandClaim = 32768,
	// Token: 0x04004504 RID: 17668
	CurrentlySynced = 65536,
	// Token: 0x04004505 RID: 17669
	All = -1
}
