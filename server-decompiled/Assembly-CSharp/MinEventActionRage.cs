using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000705 RID: 1797
[Preserve]
public class MinEventActionRage : MinEventActionTargetedBase
{
	// Token: 0x06003710 RID: 14096 RVA: 0x0016AA88 File Offset: 0x00168C88
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

	// Token: 0x06003711 RID: 14097 RVA: 0x0016AAF0 File Offset: 0x00168CF0
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

	// Token: 0x04002C3B RID: 11323
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002C3C RID: 11324
	[PublicizedFrom(EAccessModifier.Private)]
	public float speedPercent = 2f;

	// Token: 0x04002C3D RID: 11325
	[PublicizedFrom(EAccessModifier.Private)]
	public float rageTime = 60f;
}
