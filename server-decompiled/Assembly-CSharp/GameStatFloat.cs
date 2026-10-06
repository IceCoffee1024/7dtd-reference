using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006B9 RID: 1721
[Preserve]
public class GameStatFloat : TargetedCompareRequirementBase
{
	// Token: 0x06003600 RID: 13824 RVA: 0x00162CC0 File Offset: 0x00160EC0
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

	// Token: 0x06003601 RID: 13825 RVA: 0x00162D14 File Offset: 0x00160F14
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

	// Token: 0x04002B62 RID: 11106
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
