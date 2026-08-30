using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000900 RID: 2304
public class PhysicsBodyInstance
{
	// Token: 0x060042C7 RID: 17095 RVA: 0x001A1024 File Offset: 0x0019F224
	public PhysicsBodyInstance(Transform _modelRoot, PhysicsBodyLayout _layout, EnumColliderMode _initialMode)
	{
		this.modelRoot = _modelRoot;
		this.layout = _layout;
		this.Mode = _initialMode;
		this.BindColliders();
		for (int i = 0; i < this.colliders.Count; i++)
		{
			this.colliders[i].ColliderMode = _initialMode;
		}
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x001A1088 File Offset: 0x0019F288
	public void BindColliders()
	{
		this.colliders.Clear();
		for (int i = 0; i < this.layout.Colliders.Count; i++)
		{
			PhysicsBodyColliderConfiguration physicsBodyColliderConfiguration = this.layout.Colliders[i];
			this.bindCollider(physicsBodyColliderConfiguration);
		}
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x001A10D4 File Offset: 0x0019F2D4
	public void SetColliderMode(EnumColliderType colliderTypes, EnumColliderMode _mode)
	{
		this.Mode = _mode;
		for (int i = 0; i < this.colliders.Count; i++)
		{
			IBodyColliderInstance bodyColliderInstance = this.colliders[i];
			if ((bodyColliderInstance.Config.Type & colliderTypes) != EnumColliderType.None)
			{
				bodyColliderInstance.ColliderMode = _mode;
			}
		}
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x001A1124 File Offset: 0x0019F324
	public Transform GetTransformForColliderTag(string tag)
	{
		for (int i = 0; i < this.colliders.Count; i++)
		{
			IBodyColliderInstance bodyColliderInstance = this.colliders[i];
			if (bodyColliderInstance.Transform != null && bodyColliderInstance.Config.Tag == tag)
			{
				return bodyColliderInstance.Transform;
			}
		}
		return null;
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x001A1180 File Offset: 0x0019F380
	[PublicizedFrom(EAccessModifier.Private)]
	public void bindCollider(PhysicsBodyColliderConfiguration _bodyConfig)
	{
		Transform transform = this.modelRoot.Find(_bodyConfig.Path);
		if (transform)
		{
			BoxCollider component;
			CapsuleCollider component2;
			SphereCollider component3;
			if ((component = transform.GetComponent<BoxCollider>()) != null)
			{
				this.colliders.Add(new PhysicsBodyBoxCollider(component, _bodyConfig));
			}
			else if ((component2 = transform.GetComponent<CapsuleCollider>()) != null)
			{
				this.colliders.Add(new PhysicsBodyCapsuleCollider(component2, _bodyConfig));
			}
			else if ((component3 = transform.GetComponent<SphereCollider>()) != null)
			{
				this.colliders.Add(new PhysicsBodySphereCollider(component3, _bodyConfig));
			}
			else
			{
				this.colliders.Add(new PhysicsBodyNullCollider(transform, _bodyConfig));
			}
			transform.gameObject.AddMissingComponent<RootTransformRefEntity>();
			CharacterJoint component4 = transform.GetComponent<CharacterJoint>();
			if (component4)
			{
				component4.enablePreprocessing = false;
				component4.enableProjection = true;
				return;
			}
		}
		else
		{
			Entity componentInParent = this.modelRoot.GetComponentInParent<Entity>();
			Log.Warning("PhysicsBodies {0}, {1}, path not found {2}", new object[]
			{
				(componentInParent != null) ? componentInParent.GetDebugName() : this.modelRoot.name,
				_bodyConfig.Tag,
				_bodyConfig.Path
			});
		}
	}

	// Token: 0x04003601 RID: 13825
	public EnumColliderMode Mode;

	// Token: 0x04003602 RID: 13826
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modelRoot;

	// Token: 0x04003603 RID: 13827
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyLayout layout;

	// Token: 0x04003604 RID: 13828
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IBodyColliderInstance> colliders = new List<IBodyColliderInstance>();
}
