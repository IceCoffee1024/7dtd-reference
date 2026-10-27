using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020006B7 RID: 1719
[Preserve]
public class HasTrackedEntity : TargetedCompareRequirementBase
{
	// Token: 0x060035ED RID: 13805 RVA: 0x00163084 File Offset: 0x00161284
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityPlayerLocal entityPlayerLocal = this.target as EntityPlayerLocal;
		if (entityPlayerLocal == null)
		{
			return false;
		}
		bool flag = false;
		float value = EffectManager.GetValue(PassiveEffects.TrackDistance, null, 0f, entityPlayerLocal, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (value >= 0f)
		{
			List<Entity> entitiesInBounds = entityPlayerLocal.world.GetEntitiesInBounds(entityPlayerLocal, new Bounds(entityPlayerLocal.position, Vector3.one * (2f * value)));
			for (int i = 0; i < entitiesInBounds.Count; i++)
			{
				if (this.hasAllTags)
				{
					if (entitiesInBounds[i].HasAllTags(this.trackerTags))
					{
						flag = true;
						break;
					}
				}
				else if (entitiesInBounds[i].HasAnyTags(this.trackerTags))
				{
					flag = true;
					break;
				}
			}
		}
		return flag == !this.invert;
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x00163169 File Offset: 0x00161369
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} Tracking Entity", this.invert ? "NOT " : ""));
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x00163190 File Offset: 0x00161390
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.trackerTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
				return true;
			}
			if (localName == "has_all_tags")
			{
				this.hasAllTags = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002B73 RID: 11123
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> trackerTags;

	// Token: 0x04002B74 RID: 11124
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}
