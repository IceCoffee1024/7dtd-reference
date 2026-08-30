using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F4 RID: 6132
	public class TwitchCommandRedeemCharity : BaseTwitchCommand
	{
		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x0600BD36 RID: 48438 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x0600BD37 RID: 48439 RVA: 0x00461EEF File Offset: 0x004600EF
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_charity" };
			}
		}

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x0600BD38 RID: 48440 RVA: 0x00461EFF File Offset: 0x004600FF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemCharity", false, null) };
			}
		}

		// Token: 0x0600BD39 RID: 48441 RVA: 0x00461F18 File Offset: 0x00460118
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (StringParsers.TryParseSInt32(array[1], out num))
				{
					TwitchManager.Current.HandleCharityRedeem(message.UserName, num, null);
					return;
				}
			}
			else if (array.Length == 3)
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
				int num2 = 0;
				if (StringParsers.TryParseSInt32(array[2], out num2))
				{
					TwitchManager.Current.HandleCharityRedeem(text, num2, null);
				}
			}
		}

		// Token: 0x0600BD3A RID: 48442 RVA: 0x00461FA4 File Offset: 0x004601A4
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (StringParsers.TryParseSInt32(arguments[1], out num))
				{
					TwitchManager.Current.HandleCharityRedeem(TwitchManager.Current.Authentication.userName, num, null);
					return;
				}
			}
			else if (arguments.Count == 3)
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
				int num2 = 0;
				if (StringParsers.TryParseSInt32(arguments[2], out num2))
				{
					TwitchManager.Current.HandleCharityRedeem(text, num2, null);
				}
			}
		}
	}
}
