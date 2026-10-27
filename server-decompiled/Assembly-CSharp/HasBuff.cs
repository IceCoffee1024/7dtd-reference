using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200068F RID: 1679
[Preserve]
public class HasBuff : TargetedCompareRequirementBase
{
	// Token: 0x0600356F RID: 13679 RVA: 0x00161538 File Offset: 0x0015F738
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

	// Token: 0x06003570 RID: 13680 RVA: 0x00161590 File Offset: 0x0015F790
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

	// Token: 0x06003571 RID: 13681 RVA: 0x001615DD File Offset: 0x0015F7DD
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Target does {0}have buff '{1}(0)'", this.invert ? "NOT " : "", this.buffNames));
	}

	// Token: 0x04002B56 RID: 11094
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] buffNames;
}
