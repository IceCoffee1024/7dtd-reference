using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001801 RID: 6145
	public class TwitchCommandUseProgression : BaseTwitchCommand
	{
		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x0600BD84 RID: 48516 RVA: 0x0007FE9C File Offset: 0x0007E09C
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Broadcaster;
			}
		}

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x0600BD85 RID: 48517 RVA: 0x00462C24 File Offset: 0x00460E24
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#useprogression" };
			}
		}

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x0600BD86 RID: 48518 RVA: 0x00462C34 File Offset: 0x00460E34
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_UseProgression", false, null) };
			}
		}

		// Token: 0x0600BD87 RID: 48519 RVA: 0x00462C4C File Offset: 0x00460E4C
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

		// Token: 0x0600BD88 RID: 48520 RVA: 0x00462CB4 File Offset: 0x00460EB4
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
