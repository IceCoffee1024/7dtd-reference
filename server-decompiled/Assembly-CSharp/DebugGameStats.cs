using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Platform;
using UnityEngine;

// Token: 0x020013E5 RID: 5093
public static class DebugGameStats
{
	// Token: 0x06009FDB RID: 40923 RVA: 0x003C4E24 File Offset: 0x003C3024
	public static void StartStatisticsUpdate(DebugGameStats.StatisticsUpdatedCallback callback)
	{
		if (DebugGameStats.m_memorySampler == null)
		{
			IPlatformMemory memory = PlatformManager.MultiPlatform.Memory;
			DebugGameStats.m_memorySampler = ((memory != null) ? memory.CreateSampler() : null);
		}
		IPlatformMemorySampler memorySampler = DebugGameStats.m_memorySampler;
		object obj;
		if (memorySampler == null)
		{
			obj = null;
		}
		else
		{
			IReadOnlyList<IPlatformMemoryStat> statistics = memorySampler.Statistics;
			if (statistics == null)
			{
				obj = null;
			}
			else
			{
				obj = statistics.FirstOrDefault<IPlatformMemoryStat>((IPlatformMemoryStat s) => s.Name == "GameUsed");
			}
		}
		DebugGameStats.m_memoryGameUsedStat = (IPlatformMemoryStat<long>)obj;
		DebugGameStats.TryInitializeStatisticsDictionary();
		if (DebugGameStats.updateStatsCoroutine != null)
		{
			ThreadManager.StopCoroutine(DebugGameStats.updateStatsCoroutine);
		}
		if (DebugGameStats.updateDeltasCoroutine != null)
		{
			ThreadManager.StopCoroutine(DebugGameStats.updateDeltasCoroutine);
		}
		DebugGameStats.doStatisticsUpdate = true;
		DebugGameStats.updateStatsCoroutine = ThreadManager.StartCoroutine(DebugGameStats.UpdateStatisticsCo(callback));
		DebugGameStats.updateDeltasCoroutine = ThreadManager.StartCoroutine(DebugGameStats.UpdateDeltas());
	}

