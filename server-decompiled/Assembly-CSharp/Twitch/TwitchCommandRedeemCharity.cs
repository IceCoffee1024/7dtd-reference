using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001846 RID: 6214
	public class TwitchCommandRedeemCharity : BaseTwitchCommand
	{
		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x0600C013 RID: 49171 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x0600C014 RID: 49172 RVA: 0x0046C1C7 File Offset: 0x0046A3C7
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_charity" };
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x0600C015 RID: 49173 RVA: 0x0046C1D7 File Offset: 0x0046A3D7
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemCharity", false, null) };
			}
		}

		// Token: 0x0600C016 RID: 49174 RVA: 0x0046C1F0 File Offset: 0x0046A3F0
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

		// Token: 0x0600C017 RID: 49175 RVA: 0x0046C27C File Offset: 0x0046A47C
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
