using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001984 RID: 6532
	[Preserve]
	public class ActionAddChatMessage : ActionBaseClientAction
	{
		// Token: 0x0600C91B RID: 51483 RVA: 0x0049845C File Offset: 0x0049665C
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				XUiC_ChatOutput.AddMessage(LocalPlayerUI.GetUIForPlayer(entityPlayerLocal).xui, EnumGameMessages.PlainTextLocal, this.text, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
			}
		}

		// Token: 0x0600C91C RID: 51484 RVA: 0x00498494 File Offset: 0x00496694
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

		// Token: 0x0600C91D RID: 51485 RVA: 0x0049850B File Offset: 0x0049670B
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

		// Token: 0x0400973A RID: 38714
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "";

		// Token: 0x0400973B RID: 38715
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x0400973C RID: 38716
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x0400973D RID: 38717
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";
	}
}
