using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FA RID: 1786
[Preserve]
public class MinEventActionSetHeadSize : MinEventActionTargetedBase
{
	// Token: 0x060036F0 RID: 14064 RVA: 0x001698B8 File Offset: 0x00167AB8
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetHeadSize(this.standard);
		}
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x001698F4 File Offset: 0x00167AF4
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "size")
		{
			this.standard = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
			return true;
		}
		return flag;
	}

	// Token: 0x04002C1C RID: 11292
	[PublicizedFrom(EAccessModifier.Private)]
	public float standard = 1f;
}
