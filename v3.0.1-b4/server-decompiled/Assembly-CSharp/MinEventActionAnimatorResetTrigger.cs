using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C1 RID: 1729
[Preserve]
public class MinEventActionAnimatorResetTrigger : MinEventActionTargetedBase
{
	// Token: 0x060035B8 RID: 13752 RVA: 0x00163428 File Offset: 0x00161628
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

	// Token: 0x060035B9 RID: 13753 RVA: 0x001634A4 File Offset: 0x001616A4
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

	// Token: 0x04002B34 RID: 11060
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B35 RID: 11061
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
