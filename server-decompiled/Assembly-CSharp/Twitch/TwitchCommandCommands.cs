using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017ED RID: 6125
	public class TwitchCommandCommands : BaseTwitchCommand
	{
		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x0600BD0D RID: 48397 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x0600BD0E RID: 48398 RVA: 0x004618B9 File Offset: 0x0045FAB9
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#commands" };
			}
		}

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x0600BD0F RID: 48399 RVA: 0x004618C9 File Offset: 0x0045FAC9
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Commands", false, null) };
			}
		}

		// Token: 0x0600BD10 RID: 48400 RVA: 0x004618E0 File Offset: 0x0045FAE0
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayCommands(message.isBroadcaster, message.isMod, message.isVIP, message.isSub);
		}

		// Token: 0x0600BD11 RID: 48401 RVA: 0x00461904 File Offset: 0x0045FB04
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayCommands(true, true, true, true);
		}
	}
}
