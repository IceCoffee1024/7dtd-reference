using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200071B RID: 1819
[Preserve]
public class MinEventActionAddChatMessage : MinEventActionTargetedBase
{
	// Token: 0x0600371B RID: 14107 RVA: 0x0016BE2C File Offset: 0x0016A02C
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

	// Token: 0x0600371C RID: 14108 RVA: 0x0016BE83 File Offset: 0x0016A083
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x0016BE9C File Offset: 0x0016A09C
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

	// Token: 0x04002CAA RID: 11434
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
