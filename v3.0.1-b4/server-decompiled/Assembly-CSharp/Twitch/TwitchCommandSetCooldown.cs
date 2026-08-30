using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017FD RID: 6141
	public class TwitchCommandSetCooldown : BaseTwitchCommand
	{
		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x0600BD6C RID: 48492 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x0600BD6D RID: 48493 RVA: 0x00462907 File Offset: 0x00460B07
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setcooldown" };
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x0600BD6E RID: 48494 RVA: 0x00462917 File Offset: 0x00460B17
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetCooldown", false, null) };
			}
		}

		// Token: 0x0600BD6F RID: 48495 RVA: 0x00462930 File Offset: 0x00460B30
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

		// Token: 0x0600BD70 RID: 48496 RVA: 0x0046297C File Offset: 0x00460B7C
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
