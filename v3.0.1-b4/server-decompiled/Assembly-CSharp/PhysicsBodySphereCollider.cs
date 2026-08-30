using System;
using UnityEngine;

// Token: 0x020008F7 RID: 2295
public class PhysicsBodySphereCollider : PhysicsBodyColliderBase
{
	// Token: 0x0600426F RID: 17007 RVA: 0x0019F2EE File Offset: 0x0019D4EE
	public PhysicsBodySphereCollider(SphereCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.collider = _collider;
	}

	// Token: 0x170006FD RID: 1789
	// (set) Token: 0x06004270 RID: 17008 RVA: 0x0019F31C File Offset: 0x0019D51C
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

	// Token: 0x04003587 RID: 13703
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003588 RID: 13704
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x04003589 RID: 13705
	[PublicizedFrom(EAccessModifier.Private)]
	public SphereCollider collider;
}
