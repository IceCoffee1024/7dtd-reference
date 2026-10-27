using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200186E RID: 6254
	public class TwitchCommandCheckCredit : BaseTwitchCommand
	{
		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x0600C0F4 RID: 49396 RVA: 0x0046F24C File Offset: 0x0046D44C
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#checkcredit" };
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x0600C0F5 RID: 49397 RVA: 0x0046F25C File Offset: 0x0046D45C
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_CheckCredit", false, null) };
			}
		}

		// Token: 0x0600C0F6 RID: 49398 RVA: 0x0046F274 File Offset: 0x0046D474
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

		// Token: 0x0600C0F7 RID: 49399 RVA: 0x0046F31C File Offset: 0x0046D51C
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
