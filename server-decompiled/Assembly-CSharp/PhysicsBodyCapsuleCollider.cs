using System;
using UnityEngine;

// Token: 0x020008F8 RID: 2296
public class PhysicsBodyCapsuleCollider : PhysicsBodyColliderBase
{
	// Token: 0x06004271 RID: 17009 RVA: 0x0019F4AF File Offset: 0x0019D6AF
	public PhysicsBodyCapsuleCollider(CapsuleCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.height = _collider.height;
		this.collider = _collider;
	}

	// Token: 0x170006FE RID: 1790
	// (set) Token: 0x06004272 RID: 17010 RVA: 0x0019F4EC File Offset: 0x0019D6EC
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

	// Token: 0x0400358A RID: 13706
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x0400358B RID: 13707
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x0400358C RID: 13708
	[PublicizedFrom(EAccessModifier.Private)]
	public float height;

	// Token: 0x0400358D RID: 13709
	[PublicizedFrom(EAccessModifier.Private)]
	public CapsuleCollider collider;
}
