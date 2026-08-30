using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F2 RID: 6130
	public class TwitchCommandPauseCommand : BaseTwitchCommand
	{
		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x0600BD2A RID: 48426 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x0600BD2B RID: 48427 RVA: 0x00461D46 File Offset: 0x0045FF46
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#pause" };
			}
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x0600BD2C RID: 48428 RVA: 0x00461D56 File Offset: 0x0045FF56
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Pause", false, null) };
			}
		}

		// Token: 0x0600BD2D RID: 48429 RVA: 0x00461D6D File Offset: 0x0045FF6D
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}

		// Token: 0x0600BD2E RID: 48430 RVA: 0x00461D8D File Offset: 0x0045FF8D
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}
	}
}
