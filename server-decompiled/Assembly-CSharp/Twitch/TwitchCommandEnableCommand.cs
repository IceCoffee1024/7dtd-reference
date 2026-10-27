using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001873 RID: 6259
	public class TwitchCommandEnableCommand : BaseTwitchCommand
	{
		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x0600C110 RID: 49424 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x0600C111 RID: 49425 RVA: 0x0046F7D0 File Offset: 0x0046D9D0
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#enable" };
			}
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x0600C112 RID: 49426 RVA: 0x0046F7E0 File Offset: 0x0046D9E0
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Enable", false, null) };
			}
		}

		// Token: 0x0600C113 RID: 49427 RVA: 0x0046F7F8 File Offset: 0x0046D9F8
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

		// Token: 0x0600C114 RID: 49428 RVA: 0x0046F8C0 File Offset: 0x0046DAC0
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
