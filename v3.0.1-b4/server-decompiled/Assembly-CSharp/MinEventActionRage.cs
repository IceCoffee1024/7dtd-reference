using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F1 RID: 1777
[Preserve]
public class MinEventActionRage : MinEventActionTargetedBase
{
	// Token: 0x06003660 RID: 13920 RVA: 0x00166FB4 File Offset: 0x001651B4
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

	// Token: 0x06003661 RID: 13921 RVA: 0x0016701C File Offset: 0x0016521C
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

	// Token: 0x04002BB2 RID: 11186
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002BB3 RID: 11187
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedPercent = 2f;

	// Token: 0x04002BB4 RID: 11188
	[PublicizedFrom(EAccessModifier.Private)]
	public float rageTime = 60f;
}
