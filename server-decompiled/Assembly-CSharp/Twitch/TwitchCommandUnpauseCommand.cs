using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001883 RID: 6275
	public class TwitchCommandUnpauseCommand : BaseTwitchCommand
	{
		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x0600C16F RID: 49519 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x0600C170 RID: 49520 RVA: 0x0047084B File Offset: 0x0046EA4B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#unpause" };
			}
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x0600C171 RID: 49521 RVA: 0x0047085B File Offset: 0x0046EA5B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Unpause", false, null) };
			}
		}

		// Token: 0x0600C172 RID: 49522 RVA: 0x00470872 File Offset: 0x0046EA72
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}

		// Token: 0x0600C173 RID: 49523 RVA: 0x00470892 File Offset: 0x0046EA92
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}
	}
}
