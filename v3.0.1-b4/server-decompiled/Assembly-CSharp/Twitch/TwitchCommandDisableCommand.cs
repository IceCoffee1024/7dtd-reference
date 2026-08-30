using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017EF RID: 6127
	public class TwitchCommandDisableCommand : BaseTwitchCommand
	{
		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x0600BD19 RID: 48409 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x0600BD1A RID: 48410 RVA: 0x0046199C File Offset: 0x0045FB9C
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#disable" };
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x0600BD1B RID: 48411 RVA: 0x004619AC File Offset: 0x0045FBAC
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_Disable", false, null) };
			}
		}

		// Token: 0x0600BD1C RID: 48412 RVA: 0x004619C4 File Offset: 0x0045FBC4
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

		// Token: 0x0600BD1D RID: 48413 RVA: 0x00461A8C File Offset: 0x0045FC8C
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
