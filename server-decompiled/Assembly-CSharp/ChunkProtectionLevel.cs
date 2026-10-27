using System;

// Token: 0x02000BB9 RID: 3001
[Flags]
public enum ChunkProtectionLevel
{
	// Token: 0x040045F1 RID: 17905
	None = 0,
	// Token: 0x040045F2 RID: 17906
	NearOfflinePlayer = 1,
	// Token: 0x040045F3 RID: 17907
	NearBedroll = 2,
	// Token: 0x040045F4 RID: 17908
	NearSupplyCrate = 4,
	// Token: 0x040045F5 RID: 17909
	NearQuestObjective = 8,
	// Token: 0x040045F6 RID: 17910
	NearDroppedBackpack = 16,
	// Token: 0x040045F7 RID: 17911
	NearVehicle = 32,
	// Token: 0x040045F8 RID: 17912
	NearLandClaim = 64,
	// Token: 0x040045F9 RID: 17913
	OfflinePlayer = 128,
	// Token: 0x040045FA RID: 17914
	Bedroll = 256,
	// Token: 0x040045FB RID: 17915
	SupplyCrate = 512,
	// Token: 0x040045FC RID: 17916
	QuestObjective = 1024,
	// Token: 0x040045FD RID: 17917
	DroppedBackpack = 2048,
	// Token: 0x040045FE RID: 17918
	Trader = 4096,
	// Token: 0x040045FF RID: 17919
	Drone = 8192,
	// Token: 0x04004600 RID: 17920
	Vehicle = 16384,
	// Token: 0x04004601 RID: 17921
	LandClaim = 32768,
	// Token: 0x04004602 RID: 17922
	CurrentlySynced = 65536,
	// Token: 0x04004603 RID: 17923
	All = -1
}
