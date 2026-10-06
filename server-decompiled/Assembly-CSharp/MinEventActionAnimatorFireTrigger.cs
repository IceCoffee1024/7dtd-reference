using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CC RID: 1740
[Preserve]
public class MinEventActionAnimatorFireTrigger : MinEventActionTargetedBase
{
	// Token: 0x0600364E RID: 13902 RVA: 0x00165EB4 File Offset: 0x001640B4
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

	// Token: 0x0600364F RID: 13903 RVA: 0x00165F30 File Offset: 0x00164130
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

	// Token: 0x04002B9F RID: 11167
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BA0 RID: 11168
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
