using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200180F RID: 6159
	public class TwitchCommandAddPoints : BaseTwitchCommand
	{
		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x0600BE7C RID: 48764 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x0600BE7D RID: 48765 RVA: 0x00467165 File Offset: 0x00465365
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addpp" };
			}
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x0600BE7E RID: 48766 RVA: 0x00467175 File Offset: 0x00465375
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddPoints", false, null) };
			}
		}

		// Token: 0x0600BE7F RID: 48767 RVA: 0x0046718C File Offset: 0x0046538C
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

		// Token: 0x0600BE80 RID: 48768 RVA: 0x004671F8 File Offset: 0x004653F8
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
