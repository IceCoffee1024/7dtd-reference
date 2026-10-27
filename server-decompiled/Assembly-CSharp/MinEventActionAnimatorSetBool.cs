using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D2 RID: 1746
[Preserve]
public class MinEventActionAnimatorSetBool : MinEventActionTargetedBase
{
	// Token: 0x0600365A RID: 13914 RVA: 0x001669C4 File Offset: 0x00164BC4
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

	// Token: 0x0600365B RID: 13915 RVA: 0x00166A48 File Offset: 0x00164C48
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

	// Token: 0x04002BB8 RID: 11192
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BB9 RID: 11193
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool value;
}
