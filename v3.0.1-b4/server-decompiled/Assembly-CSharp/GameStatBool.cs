using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006AB RID: 1707
[Preserve]
public class GameStatBool : RequirementBase
{
	// Token: 0x06003561 RID: 13665 RVA: 0x0016004E File Offset: 0x0015E24E
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

	// Token: 0x06003562 RID: 13666 RVA: 0x00160078 File Offset: 0x0015E278
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

	// Token: 0x04002AF3 RID: 10995
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
