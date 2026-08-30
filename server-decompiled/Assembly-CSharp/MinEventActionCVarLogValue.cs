using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D8 RID: 1752
[Preserve]
public class MinEventActionCVarLogValue : MinEventActionTargetedBase
{
	// Token: 0x06003632 RID: 13874 RVA: 0x001662DC File Offset: 0x001644DC
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			Log.Out("CVarLogValue: {0} == {1}", new object[]
			{
				this.cvarName,
				this.targets[i].Buffs.GetCustomVar(this.cvarName).ToCultureInvariantString()
			});
		}
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x0016633C File Offset: 0x0016453C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "cvar")
		{
			this.cvarName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002BCA RID: 11210
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;
}
