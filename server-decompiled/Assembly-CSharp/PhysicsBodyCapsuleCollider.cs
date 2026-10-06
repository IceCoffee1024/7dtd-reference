using System;
using UnityEngine;

// Token: 0x02000909 RID: 2313
public class PhysicsBodyCapsuleCollider : PhysicsBodyColliderBase
{
	// Token: 0x06004268 RID: 17000 RVA: 0x001A168B File Offset: 0x0019F88B
	public PhysicsBodyCapsuleCollider(CapsuleCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.height = _collider.height;
		this.collider = _collider;
	}

	// Token: 0x17000729 RID: 1833
	// (set) Token: 0x06004269 RID: 17001 RVA: 0x001A16C8 File Offset: 0x0019F8C8
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
				this.collider.height = this.height;
				this.collider.center = this.center;
				base.enableRigidBody(false);
				return;
			case EnumColliderMode.Collision:
				base.enableRigidBody(false);
				if ((base.Config.EnabledFlags & EnumColliderEnabledFlags.Collision) != EnumColliderEnabledFlags.Disabled)
				{
					this.collider.enabled = true;
					this.collider.radius = this.radius * base.Config.CollisionScale.x;
					this.collider.height = this.height * base.Config.CollisionScale.y;
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
					this.collider.height = this.height * base.Config.RagdollScale.y;
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

	// Token: 0x040035FF RID: 13823
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003600 RID: 13824
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x04003601 RID: 13825
	[PublicizedFrom(EAccessModifier.Private)]
	public float height;

	// Token: 0x04003602 RID: 13826
	[PublicizedFrom(EAccessModifier.Private)]
	public CapsuleCollider collider;
}
