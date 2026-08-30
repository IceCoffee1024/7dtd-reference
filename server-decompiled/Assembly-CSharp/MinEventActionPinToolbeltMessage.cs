using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000700 RID: 1792
[Preserve]
public class MinEventActionPinToolbeltMessage : MinEventActionBase
{
	// Token: 0x060036C4 RID: 14020 RVA: 0x001697E4 File Offset: 0x001679E4
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.ShowTooltip(entityPlayerLocal, this.messageKey, false, true, 0f);
		}
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x0016981A File Offset: 0x00167A1A
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C35 RID: 11317
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
