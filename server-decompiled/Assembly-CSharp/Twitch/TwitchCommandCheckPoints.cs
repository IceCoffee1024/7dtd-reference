using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200183E RID: 6206
	public class TwitchCommandCheckPoints : BaseTwitchCommand
	{
		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x0600BFE5 RID: 49125 RVA: 0x0046BA0D File Offset: 0x00469C0D
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#checkpoints", "#cp" };
			}
		}

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x0600BFE6 RID: 49126 RVA: 0x0046BA25 File Offset: 0x00469C25
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

		// Token: 0x0600BFE7 RID: 49127 RVA: 0x0046BA4C File Offset: 0x00469C4C
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

		// Token: 0x0600BFE8 RID: 49128 RVA: 0x0046BAF4 File Offset: 0x00469CF4
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
