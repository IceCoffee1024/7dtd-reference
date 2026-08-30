using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F6 RID: 1782
[Preserve]
public class MinEventActionPinToolbeltMessage : MinEventActionBase
{
	// Token: 0x0600366E RID: 13934 RVA: 0x0016760C File Offset: 0x0016580C
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.ShowTooltip(entityPlayerLocal, this.messageKey, false, true, 0f);
		}
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x00167642 File Offset: 0x00165842
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002BC1 RID: 11201
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
