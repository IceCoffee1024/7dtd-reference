using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200187B RID: 6267
	public class TwitchCommandRedeemRaid : BaseTwitchCommand
	{
		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x0600C13F RID: 49471 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x0600C140 RID: 49472 RVA: 0x0046FFA7 File Offset: 0x0046E1A7
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_raid" };
			}
		}

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x0600C141 RID: 49473 RVA: 0x0046FFB7 File Offset: 0x0046E1B7
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemRaid", false, null) };
			}
		}

		// Token: 0x0600C142 RID: 49474 RVA: 0x0046FFD0 File Offset: 0x0046E1D0
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

		// Token: 0x0600C143 RID: 49475 RVA: 0x00470034 File Offset: 0x0046E234
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
