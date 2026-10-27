using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Platform;
using UnityEngine;

// Token: 0x020013F7 RID: 5111
public static class DebugGameStats
{
	// Token: 0x0600A033 RID: 41011 RVA: 0x003C7228 File Offset: 0x003C5428
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

	// Token: 0x0600A034 RID: 41012 RVA: 0x003C72EC File Offset: 0x003C54EC
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

	// Token: 0x0600A035 RID: 41013 RVA: 0x003C7381 File Offset: 0x003C5581
	public static void StopStatisticsUpdate()
	{
		DebugGameStats.doStatisticsUpdate = false;
		DebugGameStats.updateStatsCoroutine = null;
		DebugGameStats.updateDeltasCoroutine = null;
	}

	// Token: 0x0600A036 RID: 41014 RVA: 0x003C7398 File Offset: 0x003C5598
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

	// Token: 0x0600A037 RID: 41015 RVA: 0x003C740C File Offset: 0x003C560C
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

	// Token: 0x0600A038 RID: 41016 RVA: 0x003C747C File Offset: 0x003C567C
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

	// Token: 0x0600A039 RID: 41017 RVA: 0x003C7484 File Offset: 0x003C5684
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

	// Token: 0x04007954 RID: 31060
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> statisticsDictionary = new Dictionary<string, string>();

	// Token: 0x04007955 RID: 31061
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemorySampler m_memorySampler;

	// Token: 0x04007956 RID: 31062
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemoryStat<long> m_memoryGameUsedStat;

	// Token: 0x04007957 RID: 31063
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool doStatisticsUpdate = false;

	// Token: 0x04007958 RID: 31064
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateStatsCoroutine;

	// Token: 0x04007959 RID: 31065
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateDeltasCoroutine;

	// Token: 0x0400795A RID: 31066
	[PublicizedFrom(EAccessModifier.Private)]
	public static long deltaTextureMemory = 0L;

	// Token: 0x0400795B RID: 31067
	[PublicizedFrom(EAccessModifier.Private)]
	public static long m_textureMemoryPrevFrame = 0L;

	// Token: 0x020013F8 RID: 5112
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Statistics
	{
		// Token: 0x0400795C RID: 31068
		public const string InGameTimeKey = "gamestats.ingametime";

		// Token: 0x0400795D RID: 31069
		public const string ManagedHeapSizeKey = "gamestats.managedheap";

		// Token: 0x0400795E RID: 31070
		public const string NativeGameUsedKey = "gamestats.nativegameused";

		// Token: 0x0400795F RID: 31071
		public const string TextureMemoryDesiredKey = "gamestats.textureMemorydesired";

		// Token: 0x04007960 RID: 31072
		public const string TextureMemoryCurrentKey = "gamestats.texturememorycurrent";

		// Token: 0x04007961 RID: 31073
		public const string TextureMemoryDelta60Key = "gamestats.texturememorydelta60";

		// Token: 0x04007962 RID: 31074
		public const string WorldEntitiesCountKey = "gamestats.worldentities";

		// Token: 0x04007963 RID: 31075
		public const string EntityInstanceCountKey = "gamestats.entityinstances";

		// Token: 0x04007964 RID: 31076
		public const string ChunkObserverCountKey = "gamestats.chunkobservers";

		// Token: 0x04007965 RID: 31077
		public const string MaxUsedChunkCountKey = "gamestats.maxusedchunks";

		// Token: 0x04007966 RID: 31078
		public const string SyncedChunkCountKey = "gamestats.syncedchunks";

		// Token: 0x04007967 RID: 31079
		public const string ChunkGameObjectCountKey = "gamestats.chunkgameobjects";

		// Token: 0x04007968 RID: 31080
		public const string DisplayedPrefabCountKey = "gamestats.displayedprefabs";

		// Token: 0x04007969 RID: 31081
		public const string LocalPlayerPositionKey = "gamestats.localplayerpos";

		// Token: 0x0400796A RID: 31082
		public const string WorldTimeKey = "gamestats.worldtime";

		// Token: 0x0400796B RID: 31083
		public const string BloodMoonKey = "gamestats.isbloodmoon";

		// Token: 0x0400796C RID: 31084
		public const string GameModeKey = "gamestats.gameMode";

		// Token: 0x0400796D RID: 31085
		public const string PlayerCountKey = "gamestats.PlayerCount";

		// Token: 0x0400796E RID: 31086
		public const string ConnectStatusKey = "gamestats.ConnectionStatus";

		// Token: 0x0400796F RID: 31087
		public const string HostStatusKey = "gamestats.HostStatus";

		// Token: 0x04007970 RID: 31088
		public const string CurrentBiomeKey = "gamestats.currentPlayerBiome";

		// Token: 0x04007971 RID: 31089
		public const string WeatherStatusKey = "gamestats.weathersystemstatus";

		// Token: 0x04007972 RID: 31090
		public const string VideoScalingSetting = "gamestats.VideoScalingSetting";

		// Token: 0x04007973 RID: 31091
		public const string DiscordPluginStatusKey = "gamestats.discordstatus";

		// Token: 0x04007974 RID: 31092
		public const string DiscordPluginSettingsKey = "gamestats.discordsettings";

		// Token: 0x04007975 RID: 31093
		public const string SandboxPresetKey = "gamestats.SandboxPreset";

		// Token: 0x04007976 RID: 31094
		public const string SandboxCodeKey = "gamestats.SandboxCode";
	}

	// Token: 0x020013F9 RID: 5113
	// (Invoke) Token: 0x0600A03C RID: 41020
	public delegate void StatisticsUpdatedCallback(Dictionary<string, string> statisticsDictionary);
}
