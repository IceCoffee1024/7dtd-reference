using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004FF RID: 1279
[Preserve]
public class EntityZombieDog : EntityEnemyAnimal
{
	// Token: 0x06002998 RID: 10648 RVA: 0x00106AA8 File Offset: 0x00104CA8
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

	// Token: 0x06002999 RID: 10649 RVA: 0x00106AE0 File Offset: 0x00104CE0
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		this.timeToDie = this.world.worldTime + 1800UL + (ulong)(22000f * this.rand.RandomFloat);
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x00106B16 File Offset: 0x00104D16
	public override void OnUpdateLive()
	{
		base.OnUpdateLive();
		if (this.world.worldTime >= this.timeToDie && !this.isEntityRemote)
		{
			this.Kill(DamageResponse.New(true));
		}
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x00106B45 File Offset: 0x00104D45
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int GetMaxAttackTime()
	{
		return 30;
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x00106B49 File Offset: 0x00104D49
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityTargeted(EntityAlive target)
	{
		base.OnEntityTargeted(target);
	}

	// Token: 0x04001FAE RID: 8110
	public ulong timeToDie;
}
