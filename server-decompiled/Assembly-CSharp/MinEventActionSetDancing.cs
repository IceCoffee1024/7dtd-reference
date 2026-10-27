using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FE RID: 1790
[Preserve]
public class MinEventActionSetDancing : MinEventActionTargetedBase
{
	// Token: 0x060036FB RID: 14075 RVA: 0x0016A57C File Offset: 0x0016877C
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetDancing(this.enabled);
		}
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x0016A5B6 File Offset: 0x001687B6
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002C32 RID: 11314
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;
}
