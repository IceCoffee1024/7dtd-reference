using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200180E RID: 6158
	public class TwitchCommandAddBitCredit : BaseTwitchCommand
	{
		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x0600BE76 RID: 48758 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x0600BE77 RID: 48759 RVA: 0x004670AA File Offset: 0x004652AA
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addcredit" };
			}
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x0600BE78 RID: 48760 RVA: 0x004670BA File Offset: 0x004652BA
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddBitCredit", false, null) };
			}
		}

		// Token: 0x0600BE79 RID: 48761 RVA: 0x004670D4 File Offset: 0x004652D4
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 3)
			{
				int num = 0;
				if (int.TryParse(array[2], out num))
				{
					TwitchManager.Current.ViewerData.AddCredit(array[1], num, true);
				}
			}
		}

		// Token: 0x0600BE7A RID: 48762 RVA: 0x0046711C File Offset: 0x0046531C
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 3)
			{
				int num = 0;
				if (int.TryParse(arguments[2], out num))
				{
					TwitchManager.Current.ViewerData.AddCredit(arguments[1], num, true);
				}
			}
		}
	}
}
