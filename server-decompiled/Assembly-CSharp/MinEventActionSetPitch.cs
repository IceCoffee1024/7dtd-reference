using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F9 RID: 1785
[Preserve]
public class MinEventActionSetPitch : MinEventActionTargetedBase
{
	// Token: 0x060036B0 RID: 14000 RVA: 0x00168FF0 File Offset: 0x001671F0
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].OverridePitch = this.pitch;
		}
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x0016902C File Offset: 0x0016722C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "pitch")
		{
			this.pitch = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
			return true;
		}
		return flag;
	}

	// Token: 0x04002C24 RID: 11300
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitch = 1f;
}
