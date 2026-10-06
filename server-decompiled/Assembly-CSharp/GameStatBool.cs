using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B7 RID: 1719
[Preserve]
public class GameStatBool : RequirementBase
{
	// Token: 0x060035FA RID: 13818 RVA: 0x00162B96 File Offset: 0x00160D96
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		if (GameStats.GetBool(this.GameStat))
		{
			return !this.invert;
		}
		return this.invert;
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x00162BC0 File Offset: 0x00160DC0
	public override bool ParseXAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "gamestat")
		{
			this.GameStat = Enum.Parse<EnumGameStats>(_attribute.Value);
			return true;
		}
		return flag;
	}

	// Token: 0x04002B60 RID: 11104
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
