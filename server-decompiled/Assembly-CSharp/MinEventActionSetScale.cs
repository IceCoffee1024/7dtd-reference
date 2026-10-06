using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F9 RID: 1785
[Preserve]
public class MinEventActionSetScale : MinEventActionTargetedBase
{
	// Token: 0x060036ED RID: 14061 RVA: 0x00169810 File Offset: 0x00167A10
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			entityAlive.OverrideSize = this.scale;
			entityAlive.SetScale(this.scale);
		}
	}

	// Token: 0x060036EE RID: 14062 RVA: 0x00169858 File Offset: 0x00167A58
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

	// Token: 0x04002C1B RID: 11291
	[PublicizedFrom(EAccessModifier.Private)]
	public float scale = 1f;
}
