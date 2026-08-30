using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B1 RID: 1713
[Preserve]
public class ArmorGroupCount : TargetedCompareRequirementBase
{
	// Token: 0x060035A8 RID: 13736 RVA: 0x00161E9C File Offset: 0x0016009C
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

	// Token: 0x060035A9 RID: 13737 RVA: 0x00161EFD File Offset: 0x001600FD
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("ArmorGroupCount: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x00161F3C File Offset: 0x0016013C
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

	// Token: 0x04002B63 RID: 11107
	[PublicizedFrom(EAccessModifier.Private)]
	public string armorGroupName;
}
