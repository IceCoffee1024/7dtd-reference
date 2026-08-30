using System;
using UnityEngine;

// Token: 0x020008F6 RID: 2294
public class PhysicsBodyNullCollider : PhysicsBodyColliderBase
{
	// Token: 0x0600426D RID: 17005 RVA: 0x0019F2E4 File Offset: 0x0019D4E4
	public PhysicsBodyNullCollider(Transform _transform, PhysicsBodyColliderConfiguration _config)
		: base(_transform, _config)
	{
	}

	// Token: 0x170006FC RID: 1788
	// (set) Token: 0x0600426E RID: 17006 RVA: 0x000027FC File Offset: 0x000009FC
	public override EnumColliderMode ColliderMode
	{
		set
		{
		}
	}
}
