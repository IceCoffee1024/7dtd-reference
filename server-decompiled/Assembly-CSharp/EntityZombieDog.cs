using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004FD RID: 1277
[Preserve]
public class EntityZombieDog : EntityEnemyAnimal
{
	// Token: 0x06002991 RID: 10641 RVA: 0x00106554 File Offset: 0x00104754
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

	// Token: 0x06002992 RID: 10642 RVA: 0x0010658C File Offset: 0x0010478C
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		this.timeToDie = this.world.worldTime + 1800UL + (ulong)(22000f * this.rand.RandomFloat);
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x001065C2 File Offset: 0x001047C2
	public override void OnUpdateLive()
	{
		base.OnUpdateLive();
		if (this.world.worldTime >= this.timeToDie && !this.isEntityRemote)
		{
			this.Kill(DamageResponse.New(true));
		}
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x001065F1 File Offset: 0x001047F1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int GetMaxAttackTime()
	{
		return 30;
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x001065F5 File Offset: 0x001047F5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityTargeted(EntityAlive target)
	{
		base.OnEntityTargeted(target);
	}

	// Token: 0x04001F9E RID: 8094
	public ulong timeToDie;
}
