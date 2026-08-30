using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006BA RID: 1722
[Preserve]
public class SandboxOptionFloat : TargetedCompareRequirementBase
{
	// Token: 0x060035C6 RID: 13766 RVA: 0x00162524 File Offset: 0x00160724
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

	// Token: 0x060035C7 RID: 13767 RVA: 0x00162578 File Offset: 0x00160778
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

	// Token: 0x04002B6C RID: 11116
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
