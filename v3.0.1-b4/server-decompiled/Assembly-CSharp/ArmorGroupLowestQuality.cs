using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A8 RID: 1704
[Preserve]
public class ArmorGroupLowestQuality : TargetedCompareRequirementBase
{
	// Token: 0x06003556 RID: 13654 RVA: 0x0015FDA4 File Offset: 0x0015DFA4
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int armorGroupLowestQuality = this.target.equipment.GetArmorGroupLowestQuality(this.armorGroupName);
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)armorGroupLowestQuality, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)armorGroupLowestQuality, this.operation, this.value);
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x0015FE05 File Offset: 0x0015E005
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("ArmorGroupLowestQuality: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x0015FE44 File Offset: 0x0015E044
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

	// Token: 0x04002AF0 RID: 10992
	[PublicizedFrom(EAccessModifier.Private)]
	public string armorGroupName;
}
