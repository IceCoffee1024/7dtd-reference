using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F1 RID: 6129
	public class TwitchCommandGamestage : BaseTwitchCommand
	{
		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x0600BD25 RID: 48421 RVA: 0x00461CFC File Offset: 0x0045FEFC
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#gamestage", "#gs" };
			}
		}

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x0600BD26 RID: 48422 RVA: 0x00461D14 File Offset: 0x0045FF14
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[]
				{
					Localization.Get("TwitchCommand_Gamestage1", false, null),
					Localization.Get("TwitchCommand_Gamestage1", false, null)
				};
			}
		}

		// Token: 0x0600BD27 RID: 48423 RVA: 0x00461D3A File Offset: 0x0045FF3A
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayGameStage();
		}

		// Token: 0x0600BD28 RID: 48424 RVA: 0x00461D3A File Offset: 0x0045FF3A
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayGameStage();
		}
	}
}
