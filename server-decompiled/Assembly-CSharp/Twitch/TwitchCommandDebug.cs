using System;
using System.Collections.Generic;
using System.Text;

namespace Twitch
{
	// Token: 0x02001871 RID: 6257
	public class TwitchCommandDebug : BaseTwitchCommand
	{
		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x0600C104 RID: 49412 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x0600C105 RID: 49413 RVA: 0x0046F598 File Offset: 0x0046D798
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#debug" };
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x0600C106 RID: 49414 RVA: 0x0046F5A8 File Offset: 0x0046D7A8
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("#debug", false, null) };
			}
		}

		// Token: 0x0600C107 RID: 49415 RVA: 0x0046F5BF File Offset: 0x0046D7BF
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayDebug(message.Message);
		}

		// Token: 0x0600C108 RID: 49416 RVA: 0x0046F5D4 File Offset: 0x0046D7D4
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
