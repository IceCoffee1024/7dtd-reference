using System;

// Token: 0x02000BAE RID: 2990
[Flags]
public enum ChunkProtectionLevel
{
	// Token: 0x040045CC RID: 17868
	None = 0,
	// Token: 0x040045CD RID: 17869
	NearOfflinePlayer = 1,
	// Token: 0x040045CE RID: 17870
	NearBedroll = 2,
	// Token: 0x040045CF RID: 17871
	NearSupplyCrate = 4,
	// Token: 0x040045D0 RID: 17872
	NearQuestObjective = 8,
	// Token: 0x040045D1 RID: 17873
	NearDroppedBackpack = 16,
	// Token: 0x040045D2 RID: 17874
	NearVehicle = 32,
	// Token: 0x040045D3 RID: 17875
	NearLandClaim = 64,
	// Token: 0x040045D4 RID: 17876
	OfflinePlayer = 128,
	// Token: 0x040045D5 RID: 17877
	Bedroll = 256,
	// Token: 0x040045D6 RID: 17878
	SupplyCrate = 512,
	// Token: 0x040045D7 RID: 17879
	QuestObjective = 1024,
	// Token: 0x040045D8 RID: 17880
	DroppedBackpack = 2048,
	// Token: 0x040045D9 RID: 17881
	Trader = 4096,
	// Token: 0x040045DA RID: 17882
	Drone = 8192,
	// Token: 0x040045DB RID: 17883
	Vehicle = 16384,
	// Token: 0x040045DC RID: 17884
	LandClaim = 32768,
	// Token: 0x040045DD RID: 17885
	CurrentlySynced = 65536,
	// Token: 0x040045DE RID: 17886
	All = -1
}
