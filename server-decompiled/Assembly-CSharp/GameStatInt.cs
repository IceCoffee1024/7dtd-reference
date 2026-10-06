using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B8 RID: 1720
[Preserve]
public class GameStatInt : TargetedCompareRequirementBase
{
	// Token: 0x060035FD RID: 13821 RVA: 0x00162C14 File Offset: 0x00160E14
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		int @int = GameStats.GetInt(this.GameStat);
		if (this.invert)
		{
			return !RequirementBase.compareValues((float)@int, this.operation, this.value);
		}
		return RequirementBase.compareValues((float)@int, this.operation, this.value);
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x00162C6C File Offset: 0x00160E6C
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

	// Token: 0x04002B61 RID: 11105
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
