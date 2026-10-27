using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200187E RID: 6270
	public class TwitchCommandResetCooldowns : BaseTwitchCommand
	{
		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600C151 RID: 49489 RVA: 0x00047076 File Offset: 0x00045276
		public override BaseTwitchCommand.PermissionLevels RequiredPermission
		{
			get
			{
				return BaseTwitchCommand.PermissionLevels.Mod;
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x0600C152 RID: 49490 RVA: 0x004703BF File Offset: 0x0046E5BF
		public override string[] CommandText
		{
			get
			{
				return new string[] { "#reset_cooldowns" };
			}
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x0600C153 RID: 49491 RVA: 0x004703CF File Offset: 0x0046E5CF
		public override string[] LocalizedCommandNames
		{
			get
			{
				return new string[] { Localization.Get("TwitchCommand_ResetCooldowns", false, null) };
			}
		}

		// Token: 0x0600C154 RID: 49492 RVA: 0x004703E8 File Offset: 0x0046E5E8
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

		// Token: 0x0600C155 RID: 49493 RVA: 0x00470468 File Offset: 0x0046E668
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
