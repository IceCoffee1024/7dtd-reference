using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000698 RID: 1688
[Preserve]
public class PlayerItemCount : TargetedCompareRequirementBase
{
	// Token: 0x06003595 RID: 13717 RVA: 0x00161500 File Offset: 0x0015F700
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (this.item_name != null && this.item == null)
		{
			this.item = ItemClass.GetItem(this.item_name, true);
		}
		if (this.item == null)
		{
			return false;
		}
		int num = this.target.inventory.GetItemCount(this.item, false, -1, -1, true);
		num += this.target.bag.GetItemCount(this.item, -1, -1, true);
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)num, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)num, this.operation, this.value);
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x001615AD File Offset: 0x0015F7AD
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x001615EC File Offset: 0x0015F7EC
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "item_name")
		{
			this.item_name = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B4B RID: 11083
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;

	// Token: 0x04002B4C RID: 11084
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue item;
}
