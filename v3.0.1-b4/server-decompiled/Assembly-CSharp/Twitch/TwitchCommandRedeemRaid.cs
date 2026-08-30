using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F8 RID: 6136
	public class TwitchCommandRedeemRaid : BaseTwitchCommand
	{
		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x0600BD4E RID: 48462 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x0600BD4F RID: 48463 RVA: 0x00462323 File Offset: 0x00460523
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_raid" };
			}
		}

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x0600BD50 RID: 48464 RVA: 0x00462333 File Offset: 0x00460533
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemRaid", false, null) };
			}
		}

		// Token: 0x0600BD51 RID: 48465 RVA: 0x0046234C File Offset: 0x0046054C
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

		// Token: 0x0600BD52 RID: 48466 RVA: 0x004623B0 File Offset: 0x004605B0
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
