using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006BA RID: 1722
[Preserve]
public class ArmorGroupCount : TargetedCompareRequirementBase
{
	// Token: 0x060035F9 RID: 13817 RVA: 0x00163400 File Offset: 0x00161600
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

	// Token: 0x060035FA RID: 13818 RVA: 0x00163461 File Offset: 0x00161661
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("ArmorGroupCount: {0}{1} {2}", this.invert ? "NOT " : "", this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x001634A0 File Offset: 0x001616A0
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

	// Token: 0x04002B77 RID: 11127
	[PublicizedFrom(EAccessModifier.Private)]
	public string armorGroupName;
}
