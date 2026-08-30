using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A7 RID: 1703
[Preserve]
public class ArmorGroupCount : TargetedCompareRequirementBase
{
	// Token: 0x06003552 RID: 13650 RVA: 0x0015FCC4 File Offset: 0x0015DEC4
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int armorGroupCount = this.target.equipment.GetArmorGroupCount(this.armorGroupName);
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)armorGroupCount, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)armorGroupCount, this.operation, this.value);
	}

	// Token: 0x06003553 RID: 13651 RVA: 0x0015FD25 File Offset: 0x0015DF25
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("ArmorGroupCount: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x0015FD64 File Offset: 0x0015DF64
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "group_name")
		{
			this.armorGroupName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002AEF RID: 10991
	[PublicizedFrom(EAccessModifier.Private)]
	public string armorGroupName;
}
