using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000673 RID: 1651
[Preserve]
public class CVarCompare : TargetedCompareRequirementBase
{
	// Token: 0x0600352A RID: 13610 RVA: 0x0016041C File Offset: 0x0015E61C
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.invert != RequirementBase.compareValues(this.target.Buffs.GetCustomVar(this.cvarCompareName), this.operation, this.value);
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x0016045B File Offset: 0x0015E65B
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("cvar.{0} {1} {2}", this.cvarCompareName, this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x0016048C File Offset: 0x0015E68C
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

	// Token: 0x04002B37 RID: 11063
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvarCompareName;
}
