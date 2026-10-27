using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001879 RID: 6265
	public class TwitchCommandRedeemGiftSub : BaseTwitchCommand
	{
		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x0600C133 RID: 49459 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x0600C134 RID: 49460 RVA: 0x0046FD33 File Offset: 0x0046DF33
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_giftsub" };
			}
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x0600C135 RID: 49461 RVA: 0x0046FD43 File Offset: 0x0046DF43
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGiftSubs", false, null) };
			}
		}

		// Token: 0x0600C136 RID: 49462 RVA: 0x0046FD5C File Offset: 0x0046DF5C
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

		// Token: 0x0600C137 RID: 49463 RVA: 0x0046FE30 File Offset: 0x0046E030
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
