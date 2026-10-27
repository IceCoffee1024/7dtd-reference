using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D0 RID: 1744
[Preserve]
public class MinEventActionAnimatorSetFloat : MinEventActionTargetedBase
{
	// Token: 0x06003654 RID: 13908 RVA: 0x001667E4 File Offset: 0x001649E4
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

	// Token: 0x06003655 RID: 13909 RVA: 0x00166868 File Offset: 0x00164A68
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

	// Token: 0x04002BB4 RID: 11188
	[PublicizedFrom(EAccessModifier.Protected)]
	public string property;

	// Token: 0x04002BB5 RID: 11189
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
