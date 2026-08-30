using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EA RID: 1770
[Preserve]
public class MinEventActionSetDancing : MinEventActionTargetedBase
{
	// Token: 0x0600364B RID: 13899 RVA: 0x00166AA8 File Offset: 0x00164CA8
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targets[i].SetDancing(this.enabled);
		}
	}

	// Token: 0x0600364C RID: 13900 RVA: 0x00166AE2 File Offset: 0x00164CE2
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "enabled")
		{
			this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		}
		return flag;
	}

	// Token: 0x04002BA9 RID: 11177
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;
}
