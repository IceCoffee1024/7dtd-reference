using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200181A RID: 6170
	public class TwitchCommandRedeemCharity : BaseTwitchCommand
	{
		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x0600BEBB RID: 48827 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x0600BEBC RID: 48828 RVA: 0x00467C8B File Offset: 0x00465E8B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_charity" };
			}
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x0600BEBD RID: 48829 RVA: 0x00467C9B File Offset: 0x00465E9B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemCharity", false, null) };
			}
		}

		// Token: 0x0600BEBE RID: 48830 RVA: 0x00467CB4 File Offset: 0x00465EB4
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

		// Token: 0x0600BEBF RID: 48831 RVA: 0x00467D40 File Offset: 0x00465F40
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
