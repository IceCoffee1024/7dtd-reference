using System;
using System.IO;
using UnityEngine;

// Token: 0x02000910 RID: 2320
public abstract class PhysicsBodyColliderBase : IBodyColliderInstance
{
	// Token: 0x06004281 RID: 17025 RVA: 0x001A2454 File Offset: 0x001A0654
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

	// Token: 0x06004282 RID: 17026 RVA: 0x000882A0 File Offset: 0x000864A0
	public void WriteToXML(TextWriter stream)
	{
		throw new NotImplementedException();
	}

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06004283 RID: 17027 RVA: 0x001A24D8 File Offset: 0x001A06D8
	public Transform Transform
	{
		get
		{
			return this.transform;
		}
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06004284 RID: 17028 RVA: 0x001A24E0 File Offset: 0x001A06E0
	public PhysicsBodyColliderConfiguration Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x1700072C RID: 1836
	// (set) Token: 0x06004285 RID: 17029
	public abstract EnumColliderMode ColliderMode { set; }

	// Token: 0x06004286 RID: 17030 RVA: 0x001A24E8 File Offset: 0x001A06E8
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

	// Token: 0x04003619 RID: 13849
	[PublicizedFrom(EAccessModifier.Private)]
	public Rigidbody rigidBody;

	// Token: 0x0400361A RID: 13850
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform transform;

	// Token: 0x0400361B RID: 13851
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyColliderConfiguration config;
}
