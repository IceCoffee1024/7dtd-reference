using System;

// Token: 0x020004C0 RID: 1216
public enum RespawnProgress : byte
{
	// Token: 0x04001C8A RID: 7306
	Done,
	// Token: 0x04001C8B RID: 7307
	WaitingForVideoToPlay,
	// Token: 0x04001C8C RID: 7308
	WaitingForRespawnTime,
	// Token: 0x04001C8D RID: 7309
	WaitingForSpawnWindowToOpen,
	// Token: 0x04001C8E RID: 7310
	WaitingForSpawnWindowToClose,
	// Token: 0x04001C8F RID: 7311
	WaitingForSpawnPointSelection,
	// Token: 0x04001C90 RID: 7312
	ClampingToValidWorldPos,
	// Token: 0x04001C91 RID: 7313
	RetryingRespawn
}
