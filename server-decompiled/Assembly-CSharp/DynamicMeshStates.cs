using System;

// Token: 0x02000373 RID: 883
[Flags]
public enum DynamicMeshStates : short
{
	// Token: 0x04001061 RID: 4193
	None = 0,
	// Token: 0x04001062 RID: 4194
	ThreadUpdating = 1,
	// Token: 0x04001063 RID: 4195
	SaveRequired = 2,
	// Token: 0x04001064 RID: 4196
	LoadRequired = 4,
	// Token: 0x04001065 RID: 4197
	UnloadMark1 = 8,
	// Token: 0x04001066 RID: 4198
	UnloadMark2 = 16,
	// Token: 0x04001067 RID: 4199
	UnloadMark3 = 32,
	// Token: 0x04001068 RID: 4200
	MarkedForDelete = 64,
	// Token: 0x04001069 RID: 4201
	LoadBoosted = 128,
	// Token: 0x0400106A RID: 4202
	MainThreadLoadRequest = 256,
	// Token: 0x0400106B RID: 4203
	FileMissing = 512,
	// Token: 0x0400106C RID: 4204
	Generating = 1024
}
