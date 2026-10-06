using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F5 RID: 1781
[Preserve]
public class MinEventActionSetBigHead : MinEventActionTargetedBase
{
	// Token: 0x060036E1 RID: 14049 RVA: 0x00169510 File Offset: 0x00167710
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

	// Token: 0x060036E2 RID: 14050 RVA: 0x00169578 File Offset: 0x00167778
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

	// Token: 0x04002C14 RID: 11284
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;

	// Token: 0x04002C15 RID: 11285
	[PublicizedFrom(EAccessModifier.Private)]
	public bool instant;
}
