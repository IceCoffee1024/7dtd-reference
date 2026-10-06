using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001845 RID: 6213
	public class TwitchCommandRedeemBits : BaseTwitchCommand
	{
		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x0600C00D RID: 49165 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x0600C00E RID: 49166 RVA: 0x0046C07B File Offset: 0x0046A27B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_bits" };
			}
		}

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x0600C00F RID: 49167 RVA: 0x0046C08B File Offset: 0x0046A28B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemBits", false, null) };
			}
		}

		// Token: 0x0600C010 RID: 49168 RVA: 0x0046C0A4 File Offset: 0x0046A2A4
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

		// Token: 0x0600C011 RID: 49169 RVA: 0x0046C130 File Offset: 0x0046A330
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
