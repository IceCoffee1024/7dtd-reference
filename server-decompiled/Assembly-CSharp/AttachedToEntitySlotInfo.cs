using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003F9 RID: 1017
[Preserve]
public class AttachedToEntitySlotInfo
{
	// Token: 0x0400148C RID: 5260
	public int slotIdx;

	// Token: 0x0400148D RID: 5261
	public Transform enterParentTransform;

	// Token: 0x0400148E RID: 5262
	public Vector3 enterPosition;

	// Token: 0x0400148F RID: 5263
	public Vector3 enterRotation;

	// Token: 0x04001490 RID: 5264
	public Vector2 pitchRestriction;

	// Token: 0x04001491 RID: 5265
	public Vector2 yawRestriction;

	// Token: 0x04001492 RID: 5266
	public bool bKeep3rdPersonModelVisible;

	// Token: 0x04001493 RID: 5267
	public bool bAllow3rdPerson;

	// Token: 0x04001494 RID: 5268
	public bool bHolsterHeldItem;

	// Token: 0x04001495 RID: 5269
	public List<AttachedToEntitySlotExit> exits = new List<AttachedToEntitySlotExit>();
}
