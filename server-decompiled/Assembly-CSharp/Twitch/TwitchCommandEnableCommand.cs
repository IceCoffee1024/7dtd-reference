using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F0 RID: 6128
	public class TwitchCommandEnableCommand : BaseTwitchCommand
	{
		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x0600BD1F RID: 48415 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x0600BD20 RID: 48416 RVA: 0x00461B4C File Offset: 0x0045FD4C
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#enable" };
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x0600BD21 RID: 48417 RVA: 0x00461B5C File Offset: 0x0045FD5C
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Enable", false, null) };
			}
		}

		// Token: 0x0600BD22 RID: 48418 RVA: 0x00461B74 File Offset: 0x0045FD74
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

		// Token: 0x0600BD23 RID: 48419 RVA: 0x00461C3C File Offset: 0x0045FE3C
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
