using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001813 RID: 6163
	public class TwitchCommandCommands : BaseTwitchCommand
	{
		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x0600BE92 RID: 48786 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x0600BE93 RID: 48787 RVA: 0x00467655 File Offset: 0x00465855
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#commands" };
			}
		}

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x0600BE94 RID: 48788 RVA: 0x00467665 File Offset: 0x00465865
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Commands", false, null) };
			}
		}

		// Token: 0x0600BE95 RID: 48789 RVA: 0x0046767C File Offset: 0x0046587C
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayCommands(message.isBroadcaster, message.isMod, message.isVIP, message.isSub);
		}

		// Token: 0x0600BE96 RID: 48790 RVA: 0x004676A0 File Offset: 0x004658A0
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayCommands(true, true, true, true);
		}
	}
}
