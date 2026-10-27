using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200070A RID: 1802
[Preserve]
public class MinEventActionPinToolbeltMessage : MinEventActionBase
{
	// Token: 0x0600371E RID: 14110 RVA: 0x0016B0E0 File Offset: 0x001692E0
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.ShowTooltip(entityPlayerLocal, this.messageKey, false, true, 0f);
		}
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x0016B116 File Offset: 0x00169316
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C4A RID: 11338
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
