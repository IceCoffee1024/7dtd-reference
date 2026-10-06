using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CA RID: 1738
[Preserve]
public class MinEventActionAnimatorSetInt : MinEventActionTargetedBase
{
	// Token: 0x06003648 RID: 13896 RVA: 0x00165CE0 File Offset: 0x00163EE0
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

	// Token: 0x06003649 RID: 13897 RVA: 0x00165D64 File Offset: 0x00163F64
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

	// Token: 0x04002B9B RID: 11163
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B9C RID: 11164
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
