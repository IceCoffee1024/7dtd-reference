using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006C2 RID: 1730
[Preserve]
public class SandboxOptionInt : TargetedCompareRequirementBase
{
	// Token: 0x06003614 RID: 13844 RVA: 0x001639DC File Offset: 0x00161BDC
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

	// Token: 0x06003615 RID: 13845 RVA: 0x00163A34 File Offset: 0x00161C34
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

	// Token: 0x04002B7F RID: 11135
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
