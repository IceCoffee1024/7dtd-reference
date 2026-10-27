using System;
using UnityEngine;

// Token: 0x02000911 RID: 2321
public class PhysicsBodyNullCollider : PhysicsBodyColliderBase
{
	// Token: 0x06004287 RID: 17031 RVA: 0x001A2568 File Offset: 0x001A0768
	public PhysicsBodyNullCollider(Transform _transform, PhysicsBodyColliderConfiguration _config)
		: base(_transform, _config)
	{
	}

	// Token: 0x1700072D RID: 1837
	// (set) Token: 0x06004288 RID: 17032 RVA: 0x000027FC File Offset: 0x000009FC
	public override EnumColliderMode ColliderMode
	{
		set
		{
		}
	}
}
