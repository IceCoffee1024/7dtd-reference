using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068D RID: 1677
[Preserve]
public class PlayerItemCountByTags : TargetedCompareRequirementBase
{
	// Token: 0x06003500 RID: 13568 RVA: 0x0015EAD4 File Offset: 0x0015CCD4
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

	// Token: 0x06003501 RID: 13569 RVA: 0x0015EB54 File Offset: 0x0015CD54
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

	// Token: 0x04002AE0 RID: 10976
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> itemTags;
}
