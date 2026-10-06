using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000681 RID: 1665
[Preserve]
public class HoldingItemHasTags : TargetedCompareRequirementBase
{
	// Token: 0x06003549 RID: 13641 RVA: 0x001603AC File Offset: 0x0015E5AC
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

	// Token: 0x0600354A RID: 13642 RVA: 0x00160048 File Offset: 0x0015E248
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x00160418 File Offset: 0x0015E618
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

	// Token: 0x04002B31 RID: 11057
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> holdingItemTags;

	// Token: 0x04002B32 RID: 11058
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}
