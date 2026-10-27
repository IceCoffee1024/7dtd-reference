using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C7D RID: 3197
public interface ITileEntity : ILockTarget, IHasWorldAddress
{
	// Token: 0x14000086 RID: 134
	// (add) Token: 0x0600608D RID: 24717
	// (remove) Token: 0x0600608E RID: 24718
	event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x0600608F RID: 24719
	List<ITileEntityChangedListener> listeners { get; }

	// Token: 0x06006090 RID: 24720
	void SetUserAccessing(bool _bUserAccessing);

	// Token: 0x06006091 RID: 24721
	bool IsUserAccessing();

	// Token: 0x06006092 RID: 24722
	void SetModified();

	// Token: 0x06006093 RID: 24723
	Chunk GetChunk();

	// Token: 0x06006094 RID: 24724
	Vector3i ToWorldPos();

	// Token: 0x06006095 RID: 24725
	Vector3 ToWorldCenterPos();

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06006096 RID: 24726
	BlockValue blockValue { get; }

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06006097 RID: 24727
	// (set) Token: 0x06006098 RID: 24728
	bool IsRemoving { get; set; }
}
