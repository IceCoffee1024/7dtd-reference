using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F6 RID: 1782
[Preserve]
public class MinEventActionUnregisterSequenceLink : MinEventActionTargetedBase
{
	// Token: 0x060036A7 RID: 13991 RVA: 0x00168E18 File Offset: 0x00167018
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

	// Token: 0x060036A8 RID: 13992 RVA: 0x00168E6A File Offset: 0x0016706A
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "sequence_link")
		{
			this.sequenceLink = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C21 RID: 11297
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";
}
