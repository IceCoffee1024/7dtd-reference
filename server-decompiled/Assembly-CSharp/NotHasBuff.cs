using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000687 RID: 1671
[Preserve]
public class NotHasBuff : TargetedCompareRequirementBase
{
	// Token: 0x06003522 RID: 13602 RVA: 0x00160094 File Offset: 0x0015E294
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

	// Token: 0x06003523 RID: 13603 RVA: 0x001600E4 File Offset: 0x0015E2E4
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

	// Token: 0x06003524 RID: 13604 RVA: 0x00160127 File Offset: 0x0015E327
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Target does {0}have buff '{1}'", (!this.invert) ? "NOT " : "", this.buffName));
	}

	// Token: 0x04002B43 RID: 11075
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;
}
