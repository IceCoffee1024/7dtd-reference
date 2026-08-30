using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001823 RID: 6179
	public class TwitchCommandSetCooldown : BaseTwitchCommand
	{
		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x0600BEF1 RID: 48881 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x0600BEF2 RID: 48882 RVA: 0x004686A3 File Offset: 0x004668A3
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setcooldown" };
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x0600BEF3 RID: 48883 RVA: 0x004686B3 File Offset: 0x004668B3
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetCooldown", false, null) };
			}
		}

		// Token: 0x0600BEF4 RID: 48884 RVA: 0x004686CC File Offset: 0x004668CC
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

		// Token: 0x0600BEF5 RID: 48885 RVA: 0x00468718 File Offset: 0x00466918
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
