using System;
using UnityEngine;

// Token: 0x02000905 RID: 2309
public class PhysicsBodyBoxCollider : PhysicsBodyColliderBase
{
	// Token: 0x060042D8 RID: 17112 RVA: 0x001A17B2 File Offset: 0x0019F9B2
	public PhysicsBodyBoxCollider(BoxCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.size = _collider.size;
		this.collider = _collider;
	}

	// Token: 0x1700070B RID: 1803
	// (set) Token: 0x060042D9 RID: 17113 RVA: 0x001A17E0 File Offset: 0x0019F9E0
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
				this.collider.size = this.size;
				this.collider.center = this.center;
				base.enableRigidBody(false);
				return;
			case EnumColliderMode.Collision:
				base.enableRigidBody(false);
				if ((base.Config.EnabledFlags & EnumColliderEnabledFlags.Collision) != EnumColliderEnabledFlags.Disabled)
				{
					this.collider.enabled = true;
					this.collider.size = Vector3.Scale(this.size, base.Config.CollisionScale);
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
					this.collider.size = Vector3.Scale(this.size, base.Config.RagdollScale);
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

	// Token: 0x0400360F RID: 13839
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003610 RID: 13840
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 size;

	// Token: 0x04003611 RID: 13841
	[PublicizedFrom(EAccessModifier.Private)]
	public BoxCollider collider;
}
