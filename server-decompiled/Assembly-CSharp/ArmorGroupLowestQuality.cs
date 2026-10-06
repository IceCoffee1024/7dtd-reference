using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B4 RID: 1716
[Preserve]
public class ArmorGroupLowestQuality : TargetedCompareRequirementBase
{
	// Token: 0x060035EF RID: 13807 RVA: 0x001628EC File Offset: 0x00160AEC
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

	// Token: 0x060035F0 RID: 13808 RVA: 0x0016294D File Offset: 0x00160B4D
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("ArmorGroupLowestQuality: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x0016298C File Offset: 0x00160B8C
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

	// Token: 0x04002B5D RID: 11101
	[PublicizedFrom(EAccessModifier.Private)]
	public string armorGroupName;
}
