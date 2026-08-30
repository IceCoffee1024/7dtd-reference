using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F3 RID: 6131
	public class TwitchCommandRedeemBits : BaseTwitchCommand
	{
		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x0600BD30 RID: 48432 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x0600BD31 RID: 48433 RVA: 0x00461DA3 File Offset: 0x0045FFA3
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_bits" };
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x0600BD32 RID: 48434 RVA: 0x00461DB3 File Offset: 0x0045FFB3
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemBits", false, null) };
			}
		}

		// Token: 0x0600BD33 RID: 48435 RVA: 0x00461DCC File Offset: 0x0045FFCC
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (StringParsers.TryParseSInt32(array[1], out num))
				{
					TwitchManager.Current.HandleBitRedeem(message.UserName, num, null);
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
					TwitchManager.Current.HandleBitRedeem(text, num2, null);
				}
			}
		}

		// Token: 0x0600BD34 RID: 48436 RVA: 0x00461E58 File Offset: 0x00460058
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (StringParsers.TryParseSInt32(arguments[1], out num))
				{
					TwitchManager.Current.HandleBitRedeem(TwitchManager.Current.Authentication.userName, num, null);
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
					TwitchManager.Current.HandleBitRedeem(text, num2, null);
				}
			}
		}
	}
}
