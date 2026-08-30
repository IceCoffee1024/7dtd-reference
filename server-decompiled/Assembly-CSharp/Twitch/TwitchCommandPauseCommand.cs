using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001818 RID: 6168
	public class TwitchCommandPauseCommand : BaseTwitchCommand
	{
		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x0600BEAF RID: 48815 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x0600BEB0 RID: 48816 RVA: 0x00467AE2 File Offset: 0x00465CE2
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#pause" };
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x0600BEB1 RID: 48817 RVA: 0x00467AF2 File Offset: 0x00465CF2
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Pause", false, null) };
			}
		}

		// Token: 0x0600BEB2 RID: 48818 RVA: 0x00467B09 File Offset: 0x00465D09
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}

		// Token: 0x0600BEB3 RID: 48819 RVA: 0x00467B29 File Offset: 0x00465D29
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}
	}
}
