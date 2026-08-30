using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C0 RID: 1728
[Preserve]
public class MinEventActionAnimatorFireTrigger : MinEventActionTargetedBase
{
	// Token: 0x060035B5 RID: 13749 RVA: 0x0016336C File Offset: 0x0016156C
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

	// Token: 0x060035B6 RID: 13750 RVA: 0x001633E8 File Offset: 0x001615E8
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

	// Token: 0x04002B32 RID: 11058
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B33 RID: 11059
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
