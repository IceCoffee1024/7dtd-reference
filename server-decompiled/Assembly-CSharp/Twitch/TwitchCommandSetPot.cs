using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001824 RID: 6180
	public class TwitchCommandSetPot : BaseTwitchCommand
	{
		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x0600BEF7 RID: 48887 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x0600BEF8 RID: 48888 RVA: 0x0046875B File Offset: 0x0046695B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setpot" };
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x0600BEF9 RID: 48889 RVA: 0x0046876B File Offset: 0x0046696B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetPot", false, null) };
			}
		}

		// Token: 0x0600BEFA RID: 48890 RVA: 0x00468784 File Offset: 0x00466984
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (int.TryParse(array[1], out num))
				{
					TwitchManager.Current.SetPot(num);
				}
			}
		}

		// Token: 0x0600BEFB RID: 48891 RVA: 0x004687C0 File Offset: 0x004669C0
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (int.TryParse(arguments[1], out num))
				{
					TwitchManager.Current.SetPot(num);
				}
			}
		}
	}
}
