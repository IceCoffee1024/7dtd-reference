using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200184A RID: 6218
	public class TwitchCommandRedeemRaid : BaseTwitchCommand
	{
		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x0600C02B RID: 49195 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x0600C02C RID: 49196 RVA: 0x0046C5FB File Offset: 0x0046A7FB
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_raid" };
			}
		}

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x0600C02D RID: 49197 RVA: 0x0046C60B File Offset: 0x0046A80B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemRaid", false, null) };
			}
		}

		// Token: 0x0600C02E RID: 49198 RVA: 0x0046C624 File Offset: 0x0046A824
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

		// Token: 0x0600C02F RID: 49199 RVA: 0x0046C688 File Offset: 0x0046A888
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
