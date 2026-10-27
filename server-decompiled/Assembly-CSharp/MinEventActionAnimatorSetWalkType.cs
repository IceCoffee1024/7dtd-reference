using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D5 RID: 1749
[Preserve]
public class MinEventActionAnimatorSetWalkType : MinEventActionTargetedBase
{
	// Token: 0x06003663 RID: 13923 RVA: 0x00166C28 File Offset: 0x00164E28
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

	// Token: 0x06003664 RID: 13924 RVA: 0x00166C6C File Offset: 0x00164E6C
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

	// Token: 0x04002BBE RID: 11198
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
