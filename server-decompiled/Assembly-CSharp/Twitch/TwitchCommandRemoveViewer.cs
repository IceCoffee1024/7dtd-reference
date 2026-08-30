using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001820 RID: 6176
	public class TwitchCommandRemoveViewer : BaseTwitchCommand
	{
		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x0600BEDF RID: 48863 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x0600BEE0 RID: 48864 RVA: 0x00468459 File Offset: 0x00466659
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#remove_viewer" };
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x0600BEE1 RID: 48865 RVA: 0x00468469 File Offset: 0x00466669
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_RemoveViewer", false, null) };
			}
		}

		// Token: 0x0600BEE2 RID: 48866 RVA: 0x00468480 File Offset: 0x00466680
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(array[1]);
			}
		}

		// Token: 0x0600BEE3 RID: 48867 RVA: 0x004684B5 File Offset: 0x004666B5
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager.Current.ViewerData.RemoveViewerEntry(arguments[1]);
			}
		}
	}
}
