using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

// Token: 0x020006C6 RID: 1734
public class MinEffectController
{
	// Token: 0x06003625 RID: 13861 RVA: 0x00164630 File Offset: 0x00162830
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

	// Token: 0x06003626 RID: 13862 RVA: 0x0016466C File Offset: 0x0016286C
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

	// Token: 0x06003627 RID: 13863 RVA: 0x001646F8 File Offset: 0x001628F8
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

	// Token: 0x06003628 RID: 13864 RVA: 0x00164758 File Offset: 0x00162958
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

	// Token: 0x06003629 RID: 13865 RVA: 0x0016479C File Offset: 0x0016299C
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

	// Token: 0x0600362A RID: 13866 RVA: 0x001647D8 File Offset: 0x001629D8
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

	// Token: 0x0600362B RID: 13867 RVA: 0x00164814 File Offset: 0x00162A14
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

	// Token: 0x0600362C RID: 13868 RVA: 0x00164944 File Offset: 0x00162B44
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

	// Token: 0x04002B84 RID: 11140
	public List<MinEffectGroup> EffectGroups;

	// Token: 0x04002B85 RID: 11141
	public HashSet<PassiveEffects> PassivesIndex;

	// Token: 0x04002B86 RID: 11142
	public MinEffectController.SourceParentType ParentType;

	// Token: 0x04002B87 RID: 11143
	public object ParentPointer = -1;

	// Token: 0x020006C7 RID: 1735
	public enum SourceParentType
	{
		// Token: 0x04002B89 RID: 11145
		None,
		// Token: 0x04002B8A RID: 11146
		ItemClass,
		// Token: 0x04002B8B RID: 11147
		ItemModifierClass,
		// Token: 0x04002B8C RID: 11148
		EntityClass,
		// Token: 0x04002B8D RID: 11149
		ProgressionClass,
		// Token: 0x04002B8E RID: 11150
		BuffClass,
		// Token: 0x04002B8F RID: 11151
		ChallengeClass,
		// Token: 0x04002B90 RID: 11152
		ChallengeGroup
	}
}
