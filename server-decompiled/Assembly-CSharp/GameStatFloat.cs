using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C0 RID: 1728
[Preserve]
public class GameStatFloat : TargetedCompareRequirementBase
{
	// Token: 0x0600360E RID: 13838 RVA: 0x001638B4 File Offset: 0x00161AB4
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

	// Token: 0x0600360F RID: 13839 RVA: 0x00163908 File Offset: 0x00161B08
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

	// Token: 0x04002B7D RID: 11133
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
