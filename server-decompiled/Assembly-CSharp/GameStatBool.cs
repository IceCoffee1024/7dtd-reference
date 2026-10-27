using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006BE RID: 1726
[Preserve]
public class GameStatBool : RequirementBase
{
	// Token: 0x06003608 RID: 13832 RVA: 0x0016378A File Offset: 0x0016198A
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

	// Token: 0x06003609 RID: 13833 RVA: 0x001637B4 File Offset: 0x001619B4
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

	// Token: 0x04002B7B RID: 11131
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
