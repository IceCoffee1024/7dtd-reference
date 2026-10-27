using System;
using UnityEngine;

// Token: 0x02000914 RID: 2324
public class PhysicsBodyBoxCollider : PhysicsBodyColliderBase
{
	// Token: 0x0600428D RID: 17037 RVA: 0x001A295E File Offset: 0x001A0B5E
	public PhysicsBodyBoxCollider(BoxCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.size = _collider.size;
		this.collider = _collider;
	}

	// Token: 0x17000730 RID: 1840
	// (set) Token: 0x0600428E RID: 17038 RVA: 0x001A298C File Offset: 0x001A0B8C
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

	// Token: 0x04003623 RID: 13859
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x04003624 RID: 13860
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 size;

	// Token: 0x04003625 RID: 13861
	[PublicizedFrom(EAccessModifier.Private)]
	public BoxCollider collider;
}
