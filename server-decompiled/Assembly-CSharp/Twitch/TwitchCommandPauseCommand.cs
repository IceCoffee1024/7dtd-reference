using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001875 RID: 6261
	public class TwitchCommandPauseCommand : BaseTwitchCommand
	{
		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x0600C11B RID: 49435 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x0600C11C RID: 49436 RVA: 0x0046F9CA File Offset: 0x0046DBCA
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#pause" };
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x0600C11D RID: 49437 RVA: 0x0046F9DA File Offset: 0x0046DBDA
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Pause", false, null) };
			}
		}

		// Token: 0x0600C11E RID: 49438 RVA: 0x0046F9F1 File Offset: 0x0046DBF1
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}

		// Token: 0x0600C11F RID: 49439 RVA: 0x0046FA11 File Offset: 0x0046DC11
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}
	}
}
