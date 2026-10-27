using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000688 RID: 1672
[Preserve]
public class HoldingItemHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003557 RID: 13655 RVA: 0x00160FA0 File Offset: 0x0015F1A0
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		bool flag;
		if (!this.hasAllTags)
		{
			flag = this.target.inventory.holdingItem.HasAnyTags(this.holdingItemTags);
		}
		else
		{
			flag = this.target.inventory.holdingItem.HasAllTags(this.holdingItemTags);
		}
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x00160C3C File Offset: 0x0015EE3C
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x0016100C File Offset: 0x0015F20C
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "tags")
			{
				this.holdingItemTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
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

	// Token: 0x04002B4C RID: 11084
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> holdingItemTags;

	// Token: 0x04002B4D RID: 11085
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}
