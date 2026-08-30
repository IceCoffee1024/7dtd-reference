using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F7 RID: 1783
[Preserve]
public class MinEventActionRemoveToolbeltMessage : MinEventActionBase
{
	// Token: 0x06003671 RID: 13937 RVA: 0x00167684 File Offset: 0x00165884
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.RemovePinnedTooltip(entityPlayerLocal, this.messageKey);
		}
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x001676B3 File Offset: 0x001658B3
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002BC2 RID: 11202
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
