using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F4 RID: 1780
[Preserve]
public class MinEventActionSetDancing : MinEventActionTargetedBase
{
	// Token: 0x060036A1 RID: 13985 RVA: 0x00168C80 File Offset: 0x00166E80
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetDancing(this.enabled);
		}
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x00168CBA File Offset: 0x00166EBA
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002C1D RID: 11293
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;
}
