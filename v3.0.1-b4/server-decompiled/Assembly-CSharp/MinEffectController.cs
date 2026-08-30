using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

// Token: 0x020006B3 RID: 1715
public class MinEffectController
{
	// Token: 0x0600357E RID: 13694 RVA: 0x00160EF4 File Offset: 0x0015F0F4
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

	// Token: 0x0600357F RID: 13695 RVA: 0x00160F30 File Offset: 0x0015F130
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

	// Token: 0x06003580 RID: 13696 RVA: 0x00160FBC File Offset: 0x0015F1BC
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

	// Token: 0x06003581 RID: 13697 RVA: 0x0016101C File Offset: 0x0015F21C
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

	// Token: 0x06003582 RID: 13698 RVA: 0x00161060 File Offset: 0x0015F260
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

	// Token: 0x06003583 RID: 13699 RVA: 0x0016109C File Offset: 0x0015F29C
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

	// Token: 0x06003584 RID: 13700 RVA: 0x001610D8 File Offset: 0x0015F2D8
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

	// Token: 0x06003585 RID: 13701 RVA: 0x00161208 File Offset: 0x0015F408
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

	// Token: 0x04002AFC RID: 11004
	public List<MinEffectGroup> EffectGroups;

	// Token: 0x04002AFD RID: 11005
	public HashSet<PassiveEffects> PassivesIndex;

	// Token: 0x04002AFE RID: 11006
	public MinEffectController.SourceParentType ParentType;

	// Token: 0x04002AFF RID: 11007
	public object ParentPointer = -1;

	// Token: 0x020006B4 RID: 1716
	public enum SourceParentType
	{
		// Token: 0x04002B01 RID: 11009
		None,
		// Token: 0x04002B02 RID: 11010
		ItemClass,
		// Token: 0x04002B03 RID: 11011
		ItemModifierClass,
		// Token: 0x04002B04 RID: 11012
		EntityClass,
		// Token: 0x04002B05 RID: 11013
		ProgressionClass,
		// Token: 0x04002B06 RID: 11014
		BuffClass,
		// Token: 0x04002B07 RID: 11015
		ChallengeClass,
		// Token: 0x04002B08 RID: 11016
		ChallengeGroup
	}
}
