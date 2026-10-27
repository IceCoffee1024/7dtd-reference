using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001876 RID: 6262
	public class TwitchCommandRedeemBits : BaseTwitchCommand
	{
		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x0600C121 RID: 49441 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x0600C122 RID: 49442 RVA: 0x0046FA27 File Offset: 0x0046DC27
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_bits" };
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x0600C123 RID: 49443 RVA: 0x0046FA37 File Offset: 0x0046DC37
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemBits", false, null) };
			}
		}

		// Token: 0x0600C124 RID: 49444 RVA: 0x0046FA50 File Offset: 0x0046DC50
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

		// Token: 0x0600C125 RID: 49445 RVA: 0x0046FADC File Offset: 0x0046DCDC
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
