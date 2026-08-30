using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FB RID: 1787
[Preserve]
public class MinEventActionRage : MinEventActionTargetedBase
{
	// Token: 0x060036B6 RID: 14006 RVA: 0x0016918C File Offset: 0x0016738C
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityHuman entityHuman = this.targets[i] as EntityHuman;
			if (entityHuman != null)
			{
				if (this.enabled)
				{
					entityHuman.StartRage(this.speedPercent, this.rageTime + 1f);
				}
				else
				{
					entityHuman.StopRage();
				}
			}
		}
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x001691F4 File Offset: 0x001673F4
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "speed")
			{
				this.speedPercent = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
			if (localName == "time")
			{
				this.rageTime = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
			if (localName == "enabled")
			{
				this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
			}
		}
		return flag;
	}

	// Token: 0x04002C26 RID: 11302
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002C27 RID: 11303
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedPercent = 2f;

	// Token: 0x04002C28 RID: 11304
	[PublicizedFrom(EAccessModifier.Private)]
	public float rageTime = 60f;
}
