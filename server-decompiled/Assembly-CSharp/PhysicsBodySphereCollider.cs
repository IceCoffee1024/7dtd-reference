using System;
using UnityEngine;

// Token: 0x02000903 RID: 2307
public class PhysicsBodySphereCollider : PhysicsBodyColliderBase
{
	// Token: 0x060042D4 RID: 17108 RVA: 0x001A13C6 File Offset: 0x0019F5C6
	public PhysicsBodySphereCollider(SphereCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.collider = _collider;
	}

	// Token: 0x17000709 RID: 1801
	// (set) Token: 0x060042D5 RID: 17109 RVA: 0x001A13F4 File Offset: 0x0019F5F4
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

	// Token: 0x04003608 RID: 13832
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003609 RID: 13833
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x0400360A RID: 13834
	[PublicizedFrom(EAccessModifier.Private)]
	public SphereCollider collider;
}
