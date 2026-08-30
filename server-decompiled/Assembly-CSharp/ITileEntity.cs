using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C60 RID: 3168
public interface ITileEntity : ILockTarget
{
	// Token: 0x14000086 RID: 134
	// (add) Token: 0x0600604F RID: 24655
	// (remove) Token: 0x06006050 RID: 24656
	event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x06006051 RID: 24657
	List<ITileEntityChangedListener> listeners { get; }

	// Token: 0x06006052 RID: 24658
	void SetUserAccessing(bool _bUserAccessing);

	// Token: 0x06006053 RID: 24659
	bool IsUserAccessing();

	// Token: 0x06006054 RID: 24660
	void SetModified();

	// Token: 0x06006055 RID: 24661
	Chunk GetChunk();

	// Token: 0x06006056 RID: 24662
	Vector3i ToWorldPos();

	// Token: 0x06006057 RID: 24663
	Vector3 ToWorldCenterPos();

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x06006058 RID: 24664
	BlockValue blockValue { get; }

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x06006059 RID: 24665
	// (set) Token: 0x0600605A RID: 24666
	bool IsRemoving { get; set; }
}
