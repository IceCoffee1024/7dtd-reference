using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200183B RID: 6203
	public class TwitchCommandAddPoints : BaseTwitchCommand
	{
		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x0600BFD4 RID: 49108 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x0600BFD5 RID: 49109 RVA: 0x0046B6A1 File Offset: 0x004698A1
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addpp" };
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x0600BFD6 RID: 49110 RVA: 0x0046B6B1 File Offset: 0x004698B1
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddPoints", false, null) };
			}
		}

		// Token: 0x0600BFD7 RID: 49111 RVA: 0x0046B6C8 File Offset: 0x004698C8
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
						TwitchManager.Current.ViewerData.AddPoints("", num, false, true);
						return;
					}
					TwitchManager.Current.ViewerData.AddPoints(array[1], num, false, true);
				}
			}
		}

		// Token: 0x0600BFD8 RID: 49112 RVA: 0x0046B734 File Offset: 0x00469934
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 3)
			{
				int num = 0;
				if (int.TryParse(arguments[2], out num))
				{
					if (arguments[1].EqualsCaseInsensitive(BaseTwitchCommand.allText))
					{
						TwitchManager.Current.ViewerData.AddPoints("", num, false, true);
						return;
					}
					TwitchManager.Current.ViewerData.AddPoints(arguments[1], num, false, true);
				}
			}
		}
	}
}
