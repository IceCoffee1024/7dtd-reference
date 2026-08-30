using System;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public abstract class EntityEnemy : EntityAlive
{
	// Token: 0x06002578 RID: 9592 RVA: 0x000DDFF0 File Offset: 0x000DC1F0
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x000E4953 File Offset: 0x000E2B53
	public override void PostInit()
	{
		base.PostInit();
		if (!this.isEntityRemote)
		{
			this.IsBloodMoon = this.world.aiDirector.BloodMoonComponent.BloodMoonActive;
		}
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsDrawMapIcon()
	{
		return true;
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x000E497E File Offset: 0x000E2B7E
	public override Vector3 GetMapIconScale()
	{
		return new Vector3(0.75f, 0.75f, 1f);
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x000E4994 File Offset: 0x000E2B94
	public override bool IsSavedToFile()
	{
		return (base.GetSpawnerSource() != EnumSpawnerSource.Dynamic || this.IsDead()) && base.IsSavedToFile();
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x000E49AF File Offset: 0x000E2BAF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canDespawn()
	{
		return (!this.IsHordeZombie || this.world.GetPlayers().Count == 0) && base.canDespawn();
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isRadiationSensitive()
	{
		return false;
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000E49D3 File Offset: 0x000E2BD3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityTargeted(EntityAlive target)
	{
		base.OnEntityTargeted(target);
		if (!this.isEntityRemote && base.GetSpawnerSource() != EnumSpawnerSource.Dynamic && target is EntityPlayer)
		{
			this.world.aiDirector.NotifyIntentToAttack(this, target as EntityPlayer);
		}
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x000D8E2C File Offset: 0x000D702C
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float _impulseScale)
	{
		return base.DamageEntity(_damageSource, _strength, _criticalHit, _impulseScale);
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000DA6BC File Offset: 0x000D88BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityEnemy()
	{
	}

	// Token: 0x04001BD7 RID: 7127
	public bool IsHordeZombie;
}
