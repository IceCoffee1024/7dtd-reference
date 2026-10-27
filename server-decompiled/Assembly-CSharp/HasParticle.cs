using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B4 RID: 1716
[Preserve]
public class HasParticle : TargetedCompareRequirementBase
{
	// Token: 0x060035E4 RID: 13796 RVA: 0x00162F3C File Offset: 0x0016113C
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return _params.Self.HasParticle(this.particleName);
		}
		return !_params.Self.HasParticle(this.particleName);
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x00162F78 File Offset: 0x00161178
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "particle")
		{
			this.particleName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B72 RID: 11122
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleName = "";
}
