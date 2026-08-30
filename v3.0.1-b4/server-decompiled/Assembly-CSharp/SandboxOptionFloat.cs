using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006B0 RID: 1712
[Preserve]
public class SandboxOptionFloat : TargetedCompareRequirementBase
{
	// Token: 0x06003570 RID: 13680 RVA: 0x0016034C File Offset: 0x0015E54C
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		float @float = SandboxOptionManager.GetFloat(this.Option);
		if (this.invert)
		{
			return !RequirementBase.compareValues(@float, this.operation, this.value);
		}
		return RequirementBase.compareValues(@float, this.operation, this.value);
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x001603A0 File Offset: 0x0015E5A0
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

	// Token: 0x04002AF8 RID: 11000
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
