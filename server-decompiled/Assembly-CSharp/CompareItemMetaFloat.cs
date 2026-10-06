using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000680 RID: 1664
[Preserve]
public class CompareItemMetaFloat : TargetedCompareRequirementBase
{
	// Token: 0x06003546 RID: 13638 RVA: 0x001602F8 File Offset: 0x0015E4F8
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		ItemValue itemValue = _params.ItemValue;
		if (itemValue == null || string.IsNullOrEmpty(this.metaKey))
		{
			return false;
		}
		float num;
		if (!itemValue.TryGetMetadata(this.metaKey, out num))
		{
			return false;
		}
		if (this.invert)
		{
			return !RequirementBase.compareValues(num, this.operation, this.value);
		}
		return RequirementBase.compareValues(num, this.operation, this.value);
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x0016036C File Offset: 0x0015E56C
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "key")
		{
			this.metaKey = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B30 RID: 11056
	[PublicizedFrom(EAccessModifier.Private)]
	public string metaKey;
}
