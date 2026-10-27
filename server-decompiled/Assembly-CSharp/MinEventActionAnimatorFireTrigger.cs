using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D3 RID: 1747
[Preserve]
public class MinEventActionAnimatorFireTrigger : MinEventActionTargetedBase
{
	// Token: 0x0600365D RID: 13917 RVA: 0x00166AB0 File Offset: 0x00164CB0
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

	// Token: 0x0600365E RID: 13918 RVA: 0x00166B2C File Offset: 0x00164D2C
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

	// Token: 0x04002BBA RID: 11194
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BBB RID: 11195
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
