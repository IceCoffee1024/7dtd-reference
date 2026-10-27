using System;
using System.Collections.Generic;
using System.Linq;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class PlayerEntityStats : EntityStats
{
	// Token: 0x06000E4F RID: 3663 RVA: 0x0005E4B7 File Offset: 0x0005C6B7
	public PlayerEntityStats()
	{
		this.Stamina = new Stat();
		this.Water = new Stat();
		this.Food = new Stat();
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0005E4EB File Offset: 0x0005C6EB
	public PlayerEntityStats(EntityPlayer _ea)
		: base(_ea)
	{
		this.localPlayer = _ea as EntityPlayerLocal;
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0005E50C File Offset: 0x0005C70C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		int num = (int)EffectManager.GetValue(PassiveEffects.HealthMax, null, 100f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Health = new Stat(Stat.StatTypes.Health, this.m_entity, (float)num, (float)num)
		{
			GainPassive = PassiveEffects.HealthGain,
			MaxPassive = PassiveEffects.HealthMax,
			LossPassive = PassiveEffects.HealthLoss
		};
		int num2 = (int)EffectManager.GetValue(PassiveEffects.StaminaMax, null, 100f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Stamina = new Stat(Stat.StatTypes.Stamina, this.m_entity, (float)num2, (float)num2)
		{
			GainPassive = PassiveEffects.StaminaGain,
			MaxPassive = PassiveEffects.StaminaMax,
			LossPassive = PassiveEffects.StaminaLoss
		};
		int num3 = (int)EffectManager.GetValue(PassiveEffects.WaterMax, null, 100f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Water = new Stat(Stat.StatTypes.Water, this.m_entity, (float)num3, (float)num3)
		{
			GainPassive = PassiveEffects.WaterGain,
			MaxPassive = PassiveEffects.WaterMax,
			LossPassive = PassiveEffects.WaterLoss
		};
		int num4 = (int)EffectManager.GetValue(PassiveEffects.FoodMax, null, 100f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Food = new Stat(Stat.StatTypes.Food, this.m_entity, (float)num4, (float)num4)
		{
			GainPassive = PassiveEffects.FoodGain,
			MaxPassive = PassiveEffects.FoodMax,
			LossPassive = PassiveEffects.FoodLoss
		};
		this.buffChangedDelegates = new List<IEntityBuffsChanged>();
		this.notificationChangedDelegates = new List<IEntityUINotificationChanged>();
		this.m_notifications = new List<EntityUINotification>();
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x0005E694 File Offset: 0x0005C894
	public override void CopyFrom(EntityStats _stats)
	{
		PlayerEntityStats playerEntityStats = (PlayerEntityStats)_stats;
		this.Health.CopyFrom(playerEntityStats.Health);
		this.Stamina.CopyFrom(playerEntityStats.Stamina);
		this.Water.CopyFrom(playerEntityStats.Water);
		this.Food.CopyFrom(playerEntityStats.Food);
		this.CoreTemp = playerEntityStats.CoreTemp;
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x0005E6F8 File Offset: 0x0005C8F8
	public override EntityStats SimpleClone()
	{
		PlayerEntityStats playerEntityStats = new PlayerEntityStats();
		playerEntityStats.Health.CopyFrom(this.Health);
		playerEntityStats.Stamina.CopyFrom(this.Stamina);
		playerEntityStats.Water.CopyFrom(this.Water);
		playerEntityStats.Food.CopyFrom(this.Food);
		playerEntityStats.CoreTemp = this.CoreTemp;
		return playerEntityStats;
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x0005E75A File Offset: 0x0005C95A
	public void AddUINotificationChangedDelegate(IEntityUINotificationChanged _uiChangedDelegate)
	{
		if (!this.notificationChangedDelegates.Contains(_uiChangedDelegate))
		{
			this.notificationChangedDelegates.Add(_uiChangedDelegate);
		}
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x0005E776 File Offset: 0x0005C976
	public void RemoveUINotificationChangedDelegate(IEntityUINotificationChanged _uiChangedDelegate)
	{
		this.notificationChangedDelegates.Remove(_uiChangedDelegate);
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0005E785 File Offset: 0x0005C985
	public void AddBuffChangedDelegate(IEntityBuffsChanged _buffChangedDelegate)
	{
		if (!this.buffChangedDelegates.Contains(_buffChangedDelegate))
		{
			this.buffChangedDelegates.Add(_buffChangedDelegate);
		}
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0005E7A1 File Offset: 0x0005C9A1
	public void RemoveBuffChangedDelegate(IEntityBuffsChanged _buffChangedDelegate)
	{
		this.buffChangedDelegates.Remove(_buffChangedDelegate);
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x0005E7B0 File Offset: 0x0005C9B0
	public override void EntityBuffAdded(BuffValue _buff)
	{
		for (int i = 0; i < this.buffChangedDelegates.Count; i++)
		{
			this.buffChangedDelegates[i].EntityBuffAdded(_buff);
		}
		BuffClass buffClass2 = _buff.BuffClass;
		if (!buffClass2.Hidden && buffClass2.Icon != null)
		{
			BuffEntityUINotification buffEntityUINotification = new BuffEntityUINotification(this.m_entity, _buff);
			this.m_notifications.Add(buffEntityUINotification);
			for (int j = 0; j < this.notificationChangedDelegates.Count; j++)
			{
				this.notificationChangedDelegates[j].EntityUINotificationAdded(buffEntityUINotification);
			}
		}
		EntityPlayerLocal entityPlayerLocal = this.m_entity as EntityPlayerLocal;
		if (entityPlayerLocal != null && !Helper.IsEditingOrPlaytesting())
		{
			BuffClass buffClass = _buff.BuffClass;
			IEnumerable<PlayerStateEventData.ActiveConditionData> enumerable = entityPlayerLocal.Buffs.ActiveBuffs.Select<BuffValue, PlayerStateEventData.ActiveConditionData>((BuffValue buff) => new PlayerStateEventData.ActiveConditionData
			{
				ConditionName = buff.BuffName,
				MaxDuration = buffClass.DurationMax,
				CurrentDuration = buff.DurationInSeconds,
				DamageType = buffClass.DamageType.ToString(),
				StackType = buffClass.StackType.ToString(),
				UpdateRate = buffClass.UpdateRateTicks
			});
			PlayerStateEventData playerStateEventData = new PlayerStateEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveID(),
				StateTimestamp = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
				TriggerReason = PlayerStateEventData.triggerReasons.ConditionAquired.ToString(),
				TriggerSource = buffClass.DamageType.ToString(),
				ConditionsAdded = _buff.BuffName,
				ConditionsRemoved = null,
				ActiveConditions = enumerable
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerStateEventData);
		}
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x0005E940 File Offset: 0x0005CB40
	public override void EntityBuffRemoved(BuffValue _buff)
	{
		for (int i = 0; i < this.buffChangedDelegates.Count; i++)
		{
			this.buffChangedDelegates[i].EntityBuffRemoved(_buff);
		}
		int j = 0;
		while (j < this.m_notifications.Count)
		{
			EntityUINotification entityUINotification = this.m_notifications[j];
			if (entityUINotification.Buff == _buff)
			{
				this.m_notifications.RemoveAt(j);
				for (int k = 0; k < this.notificationChangedDelegates.Count; k++)
				{
					this.notificationChangedDelegates[k].EntityUINotificationRemoved(entityUINotification);
				}
			}
			else
			{
				j++;
			}
		}
		EntityPlayerLocal entityPlayerLocal = this.m_entity as EntityPlayerLocal;
		if (entityPlayerLocal != null && !Helper.IsEditingOrPlaytesting())
		{
			BuffClass buffClass = _buff.BuffClass;
			IEnumerable<PlayerStateEventData.ActiveConditionData> enumerable = entityPlayerLocal.Buffs.ActiveBuffs.Select<BuffValue, PlayerStateEventData.ActiveConditionData>((BuffValue buff) => new PlayerStateEventData.ActiveConditionData
			{
				ConditionName = buff.BuffName,
				MaxDuration = buffClass.DurationMax,
				CurrentDuration = buff.DurationInSeconds,
				DamageType = buffClass.DamageType.ToString(),
				StackType = buffClass.StackType.ToString(),
				UpdateRate = buffClass.UpdateRateTicks
			});
			PlayerStateEventData playerStateEventData = new PlayerStateEventData
			{
				ServerId = Helper.GetServerId(),
				SaveId = Helper.GetSaveID(),
				StateTimestamp = DateTime.UtcNow.ToString("O"),
				ElapsedTime = (int)(entityPlayerLocal.totalTimePlayed * 60f),
				TriggerReason = PlayerStateEventData.triggerReasons.ConditionRemoved.ToString(),
				TriggerSource = buffClass.DamageType.ToString(),
				ConditionsAdded = null,
				ConditionsRemoved = _buff.BuffName,
				ActiveConditions = enumerable
			};
			ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(playerStateEventData);
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0005EAD9 File Offset: 0x0005CCD9
	public List<EntityUINotification> Notifications
	{
		get
		{
			return this.m_notifications;
		}
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x0005EAE4 File Offset: 0x0005CCE4
	public override void TickWait(ulong worldTime)
	{
		float num = 0.5f;
		if (this.waitTicks == 1)
		{
			this.UpdateWeatherStats(num, worldTime, this.m_entity.IsGodMode.Value);
		}
		if (this.waitTicks == 2)
		{
			this.UpdatePlayerFoodOT(num);
			this.UpdatePlayerWaterOT(num);
		}
		if (this.waitTicks == 3)
		{
			this.UpdatePlayerHealthOT(num);
		}
		if (this.waitTicks == 4)
		{
			this.UpdatePlayerStaminaOT(num);
		}
		if (this.waitTicks == 5)
		{
			if (this.Health.Changed)
			{
				this.SendStatChangePacket(NetPackageEntityStatChanged.EnumStat.Health);
				this.Health.Changed = false;
			}
			if (this.Stamina.Changed)
			{
				this.SendStatChangePacket(NetPackageEntityStatChanged.EnumStat.Stamina);
				this.Stamina.Changed = false;
			}
			if (this.Water.Changed)
			{
				this.SendStatChangePacket(NetPackageEntityStatChanged.EnumStat.Water);
				this.Water.Changed = false;
			}
			if (this.Food.Changed)
			{
				this.SendStatChangePacket(NetPackageEntityStatChanged.EnumStat.Food);
				this.Food.Changed = false;
			}
		}
		if (this.waitTicks == 6)
		{
			if (this.netSyncWaitTicks > 0)
			{
				this.netSyncWaitTicks--;
				return;
			}
			this.netSyncWaitTicks = 10;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityStatsBuff>().Setup(this.m_entity, null), false, -1, -1, -1, null, 192, false);
				return;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEntityStatsBuff>().Setup(this.m_entity, null), false);
		}
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x0005EC5C File Offset: 0x0005CE5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateWeatherStats(float dt, ulong worldTime, bool godMode)
	{
		this.m_amountEnclosed = this.m_entity.GetAmountEnclosed();
		this.LightInsidePer = this.m_amountEnclosed;
		this.shadePer = this.CalcShadeFromSunPercent();
		EntityAlive entity = this.m_entity;
		EntityBuffs buffs = entity.Buffs;
		float wetnessRate = entity.GetWetnessRate();
		buffs.SetCustomVar("_wetnessrate", wetnessRate, true, CVarOperation.set, false);
		float outsideTemperature = this.GetOutsideTemperature();
		buffs.SetCustomVar("_outsidetemp", outsideTemperature, true, CVarOperation.set, false);
		float num = (this.localPlayer ? this.localPlayer.shelterPercent : 0f);
		buffs.SetCustomVar("_sheltered", num, true, CVarOperation.set, false);
		if (!World.TemperatureSurvival || !EntityStats.WeatherSurvivalEnabled || WeatherManager.inWeatherGracePeriod || entity.biomeStandingOn == null || entity.IsGodMode.Value || buffs.HasBuff("god"))
		{
			buffs.SetCustomVar("_degreesabsorbed", 0f, true, CVarOperation.set, false);
			buffs.SetCustomVar("_coretemp", 70f, true, CVarOperation.set, false);
			buffs.SetCustomVar("_shaded", this.shadePer, true, CVarOperation.set, false);
			return;
		}
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x0005ED70 File Offset: 0x0005CF70
	public void UpdatePlayerFoodOT(float dt)
	{
		this.Food.RegenerationAmount += EffectManager.GetValue(PassiveEffects.FoodChangeOT, null, 0f, this.m_entity, null, this.m_entity.CurrentMovementTag, true, true, true, true, true, 1, true, false) * dt;
		if (this.Food.RegenerationAmount < 0f)
		{
			this.Food.RegenerationAmount *= this.Food.LossSandboxModifier;
		}
		this.Food.MaxModifier = -EffectManager.GetValue(PassiveEffects.FoodMaxBlockage, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Food.Tick(dt);
		this.Food.RegenerationAmount = 0f;
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x0005EE34 File Offset: 0x0005D034
	public void UpdatePlayerWaterOT(float dt)
	{
		this.Water.RegenerationAmount += EffectManager.GetValue(PassiveEffects.WaterChangeOT, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) * dt;
		if (this.Water.RegenerationAmount < 0f)
		{
			this.Water.RegenerationAmount *= this.Water.LossSandboxModifier;
		}
		this.Water.MaxModifier = -EffectManager.GetValue(PassiveEffects.WaterMaxBlockage, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Water.Tick(dt);
		this.Water.RegenerationAmount = 0f;
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x0005EEFC File Offset: 0x0005D0FC
	public void UpdatePlayerHealthOT(float dt)
	{
		float num = EffectManager.GetValue(PassiveEffects.HealthChangeOT, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (this.Health.ValuePercent < 1f && num > 0f)
		{
			float waterPercent = this.GetWaterPercent();
			this.Health.RegenerationAmount = num * waterPercent * dt;
		}
		else if (num < 0f)
		{
			List<EffectManager.ModifierValuesAndSources> valuesAndSources = EffectManager.GetValuesAndSources(PassiveEffects.HealthChangeOT, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true);
			float num2 = 0f;
			float num3 = 1f;
			for (int i = 0; i < valuesAndSources.Count; i++)
			{
				if (valuesAndSources[i].ParentType == MinEffectController.SourceParentType.BuffClass)
				{
					BuffValue buff = this.m_entity.Buffs.GetBuff((string)valuesAndSources[i].Source);
					if (buff != null && buff.BuffClass != null && !buff.Remove)
					{
						BuffClass buffClass = buff.BuffClass;
						num2 = 0f;
						num3 = 1f;
						buffClass.ModifyValue(this.m_entity, PassiveEffects.HealthChangeOT, buff, ref num2, ref num3, FastTags<TagGroup.Global>.none);
						num = num2 * num3 * dt;
						if (num < 0f)
						{
							DamageSourceEntity damageSourceEntity = new DamageSourceEntity(buffClass.DamageSource, buffClass.DamageType, buff.InstigatorId);
							damageSourceEntity.BuffClass = buffClass;
							this.m_entity.DamageEntity(damageSourceEntity, (int)(-num + 0.5f), false, 0f);
						}
					}
				}
				else
				{
					this.Health.RegenerationAmount = valuesAndSources[i].Value * dt;
				}
			}
		}
		this.Health.MaxModifier = -EffectManager.GetValue(PassiveEffects.HealthMaxBlockage, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Health.Tick(dt);
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x0005F0EC File Offset: 0x0005D2EC
	public void UpdatePlayerStaminaOT(float _dt)
	{
		float value = EffectManager.GetValue(PassiveEffects.StaminaChangeOT, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (this.Stamina.ValuePercent < 1f && value > 0f)
		{
			this.Stamina.RegenerationAmount = value * _dt;
		}
		else if (value < 0f)
		{
			this.Stamina.RegenerationAmount = value * _dt;
		}
		float num = EffectManager.GetValue(PassiveEffects.StaminaChangeOT, null, this.Stamina.RegenerationAmount, this.m_entity, null, this.m_entity.CurrentMovementTag | this.m_entity.CurrentStanceTag, true, true, true, true, true, 1, true, false);
		if (num > 0f)
		{
			float num2 = this.GetWaterPercent();
			num2 = Utils.FastMax(0.2f, num2);
			num *= num2;
		}
		this.Stamina.RegenerationAmount = num * _dt;
		if (this.Stamina.RegenerationAmount > 0f)
		{
			this.Stamina.RegenerationAmount *= this.Stamina.GainSandboxModifier;
		}
		else
		{
			this.Stamina.RegenerationAmount *= this.Stamina.LossSandboxModifier;
		}
		this.Stamina.MaxModifier = -EffectManager.GetValue(PassiveEffects.StaminaMaxBlockage, null, 0f, this.m_entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.Stamina.Tick(_dt);
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x0005F255 File Offset: 0x0005D455
	public float GetOutsideTemperature()
	{
		return 70f;
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0005F25C File Offset: 0x0005D45C
	[PublicizedFrom(EAccessModifier.Private)]
	public float CalcShadeFromSunPercent()
	{
		Vector3 sunLightDirection = SkyManager.GetSunLightDirection();
		if (sunLightDirection.y > -0.25f)
		{
			return 1f;
		}
		RaycastHit raycastHit;
		if (Physics.SphereCast(new Ray(this.m_entity.getHeadPosition() - Origin.position + sunLightDirection * 0.15f, -sunLightDirection), 0.15f, out raycastHit, float.PositiveInfinity, 65809))
		{
			return 1f;
		}
		return 0f;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x0005F2D8 File Offset: 0x0005D4D8
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetWaterPercent()
	{
		float num = this.Water.ValuePercentUI * (this.Water.Max * 0.01f);
		if (num != 0f)
		{
			if (num < 0.25f)
			{
				num = 0.25f;
			}
			else if (num < 0.5f)
			{
				num = 0.5f;
			}
			else
			{
				num = 1f;
			}
		}
		return num;
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0005F334 File Offset: 0x0005D534
	[PublicizedFrom(EAccessModifier.Private)]
	public new void SendStatChangePacket(NetPackageEntityStatChanged.EnumStat enumStat)
	{
		NetPackageEntityStatChanged netPackageEntityStatChanged = NetPackageManager.GetPackage<NetPackageEntityStatChanged>().Setup(this.m_entity, this.localPlayer.entityId, enumStat);
		if (this.m_entity.world.IsRemote())
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageEntityStatChanged, false);
			return;
		}
		this.m_entity.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.m_entity.entityId, -1, netPackageEntityStatChanged, enumStat > NetPackageEntityStatChanged.EnumStat.Health);
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x0005F3A4 File Offset: 0x0005D5A4
	public override void Read(PooledBinaryReader stream)
	{
		int num = stream.ReadInt32();
		this.Health.Read(stream);
		this.Stamina.Read(stream);
		if (num <= 10)
		{
			new Stat(Stat.StatTypes.Health, null, 0f, 0f).Read(stream);
		}
		this.Water.Read(stream);
		this.Food.Read(stream);
		if (num >= 11)
		{
			this.CoreTemp = (float)(stream.ReadSByte() * 2);
		}
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0005F418 File Offset: 0x0005D618
	public override void Write(PooledBinaryWriter stream)
	{
		stream.Write(11);
		this.Health.Write(stream);
		this.Stamina.Write(stream);
		this.Water.Write(stream);
		this.Food.Write(stream);
		stream.Write((sbyte)(this.CoreTemp / 2f));
	}

	// Token: 0x04000C38 RID: 3128
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IEntityBuffsChanged> buffChangedDelegates;

	// Token: 0x04000C39 RID: 3129
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IEntityUINotificationChanged> notificationChangedDelegates;

	// Token: 0x04000C3A RID: 3130
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityUINotification> m_notifications;

	// Token: 0x04000C3B RID: 3131
	public float CoreTemp = 70f;

	// Token: 0x04000C3C RID: 3132
	public float LightInsidePer;

	// Token: 0x04000C3D RID: 3133
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04000C3E RID: 3134
	[PublicizedFrom(EAccessModifier.Private)]
	public float shadePer;

	// Token: 0x04000C3F RID: 3135
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRaycastMask = 65809;
}
