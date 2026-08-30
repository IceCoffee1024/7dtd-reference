using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001827 RID: 6183
	public class TwitchCommandUseProgression : BaseTwitchCommand
	{
		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x0600BF09 RID: 48905 RVA: 0x00080550 File Offset: 0x0007E750
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Broadcaster;
			}
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x0600BF0A RID: 48906 RVA: 0x004689C0 File Offset: 0x00466BC0
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#useprogression" };
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x0600BF0B RID: 48907 RVA: 0x004689D0 File Offset: 0x00466BD0
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_UseProgression", false, null) };
			}
		}

		// Token: 0x0600BF0C RID: 48908 RVA: 0x004689E8 File Offset: 0x00466BE8
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				bool flag = false;
				if (bool.TryParse(array[1], out flag))
				{
					TwitchManager.Current.SetUseProgression(flag);
				}
				TwitchManager.Current.SendChannelMessage("[7DTD]: Use Progression Enabled: " + (TwitchManager.Current.UseProgression ? "Yes" : "No"), true);
			}
		}

		// Token: 0x0600BF0D RID: 48909 RVA: 0x00468A50 File Offset: 0x00466C50
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				bool flag = false;
				if (bool.TryParse(arguments[1], out flag))
				{
					TwitchManager.Current.SetUseProgression(flag);
				}
				TwitchManager.Current.SendChannelMessage("[7DTD]: Use Progression Enabled: " + (TwitchManager.Current.UseProgression ? "Yes" : "No"), true);
			}
		}
	}
}
