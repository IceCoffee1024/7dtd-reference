using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000702 RID: 1794
[Preserve]
public class MinEventActionPinToolbeltMessage : MinEventActionBase
{
	// Token: 0x06003707 RID: 14087 RVA: 0x0016A148 File Offset: 0x00168348
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.ShowTooltip(entityPlayerLocal, this.messageKey, false, true, 0f);
		}
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x0016A17E File Offset: 0x0016837E
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C2E RID: 11310
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
