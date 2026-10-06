using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006BB RID: 1723
[Preserve]
public class SandboxOptionInt : TargetedCompareRequirementBase
{
	// Token: 0x06003606 RID: 13830 RVA: 0x00162DE8 File Offset: 0x00160FE8
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int @int = SandboxOptionManager.GetInt(this.Option);
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)@int, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)@int, this.operation, this.value);
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x00162E40 File Offset: 0x00161040
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "option")
		{
			this.Option = Enum.Parse<global::SandboxOptions.SandboxOptions>(_attribute.Value);
			return true;
		}
		return flag;
	}

	// Token: 0x04002B64 RID: 11108
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
