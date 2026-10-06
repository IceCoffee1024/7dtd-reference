using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F8 RID: 1784
[Preserve]
public class MinEventActionUnregisterSequenceLink : MinEventActionTargetedBase
{
	// Token: 0x060036EA RID: 14058 RVA: 0x0016977C File Offset: 0x0016797C
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

	// Token: 0x060036EB RID: 14059 RVA: 0x001697CE File Offset: 0x001679CE
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "sequence_link")
		{
			this.sequenceLink = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C1A RID: 11290
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";
}
