using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200187A RID: 6266
	public class TwitchCommandRedeemHypeTrain : BaseTwitchCommand
	{
		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x0600C139 RID: 49465 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x0600C13A RID: 49466 RVA: 0x0046FF10 File Offset: 0x0046E110
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_hypetrain" };
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x0600C13B RID: 49467 RVA: 0x0046FF20 File Offset: 0x0046E120
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemHypeTrain", false, null) };
			}
		}

		// Token: 0x0600C13C RID: 49468 RVA: 0x0046FF38 File Offset: 0x0046E138
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

		// Token: 0x0600C13D RID: 49469 RVA: 0x0046FF74 File Offset: 0x0046E174
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
