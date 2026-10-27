using System;
using UnityEngine;

// Token: 0x02000913 RID: 2323
public class PhysicsBodyCapsuleCollider : PhysicsBodyColliderBase
{
	// Token: 0x0600428B RID: 17035 RVA: 0x001A2733 File Offset: 0x001A0933
	public PhysicsBodyCapsuleCollider(CapsuleCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.height = _collider.height;
		this.collider = _collider;
	}

	// Token: 0x1700072F RID: 1839
	// (set) Token: 0x0600428C RID: 17036 RVA: 0x001A2770 File Offset: 0x001A0970
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

	// Token: 0x0400361F RID: 13855
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003620 RID: 13856
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x04003621 RID: 13857
	[PublicizedFrom(EAccessModifier.Private)]
	public float height;

	// Token: 0x04003622 RID: 13858
	[PublicizedFrom(EAccessModifier.Private)]
	public CapsuleCollider collider;
}
