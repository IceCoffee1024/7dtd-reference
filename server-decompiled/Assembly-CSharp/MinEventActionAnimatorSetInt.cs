using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D1 RID: 1745
[Preserve]
public class MinEventActionAnimatorSetInt : MinEventActionTargetedBase
{
	// Token: 0x06003657 RID: 13911 RVA: 0x001668DC File Offset: 0x00164ADC
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

	// Token: 0x06003658 RID: 13912 RVA: 0x00166960 File Offset: 0x00164B60
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

	// Token: 0x04002BB6 RID: 11190
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BB7 RID: 11191
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
