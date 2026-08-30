using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F8 RID: 1784
[Preserve]
public class MinEventActionSetHeadSize : MinEventActionTargetedBase
{
	// Token: 0x060036AD RID: 13997 RVA: 0x00168F54 File Offset: 0x00167154
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetHeadSize(this.standard);
		}
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x00168F90 File Offset: 0x00167190
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

	// Token: 0x04002C23 RID: 11299
	[PublicizedFrom(EAccessModifier.Private)]
	public float standard = 1f;
}
