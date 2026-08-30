using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200181E RID: 6174
	public class TwitchCommandRedeemRaid : BaseTwitchCommand
	{
		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x0600BED3 RID: 48851 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x0600BED4 RID: 48852 RVA: 0x004680BF File Offset: 0x004662BF
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_raid" };
			}
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x0600BED5 RID: 48853 RVA: 0x004680CF File Offset: 0x004662CF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemRaid", false, null) };
			}
		}

		// Token: 0x0600BED6 RID: 48854 RVA: 0x004680E8 File Offset: 0x004662E8
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 3)
			{
				string text = array[1];
				if (text.StartsWith("@"))
				{
					text = text.Substring(1).ToLower();
				}
				else
				{
					text = text.ToLower();
				}
				int num = 0;
				if (StringParsers.TryParseSInt32(array[2], out num))
				{
					TwitchManager.Current.HandleRaidRedeem(text, num, null);
				}
			}
		}

		// Token: 0x0600BED7 RID: 48855 RVA: 0x0046814C File Offset: 0x0046634C
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 3)
			{
				string text = arguments[1];
				if (text.StartsWith("@"))
				{
					text = text.Substring(1).ToLower();
				}
				else
				{
					text = text.ToLower();
				}
				int num = 0;
				if (StringParsers.TryParseSInt32(arguments[2], out num))
				{
					TwitchManager.Current.HandleRaidRedeem(text, num, null);
				}
			}
		}
	}
}
