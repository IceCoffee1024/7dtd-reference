using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EE RID: 1774
[Preserve]
public class MinEventActionSetHeadSize : MinEventActionTargetedBase
{
	// Token: 0x06003657 RID: 13911 RVA: 0x00166D7C File Offset: 0x00164F7C
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetHeadSize(this.standard);
		}
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x00166DB8 File Offset: 0x00164FB8
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

	// Token: 0x04002BAF RID: 11183
	[PublicizedFrom(EAccessModifier.Private)]
	public float standard = 1f;
}
