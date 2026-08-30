using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Platform;
using UnityEngine;

// Token: 0x020013AA RID: 5034
public static class DebugGameStats
{
	// Token: 0x06009E33 RID: 40499 RVA: 0x003BAF44 File Offset: 0x003B9144
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

	// Token: 0x06009E34 RID: 40500 RVA: 0x003BB008 File Offset: 0x003B9208
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

	// Token: 0x06009E35 RID: 40501 RVA: 0x003BB09D File Offset: 0x003B929D
	public static void StopStatisticsUpdate()
	{
		DebugGameStats.doStatisticsUpdate = false;
		DebugGameStats.updateStatsCoroutine = null;
		DebugGameStats.updateDeltasCoroutine = null;
	}

	// Token: 0x06009E36 RID: 40502 RVA: 0x003BB0B4 File Offset: 0x003B92B4
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

	// Token: 0x06009E37 RID: 40503 RVA: 0x003BB128 File Offset: 0x003B9328
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

	// Token: 0x06009E38 RID: 40504 RVA: 0x003BB198 File Offset: 0x003B9398
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

	// Token: 0x06009E39 RID: 40505 RVA: 0x003BB1A0 File Offset: 0x003B93A0
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

	// Token: 0x040077BF RID: 30655
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> statisticsDictionary = new Dictionary<string, string>();

	// Token: 0x040077C0 RID: 30656
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemorySampler m_memorySampler;

	// Token: 0x040077C1 RID: 30657
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemoryStat<long> m_memoryGameUsedStat;

	// Token: 0x040077C2 RID: 30658
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool doStatisticsUpdate = false;

	// Token: 0x040077C3 RID: 30659
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateStatsCoroutine;

	// Token: 0x040077C4 RID: 30660
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateDeltasCoroutine;

	// Token: 0x040077C5 RID: 30661
	[PublicizedFrom(EAccessModifier.Private)]
	public static long deltaTextureMemory = 0L;

	// Token: 0x040077C6 RID: 30662
	[PublicizedFrom(EAccessModifier.Private)]
	public static long m_textureMemoryPrevFrame = 0L;

	// Token: 0x020013AB RID: 5035
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Statistics
	{
		// Token: 0x040077C7 RID: 30663
		public const string InGameTimeKey = "gamestats.ingametime";

		// Token: 0x040077C8 RID: 30664
		public const string ManagedHeapSizeKey = "gamestats.managedheap";

		// Token: 0x040077C9 RID: 30665
		public const string NativeGameUsedKey = "gamestats.nativegameused";

		// Token: 0x040077CA RID: 30666
		public const string TextureMemoryDesiredKey = "gamestats.textureMemorydesired";

		// Token: 0x040077CB RID: 30667
		public const string TextureMemoryCurrentKey = "gamestats.texturememorycurrent";

		// Token: 0x040077CC RID: 30668
		public const string TextureMemoryDelta60Key = "gamestats.texturememorydelta60";

		// Token: 0x040077CD RID: 30669
		public const string WorldEntitiesCountKey = "gamestats.worldentities";

		// Token: 0x040077CE RID: 30670
		public const string EntityInstanceCountKey = "gamestats.entityinstances";

		// Token: 0x040077CF RID: 30671
		public const string ChunkObserverCountKey = "gamestats.chunkobservers";

		// Token: 0x040077D0 RID: 30672
		public const string MaxUsedChunkCountKey = "gamestats.maxusedchunks";

		// Token: 0x040077D1 RID: 30673
		public const string SyncedChunkCountKey = "gamestats.syncedchunks";

		// Token: 0x040077D2 RID: 30674
		public const string ChunkGameObjectCountKey = "gamestats.chunkgameobjects";

		// Token: 0x040077D3 RID: 30675
		public const string DisplayedPrefabCountKey = "gamestats.displayedprefabs";

		// Token: 0x040077D4 RID: 30676
		public const string LocalPlayerPositionKey = "gamestats.localplayerpos";

		// Token: 0x040077D5 RID: 30677
		public const string WorldTimeKey = "gamestats.worldtime";

		// Token: 0x040077D6 RID: 30678
		public const string BloodMoonKey = "gamestats.isbloodmoon";

		// Token: 0x040077D7 RID: 30679
		public const string GameModeKey = "gamestats.gameMode";

		// Token: 0x040077D8 RID: 30680
		public const string PlayerCountKey = "gamestats.PlayerCount";

		// Token: 0x040077D9 RID: 30681
		public const string ConnectStatusKey = "gamestats.ConnectionStatus";

		// Token: 0x040077DA RID: 30682
		public const string HostStatusKey = "gamestats.HostStatus";

		// Token: 0x040077DB RID: 30683
		public const string CurrentBiomeKey = "gamestats.currentPlayerBiome";

		// Token: 0x040077DC RID: 30684
		public const string WeatherStatusKey = "gamestats.weathersystemstatus";

		// Token: 0x040077DD RID: 30685
		public const string VideoScalingSetting = "gamestats.VideoScalingSetting";

		// Token: 0x040077DE RID: 30686
		public const string DiscordPluginStatusKey = "gamestats.discordstatus";

		// Token: 0x040077DF RID: 30687
		public const string DiscordPluginSettingsKey = "gamestats.discordsettings";

		// Token: 0x040077E0 RID: 30688
		public const string SandboxPresetKey = "gamestats.SandboxPreset";

		// Token: 0x040077E1 RID: 30689
		public const string SandboxCodeKey = "gamestats.SandboxCode";
	}

	// Token: 0x020013AC RID: 5036
	// (Invoke) Token: 0x06009E3C RID: 40508
	public delegate void StatisticsUpdatedCallback(Dictionary<string, string> statisticsDictionary);
}
