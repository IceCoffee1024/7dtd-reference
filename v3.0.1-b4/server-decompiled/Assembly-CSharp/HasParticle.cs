using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006A1 RID: 1697
[Preserve]
public class HasParticle : TargetedCompareRequirementBase
{
	// Token: 0x0600353D RID: 13629 RVA: 0x0015F808 File Offset: 0x0015DA08
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

	// Token: 0x0600353E RID: 13630 RVA: 0x0015F844 File Offset: 0x0015DA44
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

	// Token: 0x04002AEA RID: 10986
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleName = "";
}
