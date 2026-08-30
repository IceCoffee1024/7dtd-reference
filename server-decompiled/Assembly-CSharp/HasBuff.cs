using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200067C RID: 1660
[Preserve]
public class HasBuff : TargetedCompareRequirementBase
{
	// Token: 0x060034C8 RID: 13512 RVA: 0x0015DDE4 File Offset: 0x0015BFE4
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int num = this.buffNames.Length;
		for (int i = 0; i < num; i++)
		{
			if (this.target.Buffs.HasBuff(this.buffNames[i]))
			{
				return !this.invert;
			}
		}
		return this.invert;
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x0015DE3C File Offset: 0x0015C03C
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "buff")
		{
			this.buffNames = _attribute.Value.ToLower().Split(RequirementBase.commaSeparator);
			return true;
		}
		return flag;
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x0015DE89 File Offset: 0x0015C089
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Target does {0}have buff '{1}(0)'", this.invert ? "NOT " : "", this.buffNames));
	}

	// Token: 0x04002ACE RID: 10958
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] buffNames;
}
