using System;
using System.IO;
using UnityEngine;

// Token: 0x02000901 RID: 2305
public abstract class PhysicsBodyColliderBase : IBodyColliderInstance
{
	// Token: 0x060042CC RID: 17100 RVA: 0x001A12A8 File Offset: 0x0019F4A8
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

	// Token: 0x060042CD RID: 17101 RVA: 0x00087DB8 File Offset: 0x00085FB8
	public void WriteToXML(TextWriter stream)
	{
		throw new NotImplementedException();
	}

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x060042CE RID: 17102 RVA: 0x001A132C File Offset: 0x0019F52C
	public Transform Transform
	{
		get
		{
			return this.transform;
		}
	}

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x060042CF RID: 17103 RVA: 0x001A1334 File Offset: 0x0019F534
	public PhysicsBodyColliderConfiguration Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x17000707 RID: 1799
	// (set) Token: 0x060042D0 RID: 17104
	public abstract EnumColliderMode ColliderMode { set; }

	// Token: 0x060042D1 RID: 17105 RVA: 0x001A133C File Offset: 0x0019F53C
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

	// Token: 0x04003605 RID: 13829
	[PublicizedFrom(EAccessModifier.Private)]
	public Rigidbody rigidBody;

	// Token: 0x04003606 RID: 13830
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform transform;

	// Token: 0x04003607 RID: 13831
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyColliderConfiguration config;
}
