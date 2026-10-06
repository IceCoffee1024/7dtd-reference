using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001841 RID: 6209
	public class TwitchCommandDisableCommand : BaseTwitchCommand
	{
		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x0600BFF6 RID: 49142 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x0600BFF7 RID: 49143 RVA: 0x0046BC74 File Offset: 0x00469E74
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#disable" };
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x0600BFF8 RID: 49144 RVA: 0x0046BC84 File Offset: 0x00469E84
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Disable", false, null) };
			}
		}

		// Token: 0x0600BFF9 RID: 49145 RVA: 0x0046BC9C File Offset: 0x00469E9C
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

		// Token: 0x0600BFFA RID: 49146 RVA: 0x0046BD64 File Offset: 0x00469F64
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
