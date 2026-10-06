using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000689 RID: 1673
[Preserve]
public class NotHasBuff : TargetedCompareRequirementBase
{
	// Token: 0x06003565 RID: 13669 RVA: 0x00160A18 File Offset: 0x0015EC18
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

	// Token: 0x06003566 RID: 13670 RVA: 0x00160A68 File Offset: 0x0015EC68
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

	// Token: 0x06003567 RID: 13671 RVA: 0x00160AAB File Offset: 0x0015ECAB
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Target does {0}have buff '{1}'", (!this.invert) ? "NOT " : "", this.buffName));
	}

	// Token: 0x04002B3C RID: 11068
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;
}
