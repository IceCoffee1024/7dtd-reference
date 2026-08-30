using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000661 RID: 1633
[Preserve]
public class CVarCompare : TargetedCompareRequirementBase
{
	// Token: 0x06003486 RID: 13446 RVA: 0x0015CD54 File Offset: 0x0015AF54
	public override bool IsValid(MinEventParams _params)
	{
		return base.IsValid(_params) && this.invert != RequirementBase.compareValues(this.target.Buffs.GetCustomVar(this.cvarCompareName), this.operation, this.value);
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x0015CD93 File Offset: 0x0015AF93
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("cvar.{0} {1} {2}", this.cvarCompareName, this.operation.ToStringCached<RequirementBase.OperationTypes>(), this.value.ToCultureInvariantString()));
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x0015CDC4 File Offset: 0x0015AFC4
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

	// Token: 0x04002AAF RID: 10927
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvarCompareName;
}
