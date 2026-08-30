using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017FC RID: 6140
	public class TwitchCommandSetBitPot : BaseTwitchCommand
	{
		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x0600BD66 RID: 48486 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x0600BD67 RID: 48487 RVA: 0x00462870 File Offset: 0x00460A70
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setbitpot" };
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x0600BD68 RID: 48488 RVA: 0x00462880 File Offset: 0x00460A80
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetBitPot", false, null) };
			}
		}

		// Token: 0x0600BD69 RID: 48489 RVA: 0x00462898 File Offset: 0x00460A98
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

		// Token: 0x0600BD6A RID: 48490 RVA: 0x004628D4 File Offset: 0x00460AD4
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
