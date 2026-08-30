using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200067D RID: 1661
[Preserve]
public class NotHasBuff : TargetedCompareRequirementBase
{
	// Token: 0x060034CC RID: 13516 RVA: 0x0015DEB8 File Offset: 0x0015C0B8
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

	// Token: 0x060034CD RID: 13517 RVA: 0x0015DF08 File Offset: 0x0015C108
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

	// Token: 0x060034CE RID: 13518 RVA: 0x0015DF4B File Offset: 0x0015C14B
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Target does {0}have buff '{1}'", (!this.invert) ? "NOT " : "", this.buffName));
	}

	// Token: 0x04002ACF RID: 10959
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;
}
