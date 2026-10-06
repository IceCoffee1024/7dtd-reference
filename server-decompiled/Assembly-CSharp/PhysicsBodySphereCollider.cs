using System;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public class PhysicsBodySphereCollider : PhysicsBodyColliderBase
{
	// Token: 0x06004266 RID: 16998 RVA: 0x001A14CA File Offset: 0x0019F6CA
	public PhysicsBodySphereCollider(SphereCollider _collider, PhysicsBodyColliderConfiguration _config)
		: base(_collider.transform, _config)
	{
		this.center = _collider.center;
		this.radius = _collider.radius;
		this.collider = _collider;
	}

	// Token: 0x17000728 RID: 1832
	// (set) Token: 0x06004267 RID: 16999 RVA: 0x001A14F8 File Offset: 0x0019F6F8
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

	// Token: 0x040035FC RID: 13820
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 center;

	// Token: 0x040035FD RID: 13821
	[PublicizedFrom(EAccessModifier.Private)]
	public float radius;

	// Token: 0x040035FE RID: 13822
	[PublicizedFrom(EAccessModifier.Private)]
	public SphereCollider collider;
}
