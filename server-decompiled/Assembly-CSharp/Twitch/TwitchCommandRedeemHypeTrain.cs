using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F7 RID: 6135
	public class TwitchCommandRedeemHypeTrain : BaseTwitchCommand
	{
		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x0600BD48 RID: 48456 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x0600BD49 RID: 48457 RVA: 0x0046228C File Offset: 0x0046048C
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_hypetrain" };
			}
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x0600BD4A RID: 48458 RVA: 0x0046229C File Offset: 0x0046049C
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemHypeTrain", false, null) };
			}
		}

		// Token: 0x0600BD4B RID: 48459 RVA: 0x004622B4 File Offset: 0x004604B4
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

		// Token: 0x0600BD4C RID: 48460 RVA: 0x004622F0 File Offset: 0x004604F0
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
