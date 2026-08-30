using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C8 RID: 1736
[Preserve]
public class MinEventActionAnimatorSetInt : MinEventActionTargetedBase
{
	// Token: 0x06003605 RID: 13829 RVA: 0x00165370 File Offset: 0x00163570
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

	// Token: 0x06003606 RID: 13830 RVA: 0x001653F4 File Offset: 0x001635F4
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

	// Token: 0x04002BA2 RID: 11170
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BA3 RID: 11171
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
