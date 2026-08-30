using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Platform
{
	// Token: 0x02001BC9 RID: 7113
	public static class PlatformApplicationManager
	{
		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x0600D3C4 RID: 54212 RVA: 0x004C7E71 File Offset: 0x004C6071
		// (set) Token: 0x0600D3C5 RID: 54213 RVA: 0x004C7E78 File Offset: 0x004C6078
		public static IPlatformApplication Application
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x0600D3C6 RID: 54214 RVA: 0x004C7E80 File Offset: 0x004C6080
		public static bool IsRestartRequired
		{
			get
			{
				return PlatformApplicationManager.isRestartRequired;
			}
		}

		// Token: 0x0600D3C7 RID: 54215 RVA: 0x004C7E87 File Offset: 0x004C6087
		public static bool Init()
		{
			PlatformApplicationManager.Application = IPlatformApplication.Create();
			return true;
		}

		// Token: 0x0600D3C8 RID: 54216 RVA: 0x004C7E94 File Offset: 0x004C6094
		public static void SetRestartRequired()
		{
			PlatformApplicationManager.isRestartRequired = PlatformApplicationManager.RestartIsSupported;
			Log.Out(string.Format("[PlatformApplication] restart required = {0}", PlatformApplicationManager.isRestartRequired));
		}

		// Token: 0x0600D3C9 RID: 54217 RVA: 0x004C7EB9 File Offset: 0x004C60B9
		public static bool CheckRestartCoroutineReady()
		{
			return PlatformApplicationManager.isRestartRequired && !PlatformApplicationManager.isRestarting && !InviteManager.Instance.IsConnectingToInvite();
		}

		// Token: 0x0600D3CA RID: 54218 RVA: 0x004C7ED8 File Offset: 0x004C60D8
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

		// Token: 0x0600D3CB RID: 54219 RVA: 0x004C7EE8 File Offset: 0x004C60E8
		[PublicizedFrom(EAccessModifier.Private)]
		public static string[] RemoveFirstRunArguments(string[] argv)
		{
			return (from arg in argv
				where !arg.StartsWith("-LoadSaveGame=", StringComparison.OrdinalIgnoreCase)
				where !arg.StartsWith("-RunAutomation=", StringComparison.OrdinalIgnoreCase)
				select arg).ToArray<string>();
		}

		// Token: 0x0600D3CC RID: 54220 RVA: 0x004C7F44 File Offset: 0x004C6144
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

		// Token: 0x0400A12B RID: 41259
		public static bool RestartIsSupported = PlatformOptimizations.RestartProcessSupported;

		// Token: 0x0400A12D RID: 41261
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestartRequired;

		// Token: 0x0400A12E RID: 41262
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool isRestarting;

		// Token: 0x02001BCA RID: 7114
		public static class LoadSaveGame
		{
			// Token: 0x0600D3CE RID: 54222 RVA: 0x004C804C File Offset: 0x004C624C
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

			// Token: 0x0600D3CF RID: 54223 RVA: 0x004C8194 File Offset: 0x004C6394
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

			// Token: 0x0600D3D0 RID: 54224 RVA: 0x004C824F File Offset: 0x004C644F
			public static void SetFailed()
			{
				if (PlatformApplicationManager.LoadSaveGame.loadSaveGameState == EPlatformLoadSaveGameState.Done)
				{
					return;
				}
				Log.Warning(string.Format("[LoadSaveGame] Failed to automate creating or loading the save game. State: {0}", PlatformApplicationManager.LoadSaveGame.loadSaveGameState));
				PlatformApplicationManager.LoadSaveGame.loadSaveGameState = EPlatformLoadSaveGameState.Done;
			}

			// Token: 0x0400A12F RID: 41263
			public static string GameWorld;

			// Token: 0x0400A130 RID: 41264
			public static string GameName;

			// Token: 0x0400A131 RID: 41265
			public static UserDataStorageType SaveStorageType;

			// Token: 0x0400A132 RID: 41266
			[PublicizedFrom(EAccessModifier.Private)]
			public static EPlatformLoadSaveGameState loadSaveGameState;
		}
	}
}
