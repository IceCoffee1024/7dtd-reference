using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001810 RID: 6160
	public class TwitchCommandAddSpecialPoints : BaseTwitchCommand
	{
		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x0600BE82 RID: 48770 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x0600BE83 RID: 48771 RVA: 0x00467264 File Offset: 0x00465464
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addsp" };
			}
		}

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x0600BE84 RID: 48772 RVA: 0x00467274 File Offset: 0x00465474
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddSpecialPoints", false, null) };
			}
		}

		// Token: 0x0600BE85 RID: 48773 RVA: 0x0046728C File Offset: 0x0046548C
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

		// Token: 0x0600BE86 RID: 48774 RVA: 0x004672F8 File Offset: 0x004654F8
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
