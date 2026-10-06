using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001844 RID: 6212
	public class TwitchCommandPauseCommand : BaseTwitchCommand
	{
		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x0600C007 RID: 49159 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x0600C008 RID: 49160 RVA: 0x0046C01E File Offset: 0x0046A21E
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#pause" };
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x0600C009 RID: 49161 RVA: 0x0046C02E File Offset: 0x0046A22E
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Pause", false, null) };
			}
		}

		// Token: 0x0600C00A RID: 49162 RVA: 0x0046C045 File Offset: 0x0046A245
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			if (message.Message.Split(' ', StringSplitOptions.None).Length == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}

		// Token: 0x0600C00B RID: 49163 RVA: 0x0046C065 File Offset: 0x0046A265
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager.Current.SetTwitchActive(false);
			}
		}
	}
}
