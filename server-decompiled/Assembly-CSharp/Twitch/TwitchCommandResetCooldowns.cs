using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001821 RID: 6177
	public class TwitchCommandResetCooldowns : BaseTwitchCommand
	{
		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x0600BEE5 RID: 48869 RVA: 0x00046EDA File Offset: 0x000450DA
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x0600BEE6 RID: 48870 RVA: 0x004684D7 File Offset: 0x004666D7
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#reset_cooldowns" };
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x0600BEE7 RID: 48871 RVA: 0x004684E7 File Offset: 0x004666E7
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_ResetCooldowns", false, null) };
			}
		}

		// Token: 0x0600BEE8 RID: 48872 RVA: 0x00468500 File Offset: 0x00466700
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

		// Token: 0x0600BEE9 RID: 48873 RVA: 0x00468580 File Offset: 0x00466780
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
