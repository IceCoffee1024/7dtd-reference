using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006BF RID: 1727
[Preserve]
public class GameStatInt : TargetedCompareRequirementBase
{
	// Token: 0x0600360B RID: 13835 RVA: 0x00163808 File Offset: 0x00161A08
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

	// Token: 0x0600360C RID: 13836 RVA: 0x00163860 File Offset: 0x00161A60
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

	// Token: 0x04002B7C RID: 11132
	[PublicizedFrom(EAccessModifier.Protected)]
	public EnumGameStats GameStat = EnumGameStats.AnimalCount;
}
