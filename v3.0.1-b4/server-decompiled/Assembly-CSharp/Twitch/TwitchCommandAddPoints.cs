using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017E9 RID: 6121
	public class TwitchCommandAddPoints : BaseTwitchCommand
	{
		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x0600BCF7 RID: 48375 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x0600BCF8 RID: 48376 RVA: 0x004613C9 File Offset: 0x0045F5C9
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addpp" };
			}
		}

		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x0600BCF9 RID: 48377 RVA: 0x004613D9 File Offset: 0x0045F5D9
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddPoints", false, null) };
			}
		}

		// Token: 0x0600BCFA RID: 48378 RVA: 0x004613F0 File Offset: 0x0045F5F0
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

		// Token: 0x0600BCFB RID: 48379 RVA: 0x0046145C File Offset: 0x0045F65C
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
