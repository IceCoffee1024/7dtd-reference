using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform
{
	// Token: 0x02001BFB RID: 7163
	public static class PlatformApplicationManager
	{
		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x0600D53C RID: 54588 RVA: 0x004CC959 File Offset: 0x004CAB59
		// (set) Token: 0x0600D53D RID: 54589 RVA: 0x004CC960 File Offset: 0x004CAB60
		public static IPlatformApplication Application
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x0600D53E RID: 54590 RVA: 0x004CC968 File Offset: 0x004CAB68
		public static bool IsRestartRequired
		{
			get
			{
				return PlatformApplicationManager.isRestartRequired;
			}
		}

		// Token: 0x0600D53F RID: 54591 RVA: 0x004CC96F File Offset: 0x004CAB6F
		public static bool Init()
		{
			PlatformApplicationManager.Application = IPlatformApplication.Create();
			return true;
		}

		// Token: 0x0600D540 RID: 54592 RVA: 0x004CC97C File Offset: 0x004CAB7C
		public static void SetRestartRequired()
		{
			PlatformApplicationManager.isRestartRequired = PlatformApplicationManager.RestartIsSupported;
			Log.Out(string.Format("[PlatformApplication] restart required = {0}", PlatformApplicationManager.isRestartRequired));
		}

		// Token: 0x0600D541 RID: 54593 RVA: 0x004CC9A1 File Offset: 0x004CABA1
		public static bool CheckRestartCoroutineReady()
		{
			return PlatformApplicationManager.isRestartRequired && !PlatformApplicationManager.isRestarting && !InviteManager.Instance.IsConnectingToInvite();
		}

		// Token: 0x0600D542 RID: 54594 RVA: 0x004CC9C0 File Offset: 0x004CABC0
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

		// Token: 0x0600D543 RID: 54595 RVA: 0x004CC9D0 File Offset: 0x004CABD0
		[PublicizedFrom(EAccessModifier.Private)]
		public static string[] RemoveFirstRunArguments(string[] argv)
		{
			return (from arg in argv
				where !arg.StartsWith("-LoadSaveGame=", StringComparison.OrdinalIgnoreCase)
				where !arg.StartsWith("-RunAutomation=", StringComparison.OrdinalIgnoreCase)
				select arg).ToArray<string>();
		}

		// Token: 0x0600D544 RID: 54596 RVA: 0x004CCA2C File Offset: 0x004CAC2C
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

		// Token: 0x0400A200 RID: 41472
		public static bool RestartIsSupported = PlatformOptimizations.RestartProcessSupported;

		// Token: 0x0400A202 RID: 41474
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestartRequired;

		// Token: 0x0400A203 RID: 41475
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestarting;

		// Token: 0x02001BFC RID: 7164
		public static class LoadSaveGame
		{
			// Token: 0x0600D546 RID: 54598 RVA: 0x004CCB34 File Offset: 0x004CAD34
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

			// Token: 0x0600D547 RID: 54599 RVA: 0x004CCC7C File Offset: 0x004CAE7C
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

			// Token: 0x0600D548 RID: 54600 RVA: 0x004CCD37 File Offset: 0x004CAF37
			public static void SetFailed()
			{
				if (PlatformApplicationManager.LoadSaveGame.loadSaveGameState == EPlatformLoadSaveGameState.Done)
				{
					return;
				}
				Log.Warning(string.Format("[LoadSaveGame] Failed to automate creating or loading the save game. State: {0}", PlatformApplicationManager.LoadSaveGame.loadSaveGameState));
				PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.Done;
			}

			// Token: 0x0400A204 RID: 41476
			public static string GameWorld;

			// Token: 0x0400A205 RID: 41477
			public static string GameName;

			// Token: 0x0400A206 RID: 41478
			public static UserDataStorageType SaveStorageType;

			// Token: 0x0400A207 RID: 41479
			[PublicizedFrom(EAccessModifier.Private)]
			public static EPlatformLoadSaveGameState loadSaveGameState;
		}
	}
}
