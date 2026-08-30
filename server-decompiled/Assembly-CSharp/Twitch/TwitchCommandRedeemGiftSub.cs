using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200181C RID: 6172
	public class TwitchCommandRedeemGiftSub : BaseTwitchCommand
	{
		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x0600BEC7 RID: 48839 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x0600BEC8 RID: 48840 RVA: 0x00467E4B File Offset: 0x0046604B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_giftsub" };
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x0600BEC9 RID: 48841 RVA: 0x00467E5B File Offset: 0x0046605B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGiftSubs", false, null) };
			}
		}

		// Token: 0x0600BECA RID: 48842 RVA: 0x00467E74 File Offset: 0x00466074
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

		// Token: 0x0600BECB RID: 48843 RVA: 0x00467F48 File Offset: 0x00466148
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
