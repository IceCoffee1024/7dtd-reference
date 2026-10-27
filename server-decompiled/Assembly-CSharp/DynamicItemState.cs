using System;

// Token: 0x0200037F RID: 895
public enum DynamicItemState : byte
{
	// Token: 0x040010DD RID: 4317
	Waiting,
	// Token: 0x040010DE RID: 4318
	UpdateRequired,
	// Token: 0x040010DF RID: 4319
	Empty,
	// Token: 0x040010E0 RID: 4320
	LoadRequested,
	// Token: 0x040010E1 RID: 4321
	Loading,
	// Token: 0x040010E2 RID: 4322
	Loaded,
	// Token: 0x040010E3 RID: 4323
	ReadyToDelete,
	// Token: 0x040010E4 RID: 4324
	Invalid
}
