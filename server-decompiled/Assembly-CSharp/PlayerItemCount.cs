using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000696 RID: 1686
[Preserve]
public class PlayerItemCount : TargetedCompareRequirementBase
{
	// Token: 0x06003552 RID: 13650 RVA: 0x00160B80 File Offset: 0x0015ED80
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

	// Token: 0x06003553 RID: 13651 RVA: 0x00160C2D File Offset: 0x0015EE2D
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x00160C6C File Offset: 0x0015EE6C
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

	// Token: 0x04002B52 RID: 11090
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;

	// Token: 0x04002B53 RID: 11091
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue item;
}
