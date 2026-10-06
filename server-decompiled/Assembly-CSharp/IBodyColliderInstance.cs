using System;
using UnityEngine;

// Token: 0x02000903 RID: 2307
public interface IBodyColliderInstance
{
	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06004249 RID: 16969
	Transform Transform { get; }

	// Token: 0x1700071F RID: 1823
	// (set) Token: 0x0600424A RID: 16970
	EnumColliderMode ColliderMode { set; }

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x0600424B RID: 16971
	PhysicsBodyColliderConfiguration Config { get; }
}
