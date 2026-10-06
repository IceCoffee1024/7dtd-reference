using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001853 RID: 6227
	public class TwitchCommandUseProgression : BaseTwitchCommand
	{
		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x0600C061 RID: 49249 RVA: 0x00080804 File Offset: 0x0007EA04
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Broadcaster;
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x0600C062 RID: 49250 RVA: 0x0046CEFC File Offset: 0x0046B0FC
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#useprogression" };
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x0600C063 RID: 49251 RVA: 0x0046CF0C File Offset: 0x0046B10C
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_UseProgression", false, null) };
			}
		}

		// Token: 0x0600C064 RID: 49252 RVA: 0x0046CF24 File Offset: 0x0046B124
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

		// Token: 0x0600C065 RID: 49253 RVA: 0x0046CF8C File Offset: 0x0046B18C
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
