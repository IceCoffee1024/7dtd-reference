using System;

// Token: 0x020004CC RID: 1228
public enum RespawnProgress : byte
{
	// Token: 0x04001CB9 RID: 7353
	Done,
	// Token: 0x04001CBA RID: 7354
	WaitingForVideoToPlay,
	// Token: 0x04001CBB RID: 7355
	WaitingForRespawnTime,
	// Token: 0x04001CBC RID: 7356
	WaitingForSpawnWindowToOpen,
	// Token: 0x04001CBD RID: 7357
	WaitingForSpawnWindowToClose,
	// Token: 0x04001CBE RID: 7358
	WaitingForSpawnPointSelection,
	// Token: 0x04001CBF RID: 7359
	WaitingForSpawnChunk,
	// Token: 0x04001CC0 RID: 7360
	RetryingRespawn,
	// Token: 0x04001CC1 RID: 7361
	WaitingForCollider,
	// Token: 0x04001CC2 RID: 7362
	WaitingForUI,
	// Token: 0x04001CC3 RID: 7363
	WaitingForWorld
}
