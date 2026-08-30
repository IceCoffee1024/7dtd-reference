using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000681 RID: 1665
[Preserve]
public class ItemHasTags : TargetedCompareRequirementBase
{
	// Token: 0x0600350D RID: 13581 RVA: 0x0015FB6C File Offset: 0x0015DD6C
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

	// Token: 0x0600350E RID: 13582 RVA: 0x000027FC File Offset: 0x000009FC
	public override void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x0015FBE8 File Offset: 0x0015DDE8
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

	// Token: 0x04002B3A RID: 11066
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> currentItemTags;

	// Token: 0x04002B3B RID: 11067
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}
