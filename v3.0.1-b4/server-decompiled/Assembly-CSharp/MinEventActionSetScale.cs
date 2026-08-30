using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006ED RID: 1773
[Preserve]
public class MinEventActionSetScale : MinEventActionTargetedBase
{
	// Token: 0x06003654 RID: 13908 RVA: 0x00166CD4 File Offset: 0x00164ED4
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			entityAlive.OverrideSize = this.scale;
			entityAlive.SetScale(this.scale);
		}
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x00166D1C File Offset: 0x00164F1C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "scale")
		{
			this.scale = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
			return true;
		}
		return flag;
	}

	// Token: 0x04002BAE RID: 11182
	[PublicizedFrom(EAccessModifier.Private)]
	public float scale = 1f;
}
