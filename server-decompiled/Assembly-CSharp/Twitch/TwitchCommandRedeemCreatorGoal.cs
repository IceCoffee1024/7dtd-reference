using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001878 RID: 6264
	public class TwitchCommandRedeemCreatorGoal : BaseTwitchCommand
	{
		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x0600C12D RID: 49453 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x0600C12E RID: 49454 RVA: 0x0046FCBF File Offset: 0x0046DEBF
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_goal" };
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x0600C12F RID: 49455 RVA: 0x0046FCCF File Offset: 0x0046DECF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGoal", false, null) };
			}
		}

		// Token: 0x0600C130 RID: 49456 RVA: 0x0046FCE8 File Offset: 0x0046DEE8
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(array[1]);
			}
		}

		// Token: 0x0600C131 RID: 49457 RVA: 0x0046FD17 File Offset: 0x0046DF17
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(arguments[1]);
			}
		}
	}
}
