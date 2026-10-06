using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200184E RID: 6222
	public class TwitchCommandSetBitPot : BaseTwitchCommand
	{
		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x0600C043 RID: 49219 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x0600C044 RID: 49220 RVA: 0x0046CB48 File Offset: 0x0046AD48
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setbitpot" };
			}
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x0600C045 RID: 49221 RVA: 0x0046CB58 File Offset: 0x0046AD58
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetBitPot", false, null) };
			}
		}

		// Token: 0x0600C046 RID: 49222 RVA: 0x0046CB70 File Offset: 0x0046AD70
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

		// Token: 0x0600C047 RID: 49223 RVA: 0x0046CBAC File Offset: 0x0046ADAC
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
