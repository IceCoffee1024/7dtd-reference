using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FB RID: 1787
[Preserve]
public class MinEventActionSetPitch : MinEventActionTargetedBase
{
	// Token: 0x060036F3 RID: 14067 RVA: 0x00169954 File Offset: 0x00167B54
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].OverridePitch = this.pitch;
		}
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x00169990 File Offset: 0x00167B90
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

	// Token: 0x04002C1D RID: 11293
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitch = 1f;
}
