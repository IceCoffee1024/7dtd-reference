using System;
using UnityEngine;

// Token: 0x02000902 RID: 2306
public class PhysicsBodyNullCollider : PhysicsBodyColliderBase
{
	// Token: 0x060042D2 RID: 17106 RVA: 0x001A13BC File Offset: 0x0019F5BC
	public PhysicsBodyNullCollider(Transform _transform, PhysicsBodyColliderConfiguration _config)
		: base(_transform, _config)
	{
	}

	// Token: 0x17000708 RID: 1800
	// (set) Token: 0x060042D3 RID: 17107 RVA: 0x000027FC File Offset: 0x000009FC
	public override EnumColliderMode ColliderMode
	{
		set
		{
		}
	}
}
