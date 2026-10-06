using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000688 RID: 1672
[Preserve]
public class HasBuff : TargetedCompareRequirementBase
{
	// Token: 0x06003561 RID: 13665 RVA: 0x00160944 File Offset: 0x0015EB44
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

	// Token: 0x06003562 RID: 13666 RVA: 0x0016099C File Offset: 0x0015EB9C
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

	// Token: 0x06003563 RID: 13667 RVA: 0x001609E9 File Offset: 0x0015EBE9
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Target does {0}have buff '{1}(0)'", this.invert ? "NOT " : "", this.buffNames));
	}

	// Token: 0x04002B3B RID: 11067
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] buffNames;
}
