using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200066C RID: 1644
[Preserve]
public class CVarCompare : TargetedCompareRequirementBase
{
	// Token: 0x0600351C RID: 13596 RVA: 0x0015F828 File Offset: 0x0015DA28
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.invert != RequirementBase.compareValues(this.target.Buffs.GetCustomVar(this.cvarCompareName), this.operation, this.value);
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x0015F867 File Offset: 0x0015DA67
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("cvar.{0} {1} {2}", this.cvarCompareName, this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x0015F898 File Offset: 0x0015DA98
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "cvar")
		{
			this.cvarCompareName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B1C RID: 11036
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvarCompareName;
}
