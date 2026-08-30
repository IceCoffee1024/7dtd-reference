using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001819 RID: 6169
	public class TwitchCommandRedeemBits : BaseTwitchCommand
	{
		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x0600BEB5 RID: 48821 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x0600BEB6 RID: 48822 RVA: 0x00467B3F File Offset: 0x00465D3F
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_bits" };
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x0600BEB7 RID: 48823 RVA: 0x00467B4F File Offset: 0x00465D4F
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemBits", false, null) };
			}
		}

		// Token: 0x0600BEB8 RID: 48824 RVA: 0x00467B68 File Offset: 0x00465D68
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

		// Token: 0x0600BEB9 RID: 48825 RVA: 0x00467BF4 File Offset: 0x00465DF4
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
