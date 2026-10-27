using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000703 RID: 1795
[Preserve]
public class MinEventActionSetPitch : MinEventActionTargetedBase
{
	// Token: 0x0600370A RID: 14090 RVA: 0x0016A8EC File Offset: 0x00168AEC
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].OverridePitch = this.pitch;
		}
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x0016A928 File Offset: 0x00168B28
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

	// Token: 0x04002C39 RID: 11321
	[PublicizedFrom(EAccessModifier.Private)]
	public float pitch = 1f;
}
