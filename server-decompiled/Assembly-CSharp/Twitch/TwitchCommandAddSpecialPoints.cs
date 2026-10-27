using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200186D RID: 6253
	public class TwitchCommandAddSpecialPoints : BaseTwitchCommand
	{
		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x0600C0EE RID: 49390 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x0600C0EF RID: 49391 RVA: 0x0046F14C File Offset: 0x0046D34C
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addsp" };
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x0600C0F0 RID: 49392 RVA: 0x0046F15C File Offset: 0x0046D35C
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddSpecialPoints", false, null) };
			}
		}

		// Token: 0x0600C0F1 RID: 49393 RVA: 0x0046F174 File Offset: 0x0046D374
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

		// Token: 0x0600C0F2 RID: 49394 RVA: 0x0046F1E0 File Offset: 0x0046D3E0
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
