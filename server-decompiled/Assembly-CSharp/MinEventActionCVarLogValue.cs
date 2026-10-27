using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E2 RID: 1762
[Preserve]
public class MinEventActionCVarLogValue : MinEventActionTargetedBase
{
	// Token: 0x0600368C RID: 13964 RVA: 0x00167A34 File Offset: 0x00165C34
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

	// Token: 0x0600368D RID: 13965 RVA: 0x00167A94 File Offset: 0x00165C94
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

	// Token: 0x04002BDF RID: 11231
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;
}
