using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001881 RID: 6273
	public class TwitchCommandSetPot : BaseTwitchCommand
	{
		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x0600C163 RID: 49507 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x0600C164 RID: 49508 RVA: 0x00470643 File Offset: 0x0046E843
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setpot" };
			}
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x0600C165 RID: 49509 RVA: 0x00470653 File Offset: 0x0046E853
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetPot", false, null) };
			}
		}

		// Token: 0x0600C166 RID: 49510 RVA: 0x0047066C File Offset: 0x0046E86C
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

		// Token: 0x0600C167 RID: 49511 RVA: 0x004706A8 File Offset: 0x0046E8A8
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
