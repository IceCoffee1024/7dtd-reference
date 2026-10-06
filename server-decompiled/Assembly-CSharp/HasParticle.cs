using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006AD RID: 1709
[Preserve]
public class HasParticle : TargetedCompareRequirementBase
{
	// Token: 0x060035D6 RID: 13782 RVA: 0x00162348 File Offset: 0x00160548
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

	// Token: 0x060035D7 RID: 13783 RVA: 0x00162384 File Offset: 0x00160584
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

	// Token: 0x04002B57 RID: 11095
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleName = "";
}
