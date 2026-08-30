using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017EA RID: 6122
	public class TwitchCommandAddSpecialPoints : BaseTwitchCommand
	{
		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x0600BCFD RID: 48381 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x0600BCFE RID: 48382 RVA: 0x004614C8 File Offset: 0x0045F6C8
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addsp" };
			}
		}

		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x0600BCFF RID: 48383 RVA: 0x004614D8 File Offset: 0x0045F6D8
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddSpecialPoints", false, null) };
			}
		}

		// Token: 0x0600BD00 RID: 48384 RVA: 0x004614F0 File Offset: 0x0045F6F0
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
						TwitchManager.Current.ViewerData.AddPoints("", num, true, true);
						return;
					}
					TwitchManager.Current.ViewerData.AddPoints(array[1], num, true, true);
				}
			}
		}

		// Token: 0x0600BD01 RID: 48385 RVA: 0x0046155C File Offset: 0x0045F75C
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 3)
			{
				int num = 0;
				if (int.TryParse(arguments[2], out num))
				{
					if (arguments[1].EqualsCaseInsensitive(BaseTwitchCommand.allText))
					{
						TwitchManager.Current.ViewerData.AddPoints("", num, true, true);
						return;
					}
					TwitchManager.Current.ViewerData.AddPoints(arguments[1], num, true, true);
				}
			}
		}
	}
}
