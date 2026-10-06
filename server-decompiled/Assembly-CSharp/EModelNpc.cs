using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200051C RID: 1308
[Preserve]
public class EModelNpc : EModelBase
{
	// Token: 0x06002ACE RID: 10958 RVA: 0x0010FA34 File Offset: 0x0010DC34
	public override void Init(World _world, Entity _entity, EModelInstanceAssets _assets)
	{
		this.entity = _entity;
		this.assets = _assets;
		EntityClass entityClass = EntityClass.list[this.entity.entityClass];
		this.ragdollChance = entityClass.RagdollOnDeathChance;
		this.bHasRagdoll = entityClass.HasRagdoll;
		this.modelTransformParent = EModelBase.FindModel(base.transform);
		this.createModel(_world, entityClass);
		this.setupColliders(this.entity.transform);
		bool bIsMale = entityClass.bIsMale;
		if (GameManager.IsDedicatedServer && !this.entity.RootMotion)
		{
			this.avatarController = base.transform.gameObject.AddComponent<AvatarControllerDummy>();
			Animator[] componentsInChildren = base.transform.GetComponentsInChildren<Animator>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			if (this.modelTransformParent != null)
			{
				this.SwitchModelAndView(false, bIsMale);
			}
		}
		else
		{
			this.createAvatarController(entityClass);
			if (this.modelTransformParent != null)
			{
				this.SwitchModelAndView(false, bIsMale);
			}
			if (GameManager.IsDedicatedServer && this.avatarController != null && this.entity.RootMotion)
			{
				this.avatarController.SetVisible(true);
			}
		}
		base.LookAtInit();
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x0010FB64 File Offset: 0x0010DD64
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupColliders(Transform bodyRoot)
	{
		Transform transform = bodyRoot.FindInChilds("Position", false);
		if (transform == null)
		{
			transform = bodyRoot.FindInChilds("Origin", false);
		}
		transform.tag = "E_BP_BipedRoot";
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x0010FBA0 File Offset: 0x0010DDA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createAvatarController(EntityClass _ec)
	{
		Type type = Type.GetType(_ec.Properties.GetValue(EntityClass.PropAvatarController));
		this.avatarController = base.transform.gameObject.AddComponent(type) as AvatarController;
		(this.entity as EntityAlive).ReassignEquipmentTransforms();
	}
}
