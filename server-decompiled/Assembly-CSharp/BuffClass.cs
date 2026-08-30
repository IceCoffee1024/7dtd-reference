using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000648 RID: 1608
public class BuffClass
{
	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06003409 RID: 13321 RVA: 0x00159F03 File Offset: 0x00158103
	// (set) Token: 0x0600340A RID: 13322 RVA: 0x00159F0B File Offset: 0x0015810B
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

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x0600340B RID: 13323 RVA: 0x00159F30 File Offset: 0x00158130
	public float InitialDurationMax
	{
		get
		{
			return this.initialDurationMax;
		}
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x00159F38 File Offset: 0x00158138
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

	// Token: 0x0600340D RID: 13325 RVA: 0x00159FE5 File Offset: 0x001581E5
	public void Tick(BuffValue _ev)
	{
		_ev.DurationTick();
		if (this.DurationMax > 0f && _ev.DurationInSeconds >= this.DurationMax)
		{
			_ev.Finished = true;
		}
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x0015A010 File Offset: 0x00158210
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

	// Token: 0x0600340F RID: 13327 RVA: 0x0015A080 File Offset: 0x00158280
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

	// Token: 0x06003410 RID: 13328 RVA: 0x0015A0F6 File Offset: 0x001582F6
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

	// Token: 0x06003411 RID: 13329 RVA: 0x0015A118 File Offset: 0x00158318
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x04002931 RID: 10545
	public string Name;

	// Token: 0x04002932 RID: 10546
	public string LocalizedName;

	// Token: 0x04002933 RID: 10547
	public string Description;

	// Token: 0x04002934 RID: 10548
	public string DescriptionKey;

	// Token: 0x04002935 RID: 10549
	public string Tooltip;

	// Token: 0x04002936 RID: 10550
	public string TooltipKey;

	// Token: 0x04002937 RID: 10551
	public string Icon;

	// Token: 0x04002938 RID: 10552
	public string DisplayValueCVar;

	// Token: 0x04002939 RID: 10553
	public string DisplayValueKey;

	// Token: 0x0400293A RID: 10554
	public BuffClass.CVarDisplayFormat DisplayValueFormat;

	// Token: 0x0400293B RID: 10555
	public Color IconColor;

	// Token: 0x0400293C RID: 10556
	public bool IconBlink;

	// Token: 0x0400293D RID: 10557
	public EnumEntityUINotificationDisplayMode DisplayType;

	// Token: 0x0400293E RID: 10558
	public RequirementGroup Requirements;

	// Token: 0x0400293F RID: 10559
	public MinEffectController Effects;

	// Token: 0x04002940 RID: 10560
	public bool Hidden;

	// Token: 0x04002941 RID: 10561
	public bool ShowOnHUD = true;

	// Token: 0x04002942 RID: 10562
	public bool AllowInEditor;

	// Token: 0x04002943 RID: 10563
	public EnumGameStats RequiredGameStat = EnumGameStats.Last;

	// Token: 0x04002944 RID: 10564
	[PublicizedFrom(EAccessModifier.Private)]
	public float durationMax;

	// Token: 0x04002945 RID: 10565
	[PublicizedFrom(EAccessModifier.Private)]
	public float initialDurationMax;

	// Token: 0x04002946 RID: 10566
	public int UpdateRateTicks = 20;

	// Token: 0x04002947 RID: 10567
	public EnumDamageTypes DamageType;

	// Token: 0x04002948 RID: 10568
	public EnumDamageSource DamageSource;

	// Token: 0x04002949 RID: 10569
	public BuffEffectStackTypes StackType;

	// Token: 0x0400294A RID: 10570
	public bool RemoveOnDeath = true;

	// Token: 0x0400294B RID: 10571
	public FastTags<TagGroup.Global> NameTag;

	// Token: 0x0400294C RID: 10572
	public FastTags<TagGroup.Global> Tags = FastTags<TagGroup.Global>.none;

	// Token: 0x02000649 RID: 1609
	public enum CVarDisplayFormat
	{
		// Token: 0x0400294E RID: 10574
		None,
		// Token: 0x0400294F RID: 10575
		Float,
		// Token: 0x04002950 RID: 10576
		FlooredToInt,
		// Token: 0x04002951 RID: 10577
		RoundedToInt,
		// Token: 0x04002952 RID: 10578
		CeiledToInt,
		// Token: 0x04002953 RID: 10579
		Time,
		// Token: 0x04002954 RID: 10580
		Percentage,
		// Token: 0x04002955 RID: 10581
		Degrees
	}
}
