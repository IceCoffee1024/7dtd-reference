using System;
using System.Collections.Generic;
using System.Text;

namespace Twitch
{
	// Token: 0x02001840 RID: 6208
	public class TwitchCommandDebug : BaseTwitchCommand
	{
		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x0600BFF0 RID: 49136 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x0600BFF1 RID: 49137 RVA: 0x0046BBEC File Offset: 0x00469DEC
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#debug" };
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x0600BFF2 RID: 49138 RVA: 0x0046BBFC File Offset: 0x00469DFC
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("#debug", false, null) };
			}
		}

		// Token: 0x0600BFF3 RID: 49139 RVA: 0x0046BC13 File Offset: 0x00469E13
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayDebug(message.Message);
		}

		// Token: 0x0600BFF4 RID: 49140 RVA: 0x0046BC28 File Offset: 0x00469E28
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
