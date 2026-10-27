using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000701 RID: 1793
[Preserve]
public class MinEventActionSetScale : MinEventActionTargetedBase
{
	// Token: 0x06003704 RID: 14084 RVA: 0x0016A7A8 File Offset: 0x001689A8
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			entityAlive.OverrideSize = this.scale;
			entityAlive.SetScale(this.scale);
		}
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x0016A7F0 File Offset: 0x001689F0
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

	// Token: 0x04002C37 RID: 11319
	[PublicizedFrom(EAccessModifier.Private)]
	public float scale = 1f;
}
