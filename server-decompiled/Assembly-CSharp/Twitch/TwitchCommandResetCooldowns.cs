using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017FB RID: 6139
	public class TwitchCommandResetCooldowns : BaseTwitchCommand
	{
		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x0600BD60 RID: 48480 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x0600BD61 RID: 48481 RVA: 0x0046273B File Offset: 0x0046093B
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#reset_cooldowns" };
			}
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x0600BD62 RID: 48482 RVA: 0x0046274B File Offset: 0x0046094B
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_ResetCooldowns", false, null) };
			}
		}

		// Token: 0x0600BD63 RID: 48483 RVA: 0x00462764 File Offset: 0x00460964
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

		// Token: 0x0600BD64 RID: 48484 RVA: 0x004627E4 File Offset: 0x004609E4
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
