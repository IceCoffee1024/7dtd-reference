using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B5 RID: 6581
	[Preserve]
	public class ActionAddChatMessage : ActionBaseClientAction
	{
		// Token: 0x0600CA2F RID: 51759 RVA: 0x0049BE50 File Offset: 0x0049A050
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				XUiC_ChatOutput.AddMessage(LocalPlayerUI.GetUIForPlayer(entityPlayerLocal).xui, EnumGameMessages.PlainTextLocal, this.text, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
			}
		}

		// Token: 0x0600CA30 RID: 51760 RVA: 0x0049BE88 File Offset: 0x0049A088
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

		// Token: 0x0600CA31 RID: 51761 RVA: 0x0049BEFF File Offset: 0x0049A0FF
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

		// Token: 0x04009823 RID: 38947
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "";

		// Token: 0x04009824 RID: 38948
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x04009825 RID: 38949
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x04009826 RID: 38950
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";
	}
}
