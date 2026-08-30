using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000711 RID: 1809
[Preserve]
public class MinEventActionAddChatMessage : MinEventActionTargetedBase
{
	// Token: 0x060036C5 RID: 14021 RVA: 0x00169C10 File Offset: 0x00167E10
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

	// Token: 0x060036C6 RID: 14022 RVA: 0x00169C67 File Offset: 0x00167E67
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00169C80 File Offset: 0x00167E80
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

	// Token: 0x04002C35 RID: 11317
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
