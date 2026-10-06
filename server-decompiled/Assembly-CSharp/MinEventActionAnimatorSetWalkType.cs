using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006CE RID: 1742
[Preserve]
public class MinEventActionAnimatorSetWalkType : MinEventActionTargetedBase
{
	// Token: 0x06003654 RID: 13908 RVA: 0x0016602C File Offset: 0x0016422C
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

	// Token: 0x06003655 RID: 13909 RVA: 0x00166070 File Offset: 0x00164270
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

	// Token: 0x04002BA3 RID: 11171
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
