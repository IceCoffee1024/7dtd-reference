using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006AD RID: 1709
[Preserve]
public class GameStatFloat : TargetedCompareRequirementBase
{
	// Token: 0x06003567 RID: 13671 RVA: 0x00160178 File Offset: 0x0015E378
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		float @float = GameStats.GetFloat(this.GameStat);
		if (this.invert)
		{
			return !RequirementBase.compareValues(@float, this.operation, this.value);
		}
		return RequirementBase.compareValues(@float, this.operation, this.value);
	}

	// Token: 0x06003568 RID: 13672 RVA: 0x001601CC File Offset: 0x0015E3CC
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

	// Token: 0x04002AF5 RID: 10997
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
