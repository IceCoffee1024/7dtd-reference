using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004FC RID: 1276
[Preserve]
public class EntityZombieCop : EntityZombie
{
	// Token: 0x06002986 RID: 10630 RVA: 0x001060C5 File Offset: 0x001042C5
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
	}

	// Token: 0x06002987 RID: 10631 RVA: 0x001060D0 File Offset: 0x001042D0
	public override void PostInit()
	{
		base.PostInit();
		this.inventory.SetItem(0, base.Hand.BareHandItemValue, 1);
	}

	// Token: 0x06002988 RID: 10632 RVA: 0x001060F0 File Offset: 0x001042F0
	public override void CopyPropertiesFromEntityClass()
	{
		DynamicProperties properties = EntityClass.list[this.entityClass].Properties;
		properties.ParseFloat(EntityClass.PropExplodeDelay, ref this.explodeDelay);
		properties.ParseFloat(EntityClass.PropExplodeHealthThreshold, ref this.explodeHealthThreshold);
		properties.ParseString(EntityClass.PropSoundExplodeWarn, ref this.warnSoundName);
		properties.ParseString(EntityClass.PropSoundTick, ref this.tickSoundName);
		if (this.tickSoundName != null)
		{
			string[] array = this.tickSoundName.Split(',', StringSplitOptions.None);
			this.tickSoundName = array[0];
			if (array.Length >= 2)
			{
				this.tickSoundDelayStart = StringParsers.ParseFloat(array[1], 0, -1, NumberStyles.Any);
				if (array.Length >= 3)
				{
					this.tickSoundDelayScale = StringParsers.ParseFloat(array[2], 0, -1, NumberStyles.Any);
				}
			}
		}
		base.CopyPropertiesFromEntityClass();
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x001061B4 File Offset: 0x001043B4
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		if (this.isEntityRemote)
		{
			return;
		}
		if (!this.isPrimed && !this.IsSleeping && !this.Buffs.HasBuff("buffShocked"))
		{
			float num = (float)this.Health;
			if (num > 0f && num < (float)this.GetMaxHealth() * this.explodeHealthThreshold)
			{
				this.isPrimed = true;
				this.ticksToStartToExplode = (int)(this.explodeDelay * 20f);
				this.PlayOneShot(this.warnSoundName, false, false, false, null, 1f);
			}
		}
		if (this.isPrimed && !this.IsDead())
		{
			if (this.ticksToStartToExplode > 0)
			{
				this.ticksToStartToExplode--;
				if (this.ticksToStartToExplode == 0)
				{
					this.SpecialAttack2 = true;
					this.ticksToExplode = (int)(this.explodeDelay / 5f * 1.5f * 20f);
				}
			}
			if (this.ticksToExplode > 0)
			{
				this.ticksToExplode--;
				if (this.ticksToExplode == 0)
				{
					base.NotifySleeperDeath();
					this.SetModelLayer(2, false, null);
					this.ticksToExplode = -1;
					GameManager.Instance.ExplosionServer(base.GetPosition(), World.worldToBlockPos(base.GetPosition()), base.transform.rotation, EntityClass.list[this.entityClass].explosionData, this.entityId, 0f, false, null);
					this.timeStayAfterDeath = 0;
					this.SetDead();
				}
			}
			this.tickSoundDelay -= 0.05f;
			if (this.tickSoundDelay <= 0f)
			{
				this.tickSoundDelayStart *= this.tickSoundDelayScale;
				this.tickSoundDelay = this.tickSoundDelayStart;
				if (this.tickSoundDelay < 0.2f)
				{
					this.tickSoundDelay = 0.2f;
				}
				this.PlayOneShot(this.tickSoundName, false, false, false, null, 1f);
			}
		}
		if (this.ticksToExplode < 0)
		{
			this.motion.x = this.motion.x * 0.7f;
			this.motion.z = this.motion.z * 0.7f;
		}
	}

	// Token: 0x0600298A RID: 10634 RVA: 0x001063C6 File Offset: 0x001045C6
	public override float GetMoveSpeed()
	{
		if (this.ticksToExplode != 0)
		{
			return 0f;
		}
		return base.GetMoveSpeed();
	}

	// Token: 0x0600298B RID: 10635 RVA: 0x001063DC File Offset: 0x001045DC
	public override float GetMoveSpeedAggro()
	{
		if (this.ticksToExplode != 0)
		{
			return 0f;
		}
		if (this.isPrimed)
		{
			return this.moveSpeedAggroMax;
		}
		return base.GetMoveSpeedAggro();
	}

	// Token: 0x0600298C RID: 10636 RVA: 0x00106401 File Offset: 0x00104601
	public override bool IsAttackValid()
	{
		return !this.isPrimed && base.IsAttackValid();
	}

	// Token: 0x0600298D RID: 10637 RVA: 0x00106413 File Offset: 0x00104613
	public override void ProcessDamageResponseLocal(DamageResponse _dmResponse)
	{
		if (!this.isEntityRemote && !this.isPrimed && (_dmResponse.HitBodyPart & EnumBodyPartHit.Special) > EnumBodyPartHit.None && _dmResponse.Source.canHitSpecialBodyParts)
		{
			this.HandlePrimingDetonator(-1f);
		}
		base.ProcessDamageResponseLocal(_dmResponse);
	}

	// Token: 0x0600298E RID: 10638 RVA: 0x00106454 File Offset: 0x00104654
	public void PrimeDetonator()
	{
		Detonator componentInChildren = base.gameObject.GetComponentInChildren<Detonator>(true);
		if (componentInChildren != null)
		{
			componentInChildren.PulseRateScale = 1f;
			componentInChildren.gameObject.GetComponent<Light>().color = Color.red;
			componentInChildren.StartCountdown();
			return;
		}
		Log.Out("PrimeDetonator found no Detonator component");
	}

	// Token: 0x0600298F RID: 10639 RVA: 0x001064A8 File Offset: 0x001046A8
	public void HandlePrimingDetonator(float overrideDelay = -1f)
	{
		this.PlayOneShot(this.warnSoundName, false, false, false, null, 1f);
		this.isPrimed = true;
		this.ticksToStartToExplode = (int)(((overrideDelay > 0f) ? overrideDelay : this.explodeDelay) * 20f);
		this.PrimeDetonator();
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityPrimeDetonator>().Setup(this), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x04001F94 RID: 8084
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToStartToExplode;

	// Token: 0x04001F95 RID: 8085
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToExplode;

	// Token: 0x04001F96 RID: 8086
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float explodeDelay = 5f;

	// Token: 0x04001F97 RID: 8087
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float explodeHealthThreshold = 0.4f;

	// Token: 0x04001F98 RID: 8088
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isPrimed;

	// Token: 0x04001F99 RID: 8089
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string warnSoundName;

	// Token: 0x04001F9A RID: 8090
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string tickSoundName;

	// Token: 0x04001F9B RID: 8091
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float tickSoundDelayStart = 1f;

	// Token: 0x04001F9C RID: 8092
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float tickSoundDelayScale = 1f;

	// Token: 0x04001F9D RID: 8093
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float tickSoundDelay;
}
