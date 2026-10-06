using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CB RID: 1739
[Preserve]
public class MinEventActionAnimatorSetBool : MinEventActionTargetedBase
{
	// Token: 0x0600364B RID: 13899 RVA: 0x00165DC8 File Offset: 0x00163FC8
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

	// Token: 0x0600364C RID: 13900 RVA: 0x00165E4C File Offset: 0x0016404C
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

	// Token: 0x04002B9D RID: 11165
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002B9E RID: 11166
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
