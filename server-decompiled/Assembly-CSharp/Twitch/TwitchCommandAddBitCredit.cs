using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200183A RID: 6202
	public class TwitchCommandAddBitCredit : BaseTwitchCommand
	{
		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x0600BFCE RID: 49102 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x0600BFCF RID: 49103 RVA: 0x0046B5E6 File Offset: 0x004697E6
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addcredit" };
			}
		}

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x0600BFD0 RID: 49104 RVA: 0x0046B5F6 File Offset: 0x004697F6
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddBitCredit", false, null) };
			}
		}

		// Token: 0x0600BFD1 RID: 49105 RVA: 0x0046B610 File Offset: 0x00469810
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

		// Token: 0x0600BFD2 RID: 49106 RVA: 0x0046B658 File Offset: 0x00469858
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
