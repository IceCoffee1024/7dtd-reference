using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200065A RID: 1626
public class BuffClass
{
	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x060034AD RID: 13485 RVA: 0x0015D583 File Offset: 0x0015B783
	// (set) Token: 0x060034AE RID: 13486 RVA: 0x0015D58B File Offset: 0x0015B78B
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
	// (get) Token: 0x060034AF RID: 13487 RVA: 0x0015D5B0 File Offset: 0x0015B7B0
	public float InitialDurationMax
	{
		get
		{
			return this.initialDurationMax;
		}
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0015D5B8 File Offset: 0x0015B7B8
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

	// Token: 0x060034B1 RID: 13489 RVA: 0x0015D665 File Offset: 0x0015B865
	public void Tick(BuffValue _ev)
	{
		_ev.DurationTick();
		if (this.DurationMax > 0f && _ev.DurationInSeconds >= this.DurationMax)
		{
			_ev.Finished = true;
		}
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x0015D690 File Offset: 0x0015B890
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

	// Token: 0x060034B3 RID: 13491 RVA: 0x0015D700 File Offset: 0x0015B900
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

	// Token: 0x060034B4 RID: 13492 RVA: 0x0015D776 File Offset: 0x0015B976
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

	// Token: 0x060034B5 RID: 13493 RVA: 0x0015D798 File Offset: 0x0015B998
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x040029B8 RID: 10680
	public string Name;

	// Token: 0x040029B9 RID: 10681
	public string LocalizedName;

	// Token: 0x040029BA RID: 10682
	public string Description;

	// Token: 0x040029BB RID: 10683
	public string DescriptionKey;

	// Token: 0x040029BC RID: 10684
	public string Tooltip;

	// Token: 0x040029BD RID: 10685
	public string TooltipKey;

	// Token: 0x040029BE RID: 10686
	public string Icon;

	// Token: 0x040029BF RID: 10687
	public string DisplayValueCVar;

	// Token: 0x040029C0 RID: 10688
	public string DisplayValueKey;

	// Token: 0x040029C1 RID: 10689
	public BuffClass.CVarDisplayFormat DisplayValueFormat;

	// Token: 0x040029C2 RID: 10690
	public Color IconColor;

	// Token: 0x040029C3 RID: 10691
	public bool IconBlink;

	// Token: 0x040029C4 RID: 10692
	public EnumEntityUINotificationDisplayMode DisplayType;

	// Token: 0x040029C5 RID: 10693
	public RequirementGroup Requirements;

	// Token: 0x040029C6 RID: 10694
	public MinEffectController Effects;

	// Token: 0x040029C7 RID: 10695
	public bool Hidden;

	// Token: 0x040029C8 RID: 10696
	public bool ShowOnHUD = true;

	// Token: 0x040029C9 RID: 10697
	public bool AllowInEditor;

	// Token: 0x040029CA RID: 10698
	public EnumGameStats RequiredGameStat = EnumGameStats.Last;

	// Token: 0x040029CB RID: 10699
	[PublicizedFrom(EAccessModifier.Private)]
	public float durationMax;

	// Token: 0x040029CC RID: 10700
	[PublicizedFrom(EAccessModifier.Private)]
	public float initialDurationMax;

	// Token: 0x040029CD RID: 10701
	public int UpdateRateTicks = 20;

	// Token: 0x040029CE RID: 10702
	public EnumDamageTypes DamageType;

	// Token: 0x040029CF RID: 10703
	public EnumDamageSource DamageSource;

	// Token: 0x040029D0 RID: 10704
	public BuffEffectStackTypes StackType;

	// Token: 0x040029D1 RID: 10705
	public bool RemoveOnDeath = true;

	// Token: 0x040029D2 RID: 10706
	public FastTags<TagGroup.Global> NameTag;

	// Token: 0x040029D3 RID: 10707
	public FastTags<TagGroup.Global> Tags = FastTags<TagGroup.Global>.none;

	// Token: 0x0200065B RID: 1627
	public enum CVarDisplayFormat
	{
		// Token: 0x040029D5 RID: 10709
		None,
		// Token: 0x040029D6 RID: 10710
		Float,
		// Token: 0x040029D7 RID: 10711
		FlooredToInt,
		// Token: 0x040029D8 RID: 10712
		RoundedToInt,
		// Token: 0x040029D9 RID: 10713
		CeiledToInt,
		// Token: 0x040029DA RID: 10714
		Time,
		// Token: 0x040029DB RID: 10715
		Percentage,
		// Token: 0x040029DC RID: 10716
		Degrees
	}
}
