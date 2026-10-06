using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000699 RID: 1689
[Preserve]
public class PlayerItemCountByTags : TargetedCompareRequirementBase
{
	// Token: 0x06003599 RID: 13721 RVA: 0x0016162C File Offset: 0x0015F82C
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

	// Token: 0x0600359A RID: 13722 RVA: 0x001616AC File Offset: 0x0015F8AC
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

	// Token: 0x04002B4D RID: 11085
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;
}