	// Token: 0x06009FDC RID: 40924 RVA: 0x003C4EE8 File Offset: 0x003C30E8
	public static void TryInitializeStatisticsDictionary()
	{
		if (DebugGameStats.statisticsDictionary.Keys.Count > 0)
		{
			return;
		}
		foreach (FieldInfo fieldInfo in typeof(DebugGameStats.Statistics).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
		{
			if (fieldInfo.IsLiteral && !fieldInfo.IsInitOnly && fieldInfo.FieldType == typeof(string))
			{
				string text = (string)fieldInfo.GetValue(null);
				if (!DebugGameStats.statisticsDictionary.ContainsKey(text))
				{
					DebugGameStats.statisticsDictionary[text] = string.Empty;
				}
			}
		}
	}

	// Token: 0x06009FDD RID: 40925 RVA: 0x003C4F7D File Offset: 0x003C317D
	public static void StopStatisticsUpdate()
	{
		DebugGameStats.doStatisticsUpdate = false;
		DebugGameStats.updateStatsCoroutine = null;
		DebugGameStats.updateDeltasCoroutine = null;
	}

	// Token: 0x06009FDE RID: 40926 RVA: 0x003C4F94 File Offset: 0x003C3194
	public static string GetHeader(char separator)
	{
		DebugGameStats.TryInitializeStatisticsDictionary();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, string> keyValuePair in DebugGameStats.statisticsDictionary)
		{
			stringBuilder.Append(keyValuePair.Key);
			stringBuilder.Append(separator);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06009FDF RID: 40927 RVA: 0x003C5008 File Offset: 0x003C3208
	public static string GetCurrentStatsString(char separator = ',')
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, string> keyValuePair in DebugGameStats.statisticsDictionary)
		{
			stringBuilder.Append(keyValuePair.Value);
			stringBuilder.Append(separator);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06009FE0 RID: 40928 RVA: 0x003C5078 File Offset: 0x003C3278
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator UpdateDeltas()
	{
		while (DebugGameStats.doStatisticsUpdate)
		{
			DebugGameStats.deltaTextureMemory += (long)(Texture.currentTextureMemory - (ulong)DebugGameStats.m_textureMemoryPrevFrame);
			DebugGameStats.m_textureMemoryPrevFrame = (long)Texture.currentTextureMemory;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06009FE1 RID: 40929 RVA: 0x003C5080 File Offset: 0x003C3280
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator UpdateStatisticsCo(DebugGameStats.StatisticsUpdatedCallback callback)
	{
		long num = 0L;
		while (DebugGameStats.doStatisticsUpdate)
		{
			DebugGameStats.statisticsDictionary["gamestats.ingametime"] = (GameTimer.Instance.ticksSincePlayfieldLoaded / 20UL).ToString();
			if (DebugGameStats.m_memorySampler != null)
			{
				DebugGameStats.m_memorySampler.Sample();
				if (DebugGameStats.m_memoryGameUsedStat != null && DebugGameStats.m_memoryGameUsedStat.TryGet(MemoryStatColumn.Current, out num))
				{
					DebugGameStats.statisticsDictionary["gamestats.nativegameused"] = (num / 1024L).ToString();
				}
			}
			DebugGameStats.statisticsDictionary["gamestats.entityinstances"] = Entity.InstanceCount.ToString();
			DebugGameStats.statisticsDictionary["gamestats.maxusedchunks"] = Chunk.InstanceCount.ToString();
			DebugGameStats.statisticsDictionary["gamestats.displayedprefabs"] = GameManager.Instance.prefabLODManager.displayedPrefabs.Count.ToString();
			long currentTextureMemory = (long)Texture.currentTextureMemory;
			DebugGameStats.statisticsDictionary["gamestats.texturememorydelta60"] = (DebugGameStats.deltaTextureMemory / 1024L).ToString();
			DebugGameStats.deltaTextureMemory = 0L;
			DebugGameStats.statisticsDictionary["gamestats.texturememorycurrent"] = (currentTextureMemory / 1024L).ToString();
			DebugGameStats.statisticsDictionary["gamestats.textureMemorydesired"] = (Texture.desiredTextureMemory / 1024UL).ToString();
			Log.Out(string.Concat(new string[]
			{
				"60sec delta: ",
				DebugGameStats.statisticsDictionary["gamestats.texturememorydelta60"],
				",current: ",
				DebugGameStats.statisticsDictionary["gamestats.texturememorycurrent"],
				",desired: ",
				DebugGameStats.statisticsDictionary["gamestats.textureMemorydesired"]
			}));
			int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode);
			if (@int - 1 <= 1)
			{
				DebugGameStats.statisticsDictionary["gamestats.VideoScalingSetting"] = string.Format("{0}, Quality preset {1}, FSR preset {2}", GameOptionsPlatforms.UpscalerMode.ToString(@int), GamePrefs.GetInt(EnumGamePrefs.OptionsGfxQualityPreset), GamePrefs.GetInt(EnumGamePrefs.OptionsGfxFSRPreset));
			}
			else
			{
				DebugGameStats.statisticsDictionary["gamestats.VideoScalingSetting"] = string.Format("{0}, Quality preset {1}", GameOptionsPlatforms.UpscalerMode.ToString(@int), GamePrefs.GetInt(EnumGamePrefs.OptionsGfxQualityPreset));
			}
			if (GameManager.Instance.World != null)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
				{
					DebugGameStats.statisticsDictionary["gamestats.ConnectionStatus"] = "Connected";
				}
				else
				{
					DebugGameStats.statisticsDictionary["gamestats.ConnectionStatus"] = "Disconnected";
				}
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsSinglePlayer)
				{
					DebugGameStats.statisticsDictionary["gamestats.HostStatus"] = "SinglePlayer";
				}
				else if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					DebugGameStats.statisticsDictionary["gamestats.HostStatus"] = "MultiplayerHostOrServer";
				}
				else if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					DebugGameStats.statisticsDictionary["gamestats.HostStatus"] = "Client";
				}
				else
				{
					DebugGameStats.statisticsDictionary["gamestats.HostStatus"] = "Unknown";
				}
				Dictionary<string, string> dictionary = DebugGameStats.statisticsDictionary;
				string text = "gamestats.gameMode";
				GameMode gameMode = GameManager.Instance.GetGameStateManager().GetGameMode();
				dictionary[text] = ((gameMode != null) ? gameMode.GetName() : null);
				DebugGameStats.statisticsDictionary["gamestats.PlayerCount"] = string.Format("Clients: {0}", SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount());
				DebugGameStats.statisticsDictionary["gamestats.worldentities"] = GameManager.Instance.World.Entities.Count.ToString();
				DebugGameStats.statisticsDictionary["gamestats.chunkobservers"] = GameManager.Instance.World.m_ChunkManager.m_ObservedEntities.Count.ToString();
				DebugGameStats.statisticsDictionary["gamestats.syncedchunks"] = GameManager.Instance.World.ChunkCache.chunks.list.Count.ToString();
				DebugGameStats.statisticsDictionary["gamestats.chunkgameobjects"] = GameManager.Instance.World.m_ChunkManager.GetDisplayedChunkGameObjectsCount().ToString();
				DebugGameStats.statisticsDictionary["gamestats.worldtime"] = ValueDisplayFormatters.WorldTime(GameManager.Instance.World.worldTime, "Day {0}, {1:00}:{2:00}");
				DebugGameStats.statisticsDictionary["gamestats.isbloodmoon"] = GameManager.Instance.World.isEventBloodMoon.ToString();
				DebugGameStats.statisticsDictionary["gamestats.SandboxPreset"] = GamePrefs.GetString(EnumGamePrefs.SandboxPreset);
				DebugGameStats.statisticsDictionary["gamestats.SandboxCode"] = GamePrefs.GetString(EnumGamePrefs.SandboxCode);
				EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				if (primaryPlayer)
				{
					DebugGameStats.statisticsDictionary["gamestats.currentPlayerBiome"] = string.Format("Player biome {0}", primaryPlayer.biomeStandingOn);
				}
				if (WeatherManager.Instance)
				{
					DebugGameStats.statisticsDictionary["gamestats.weathersystemstatus"] = "WM status: " + WeatherManager.Instance.ToString();
				}
				if (GameManager.Instance.World.GetLocalPlayers().Count > 0)
				{
					if (!DiscordManager.Instance.IsInitialized)
					{
						DebugGameStats.statisticsDictionary["gamestats.discordstatus"] = (DiscordManager.Instance.Settings.DiscordDisabled ? "Disabled and not initialized" : "Not initialized");
					}
					else if (!DiscordManager.Instance.IsReady)
					{
						DebugGameStats.statisticsDictionary["gamestats.discordstatus"] = (DiscordManager.Instance.Settings.DiscordDisabled ? "Disabled and not ready" : "Not ready");
					}
					else
					{
						DebugGameStats.statisticsDictionary["gamestats.discordstatus"] = "Discord enabled initialized and ready";
						DebugGameStats.statisticsDictionary["gamestats.discordsettings"] = SdPlayerPrefs.GetString("DiscordSettings");
					}
					DebugGameStats.statisticsDictionary["gamestats.localplayerpos"] = GameManager.Instance.World.GetLocalPlayers()[0].position.ToString();
				}
			}
			Log.Out("[Backtrace] Updated Statistics");
			callback(DebugGameStats.statisticsDictionary);
			yield return new WaitForSeconds(60f);
		}
		yield break;
	}

	// Token: 0x04007913 RID: 30995
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> statisticsDictionary = new Dictionary<string, string>();

	// Token: 0x04007914 RID: 30996
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemorySampler m_memorySampler;

	// Token: 0x04007915 RID: 30997
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemoryStat<long> m_memoryGameUsedStat;

	// Token: 0x04007916 RID: 30998
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool doStatisticsUpdate = false;

	// Token: 0x04007917 RID: 30999
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateStatsCoroutine;

	// Token: 0x04007918 RID: 31000
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateDeltasCoroutine;

	// Token: 0x04007919 RID: 31001
	[PublicizedFrom(EAccessModifier.Private)]
	public static long deltaTextureMemory = 0L;

	// Token: 0x0400791A RID: 31002
	[PublicizedFrom(EAccessModifier.Private)]
	public static long m_textureMemoryPrevFrame = 0L;

	// Token: 0x020013E6 RID: 5094
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Statistics
	{
		// Token: 0x0400791B RID: 31003
		public const string InGameTimeKey = "gamestats.ingametime";

		// Token: 0x0400791C RID: 31004
		public const string ManagedHeapSizeKey = "gamestats.managedheap";

		// Token: 0x0400791D RID: 31005
		public const string NativeGameUsedKey = "gamestats.nativegameused";

		// Token: 0x0400791E RID: 31006
		public const string TextureMemoryDesiredKey = "gamestats.textureMemorydesired";

		// Token: 0x0400791F RID: 31007
		public const string TextureMemoryCurrentKey = "gamestats.texturememorycurrent";

		// Token: 0x04007920 RID: 31008
		public const string TextureMemoryDelta60Key = "gamestats.texturememorydelta60";

		// Token: 0x04007921 RID: 31009
		public const string WorldEntitiesCountKey = "gamestats.worldentities";

		// Token: 0x04007922 RID: 31010
		public const string EntityInstanceCountKey = "gamestats.entityinstances";

		// Token: 0x04007923 RID: 31011
		public const string ChunkObserverCountKey = "gamestats.chunkobservers";

		// Token: 0x04007924 RID: 31012
		public const string MaxUsedChunkCountKey = "gamestats.maxusedchunks";

		// Token: 0x04007925 RID: 31013
		public const string SyncedChunkCountKey = "gamestats.syncedchunks";

		// Token: 0x04007926 RID: 31014
		public const string ChunkGameObjectCountKey = "gamestats.chunkgameobjects";

		// Token: 0x04007927 RID: 31015
		public const string DisplayedPrefabCountKey = "gamestats.displayedprefabs";

		// Token: 0x04007928 RID: 31016
		public const string LocalPlayerPositionKey = "gamestats.localplayerpos";

		// Token: 0x04007929 RID: 31017
		public const string WorldTimeKey = "gamestats.worldtime";

		// Token: 0x0400792A RID: 31018
		public const string BloodMoonKey = "gamestats.isbloodmoon";

		// Token: 0x0400792B RID: 31019
		public const string GameModeKey = "gamestats.gameMode";

		// Token: 0x0400792C RID: 31020
		public const string PlayerCountKey = "gamestats.PlayerCount";

		// Token: 0x0400792D RID: 31021
		public const string ConnectStatusKey = "gamestats.ConnectionStatus";

		// Token: 0x0400792E RID: 31022
		public const string HostStatusKey = "gamestats.HostStatus";

		// Token: 0x0400792F RID: 31023
		public const string CurrentBiomeKey = "gamestats.currentPlayerBiome";

		// Token: 0x04007930 RID: 31024
		public const string WeatherStatusKey = "gamestats.weathersystemstatus";

		// Token: 0x04007931 RID: 31025
		public const string VideoScalingSetting = "gamestats.VideoScalingSetting";

		// Token: 0x04007932 RID: 31026
		public const string DiscordPluginStatusKey = "gamestats.discordstatus";

		// Token: 0x04007933 RID: 31027
		public const string DiscordPluginSettingsKey = "gamestats.discordsettings";

		// Token: 0x04007934 RID: 31028
		public const string SandboxPresetKey = "gamestats.SandboxPreset";

		// Token: 0x04007935 RID: 31029
		public const string SandboxCodeKey = "gamestats.SandboxCode";
	}

	// Token: 0x020013E7 RID: 5095
	// (Invoke) Token: 0x06009FE4 RID: 40932
	public delegate void StatisticsUpdatedCallback(Dictionary<string, string> statisticsDictionary);
}
