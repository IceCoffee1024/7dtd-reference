using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200184F RID: 6223
	public class TwitchCommandSetCooldown : BaseTwitchCommand
	{
		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x0600C049 RID: 49225 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x0600C04A RID: 49226 RVA: 0x0046CBDF File Offset: 0x0046ADDF
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setcooldown" };
			}
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x0600C04B RID: 49227 RVA: 0x0046CBEF File Offset: 0x0046ADEF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetCooldown", false, null) };
			}
		}

		// Token: 0x0600C04C RID: 49228 RVA: 0x0046CC08 File Offset: 0x0046AE08
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (int.TryParse(array[1], out num))
				{
					if (num < 0)
					{
						num = 0;
					}
					TwitchManager.Current.SetCooldown((float)num + 0.5f, TwitchManager.CooldownTypes.Time, false, true);
				}
			}
		}

		// Token: 0x0600C04D RID: 49229 RVA: 0x0046CC54 File Offset: 0x0046AE54
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (int.TryParse(arguments[1], out num))
				{
					if (num < 0)
					{
						num = 0;
					}
					TwitchManager.Current.SetCooldown((float)num + 0.5f, TwitchManager.CooldownTypes.Time, false, true);
				}
			}
		}
	}
}
