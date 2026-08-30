using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F3 RID: 1779
[Preserve]
public class MinEventActionSetBigHead : MinEventActionTargetedBase
{
	// Token: 0x0600369E RID: 13982 RVA: 0x00168BAC File Offset: 0x00166DAC
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

	// Token: 0x0600369F RID: 13983 RVA: 0x00168C14 File Offset: 0x00166E14
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

	// Token: 0x04002C1B RID: 11291
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002C1C RID: 11292
	[PublicizedFrom(EAccessModifier.Private)]
	public bool instant;
}
