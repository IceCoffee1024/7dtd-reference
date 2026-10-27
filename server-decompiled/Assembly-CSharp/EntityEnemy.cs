using System;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public abstract class EntityEnemy : EntityAlive
{
	// Token: 0x060025A0 RID: 9632 RVA: 0x000DF3AC File Offset: 0x000DD5AC
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x000E5CEB File Offset: 0x000E3EEB
	public override void PostInit()
	{
		base.PostInit();
		if (!this.isEntityRemote)
		{
			this.IsBloodMoon = this.world.aiDirector.BloodMoonComponent.BloodMoonActive;
		}
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsDrawMapIcon()
	{
		return true;
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000E5D16 File Offset: 0x000E3F16
	public override Vector3 GetMapIconScale()
	{
		return new Vector3(0.75f, 0.75f, 1f);
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x000E5D2C File Offset: 0x000E3F2C
	public override bool IsSavedToFile()
	{
		return (base.GetSpawnerSource() != EnumSpawnerSource.Dynamic || this.IsDead()) && base.IsSavedToFile();
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x000E5D47 File Offset: 0x000E3F47
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canDespawn()
	{
		return (!this.IsHordeZombie || this.world.GetPlayers().Count == 0) && base.canDespawn();
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isRadiationSensitive()
	{
		return false;
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000E5D6B File Offset: 0x000E3F6B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEntityTargeted(EntityAlive target)
	{
		base.OnEntityTargeted(target);
		if (!this.isEntityRemote && base.GetSpawnerSource() != EnumSpawnerSource.Dynamic && target is EntityPlayer)
		{
			this.world.aiDirector.NotifyIntentToAttack(this, target as EntityPlayer);
		}
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000D9E5C File Offset: 0x000D805C
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float _impulseScale)
	{
		return base.DamageEntity(_damageSource, _strength, _criticalHit, _impulseScale);
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x000DB4C0 File Offset: 0x000D96C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityEnemy()
	{
	}

	// Token: 0x04001C0A RID: 7178
	public bool IsHordeZombie;
}
