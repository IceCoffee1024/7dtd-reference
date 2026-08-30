using System;

// Token: 0x020004C4 RID: 1220
public enum RespawnProgress : byte
{
	// Token: 0x04001CAC RID: 7340
	Done,
	// Token: 0x04001CAD RID: 7341
	WaitingForVideoToPlay,
	// Token: 0x04001CAE RID: 7342
	WaitingForRespawnTime,
	// Token: 0x04001CAF RID: 7343
	WaitingForSpawnWindowToOpen,
	// Token: 0x04001CB0 RID: 7344
	WaitingForSpawnWindowToClose,
	// Token: 0x04001CB1 RID: 7345
	WaitingForSpawnPointSelection,
	// Token: 0x04001CB2 RID: 7346
	ClampingToValidWorldPos,
	// Token: 0x04001CB3 RID: 7347
	RetryingRespawn,
	// Token: 0x04001CB4 RID: 7348
	WaitingForCollider
}
