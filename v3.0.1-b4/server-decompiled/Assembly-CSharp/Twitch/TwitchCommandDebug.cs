using System;
using System.Collections.Generic;
using System.Text;

namespace Twitch
{
	// Token: 0x020017EE RID: 6126
	public class TwitchCommandDebug : BaseTwitchCommand
	{
		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x0600BD13 RID: 48403 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x0600BD14 RID: 48404 RVA: 0x00461914 File Offset: 0x0045FB14
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#debug" };
			}
		}

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x0600BD15 RID: 48405 RVA: 0x00461924 File Offset: 0x0045FB24
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("#debug", false, null) };
			}
		}

		// Token: 0x0600BD16 RID: 48406 RVA: 0x0046193B File Offset: 0x0045FB3B
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager.Current.DisplayDebug(message.Message);
		}

		// Token: 0x0600BD17 RID: 48407 RVA: 0x00461950 File Offset: 0x0045FB50
		public override void ExecuteConsole(List<string> arguments)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < arguments.Count; i++)
			{
				stringBuilder.Append(arguments[i] + " ");
			}
			TwitchManager.Current.DisplayDebug(stringBuilder.ToString());
		}
	}
}
