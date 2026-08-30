using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200181D RID: 6173
	public class TwitchCommandRedeemHypeTrain : BaseTwitchCommand
	{
		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x0600BECD RID: 48845 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x0600BECE RID: 48846 RVA: 0x00468028 File Offset: 0x00466228
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#redeem_hypetrain" };
			}
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x0600BECF RID: 48847 RVA: 0x00468038 File Offset: 0x00466238
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RedeemHypeTrain", false, null) };
			}
		}

		// Token: 0x0600BED0 RID: 48848 RVA: 0x00468050 File Offset: 0x00466250
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

		// Token: 0x0600BED1 RID: 48849 RVA: 0x0046808C File Offset: 0x0046628C
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
