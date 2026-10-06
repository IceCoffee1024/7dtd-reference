using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public abstract class EntityEnemy : EntityAlive
{
	// Token: 0x06002599 RID: 9625 RVA: 0x000DF028 File Offset: 0x000DD228
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x000E58DB File Offset: 0x000E3ADB
	public override void PostInit()
	{
		base.PostInit();
		if (!this.isEntityRemote)
		{
			this.IsBloodMoon = this.world.aiDirector.BloodMoonComponent.BloodMoonActive;
		}
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsDrawMapIcon()
	{
		return true;
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x000E5906 File Offset: 0x000E3B06
	public override Vector3 GetMapIconScale()
	{
		return new Vector3(0.75f, 0.75f, 1f);
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x000E591C File Offset: 0x000E3B1C
	public override bool IsSavedToFile()
	{
		return (base.GetSpawnerSource() != EnumSpawnerSource.Dynamic || this.IsDead()) && base.IsSavedToFile();
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x000E5937 File Offset: 0x000E3B37
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canDespawn()
	{
		return (!this.IsHordeZombie || this.world.GetPlayers().Count == 0) && base.canDespawn();
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isRadiationSensitive()
	{
		return false;
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000E595B File Offset: 0x000E3B5B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityTargeted(EntityAlive target)
	{
		base.OnEntityTargeted(target);
		if (!this.isEntityRemote && base.GetSpawnerSource() != EnumSpawnerSource.Dynamic && target is EntityPlayer)
		{
			this.world.aiDirector.NotifyIntentToAttack(this, target as EntityPlayer);
		}
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000D9AD8 File Offset: 0x000D7CD8
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float _impulseScale)
	{
		return base.DamageEntity(_damageSource, _strength, _criticalHit, _impulseScale);
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x000DB13C File Offset: 0x000D933C
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityEnemy()
	{
	}

	// Token: 0x04001BFA RID: 7162
	public bool IsHordeZombie;
}
