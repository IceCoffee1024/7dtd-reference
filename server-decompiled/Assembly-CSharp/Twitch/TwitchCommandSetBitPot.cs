using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200187F RID: 6271
	public class TwitchCommandSetBitPot : BaseTwitchCommand
	{
		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x0600C157 RID: 49495 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x0600C158 RID: 49496 RVA: 0x004704F4 File Offset: 0x0046E6F4
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setbitpot" };
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x0600C159 RID: 49497 RVA: 0x00470504 File Offset: 0x0046E704
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetBitPot", false, null) };
			}
		}

		// Token: 0x0600C15A RID: 49498 RVA: 0x0047051C File Offset: 0x0046E71C
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (int.TryParse(array[1], out num))
				{
					TwitchManager.Current.SetBitPot(num);
				}
			}
		}

		// Token: 0x0600C15B RID: 49499 RVA: 0x00470558 File Offset: 0x0046E758
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (int.TryParse(arguments[1], out num))
				{
					TwitchManager.Current.SetBitPot(num);
				}
			}
		}
	}
}
