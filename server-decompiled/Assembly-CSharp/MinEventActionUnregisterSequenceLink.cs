using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000700 RID: 1792
[Preserve]
public class MinEventActionUnregisterSequenceLink : MinEventActionTargetedBase
{
	// Token: 0x06003701 RID: 14081 RVA: 0x0016A714 File Offset: 0x00168914
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

	// Token: 0x06003702 RID: 14082 RVA: 0x0016A766 File Offset: 0x00168966
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "sequence_link")
		{
			this.sequenceLink = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C36 RID: 11318
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";
}
