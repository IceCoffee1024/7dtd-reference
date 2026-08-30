using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001816 RID: 6166
	public class TwitchCommandEnableCommand : BaseTwitchCommand
	{
		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x0600BEA4 RID: 48804 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x0600BEA5 RID: 48805 RVA: 0x004678E8 File Offset: 0x00465AE8
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#enable" };
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x0600BEA6 RID: 48806 RVA: 0x004678F8 File Offset: 0x00465AF8
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Enable", false, null) };
			}
		}

		// Token: 0x0600BEA7 RID: 48807 RVA: 0x00467910 File Offset: 0x00465B10
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			string[] array = message.Message.Split(' ', StringSplitOptions.None);
			if (array.Length == 2)
			{
				TwitchManager twitchManager = TwitchManager.Current;
				bool flag = false;
				foreach (string text in TwitchActionManager.TwitchActions.Keys)
				{
					TwitchAction twitchAction = TwitchActionManager.TwitchActions[text];
					if (twitchAction.IsInPreset(twitchManager.CurrentActionPreset) && twitchAction.Command.Equals(array[1]))
					{
						twitchAction.Enabled = true;
						flag = true;
					}
				}
				if (flag)
				{
					twitchManager.SendChannelMessage("[7DTD]: Command Enabled: " + array[1], true);
					twitchManager.SetupAvailableCommands();
				}
			}
		}

		// Token: 0x0600BEA8 RID: 48808 RVA: 0x004679D8 File Offset: 0x00465BD8
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 2)
			{
				TwitchManager twitchManager = TwitchManager.Current;
				bool flag = false;
				foreach (string text in TwitchActionManager.TwitchActions.Keys)
				{
					TwitchAction twitchAction = TwitchActionManager.TwitchActions[text];
					if (twitchAction.IsInPreset(twitchManager.CurrentActionPreset) && twitchAction.Command.Equals(arguments[1]))
					{
						twitchAction.Enabled = true;
						flag = true;
					}
				}
				if (flag)
				{
					twitchManager.SendChannelMessage("[7DTD]: Command Enabled: " + arguments[1], true);
					twitchManager.SetupAvailableCommands();
				}
			}
		}
	}
}
