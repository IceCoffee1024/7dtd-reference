using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200187D RID: 6269
	public class TwitchCommandRemoveViewer : BaseTwitchCommand
	{
		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x0600C14B RID: 49483 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x0600C14C RID: 49484 RVA: 0x00470341 File Offset: 0x0046E541
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#remove_viewer" };
			}
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x0600C14D RID: 49485 RVA: 0x00470351 File Offset: 0x0046E551
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RemoveViewer", false, null) };
			}
		}

		// Token: 0x0600C14E RID: 49486 RVA: 0x00470368 File Offset: 0x0046E568
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(array[1]);
			}
		}

		// Token: 0x0600C14F RID: 49487 RVA: 0x0047039D File Offset: 0x0046E59D
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(arguments[1]);
			}
		}
	}
}
