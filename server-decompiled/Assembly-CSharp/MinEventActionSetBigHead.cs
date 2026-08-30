using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E9 RID: 1769
[Preserve]
public class MinEventActionSetBigHead : MinEventActionTargetedBase
{
	// Token: 0x06003648 RID: 13896 RVA: 0x001669D4 File Offset: 0x00164BD4
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

	// Token: 0x06003649 RID: 13897 RVA: 0x00166A3C File Offset: 0x00164C3C
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

	// Token: 0x04002BA7 RID: 11175
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002BA8 RID: 11176
	[PublicizedFrom(EAccessModifier.Private)]
	public bool instant;
}
