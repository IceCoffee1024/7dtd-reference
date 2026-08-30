using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x020006C0 RID: 1728
public class MinEffectGroup
{
	// Token: 0x060035DF RID: 13791 RVA: 0x0016359C File Offset: 0x0016179C
	public MinEffectGroup()
	{
		this.Requirements = null;
		this.PassiveEffects = new List<PassiveEffect>();
		this.TriggeredEffects = new Dictionary<MinEventTypes, List<MinEventActionBase>>();
		this.EffectDescriptions = new List<EffectGroupDescription>();
		this.OwnerTiered = true;
		this.PassivesIndices = new List<PassiveEffects>();
		this.EffectDisplayValues = new CaseInsensitiveStringDictionary<EffectDisplayValue>();
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x001635F4 File Offset: 0x001617F4
	public void ModifyValue(MinEventParams _params, EntityAlive _self, PassiveEffects _effect, ref float _base_value, ref float _perc_value, float level, FastTags<TagGroup.Global> _tags, int _multiplier = 1)
	{
		if (!this.canRun(_params))
		{
			return;
		}
		int count = this.PassiveEffects.Count;
		for (int i = 0; i < count; i++)
		{
			PassiveEffect passiveEffect = this.PassiveEffects[i];
			if (passiveEffect.Type == _effect && passiveEffect.RequirementsMet(_params))
			{
				passiveEffect.ModifyValue(_self, level, ref _base_value, ref _perc_value, _tags, _multiplier);
			}
		}
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x00163654 File Offset: 0x00161854
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSource, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, MinEffectController.SourceParentType _parentType, EntityAlive _self, PassiveEffects _effect, ref float _base_value, ref float _perc_value, float level, FastTags<TagGroup.Global> _tags, int _multiplier = 1, object _parentPointer = null)
	{
		MinEventParams minEventParams;
		if (_self == null)
		{
			minEventParams = MinEventParams.CachedEventParam;
			minEventParams.Self = null;
		}
		else
		{
			minEventParams = _self.MinEventContext;
		}
		minEventParams.Tags = _tags;
		if (!this.canRun(minEventParams))
		{
			return;
		}
		for (int i = 0; i < this.PassiveEffects.Count; i++)
		{
			PassiveEffect passiveEffect = this.PassiveEffects[i];
			if (passiveEffect.Type == _effect && passiveEffect.RequirementsMet(minEventParams))
			{
				passiveEffect.GetModifiedValueData(_modValueSource, _sourceType, _parentType, _self, level, ref _base_value, ref _perc_value, _tags, _multiplier, _parentPointer);
			}
		}
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x001636E4 File Offset: 0x001618E4
	public IReadOnlyList<MinEventActionBase> GetTriggeredEffects(MinEventTypes _eventType)
	{
		List<MinEventActionBase> list;
		if (!this.TriggeredEffects.TryGetValue(_eventType, out list))
		{
			return Array.Empty<MinEventActionBase>();
		}
		return list;
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x00163708 File Offset: 0x00161908
	public void AddTriggeredEffect(MinEventActionBase triggeredEffect)
	{
		MinEventTypes eventType = triggeredEffect.EventType;
		List<MinEventActionBase> list;
		if (!this.TriggeredEffects.TryGetValue(eventType, out list))
		{
			list = new List<MinEventActionBase>();
			this.TriggeredEffects.Add(eventType, list);
		}
		list.Add(triggeredEffect);
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x00163746 File Offset: 0x00161946
	public bool HasEvents()
	{
		return this.TriggeredEffects.Count > 0;
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x00163758 File Offset: 0x00161958
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		if (this.TriggeredEffects.Count <= 0)
		{
			return;
		}
		IReadOnlyList<MinEventActionBase> triggeredEffects = this.GetTriggeredEffects(_eventType);
		if (triggeredEffects.Count <= 0)
		{
			return;
		}
		if (!this.canRun(_eventParms))
		{
			return;
		}
		for (int i = 0; i < triggeredEffects.Count; i++)
		{
			MinEventActionBase minEventActionBase = triggeredEffects[i];
			if (minEventActionBase.CanExecute(_eventType, _eventParms))
			{
				minEventActionBase.Execute(_eventParms);
			}
		}
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x001637BA File Offset: 0x001619BA
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x001637D2 File Offset: 0x001619D2
	public bool HasTrigger(MinEventTypes _eventType)
	{
		return this.GetTriggeredEffects(_eventType).Count > 0;
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x001637E4 File Offset: 0x001619E4
	public static MinEffectGroup ParseXml(XElement _element)
	{
		MinEffectGroup minEffectGroup = new MinEffectGroup();
		if (_element.HasAttribute("tiered"))
		{
			minEffectGroup.OwnerTiered = StringParsers.ParseBool(_element.GetAttribute("tiered"), 0, -1, true);
		}
		minEffectGroup.Requirements = RequirementBase.ParseRequirementGroup(_element);
		foreach (XElement xelement in _element.Elements())
		{
			if (xelement.Name == "passive_effect")
			{
				PassiveEffect passiveEffect = PassiveEffect.ParsePassiveEffect(xelement);
				if (passiveEffect != null)
				{
					MinEffectGroup.AddPassiveEffectToGroup(minEffectGroup, passiveEffect);
				}
			}
			else if (xelement.Name == "triggered_effect")
			{
				MinEventActionBase minEventActionBase = MinEventActionBase.ParseAction(xelement);
				if (minEventActionBase != null)
				{
					minEffectGroup.AddTriggeredEffect(minEventActionBase);
				}
			}
			else if (xelement.Name == "effect_description")
			{
				EffectGroupDescription effectGroupDescription = EffectGroupDescription.ParseDescription(xelement);
				if (effectGroupDescription != null)
				{
					minEffectGroup.EffectDescriptions.Add(effectGroupDescription);
				}
			}
			else if (xelement.Name == "display_value")
			{
				EffectDisplayValue effectDisplayValue = EffectDisplayValue.ParseDisplayValue(xelement);
				if (effectDisplayValue != null)
				{
					minEffectGroup.EffectDisplayValues.Add(effectDisplayValue.Name, effectDisplayValue);
				}
			}
		}
		return minEffectGroup;
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x0016393C File Offset: 0x00161B3C
	public static void AddPassiveEffectToGroup(MinEffectGroup _effectGroup, PassiveEffect _pe)
	{
		_effectGroup.PassivesIndices.Add(_pe.Type);
		_effectGroup.PassiveEffects.Add(_pe);
	}

	// Token: 0x04002B7E RID: 11134
	public RequirementGroup Requirements;

	// Token: 0x04002B7F RID: 11135
	public List<PassiveEffect> PassiveEffects;

	// Token: 0x04002B80 RID: 11136
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<MinEventTypes, List<MinEventActionBase>> TriggeredEffects;

	// Token: 0x04002B81 RID: 11137
	public List<EffectGroupDescription> EffectDescriptions;

	// Token: 0x04002B82 RID: 11138
	public CaseInsensitiveStringDictionary<EffectDisplayValue> EffectDisplayValues;

	// Token: 0x04002B83 RID: 11139
	public bool OwnerTiered;

	// Token: 0x04002B84 RID: 11140
	public List<PassiveEffects> PassivesIndices;
}
