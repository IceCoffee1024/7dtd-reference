using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C9 RID: 1737
[Preserve]
public class MinEventActionAnimatorSetFloat : MinEventActionTargetedBase
{
	// Token: 0x06003645 RID: 13893 RVA: 0x00165BE8 File Offset: 0x00163DE8
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

	// Token: 0x06003646 RID: 13894 RVA: 0x00165C6C File Offset: 0x00163E6C
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

	// Token: 0x04002B99 RID: 11161
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B9A RID: 11162
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
