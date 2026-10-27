using System;
using UnityEngine;

// Token: 0x02000912 RID: 2322
public class PhysicsBodySphereCollider : PhysicsBodyColliderBase
{
	// Token: 0x06004289 RID: 17033 RVA: 0x001A2572 File Offset: 0x001A0772
	public PhysicsBodySphereCollider(SphereCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.collider = _collider;
	}

	// Token: 0x1700072E RID: 1838
	// (set) Token: 0x0600428A RID: 17034 RVA: 0x001A25A0 File Offset: 0x001A07A0
	public override EnumColliderMode ColliderMode
	{
		set
		{
			if (this.collider == null)
			{
				return;
			}
			switch (value)
			{
			case EnumColliderMode.Disabled:
				this.collider.enabled = false;
				this.collider.radius = this.radius;
				this.collider.center = this.center;
				base.enableRigidBody(false);
				return;
			case EnumColliderMode.Collision:
				base.enableRigidBody(false);
				if ((base.Config.EnabledFlags & EnumColliderEnabledFlags.Collision) != EnumColliderEnabledFlags.Disabled)
				{
					this.collider.enabled = true;
					this.collider.radius = this.radius * base.Config.CollisionScale.x;
					this.collider.center = this.center + base.Config.CollisionOffset;
					this.collider.gameObject.layer = base.Config.CollisionLayer;
					return;
				}
				this.collider.enabled = false;
				return;
			case EnumColliderMode.Ragdoll:
			case EnumColliderMode.RagdollDead:
				if ((base.Config.EnabledFlags & EnumColliderEnabledFlags.Ragdoll) != EnumColliderEnabledFlags.Disabled)
				{
					this.collider.enabled = true;
					this.collider.radius = this.radius * base.Config.RagdollScale.x;
					this.collider.center = this.center + base.Config.RagdollOffset;
					this.collider.gameObject.layer = ((value == EnumColliderMode.Ragdoll) ? base.Config.RagdollLayer : 17);
					base.enableRigidBody(true);
					return;
				}
				this.collider.enabled = false;
				base.enableRigidBody(false);
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x0400361C RID: 13852
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x0400361D RID: 13853
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x0400361E RID: 13854
	[PublicizedFrom(EAccessModifier.Private)]
	public SphereCollider collider;
}
