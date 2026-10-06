using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006BA RID: 1722
[Preserve]
public class SandboxOptionBool : RequirementBase
{
	// Token: 0x06003603 RID: 13827 RVA: 0x00162D67 File Offset: 0x00160F67
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

	// Token: 0x06003604 RID: 13828 RVA: 0x00162D94 File Offset: 0x00160F94
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

	// Token: 0x04002B63 RID: 11107
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
