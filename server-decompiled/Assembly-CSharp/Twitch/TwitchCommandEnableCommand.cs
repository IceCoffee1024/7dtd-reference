using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001842 RID: 6210
	public class TwitchCommandEnableCommand : BaseTwitchCommand
	{
		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x0600BFFC RID: 49148 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x0600BFFD RID: 49149 RVA: 0x0046BE24 File Offset: 0x0046A024
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#enable" };
			}
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x0600BFFE RID: 49150 RVA: 0x0046BE34 File Offset: 0x0046A034
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Enable", false, null) };
			}
		}

		// Token: 0x0600BFFF RID: 49151 RVA: 0x0046BE4C File Offset: 0x0046A04C
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

		// Token: 0x0600C000 RID: 49152 RVA: 0x0046BF14 File Offset: 0x0046A114
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
