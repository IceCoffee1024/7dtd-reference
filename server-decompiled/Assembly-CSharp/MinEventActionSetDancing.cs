using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F6 RID: 1782
[Preserve]
public class MinEventActionSetDancing : MinEventActionTargetedBase
{
	// Token: 0x060036E4 RID: 14052 RVA: 0x001695E4 File Offset: 0x001677E4
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetDancing(this.enabled);
		}
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x0016961E File Offset: 0x0016781E
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002C16 RID: 11286
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;
}
