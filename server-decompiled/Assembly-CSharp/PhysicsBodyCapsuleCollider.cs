using System;
using UnityEngine;

// Token: 0x02000904 RID: 2308
public class PhysicsBodyCapsuleCollider : PhysicsBodyColliderBase
{
	// Token: 0x060042D6 RID: 17110 RVA: 0x001A1587 File Offset: 0x0019F787
	public PhysicsBodyCapsuleCollider(CapsuleCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.height = _collider.height;
		this.collider = _collider;
	}

	// Token: 0x1700070A RID: 1802
	// (set) Token: 0x060042D7 RID: 17111 RVA: 0x001A15C4 File Offset: 0x0019F7C4
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

	// Token: 0x0400360B RID: 13835
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x0400360C RID: 13836
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x0400360D RID: 13837
	[PublicizedFrom(EAccessModifier.Private)]
	public float height;

	// Token: 0x0400360E RID: 13838
	[PublicizedFrom(EAccessModifier.Private)]
	public CapsuleCollider collider;
}
