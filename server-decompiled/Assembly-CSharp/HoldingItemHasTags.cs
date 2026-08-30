using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000676 RID: 1654
[Preserve]
public class HoldingItemHasTags : TargetedCompareRequirementBase
{
	// Token: 0x060034B3 RID: 13491 RVA: 0x0015D8B8 File Offset: 0x0015BAB8
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

	// Token: 0x060034B4 RID: 13492 RVA: 0x0015D574 File Offset: 0x0015B774
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0}Male", this.invert ? "NOT " : ""));
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x0015D924 File Offset: 0x0015BB24
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

	// Token: 0x04002AC4 RID: 10948
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> holdingItemTags;

	// Token: 0x04002AC5 RID: 10949
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAllTags;
}
