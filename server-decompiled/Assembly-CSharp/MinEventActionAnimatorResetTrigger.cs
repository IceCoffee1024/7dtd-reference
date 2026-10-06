using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CD RID: 1741
[Preserve]
public class MinEventActionAnimatorResetTrigger : MinEventActionTargetedBase
{
	// Token: 0x06003651 RID: 13905 RVA: 0x00165F70 File Offset: 0x00164170
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].emodel != null && this.targets[i].emodel.avatarController != null)
			{
				this.targets[i].emodel.avatarController.CancelEvent(this.property);
			}
		}
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x00165FEC File Offset: 0x001641EC
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "property")
		{
			this.property = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002BA1 RID: 11169
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BA2 RID: 11170
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
