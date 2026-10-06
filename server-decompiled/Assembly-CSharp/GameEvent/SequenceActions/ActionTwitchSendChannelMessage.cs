using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2C RID: 6700
	[Preserve]
	public class ActionTwitchSendChannelMessage : ActionBaseClientAction
	{
		// Token: 0x0600CC1D RID: 52253 RVA: 0x004A90C4 File Offset: 0x004A72C4
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

		// Token: 0x0600CC1E RID: 52254 RVA: 0x004A9104 File Offset: 0x004A7304
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

		// Token: 0x0600CC1F RID: 52255 RVA: 0x004A913C File Offset: 0x004A733C
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

		// Token: 0x0600CC20 RID: 52256 RVA: 0x004A91B3 File Offset: 0x004A73B3
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

		// Token: 0x04009AC5 RID: 39621
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "";

		// Token: 0x04009AC6 RID: 39622
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x04009AC7 RID: 39623
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x04009AC8 RID: 39624
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x04009AC9 RID: 39625
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
