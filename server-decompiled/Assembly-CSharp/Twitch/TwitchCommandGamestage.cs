using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001817 RID: 6167
	public class TwitchCommandGamestage : BaseTwitchCommand
	{
		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x0600BEAA RID: 48810 RVA: 0x00467A98 File Offset: 0x00465C98
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#gamestage", "#gs" };
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x0600BEAB RID: 48811 RVA: 0x00467AB0 File Offset: 0x00465CB0
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

		// Token: 0x0600BEAC RID: 48812 RVA: 0x00467AD6 File Offset: 0x00465CD6
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayGameStage();
		}

		// Token: 0x0600BEAD RID: 48813 RVA: 0x00467AD6 File Offset: 0x00465CD6
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayGameStage();
		}
	}
}
