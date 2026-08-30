using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EF RID: 1775
[Preserve]
public class MinEventActionSetPitch : MinEventActionTargetedBase
{
	// Token: 0x0600365A RID: 13914 RVA: 0x00166E18 File Offset: 0x00165018
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].OverridePitch = this.pitch;
		}
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x00166E54 File Offset: 0x00165054
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

	// Token: 0x04002BB0 RID: 11184
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitch = 1f;
}
