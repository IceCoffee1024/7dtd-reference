using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A00 RID: 6656
	[Preserve]
	public class ActionTwitchSendChannelMessage : ActionBaseClientAction
	{
		// Token: 0x0600CAC5 RID: 51909 RVA: 0x004A4BF8 File Offset: 0x004A2DF8
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

		// Token: 0x0600CAC6 RID: 51910 RVA: 0x004A4C38 File Offset: 0x004A2E38
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

		// Token: 0x0600CAC7 RID: 51911 RVA: 0x004A4C70 File Offset: 0x004A2E70
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

		// Token: 0x0600CAC8 RID: 51912 RVA: 0x004A4CE7 File Offset: 0x004A2EE7
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

		// Token: 0x040099FF RID: 39423
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "";

		// Token: 0x04009A00 RID: 39424
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x04009A01 RID: 39425
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x04009A02 RID: 39426
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x04009A03 RID: 39427
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal player;
	}
}
