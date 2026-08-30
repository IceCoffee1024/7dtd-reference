using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006C2 RID: 1730
[Preserve]
public class MinEventActionAnimatorSetWalkType : MinEventActionTargetedBase
{
	// Token: 0x060035BB RID: 13755 RVA: 0x001634E4 File Offset: 0x001616E4
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

	// Token: 0x060035BC RID: 13756 RVA: 0x00163528 File Offset: 0x00161728
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

	// Token: 0x04002B36 RID: 11062
	[PublicizedFrom(EAccessModifier.Protected)]
	public int value;
}
