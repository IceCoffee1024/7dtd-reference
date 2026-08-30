using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017EC RID: 6124
	public class TwitchCommandCheckPoints : BaseTwitchCommand
	{
		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x0600BD08 RID: 48392 RVA: 0x00461735 File Offset: 0x0045F935
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#checkpoints", "#cp" };
			}
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x0600BD09 RID: 48393 RVA: 0x0046174D File Offset: 0x0045F94D
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

		// Token: 0x0600BD0A RID: 48394 RVA: 0x00461774 File Offset: 0x0045F974
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

		// Token: 0x0600BD0B RID: 48395 RVA: 0x0046181C File Offset: 0x0045FA1C
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
