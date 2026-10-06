using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200183C RID: 6204
	public class TwitchCommandAddSpecialPoints : BaseTwitchCommand
	{
		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x0600BFDA RID: 49114 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x0600BFDB RID: 49115 RVA: 0x0046B7A0 File Offset: 0x004699A0
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#addsp" };
			}
		}

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x0600BFDC RID: 49116 RVA: 0x0046B7B0 File Offset: 0x004699B0
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_AddSpecialPoints", false, null) };
			}
		}

		// Token: 0x0600BFDD RID: 49117 RVA: 0x0046B7C8 File Offset: 0x004699C8
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

		// Token: 0x0600BFDE RID: 49118 RVA: 0x0046B834 File Offset: 0x00469A34
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
