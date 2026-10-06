using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001848 RID: 6216
	public class TwitchCommandRedeemGiftSub : BaseTwitchCommand
	{
		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x0600C01F RID: 49183 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x0600C020 RID: 49184 RVA: 0x0046C387 File Offset: 0x0046A587
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_giftsub" };
			}
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x0600C021 RID: 49185 RVA: 0x0046C397 File Offset: 0x0046A597
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGiftSubs", false, null) };
			}
		}

		// Token: 0x0600C022 RID: 49186 RVA: 0x0046C3B0 File Offset: 0x0046A5B0
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

		// Token: 0x0600C023 RID: 49187 RVA: 0x0046C484 File Offset: 0x0046A684
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
