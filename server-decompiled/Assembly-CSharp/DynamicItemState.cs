using System;

// Token: 0x0200037D RID: 893
public enum DynamicItemState : byte
{
	// Token: 0x040010CD RID: 4301
	Waiting,
	// Token: 0x040010CE RID: 4302
	UpdateRequired,
	// Token: 0x040010CF RID: 4303
	Empty,
	// Token: 0x040010D0 RID: 4304
	LoadRequested,
	// Token: 0x040010D1 RID: 4305
	Loading,
	// Token: 0x040010D2 RID: 4306
	Loaded,
	// Token: 0x040010D3 RID: 4307
	ReadyToDelete,
	// Token: 0x040010D4 RID: 4308
	Invalid
}
