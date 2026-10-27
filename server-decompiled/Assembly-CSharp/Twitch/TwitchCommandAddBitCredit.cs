using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200186B RID: 6251
	public class TwitchCommandAddBitCredit : BaseTwitchCommand
	{
		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x0600C0E2 RID: 49378 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x0600C0E3 RID: 49379 RVA: 0x0046EF92 File Offset: 0x0046D192
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addcredit" };
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x0600C0E4 RID: 49380 RVA: 0x0046EFA2 File Offset: 0x0046D1A2
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddBitCredit", false, null) };
			}
		}

		// Token: 0x0600C0E5 RID: 49381 RVA: 0x0046EFBC File Offset: 0x0046D1BC
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

		// Token: 0x0600C0E6 RID: 49382 RVA: 0x0046F004 File Offset: 0x0046D204
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
