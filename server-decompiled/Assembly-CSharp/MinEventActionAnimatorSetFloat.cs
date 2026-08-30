using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006BD RID: 1725
[Preserve]
public class MinEventActionAnimatorSetFloat : MinEventActionTargetedBase
{
	// Token: 0x060035AC RID: 13740 RVA: 0x001630A0 File Offset: 0x001612A0
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

	// Token: 0x060035AD RID: 13741 RVA: 0x00163124 File Offset: 0x00161324
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

	// Token: 0x04002B2C RID: 11052
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B2D RID: 11053
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
