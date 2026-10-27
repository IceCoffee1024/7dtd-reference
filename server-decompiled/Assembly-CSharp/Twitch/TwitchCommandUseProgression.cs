using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001884 RID: 6276
	public class TwitchCommandUseProgression : BaseTwitchCommand
	{
		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x0600C175 RID: 49525 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Broadcaster;
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x0600C176 RID: 49526 RVA: 0x004708A8 File Offset: 0x0046EAA8
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#useprogression" };
			}
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x0600C177 RID: 49527 RVA: 0x004708B8 File Offset: 0x0046EAB8
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_UseProgression", false, null) };
			}
		}

		// Token: 0x0600C178 RID: 49528 RVA: 0x004708D0 File Offset: 0x0046EAD0
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

		// Token: 0x0600C179 RID: 49529 RVA: 0x00470938 File Offset: 0x0046EB38
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
