using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

// Token: 0x020006BD RID: 1725
public class MinEffectController
{
	// Token: 0x060035D4 RID: 13780 RVA: 0x001630CC File Offset: 0x001612CC
	public bool IsOwnerTiered()
	{
		byte b = 0;
		while ((int)b < this.EffectGroups.Count)
		{
			if (this.EffectGroups[(int)b].OwnerTiered)
			{
				return true;
			}
			b += 1;
		}
		return false;
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x00163108 File Offset: 0x00161308
	public void ModifyValue(EntityAlive _self, PassiveEffects _effect, ref float _base_value, ref float _perc_value, float _level = 0f, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>), int multiplier = 1)
	{
		if (!this.PassivesIndex.Contains(_effect))
		{
			return;
		}
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
		for (int i = 0; i < this.EffectGroups.Count; i++)
		{
			if (!minEventParams.Tags.Equals(_tags))
			{
				minEventParams.Tags = new FastTags<TagGroup.Global>(_tags);
			}
			this.EffectGroups[i].ModifyValue(minEventParams, _self, _effect, ref _base_value, ref _perc_value, _level, _tags, multiplier);
		}
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x00163194 File Offset: 0x00161394
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, EntityAlive _self, PassiveEffects _effect, ref float _base_value, ref float _perc_value, float _level = 0f, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>), int multiplier = 1)
	{
		if (!this.PassivesIndex.Contains(_effect))
		{
			return;
		}
		byte b = 0;
		while ((int)b < this.EffectGroups.Count)
		{
			this.EffectGroups[(int)b].GetModifiedValueData(_modValueSources, _sourceType, this.ParentType, _self, _effect, ref _base_value, ref _perc_value, _level, _tags, multiplier, this.ParentPointer);
			b += 1;
		}
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x001631F4 File Offset: 0x001613F4
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		_eventParms.ParentType = this.ParentType;
		byte b = 0;
		while ((int)b < this.EffectGroups.Count)
		{
			this.EffectGroups[(int)b].FireEvent(_eventType, _eventParms);
			b += 1;
		}
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x00163238 File Offset: 0x00161438
	public bool HasEvents()
	{
		byte b = 0;
		while ((int)b < this.EffectGroups.Count)
		{
			if (this.EffectGroups[(int)b].HasEvents())
			{
				return true;
			}
			b += 1;
		}
		return false;
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x00163274 File Offset: 0x00161474
	public bool HasTrigger(MinEventTypes _eventType)
	{
		byte b = 0;
		while ((int)b < this.EffectGroups.Count)
		{
			if (this.EffectGroups[(int)b].HasTrigger(_eventType))
			{
				return true;
			}
			b += 1;
		}
		return false;
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x001632B0 File Offset: 0x001614B0
	public void AddEffectGroup(MinEffectGroup item, int _order = 0, bool _extends = false)
	{
		if (this.EffectGroups == null)
		{
			this.EffectGroups = new List<MinEffectGroup>();
		}
		if (this.PassivesIndex == null)
		{
			this.PassivesIndex = new HashSet<PassiveEffects>(EffectManager.PassiveEffectsComparer);
		}
		if (_extends)
		{
			for (int i = 0; i < this.EffectGroups.Count; i++)
			{
				MinEffectGroup minEffectGroup = this.EffectGroups[i];
				if (minEffectGroup.Requirements == null)
				{
					for (int j = 0; j < minEffectGroup.PassiveEffects.Count; j++)
					{
						PassiveEffect passiveEffect = minEffectGroup.PassiveEffects[j];
						if (passiveEffect.Tags.IsEmpty && (passiveEffect.Modifier == PassiveEffect.ValueModifierTypes.base_set || passiveEffect.Modifier == PassiveEffect.ValueModifierTypes.perc_set))
						{
							for (int k = item.PassiveEffects.Count - 1; k >= 0; k--)
							{
								PassiveEffect passiveEffect2 = item.PassiveEffects[k];
								if (passiveEffect2.Type == passiveEffect.Type && passiveEffect2.Modifier == passiveEffect.Modifier)
								{
									item.PassiveEffects.RemoveAt(k);
								}
							}
						}
					}
				}
			}
		}
		this.EffectGroups.Insert(_order, item);
		this.PassivesIndex.UnionWith(item.PassivesIndices);
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x001633E0 File Offset: 0x001615E0
	public static MinEffectController ParseXml(XElement _element, XElement _elementToExtend = null, MinEffectController.SourceParentType _type = MinEffectController.SourceParentType.None, object _parentPointer = null)
	{
		bool flag = false;
		MinEffectController minEffectController = new MinEffectController();
		minEffectController.EffectGroups = new List<MinEffectGroup>();
		minEffectController.PassivesIndex = new HashSet<PassiveEffects>(EffectManager.PassiveEffectsComparer);
		minEffectController.ParentType = _type;
		minEffectController.ParentPointer = _parentPointer;
		int num = 0;
		foreach (XElement xelement in _element.Elements("effect_group"))
		{
			flag = true;
			minEffectController.AddEffectGroup(MinEffectGroup.ParseXml(xelement), num++, false);
		}
		if (_elementToExtend != null)
		{
			flag = true;
			XElement xelement2 = _elementToExtend;
			while (xelement2 != null)
			{
				num = 0;
				foreach (XElement xelement3 in xelement2.Elements("effect_group"))
				{
					minEffectController.AddEffectGroup(MinEffectGroup.ParseXml(xelement3), num++, true);
				}
				XAttribute xattribute = xelement2.Attribute("extends");
				if (xattribute != null)
				{
					string extendName = xattribute.Value;
					xelement2 = _element.Document.Descendants(xelement2.Name).FirstOrDefault<XElement>((XElement e) => (string)e.Attribute("name") == extendName);
					if (xelement2 == null)
					{
						Log.Warning("Unable to find element to extend '" + extendName + "'");
					}
				}
				else
				{
					xelement2 = null;
				}
			}
		}
		if (!flag)
		{
			return null;
		}
		return minEffectController;
	}

	// Token: 0x04002B70 RID: 11120
	public List<MinEffectGroup> EffectGroups;

	// Token: 0x04002B71 RID: 11121
	public HashSet<PassiveEffects> PassivesIndex;

	// Token: 0x04002B72 RID: 11122
	public MinEffectController.SourceParentType ParentType;

	// Token: 0x04002B73 RID: 11123
	public object ParentPointer = -1;

	// Token: 0x020006BE RID: 1726
	public enum SourceParentType
	{
		// Token: 0x04002B75 RID: 11125
		None,
		// Token: 0x04002B76 RID: 11126
		ItemClass,
		// Token: 0x04002B77 RID: 11127
		ItemModifierClass,
		// Token: 0x04002B78 RID: 11128
		EntityClass,
		// Token: 0x04002B79 RID: 11129
		ProgressionClass,
		// Token: 0x04002B7A RID: 11130
		BuffClass,
		// Token: 0x04002B7B RID: 11131
		ChallengeClass,
		// Token: 0x04002B7C RID: 11132
		ChallengeGroup
	}
}
