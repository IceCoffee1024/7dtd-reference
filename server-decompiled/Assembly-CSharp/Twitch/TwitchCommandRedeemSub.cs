using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200181F RID: 6175
	public class TwitchCommandRedeemSub : BaseTwitchCommand
	{
		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x0600BED9 RID: 48857 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x0600BEDA RID: 48858 RVA: 0x004681AC File Offset: 0x004663AC
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_sub" };
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x0600BEDB RID: 48859 RVA: 0x004681BC File Offset: 0x004663BC
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemSub", false, null) };
			}
		}

		// Token: 0x0600BEDC RID: 48860 RVA: 0x004681D4 File Offset: 0x004663D4
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
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
				TwitchManager.Current.HandleSubEvent(text, 1, TwitchSubEventEntry.SubTierTypes.Tier1);
				return;
			}
			if (array.Length == 3)
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
				int num = 0;
				if (StringParsers.TryParseSInt32(array[2], out num))
				{
					TwitchManager.Current.HandleSubEvent(text2, num, TwitchSubEventEntry.SubTierTypes.Tier1);
					return;
				}
			}
			else if (array.Length == 4)
			{
				string text3 = array[1];
				if (text3.StartsWith("@"))
				{
					text3 = text3.Substring(1).ToLower();
				}
				else
				{
					text3 = text3.ToLower();
				}
				int num2 = 0;
				int num3 = 0;
				TwitchSubEventEntry.SubTierTypes subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier1;
				StringParsers.TryParseSInt32(array[2], out num2);
				if (array[3].Trim().ToLower() == "prime")
				{
					subTierTypes = TwitchSubEventEntry.SubTierTypes.Prime;
				}
				else
				{
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
				}
				TwitchManager.Current.HandleSubEvent(text3, num2, subTierTypes);
			}
		}

		// Token: 0x0600BEDD RID: 48861 RVA: 0x00468310 File Offset: 0x00466510
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
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
				TwitchManager.Current.HandleSubEvent(text, 1, TwitchSubEventEntry.SubTierTypes.Tier1);
				return;
			}
			if (arguments.Count == 3)
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
				int num = 0;
				if (StringParsers.TryParseSInt32(arguments[2], out num))
				{
					TwitchManager.Current.HandleSubEvent(text2, num, TwitchSubEventEntry.SubTierTypes.Tier1);
					return;
				}
			}
			else if (arguments.Count == 4)
			{
				string text3 = arguments[1];
				if (text3.StartsWith("@"))
				{
					text3 = text3.Substring(1).ToLower();
				}
				else
				{
					text3 = text3.ToLower();
				}
				int num2 = 0;
				int num3 = 0;
				TwitchSubEventEntry.SubTierTypes subTierTypes = TwitchSubEventEntry.SubTierTypes.Tier1;
				StringParsers.TryParseSInt32(arguments[2], out num2);
				if (arguments[3].Trim().ToLower() == "prime")
				{
					subTierTypes = TwitchSubEventEntry.SubTierTypes.Prime;
				}
				else
				{
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
				}
				TwitchManager.Current.HandleSubEvent(text3, num2, subTierTypes);
			}
		}
	}
}
