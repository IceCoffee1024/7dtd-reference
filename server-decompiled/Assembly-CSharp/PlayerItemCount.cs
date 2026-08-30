using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068C RID: 1676
[Preserve]
public class PlayerItemCount : TargetedCompareRequirementBase
{
	// Token: 0x060034FC RID: 13564 RVA: 0x0015E9A8 File Offset: 0x0015CBA8
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

	// Token: 0x060034FD RID: 13565 RVA: 0x0015EA55 File Offset: 0x0015CC55
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Item count {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x0015EA94 File Offset: 0x0015CC94
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

	// Token: 0x04002ADE RID: 10974
	[PublicizedFrom(EAccessModifier.Private)]
	public string item_name;

	// Token: 0x04002ADF RID: 10975
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue item;
}
