using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000690 RID: 1680
[Preserve]
public class NotHasBuff : TargetedCompareRequirementBase
{
	// Token: 0x06003573 RID: 13683 RVA: 0x0016160C File Offset: 0x0015F80C
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (!this.invert)
		{
			return !this.target.Buffs.HasBuff(this.buffName);
		}
		return this.target.Buffs.HasBuff(this.buffName);
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x0016165C File Offset: 0x0015F85C
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "buff")
		{
			this.buffName = _attribute.Value.ToLower();
			return true;
		}
		return flag;
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x0016169F File Offset: 0x0015F89F
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Target does {0}have buff '{1}'", (!this.invert) ? "NOT " : "", this.buffName));
	}

	// Token: 0x04002B57 RID: 11095
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;
}
