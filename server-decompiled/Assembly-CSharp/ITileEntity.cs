using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C72 RID: 3186
public interface ITileEntity : ILockTarget, IHasWorldAddress
{
	// Token: 0x14000086 RID: 134
	// (add) Token: 0x06006068 RID: 24680
	// (remove) Token: 0x06006069 RID: 24681
	event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x0600606A RID: 24682
	List<ITileEntityChangedListener> listeners { get; }

	// Token: 0x0600606B RID: 24683
	void SetUserAccessing(bool _bUserAccessing);

	// Token: 0x0600606C RID: 24684
	bool IsUserAccessing();

	// Token: 0x0600606D RID: 24685
	void SetModified();

	// Token: 0x0600606E RID: 24686
	Chunk GetChunk();

	// Token: 0x0600606F RID: 24687
	Vector3i ToWorldPos();

	// Token: 0x06006070 RID: 24688
	Vector3 ToWorldCenterPos();

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06006071 RID: 24689
	BlockValue blockValue { get; }

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06006072 RID: 24690
	// (set) Token: 0x06006073 RID: 24691
	bool IsRemoving { get; set; }
}
