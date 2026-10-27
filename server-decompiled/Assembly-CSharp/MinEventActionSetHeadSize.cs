using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000702 RID: 1794
[Preserve]
public class MinEventActionSetHeadSize : MinEventActionTargetedBase
{
	// Token: 0x06003707 RID: 14087 RVA: 0x0016A850 File Offset: 0x00168A50
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetHeadSize(this.standard);
		}
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x0016A88C File Offset: 0x00168A8C
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

	// Token: 0x04002C38 RID: 11320
	[PublicizedFrom(EAccessModifier.Private)]
	public float standard = 1f;
}
