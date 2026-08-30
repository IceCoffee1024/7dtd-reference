using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006B9 RID: 1721
[Preserve]
public class SandboxOptionInt : TargetedCompareRequirementBase
{
	// Token: 0x060035C3 RID: 13763 RVA: 0x00162478 File Offset: 0x00160678
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

	// Token: 0x060035C4 RID: 13764 RVA: 0x001624D0 File Offset: 0x001606D0
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

	// Token: 0x04002B6B RID: 11115
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
