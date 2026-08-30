using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006BF RID: 1727
[Preserve]
public class MinEventActionAnimatorSetBool : MinEventActionTargetedBase
{
	// Token: 0x060035B2 RID: 13746 RVA: 0x00163280 File Offset: 0x00161480
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].emodel != null && this.targets[i].emodel.avatarController != null)
			{
				this.targets[i].emodel.avatarController.UpdateBool(this.property, this.value, true);
			}
		}
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x00163304 File Offset: 0x00161504
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
				this.value = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002B30 RID: 11056
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B31 RID: 11057
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
