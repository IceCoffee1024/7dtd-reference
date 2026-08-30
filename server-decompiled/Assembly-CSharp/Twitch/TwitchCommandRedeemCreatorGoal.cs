using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F5 RID: 6133
	public class TwitchCommandRedeemCreatorGoal : BaseTwitchCommand
	{
		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x0600BD3C RID: 48444 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x0600BD3D RID: 48445 RVA: 0x0046203B File Offset: 0x0046023B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_goal" };
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x0600BD3E RID: 48446 RVA: 0x0046204B File Offset: 0x0046024B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemGoal", false, null) };
			}
		}

		// Token: 0x0600BD3F RID: 48447 RVA: 0x00462064 File Offset: 0x00460264
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(array[1]);
			}
		}

		// Token: 0x0600BD40 RID: 48448 RVA: 0x00462093 File Offset: 0x00460293
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.HandleCreatorGoalRedeem(arguments[1]);
			}
		}
	}
}
