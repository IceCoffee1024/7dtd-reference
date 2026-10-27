using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200051D RID: 1309
[Preserve]
public class EModelDrone : EModelBase
{
	// Token: 0x06002AD2 RID: 10962 RVA: 0x0010FEC8 File Offset: 0x0010E0C8
	public override void Init(World _world, Entity _entity, EModelInstanceAssets _assets)
	{
		this.entity = _entity;
		this.assets = _assets;
		EntityClass entityClass = EntityClass.list[this.entity.entityClass];
		this.modelTransformParent = EModelBase.FindModel(base.transform);
		this.createModel(_world, entityClass);
		if (GameManager.IsDedicatedServer && !this.entity.RootMotion)
		{
			this.avatarController = base.transform.gameObject.AddComponent<AvatarControllerDummy>();
			Animator[] componentsInChildren = base.transform.GetComponentsInChildren<Animator>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			return;
		}
		this.createAvatarController(entityClass);
		if (GameManager.IsDedicatedServer && this.avatarController != null && this.entity.RootMotion)
		{
			this.avatarController.SetVisible(true);
		}
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x0010FF95 File Offset: 0x0010E195
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createAvatarController(EntityClass _ec)
	{
		this.avatarController = base.gameObject.AddComponent<AvatarControllerDummy>();
	}
}
