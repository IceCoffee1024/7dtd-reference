using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000697 RID: 1687
[Preserve]
public class PlayerItemCountByTags : TargetedCompareRequirementBase
{
	// Token: 0x06003556 RID: 13654 RVA: 0x00160CAC File Offset: 0x0015EEAC
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

	// Token: 0x06003557 RID: 13655 RVA: 0x00160D2C File Offset: 0x0015EF2C
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

	// Token: 0x04002B54 RID: 11092
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;
}
