using System;
using System.IO;
using UnityEngine;

// Token: 0x02000906 RID: 2310
public abstract class PhysicsBodyColliderBase : IBodyColliderInstance
{
	// Token: 0x0600425E RID: 16990 RVA: 0x001A13AC File Offset: 0x0019F5AC
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

	// Token: 0x0600425F RID: 16991 RVA: 0x0008806C File Offset: 0x0008626C
	public void WriteToXML(TextWriter stream)
	{
		throw new NotImplementedException();
	}

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06004260 RID: 16992 RVA: 0x001A1430 File Offset: 0x0019F630
	public Transform Transform
	{
		get
		{
			return this.transform;
		}
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06004261 RID: 16993 RVA: 0x001A1438 File Offset: 0x0019F638
	public PhysicsBodyColliderConfiguration Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x17000726 RID: 1830
	// (set) Token: 0x06004262 RID: 16994
	public abstract EnumColliderMode ColliderMode { set; }

	// Token: 0x06004263 RID: 16995 RVA: 0x001A1440 File Offset: 0x0019F640
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

	// Token: 0x040035F9 RID: 13817
	[PublicizedFrom(EAccessModifier.Private)]
	public Rigidbody rigidBody;

	// Token: 0x040035FA RID: 13818
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform transform;

	// Token: 0x040035FB RID: 13819
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyColliderConfiguration config;
}
