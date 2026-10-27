using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090F RID: 2319
public class PhysicsBodyInstance
{
	// Token: 0x0600427C RID: 17020 RVA: 0x001A21D0 File Offset: 0x001A03D0
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

	// Token: 0x0600427D RID: 17021 RVA: 0x001A2234 File Offset: 0x001A0434
	public void BindColliders()
	{
		this.colliders.Clear();
		for (int i = 0; i < this.layout.Colliders.Count; i++)
		{
			PhysicsBodyColliderConfiguration physicsBodyColliderConfiguration = this.layout.Colliders[i];
			this.bindCollider(physicsBodyColliderConfiguration);
		}
	}

	// Token: 0x0600427E RID: 17022 RVA: 0x001A2280 File Offset: 0x001A0480
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

	// Token: 0x0600427F RID: 17023 RVA: 0x001A22D0 File Offset: 0x001A04D0
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

	// Token: 0x06004280 RID: 17024 RVA: 0x001A232C File Offset: 0x001A052C
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

	// Token: 0x04003615 RID: 13845
	public EnumColliderMode Mode;

	// Token: 0x04003616 RID: 13846
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modelRoot;

	// Token: 0x04003617 RID: 13847
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyLayout layout;

	// Token: 0x04003618 RID: 13848
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IBodyColliderInstance> colliders = new List<IBodyColliderInstance>();
}
