using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x020006B6 RID: 1718
public class MinEffectGroup
{
	// Token: 0x06003589 RID: 13705 RVA: 0x001613C4 File Offset: 0x0015F5C4
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

	// Token: 0x0600358A RID: 13706 RVA: 0x0016141C File Offset: 0x0015F61C
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

	// Token: 0x0600358B RID: 13707 RVA: 0x0016147C File Offset: 0x0015F67C
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

	// Token: 0x0600358C RID: 13708 RVA: 0x0016150C File Offset: 0x0015F70C
	public IReadOnlyList<MinEventActionBase> GetTriggeredEffects(MinEventTypes _eventType)
	{
		List<MinEventActionBase> list;
		if (!this.TriggeredEffects.TryGetValue(_eventType, out list))
		{
			return Array.Empty<MinEventActionBase>();
		}
		return list;
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x00161530 File Offset: 0x0015F730
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

	// Token: 0x0600358E RID: 13710 RVA: 0x0016156E File Offset: 0x0015F76E
	public bool HasEvents()
	{
		return this.TriggeredEffects.Count > 0;
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x00161580 File Offset: 0x0015F780
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

	// Token: 0x06003590 RID: 13712 RVA: 0x001615E2 File Offset: 0x0015F7E2
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x001615FA File Offset: 0x0015F7FA
	public bool HasTrigger(MinEventTypes _eventType)
	{
		return this.GetTriggeredEffects(_eventType).Count > 0;
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x0016160C File Offset: 0x0015F80C
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

	// Token: 0x06003593 RID: 13715 RVA: 0x00161764 File Offset: 0x0015F964
	public static void AddPassiveEffectToGroup(MinEffectGroup _effectGroup, PassiveEffect _pe)
	{
		_effectGroup.PassivesIndices.Add(_pe.Type);
		_effectGroup.PassiveEffects.Add(_pe);
	}

	// Token: 0x04002B0A RID: 11018
	public RequirementGroup Requirements;

	// Token: 0x04002B0B RID: 11019
	public List<PassiveEffect> PassiveEffects;

	// Token: 0x04002B0C RID: 11020
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<MinEventTypes, List<MinEventActionBase>> TriggeredEffects;

	// Token: 0x04002B0D RID: 11021
	public List<EffectGroupDescription> EffectDescriptions;

	// Token: 0x04002B0E RID: 11022
	public CaseInsensitiveStringDictionary<EffectDisplayValue> EffectDisplayValues;

	// Token: 0x04002B0F RID: 11023
	public bool OwnerTiered;

	// Token: 0x04002B10 RID: 11024
	public List<PassiveEffects> PassivesIndices;
}
