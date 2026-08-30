using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017FE RID: 6142
	public class TwitchCommandSetPot : BaseTwitchCommand
	{
		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x0600BD72 RID: 48498 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x0600BD73 RID: 48499 RVA: 0x004629BF File Offset: 0x00460BBF
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setpot" };
			}
		}

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x0600BD74 RID: 48500 RVA: 0x004629CF File Offset: 0x00460BCF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetPot", false, null) };
			}
		}

		// Token: 0x0600BD75 RID: 48501 RVA: 0x004629E8 File Offset: 0x00460BE8
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

		// Token: 0x0600BD76 RID: 48502 RVA: 0x00462A24 File Offset: 0x00460C24
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
