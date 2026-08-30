using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CB RID: 1739
[Preserve]
public class MinEventActionAnimatorResetTrigger : MinEventActionTargetedBase
{
	// Token: 0x0600360E RID: 13838 RVA: 0x00165600 File Offset: 0x00163800
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

	// Token: 0x0600360F RID: 13839 RVA: 0x0016567C File Offset: 0x0016387C
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

	// Token: 0x04002BA8 RID: 11176
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BA9 RID: 11177
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
