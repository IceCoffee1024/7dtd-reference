using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003F7 RID: 1015
[Preserve]
public class AttachedToEntitySlotInfo
{
	// Token: 0x0400147C RID: 5244
	public int slotIdx;

	// Token: 0x0400147D RID: 5245
	public Transform enterParentTransform;

	// Token: 0x0400147E RID: 5246
	public Vector3 enterPosition;

	// Token: 0x0400147F RID: 5247
	public Vector3 enterRotation;

	// Token: 0x04001480 RID: 5248
	public Vector2 pitchRestriction;

	// Token: 0x04001481 RID: 5249
	public Vector2 yawRestriction;

	// Token: 0x04001482 RID: 5250
	public bool bKeep3rdPersonModelVisible;

	// Token: 0x04001483 RID: 5251
	public bool bAllow3rdPerson;

	// Token: 0x04001484 RID: 5252
	public bool bHolsterHeldItem;

	// Token: 0x04001485 RID: 5253
	public List<AttachedToEntitySlotExit> exits = new List<AttachedToEntitySlotExit>();
}
