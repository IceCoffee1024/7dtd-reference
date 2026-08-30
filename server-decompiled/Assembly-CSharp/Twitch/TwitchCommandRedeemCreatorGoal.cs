using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200181B RID: 6171
	public class TwitchCommandRedeemCreatorGoal : BaseTwitchCommand
	{
		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x0600BEC1 RID: 48833 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x0600BEC2 RID: 48834 RVA: 0x00467DD7 File Offset: 0x00465FD7
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_goal" };
			}
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x0600BEC3 RID: 48835 RVA: 0x00467DE7 File Offset: 0x00465FE7
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGoal", false, null) };
			}
		}

		// Token: 0x0600BEC4 RID: 48836 RVA: 0x00467E00 File Offset: 0x00466000
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(array[1]);
			}
		}

		// Token: 0x0600BEC5 RID: 48837 RVA: 0x00467E2F File Offset: 0x0046602F
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(arguments[1]);
			}
		}
	}
}
