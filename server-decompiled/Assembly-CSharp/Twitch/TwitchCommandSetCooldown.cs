using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001880 RID: 6272
	public class TwitchCommandSetCooldown : BaseTwitchCommand
	{
		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x0600C15D RID: 49501 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x0600C15E RID: 49502 RVA: 0x0047058B File Offset: 0x0046E78B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setcooldown" };
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x0600C15F RID: 49503 RVA: 0x0047059B File Offset: 0x0046E79B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetCooldown", false, null) };
			}
		}

		// Token: 0x0600C160 RID: 49504 RVA: 0x004705B4 File Offset: 0x0046E7B4
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

		// Token: 0x0600C161 RID: 49505 RVA: 0x00470600 File Offset: 0x0046E800
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
