using System;

// Token: 0x02000376 RID: 886
public enum DynamicItemState : byte
{
	// Token: 0x040010BF RID: 4287
	Waiting,
	// Token: 0x040010C0 RID: 4288
	UpdateRequired,
	// Token: 0x040010C1 RID: 4289
	Empty,
	// Token: 0x040010C2 RID: 4290
	LoadRequested,
	// Token: 0x040010C3 RID: 4291
	Loading,
	// Token: 0x040010C4 RID: 4292
	Loaded,
	// Token: 0x040010C5 RID: 4293
	ReadyToDelete,
	// Token: 0x040010C6 RID: 4294
	Invalid
}
