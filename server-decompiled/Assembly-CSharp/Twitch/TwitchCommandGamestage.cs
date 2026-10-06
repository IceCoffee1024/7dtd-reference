using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001843 RID: 6211
	public class TwitchCommandGamestage : BaseTwitchCommand
	{
		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x0600C002 RID: 49154 RVA: 0x0046BFD4 File Offset: 0x0046A1D4
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#gamestage", "#gs" };
			}
		}

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x0600C003 RID: 49155 RVA: 0x0046BFEC File Offset: 0x0046A1EC
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

		// Token: 0x0600C004 RID: 49156 RVA: 0x0046C012 File Offset: 0x0046A212
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayGameStage();
		}

		// Token: 0x0600C005 RID: 49157 RVA: 0x0046C012 File Offset: 0x0046A212
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayGameStage();
		}
	}
}
