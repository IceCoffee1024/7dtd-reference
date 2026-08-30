using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017FA RID: 6138
	public class TwitchCommandRemoveViewer : BaseTwitchCommand
	{
		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x0600BD5A RID: 48474 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x0600BD5B RID: 48475 RVA: 0x004626BD File Offset: 0x004608BD
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#remove_viewer" };
			}
		}

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x0600BD5C RID: 48476 RVA: 0x004626CD File Offset: 0x004608CD
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RemoveViewer", false, null) };
			}
		}

		// Token: 0x0600BD5D RID: 48477 RVA: 0x004626E4 File Offset: 0x004608E4
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(array[1]);
			}
		}

		// Token: 0x0600BD5E RID: 48478 RVA: 0x00462719 File Offset: 0x00460919
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(arguments[1]);
			}
		}
	}
}
