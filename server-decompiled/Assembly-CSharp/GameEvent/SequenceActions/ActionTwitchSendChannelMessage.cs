using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D9 RID: 6617
	[Preserve]
	public class ActionTwitchSendChannelMessage : ActionBaseClientAction
	{
		// Token: 0x0600C93C RID: 51516 RVA: 0x0049E804 File Offset: 0x0049CA04
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				TwitchManager twitchManager = TwitchManager.Current;
				this.player = entityPlayerLocal;
				if (!twitchManager.TwitchActive)
				{
					return;
				}
				twitchManager.SendChannelMessage(base.GetTextWithElements(this.text), true);
			}
		}

		// Token: 0x0600C93D RID: 51517 RVA: 0x0049E844 File Offset: 0x0049CA44
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string ParseTextElement(string element)
		{
			if (element == "viewer")
			{
				return base.Owner.ExtraData;
			}
			if (!(element == "target"))
			{
				return element;
			}
			return this.player.EntityName;
		}

		// Token: 0x0600C93E RID: 51518 RVA: 0x0049E87C File Offset: 0x0049CA7C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionTwitchSendChannelMessage.PropText))
			{
				this.text = properties.Values[ActionTwitchSendChannelMessage.PropText];
			}
			if (properties.Values.ContainsKey(ActionTwitchSendChannelMessage.PropTextKey))
			{
				this.textKey = properties.Values[ActionTwitchSendChannelMessage.PropTextKey];
				this.text = Localization.Get(this.textKey, false, null);
			}
		}

		// Token: 0x0600C93F RID: 51519 RVA: 0x0049E8F3 File Offset: 0x0049CAF3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchSendChannelMessage
			{
				targetGroup = this.targetGroup,
				textKey = this.textKey,
				text = this.text
			};
		}

		// Token: 0x040098CE RID: 39118
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "";

		// Token: 0x040098CF RID: 39119
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x040098D0 RID: 39120
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x040098D1 RID: 39121
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x040098D2 RID: 39122
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
