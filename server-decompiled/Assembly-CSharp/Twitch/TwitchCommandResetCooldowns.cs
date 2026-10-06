using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200184D RID: 6221
	public class TwitchCommandResetCooldowns : BaseTwitchCommand
	{
		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x0600C03D RID: 49213 RVA: 0x000470B6 File Offset: 0x000452B6
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x0600C03E RID: 49214 RVA: 0x0046CA13 File Offset: 0x0046AC13
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#reset_cooldowns" };
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x0600C03F RID: 49215 RVA: 0x0046CA23 File Offset: 0x0046AC23
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_ResetCooldowns", false, null) };
			}
		}

		// Token: 0x0600C040 RID: 49216 RVA: 0x0046CA3C File Offset: 0x0046AC3C
		public override void Execute(ViewerEntry entry, TwitchIRCClient.TwitchChatMessage message)
		{
			TwitchManager twitchManager = TwitchManager.Current;
			float currentUnityTime = twitchManager.CurrentUnityTime;
			foreach (string text in TwitchActionManager.TwitchActions.Keys)
			{
				TwitchActionManager.TwitchActions[text].ResetCooldown(currentUnityTime);
			}
			twitchManager.SetupAvailableCommands();
			twitchManager.SendChannelMessage("Action Cooldowns have been reset!", true);
		}

		// Token: 0x0600C041 RID: 49217 RVA: 0x0046CABC File Offset: 0x0046ACBC
		public override void ExecuteConsole(List<string> arguments)
		{
			if (arguments.Count == 1)
			{
				TwitchManager twitchManager = TwitchManager.Current;
				float currentUnityTime = twitchManager.CurrentUnityTime;
				foreach (string text in TwitchActionManager.TwitchActions.Keys)
				{
					TwitchActionManager.TwitchActions[text].ResetCooldown(currentUnityTime);
				}
				twitchManager.SetupAvailableCommands();
				twitchManager.SendChannelMessage("Action Cooldowns have been reset!", true);
			}
		}
	}
}
