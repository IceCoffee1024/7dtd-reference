using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F6 RID: 6134
	public class TwitchCommandRedeemGiftSub : BaseTwitchCommand
	{
		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x0600BD42 RID: 48450 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x0600BD43 RID: 48451 RVA: 0x004620AF File Offset: 0x004602AF
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_giftsub" };
			}
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x0600BD44 RID: 48452 RVA: 0x004620BF File Offset: 0x004602BF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGiftSubs", false, null) };
			}
		}

		// Token: 0x0600BD45 RID: 48453 RVA: 0x004620D8 File Offset: 0x004602D8
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
					TwitchManager.Current.HandleGiftSubEvent(text, num, TwitchSubEventEntry.SubTierTypes.Tier1);
					return;
				}
			}
			else if (array.Length == 4)
			{
				string text2 = array[1];
				if (text2.StartsWith("@"))
				{
					text2 = text2.Substring(1).ToLower();
				}
				else
				{
					text2 = text2.ToLower();
				}
				int num2 = 0;
				int num3 = 0;
				TwitchSubEventEntry.SubTierTypes subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier1;
				StringParsers.TryParseSInt32(array[2], out num2);
				StringParsers.TryParseSInt32(array[3], out num3);
				if (num3 != 2)
				{
					if (num3 == 3)
					{
						subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier3;
					}
				}
				else
				{
					subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier2;
				}
				TwitchManager.Current.HandleGiftSubEvent(text2, num2, subTierTypes);
			}
		}

		// Token: 0x0600BD46 RID: 48454 RVA: 0x004621AC File Offset: 0x004603AC
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
					TwitchManager.Current.HandleGiftSubEvent(text, num, TwitchSubEventEntry.SubTierTypes.Tier1);
					return;
				}
			}
			else if (arguments.Count == 4)
			{
				string text2 = arguments[1];
				if (text2.StartsWith("@"))
				{
					text2 = text2.Substring(1).ToLower();
				}
				else
				{
					text2 = text2.ToLower();
				}
				int num2 = 0;
				int num3 = 0;
				TwitchSubEventEntry.SubTierTypes subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier1;
				StringParsers.TryParseSInt32(arguments[2], out num2);
				StringParsers.TryParseSInt32(arguments[3], out num3);
				if (num3 != 2)
				{
					if (num3 == 3)
					{
						subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier3;
					}
				}
				else
				{
					subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier2;
				}
				TwitchManager.Current.HandleGiftSubEvent(text2, num2, subTierTypes);
			}
		}
	}
}
