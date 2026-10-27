using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A5D RID: 6749
	[Preserve]
	public class ActionTwitchSendChannelMessage : ActionBaseClientAction
	{
		// Token: 0x0600CD31 RID: 52529 RVA: 0x004ACBC0 File Offset: 0x004AADC0
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

		// Token: 0x0600CD32 RID: 52530 RVA: 0x004ACC00 File Offset: 0x004AAE00
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

		// Token: 0x0600CD33 RID: 52531 RVA: 0x004ACC38 File Offset: 0x004AAE38
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

		// Token: 0x0600CD34 RID: 52532 RVA: 0x004ACCAF File Offset: 0x004AAEAF
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

		// Token: 0x04009BAE RID: 39854
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "";

		// Token: 0x04009BAF RID: 39855
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x04009BB0 RID: 39856
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x04009BB1 RID: 39857
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x04009BB2 RID: 39858
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
