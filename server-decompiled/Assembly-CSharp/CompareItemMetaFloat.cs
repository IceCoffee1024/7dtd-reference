using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000687 RID: 1671
[Preserve]
public class CompareItemMetaFloat : TargetedCompareRequirementBase
{
	// Token: 0x06003554 RID: 13652 RVA: 0x00160EEC File Offset: 0x0015F0EC
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

	// Token: 0x06003555 RID: 13653 RVA: 0x00160F60 File Offset: 0x0015F160
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

	// Token: 0x04002B4B RID: 11083
	[PublicizedFrom(EAccessModifier.Private)]
	public string metaKey;
}
