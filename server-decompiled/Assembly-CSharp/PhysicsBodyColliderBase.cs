using System;
using System.IO;
using UnityEngine;

// Token: 0x020008F5 RID: 2293
public abstract class PhysicsBodyColliderBase : IBodyColliderInstance
{
	// Token: 0x06004267 RID: 16999 RVA: 0x0019F1D0 File Offset: 0x0019D3D0
	public PhysicsBodyColliderBase(Transform _transform, PhysicsBodyColliderConfiguration _config)
	{
		this.transform = _transform;
		this.config = _config;
		this.rigidBody = _transform.GetComponent<Rigidbody>();
		if (this.rigidBody == null)
		{
			this.rigidBody = _transform.gameObject.AddComponent<Rigidbody>();
		}
		this.transform.tag = this.config.Tag;
		this.transform.gameObject.layer = this.config.CollisionLayer;
		this.enableRigidBody(false);
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x000876FD File Offset: 0x000858FD
	public void WriteToXML(TextWriter stream)
	{
		throw new NotImplementedException();
	}

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06004269 RID: 17001 RVA: 0x0019F254 File Offset: 0x0019D454
	public Transform Transform
	{
		get
		{
			return this.transform;
		}
	}

	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x0600426A RID: 17002 RVA: 0x0019F25C File Offset: 0x0019D45C
	public PhysicsBodyColliderConfiguration Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x170006FB RID: 1787
	// (set) Token: 0x0600426B RID: 17003
	public abstract EnumColliderMode ColliderMode { set; }

	// Token: 0x0600426C RID: 17004 RVA: 0x0019F264 File Offset: 0x0019D464
	[PublicizedFrom(EAccessModifier.Protected)]
	public void enableRigidBody(bool _enabled)
	{
		if (this.rigidBody)
		{
			bool isKinematic = this.rigidBody.isKinematic;
			this.rigidBody.isKinematic = !_enabled;
			if (_enabled)
			{
				if (isKinematic)
				{
					this.rigidBody.velocity = Vector3.zero;
					this.rigidBody.angularVelocity = Vector3.zero;
				}
				this.rigidBody.useGravity = true;
				this.rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
				return;
			}
			this.rigidBody.interpolation = RigidbodyInterpolation.None;
		}
	}

	// Token: 0x04003584 RID: 13700
	[PublicizedFrom(EAccessModifier.Private)]
	public Rigidbody rigidBody;

	// Token: 0x04003585 RID: 13701
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform transform;

	// Token: 0x04003586 RID: 13702
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyColliderConfiguration config;
}
