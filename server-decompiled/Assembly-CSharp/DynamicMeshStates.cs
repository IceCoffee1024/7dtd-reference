using System;

// Token: 0x02000371 RID: 881
[Flags]
public enum DynamicMeshStates : short
{
	// Token: 0x04001051 RID: 4177
	None = 0,
	// Token: 0x04001052 RID: 4178
	ThreadUpdating = 1,
	// Token: 0x04001053 RID: 4179
	SaveRequired = 2,
	// Token: 0x04001054 RID: 4180
	LoadRequired = 4,
	// Token: 0x04001055 RID: 4181
	UnloadMark1 = 8,
	// Token: 0x04001056 RID: 4182
	UnloadMark2 = 16,
	// Token: 0x04001057 RID: 4183
	UnloadMark3 = 32,
	// Token: 0x04001058 RID: 4184
	MarkedForDelete = 64,
	// Token: 0x04001059 RID: 4185
	LoadBoosted = 128,
	// Token: 0x0400105A RID: 4186
	MainThreadLoadRequest = 256,
	// Token: 0x0400105B RID: 4187
	FileMissing = 512,
	// Token: 0x0400105C RID: 4188
	Generating = 1024
}
