using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017E8 RID: 6120
	public class TwitchCommandAddBitCredit : BaseTwitchCommand
	{
		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x0600BCF1 RID: 48369 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x0600BCF2 RID: 48370 RVA: 0x0046130E File Offset: 0x0045F50E
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addcredit" };
			}
		}

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x0600BCF3 RID: 48371 RVA: 0x0046131E File Offset: 0x0045F51E
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddBitCredit", false, null) };
			}
		}

		// Token: 0x0600BCF4 RID: 48372 RVA: 0x00461338 File Offset: 0x0045F538
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

		// Token: 0x0600BCF5 RID: 48373 RVA: 0x00461380 File Offset: 0x0045F580
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
