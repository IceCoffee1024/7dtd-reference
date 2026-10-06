using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000683 RID: 1667
[Preserve]
public class ItemHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003550 RID: 13648 RVA: 0x001604F0 File Offset: 0x0015E6F0
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag = false;
		if (!_params.ItemValue.IsEmpty() && _params.ItemValue.ItemClass != null)
		{
			if (!this.hasAllTags)
			{
				flag = _params.ItemValue.ItemClass.HasAnyTags(this.currentItemTags);
			}
			else
			{
				flag = _params.ItemValue.ItemClass.HasAllTags(this.currentItemTags);
			}
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x0016056C File Offset: 0x0015E76C
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.currentItemTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
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

	// Token: 0x04002B33 RID: 11059
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentItemTags;

	// Token: 0x04002B34 RID: 11060
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}
