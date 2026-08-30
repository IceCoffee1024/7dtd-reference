using System;
using UnityEngine;

// Token: 0x020008F2 RID: 2290
public interface IBodyColliderInstance
{
	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06004252 RID: 16978
	Transform Transform { get; }

	// Token: 0x170006F4 RID: 1780
	// (set) Token: 0x06004253 RID: 16979
	EnumColliderMode ColliderMode { set; }

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06004254 RID: 16980
	PhysicsBodyColliderConfiguration Config { get; }
}
