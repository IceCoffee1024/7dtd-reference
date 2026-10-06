using System;
using UnityEngine;

// Token: 0x0200090A RID: 2314
public class PhysicsBodyBoxCollider : PhysicsBodyColliderBase
{
	// Token: 0x0600426A RID: 17002 RVA: 0x001A18B6 File Offset: 0x0019FAB6
	public PhysicsBodyBoxCollider(BoxCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.size = _collider.size;
		this.collider = _collider;
	}

	// Token: 0x1700072A RID: 1834
	// (set) Token: 0x0600426B RID: 17003 RVA: 0x001A18E4 File Offset: 0x0019FAE4
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

	// Token: 0x04003603 RID: 13827
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003604 RID: 13828
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 size;

	// Token: 0x04003605 RID: 13829
	[PublicizedFrom(EAccessModifier.Private)]
	public BoxCollider collider;
}
