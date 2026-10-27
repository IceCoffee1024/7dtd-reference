using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FD RID: 1789
[Preserve]
public class MinEventActionSetBigHead : MinEventActionTargetedBase
{
	// Token: 0x060036F8 RID: 14072 RVA: 0x0016A4A8 File Offset: 0x001686A8
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			if (this.enabled)
			{
				if (this.instant)
				{
					entityAlive.ForceBigHead();
				}
				else
				{
					entityAlive.SetBigHead();
				}
			}
			else if (this.instant)
			{
				entityAlive.ForceResetHead();
			}
			else
			{
				entityAlive.ResetHead();
			}
		}
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x0016A510 File Offset: 0x00168710
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "instant"))
			{
				if (localName == "enabled")
				{
					this.enabled = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				}
			}
			else
			{
				this.instant = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
			}
		}
		return flag;
	}

	// Token: 0x04002C30 RID: 11312
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002C31 RID: 11313
	[PublicizedFrom(EAccessModifier.Private)]
	public bool instant;
}
