using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200069F RID: 1695
[Preserve]
public class PlayerItemCount : TargetedCompareRequirementBase
{
	// Token: 0x060035A3 RID: 13731 RVA: 0x001620F4 File Offset: 0x001602F4
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

	// Token: 0x060035A4 RID: 13732 RVA: 0x001621A1 File Offset: 0x001603A1
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x001621E0 File Offset: 0x001603E0
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

	// Token: 0x04002B66 RID: 11110
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;

	// Token: 0x04002B67 RID: 11111
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue item;
}
