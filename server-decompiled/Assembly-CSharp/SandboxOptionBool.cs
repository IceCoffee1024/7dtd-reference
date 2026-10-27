using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006C1 RID: 1729
[Preserve]
public class SandboxOptionBool : RequirementBase
{
	// Token: 0x06003611 RID: 13841 RVA: 0x0016395B File Offset: 0x00161B5B
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

	// Token: 0x06003612 RID: 13842 RVA: 0x00163988 File Offset: 0x00161B88
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

	// Token: 0x04002B7E RID: 11134
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
