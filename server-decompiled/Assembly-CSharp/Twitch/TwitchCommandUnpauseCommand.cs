using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001826 RID: 6182
	public class TwitchCommandUnpauseCommand : BaseTwitchCommand
	{
		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x0600BF03 RID: 48899 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x0600BF04 RID: 48900 RVA: 0x00468963 File Offset: 0x00466B63
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#unpause" };
			}
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x0600BF05 RID: 48901 RVA: 0x00468973 File Offset: 0x00466B73
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Unpause", false, null) };
			}
		}

		// Token: 0x0600BF06 RID: 48902 RVA: 0x0046898A File Offset: 0x00466B8A
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}

		// Token: 0x0600BF07 RID: 48903 RVA: 0x004689AA File Offset: 0x00466BAA
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(true);
			}
		}
	}
}
