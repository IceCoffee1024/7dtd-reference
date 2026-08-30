using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C7 RID: 1735
[Preserve]
public class MinEventActionAnimatorSetFloat : MinEventActionTargetedBase
{
	// Token: 0x06003602 RID: 13826 RVA: 0x00165278 File Offset: 0x00163478
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].emodel != null && this.targets[i].emodel.avatarController != null)
			{
				this.targets[i].emodel.avatarController.UpdateFloat(this.property, this.value, true);
			}
		}
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x001652FC File Offset: 0x001634FC
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
				this.value = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002BA0 RID: 11168
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BA1 RID: 11169
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
