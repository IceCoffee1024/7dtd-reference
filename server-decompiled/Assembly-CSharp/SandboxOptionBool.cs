using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020006AE RID: 1710
[Preserve]
public class SandboxOptionBool : RequirementBase
{
	// Token: 0x0600356A RID: 13674 RVA: 0x0016021F File Offset: 0x0015E41F
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

	// Token: 0x0600356B RID: 13675 RVA: 0x0016024C File Offset: 0x0015E44C
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

	// Token: 0x04002AF6 RID: 10998
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.BiomeProgression;
}
