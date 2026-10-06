using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B3 RID: 1715
[Preserve]
public class ArmorGroupCount : TargetedCompareRequirementBase
{
	// Token: 0x060035EB RID: 13803 RVA: 0x0016280C File Offset: 0x00160A0C
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

	// Token: 0x060035EC RID: 13804 RVA: 0x0016286D File Offset: 0x00160A6D
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("ArmorGroupCount: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x001628AC File Offset: 0x00160AAC
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

	// Token: 0x04002B5C RID: 11100
	[PublicizedFrom(EAccessModifier.Private)]
	public string armorGroupName;
}
