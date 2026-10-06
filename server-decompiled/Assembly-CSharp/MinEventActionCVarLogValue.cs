using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DA RID: 1754
[Preserve]
public class MinEventActionCVarLogValue : MinEventActionTargetedBase
{
	// Token: 0x06003675 RID: 13941 RVA: 0x00166C4C File Offset: 0x00164E4C
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

	// Token: 0x06003676 RID: 13942 RVA: 0x00166CAC File Offset: 0x00164EAC
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

	// Token: 0x04002BC3 RID: 11203
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;
}
