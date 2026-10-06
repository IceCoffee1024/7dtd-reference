using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x020006C2 RID: 1730
public class MinEffectGroup
{
	// Token: 0x06003622 RID: 13858 RVA: 0x00163F0C File Offset: 0x0016210C
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

	// Token: 0x06003623 RID: 13859 RVA: 0x00163F64 File Offset: 0x00162164
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

	// Token: 0x06003624 RID: 13860 RVA: 0x00163FC4 File Offset: 0x001621C4
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

	// Token: 0x06003625 RID: 13861 RVA: 0x00164054 File Offset: 0x00162254
	public IReadOnlyList<MinEventActionBase> GetTriggeredEffects(MinEventTypes _eventType)
	{
		List<MinEventActionBase> list;
		if (!this.TriggeredEffects.TryGetValue(_eventType, out list))
		{
			return Array.Empty<MinEventActionBase>();
		}
		return list;
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x00164078 File Offset: 0x00162278
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

	// Token: 0x06003627 RID: 13863 RVA: 0x001640B6 File Offset: 0x001622B6
	public bool HasEvents()
	{
		return this.TriggeredEffects.Count > 0;
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x001640C8 File Offset: 0x001622C8
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

	// Token: 0x06003629 RID: 13865 RVA: 0x0016412A File Offset: 0x0016232A
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x00164142 File Offset: 0x00162342
	public bool HasTrigger(MinEventTypes _eventType)
	{
		return this.GetTriggeredEffects(_eventType).Count > 0;
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x00164154 File Offset: 0x00162354
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

	// Token: 0x0600362C RID: 13868 RVA: 0x001642AC File Offset: 0x001624AC
	public static void AddPassiveEffectToGroup(MinEffectGroup _effectGroup, PassiveEffect _pe)
	{
		_effectGroup.PassivesIndices.Add(_pe.Type);
		_effectGroup.PassiveEffects.Add(_pe);
	}

	// Token: 0x04002B77 RID: 11127
	public RequirementGroup Requirements;

	// Token: 0x04002B78 RID: 11128
	public List<PassiveEffect> PassiveEffects;

	// Token: 0x04002B79 RID: 11129
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<MinEventTypes, List<MinEventActionBase>> TriggeredEffects;

	// Token: 0x04002B7A RID: 11130
	public List<EffectGroupDescription> EffectDescriptions;

	// Token: 0x04002B7B RID: 11131
	public CaseInsensitiveStringDictionary<EffectDisplayValue> EffectDisplayValues;

	// Token: 0x04002B7C RID: 11132
	public bool OwnerTiered;

	// Token: 0x04002B7D RID: 11133
	public List<PassiveEffects> PassivesIndices;
}
