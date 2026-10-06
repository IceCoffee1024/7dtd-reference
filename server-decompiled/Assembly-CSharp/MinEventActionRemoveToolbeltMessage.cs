using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000703 RID: 1795
[Preserve]
public class MinEventActionRemoveToolbeltMessage : MinEventActionBase
{
	// Token: 0x0600370A RID: 14090 RVA: 0x0016A1C0 File Offset: 0x001683C0
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.RemovePinnedTooltip(entityPlayerLocal, this.messageKey);
		}
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x0016A1EF File Offset: 0x001683EF
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C2F RID: 11311
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
