using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200183F RID: 6207
	public class TwitchCommandCommands : BaseTwitchCommand
	{
		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x0600BFEA RID: 49130 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x0600BFEB RID: 49131 RVA: 0x0046BB91 File Offset: 0x00469D91
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#commands" };
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x0600BFEC RID: 49132 RVA: 0x0046BBA1 File Offset: 0x00469DA1
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Commands", false, null) };
			}
		}

		// Token: 0x0600BFED RID: 49133 RVA: 0x0046BBB8 File Offset: 0x00469DB8
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayCommands(message.isBroadcaster, message.isMod, message.isVIP, message.isSub);
		}

		// Token: 0x0600BFEE RID: 49134 RVA: 0x0046BBDC File Offset: 0x00469DDC
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayCommands(true, true, true, true);
		}
	}
}
