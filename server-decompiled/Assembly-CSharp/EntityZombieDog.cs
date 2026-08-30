using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004F2 RID: 1266
[Preserve]
public class EntityZombieDog : EntityEnemyAnimal
{
	// Token: 0x06002972 RID: 10610 RVA: 0x00104C18 File Offset: 0x00102E18
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		Transform transform = base.transform.Find("Graphics/BlobShadowProjector");
		if (transform)
		{
			transform.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x00104C50 File Offset: 0x00102E50
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		this.timeToDie = this.world.worldTime + 1800UL + (ulong)(22000f * this.rand.RandomFloat);
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x00104C86 File Offset: 0x00102E86
	public override void OnUpdateLive()
	{
		base.OnUpdateLive();
		if (this.world.worldTime >= this.timeToDie && !this.isEntityRemote)
		{
			this.Kill(DamageResponse.New(true));
		}
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x00104CB5 File Offset: 0x00102EB5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int GetMaxAttackTime()
	{
		return 30;
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x00104CB9 File Offset: 0x00102EB9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityTargeted(EntityAlive target)
	{
		base.OnEntityTargeted(target);
	}

	// Token: 0x04001F67 RID: 8039
	public ulong timeToDie;
}
