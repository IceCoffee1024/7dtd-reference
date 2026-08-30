using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017EB RID: 6123
	public class TwitchCommandCheckCredit : BaseTwitchCommand
	{
		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x0600BD03 RID: 48387 RVA: 0x004615C8 File Offset: 0x0045F7C8
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#checkcredit" };
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x0600BD04 RID: 48388 RVA: 0x004615D8 File Offset: 0x0045F7D8
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_CheckCredit", false, null) };
			}
		}

		// Token: 0x0600BD05 RID: 48389 RVA: 0x004615F0 File Offset: 0x0045F7F0
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

		// Token: 0x0600BD06 RID: 48390 RVA: 0x00461698 File Offset: 0x0045F898
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
