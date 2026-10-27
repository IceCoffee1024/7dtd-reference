using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D4 RID: 1748
[Preserve]
public class MinEventActionAnimatorResetTrigger : MinEventActionTargetedBase
{
	// Token: 0x06003660 RID: 13920 RVA: 0x00166B6C File Offset: 0x00164D6C
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

	// Token: 0x06003661 RID: 13921 RVA: 0x00166BE8 File Offset: 0x00164DE8
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

	// Token: 0x04002BBC RID: 11196
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BBD RID: 11197
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
