using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A0 RID: 1696
[Preserve]
public class PlayerItemCountByTags : TargetedCompareRequirementBase
{
	// Token: 0x060035A7 RID: 13735 RVA: 0x00162220 File Offset: 0x00160420
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int num = this.target.inventory.GetItemCount(this.itemTags, -1, -1, true);
		num += this.target.bag.GetItemCount(this.itemTags, -1, -1, true);
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)num, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)num, this.operation, this.value);
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x001622A0 File Offset: 0x001604A0
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "item_tags")
		{
			this.itemTags = FastTags<TagGroup.Global>.Parse(_attribute.Value);
			return true;
		}
		return flag;
	}

	// Token: 0x04002B68 RID: 11112
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;
}
