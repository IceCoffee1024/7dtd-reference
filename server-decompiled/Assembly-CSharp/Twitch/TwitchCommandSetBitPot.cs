using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001822 RID: 6178
	public class TwitchCommandSetBitPot : BaseTwitchCommand
	{
		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x0600BEEB RID: 48875 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x0600BEEC RID: 48876 RVA: 0x0046860C File Offset: 0x0046680C
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#setbitpot" };
			}
		}

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x0600BEED RID: 48877 RVA: 0x0046861C File Offset: 0x0046681C
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_SetBitPot", false, null) };
			}
		}

		// Token: 0x0600BEEE RID: 48878 RVA: 0x00468634 File Offset: 0x00466834
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				int num = 0;
				if (int.TryParse(array[1], out num))
				{
					TwitchManager.Current.SetBitPot(num);
				}
			}
		}

		// Token: 0x0600BEEF RID: 48879 RVA: 0x00468670 File Offset: 0x00466870
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				int num = 0;
				if (int.TryParse(arguments[1], out num))
				{
					TwitchManager.Current.SetBitPot(num);
				}
			}
		}
	}
}
