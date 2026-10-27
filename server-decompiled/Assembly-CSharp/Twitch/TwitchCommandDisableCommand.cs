using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001872 RID: 6258
	public class TwitchCommandDisableCommand : BaseTwitchCommand
	{
		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x0600C10A RID: 49418 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x0600C10B RID: 49419 RVA: 0x0046F620 File Offset: 0x0046D820
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#disable" };
			}
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x0600C10C RID: 49420 RVA: 0x0046F630 File Offset: 0x0046D830
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Disable", false, null) };
			}
		}

		// Token: 0x0600C10D RID: 49421 RVA: 0x0046F648 File Offset: 0x0046D848
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
						twitchAction.Enabled = false;
						flag = true;
					}
				}
				if (flag)
				{
					twitchManager.SendChannelMessage("[7DTD]: Command Disabled: " + array[1], true);
					twitchManager.SetupAvailableCommands();
				}
			}
		}

		// Token: 0x0600C10E RID: 49422 RVA: 0x0046F710 File Offset: 0x0046D910
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
						twitchAction.Enabled = false;
						flag = true;
					}
				}
				if (flag)
				{
					twitchManager.SendChannelMessage("[7DTD]: Command Disabled: " + arguments[1], true);
					twitchManager.SetupAvailableCommands();
				}
			}
		}
	}
}
