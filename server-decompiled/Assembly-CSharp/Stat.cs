using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x020001DA RID: 474
public sealed class Stat
{
	// Token: 0x06000E9B RID: 3739 RVA: 0x0000640C File Offset: 0x0000460C
	public Stat()
	{
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x0005FB6C File Offset: 0x0005DD6C
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

	// Token: 0x06000E9D RID: 3741 RVA: 0x0005FBDC File Offset: 0x0005DDDC
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

	// Token: 0x06000E9E RID: 3742 RVA: 0x0005FEEF File Offset: 0x0005E0EF
	public void ResetValue()
	{
		this.m_value = this.m_originalValue;
		this.m_baseMax = this.m_originalBaseMax;
		this.m_maxModifier = 0f;
		this.m_changed = true;
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x0005FF1C File Offset: 0x0005E11C
	public void CopyFrom(Stat _stat)
	{
		this.m_value = _stat.m_value;
		this.m_originalValue = _stat.m_originalValue;
		this.m_baseMax = _stat.m_baseMax;
		this.m_originalBaseMax = _stat.m_originalBaseMax;
		this.m_maxModifier = _stat.m_maxModifier;
		this.m_changed = false;
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0005FF6C File Offset: 0x0005E16C
	// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x0005FF74 File Offset: 0x0005E174
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

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0005FF7D File Offset: 0x0005E17D
	// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0005FF85 File Offset: 0x0005E185
	public float RegenerationAmountUI
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0005FF8E File Offset: 0x0005E18E
	public float Max
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_baseMax;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0005FF8E File Offset: 0x0005E18E
	// (set) Token: 0x06000EA6 RID: 3750 RVA: 0x0005FF96 File Offset: 0x0005E196
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

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0005FFB5 File Offset: 0x0005E1B5
	public float ModifiedMax
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_baseMax + this.m_maxModifier;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0005FFC4 File Offset: 0x0005E1C4
	// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x0005FFEC File Offset: 0x0005E1EC
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

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000EAA RID: 3754 RVA: 0x00060028 File Offset: 0x0005E228
	public float ValuePercent
	{
		get
		{
			return Utils.FastClamp01(this.Value / this.ModifiedMax);
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0006003C File Offset: 0x0005E23C
	public float ValuePercentUI
	{
		get
		{
			return Utils.FastClamp01(this.Value / this.Max);
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00060050 File Offset: 0x0005E250
	public float ModifiedMaxPercent
	{
		get
		{
			return Utils.FastClamp01(this.ModifiedMax / this.Max);
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00060064 File Offset: 0x0005E264
	// (set) Token: 0x06000EAE RID: 3758 RVA: 0x0006006C File Offset: 0x0005E26C
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

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0006008C File Offset: 0x0005E28C
	// (set) Token: 0x06000EB0 RID: 3760 RVA: 0x00060094 File Offset: 0x0005E294
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

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x0006009D File Offset: 0x0005E29D
	// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x000600A5 File Offset: 0x0005E2A5
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

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x000600AE File Offset: 0x0005E2AE
	// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x000600B6 File Offset: 0x0005E2B6
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

	// Token: 0x06000EB5 RID: 3765 RVA: 0x000600C0 File Offset: 0x0005E2C0
	public void Write(BinaryWriter stream)
	{
		stream.Write(6);
		stream.Write(this.m_value);
		stream.Write(this.m_maxModifier);
		stream.Write(this.m_baseMax);
		stream.Write(this.m_originalBaseMax);
		stream.Write(this.m_originalValue);
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00060110 File Offset: 0x0005E310
	public void Read(BinaryReader stream)
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

	// Token: 0x06000EB7 RID: 3767 RVA: 0x00060175 File Offset: 0x0005E375
	[PublicizedFrom(EAccessModifier.Private)]
	public bool GodModeEntity()
	{
		return this.Entity && this.Entity.entityId == this.Entity.world.GetPrimaryPlayerId() && !GameStats.GetBool(EnumGameStats.IsPlayerDamageEnabled);
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x000601AD File Offset: 0x0005E3AD
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetChangedFlag(float newValue, float oldValue)
	{
		this.m_changed = this.m_changed || Utils.Fastfloor(newValue) != Utils.Fastfloor(oldValue);
	}

	// Token: 0x04000C6D RID: 3181
	public const int cBinaryVersion = 6;

	// Token: 0x04000C6E RID: 3182
	public Stat.StatTypes StatType;

	// Token: 0x04000C6F RID: 3183
	public PassiveEffects GainPassive;

	// Token: 0x04000C70 RID: 3184
	public PassiveEffects LossPassive;

	// Token: 0x04000C71 RID: 3185
	public PassiveEffects MaxPassive;

	// Token: 0x04000C72 RID: 3186
	public EntityAlive Entity;

	// Token: 0x04000C73 RID: 3187
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_value;

	// Token: 0x04000C74 RID: 3188
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_originalValue;

	// Token: 0x04000C75 RID: 3189
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_lastValue;

	// Token: 0x04000C76 RID: 3190
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_baseMax;

	// Token: 0x04000C77 RID: 3191
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_originalBaseMax;

	// Token: 0x04000C78 RID: 3192
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_maxModifier;

	// Token: 0x04000C79 RID: 3193
	public float GainSandboxModifier;

	// Token: 0x04000C7A RID: 3194
	public float LossSandboxModifier;

	// Token: 0x04000C7B RID: 3195
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_changed;

	// Token: 0x04000C7C RID: 3196
	[PublicizedFrom(EAccessModifier.Private)]
	public float regenAmount;

	// Token: 0x020001DB RID: 475
	public enum StatTypes
	{
		// Token: 0x04000C7F RID: 3199
		Health,
		// Token: 0x04000C80 RID: 3200
		Stamina,
		// Token: 0x04000C81 RID: 3201
		Food,
		// Token: 0x04000C82 RID: 3202
		Water,
		// Token: 0x04000C83 RID: 3203
		CoreTemp
	}
}
