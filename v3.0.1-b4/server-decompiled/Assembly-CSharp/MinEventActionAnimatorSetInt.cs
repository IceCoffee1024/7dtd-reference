using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006BE RID: 1726
[Preserve]
public class MinEventActionAnimatorSetInt : MinEventActionTargetedBase
{
	// Token: 0x060035AF RID: 13743 RVA: 0x00163198 File Offset: 0x00161398
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].emodel != null && this.targets[i].emodel.avatarController != null)
			{
				this.targets[i].emodel.avatarController.UpdateInt(this.property, this.value, true);
			}
		}
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x0016321C File Offset: 0x0016141C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "property")
			{
				this.property = _attribute.Value;
				return true;
			}
			if (localName == "value")
			{
				this.value = int.Parse(_attribute.Value);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002B2E RID: 11054
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B2F RID: 11055
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
