using System;

// Token: 0x0200036A RID: 874
[Flags]
public enum DynamicMeshStates : short
{
	// Token: 0x04001043 RID: 4163
	None = 0,
	// Token: 0x04001044 RID: 4164
	ThreadUpdating = 1,
	// Token: 0x04001045 RID: 4165
	SaveRequired = 2,
	// Token: 0x04001046 RID: 4166
	LoadRequired = 4,
	// Token: 0x04001047 RID: 4167
	UnloadMark1 = 8,
	// Token: 0x04001048 RID: 4168
	UnloadMark2 = 16,
	// Token: 0x04001049 RID: 4169
	UnloadMark3 = 32,
	// Token: 0x0400104A RID: 4170
	MarkedForDelete = 64,
	// Token: 0x0400104B RID: 4171
	LoadBoosted = 128,
	// Token: 0x0400104C RID: 4172
	MainThreadLoadRequest = 256,
	// Token: 0x0400104D RID: 4173
	FileMissing = 512,
	// Token: 0x0400104E RID: 4174
	Generating = 1024
}
