using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001958 RID: 6488
	[Preserve]
	public class ActionAddChatMessage : ActionBaseClientAction
	{
		// Token: 0x0600C7C3 RID: 51139 RVA: 0x00493F70 File Offset: 0x00492170
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				XUiC_ChatOutput.AddMessage(LocalPlayerUI.GetUIForPlayer(entityPlayerLocal).xui, EnumGameMessages.PlainTextLocal, this.text, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
			}
		}

		// Token: 0x0600C7C4 RID: 51140 RVA: 0x00493FA8 File Offset: 0x004921A8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionAddChatMessage.PropText))
			{
				this.text = properties.Values[ActionAddChatMessage.PropText];
			}
			if (properties.Values.ContainsKey(ActionAddChatMessage.PropTextKey))
			{
				this.textKey = properties.Values[ActionAddChatMessage.PropTextKey];
				this.text = Localization.Get(this.textKey, false, null);
			}
		}

		// Token: 0x0600C7C5 RID: 51141 RVA: 0x0049401F File Offset: 0x0049221F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddChatMessage
			{
				targetGroup = this.targetGroup,
				textKey = this.textKey,
				text = this.text
			};
		}

		// Token: 0x04009674 RID: 38516
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "";

		// Token: 0x04009675 RID: 38517
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x04009676 RID: 38518
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x04009677 RID: 38519
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";
	}
}
