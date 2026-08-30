using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Platform;
using UnityEngine;

// Token: 0x020013C5 RID: 5061
public static class DebugGameStats
{
	// Token: 0x06009F30 RID: 40752 RVA: 0x003C0288 File Offset: 0x003BE488
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

	// Token: 0x06009F31 RID: 40753 RVA: 0x003C034C File Offset: 0x003BE54C
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

	// Token: 0x06009F32 RID: 40754 RVA: 0x003C03E1 File Offset: 0x003BE5E1
	public static void StopStatisticsUpdate()
	{
		DebugGameStats.doStatisticsUpdate = false;
		DebugGameStats.updateStatsCoroutine = null;
		DebugGameStats.updateDeltasCoroutine = null;
	}

	// Token: 0x06009F33 RID: 40755 RVA: 0x003C03F8 File Offset: 0x003BE5F8
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

	// Token: 0x06009F34 RID: 40756 RVA: 0x003C046C File Offset: 0x003BE66C
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

	// Token: 0x06009F35 RID: 40757 RVA: 0x003C04DC File Offset: 0x003BE6DC
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

	// Token: 0x06009F36 RID: 40758 RVA: 0x003C04E4 File Offset: 0x003BE6E4
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

	// Token: 0x04007898 RID: 30872
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> statisticsDictionary = new Dictionary<string, string>();

	// Token: 0x04007899 RID: 30873
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemorySampler m_memorySampler;

	// Token: 0x0400789A RID: 30874
	[PublicizedFrom(EAccessModifier.Private)]
	public static IPlatformMemoryStat<long> m_memoryGameUsedStat;

	// Token: 0x0400789B RID: 30875
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool doStatisticsUpdate = false;

	// Token: 0x0400789C RID: 30876
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateStatsCoroutine;

	// Token: 0x0400789D RID: 30877
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine updateDeltasCoroutine;

	// Token: 0x0400789E RID: 30878
	[PublicizedFrom(EAccessModifier.Private)]
	public static long deltaTextureMemory = 0L;

	// Token: 0x0400789F RID: 30879
	[PublicizedFrom(EAccessModifier.Private)]
	public static long m_textureMemoryPrevFrame = 0L;

	// Token: 0x020013C6 RID: 5062
	[PublicizedFrom(EAccessModifier.Private)]
	public static class Statistics
	{
		// Token: 0x040078A0 RID: 30880
		public const string InGameTimeKey = "gamestats.ingametime";

		// Token: 0x040078A1 RID: 30881
		public const string ManagedHeapSizeKey = "gamestats.managedheap";

		// Token: 0x040078A2 RID: 30882
		public const string NativeGameUsedKey = "gamestats.nativegameused";

		// Token: 0x040078A3 RID: 30883
		public const string TextureMemoryDesiredKey = "gamestats.textureMemorydesired";

		// Token: 0x040078A4 RID: 30884
		public const string TextureMemoryCurrentKey = "gamestats.texturememorycurrent";

		// Token: 0x040078A5 RID: 30885
		public const string TextureMemoryDelta60Key = "gamestats.texturememorydelta60";

		// Token: 0x040078A6 RID: 30886
		public const string WorldEntitiesCountKey = "gamestats.worldentities";

		// Token: 0x040078A7 RID: 30887
		public const string EntityInstanceCountKey = "gamestats.entityinstances";

		// Token: 0x040078A8 RID: 30888
		public const string ChunkObserverCountKey = "gamestats.chunkobservers";

		// Token: 0x040078A9 RID: 30889
		public const string MaxUsedChunkCountKey = "gamestats.maxusedchunks";

		// Token: 0x040078AA RID: 30890
		public const string SyncedChunkCountKey = "gamestats.syncedchunks";

		// Token: 0x040078AB RID: 30891
		public const string ChunkGameObjectCountKey = "gamestats.chunkgameobjects";

		// Token: 0x040078AC RID: 30892
		public const string DisplayedPrefabCountKey = "gamestats.displayedprefabs";

		// Token: 0x040078AD RID: 30893
		public const string LocalPlayerPositionKey = "gamestats.localplayerpos";

		// Token: 0x040078AE RID: 30894
		public const string WorldTimeKey = "gamestats.worldtime";

		// Token: 0x040078AF RID: 30895
		public const string BloodMoonKey = "gamestats.isbloodmoon";

		// Token: 0x040078B0 RID: 30896
		public const string GameModeKey = "gamestats.gameMode";

		// Token: 0x040078B1 RID: 30897
		public const string PlayerCountKey = "gamestats.PlayerCount";

		// Token: 0x040078B2 RID: 30898
		public const string ConnectStatusKey = "gamestats.ConnectionStatus";

		// Token: 0x040078B3 RID: 30899
		public const string HostStatusKey = "gamestats.HostStatus";

		// Token: 0x040078B4 RID: 30900
		public const string CurrentBiomeKey = "gamestats.currentPlayerBiome";

		// Token: 0x040078B5 RID: 30901
		public const string WeatherStatusKey = "gamestats.weathersystemstatus";

		// Token: 0x040078B6 RID: 30902
		public const string VideoScalingSetting = "gamestats.VideoScalingSetting";

		// Token: 0x040078B7 RID: 30903
		public const string DiscordPluginStatusKey = "gamestats.discordstatus";

		// Token: 0x040078B8 RID: 30904
		public const string DiscordPluginSettingsKey = "gamestats.discordsettings";

		// Token: 0x040078B9 RID: 30905
		public const string SandboxPresetKey = "gamestats.SandboxPreset";

		// Token: 0x040078BA RID: 30906
		public const string SandboxCodeKey = "gamestats.SandboxCode";
	}

	// Token: 0x020013C7 RID: 5063
	// (Invoke) Token: 0x06009F39 RID: 40761
	public delegate void StatisticsUpdatedCallback(Dictionary<string, string> statisticsDictionary);
}
