using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001812 RID: 6162
	public class TwitchCommandCheckPoints : BaseTwitchCommand
	{
		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x0600BE8D RID: 48781 RVA: 0x004674D1 File Offset: 0x004656D1
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#checkpoints", "#cp" };
			}
		}

		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x0600BE8E RID: 48782 RVA: 0x004674E9 File Offset: 0x004656E9
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

		// Token: 0x0600BE8F RID: 48783 RVA: 0x00467510 File Offset: 0x00465710
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

		// Token: 0x0600BE90 RID: 48784 RVA: 0x004675B8 File Offset: 0x004657B8
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
