using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001847 RID: 6215
	public class TwitchCommandRedeemCreatorGoal : BaseTwitchCommand
	{
		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x0600C019 RID: 49177 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x0600C01A RID: 49178 RVA: 0x0046C313 File Offset: 0x0046A513
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_goal" };
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x0600C01B RID: 49179 RVA: 0x0046C323 File Offset: 0x0046A523
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGoal", false, null) };
			}
		}

		// Token: 0x0600C01C RID: 49180 RVA: 0x0046C33C File Offset: 0x0046A53C
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(array[1]);
			}
		}

		// Token: 0x0600C01D RID: 49181 RVA: 0x0046C36B File Offset: 0x0046A56B
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(arguments[1]);
			}
		}
	}
}
