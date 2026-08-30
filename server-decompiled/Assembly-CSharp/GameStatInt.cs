using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006AC RID: 1708
[Preserve]
public class GameStatInt : TargetedCompareRequirementBase
{
	// Token: 0x06003564 RID: 13668 RVA: 0x001600CC File Offset: 0x0015E2CC
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

	// Token: 0x06003565 RID: 13669 RVA: 0x00160124 File Offset: 0x0015E324
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

	// Token: 0x04002AF4 RID: 10996
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
