using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000653 RID: 1619
public class BuffClass
{
	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x0600349F RID: 13471 RVA: 0x0015C98F File Offset: 0x0015AB8F
	// (set) Token: 0x060034A0 RID: 13472 RVA: 0x0015C997 File Offset: 0x0015AB97
	public float DurationMax
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.durationMax;
		}
		set
		{
			if (this.initialDurationMax == 0f && value > 0f)
			{
				this.initialDurationMax = value;
			}
			this.durationMax = value;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x060034A1 RID: 13473 RVA: 0x0015C9BC File Offset: 0x0015ABBC
	public float InitialDurationMax
	{
		get
		{
			return this.initialDurationMax;
		}
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0015C9C4 File Offset: 0x0015ABC4
	public BuffClass(string _name = "")
	{
		this.Name = _name.ToLower();
		this.LocalizedName = string.Empty;
		this.DescriptionKey = string.Empty;
		this.TooltipKey = string.Empty;
		this.Icon = string.Empty;
		this.IconBlink = false;
		this.Requirements = null;
		this.Hidden = false;
		this.DamageType = EnumDamageTypes.None;
		this.StackType = BuffEffectStackTypes.Replace;
		this.durationMax = 0f;
		this.initialDurationMax = 0f;
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0015CA71 File Offset: 0x0015AC71
	public void Tick(BuffValue _ev)
	{
		_ev.DurationTick();
		if (this.DurationMax > 0f && _ev.DurationInSeconds >= this.DurationMax)
		{
			_ev.Finished = true;
		}
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0015CA9C File Offset: 0x0015AC9C
	public void ModifyValue(EntityAlive _self, PassiveEffects _effect, BuffValue _bv, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags)
	{
		if (_bv.Remove)
		{
			return;
		}
		_self.MinEventContext.Tags |= _tags;
		if (!this.canRun(_self.MinEventContext))
		{
			return;
		}
		if (this.Effects != null)
		{
			this.Effects.ModifyValue(_self, _effect, ref _base_value, ref _perc_value, _bv.DurationInSeconds, _tags, (this.StackType == BuffEffectStackTypes.Effect) ? _bv.StackEffectMultiplier : 1);
		}
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x0015CB0C File Offset: 0x0015AD0C
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, EntityAlive _self, PassiveEffects _effect, BuffValue _bv, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags)
	{
		if (_bv.Remove)
		{
			return;
		}
		_self.MinEventContext.Tags |= _tags;
		if (!this.canRun(_self.MinEventContext))
		{
			return;
		}
		if (this.Effects != null)
		{
			this.Effects.GetModifiedValueData(_modValueSources, _sourceType, _self, _effect, ref _base_value, ref _perc_value, _bv.DurationInSeconds, _tags, (this.StackType == BuffEffectStackTypes.Effect) ? _bv.StackEffectMultiplier : 1);
		}
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x0015CB82 File Offset: 0x0015AD82
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.Effects == null)
		{
			return;
		}
		if (!this.canRun(_params))
		{
			return;
		}
		this.Effects.FireEvent(_eventType, _params);
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x0015CBA4 File Offset: 0x0015ADA4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x0400299D RID: 10653
	public string Name;

	// Token: 0x0400299E RID: 10654
	public string LocalizedName;

	// Token: 0x0400299F RID: 10655
	public string Description;

	// Token: 0x040029A0 RID: 10656
	public string DescriptionKey;

	// Token: 0x040029A1 RID: 10657
	public string Tooltip;

	// Token: 0x040029A2 RID: 10658
	public string TooltipKey;

	// Token: 0x040029A3 RID: 10659
	public string Icon;

	// Token: 0x040029A4 RID: 10660
	public string DisplayValueCVar;

	// Token: 0x040029A5 RID: 10661
	public string DisplayValueKey;

	// Token: 0x040029A6 RID: 10662
	public BuffClass.CVarDisplayFormat DisplayValueFormat;

	// Token: 0x040029A7 RID: 10663
	public Color IconColor;

	// Token: 0x040029A8 RID: 10664
	public bool IconBlink;

	// Token: 0x040029A9 RID: 10665
	public EnumEntityUINotificationDisplayMode DisplayType;

	// Token: 0x040029AA RID: 10666
	public RequirementGroup Requirements;

	// Token: 0x040029AB RID: 10667
	public MinEffectController Effects;

	// Token: 0x040029AC RID: 10668
	public bool Hidden;

	// Token: 0x040029AD RID: 10669
	public bool ShowOnHUD = true;

	// Token: 0x040029AE RID: 10670
	public bool AllowInEditor;

	// Token: 0x040029AF RID: 10671
	public EnumGameStats RequiredGameStat = EnumGameStats.Last;

	// Token: 0x040029B0 RID: 10672
	[PublicizedFrom(EAccessModifier.Private)]
	public float durationMax;

	// Token: 0x040029B1 RID: 10673
	[PublicizedFrom(EAccessModifier.Private)]
	public float initialDurationMax;

	// Token: 0x040029B2 RID: 10674
	public int UpdateRateTicks = 20;

	// Token: 0x040029B3 RID: 10675
	public EnumDamageTypes DamageType;

	// Token: 0x040029B4 RID: 10676
	public EnumDamageSource DamageSource;

	// Token: 0x040029B5 RID: 10677
	public BuffEffectStackTypes StackType;

	// Token: 0x040029B6 RID: 10678
	public bool RemoveOnDeath = true;

	// Token: 0x040029B7 RID: 10679
	public FastTags<TagGroup.Global> NameTag;

	// Token: 0x040029B8 RID: 10680
	public FastTags<TagGroup.Global> Tags = FastTags<TagGroup.Global>.none;

	// Token: 0x02000654 RID: 1620
	public enum CVarDisplayFormat
	{
		// Token: 0x040029BA RID: 10682
		None,
		// Token: 0x040029BB RID: 10683
		Float,
		// Token: 0x040029BC RID: 10684
		FlooredToInt,
		// Token: 0x040029BD RID: 10685
		RoundedToInt,
		// Token: 0x040029BE RID: 10686
		CeiledToInt,
		// Token: 0x040029BF RID: 10687
		Time,
		// Token: 0x040029C0 RID: 10688
		Percentage,
		// Token: 0x040029C1 RID: 10689
		Degrees
	}
}
