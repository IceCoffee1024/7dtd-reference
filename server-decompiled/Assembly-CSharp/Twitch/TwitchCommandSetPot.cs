using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001850 RID: 6224
	public class TwitchCommandSetPot : BaseTwitchCommand
	{
		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x0600C04F RID: 49231 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x0600C050 RID: 49232 RVA: 0x0046CC97 File Offset: 0x0046AE97
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setpot" };
			}
		}

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x0600C051 RID: 49233 RVA: 0x0046CCA7 File Offset: 0x0046AEA7
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetPot", false, null) };
			}
		}

		// Token: 0x0600C052 RID: 49234 RVA: 0x0046CCC0 File Offset: 0x0046AEC0
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (int.TryParse(array[1], out num))
				{
					TwitchManager.Current.SetPot(num);
				}
			}
		}

		// Token: 0x0600C053 RID: 49235 RVA: 0x0046CCFC File Offset: 0x0046AEFC
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (int.TryParse(arguments[1], out num))
				{
					TwitchManager.Current.SetPot(num);
				}
			}
		}
	}
}
