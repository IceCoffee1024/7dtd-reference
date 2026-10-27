using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006BB RID: 1723
[Preserve]
public class ArmorGroupLowestQuality : TargetedCompareRequirementBase
{
	// Token: 0x060035FD RID: 13821 RVA: 0x001634E0 File Offset: 0x001616E0
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

	// Token: 0x060035FE RID: 13822 RVA: 0x00163541 File Offset: 0x00161741
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("ArmorGroupLowestQuality: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x00163580 File Offset: 0x00161780
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

	// Token: 0x04002B78 RID: 11128
	[PublicizedFrom(EAccessModifier.Private)]
	public string armorGroupName;
}
