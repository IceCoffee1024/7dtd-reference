using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001815 RID: 6165
	public class TwitchCommandDisableCommand : BaseTwitchCommand
	{
		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x0600BE9E RID: 48798 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x0600BE9F RID: 48799 RVA: 0x00467738 File Offset: 0x00465938
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#disable" };
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x0600BEA0 RID: 48800 RVA: 0x00467748 File Offset: 0x00465948
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Disable", false, null) };
			}
		}

		// Token: 0x0600BEA1 RID: 48801 RVA: 0x00467760 File Offset: 0x00465960
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

		// Token: 0x0600BEA2 RID: 48802 RVA: 0x00467828 File Offset: 0x00465A28
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
