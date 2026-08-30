using System;
using System.Collections.Generic;
using System.Text;

namespace Twitch
{
	// Token: 0x02001814 RID: 6164
	public class TwitchCommandDebug : BaseTwitchCommand
	{
		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x0600BE98 RID: 48792 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x0600BE99 RID: 48793 RVA: 0x004676B0 File Offset: 0x004658B0
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#debug" };
			}
		}

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x0600BE9A RID: 48794 RVA: 0x004676C0 File Offset: 0x004658C0
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("#debug", false, null) };
			}
		}

		// Token: 0x0600BE9B RID: 48795 RVA: 0x004676D7 File Offset: 0x004658D7
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayDebug(message.Message);
		}

		// Token: 0x0600BE9C RID: 48796 RVA: 0x004676EC File Offset: 0x004658EC
		public override void ExecuteConsole(List<string> arguments)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < arguments.Count; i++)
			{
				stringBuilder.Append(arguments[i] + " ");
			}
			TwitchManager.Current.DisplayDebug(stringBuilder.ToString());
		}
	}
}
