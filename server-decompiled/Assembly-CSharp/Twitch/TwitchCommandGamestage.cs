using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001874 RID: 6260
	public class TwitchCommandGamestage : BaseTwitchCommand
	{
		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x0600C116 RID: 49430 RVA: 0x0046F980 File Offset: 0x0046DB80
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#gamestage", "#gs" };
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x0600C117 RID: 49431 RVA: 0x0046F998 File Offset: 0x0046DB98
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

		// Token: 0x0600C118 RID: 49432 RVA: 0x0046F9BE File Offset: 0x0046DBBE
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayGameStage();
		}

		// Token: 0x0600C119 RID: 49433 RVA: 0x0046F9BE File Offset: 0x0046DBBE
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayGameStage();
		}
	}
}
