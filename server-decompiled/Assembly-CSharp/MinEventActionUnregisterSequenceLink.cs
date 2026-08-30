using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EC RID: 1772
[Preserve]
public class MinEventActionUnregisterSequenceLink : MinEventActionTargetedBase
{
	// Token: 0x06003651 RID: 13905 RVA: 0x00166C40 File Offset: 0x00164E40
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] != null)
			{
				GameEventManager.Current.UnRegisterLink(_params.Self as EntityPlayer, this.sequenceLink);
			}
		}
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x00166C92 File Offset: 0x00164E92
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "sequence_link")
		{
			this.sequenceLink = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002BAD RID: 11181
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";
}
