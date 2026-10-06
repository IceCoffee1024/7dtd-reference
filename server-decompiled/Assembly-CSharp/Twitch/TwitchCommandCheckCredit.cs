using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200183D RID: 6205
	public class TwitchCommandCheckCredit : BaseTwitchCommand
	{
		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600BFE0 RID: 49120 RVA: 0x0046B8A0 File Offset: 0x00469AA0
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#checkcredit" };
			}
		}

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x0600BFE1 RID: 49121 RVA: 0x0046B8B0 File Offset: 0x00469AB0
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_CheckCredit", false, null) };
			}
		}

		// Token: 0x0600BFE2 RID: 49122 RVA: 0x0046B8C8 File Offset: 0x00469AC8
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
						twitchManager.SendChannelCreditOutputMessage(text);
						return;
					}
					twitchManager.ircClient.SendChannelMessage(string.Format("[7DTD]: No viewer data for {0}.", array[1]), true);
					return;
				}
			}
			else if (array.Length == 1)
			{
				TwitchManager.Current.SendChannelCreditOutputMessage(message.UserName);
			}
		}

		// Token: 0x0600BFE3 RID: 49123 RVA: 0x0046B970 File Offset: 0x00469B70
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
