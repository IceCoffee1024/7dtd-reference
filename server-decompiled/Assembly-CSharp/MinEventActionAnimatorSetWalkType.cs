using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CC RID: 1740
[Preserve]
public class MinEventActionAnimatorSetWalkType : MinEventActionTargetedBase
{
	// Token: 0x06003611 RID: 13841 RVA: 0x001656BC File Offset: 0x001638BC
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityAlive entityAlive = this.targets[i];
			if (entityAlive)
			{
				entityAlive.SetWalkType(this.value);
			}
		}
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x00165700 File Offset: 0x00163900
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "value")
		{
			this.value = int.Parse(_attribute.Value);
			return true;
		}
		return flag;
	}

	// Token: 0x04002BAA RID: 11178
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
