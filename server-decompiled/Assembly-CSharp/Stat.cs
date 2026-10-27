using System;
using System.Runtime.CompilerServices;

// Token: 0x020001DD RID: 477
public sealed class Stat
{
	// Token: 0x06000E92 RID: 3730 RVA: 0x0000640C File Offset: 0x0000460C
	public Stat()
	{
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0005FE20 File Offset: 0x0005E020
	public Stat(Stat.StatTypes _statType, EntityAlive _entity, float _value, float _baseMax)
	{
		this.StatType = _statType;
		this.Entity = _entity;
		this.m_value = _value;
		this.m_originalValue = _value;
		this.m_lastValue = _value;
		this.m_baseMax = _baseMax;
		this.m_originalBaseMax = _baseMax;
		this.m_maxModifier = 0f;
		this.GainSandboxModifier = 1f;
		this.LossSandboxModifier = 1f;
		this.m_changed = false;
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0005FE90 File Offset: 0x0005E090
	public void Tick(float dt)
	{
		if (this.MaxPassive != PassiveEffects.None)
		{
			this.BaseMax = EffectManager.GetValue(this.MaxPassive, null, this.m_originalBaseMax, this.Entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		if ((this.StatType == Stat.StatTypes.Health || this.StatType == Stat.StatTypes.Stamina) && Utils.FastAbs(this.m_lastValue - this.m_value) >= 1f)
		{
			if (this.m_value > this.m_lastValue && this.GainPassive != PassiveEffects.None)
			{
				this.m_value = Utils.FastClamp(this.m_lastValue + EffectManager.GetValue(this.GainPassive, null, this.m_value - this.m_lastValue, this.Entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false), 0f, this.m_baseMax);
			}
			else if (this.m_value < this.m_lastValue && this.LossPassive != PassiveEffects.None)
			{
				this.m_value = Utils.FastClamp(this.m_lastValue - EffectManager.GetValue(this.LossPassive, null, this.m_lastValue - this.m_value, this.Entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false), 0f, this.m_baseMax);
			}
		}
		if (this.m_value + this.regenAmount > this.ModifiedMax)
		{
			this.regenAmount = this.ModifiedMax - this.m_value;
		}
		this.RegenerationAmountUI = this.m_value - this.m_lastValue + this.regenAmount / dt;
		this.m_value += this.regenAmount;
		if (this.regenAmount > 0f)
		{
			if (this.StatType == Stat.StatTypes.Stamina)
			{
				this.Entity.Stats.Water.RegenerationAmount -= this.regenAmount * EffectManager.GetValue(PassiveEffects.WaterLossPerStaminaPointGained, null, 1f, this.Entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				this.Entity.Stats.Food.RegenerationAmount -= this.regenAmount * EffectManager.GetValue(PassiveEffects.FoodLossPerStaminaPointGained, null, 1f, this.Entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			}
			else if (this.StatType == Stat.StatTypes.Health)
			{
				this.Entity.Stats.Water.RegenerationAmount -= this.regenAmount * EffectManager.GetValue(PassiveEffects.WaterLossPerHealthPointGained, null, 1f, this.Entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				this.Entity.Stats.Food.RegenerationAmount -= this.regenAmount * EffectManager.GetValue(PassiveEffects.FoodLossPerHealthPointGained, null, 1f, this.Entity, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			}
		}
		this.regenAmount = this.m_value - this.m_lastValue;
		this.SetChangedFlag(this.m_value, this.m_lastValue);
		this.m_lastValue = this.m_value;
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x000601A3 File Offset: 0x0005E3A3
	public void ResetValue()
	{
		this.m_value = this.m_originalValue;
		this.m_baseMax = this.m_originalBaseMax;
		this.m_maxModifier = 0f;
		this.m_changed = true;
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x000601D0 File Offset: 0x0005E3D0
	public void CopyFrom(Stat _stat)
	{
		this.m_value = _stat.m_value;
		this.m_originalValue = _stat.m_originalValue;
		this.m_baseMax = _stat.m_baseMax;
		this.m_originalBaseMax = _stat.m_originalBaseMax;
		this.m_maxModifier = _stat.m_maxModifier;
		this.m_changed = false;
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00060220 File Offset: 0x0005E420
	// (set) Token: 0x06000E98 RID: 3736 RVA: 0x00060228 File Offset: 0x0005E428
	public float RegenerationAmount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.regenAmount;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			this.regenAmount = value;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00060231 File Offset: 0x0005E431
	// (set) Token: 0x06000E9A RID: 3738 RVA: 0x00060239 File Offset: 0x0005E439
	public float RegenerationAmountUI
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000E9B RID: 3739 RVA: 0x00060242 File Offset: 0x0005E442
	public float Max
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_baseMax;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000E9C RID: 3740 RVA: 0x00060242 File Offset: 0x0005E442
	// (set) Token: 0x06000E9D RID: 3741 RVA: 0x0006024A File Offset: 0x0005E44A
	public float BaseMax
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_baseMax;
		}
		set
		{
			if (this.m_baseMax != value)
			{
				this.SetChangedFlag(this.m_baseMax, value);
				this.m_baseMax = value;
			}
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00060269 File Offset: 0x0005E469
	public float ModifiedMax
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_baseMax + this.m_maxModifier;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000E9F RID: 3743 RVA: 0x00060278 File Offset: 0x0005E478
	// (set) Token: 0x06000EA0 RID: 3744 RVA: 0x000602A0 File Offset: 0x0005E4A0
	public float Value
	{
		get
		{
			if (this.GodModeEntity())
			{
				return this.ModifiedMax;
			}
			return Utils.FastClamp(this.m_value, 0f, this.ModifiedMax);
		}
		set
		{
			if (this.m_value != value)
			{
				float value2 = this.m_value;
				this.m_value = Utils.FastClamp(value, 0f, this.ModifiedMax);
				this.SetChangedFlag(value2, value);
			}
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x000602DC File Offset: 0x0005E4DC
	public float ValuePercent
	{
		get
		{
			return Utils.FastClamp01(this.Value / this.ModifiedMax);
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x000602F0 File Offset: 0x0005E4F0
	public float ValuePercentUI
	{
		get
		{
			return Utils.FastClamp01(this.Value / this.Max);
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x00060304 File Offset: 0x0005E504
	public float ModifiedMaxPercent
	{
		get
		{
			return Utils.FastClamp01(this.ModifiedMax / this.Max);
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00060318 File Offset: 0x0005E518
	// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x00060320 File Offset: 0x0005E520
	public float MaxModifier
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_maxModifier;
		}
		set
		{
			this.m_maxModifier = Utils.FastClamp(value, -(this.Max * 0.75f), 0f);
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00060340 File Offset: 0x0005E540
	// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x00060348 File Offset: 0x0005E548
	public float OriginalValue
	{
		get
		{
			return this.m_originalValue;
		}
		set
		{
			this.m_originalValue = value;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00060351 File Offset: 0x0005E551
	// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x00060359 File Offset: 0x0005E559
	public float OriginalMax
	{
		get
		{
			return this.m_originalBaseMax;
		}
		set
		{
			this.m_originalBaseMax = value;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00060362 File Offset: 0x0005E562
	// (set) Token: 0x06000EAB RID: 3755 RVA: 0x0006036A File Offset: 0x0005E56A
	public bool Changed
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_changed;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			this.m_changed = value;
		}
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00060374 File Offset: 0x0005E574
	public void Write(PooledBinaryWriter stream)
	{
		stream.Write(6);
		stream.Write(this.m_value);
		stream.Write(this.m_maxModifier);
		stream.Write(this.m_baseMax);
		stream.Write(this.m_originalBaseMax);
		stream.Write(this.m_originalValue);
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x000603C4 File Offset: 0x0005E5C4
	public void Read(PooledBinaryReader stream)
	{
		int num = stream.ReadInt32();
		this.m_value = stream.ReadSingle();
		this.m_maxModifier = stream.ReadSingle();
		if (num <= 5)
		{
			stream.ReadSingle();
		}
		this.m_baseMax = stream.ReadSingle();
		this.m_originalBaseMax = stream.ReadSingle();
		this.m_originalValue = stream.ReadSingle();
		this.m_lastValue = this.m_value;
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00060429 File Offset: 0x0005E629
	[PublicizedFrom(EAccessModifier.Private)]
	public bool GodModeEntity()
	{
		return this.Entity && this.Entity.entityId == this.Entity.world.GetPrimaryPlayerId() && !GameStats.GetBool(EnumGameStats.IsPlayerDamageEnabled);
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00060461 File Offset: 0x0005E661
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetChangedFlag(float newValue, float oldValue)
	{
		this.m_changed = this.m_changed || Utils.Fastfloor(newValue) != Utils.Fastfloor(oldValue);
	}

	// Token: 0x04000C68 RID: 3176
	public const int cBinaryVersion = 6;

	// Token: 0x04000C69 RID: 3177
	public Stat.StatTypes StatType;

	// Token: 0x04000C6A RID: 3178
	public PassiveEffects GainPassive;

	// Token: 0x04000C6B RID: 3179
	public PassiveEffects LossPassive;

	// Token: 0x04000C6C RID: 3180
	public PassiveEffects MaxPassive;

	// Token: 0x04000C6D RID: 3181
	public EntityAlive Entity;

	// Token: 0x04000C6E RID: 3182
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_value;

	// Token: 0x04000C6F RID: 3183
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_originalValue;

	// Token: 0x04000C70 RID: 3184
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_lastValue;

	// Token: 0x04000C71 RID: 3185
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_baseMax;

	// Token: 0x04000C72 RID: 3186
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_originalBaseMax;

	// Token: 0x04000C73 RID: 3187
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxModifier;

	// Token: 0x04000C74 RID: 3188
	public float GainSandboxModifier;

	// Token: 0x04000C75 RID: 3189
	public float LossSandboxModifier;

	// Token: 0x04000C76 RID: 3190
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_changed;

	// Token: 0x04000C77 RID: 3191
	[PublicizedFrom(EAccessModifier.Private)]
	public float regenAmount;

	// Token: 0x020001DE RID: 478
	public enum StatTypes
	{
		// Token: 0x04000C7A RID: 3194
		Health,
		// Token: 0x04000C7B RID: 3195
		Stamina,
		// Token: 0x04000C7C RID: 3196
		Food,
		// Token: 0x04000C7D RID: 3197
		Water,
		// Token: 0x04000C7E RID: 3198
		CoreTemp
	}
}
