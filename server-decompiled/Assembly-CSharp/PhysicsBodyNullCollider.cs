using System;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class PhysicsBodyNullCollider : PhysicsBodyColliderBase
{
	// Token: 0x06004264 RID: 16996 RVA: 0x001A14C0 File Offset: 0x0019F6C0
	public PhysicsBodyNullCollider(Transform _transform, PhysicsBodyColliderConfiguration _config)
		: base(_transform, _config)
	{
	}

	// Token: 0x17000727 RID: 1831
	// (set) Token: 0x06004265 RID: 16997 RVA: 0x000027FC File Offset: 0x000009FC
	public override EnumColliderMode ColliderMode
	{
		set
		{
		}
	}
}
