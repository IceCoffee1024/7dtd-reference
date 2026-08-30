using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000701 RID: 1793
[Preserve]
public class MinEventActionRemoveToolbeltMessage : MinEventActionBase
{
	// Token: 0x060036C7 RID: 14023 RVA: 0x0016985C File Offset: 0x00167A5C
	public override void Execute(MinEventParams _params)
	{
		base.Execute(_params);
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			GameManager.RemovePinnedTooltip(entityPlayerLocal, this.messageKey);
		}
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x0016988B File Offset: 0x00167A8B
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "message_key")
		{
			this.messageKey = _attribute.Value;
		}
		return flag;
	}

	// Token: 0x04002C36 RID: 11318
	[PublicizedFrom(EAccessModifier.Private)]
	public string messageKey = "";
}
