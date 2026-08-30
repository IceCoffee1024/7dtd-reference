using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C9 RID: 1737
[Preserve]
public class MinEventActionAnimatorSetBool : MinEventActionTargetedBase
{
	// Token: 0x06003608 RID: 13832 RVA: 0x00165458 File Offset: 0x00163658
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

	// Token: 0x06003609 RID: 13833 RVA: 0x001654DC File Offset: 0x001636DC
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

	// Token: 0x04002BA4 RID: 11172
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BA5 RID: 11173
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
