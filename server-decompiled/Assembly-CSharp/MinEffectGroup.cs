using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x020006C9 RID: 1737
public class MinEffectGroup
{
	// Token: 0x06003630 RID: 13872 RVA: 0x00164B00 File Offset: 0x00162D00
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

	// Token: 0x06003631 RID: 13873 RVA: 0x00164B58 File Offset: 0x00162D58
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

	// Token: 0x06003632 RID: 13874 RVA: 0x00164BB8 File Offset: 0x00162DB8
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

	// Token: 0x06003633 RID: 13875 RVA: 0x00164C48 File Offset: 0x00162E48
	public IReadOnlyList<MinEventActionBase> GetTriggeredEffects(MinEventTypes _eventType)
	{
		List<MinEventActionBase> list;
		if (!this.TriggeredEffects.TryGetValue(_eventType, out list))
		{
			return Array.Empty<MinEventActionBase>();
		}
		return list;
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x00164C6C File Offset: 0x00162E6C
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

	// Token: 0x06003635 RID: 13877 RVA: 0x00164CAA File Offset: 0x00162EAA
	public bool HasEvents()
	{
		return this.TriggeredEffects.Count > 0;
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x00164CBC File Offset: 0x00162EBC
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

	// Token: 0x06003637 RID: 13879 RVA: 0x00164D1E File Offset: 0x00162F1E
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x00164D36 File Offset: 0x00162F36
	public bool HasTrigger(MinEventTypes _eventType)
	{
		return this.GetTriggeredEffects(_eventType).Count > 0;
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x00164D48 File Offset: 0x00162F48
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

	// Token: 0x0600363A RID: 13882 RVA: 0x00164EA0 File Offset: 0x001630A0
	public static void AddPassiveEffectToGroup(MinEffectGroup _effectGroup, PassiveEffect _pe)
	{
		_effectGroup.PassivesIndices.Add(_pe.Type);
		_effectGroup.PassiveEffects.Add(_pe);
	}

	// Token: 0x04002B92 RID: 11154
	public RequirementGroup Requirements;

	// Token: 0x04002B93 RID: 11155
	public List<PassiveEffect> PassiveEffects;

	// Token: 0x04002B94 RID: 11156
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<MinEventTypes, List<MinEventActionBase>> TriggeredEffects;

	// Token: 0x04002B95 RID: 11157
	public List<EffectGroupDescription> EffectDescriptions;

	// Token: 0x04002B96 RID: 11158
	public CaseInsensitiveStringDictionary<EffectDisplayValue> EffectDisplayValues;

	// Token: 0x04002B97 RID: 11159
	public bool OwnerTiered;

	// Token: 0x04002B98 RID: 11160
	public List<PassiveEffects> PassivesIndices;
}
