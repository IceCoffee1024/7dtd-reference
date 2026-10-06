using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006BC RID: 1724
[Preserve]
public class SandboxOptionFloat : TargetedCompareRequirementBase
{
	// Token: 0x06003609 RID: 13833 RVA: 0x00162E94 File Offset: 0x00161094
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

	// Token: 0x0600360A RID: 13834 RVA: 0x00162EE8 File Offset: 0x001610E8
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

	// Token: 0x04002B65 RID: 11109
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
