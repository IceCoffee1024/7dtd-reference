using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200071D RID: 1821
[Preserve]
public class MinEventActionAddChatMessage : MinEventActionTargetedBase
{
	// Token: 0x0600375E RID: 14174 RVA: 0x0016C790 File Offset: 0x0016A990
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

	// Token: 0x0600375F RID: 14175 RVA: 0x0016C7E7 File Offset: 0x0016A9E7
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x0016C800 File Offset: 0x0016AA00
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

	// Token: 0x04002CA3 RID: 11427
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;
}
