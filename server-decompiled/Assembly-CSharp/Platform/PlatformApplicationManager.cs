using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform
{
	// Token: 0x02001C2C RID: 7212
	public static class PlatformApplicationManager
	{
		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x0600D650 RID: 54864 RVA: 0x004D0481 File Offset: 0x004CE681
		// (set) Token: 0x0600D651 RID: 54865 RVA: 0x004D0488 File Offset: 0x004CE688
		public static IPlatformApplication Application
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x0600D652 RID: 54866 RVA: 0x004D0490 File Offset: 0x004CE690
		public static bool IsRestartRequired
		{
			get
			{
				return PlatformApplicationManager.isRestartRequired;
			}
		}

		// Token: 0x0600D653 RID: 54867 RVA: 0x004D0497 File Offset: 0x004CE697
		public static bool Init()
		{
			PlatformApplicationManager.Application = IPlatformApplication.Create();
			return true;
		}

		// Token: 0x0600D654 RID: 54868 RVA: 0x004D04A4 File Offset: 0x004CE6A4
		public static void SetRestartRequired()
		{
			PlatformApplicationManager.isRestartRequired = PlatformApplicationManager.RestartIsSupported;
			Log.Out(string.Format("[PlatformApplication] restart required = {0}", PlatformApplicationManager.isRestartRequired));
		}

		// Token: 0x0600D655 RID: 54869 RVA: 0x004D04C9 File Offset: 0x004CE6C9
		public static bool CheckRestartCoroutineReady()
		{
			return PlatformApplicationManager.isRestartRequired && !PlatformApplicationManager.isRestarting && !InviteManager.Instance.IsConnectingToInvite();
		}

		// Token: 0x0600D656 RID: 54870 RVA: 0x004D04E8 File Offset: 0x004CE6E8
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

		// Token: 0x0600D657 RID: 54871 RVA: 0x004D04F8 File Offset: 0x004CE6F8
		[PublicizedFrom(EAccessModifier.Private)]
		public static string[] RemoveFirstRunArguments(string[] argv)
		{
			return (from arg in argv
				where !arg.StartsWith("-LoadSaveGame=", StringComparison.OrdinalIgnoreCase)
				where !arg.StartsWith("-RunAutomation=", StringComparison.OrdinalIgnoreCase)
				select arg).ToArray<string>();
		}

		// Token: 0x0600D658 RID: 54872 RVA: 0x004D0554 File Offset: 0x004CE754
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

		// Token: 0x0400A2E9 RID: 41705
		public static bool RestartIsSupported = PlatformOptimizations.RestartProcessSupported;

		// Token: 0x0400A2EB RID: 41707
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestartRequired;

		// Token: 0x0400A2EC RID: 41708
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestarting;

		// Token: 0x02001C2D RID: 7213
		public static class LoadSaveGame
		{
			// Token: 0x0600D65A RID: 54874 RVA: 0x004D065C File Offset: 0x004CE85C
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

			// Token: 0x0600D65B RID: 54875 RVA: 0x004D07A4 File Offset: 0x004CE9A4
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

			// Token: 0x0600D65C RID: 54876 RVA: 0x004D085F File Offset: 0x004CEA5F
			public static void SetFailed()
			{
				if (PlatformApplicationManager.LoadSaveGame.loadSaveGameState == EPlatformLoadSaveGameState.Done)
				{
					return;
				}
				Log.Warning(string.Format("[LoadSaveGame] Failed to automate creating or loading the save game. State: {0}", PlatformApplicationManager.LoadSaveGame.loadSaveGameState));
				PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.Done;
			}

			// Token: 0x0400A2ED RID: 41709
			public static string GameWorld;

			// Token: 0x0400A2EE RID: 41710
			public static string GameName;

			// Token: 0x0400A2EF RID: 41711
			public static UserDataStorageType SaveStorageType;

			// Token: 0x0400A2F0 RID: 41712
			[PublicizedFrom(EAccessModifier.Private)]
			public static EPlatformLoadSaveGameState loadSaveGameState;
		}
	}
}
