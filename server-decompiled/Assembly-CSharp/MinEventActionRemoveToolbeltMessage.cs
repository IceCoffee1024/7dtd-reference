using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200070B RID: 1803
[Preserve]
public class MinEventActionRemoveToolbeltMessage : MinEventActionBase
{
	// Token: 0x06003721 RID: 14113 RVA: 0x0016B158 File Offset: 0x00169358
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.RemovePinnedTooltip(entityPlayerLocal, this.messageKey);
		}
	}

	// Token: 0x06003722 RID: 14114 RVA: 0x0016B187 File Offset: 0x00169387
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C4B RID: 11339
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
