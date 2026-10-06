using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200184C RID: 6220
	public class TwitchCommandRemoveViewer : BaseTwitchCommand
	{
		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600C037 RID: 49207 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x0600C038 RID: 49208 RVA: 0x0046C995 File Offset: 0x0046AB95
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#remove_viewer" };
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x0600C039 RID: 49209 RVA: 0x0046C9A5 File Offset: 0x0046ABA5
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RemoveViewer", false, null) };
			}
		}

		// Token: 0x0600C03A RID: 49210 RVA: 0x0046C9BC File Offset: 0x0046ABBC
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(array[1]);
			}
		}

		// Token: 0x0600C03B RID: 49211 RVA: 0x0046C9F1 File Offset: 0x0046ABF1
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(arguments[1]);
			}
		}
	}
}
