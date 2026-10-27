using System;
using UnityEngine;

// Token: 0x0200090D RID: 2317
public interface IBodyColliderInstance
{
	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x0600426C RID: 17004
	Transform Transform { get; }

	// Token: 0x17000725 RID: 1829
	// (set) Token: 0x0600426D RID: 17005
	EnumColliderMode ColliderMode { set; }

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x0600426E RID: 17006
	PhysicsBodyColliderConfiguration Config { get; }
}
