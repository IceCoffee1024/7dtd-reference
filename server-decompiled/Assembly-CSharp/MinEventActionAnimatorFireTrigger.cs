using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CA RID: 1738
[Preserve]
public class MinEventActionAnimatorFireTrigger : MinEventActionTargetedBase
{
	// Token: 0x0600360B RID: 13835 RVA: 0x00165544 File Offset: 0x00163744
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].emodel != null && this.targets[i].emodel.avatarController != null)
			{
				this.targets[i].emodel.avatarController.TriggerEvent(this.property);
			}
		}
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x001655C0 File Offset: 0x001637C0
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

	// Token: 0x04002BA6 RID: 11174
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BA7 RID: 11175
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
