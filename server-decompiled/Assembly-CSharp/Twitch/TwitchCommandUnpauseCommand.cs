using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001800 RID: 6144
	public class TwitchCommandUnpauseCommand : BaseTwitchCommand
	{
		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x0600BD7E RID: 48510 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x0600BD7F RID: 48511 RVA: 0x00462BC7 File Offset: 0x00460DC7
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#unpause" };
			}
		}

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x0600BD80 RID: 48512 RVA: 0x00462BD7 File Offset: 0x00460DD7
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Unpause", false, null) };
			}
		}

		// Token: 0x0600BD81 RID: 48513 RVA: 0x00462BEE File Offset: 0x00460DEE
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}

		// Token: 0x0600BD82 RID: 48514 RVA: 0x00462C0E File Offset: 0x00460E0E
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}
	}
}
