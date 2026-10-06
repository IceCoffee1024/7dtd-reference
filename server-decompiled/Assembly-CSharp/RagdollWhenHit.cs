using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
[RequireComponent(typeof(Collider))]
public class RagdollWhenHit : RootTransformRefEntity
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600060E RID: 1550 RVA: 0x0002B72B File Offset: 0x0002992B
	public Entity theEntity
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this._entity == null && this.RootTransform != null)
			{
				this._entity = this.RootTransform.GetComponent<Entity>();
			}
			return this._entity;
		}
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0002B760 File Offset: 0x00029960
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		CapsuleCollider component = base.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			this._radius = component.radius;
			this._offset = component.center;
			return;
		}
		SphereCollider component2 = base.GetComponent<SphereCollider>();
		if (component2 != null)
		{
			this._radius = component2.radius;
			this._offset = component2.center;
			return;
		}
		BoxCollider component3 = base.GetComponent<BoxCollider>();
		this._radius = Mathf.Max(new float[]
		{
			component3.size.x,
			component3.size.y,
			component3.size.z
		});
		this._offset = component3.center;
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002B813 File Offset: 0x00029A13
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this._pos = base.transform.position;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0002B828 File Offset: 0x00029A28
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		Vector3 vector = base.transform.position - this._pos;
		if (vector.sqrMagnitude > 0.001f)
		{
			float magnitude = vector.magnitude;
			vector.Normalize();
			RaycastHit raycastHit;
			if (Physics.SphereCast(this._pos + this._offset, this._radius, vector, out raycastHit, magnitude, 65536))
			{
				base.enabled = false;
				if (this.theEntity != null)
				{
					DamageResponse damageResponse = DamageResponse.New(false);
					damageResponse.ImpulseScale = 0f;
					this.theEntity.emodel.DoRagdoll(in damageResponse, EModelBase.RagdollMode.Default, 999999f);
					return;
				}
			}
			else
			{
				this._pos = base.transform.position;
			}
		}
	}

	// Token: 0x040006BD RID: 1725
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Entity _entity;

	// Token: 0x040006BE RID: 1726
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public SphereCollider _sphere;

	// Token: 0x040006BF RID: 1727
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BoxCollider _box;

	// Token: 0x040006C0 RID: 1728
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CapsuleCollider _capsule;

	// Token: 0x040006C1 RID: 1729
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool _hasFrame;

	// Token: 0x040006C2 RID: 1730
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 _pos;

	// Token: 0x040006C3 RID: 1731
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 _offset;

	// Token: 0x040006C4 RID: 1732
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _radius;
}
