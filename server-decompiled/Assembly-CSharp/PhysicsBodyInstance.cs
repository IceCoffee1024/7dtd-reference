using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000905 RID: 2309
public class PhysicsBodyInstance
{
	// Token: 0x06004259 RID: 16985 RVA: 0x001A1128 File Offset: 0x0019F328
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

	// Token: 0x0600425A RID: 16986 RVA: 0x001A118C File Offset: 0x0019F38C
	public void BindColliders()
	{
		this.colliders.Clear();
		for (int i = 0; i < this.layout.Colliders.Count; i++)
		{
			PhysicsBodyColliderConfiguration physicsBodyColliderConfiguration = this.layout.Colliders[i];
			this.bindCollider(physicsBodyColliderConfiguration);
		}
	}

	// Token: 0x0600425B RID: 16987 RVA: 0x001A11D8 File Offset: 0x0019F3D8
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

	// Token: 0x0600425C RID: 16988 RVA: 0x001A1228 File Offset: 0x0019F428
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

	// Token: 0x0600425D RID: 16989 RVA: 0x001A1284 File Offset: 0x0019F484
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

	// Token: 0x040035F5 RID: 13813
	public EnumColliderMode Mode;

	// Token: 0x040035F6 RID: 13814
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform modelRoot;

	// Token: 0x040035F7 RID: 13815
	[PublicizedFrom(EAccessModifier.Private)]
	public PhysicsBodyLayout layout;

	// Token: 0x040035F8 RID: 13816
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IBodyColliderInstance> colliders = new List<IBodyColliderInstance>();
}
