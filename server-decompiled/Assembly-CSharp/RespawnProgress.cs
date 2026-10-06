using System;

// Token: 0x020004CA RID: 1226
public enum RespawnProgress : byte
{
	// Token: 0x04001CA9 RID: 7337
	Done,
	// Token: 0x04001CAA RID: 7338
	WaitingForVideoToPlay,
	// Token: 0x04001CAB RID: 7339
	WaitingForRespawnTime,
	// Token: 0x04001CAC RID: 7340
	WaitingForSpawnWindowToOpen,
	// Token: 0x04001CAD RID: 7341
	WaitingForSpawnWindowToClose,
	// Token: 0x04001CAE RID: 7342
	WaitingForSpawnPointSelection,
	// Token: 0x04001CAF RID: 7343
	WaitingForSpawnChunk,
	// Token: 0x04001CB0 RID: 7344
	RetryingRespawn,
	// Token: 0x04001CB1 RID: 7345
	WaitingForCollider,
	// Token: 0x04001CB2 RID: 7346
	WaitingForUI,
	// Token: 0x04001CB3 RID: 7347
	WaitingForWorld
}
