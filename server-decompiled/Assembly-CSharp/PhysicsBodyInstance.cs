using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F4 RID: 2292
public class PhysicsBodyInstance
{
	// Token: 0x06004262 RID: 16994 RVA: 0x0019EF4C File Offset: 0x0019D14C
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

	// Token: 0x06004263 RID: 16995 RVA: 0x0019EFB0 File Offset: 0x0019D1B0
	public void BindColliders()
	{
		this.colliders.Clear();
		for (int i = 0; i < this.layout.Colliders.Count; i++)
		{
			PhysicsBodyColliderConfiguration physicsBodyColliderConfiguration = this.layout.Colliders[i];
			this.bindCollider(physicsBodyColliderConfiguration);
		}
	}

	// Token: 0x06004264 RID: 16996 RVA: 0x0019EFFC File Offset: 0x0019D1FC
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

	// Token: 0x06004265 RID: 16997 RVA: 0x0019F04C File Offset: 0x0019D24C
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

	// Token: 0x06004266 RID: 16998 RVA: 0x0019F0A8 File Offset: 0x0019D2A8
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

	// Token: 0x04003580 RID: 13696
	public EnumColliderMode Mode;

	// Token: 0x04003581 RID: 13697
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modelRoot;

	// Token: 0x04003582 RID: 13698
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyLayout layout;

	// Token: 0x04003583 RID: 13699
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IBodyColliderInstance> colliders = new List<IBodyColliderInstance>();
}
