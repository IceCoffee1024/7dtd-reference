using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001849 RID: 6217
	public class TwitchCommandRedeemHypeTrain : BaseTwitchCommand
	{
		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x0600C025 RID: 49189 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x0600C026 RID: 49190 RVA: 0x0046C564 File Offset: 0x0046A764
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_hypetrain" };
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x0600C027 RID: 49191 RVA: 0x0046C574 File Offset: 0x0046A774
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemHypeTrain", false, null) };
			}
		}

		// Token: 0x0600C028 RID: 49192 RVA: 0x0046C58C File Offset: 0x0046A78C
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (StringParsers.TryParseSInt32(array[1], out num))
				{
					TwitchManager.Current.HandleHypeTrainRedeem(num);
				}
			}
		}

		// Token: 0x0600C029 RID: 49193 RVA: 0x0046C5C8 File Offset: 0x0046A7C8
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (StringParsers.TryParseSInt32(arguments[1], out num))
				{
					TwitchManager.Current.HandleHypeTrainRedeem(num);
				}
			}
		}
	}
}
