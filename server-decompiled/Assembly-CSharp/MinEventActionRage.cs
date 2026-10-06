using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FD RID: 1789
[Preserve]
public class MinEventActionRage : MinEventActionTargetedBase
{
	// Token: 0x060036F9 RID: 14073 RVA: 0x00169AF0 File Offset: 0x00167CF0
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

	// Token: 0x060036FA RID: 14074 RVA: 0x00169B58 File Offset: 0x00167D58
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

	// Token: 0x04002C1F RID: 11295
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002C20 RID: 11296
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedPercent = 2f;

	// Token: 0x04002C21 RID: 11297
	[PublicizedFrom(EAccessModifier.Private)]
	public float rageTime = 60f;
}
