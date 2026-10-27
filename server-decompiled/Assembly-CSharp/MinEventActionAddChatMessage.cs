using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000725 RID: 1829
[Preserve]
public class MinEventActionAddChatMessage : MinEventActionTargetedBase
{
	// Token: 0x06003775 RID: 14197 RVA: 0x0016D728 File Offset: 0x0016B928
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				XUiC_ChatOutput.AddMessage(LocalPlayerUI.GetUIForPlayer(entityPlayerLocal).xui, EnumGameMessages.PlainTextLocal, this.message, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
			}
		}
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x0016D77F File Offset: 0x0016B97F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x0016D798 File Offset: 0x0016B998
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "message")
			{
				if (this.message == null || this.message == "")
				{
					this.message = _attribute.Value;
				}
				return true;
			}
			if (localName == "message_key")
			{
				if (Localization.Exists(_attribute.Value, false))
				{
					this.message = Localization.Get(_attribute.Value, false, null);
				}
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002CBF RID: 11455
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
