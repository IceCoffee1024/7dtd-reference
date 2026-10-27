using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200186F RID: 6255
	public class TwitchCommandCheckPoints : BaseTwitchCommand
	{
		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x0600C0F9 RID: 49401 RVA: 0x0046F3B9 File Offset: 0x0046D5B9
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#checkpoints", "#cp" };
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x0600C0FA RID: 49402 RVA: 0x0046F3D1 File Offset: 0x0046D5D1
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[]
				{
					Localization.Get("TwitchCommand_CheckPoints1", false, null),
					Localization.Get("TwitchCommand_CheckPoints2", false, null)
				};
			}
		}

		// Token: 0x0600C0FB RID: 49403 RVA: 0x0046F3F8 File Offset: 0x0046D5F8
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				if (message.isMod || message.isBroadcaster)
				{
					string text = array[1];
					if (text.StartsWith("@"))
					{
						text = text.Substring(1).ToLower();
					}
					else
					{
						text = text.ToLower();
					}
					TwitchManager twitchManager = TwitchManager.Current;
					if (twitchManager.ViewerData.HasViewerEntry(text))
					{
						twitchManager.SendChannelPointOutputMessage(text);
						return;
					}
					twitchManager.ircClient.SendChannelMessage(string.Format("[7DTD]: No viewer data for {0}.", array[1]), true);
					return;
				}
			}
			else if (array.Length == 1)
			{
				TwitchManager.Current.SendChannelPointOutputMessage(message.UserName);
			}
		}

		// Token: 0x0600C0FC RID: 49404 RVA: 0x0046F4A0 File Offset: 0x0046D6A0
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count != 2)
			{
				if (arguments.Count == 1)
				{
					TwitchManager.Current.SendChannelPointOutputMessage(TwitchManager.Current.Authentication.userName);
				}
				return;
			}
			string text = arguments[1];
			if (text.StartsWith("@"))
			{
				text = text.Substring(1).ToLower();
			}
			else
			{
				text = text.ToLower();
			}
			TwitchManager twitchManager = TwitchManager.Current;
			if (twitchManager.ViewerData.HasViewerEntry(text))
			{
				twitchManager.SendChannelPointOutputMessage(text);
				return;
			}
			twitchManager.ircClient.SendChannelMessage(string.Format("[7DTD]: No viewer data for {0}.", arguments[1]), true);
		}
	}
}
