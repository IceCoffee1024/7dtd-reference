using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200186C RID: 6252
	public class TwitchCommandAddPoints : BaseTwitchCommand
	{
		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x0600C0E8 RID: 49384 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x0600C0E9 RID: 49385 RVA: 0x0046F04D File Offset: 0x0046D24D
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addpp" };
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x0600C0EA RID: 49386 RVA: 0x0046F05D File Offset: 0x0046D25D
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddPoints", false, null) };
			}
		}

		// Token: 0x0600C0EB RID: 49387 RVA: 0x0046F074 File Offset: 0x0046D274
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 3)
			{
				int num = 0;
				if (int.TryParse(array[2], out num))
				{
					if (array[1].EqualsCaseInsensitive(BaseTwitchCommand.allText))
					{
						TwitchManager.Current.ViewerData.AddPoints("", num, false, true);
						return;
					}
					TwitchManager.Current.ViewerData.AddPoints(array[1], num, false, true);
				}
			}
		}

		// Token: 0x0600C0EC RID: 49388 RVA: 0x0046F0E0 File Offset: 0x0046D2E0
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 3)
			{
				int num = 0;
				if (int.TryParse(arguments[2], out num))
				{
					if (arguments[1].EqualsCaseInsensitive(BaseTwitchCommand.allText))
					{
						TwitchManager.Current.ViewerData.AddPoints("", num, false, true);
						return;
					}
					TwitchManager.Current.ViewerData.AddPoints(arguments[1], num, false, true);
				}
			}
		}
	}
}
