using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001877 RID: 6263
	public class TwitchCommandRedeemCharity : BaseTwitchCommand
	{
		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x0600C127 RID: 49447 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x0600C128 RID: 49448 RVA: 0x0046FB73 File Offset: 0x0046DD73
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_charity" };
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x0600C129 RID: 49449 RVA: 0x0046FB83 File Offset: 0x0046DD83
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemCharity", false, null) };
			}
		}

		// Token: 0x0600C12A RID: 49450 RVA: 0x0046FB9C File Offset: 0x0046DD9C
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

		// Token: 0x0600C12B RID: 49451 RVA: 0x0046FC28 File Offset: 0x0046DE28
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
