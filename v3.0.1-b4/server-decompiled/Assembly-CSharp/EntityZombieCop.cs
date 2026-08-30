using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004F1 RID: 1265
[Preserve]
public class EntityZombieCop : EntityZombie
{
	// Token: 0x06002967 RID: 10599 RVA: 0x00104785 File Offset: 0x00102985
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x00104790 File Offset: 0x00102990
	public override void PostInit()
	{
		base.PostInit();
		this.inventory.SetItem(0, this.inventory.GetBareHandItemValue(), 1, true);
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x001047B4 File Offset: 0x001029B4
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

	// Token: 0x0600296A RID: 10602 RVA: 0x00104878 File Offset: 0x00102A78
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

	// Token: 0x0600296B RID: 10603 RVA: 0x00104A8A File Offset: 0x00102C8A
	public override float GetMoveSpeed()
	{
		if (this.ticksToExplode != 0)
		{
			return 0f;
		}
		return base.GetMoveSpeed();
	}

	// Token: 0x0600296C RID: 10604 RVA: 0x00104AA0 File Offset: 0x00102CA0
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

	// Token: 0x0600296D RID: 10605 RVA: 0x00104AC5 File Offset: 0x00102CC5
	public override bool IsAttackValid()
	{
		return !this.isPrimed && base.IsAttackValid();
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x00104AD7 File Offset: 0x00102CD7
	public override void ProcessDamageResponseLocal(DamageResponse _dmResponse)
	{
		if (!this.isEntityRemote && !this.isPrimed && (_dmResponse.HitBodyPart & EnumBodyPartHit.Special) > EnumBodyPartHit.None && _dmResponse.Source.canHitSpecialBodyParts)
		{
			this.HandlePrimingDetonator(-1f);
		}
		base.ProcessDamageResponseLocal(_dmResponse);
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x00104B18 File Offset: 0x00102D18
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

	// Token: 0x06002970 RID: 10608 RVA: 0x00104B6C File Offset: 0x00102D6C
	public void HandlePrimingDetonator(float overrideDelay = -1f)
	{
		this.PlayOneShot(this.warnSoundName, false, false, false, null, 1f);
		this.isPrimed = true;
		this.ticksToStartToExplode = (int)(((overrideDelay > 0f) ? overrideDelay : this.explodeDelay) * 20f);
		this.PrimeDetonator();
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityPrimeDetonator>().Setup(this), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x04001F5D RID: 8029
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToStartToExplode;

	// Token: 0x04001F5E RID: 8030
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToExplode;

	// Token: 0x04001F5F RID: 8031
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float explodeDelay = 5f;

	// Token: 0x04001F60 RID: 8032
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float explodeHealthThreshold = 0.4f;

	// Token: 0x04001F61 RID: 8033
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isPrimed;

	// Token: 0x04001F62 RID: 8034
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string warnSoundName;

	// Token: 0x04001F63 RID: 8035
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string tickSoundName;

	// Token: 0x04001F64 RID: 8036
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float tickSoundDelayStart = 1f;

	// Token: 0x04001F65 RID: 8037
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float tickSoundDelayScale = 1f;

	// Token: 0x04001F66 RID: 8038
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float tickSoundDelay;
}
