using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003F0 RID: 1008
[Preserve]
public class AttachedToEntitySlotInfo
{
	// Token: 0x0400146E RID: 5230
	public int slotIdx;

	// Token: 0x0400146F RID: 5231
	public Transform enterParentTransform;

	// Token: 0x04001470 RID: 5232
	public Vector3 enterPosition;

	// Token: 0x04001471 RID: 5233
	public Vector3 enterRotation;

	// Token: 0x04001472 RID: 5234
	public Vector2 pitchRestriction;

	// Token: 0x04001473 RID: 5235
	public Vector2 yawRestriction;

	// Token: 0x04001474 RID: 5236
	public bool bKeep3rdPersonModelVisible;

	// Token: 0x04001475 RID: 5237
	public bool bAllow3rdPerson;

	// Token: 0x04001476 RID: 5238
	public bool bReplaceLocalInventory;

	// Token: 0x04001477 RID: 5239
	public List<AttachedToEntitySlotExit> exits = new List<AttachedToEntitySlotExit>();
}
