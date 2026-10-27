using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200187C RID: 6268
	public class TwitchCommandRedeemSub : BaseTwitchCommand
	{
		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x0600C145 RID: 49477 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x0600C146 RID: 49478 RVA: 0x00470094 File Offset: 0x0046E294
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_sub" };
			}
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x0600C147 RID: 49479 RVA: 0x004700A4 File Offset: 0x0046E2A4
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemSub", false, null) };
			}
		}

		// Token: 0x0600C148 RID: 49480 RVA: 0x004700BC File Offset: 0x0046E2BC
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

		// Token: 0x0600C149 RID: 49481 RVA: 0x004701F8 File Offset: 0x0046E3F8
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
