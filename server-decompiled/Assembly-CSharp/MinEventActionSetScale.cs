using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F7 RID: 1783
[Preserve]
public class MinEventActionSetScale : MinEventActionTargetedBase
{
	// Token: 0x060036AA RID: 13994 RVA: 0x00168EAC File Offset: 0x001670AC
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			entityAlive.OverrideSize = this.scale;
			entityAlive.SetScale(this.scale);
		}
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x00168EF4 File Offset: 0x001670F4
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

	// Token: 0x04002C22 RID: 11298
	[PublicizedFrom(EAccessModifier.Private)]
	public float scale = 1f;
}
