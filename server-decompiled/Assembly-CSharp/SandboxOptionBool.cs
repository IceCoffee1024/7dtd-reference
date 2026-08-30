using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006B8 RID: 1720
[Preserve]
public class SandboxOptionBool : RequirementBase
{
	// Token: 0x060035C0 RID: 13760 RVA: 0x001623F7 File Offset: 0x001605F7
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (SandboxOptionManager.GetBool(this.Option))
		{
			return !this.invert;
		}
		return this.invert;
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x00162424 File Offset: 0x00160624
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

	// Token: 0x04002B6A RID: 11114
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
