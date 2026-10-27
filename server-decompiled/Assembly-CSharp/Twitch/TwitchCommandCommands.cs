using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001870 RID: 6256
	public class TwitchCommandCommands : BaseTwitchCommand
	{
		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x0600C0FE RID: 49406 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x0600C0FF RID: 49407 RVA: 0x0046F53D File Offset: 0x0046D73D
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#commands" };
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x0600C100 RID: 49408 RVA: 0x0046F54D File Offset: 0x0046D74D
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Commands", false, null) };
			}
		}

		// Token: 0x0600C101 RID: 49409 RVA: 0x0046F564 File Offset: 0x0046D764
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayCommands(message.isBroadcaster, message.isMod, message.isVIP, message.isSub);
		}

		// Token: 0x0600C102 RID: 49410 RVA: 0x0046F588 File Offset: 0x0046D788
		public override void ExecuteConsole(List<string> arguments)
		{
			TwitchManager.Current.DisplayCommands(true, true, true, true);
		}
	}
}
