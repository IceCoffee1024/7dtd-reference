using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001852 RID: 6226
	public class TwitchCommandUnpauseCommand : BaseTwitchCommand
	{
		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x0600C05B RID: 49243 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x0600C05C RID: 49244 RVA: 0x0046CE9F File Offset: 0x0046B09F
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#unpause" };
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x0600C05D RID: 49245 RVA: 0x0046CEAF File Offset: 0x0046B0AF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Unpause", false, null) };
			}
		}

		// Token: 0x0600C05E RID: 49246 RVA: 0x0046CEC6 File Offset: 0x0046B0C6
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}

		// Token: 0x0600C05F RID: 49247 RVA: 0x0046CEE6 File Offset: 0x0046B0E6
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}
	}
}
