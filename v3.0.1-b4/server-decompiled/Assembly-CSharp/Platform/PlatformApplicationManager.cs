using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform
{
	// Token: 0x02001BA2 RID: 7074
	public static class PlatformApplicationManager
	{
		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x0600D236 RID: 53814 RVA: 0x004C1A7D File Offset: 0x004BFC7D
		// (set) Token: 0x0600D237 RID: 53815 RVA: 0x004C1A84 File Offset: 0x004BFC84
		public static IPlatformApplication Application
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x0600D238 RID: 53816 RVA: 0x004C1A8C File Offset: 0x004BFC8C
		public static bool IsRestartRequired
		{
			get
			{
				return PlatformApplicationManager.isRestartRequired;
			}
		}

		// Token: 0x0600D239 RID: 53817 RVA: 0x004C1A93 File Offset: 0x004BFC93
		public static bool Init()
		{
			PlatformApplicationManager.Application = IPlatformApplication.Create();
			return true;
		}

		// Token: 0x0600D23A RID: 53818 RVA: 0x004C1AA0 File Offset: 0x004BFCA0
		public static void SetRestartRequired()
		{
			PlatformApplicationManager.isRestartRequired = PlatformApplicationManager.RestartIsSupported;
			Log.Out(string.Format("[PlatformApplication] restart required = {0}", PlatformApplicationManager.isRestartRequired));
		}

		// Token: 0x0600D23B RID: 53819 RVA: 0x004C1AC5 File Offset: 0x004BFCC5
		public static bool CheckRestartCoroutineReady()
		{
			return PlatformApplicationManager.isRestartRequired && !PlatformApplicationManager.isRestarting && !InviteManager.Instance.IsConnectingToInvite();
		}

		// Token: 0x0600D23C RID: 53820 RVA: 0x004C1AE4 File Offset: 0x004BFCE4
		public static IEnumerator CheckRestartCoroutine(bool loadSaveGame = false)
		{
			if (!PlatformApplicationManager.CheckRestartCoroutineReady())
			{
				yield break;
			}
			PlatformApplicationManager.isRestartRequired = false;
			if (!PlatformOptimizations.RestartProcessSupported)
			{
				Log.Error("[PlatformApplication] Process restart debug: application would have restarted here if supported by the platform/device");
				yield break;
			}
			PlatformApplicationManager.isRestarting = true;
			try
			{
				yield return GameManager.Instance.ShowExitingGameUICoroutine();
				PlatformApplicationManager.RestartProcess(loadSaveGame);
			}
			finally
			{
				Log.Error("[PlatformApplication] failed to restart process.");
				PlatformApplicationManager.isRestarting = false;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600D23D RID: 53821 RVA: 0x004C1AF4 File Offset: 0x004BFCF4
		[PublicizedFrom(EAccessModifier.Private)]
		public static string[] RemoveFirstRunArguments(string[] argv)
		{
			return (from arg in argv
				where !arg.StartsWith("-LoadSaveGame=", StringComparison.OrdinalIgnoreCase)
				where !arg.StartsWith("-RunAutomation=", StringComparison.OrdinalIgnoreCase)
				select arg).ToArray<string>();
		}

		// Token: 0x0600D23E RID: 53822 RVA: 0x004C1B50 File Offset: 0x004BFD50
		[PublicizedFrom(EAccessModifier.Private)]
		public static void RestartProcess(bool loadSaveGame)
		{
			List<string> list = new List<string>();
			list.AddRange(PlatformApplicationManager.RemoveFirstRunArguments(GameStartupHelper.RemoveTemporaryArguments(GameStartupHelper.GetCommandLineArgs())));
			list.Add("[REMOVE_ON_RESTART]");
			list.Add("-skipintro");
			list.Add(LaunchPrefs.SkipNewsScreen.ToCommandLine(true));
			if (PlatformOptimizations.RestartAfterRwg && loadSaveGame)
			{
				Log.Out(string.Format("[LoadSaveGame] After restart should load: worldName={0} saveName={1} ({2})", GamePrefs.GetString(EnumGamePrefs.GameWorld), GamePrefs.GetString(EnumGamePrefs.GameName), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType)));
				list.Add(LaunchPrefs.LoadSaveGame.ToCommandLine(true));
			}
			list.AddRange(InviteManager.Instance.GetCommandLineArguments());
			list.AddRange(PlatformManager.NativePlatform.GetArgumentsForRelaunch());
			try
			{
				GamePrefs.Instance.Save();
				SaveDataUtils.Destroy();
				PlatformManager.Destroy();
			}
			catch (Exception ex)
			{
				Log.Error("Exception thrown while preparing for process restart. This may cause errors in the next run");
				Log.Exception(ex);
			}
			PlatformApplicationManager.Application.RestartProcess(list.ToArray());
		}

		// Token: 0x04009FFA RID: 40954
		public static bool RestartIsSupported = PlatformOptimizations.RestartProcessSupported;

		// Token: 0x04009FFC RID: 40956
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestartRequired;

		// Token: 0x04009FFD RID: 40957
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestarting;

		// Token: 0x02001BA3 RID: 7075
		public static class LoadSaveGame
		{
			// Token: 0x0600D240 RID: 53824 RVA: 0x004C1C58 File Offset: 0x004BFE58
			public static EPlatformLoadSaveGameState GetState()
			{
				if (!LaunchPrefs.LoadSaveGame.Value)
				{
					return EPlatformLoadSaveGameState.Done;
				}
				if (PlatformApplicationManager.LoadSaveGame.loadSaveGameState != EPlatformLoadSaveGameState.Init)
				{
					return PlatformApplicationManager.LoadSaveGame.loadSaveGameState;
				}
				PlatformApplicationManager.LoadSaveGame.GameWorld = GamePrefs.GetString(EnumGamePrefs.GameWorld);
				PlatformApplicationManager.LoadSaveGame.GameName = GamePrefs.GetString(EnumGamePrefs.GameName);
				PlatformApplicationManager.LoadSaveGame.SaveStorageType = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
				bool found = false;
				bool isArchived = false;
				GameIO.GetPlayerSaves(delegate(UserDataStorageType foundStorage, string foundSaveName, string foundWorldName, DateTime _, WorldState _, bool foundIsArchived)
				{
					if (foundStorage == PlatformApplicationManager.LoadSaveGame.SaveStorageType && foundSaveName.EqualsCaseInsensitive(PlatformApplicationManager.LoadSaveGame.GameName) && foundWorldName.EqualsCaseInsensitive(PlatformApplicationManager.LoadSaveGame.GameWorld))
					{
						found = true;
						isArchived = foundIsArchived;
					}
				}, true);
				if (!found)
				{
					Log.Out(string.Concat(new string[]
					{
						"[LoadSaveGame] Creating new save game '",
						PlatformApplicationManager.LoadSaveGame.GameName,
						"' from the world '",
						PlatformApplicationManager.LoadSaveGame.GameWorld,
						"'."
					}));
					return PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.NewGameOpen;
				}
				if (isArchived)
				{
					Log.Warning(string.Concat(new string[]
					{
						"[LoadSaveGame] Can not load archived save '",
						PlatformApplicationManager.LoadSaveGame.GameName,
						"' (world '",
						PlatformApplicationManager.LoadSaveGame.GameWorld,
						"')."
					}));
					return PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.Done;
				}
				Log.Out(string.Concat(new string[]
				{
					"[LoadSaveGame] Loading existing save game '",
					PlatformApplicationManager.LoadSaveGame.GameName,
					"' (world '",
					PlatformApplicationManager.LoadSaveGame.GameWorld,
					"')."
				}));
				return PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.ContinueGameOpen;
			}

			// Token: 0x0600D241 RID: 53825 RVA: 0x004C1DA0 File Offset: 0x004BFFA0
			public static void AdvanceStateFrom(EPlatformLoadSaveGameState previousState)
			{
				if (PlatformApplicationManager.LoadSaveGame.loadSaveGameState != previousState)
				{
					Log.Error(string.Format("[LoadSaveGame] Expected advance from {0} but was {1}", PlatformApplicationManager.LoadSaveGame.loadSaveGameState, previousState));
					PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.Done;
				}
				EPlatformLoadSaveGameState eplatformLoadSaveGameState;
				switch (previousState)
				{
				case EPlatformLoadSaveGameState.Init:
					throw new NotSupportedException("Init state should be manually advanced from because it branches.");
				case EPlatformLoadSaveGameState.NewGameOpen:
					eplatformLoadSaveGameState = EPlatformLoadSaveGameState.NewGameSelect;
					break;
				case EPlatformLoadSaveGameState.NewGameSelect:
					eplatformLoadSaveGameState = EPlatformLoadSaveGameState.NewGamePlay;
					break;
				case EPlatformLoadSaveGameState.NewGamePlay:
					eplatformLoadSaveGameState = EPlatformLoadSaveGameState.Done;
					break;
				case EPlatformLoadSaveGameState.ContinueGameOpen:
					eplatformLoadSaveGameState = EPlatformLoadSaveGameState.ContinueGameSelect;
					break;
				case EPlatformLoadSaveGameState.ContinueGameSelect:
					eplatformLoadSaveGameState = EPlatformLoadSaveGameState.ContinueGamePlay;
					break;
				case EPlatformLoadSaveGameState.ContinueGamePlay:
					eplatformLoadSaveGameState = EPlatformLoadSaveGameState.Done;
					break;
				case EPlatformLoadSaveGameState.Done:
					throw new NotSupportedException("Can't advance from the final state.");
				default:
					throw new ArgumentOutOfRangeException();
				}
				PlatformApplicationManager.LoadSaveGame.loadSaveGameState = eplatformLoadSaveGameState;
				Log.Out(string.Format("[LoadSaveGame] Advanced to state {0} (was {1})", PlatformApplicationManager.LoadSaveGame.loadSaveGameState, previousState));
			}

			// Token: 0x0600D242 RID: 53826 RVA: 0x004C1E5B File Offset: 0x004C005B
			public static void SetFailed()
			{
				if (PlatformApplicationManager.LoadSaveGame.loadSaveGameState == EPlatformLoadSaveGameState.Done)
				{
					return;
				}
				Log.Warning(string.Format("[LoadSaveGame] Failed to automate creating or loading the save game. State: {0}", PlatformApplicationManager.LoadSaveGame.loadSaveGameState));
				PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.Done;
			}

			// Token: 0x04009FFE RID: 40958
			public static string GameWorld;

			// Token: 0x04009FFF RID: 40959
			public static string GameName;

			// Token: 0x0400A000 RID: 40960
			public static UserDataStorageType SaveStorageType;

			// Token: 0x0400A001 RID: 40961
			[PublicizedFrom(EAccessModifier.Private)]
			public static EPlatformLoadSaveGameState loadSaveGameState;
		}
	}
}
