using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C4E RID: 3150
public interface ITileEntity : ILockTarget
{
	// Token: 0x14000086 RID: 134
	// (add) Token: 0x06005FC5 RID: 24517
	// (remove) Token: 0x06005FC6 RID: 24518
	event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06005FC7 RID: 24519
	List<ITileEntityChangedListener> listeners { get; }

	// Token: 0x06005FC8 RID: 24520
	void SetUserAccessing(bool _bUserAccessing);

	// Token: 0x06005FC9 RID: 24521
	bool IsUserAccessing();

	// Token: 0x06005FCA RID: 24522
	void SetModified();

	// Token: 0x06005FCB RID: 24523
	Chunk GetChunk();

	// Token: 0x06005FCC RID: 24524
	Vector3i ToWorldPos();

	// Token: 0x06005FCD RID: 24525
	Vector3 ToWorldCenterPos();

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06005FCE RID: 24526
	BlockValue blockValue { get; }

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x06005FCF RID: 24527
	// (set) Token: 0x06005FD0 RID: 24528
	bool IsRemoving { get; set; }
}
