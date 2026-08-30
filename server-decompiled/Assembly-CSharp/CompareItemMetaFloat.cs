using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000675 RID: 1653
[Preserve]
public class CompareItemMetaFloat : TargetedCompareRequirementBase
{
	// Token: 0x060034B0 RID: 13488 RVA: 0x0015D804 File Offset: 0x0015BA04
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

	// Token: 0x060034B1 RID: 13489 RVA: 0x0015D878 File Offset: 0x0015BA78
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

	// Token: 0x04002AC3 RID: 10947
	[PublicizedFrom(EAccessModifier.Private)]
	public string metaKey;
}
