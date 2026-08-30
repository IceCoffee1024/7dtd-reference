using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Platform;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200171D RID: 5917
	[BurstCompile(CompileSynchronously = true)]
	public class WorldBuilder
	{
		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x0600B7A0 RID: 47008 RVA: 0x00447B1B File Offset: 0x00445D1B
		public int HalfWorldSize
		{
			get
			{
				return this.WorldSize / 2;
			}
		}

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x0600B7A1 RID: 47009 RVA: 0x00447B25 File Offset: 0x00445D25
		public Vector3i PrefabWorldOffset
		{
			get
			{
				return new Vector3i(-this.HalfWorldSize, 0, -this.HalfWorldSize);
			}
		}

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x0600B7A2 RID: 47010 RVA: 0x00447B3B File Offset: 0x00445D3B
		public long SerializedSize
		{
			get
			{
				if (!SaveInfoProvider.DataLimitEnabled || !this.StorageLocation.UsesDataLimit())
				{
					return 0L;
				}
				return this.serializedTotalSize;
			}
		}

		// Token: 0x0600B7A3 RID: 47011 RVA: 0x00447B5C File Offset: 0x00445D5C
		public WorldBuilder(string _seed, int _worldSize, UserDataStorageType _storageLocation)
		{
			this.WorldSeedName = _seed;
			this.WorldSize = _worldSize;
			this.WorldName = WorldBuilder.GetGeneratedWorldName(this.WorldSeedName, this.WorldSize);
			this.StorageLocation = _storageLocation;
			this.WorldPath = WorldBuilder.GetWorldPath(_storageLocation, this.WorldName);
			this.WorldSizeDistDiv = ((this.WorldSize > 4500) ? 1 : ((this.WorldSize > 3500) ? 2 : ((this.WorldSize > 2500) ? 3 : 4)));
			this.DistrictPlanner = new DistrictPlanner(this);
			this.HighwayPlanner = new HighwayPlanner(this);
			this.PathingUtils = new PathingUtils(this);
			this.PathShared = new PathShared(this);
			this.POISmoother = new POISmoother(this);
			this.PrefabManager = new PrefabManager(this);
			this.StampManager = new StampManager(this);
			this.StreetTileShared = new StreetTileShared(this);
			this.TownPlanner = new TownPlanner(this);
			this.TownshipShared = new TownshipShared(this);
			this.WildernessPathPlanner = new WildernessPathPlanner(this);
			this.WildernessPlanner = new WildernessPlanner(this);
			List<ValueTuple<string, string, Action<Stream>>> list = new List<ValueTuple<string, string, Action<Stream>>>();
			List<ValueTuple<string, string, Func<Stream, IEnumerator>>> list2 = new List<ValueTuple<string, string, Func<Stream, IEnumerator>>>();
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiBiomes", "biomes.png", delegate(Stream stream)
			{
				stream.Write(ImageConversion.EncodeArrayToPNG(this.biomeDest, GraphicsFormat.R8G8B8A8_UNorm, (uint)this.BiomeSize, (uint)this.BiomeSize, (uint)(this.BiomeSize * 4)));
			}));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiRadiation", "radiation.png", delegate(Stream stream)
			{
				stream.Write(ImageConversion.EncodeArrayToPNG(this.radDest, GraphicsFormat.R8G8B8A8_UNorm, (uint)this.RadSize, (uint)this.RadSize, (uint)(this.RadSize * 4)));
			}));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiRoads", "splat3.png", delegate(Stream stream)
			{
				stream.Write(ImageConversion.EncodeArrayToPNG(this.roadDest, GraphicsFormat.R8G8B8A8_UNorm, (uint)this.WorldSize, (uint)this.WorldSize, (uint)(this.WorldSize * 4)));
			}));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiWater", "splat4.png", new Action<Stream>(this.SerializeWater)));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiHeightmap", "dtm.raw", new Action<Stream>(this.serializeRawHeightmap)));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiPrefabs", "prefabs.xml", new Action<Stream>(this.serializePrefabs)));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiPlayerSpawns", "spawnpoints.xml", new Action<Stream>(this.serializePlayerSpawns)));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiLevelMetadata", "main.ttw", new Action<Stream>(this.serializeRWGTTW)));
			list.Add(new ValueTuple<string, string, Action<Stream>>("xuiMapInfo", "map_info.xml", new Action<Stream>(this.serializeDynamicProperties)));
			this.threadedSerializers = list.ToArray();
			this.mainThreadSerializers = list2.ToArray();
			this.threadedSerializerBuffers = new MemoryStream[this.threadedSerializers.Length];
			this.mainThreadSerializerBuffers = new MemoryStream[this.mainThreadSerializers.Length];
		}

		// Token: 0x0600B7A4 RID: 47012 RVA: 0x00447F9C File Offset: 0x0044619C
		public WorldBuilder(int _worldSize)
		{
			this.WorldSize = _worldSize;
			this.data.Init(_worldSize);
			this.PathingUtils = new PathingUtils(this);
			this.RadSize = this.WorldSize / 32;
			this.radDest = new Color32[this.RadSize * this.RadSize];
			this.StreetTileShared = new StreetTileShared(this);
			this.InitStreetTiles();
		}

		// Token: 0x0600B7A5 RID: 47013 RVA: 0x004481BB File Offset: 0x004463BB
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator Init()
		{
			if (PlatformOptimizations.RestartAfterRwg)
			{
				PlatformApplicationManager.SetRestartRequired();
			}
			this.LocalizationInit();
			this.data.Init(this.WorldSize);
			int num = this.WorldSize * this.WorldSize;
			this.roadDest = new Color32[num];
			this.RadSize = this.WorldSize / 32;
			this.radDest = new Color32[this.RadSize * this.RadSize];
			yield return this.StampManager.LoadStamps();
			this.PrefabManager.PrefabInstanceId = 0;
			this.playerSpawns = new List<WorldBuilder.PlayerSpawn>();
			foreach (KeyValuePair<string, Vector2i> keyValuePair in WorldBuilderStatic.WorldSizeMapper)
			{
				string text;
				Vector2i vector2i;
				keyValuePair.Deconstruct(out text, out vector2i);
				string text2 = text;
				Vector2i vector2i2 = vector2i;
				if (this.WorldSize >= vector2i2.x && this.WorldSize < vector2i2.y)
				{
					this.worldSizeName = text2;
				}
			}
			if (this.worldSizeName == null)
			{
				Log.Error(string.Format("There was an error finding rwgmixer world entry for the current world size! WorldSize: {0}/n Please make sure that the world size falls within the min/max ranges listed in xml.", this.WorldSize));
				yield break;
			}
			this.thisWorldProperties = WorldBuilderStatic.Properties[this.worldSizeName];
			this.Seed = this.WorldSeedName.GetHashCode() + this.WorldSize;
			Rand.Instance.SetSeed(this.Seed);
			this.biomeColors[BiomeType.forest] = WorldBuilderConstants.forestCol;
			this.biomeColors[BiomeType.burntForest] = WorldBuilderConstants.burntForestCol;
			this.biomeColors[BiomeType.desert] = WorldBuilderConstants.desertCol;
			this.biomeColors[BiomeType.snow] = WorldBuilderConstants.snowCol;
			this.biomeColors[BiomeType.wasteland] = WorldBuilderConstants.wastelandCol;
			this.biomeColors[BiomeType.waterDebug] = WorldBuilderConstants.waterCol;
			yield break;
		}

		// Token: 0x0600B7A6 RID: 47014 RVA: 0x004481CC File Offset: 0x004463CC
		[PublicizedFrom(EAccessModifier.Private)]
		public void LocalizationInit()
		{
			this.creatingTerrainAndBiomeStamps = Localization.Get("xuiRwgCreatingTerrainAndBiomeStamps", false, null);
			this.creatingTerrainStamps = Localization.Get("xuiRwgCreatingTerrainStamps", false, null);
			this.creatingBiomeStamps = Localization.Get("xuiRwgCreatingBiomeStamps", false, null);
			this.messageGeneratingTerrain = Localization.Get("xuiRwgGeneratingTerrain", false, null);
			this.messageWritingStampsToMap = Localization.Get("xuiRwgWritingStampsToMap", false, null);
			this.messageTerrainGenerationFinished = Localization.Get("xuiRwgTerrainGenerationFinished", false, null);
			this.messageTownPlanning = Localization.Get("xuiRwgTownPlanning", false, null);
			this.messageTownPlanningFinished = Localization.Get("xuiRwgTownPlanningFinished", false, null);
			this.messageCleaningUpWaterMapData = Localization.Get("xuiRwgCleaningUpWaterMapData", false, null);
			this.messageWritingTerrainStampsToMap = Localization.Get("xuiRwgWritingTerrainStampsToMap", false, null);
			this.messageWritingWaterStampsToMap = Localization.Get("xuiRwgWritingWaterStampsToMap", false, null);
			this.messageWritingTerrainAndWaterStampsToMap = Localization.Get("xuiRwgWritingTerrainAndWaterStampsToMap", false, null);
			this.messageSmoothingStreetTiles = Localization.Get("xuiRwgSmoothingStreetTiles", false, null);
			this.messageWildernessPOIs = Localization.Get("xuiRwgWildernessPOIs", false, null);
			this.messageHighways = Localization.Get("xuiRwgHighways", false, null);
			this.messageHighwaysConnections = Localization.Get("xuiRwgHighwaysConnections", false, null);
			this.messageHighwaysTownExitsSelf = Localization.Get("xuiRwgHighwaysTownExitsSelf", false, null);
			this.messageHighwaysTownExitsOther = Localization.Get("xuiRwgHighwaysTownExitsOther", false, null);
			this.messageHighwaysTownship = Localization.Get("xuiRwgHighwaysTownship", false, null);
			this.messageHighwaysTownExits = Localization.Get("xuiRwgHighwaysTownExits", false, null);
			this.messageWildernessPaths = Localization.Get("xuiRwgWildernessPaths", false, null);
			this.messageDrawRoads = Localization.Get("xuiRwgDrawRoads", false, null);
			this.messageDrawRoadsWilderness = Localization.Get("xuiRwgDrawRoadsWilderness", false, null);
			this.messageDrawRoadsProgress = Localization.Get("xuiRwgDrawRoadsProgress", false, null);
			this.messageSmoothRoadTerrainCount = Localization.Get("xuiRwgSmoothRoadTerrainCount", false, null);
			this.messageSmoothRoadTerrain = Localization.Get("xuiRwgSmoothRoadTerrain", false, null);
		}

		// Token: 0x0600B7A7 RID: 47015 RVA: 0x004483B0 File Offset: 0x004465B0
		public void SetBiomeWeight(BiomeType _type, int _weight)
		{
			switch (_type)
			{
			case BiomeType.forest:
				this.ForestBiomeWeight = _weight;
				return;
			case BiomeType.burntForest:
				this.BurntForestBiomeWeight = _weight;
				return;
			case BiomeType.desert:
				this.DesertBiomeWeight = _weight;
				return;
			case BiomeType.snow:
				this.SnowBiomeWeight = _weight;
				return;
			case BiomeType.wasteland:
				this.WastelandBiomeWeight = _weight;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600B7A8 RID: 47016 RVA: 0x004483FF File Offset: 0x004465FF
		public static string GetWorldPath(UserDataStorageType _storageType, string _worldName)
		{
			return GameIO.GetUserGameDataDir(_storageType) + "/GeneratedWorlds/" + _worldName + "/";
		}

		// Token: 0x0600B7A9 RID: 47017 RVA: 0x00448417 File Offset: 0x00446617
		public void SetStorageType(UserDataStorageType _storageType)
		{
			this.StorageLocation = _storageType;
			this.WorldPath = WorldBuilder.GetWorldPath(_storageType, this.WorldName);
		}

		// Token: 0x0600B7AA RID: 47018 RVA: 0x00448432 File Offset: 0x00446632
		public IEnumerator GenerateFromServer()
		{
			this.PreviewWindow = null;
			this.totalMS = new MicroStopwatch(true);
			yield return this.GenerateData();
			yield return this.SaveData(WorldBuilder.SaveDataPromptMode.Off, null, null, null, null);
			this.Cleanup();
			this.SetMessage(null, false, false);
			yield break;
		}

		// Token: 0x0600B7AB RID: 47019 RVA: 0x00448441 File Offset: 0x00446641
		public IEnumerator GenerateFromUI()
		{
			this.IsCanceled = false;
			this.IsFinished = false;
			this.totalMS = new MicroStopwatch(true);
			yield return this.SetMessage(Localization.Get("xuiStarting", false, null), false, false);
			yield return new WaitForSeconds(0.1f);
			yield return this.GenerateData();
			yield break;
		}

		// Token: 0x0600B7AC RID: 47020 RVA: 0x00448450 File Offset: 0x00446650
		public IEnumerator FinishForPreview()
		{
			if (!this.IsCanceled)
			{
				yield return this.SetMessage(Localization.Get("xuiRwgGenerationComplete", false, null), true, false);
			}
			else
			{
				yield return this.SetMessage(Localization.Get("xuiRwgGenerationCanceled", false, null), true, true);
			}
			this.IsFinished = true;
			yield break;
		}

		// Token: 0x0600B7AD RID: 47021 RVA: 0x0044845F File Offset: 0x0044665F
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator GenerateData()
		{
			yield return this.Init();
			yield return this.SetMessage(string.Format(Localization.Get("xuiWorldGenerationGenerating", false, null), this.WorldName), true, false);
			Task task = new Task(new Action(this.GenerateTask));
			task.Start();
			do
			{
				yield return new WaitForSeconds(0.1f);
				yield return this.TaskMessageUpdate();
				if (!this.IsCanceled && this.PreviewWindow != null && this.previewStepOfTask > this.previewStep)
				{
					this.previewStep++;
					yield return this.PreviewWindow.ShowPreview(this.previewStep);
				}
			}
			while (!task.IsCompleted || (!this.IsCanceled && this.PreviewWindow != null && this.previewStep < XUiC_WorldGenerationPreview.PreviewStep.Terrain));
			if (task.IsFaulted)
			{
				Log.Error("RWG generation task failed.");
				Log.Exception(task.Exception);
				this.IsCanceled = true;
				yield break;
			}
			if (this.IsCanceled)
			{
				yield break;
			}
			yield return GCUtils.UnloadAndCollectCo();
			yield return this.SerializeData();
			yield return GCUtils.UnloadAndCollectCo();
			Log.Out("RWG final in {0}:{1:00}, r={2:x}", new object[]
			{
				this.totalMS.Elapsed.Minutes,
				this.totalMS.Elapsed.Seconds,
				Rand.Instance.PeekSample()
			});
			yield break;
		}

		// Token: 0x0600B7AE RID: 47022 RVA: 0x00448470 File Offset: 0x00446670
		[PublicizedFrom(EAccessModifier.Private)]
		public void GenerateTask()
		{
			this.GenerateTerrain();
			bool flag = this.Towns != WorldBuilder.GenerationSelections.None || this.Wilderness > WorldBuilder.GenerationSelections.None;
			this.PrefabManager.ClearDisplayed();
			if (flag)
			{
				this.PrefabManager.LoadPrefabs();
				this.PrefabManager.ShufflePrefabData(this.Seed);
				this.PathingUtils.SetupPathingGrid();
			}
			this.InitStreetTiles();
			if (this.Towns != WorldBuilder.GenerationSelections.None)
			{
				this.TownPlanner.Plan(this.thisWorldProperties, this.Seed);
			}
			this.GenerateTerrainLast();
			this.previewStepOfTask = XUiC_WorldGenerationPreview.PreviewStep.Terrain;
			this.POISmoother.SmoothStreetTiles();
			if (this.IsCanceled)
			{
				return;
			}
			if (this.Wilderness != WorldBuilder.GenerationSelections.None)
			{
				this.WildernessPlanner.Plan(this.thisWorldProperties, this.Seed);
				this.SmoothWildernessTerrain();
			}
			if (this.IsCanceled)
			{
				return;
			}
			if (flag)
			{
				this.CalcTownshipsHeightMask();
				this.HighwayPlanner.Plan(this.thisWorldProperties, this.Seed);
				this.TownPlanner.SpawnPrefabs();
			}
			if (this.IsCanceled)
			{
				return;
			}
			if (this.Wilderness != WorldBuilder.GenerationSelections.None)
			{
				this.WildernessPathPlanner.Plan(this.Seed);
			}
			int num = 12 - this.playerSpawns.Count;
			if (num > 0)
			{
				foreach (StreetTile streetTile in this.CalcPlayerSpawnTiles())
				{
					if (this.CreatePlayerSpawn(streetTile.WorldPositionCenter, true) && --num <= 0)
					{
						break;
					}
				}
			}
			if (this.IsCanceled)
			{
				return;
			}
			this.DrawRoads(this.roadDest);
			if (flag)
			{
				this.SetTaskMessage(this.messageSmoothRoadTerrain);
				this.CalcWindernessPOIsHeightMask(this.roadDest);
				this.SmoothRoadTerrain(this.roadDest, this.data.HeightMap, this.WorldSize, this.Townships);
			}
			foreach (Path path in this.highwayPaths)
			{
				path.Cleanup();
			}
			this.highwayPaths.Clear();
			foreach (Path path2 in this.wildernessPaths)
			{
				path2.Cleanup();
			}
			this.wildernessPaths.Clear();
			this.FinalizeWater();
		}

		// Token: 0x0600B7AF RID: 47023 RVA: 0x004486E8 File Offset: 0x004468E8
		[PublicizedFrom(EAccessModifier.Private)]
		public static void ThrowIfTaskFaulted(Task task, string taskName)
		{
			if (task == null || !task.IsFaulted)
			{
				return;
			}
			if (task.Exception != null)
			{
				throw new Exception("RWG task '" + taskName + "' failed.", task.Exception.Flatten());
			}
			throw new Exception("RWG task '" + taskName + "' failed.");
		}

		// Token: 0x0600B7B0 RID: 47024 RVA: 0x0044873F File Offset: 0x0044693F
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator SerializeData()
		{
			if (!SaveInfoProvider.DataLimitEnabled)
			{
				yield break;
			}
			MicroStopwatch totalMs = new MicroStopwatch(true);
			Task[] threadedSerializerTasks = new Task[this.threadedSerializers.Length];
			for (int j = 0; j < this.threadedSerializers.Length; j++)
			{
				WorldBuilder.<>c__DisplayClass123_1 CS$<>8__locals2 = new WorldBuilder.<>c__DisplayClass123_1();
				ValueTuple<string, string, Action<Stream>> valueTuple = this.threadedSerializers[j];
				CS$<>8__locals2.fileName = valueTuple.Item2;
				CS$<>8__locals2.serializer = valueTuple.Item3;
				CS$<>8__locals2.buffer = new MemoryStream();
				this.threadedSerializerBuffers[j] = CS$<>8__locals2.buffer;
				Task task = new Task(new Action(CS$<>8__locals2.<SerializeData>g__SerializeToBuffer|1));
				threadedSerializerTasks[j] = task;
				task.Start();
			}
			int k;
			for (int l = 0; l < this.mainThreadSerializers.Length; l = k + 1)
			{
				WorldBuilder.<>c__DisplayClass123_2 CS$<>8__locals3 = new WorldBuilder.<>c__DisplayClass123_2();
				ValueTuple<string, string, Func<Stream, IEnumerator>> valueTuple2 = this.mainThreadSerializers[l];
				string item = valueTuple2.Item1;
				CS$<>8__locals3.fileName = valueTuple2.Item2;
				CS$<>8__locals3.serializer = valueTuple2.Item3;
				CS$<>8__locals3.buffer = new MemoryStream();
				this.mainThreadSerializerBuffers[l] = CS$<>8__locals3.buffer;
				yield return this.SetMessage(string.Format(Localization.Get("xuiRwgSerializing", false, null), Localization.Get(item, false, null)), false, false);
				yield return ThreadManager.CoroutineWrapperWithExceptionCallback(CS$<>8__locals3.<SerializeData>g__SerializeToBuffer|3(), delegate(Exception ex)
				{
					Log.Error(string.Format("Exception while serializing '{0}': {1}", CS$<>8__locals3.fileName, ex));
				});
				k = l;
			}
			object[] lastTaskNames = null;
			Func<ValueTuple<string, string, Action<Stream>>, int, bool> <>9__4;
			for (;;)
			{
				if (!threadedSerializerTasks.Any<Task>((Task x) => !x.IsCompleted))
				{
					break;
				}
				IEnumerable<ValueTuple<string, string, Action<Stream>>> enumerable = this.threadedSerializers;
				Func<ValueTuple<string, string, Action<Stream>>, int, bool> func;
				if ((func = <>9__4) == null)
				{
					func = (<>9__4 = ([TupleElementNames(new string[] { "langKey", "fileName", "serializer" })] ValueTuple<string, string, Action<Stream>> _, int i) => !threadedSerializerTasks[i].IsCompleted);
				}
				object[] array = (from x in enumerable.Where<ValueTuple<string, string, Action<Stream>>>(func).Take<ValueTuple<string, string, Action<Stream>>>(3)
					select Localization.Get(x.Item1, false, null)).Cast<object>().ToArray<object>();
				if (lastTaskNames != null && array.SequenceEqual<object>(lastTaskNames))
				{
					yield return null;
				}
				else
				{
					lastTaskNames = array;
					yield return this.SetMessage(string.Format(Localization.Get("xuiRwgSerializing", false, null), Localization.FormatListAnd(array)), false, false);
				}
			}
			long num = 0L;
			foreach (MemoryStream memoryStream in this.threadedSerializerBuffers)
			{
				num += memoryStream.Length;
			}
			foreach (MemoryStream memoryStream2 in this.mainThreadSerializerBuffers)
			{
				num += memoryStream2.Length;
			}
			this.serializedTotalSize = num;
			Log.Out(string.Format("RWG SerializeData {0} in {1:F3} s", this.serializedTotalSize.FormatSize(true), totalMs.Elapsed.TotalSeconds));
			yield break;
		}

		// Token: 0x0600B7B1 RID: 47025 RVA: 0x0044874E File Offset: 0x0044694E
		public bool CanSaveData()
		{
			return !SdDirectory.Exists(this.WorldPath);
		}

		// Token: 0x0600B7B2 RID: 47026 RVA: 0x0044875E File Offset: 0x0044695E
		public IEnumerator SaveData(WorldBuilder.SaveDataPromptMode promptMode, XUiController parentController = null, Action onCancel = null, Action onDiscard = null, Action onConfirm = null)
		{
			WorldBuilder.<>c__DisplayClass126_0 CS$<>8__locals1 = new WorldBuilder.<>c__DisplayClass126_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.CanSaveData())
			{
				if (promptMode != WorldBuilder.SaveDataPromptMode.Off)
				{
					XUiC_SaveSpaceNeeded confirmationWindow = XUiC_SaveSpaceNeeded.Open(this.SerializedSize, this.WorldPath, this.StorageLocation, promptMode == WorldBuilder.SaveDataPromptMode.OnInsufficientSpace, onCancel != null, onDiscard != null, null, "xuiRwgSaveWorld", null, null, "xuiSave", null);
					if (confirmationWindow == null)
					{
						if (onConfirm != null)
						{
							onConfirm();
						}
					}
					else
					{
						while (confirmationWindow.IsOpen || confirmationWindow.Result == XUiC_SaveSpaceNeeded.ConfirmationResult.Pending)
						{
							yield return null;
						}
						switch (confirmationWindow.Result)
						{
						case XUiC_SaveSpaceNeeded.ConfirmationResult.Cancelled:
							if (onCancel != null)
							{
								onCancel();
							}
							yield break;
						case XUiC_SaveSpaceNeeded.ConfirmationResult.Discarded:
							if (onDiscard != null)
							{
								onDiscard();
							}
							yield break;
						case XUiC_SaveSpaceNeeded.ConfirmationResult.Confirmed:
							if (onConfirm != null)
							{
								onConfirm();
							}
							break;
						default:
							throw new ArgumentOutOfRangeException();
						}
					}
					confirmationWindow = null;
				}
				this.totalMS.ResetAndRestart();
				SdDirectory.CreateDirectory(this.WorldPath);
				CS$<>8__locals1.threadedSaveTasks = new Task[this.threadedSerializers.Length];
				for (int j = 0; j < this.threadedSerializers.Length; j++)
				{
					WorldBuilder.<>c__DisplayClass126_1 CS$<>8__locals2 = new WorldBuilder.<>c__DisplayClass126_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.buffer = this.threadedSerializerBuffers[j];
					ValueTuple<string, string, Action<Stream>> valueTuple = this.threadedSerializers[j];
					CS$<>8__locals2.fileName = valueTuple.Item2;
					CS$<>8__locals2.serializer = valueTuple.Item3;
					Task task = new Task(new Action(CS$<>8__locals2.<SaveData>g__SaveToFile|2));
					CS$<>8__locals2.CS$<>8__locals1.threadedSaveTasks[j] = task;
					task.Start();
				}
				int num;
				for (int k = 0; k < this.mainThreadSerializers.Length; k = num + 1)
				{
					WorldBuilder.<>c__DisplayClass126_2 CS$<>8__locals3 = new WorldBuilder.<>c__DisplayClass126_2();
					CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
					CS$<>8__locals3.buffer = this.mainThreadSerializerBuffers[k];
					ValueTuple<string, string, Func<Stream, IEnumerator>> valueTuple2 = this.mainThreadSerializers[k];
					string item = valueTuple2.Item1;
					CS$<>8__locals3.fileName = valueTuple2.Item2;
					CS$<>8__locals3.serializer = valueTuple2.Item3;
					yield return this.SetMessage(string.Format(Localization.Get("xuiRwgSaving", false, null), Localization.Get(item, false, null)), false, false);
					yield return ThreadManager.CoroutineWrapperWithExceptionCallback(CS$<>8__locals3.<SaveData>g__SerializeToFile|4(), delegate(Exception ex)
					{
						Log.Error(string.Format("Exception while saving '{0}': {1}", CS$<>8__locals3.fileName, ex));
					});
					num = k;
				}
				object[] lastTaskNames = null;
				for (;;)
				{
					if (!CS$<>8__locals1.threadedSaveTasks.Any<Task>((Task x) => !x.IsCompleted))
					{
						break;
					}
					IEnumerable<ValueTuple<string, string, Action<Stream>>> enumerable = this.threadedSerializers;
					Func<ValueTuple<string, string, Action<Stream>>, int, bool> func;
					if ((func = CS$<>8__locals1.<>9__5) == null)
					{
						func = (CS$<>8__locals1.<>9__5 = ([TupleElementNames(new string[] { "langKey", "fileName", "serializer" })] ValueTuple<string, string, Action<Stream>> _, int i) => !CS$<>8__locals1.threadedSaveTasks[i].IsCompleted);
					}
					object[] array = (from x in enumerable.Where<ValueTuple<string, string, Action<Stream>>>(func).Take<ValueTuple<string, string, Action<Stream>>>(3)
						select Localization.Get(x.Item1, false, null)).Cast<object>().ToArray<object>();
					if (lastTaskNames != null && array.SequenceEqual<object>(lastTaskNames))
					{
						yield return null;
					}
					else
					{
						lastTaskNames = array;
						yield return this.SetMessage(string.Format(Localization.Get("xuiRwgSaving", false, null), Localization.FormatListAnd(array)), false, false);
					}
				}
				yield return CS$<>8__locals1.<SaveData>g__CommitSaveWithMessageCo|1();
				SaveInfoProvider.Instance.ClearResources();
				yield return this.SetMessage(null, false, false);
				Log.Out(string.Format("RWG SaveData in {0:F3} s", this.totalMS.Elapsed.TotalSeconds));
				yield break;
			}
			if (promptMode == WorldBuilder.SaveDataPromptMode.Off)
			{
				yield break;
			}
			if (onCancel != null)
			{
				onCancel();
			}
			else if (onDiscard != null)
			{
				onDiscard();
			}
			yield break;
		}

		// Token: 0x0600B7B3 RID: 47027 RVA: 0x0044878B File Offset: 0x0044698B
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> CalcPlayerSpawnTiles()
		{
			List<StreetTile> list = (from StreetTile st in this.StreetTileMap
				where !st.OverlapsRadiation && !st.AllIsWater && st.Township == null && (st.District == null || st.District.name == "wilderness") && (this.ForestBiomeWeight == 0 || st.BiomeType == BiomeType.forest) && !st.Used
				select st).ToList<StreetTile>();
			list.Sort((StreetTile _t1, StreetTile _t2) => this.CalcClosestTraderDistance(_t1).CompareTo(this.CalcClosestTraderDistance(_t2)));
			return list;
		}

		// Token: 0x0600B7B4 RID: 47028 RVA: 0x004487C0 File Offset: 0x004469C0
		[PublicizedFrom(EAccessModifier.Private)]
		public float CalcClosestTraderDistance(StreetTile _st)
		{
			float num = float.MaxValue;
			foreach (Vector2i vector2i in this.TraderCenterPositions)
			{
				float num2 = Vector2i.Distance(_st.WorldPositionCenter, vector2i);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x0600B7B5 RID: 47029 RVA: 0x00448828 File Offset: 0x00446A28
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> getWildernessTilesToSmooth()
		{
			return (from StreetTile st in this.StreetTileMap
				where st.NeedsWildernessSmoothing
				select st).ToList<StreetTile>();
		}

		// Token: 0x0600B7B6 RID: 47030 RVA: 0x00448860 File Offset: 0x00446A60
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitStreetTiles()
		{
			this.StreetTileMapWidth = this.WorldSize / 150;
			this.data.StreetTileDataGrid = new NativeArray<StreetTileData>(this.StreetTileMapWidth * this.StreetTileMapWidth, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.StreetTileMap = new StreetTile[this.StreetTileMapWidth * this.StreetTileMapWidth];
			for (int i = 0; i < this.StreetTileMapWidth; i++)
			{
				for (int j = 0; j < this.StreetTileMapWidth; j++)
				{
					this.StreetTileMap[j + i * this.StreetTileMapWidth] = new StreetTile(this, new Vector2i(j, i));
				}
			}
		}

		// Token: 0x0600B7B7 RID: 47031 RVA: 0x004488F8 File Offset: 0x00446AF8
		public void CleanupGeneratedData()
		{
			this.roadDest = null;
			this.biomeDest = null;
			this.radDest = null;
			this.Townships.Clear();
			this.data.Cleanup();
			PrefabManager prefabManager = this.PrefabManager;
			if (prefabManager != null)
			{
				prefabManager.Clear();
			}
			this.PathingUtils.Cleanup();
			Rand.Instance.Cleanup();
		}

		// Token: 0x0600B7B8 RID: 47032 RVA: 0x00448958 File Offset: 0x00446B58
		public void Cleanup()
		{
			this.serializedTotalSize = 0L;
			Span<MemoryStream> span = this.mainThreadSerializerBuffers.AsSpan<MemoryStream>();
			for (int i = 0; i < span.Length; i++)
			{
				ref MemoryStream ptr = ref span[i];
				MemoryStream memoryStream = ptr;
				if (memoryStream != null)
				{
					memoryStream.Dispose();
				}
				ptr = null;
			}
			span = this.threadedSerializerBuffers.AsSpan<MemoryStream>();
			for (int i = 0; i < span.Length; i++)
			{
				ref MemoryStream ptr2 = ref span[i];
				MemoryStream memoryStream2 = ptr2;
				if (memoryStream2 != null)
				{
					memoryStream2.Dispose();
				}
				ptr2 = null;
			}
			this.CleanupGeneratedData();
			PrefabManager prefabManager = this.PrefabManager;
			if (prefabManager != null)
			{
				prefabManager.Cleanup();
			}
			StampManager stampManager = this.StampManager;
			if (stampManager != null)
			{
				stampManager.ClearStamps();
			}
			GCUtils.UnloadAndCollectStart();
			this.IsFinished = true;
		}

		// Token: 0x0600B7B9 RID: 47033 RVA: 0x00448A0C File Offset: 0x00446C0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void GenerateTerrain()
		{
			this.BiomeSize = this.WorldSize / 8;
			this.biomeDest = new Color32[this.BiomeSize * this.BiomeSize];
			this.BorderWaterMask = Rand.Instance.Int() & 15;
			Log.Out("generateBiomeTiles start at {0}, r={1:x}", new object[]
			{
				(float)this.totalMS.ElapsedMilliseconds * 0.001f,
				Rand.Instance.PeekSample()
			});
			this.GenerateBiomeTiles();
			Log.Out("GenerateTerrainTiles start at {0}, r={1:x}", new object[]
			{
				(float)this.totalMS.ElapsedMilliseconds * 0.001f,
				Rand.Instance.PeekSample()
			});
			this.GenerateTerrainTiles();
			Log.Out("GenerateBaseStamps start at {0}, r={1:x}", new object[]
			{
				(float)this.totalMS.ElapsedMilliseconds * 0.001f,
				Rand.Instance.PeekSample()
			});
			this.GenerateBaseStamps();
			this.GenerateTerrainFromTiles(TerrainType.plains, 1024);
			this.GenerateTerrainFromTiles(TerrainType.hills, 512);
			this.GenerateTerrainFromTiles(TerrainType.mountains, 256);
			this.previewStepOfTask = XUiC_WorldGenerationPreview.PreviewStep.Biome;
			Log.Out("WriteStampsToMaps start at {0}, r={1:x}", new object[]
			{
				(float)this.totalMS.ElapsedMilliseconds * 0.001f,
				Rand.Instance.PeekSample()
			});
			this.DrawBiomeRadStampsToMaps();
			this.SetTaskMessage(this.messageTerrainGenerationFinished);
		}

		// Token: 0x0600B7BA RID: 47034 RVA: 0x00448B94 File Offset: 0x00446D94
		[PublicizedFrom(EAccessModifier.Private)]
		public void GenerateBaseStamps()
		{
			for (int i = 0; i < this.data.HeightMap.Length; i++)
			{
				this.data.HeightMap[i] = 35f;
			}
			Vector2 sizeMinMax = new Vector2(0.6f, 0.85f);
			Task task = new Task(delegate
			{
				MicroStopwatch microStopwatch = new MicroStopwatch();
				Rand rand = new Rand(this.Seed + 1);
				TranslationData translationData = new TranslationData(0, 0, 0f, 0);
				int num = 0;
				while (num < this.WorldSize + 160 && !this.IsCanceled)
				{
					if (this.BorderWaterMask != 15)
					{
						for (int k = 0; k < 4; k++)
						{
							translationData.x = -9999;
							if (k == 0 && (this.BorderWaterMask & 1) == 0)
							{
								translationData.x = num + rand.Range(0, 75);
								translationData.y = rand.Range(0, 75);
							}
							else if (k == 1 && (this.BorderWaterMask & 2) == 0)
							{
								translationData.x = num + rand.Range(0, 75);
								translationData.y = this.WorldSize - rand.Range(0, 75);
							}
							else if (k == 2 && (this.BorderWaterMask & 4) == 0)
							{
								translationData.x = rand.Range(0, 75);
								translationData.y = num + rand.Range(0, 75);
							}
							else if (k == 3 && (this.BorderWaterMask & 8) == 0)
							{
								translationData.x = this.WorldSize - rand.Range(0, 75);
								translationData.y = num + rand.Range(0, 75);
							}
							if (translationData.x != -9999)
							{
								translationData.scale = rand.Range(sizeMinMax.x, sizeMinMax.y);
								translationData.rotation = rand.Angle();
								int num2 = this.WorldSize / 1024 - 1;
								string text = this.biomeMap.Get(Mathf.Clamp(translationData.x / 1024, 0, num2), Mathf.Clamp(translationData.y / 1024, 0, num2)).ToStringCached<BiomeType>();
								RawStamp rawStamp;
								if (this.StampManager.TryGetStamp(text + "_land_border", out rawStamp, rand) || this.StampManager.TryGetStamp("land_border", out rawStamp, rand))
								{
									this.StampManager.DrawStamp(ref this.data.HeightMap, new Stamp(this, rawStamp, translationData, default(Color32), 0.1f, false, ""));
								}
							}
						}
					}
					if (this.BorderWaterMask > 0)
					{
						for (int l = 0; l < 4; l++)
						{
							translationData.x = -9999;
							if (l == 0 && (this.BorderWaterMask & 1) > 0)
							{
								translationData.x = num;
								translationData.y = -40 + rand.Range(0);
							}
							else if (l == 1 && (this.BorderWaterMask & 2) > 0)
							{
								translationData.x = num;
								translationData.y = this.WorldSize - -40 - rand.Range(0);
							}
							else if (l == 2 && (this.BorderWaterMask & 4) > 0)
							{
								translationData.x = -40 + rand.Range(0);
								translationData.y = num;
							}
							else if (l == 3 && (this.BorderWaterMask & 8) > 0)
							{
								translationData.x = this.WorldSize - -40 - rand.Range(0);
								translationData.y = num;
							}
							if (translationData.x != -9999)
							{
								translationData.scale = rand.Range(sizeMinMax.x, sizeMinMax.y);
								translationData.rotation = rand.Angle();
								RawStamp rawStamp2;
								if (this.StampManager.TryGetStamp("water_border", out rawStamp2, rand))
								{
									this.StampManager.DrawStamp(ref this.data.HeightMap, new Stamp(this, rawStamp2, translationData, default(Color32), 0.1f, false, ""));
									Stamp stamp = new Stamp(this, rawStamp2, translationData, new Color32(0, 0, (byte)this.WaterHeight, byte.MaxValue), 0.1f, true, "");
									this.waterLayer.Stamps.Add(stamp);
									StampManager.DrawWaterStamp(stamp, ref this.data.waterDest, this.WorldSize);
								}
							}
						}
					}
					num += 160;
				}
				rand.Free();
				Log.Out("GenerateBaseStamps terrainBorderThread in {0}", new object[] { (float)microStopwatch.ElapsedMilliseconds * 0.001f });
			});
			task.Start();
			Task task2 = new Task(delegate
			{
			});
			task2.Start();
			Task[] array = new Task[]
			{
				new Task(delegate
				{
					MicroStopwatch microStopwatch2 = new MicroStopwatch(true);
					Rand rand2 = new Rand(this.Seed + 3);
					Color32 color = this.biomeColors[BiomeType.forest];
					for (int m = 0; m < this.biomeDest.Length; m++)
					{
						this.biomeDest[m] = color;
					}
					RawStamp stamp2 = this.StampManager.GetStamp("filler_biome", rand2);
					if (stamp2 != null)
					{
						int num3 = this.WorldSize / 256;
						int num4 = 32;
						int num5 = num4 / 2;
						float num6 = (float)num4 / (float)stamp2.width * 1.5f;
						for (int n = 0; n < num3; n++)
						{
							int num7 = n * 256 / 8;
							for (int num8 = 0; num8 < num3; num8++)
							{
								int num9 = num8 * 256 / 8;
								BiomeType biomeType = this.biomeMap.Get(num8, n);
								if (biomeType != BiomeType.none)
								{
									float num10 = num6 + rand2.Range(0f, 0.2f);
									float num11 = (float)(rand2.Range(0, 4) * 90 + rand2.Range(-20, 20));
									StampManager.DrawBiomeStamp(this.biomeDest, ref stamp2.data.alphaPixels, num9 + num5, num7 + num5, this.BiomeSize, this.BiomeSize, stamp2.width, stamp2.height, num10, this.biomeColors[biomeType], 0.1f, num11);
								}
							}
						}
					}
					rand2.Free();
					Log.Out("GenerateBaseStamps biomeThreads in {0}", new object[] { (float)microStopwatch2.ElapsedMilliseconds * 0.001f });
				})
			};
			Task[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].Start();
			}
			bool flag = true;
			while (flag || !task.IsCompleted || !task2.IsCompleted)
			{
				flag = false;
				foreach (Task task3 in array)
				{
					flag |= !task3.IsCompleted;
				}
				if (!task.IsCompleted && flag)
				{
					this.SetTaskMessage(this.creatingTerrainAndBiomeStamps);
				}
				else if (!task.IsCompleted && !flag)
				{
					this.SetTaskMessage(this.creatingTerrainStamps);
				}
				else
				{
					this.SetTaskMessage(this.creatingBiomeStamps);
				}
			}
			WorldBuilder.ThrowIfTaskFaulted(task, "GenerateBaseStamps.terrainBorderTask");
			WorldBuilder.ThrowIfTaskFaulted(task2, "GenerateBaseStamps.radTask");
			array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				WorldBuilder.ThrowIfTaskFaulted(array2[j], "GenerateBaseStamps.biomeTask");
			}
		}

		// Token: 0x0600B7BB RID: 47035 RVA: 0x00448D40 File Offset: 0x00446F40
		[PublicizedFrom(EAccessModifier.Private)]
		public void GenerateTerrainFromTiles(TerrainType _terrainType, int _tileSize)
		{
			Log.Out("GenerateTerrainFromTiles {0}, start at {1}, r={2:x}", new object[]
			{
				_terrainType,
				(float)this.totalMS.ElapsedMilliseconds * 0.001f,
				Rand.Instance.PeekSample()
			});
			int num = this.WorldSize / 256;
			int num2 = 0;
			string text = _terrainType.ToStringCached<TerrainType>();
			int num3 = _tileSize / 256;
			for (int i = 0; i < num; i += num3)
			{
				this.SetTaskMessage(string.Format(this.messageGeneratingTerrain, Mathf.FloorToInt(100f * ((float)num2 / (float)(num * num)))));
				for (int j = 0; j < num; j += num3)
				{
					num2++;
					bool flag = true;
					for (int k = 0; k < num3; k++)
					{
						for (int l = 0; l < num3; l++)
						{
							if (this.terrainTypeMap.Get(i + k, j + l) == _terrainType)
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (!flag)
					{
						BiomeType biomeType = this.biomeMap.Get(i, j);
						if (biomeType == BiomeType.none)
						{
							biomeType = BiomeType.forest;
						}
						if (_terrainType == TerrainType.mountains && biomeType == BiomeType.wasteland)
						{
							this.terrainTypeMap.Set(i, j, TerrainType.plains);
						}
						else
						{
							int num4 = i * 256 + _tileSize / 2;
							int num5 = j * 256 + _tileSize / 2;
							string text2 = biomeType.ToStringCached<BiomeType>();
							string text3 = string.Format("{0}_{1}", text2, text);
							Vector2 vector;
							int num6;
							int num7;
							float num8;
							bool flag2;
							float num9;
							this.GetTerrainProperties(text2, text, text3, out vector, out num6, out num7, out num8, out flag2, out num9);
							vector *= (float)num3;
							num7 *= num3;
							int num10 = 0;
							float num11 = num8;
							bool flag3 = false;
							for (int m = 0; m < num6; m++)
							{
								RawStamp rawStamp;
								if (this.StampManager.TryGetStamp(text, text3, out rawStamp))
								{
									Vector2 vector2 = Rand.Instance.RandomOnUnitCircle() * (float)num10;
									TranslationData translationData = new TranslationData(num4 + Mathf.RoundToInt(vector2.x), num5 + Mathf.RoundToInt(vector2.y), vector.x, vector.y);
									RawStamp rawStamp2 = rawStamp;
									TranslationData translationData2 = translationData;
									string name = rawStamp.name;
									Stamp stamp = new Stamp(this, rawStamp2, translationData2, default(Color32), 0.1f, false, name);
									stamp.alpha = num11;
									stamp.additive = flag3;
									this.terrainLayer.Stamps.Add(stamp);
									if (flag2)
									{
										this.biomeLayer.Stamps.Add(new Stamp(this, rawStamp, translationData, this.biomeColors[biomeType], num9, false, ""));
									}
									num10 = num7;
									num11 = num8 * 0.45f;
									flag3 = true;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B7BC RID: 47036 RVA: 0x00448FF8 File Offset: 0x004471F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void DrawBiomeRadStampsToMaps()
		{
			Task task = new Task(delegate
			{
				MicroStopwatch microStopwatch = new MicroStopwatch(true);
				this.StampManager.DrawStampGroup(this.biomeLayer, this.biomeDest, this.BiomeSize, 0.125f);
				this.biomeLayer.Stamps.Clear();
				Log.Out("DrawBiomeRadStampsToMaps biome in {0}", new object[] { (float)microStopwatch.ElapsedMilliseconds * 0.001f });
			});
			Task task2 = new Task(delegate
			{
				MicroStopwatch microStopwatch2 = new MicroStopwatch(true);
				this.StampManager.DrawStampGroup(this.radiationLayer, this.radDest, this.RadSize, 1f);
				Log.Out("DrawBiomeRadStampsToMaps rad in {0}", new object[] { (float)microStopwatch2.ElapsedMilliseconds * 0.001f });
				microStopwatch2.ResetAndRestart();
				this.ClearWaterUnderTerrain();
				Log.Out("DrawBiomeRadStampsToMaps water #{0} in {1}", new object[]
				{
					this.waterLayer.Stamps.Count,
					(float)microStopwatch2.ElapsedMilliseconds * 0.001f
				});
				this.waterLayer.Stamps.Clear();
			});
			task.Start();
			task2.Start();
			this.SetTaskMessage(this.messageWritingStampsToMap);
			while (!task.IsCompleted || !task2.IsCompleted)
			{
				Thread.Sleep(1);
			}
			WorldBuilder.ThrowIfTaskFaulted(task, "DrawBiomeRadStampsToMaps.biomeTask");
			WorldBuilder.ThrowIfTaskFaulted(task2, "DrawBiomeRadStampsToMaps.radnwatTask");
			Log.Out("DrawBiomeRadStampsToMaps end at {0}, r={1:x}", new object[]
			{
				(float)this.totalMS.ElapsedMilliseconds * 0.001f,
				Rand.Instance.PeekSample()
			});
		}

		// Token: 0x0600B7BD RID: 47037 RVA: 0x004490AC File Offset: 0x004472AC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ClearWaterUnderTerrain()
		{
			for (int i = 0; i < this.waterLayer.Stamps.Count; i++)
			{
				Stamp stamp = this.waterLayer.Stamps[i];
				int num = Utils.FastMax(Mathf.FloorToInt(stamp.Area.min.x), 0);
				int num2 = Utils.FastMin(Mathf.FloorToInt(stamp.Area.max.x), this.WorldSize - 1);
				int num3 = Utils.FastMax(Mathf.FloorToInt(stamp.Area.min.y), 0);
				int num4 = Utils.FastMin(Mathf.FloorToInt(stamp.Area.max.y), this.WorldSize - 1);
				WorldBuilder.ClearWaterUnderTerrain(ref this.data, ref this.data.HeightMap, num, num2, num3, num4);
			}
		}

		// Token: 0x0600B7BE RID: 47038 RVA: 0x00449184 File Offset: 0x00447384
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void ClearWaterUnderTerrain(ref WorldBuilder.Data _data, ref NativeArray<float> _terrain, int startX, int endX, int startY, int endY)
		{
			WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.Invoke(ref _data, ref _terrain, startX, endX, startY, endY);
		}

		// Token: 0x0600B7BF RID: 47039 RVA: 0x00449194 File Offset: 0x00447394
		[PublicizedFrom(EAccessModifier.Private)]
		public void GenerateTerrainLast()
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			this.StampManager.GetStamp("base", null);
			if (this.Lakes > WorldBuilder.GenerationSelections.None)
			{
				this.generateTerrainFeature("lake", this.Lakes, true);
			}
			if (this.Rivers > WorldBuilder.GenerationSelections.None)
			{
				this.generateTerrainFeature("river", this.Rivers, true);
			}
			if (this.Canyons > WorldBuilder.GenerationSelections.None)
			{
				this.generateTerrainFeature("canyon", this.Canyons, false);
			}
			if (this.Craters > WorldBuilder.GenerationSelections.None)
			{
				this.generateTerrainFeature("crater", this.Craters, false);
			}
			Task task = new Task(delegate
			{
				this.StampManager.DrawStampGroup(this.lowerLayer, ref this.data.HeightMap, this.WorldSize);
				this.StampManager.DrawStampGroup(this.terrainLayer, ref this.data.HeightMap, this.WorldSize);
				WorldBuilder.AdjustHeights(ref this.data.HeightMap, 2f);
			});
			Task task2 = new Task(delegate
			{
				this.StampManager.DrawWaterStampGroup(this.waterLayer, ref this.data.waterDest, this.WorldSize);
			});
			task.Start();
			task2.Start();
			while (!task.IsCompleted || !task2.IsCompleted)
			{
				if (!task.IsCompleted && task2.IsCompleted)
				{
					this.SetTaskMessage(this.messageWritingTerrainStampsToMap);
				}
				else if (task.IsCompleted && !task2.IsCompleted)
				{
					this.SetTaskMessage(this.messageWritingWaterStampsToMap);
				}
				else
				{
					this.SetTaskMessage(this.messageWritingTerrainAndWaterStampsToMap);
				}
				Thread.Sleep(1);
			}
			WorldBuilder.ThrowIfTaskFaulted(task, "GenerateTerrainLast.terrainTask");
			WorldBuilder.ThrowIfTaskFaulted(task2, "GenerateTerrainLast.waterTask");
			this.SetTaskMessage(this.messageCleaningUpWaterMapData);
			MicroStopwatch microStopwatch2 = new MicroStopwatch(true);
			this.ClearWaterUnderTerrain();
			Log.Out("WaterToMap last #{0} in {1}", new object[]
			{
				this.waterLayer.Stamps.Count,
				(float)microStopwatch2.ElapsedMilliseconds * 0.001f
			});
			Log.Out("GenerateTerrainLast done in {0}, r={1:x}", new object[]
			{
				(float)microStopwatch.ElapsedMilliseconds * 0.001f,
				Rand.Instance.PeekSample()
			});
		}

		// Token: 0x0600B7C0 RID: 47040 RVA: 0x0044935C File Offset: 0x0044755C
		[PublicizedFrom(EAccessModifier.Private)]
		public void FinalizeWater()
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			WorldBuilder.FinalizeWater(ref this.data, (float)this.WaterHeight);
			Log.Out("FinalizeWater in {0}", new object[] { (float)microStopwatch.ElapsedMilliseconds * 0.001f });
		}

		// Token: 0x0600B7C1 RID: 47041 RVA: 0x004493A7 File Offset: 0x004475A7
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void FinalizeWater(ref WorldBuilder.Data _data, float _WaterHeight)
		{
			WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.Invoke(ref _data, _WaterHeight);
		}

		// Token: 0x0600B7C2 RID: 47042 RVA: 0x004493B0 File Offset: 0x004475B0
		[PublicizedFrom(EAccessModifier.Private)]
		public void SerializeWater(Stream stream)
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			Color32[] array = new Color32[this.WorldSize * this.WorldSize];
			Color32 color = new Color32(0, 0, 0, 0);
			for (int i = 0; i < this.WorldSize; i++)
			{
				for (int j = 0; j < this.WorldSize; j++)
				{
					int num = j + i * this.WorldSize;
					color.b = (byte)this.data.waterDest[num];
					array[num] = color;
				}
			}
			Log.Out(string.Format("Create water in {0}", (float)microStopwatch.ElapsedMilliseconds * 0.001f));
			stream.Write(ImageConversion.EncodeArrayToPNG(array, GraphicsFormat.R8G8B8A8_UNorm, (uint)this.WorldSize, (uint)this.WorldSize, (uint)(this.WorldSize * 4)));
		}

		// Token: 0x0600B7C3 RID: 47043 RVA: 0x00449480 File Offset: 0x00447680
		[PublicizedFrom(EAccessModifier.Private)]
		public void generateTerrainFeature(string featureName, WorldBuilder.GenerationSelections selection, bool isWaterFeature = false)
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			Vector2 vector = new Vector2(0.5f, 1.5f);
			Vector2i vector2i = Vector2i.zero;
			Vector2i vector2i2 = Vector2i.zero;
			Vector2i vector2i3 = Vector2i.zero;
			Vector2i vector2i4 = Vector2i.zero;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(this.Seed + featureName.GetHashCode() + 1);
			GameRandom rnd2 = GameRandomManager.Instance.CreateGameRandom(this.Seed + featureName.GetHashCode() + 2);
			string @string;
			if ((@string = this.thisWorldProperties.GetString(featureName + "s", "scale")) != string.Empty)
			{
				vector = StringParsers.ParseVector2(@string);
			}
			int count = this.GetCount(featureName + "s", selection, null);
			Func<StreetTile, int> <>9__1;
			for (int i = 0; i < count; i++)
			{
				RawStamp rawStamp;
				if (!this.StampManager.TryGetStamp(featureName, out rawStamp))
				{
					if (featureName.Contains("river"))
					{
						Log.Out("Could not find stamp {0}", new object[] { featureName });
					}
				}
				else
				{
					float num = gameRandom.RandomRange(vector.x, vector.y) * 1.4f;
					int num2 = gameRandom.RandomRange(0, 360);
					int num3 = (int)((float)rawStamp.width * num);
					int num4 = (int)((float)rawStamp.height * num);
					int num5 = -(num3 / 2);
					int num6 = -(num4 / 2);
					vector2i = this.getRotatedPoint(num5, num6, num5 + num3 / 2, num6 + num4 / 2, num2);
					vector2i2 = this.getRotatedPoint(num5 + num3, num6, num5 + num3 / 2, num6 + num4 / 2, num2);
					vector2i3 = this.getRotatedPoint(num5, num6 + num4, num5 + num3 / 2, num6 + num4 / 2, num2);
					vector2i4 = this.getRotatedPoint(num5 + num3, num6 + num4, num5 + num3 / 2, num6 + num4 / 2, num2);
					zero.x = (float)Mathf.Min(Mathf.Min(vector2i.x, vector2i2.x), Mathf.Min(vector2i3.x, vector2i4.x));
					zero.y = (float)Mathf.Min(Mathf.Min(vector2i.y, vector2i2.y), Mathf.Min(vector2i3.y, vector2i4.y));
					zero2.x = (float)Mathf.Max(Mathf.Max(vector2i.x, vector2i2.x), Mathf.Max(vector2i3.x, vector2i4.x));
					zero2.y = (float)Mathf.Max(Mathf.Max(vector2i.y, vector2i2.y), Mathf.Max(vector2i3.y, vector2i4.y));
					Rect rect = new Rect(zero, zero2 - zero);
					IEnumerable<StreetTile> enumerable = from StreetTile st in this.StreetTileMap
						where (st.Township == null || st.District == null || st.District.name == "wilderness") && st.TerrainType != TerrainType.mountains && !st.HasFeature && st.GetNeighborCount() > 3
						select st;
					Func<StreetTile, int> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = (StreetTile st) => rnd2.RandomInt);
					}
					using (List<StreetTile>.Enumerator enumerator = enumerable.OrderBy<StreetTile, int>(func).ToList<StreetTile>().GetEnumerator())
					{
						IL_06A8:
						while (enumerator.MoveNext())
						{
							StreetTile streetTile = enumerator.Current;
							if (streetTile.GridPosition.x != 0 && streetTile.GridPosition.y != 0)
							{
								int num7 = streetTile.WorldPositionCenter.x - (int)rect.width / 2;
								while ((float)num7 < (float)streetTile.WorldPositionCenter.x + rect.width / 2f)
								{
									int num8 = streetTile.WorldPositionCenter.y - (int)rect.height / 2;
									while ((float)num8 < (float)streetTile.WorldPositionCenter.y + rect.height / 2f)
									{
										StreetTile streetTileWorld = this.GetStreetTileWorld(num7, num8);
										if (streetTileWorld == null || streetTileWorld.Township != null || streetTileWorld.District != null || streetTileWorld.Used || streetTileWorld.HasFeature)
										{
											goto IL_06A8;
										}
										num8 += 150;
									}
									num7 += 150;
								}
								int num9 = streetTile.WorldPositionCenter.x - (int)rect.width / 2;
								while ((float)num9 < (float)streetTile.WorldPositionCenter.x + rect.width / 2f)
								{
									int num10 = streetTile.WorldPositionCenter.y - (int)rect.height / 2;
									while ((float)num10 < (float)streetTile.WorldPositionCenter.y + rect.height / 2f)
									{
										this.GetStreetTileWorld(num9, num10).HasFeature = true;
										num10 += 150;
									}
									num9 += 150;
								}
								TranslationData translationData = new TranslationData(streetTile.WorldPositionCenter.x, streetTile.WorldPositionCenter.y, num, num2);
								Stamp stamp = new Stamp(this, rawStamp, translationData, default(Color32), 0.1f, false, "");
								if (!isWaterFeature)
								{
									this.lowerLayer.Stamps.Add(stamp);
									bool flag = true;
									for (int j = 0; j < this.waterLayer.Stamps.Count; j++)
									{
										if (stamp.Area.Overlaps(this.waterLayer.Stamps[j].Area))
										{
											flag = false;
											break;
										}
									}
									if (flag)
									{
										for (int k = 0; k < this.waterRects.Count; k++)
										{
											if (stamp.Area.Overlaps(this.waterRects[k]))
											{
												flag = false;
												break;
											}
										}
									}
									if (!flag)
									{
										this.waterLayer.Stamps.Add(new Stamp(this, rawStamp, translationData, new Color32(0, 0, (byte)this.WaterHeight, byte.MaxValue), 0.05f, true, ""));
									}
									break;
								}
								bool flag2 = false;
								for (int l = 0; l < this.terrainLayer.Stamps.Count; l++)
								{
									if (stamp.Name.Contains("mountain") && stamp.Area.Overlaps(this.terrainLayer.Stamps[l].Area))
									{
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									this.lowerLayer.Stamps.Add(stamp);
									this.waterLayer.Stamps.Add(new Stamp(this, rawStamp, translationData, new Color32(0, 0, (byte)this.WaterHeight, byte.MaxValue), 0.1f, true, ""));
									break;
								}
								i--;
							}
						}
					}
				}
			}
			GameRandomManager.Instance.FreeGameRandom(rnd2);
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			Log.Out("generateTerrainFeature {0} in {1}", new object[]
			{
				featureName,
				(float)microStopwatch.ElapsedMilliseconds * 0.001f
			});
		}

		// Token: 0x0600B7C4 RID: 47044 RVA: 0x00449BC4 File Offset: 0x00447DC4
		public bool CreatePlayerSpawn(Vector2i worldPos, bool _isFallback = false)
		{
			Vector3 vector = new Vector3((float)worldPos.x, this.GetHeight(worldPos), (float)worldPos.y);
			if (!_isFallback)
			{
				for (int i = 0; i < this.playerSpawns.Count; i++)
				{
					if (this.playerSpawns[i].IsTooClose(vector))
					{
						return false;
					}
				}
				StreetTile streetTileWorld = this.GetStreetTileWorld(worldPos);
				if (streetTileWorld != null && streetTileWorld.HasPrefabs)
				{
					if (this.ForestBiomeWeight > 0 && streetTileWorld.BiomeType != BiomeType.forest)
					{
						return false;
					}
					using (List<PrefabDataInstance>.Enumerator enumerator = streetTileWorld.StreetTilePrefabDatas.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.prefab.DifficultyTier >= 2)
							{
								return false;
							}
						}
					}
				}
				List<Vector2i> list = ((this.ForestBiomeWeight > 0) ? this.TraderForestCenterPositions : this.TraderCenterPositions);
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (Vector2i.DistanceSqr(list[j], worldPos) < 810000f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			WorldBuilder.PlayerSpawn playerSpawn = new WorldBuilder.PlayerSpawn(vector, (float)Rand.Instance.Range(0, 360));
			this.playerSpawns.Add(playerSpawn);
			return true;
		}

		// Token: 0x0600B7C5 RID: 47045 RVA: 0x00449D1C File Offset: 0x00447F1C
		[PublicizedFrom(EAccessModifier.Private)]
		public void CalcTownshipsHeightMask()
		{
			int worldSize = this.WorldSize;
			int worldSize2 = this.WorldSize;
			int num = worldSize * worldSize2;
			this.data.poiHeightMask = new NativeArray<byte>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			if (this.Townships != null)
			{
				for (int i = 0; i < this.Townships.Count; i++)
				{
					foreach (StreetTile streetTile in this.Townships[i].Streets.Values)
					{
						int num2 = 0;
						int num3 = streetTile.WorldPosition.x + streetTile.WorldPosition.y * worldSize;
						for (int j = -num2; j < 150 + num2; j++)
						{
							for (int k = -num2; k < 150 + num2; k++)
							{
								this.data.poiHeightMask[k + num3] = 1;
							}
							num3 += worldSize;
						}
					}
				}
			}
		}

		// Token: 0x0600B7C6 RID: 47046 RVA: 0x00449E34 File Offset: 0x00448034
		[PublicizedFrom(EAccessModifier.Private)]
		public void CalcWindernessPOIsHeightMask(Color32[] roadMask)
		{
			int worldSize = this.WorldSize;
			for (int i = 0; i < this.StreetTileMapWidth; i++)
			{
				for (int j = 0; j < this.StreetTileMapWidth; j++)
				{
					StreetTile streetTile = this.StreetTileMap[j + i * this.StreetTileMapWidth];
					if (streetTile.NeedsWildernessSmoothing)
					{
						int num = streetTile.WildernessPOIPos.x + streetTile.WildernessPOIPos.y * worldSize;
						for (int k = 0; k < streetTile.WildernessPOISize.y; k++)
						{
							for (int l = 0; l < streetTile.WildernessPOISize.x; l++)
							{
								this.data.poiHeightMask[l + num] = 1;
							}
							num += worldSize;
						}
					}
				}
			}
		}

		// Token: 0x0600B7C7 RID: 47047 RVA: 0x00449EFC File Offset: 0x004480FC
		public void SmoothRoadTerrain(Color32[] _roadMask, NativeArray<float> _heightMap, int WorldSize, List<Township> _townships = null)
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			NativeArray<Color32> roadMask = new NativeArray<Color32>(_roadMask.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			roadMask.CopyFrom(_roadMask);
			Task task = new Task(delegate
			{
				WorldBuilder.SmoothRoadTerrainTask(ref this.data, ref roadMask, ref _heightMap, WorldSize);
			});
			task.Start();
			while (!task.IsCompleted)
			{
				this.SetTaskMessage(string.Format(this.messageSmoothRoadTerrainCount, this.data.messageCnt));
				Thread.Sleep(50);
			}
			WorldBuilder.ThrowIfTaskFaulted(task, "SmoothRoadTerrain.task");
			roadMask.Dispose();
			Log.Out("Smooth Road Terrain in {0}", new object[] { (float)microStopwatch.ElapsedMilliseconds * 0.001f });
		}

		// Token: 0x0600B7C8 RID: 47048 RVA: 0x00449FCE File Offset: 0x004481CE
		[BurstCompile(CompileSynchronously = true)]
		public static void SmoothRoadTerrainTask(ref WorldBuilder.Data _data, ref NativeArray<Color32> roadMask, ref NativeArray<float> _heightMap, int WorldSize)
		{
			WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.Invoke(ref _data, ref roadMask, ref _heightMap, WorldSize);
		}

		// Token: 0x0600B7C9 RID: 47049 RVA: 0x00449FDC File Offset: 0x004481DC
		[PublicizedFrom(EAccessModifier.Private)]
		public void SmoothWildernessTerrain()
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			foreach (StreetTile streetTile in this.getWildernessTilesToSmooth())
			{
				streetTile.SmoothWildernessTerrain();
			}
			Log.Out(string.Format("Smooth Wilderness Terrain in {0}, r={1:x}", (float)microStopwatch.ElapsedMilliseconds * 0.001f, Rand.Instance.PeekSample()));
		}

		// Token: 0x0600B7CA RID: 47050 RVA: 0x0044A064 File Offset: 0x00448264
		[PublicizedFrom(EAccessModifier.Private)]
		public void GenerateTerrainTiles()
		{
			int num = this.WorldSize / 256;
			this.terrainTypeMap = new DataMap<TerrainType>(num, TerrainType.none);
			Rand instance = Rand.Instance;
			List<TileGroup> list = new List<TileGroup>();
			for (int i = 0; i < 5; i++)
			{
				list.Add(new TileGroup
				{
					Biome = (BiomeType)i
				});
			}
			for (int j = 0; j < this.biomeMap.width; j++)
			{
				for (int k = 0; k < this.biomeMap.width; k++)
				{
					Vector2i vector2i = new Vector2i(j, k);
					BiomeType biomeType = this.biomeMap.Get(j, k);
					list[(int)biomeType].Positions.Add(vector2i);
				}
			}
			float num2 = (float)(this.Plains + this.Hills + this.Mountains);
			if (num2 == 0f)
			{
				this.Plains = 1;
				num2 = 1f;
			}
			foreach (TileGroup tileGroup in list)
			{
				int num3 = Mathf.FloorToInt((float)this.Plains / num2 * (float)tileGroup.Positions.Count);
				int num4 = Mathf.FloorToInt((float)this.Hills / num2 * (float)tileGroup.Positions.Count);
				int num5 = Mathf.FloorToInt((float)this.Mountains / num2 * (float)tileGroup.Positions.Count);
				while (tileGroup.Positions.Count > num3 + num4 + num5)
				{
					int num6 = instance.Range(3);
					if (num6 == 0)
					{
						if (this.Plains > 0)
						{
							num3++;
						}
						else
						{
							num6++;
						}
					}
					if (num6 == 1)
					{
						if (this.Hills > 0)
						{
							num4++;
						}
						else
						{
							num6++;
						}
					}
					if (num6 == 2 && this.Mountains > 0)
					{
						num5++;
					}
				}
				int num7 = instance.Range(tileGroup.Positions.Count);
				while (tileGroup.Positions.Count > 0)
				{
					Vector2i vector2i2 = tileGroup.Positions[num7];
					tileGroup.Positions.RemoveAt(num7);
					num7 = instance.Range(tileGroup.Positions.Count);
					int num8 = vector2i2.x / 1;
					int num9 = vector2i2.y / 1;
					if (this.terrainTypeMap.Get(num8, num9) == TerrainType.none)
					{
						if (num4 > 0)
						{
							if (num4 >= 2)
							{
								num8 &= -2;
								num9 &= -2;
								this.terrainTypeMap.Set(num8, num9, TerrainType.hills);
								this.terrainTypeMap.Set(num8 + 1, num9, TerrainType.hills);
								this.terrainTypeMap.Set(num8, num9 + 1, TerrainType.hills);
								this.terrainTypeMap.Set(num8 + 1, num9 + 1, TerrainType.hills);
							}
							num4 -= 4;
						}
						else if (num5 > 0)
						{
							num5--;
							this.terrainTypeMap.Set(num8, num9, TerrainType.mountains);
							if (num5 > 0 && instance.Float() < 0.8f)
							{
								int num10 = instance.Range(4);
								for (int l = 0; l < 4; l++)
								{
									num10 = (num10 + 1) & 3;
									Vector2i vector2i3;
									vector2i3.x = vector2i2.x + WorldBuilder.directions4way[num10].x;
									vector2i3.y = vector2i2.y + WorldBuilder.directions4way[num10].y;
									int num11 = tileGroup.Positions.IndexOf(vector2i3);
									if (num11 >= 0)
									{
										num8 = vector2i3.x / 1;
										num9 = vector2i3.y / 1;
										if (this.terrainTypeMap.Get(num8, num9) == TerrainType.none)
										{
											num7 = num11;
											break;
										}
									}
								}
							}
						}
						else
						{
							this.terrainTypeMap.Set(num8, num9, TerrainType.plains);
						}
					}
				}
			}
		}

		// Token: 0x0600B7CB RID: 47051 RVA: 0x0044A450 File Offset: 0x00448650
		[PublicizedFrom(EAccessModifier.Private)]
		public List<WorldBuilder.BiomeTypeData> CalcBiomeTileBiomeData(int totalTiles, int multiple)
		{
			List<WorldBuilder.BiomeTypeData> list = new List<WorldBuilder.BiomeTypeData>();
			float num = (float)(this.ForestBiomeWeight + this.BurntForestBiomeWeight + this.DesertBiomeWeight + this.SnowBiomeWeight + this.WastelandBiomeWeight);
			num *= (float)multiple;
			List<WorldBuilder.BiomeTypeData> list2 = new List<WorldBuilder.BiomeTypeData>();
			for (int i = 0; i < multiple; i++)
			{
				list2.Add(new WorldBuilder.BiomeTypeData(BiomeType.forest, (float)this.ForestBiomeWeight / num, totalTiles));
				list2.Add(new WorldBuilder.BiomeTypeData(BiomeType.burntForest, (float)this.BurntForestBiomeWeight / num, totalTiles));
				list2.Add(new WorldBuilder.BiomeTypeData(BiomeType.desert, (float)this.DesertBiomeWeight / num, totalTiles));
				list2.Add(new WorldBuilder.BiomeTypeData(BiomeType.snow, (float)this.SnowBiomeWeight / num, totalTiles));
				list2.Add(new WorldBuilder.BiomeTypeData(BiomeType.wasteland, (float)this.WastelandBiomeWeight / num, totalTiles));
				list2 = (from b in list2
					where b.Percent > 0f
					orderby -b.Percent
					select b).ToList<WorldBuilder.BiomeTypeData>();
				list.AddRange(list2);
				list2.Clear();
			}
			int num2 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				num2 += list[j].TileCount;
			}
			int num3 = 0;
			for (int k = num2; k < totalTiles; k++)
			{
				list[num3].TileCount++;
				num3 = (num3 + 1) % list.Count;
			}
			return list;
		}

		// Token: 0x0600B7CC RID: 47052 RVA: 0x0044A5D0 File Offset: 0x004487D0
		[PublicizedFrom(EAccessModifier.Private)]
		public void GenerateBiomeTiles()
		{
			int num = this.WorldSize / 256;
			float num2 = (float)num * 0.5f;
			int num3 = num * num;
			this.biomeMap = new DataMap<BiomeType>(num, BiomeType.none);
			int num4 = ((this.biomeLayout != WorldBuilder.BiomeLayout.Circle2) ? 1 : 2);
			List<WorldBuilder.BiomeTypeData> list = this.CalcBiomeTileBiomeData(num3, num4);
			BiomeType biomeType = BiomeType.none;
			if (this.biomeLayout == WorldBuilder.BiomeLayout.CenterForest)
			{
				biomeType = BiomeType.forest;
			}
			if (this.biomeLayout == WorldBuilder.BiomeLayout.CenterWasteland)
			{
				biomeType = BiomeType.wasteland;
			}
			int num5 = 1;
			float num6 = num2;
			float num7 = num2;
			for (int i = 0; i < num5; i++)
			{
				if (this.biomeLayout == WorldBuilder.BiomeLayout.Line)
				{
					float num8 = (float)num * 0.4f;
					float num9 = (float)(Rand.Instance.Range(4) * 90) * 0.017453292f;
					Vector2 vector;
					vector.x = num6 - Mathf.Cos(num9) * num8;
					vector.y = num7 - Mathf.Sin(num9) * num8;
					Vector2 vector2;
					vector2.x = num6 + Mathf.Cos(num9) * num8;
					vector2.y = num7 + Mathf.Sin(num9) * num8;
					float num10 = 0f;
					float num11 = 1f / (float)(list.Count - 1);
					for (int j = 0; j < list.Count; j++)
					{
						WorldBuilder.BiomeTypeData biomeTypeData = list[j];
						Vector2 vector3 = Vector2.Lerp(vector, vector2, num10);
						biomeTypeData.Center = new Vector2i((int)vector3.x, (int)vector3.y);
						biomeTypeData.TileCount--;
						this.biomeMap.Set(biomeTypeData.Center.x, biomeTypeData.Center.y, biomeTypeData.Type);
						num10 += num11;
					}
				}
				else
				{
					float num12 = (float)num * 0.4f;
					int num13 = -1;
					int num14 = list.Count - 1;
					if (this.biomeLayout == WorldBuilder.BiomeLayout.Circle)
					{
						num14++;
					}
					if (this.biomeLayout == WorldBuilder.BiomeLayout.Circle2)
					{
						num13 = list.Count / 2;
						num14 = num13;
					}
					float num15 = (float)Rand.Instance.Angle();
					float num16 = 360f / (float)num14 / (float)num5;
					if (Rand.Instance.Float() < 0.5f)
					{
						num16 *= -1f;
					}
					for (int k = 0; k < list.Count; k++)
					{
						WorldBuilder.BiomeTypeData biomeTypeData2 = list[k];
						if (biomeTypeData2.Type == biomeType)
						{
							biomeTypeData2.Center = new Vector2i((int)num6, (int)num7);
						}
						else
						{
							if (k == num13)
							{
								num15 += (float)Rand.Instance.Range(70, 200);
								num12 *= 0.5f;
							}
							float num17 = num6 + Mathf.Cos(num15 * 0.017453292f) * num12;
							float num18 = num7 + Mathf.Sin(num15 * 0.017453292f) * num12;
							num15 += num16;
							biomeTypeData2.Center = new Vector2i((int)num17, (int)num18);
						}
						biomeTypeData2.TileCount--;
						this.biomeMap.Set(biomeTypeData2.Center.x, biomeTypeData2.Center.y, biomeTypeData2.Type);
					}
				}
				num6 += 3f;
				num7 += 2f;
			}
			int num19 = num3 - list.Count;
			int num20 = 1 + this.WorldSize / 2048;
			int num21;
			do
			{
				num21 = num19;
				for (int l = 0; l < list.Count; l++)
				{
					WorldBuilder.BiomeTypeData biomeTypeData3 = list[l];
					if (biomeTypeData3.TileCount > 0)
					{
						int num22 = 0;
						if (biomeTypeData3.Type == biomeType)
						{
							num22 = num20;
						}
						int num23 = 1 + (int)(biomeTypeData3.Percent * 4f);
						int num24 = 0;
						while (num24 < num23 && this.FindBiomeEmptyAndSet(biomeTypeData3, num22))
						{
							biomeTypeData3.TileCount--;
							num19--;
							if (biomeTypeData3.TileCount <= 0)
							{
								break;
							}
							num24++;
						}
					}
				}
			}
			while (num19 != num21);
			do
			{
				num21 = num19;
				for (int m = 0; m < list.Count; m++)
				{
					WorldBuilder.BiomeTypeData biomeTypeData4 = list[m];
					if (biomeTypeData4.Type != BiomeType.forest && this.FindBiomeEmptyAndSet(biomeTypeData4, 0))
					{
						num19--;
					}
				}
			}
			while (num19 != num21);
			this.biomeMap.Replace(BiomeType.none, BiomeType.forest);
		}

		// Token: 0x0600B7CD RID: 47053 RVA: 0x0044A9F8 File Offset: 0x00448BF8
		[PublicizedFrom(EAccessModifier.Private)]
		public bool FindBiomeEmptyAndSet(WorldBuilder.BiomeTypeData _b, int _edge)
		{
			int num = this.WorldSize / 256 - 1 - _edge;
			int num2 = _b.Center.x + Rand.Instance.Range(-2, 3);
			int num3 = _b.Center.y + Rand.Instance.Range(-2, 3);
			for (int i = 1; i <= 39; i++)
			{
				int num4 = Utils.FastMax(_edge, num2 - i);
				int num5 = Utils.FastMin(num2 + i, num);
				int num6 = Utils.FastMax(_edge, num3 - i);
				int num7 = Utils.FastMin(num3 + i, num);
				for (int j = 0; j <= i; j++)
				{
					int num8 = num3 - i;
					int num9;
					if (num8 >= num6 && num8 <= num7)
					{
						num9 = num2 - j;
						if (num9 >= num4 && num9 <= num5 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
						num9 = num2 + j;
						if (num9 >= num4 && num9 <= num5 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
					}
					num8 = num3 + i;
					if (num8 >= num6 && num8 <= num7)
					{
						num9 = num2 - j;
						if (num9 >= num4 && num9 <= num5 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
						num9 = num2 + j;
						if (num9 >= num4 && num9 <= num5 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
					}
					num9 = num2 - i;
					if (num9 >= num4 && num9 <= num5)
					{
						num8 = num3 - j;
						if (num8 >= num6 && num8 <= num7 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
						num8 = num3 + j;
						if (num8 >= num6 && num8 <= num7 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
					}
					num9 = num2 + i;
					if (num9 >= num4 && num9 <= num5)
					{
						num8 = num3 - j;
						if (num8 >= num6 && num8 <= num7 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
						num8 = num3 + j;
						if (num8 >= num6 && num8 <= num7 && this.biomeMap.Get(num9, num8) == BiomeType.none && this.HasBiomeNeighbor(num9, num8, _b.Type))
						{
							this.biomeMap.Set(num9, num8, _b.Type);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600B7CE RID: 47054 RVA: 0x0044AD84 File Offset: 0x00448F84
		[PublicizedFrom(EAccessModifier.Private)]
		public bool HasBiomeNeighbor(int _x, int _y, BiomeType _biomeType)
		{
			int num = this.WorldSize / 256;
			int num2 = _x - 1;
			if (num2 >= 0 && this.biomeMap.Get(num2, _y) == _biomeType)
			{
				return true;
			}
			num2 = _x + 1;
			if (num2 < num && this.biomeMap.Get(num2, _y) == _biomeType)
			{
				return true;
			}
			int num3 = _y - 1;
			if (num3 >= 0 && this.biomeMap.Get(_x, num3) == _biomeType)
			{
				return true;
			}
			num3 = _y + 1;
			return num3 < num && this.biomeMap.Get(_x, num3) == _biomeType;
		}

		// Token: 0x0600B7CF RID: 47055 RVA: 0x0044AE08 File Offset: 0x00449008
		[PublicizedFrom(EAccessModifier.Private)]
		public BiomeType GetBiomeFromNeighbors(int _x, int _y)
		{
			int num = this.WorldSize / 256;
			int num2 = _x - 1;
			if (num2 >= 0)
			{
				BiomeType biomeType = this.biomeMap.Get(num2, _y);
				if (biomeType != BiomeType.none && biomeType != BiomeType.wasteland)
				{
					return biomeType;
				}
			}
			num2 = _x + 1;
			if (num2 < num)
			{
				BiomeType biomeType2 = this.biomeMap.Get(num2, _y);
				if (biomeType2 != BiomeType.none && biomeType2 != BiomeType.wasteland)
				{
					return biomeType2;
				}
			}
			int num3 = _y - 1;
			if (num3 >= 0)
			{
				BiomeType biomeType3 = this.biomeMap.Get(_x, num3);
				if (biomeType3 != BiomeType.none && biomeType3 != BiomeType.wasteland)
				{
					return biomeType3;
				}
			}
			num3 = _y + 1;
			if (num3 < num)
			{
				BiomeType biomeType4 = this.biomeMap.Get(_x, num3);
				if (biomeType4 != BiomeType.none && biomeType4 != BiomeType.wasteland)
				{
					return biomeType4;
				}
			}
			return BiomeType.none;
		}

		// Token: 0x0600B7D0 RID: 47056 RVA: 0x0044AEC4 File Offset: 0x004490C4
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializeRWGTTW(Stream stream)
		{
			World world = new World();
			WorldState worldState = new WorldState();
			worldState.SetFrom(world, EnumChunkProviderId.ChunkDataDriven);
			worldState.ResetDynamicData();
			worldState.Save(stream);
		}

		// Token: 0x0600B7D1 RID: 47057 RVA: 0x0044AEF4 File Offset: 0x004490F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializeDynamicProperties(Stream stream)
		{
			DynamicProperties dynamicProperties = new DynamicProperties();
			dynamicProperties.Values["SchemaVersion"] = 1.ToString();
			dynamicProperties.Values["Scale"] = "1";
			dynamicProperties.Values["HeightMapSize"] = string.Format("{0},{0}", this.WorldSize);
			dynamicProperties.Values["Modes"] = "Survival,SurvivalSP,SurvivalMP,Creative";
			dynamicProperties.Values["FixedWaterLevel"] = "false";
			dynamicProperties.Values["RandomGeneratedWorld"] = "true";
			dynamicProperties.Values["GameVersion"] = Constants.cVersionInformation.SerializableString;
			dynamicProperties.Values["Seed"] = this.Seed.ToString();
			DynamicProperties dynamicProperties2 = new DynamicProperties();
			dynamicProperties.Classes["Generation"] = dynamicProperties2;
			dynamicProperties2.Values["Seed"] = this.WorldSeedName;
			dynamicProperties2.Values["Towns"] = this.Towns.ToString();
			dynamicProperties2.Values["Wilderness"] = this.Wilderness.ToString();
			dynamicProperties2.Values["Lakes"] = this.Lakes.ToString();
			dynamicProperties2.Values["Rivers"] = this.Rivers.ToString();
			dynamicProperties2.Values["Cracks"] = this.Canyons.ToString();
			dynamicProperties2.Values["Craters"] = this.Craters.ToString();
			dynamicProperties2.Values["Plains"] = this.Plains.ToString();
			dynamicProperties2.Values["Hills"] = this.Hills.ToString();
			dynamicProperties2.Values["Mountains"] = this.Mountains.ToString();
			dynamicProperties2.Values["Forest"] = this.ForestBiomeWeight.ToString();
			dynamicProperties2.Values["BurntForest"] = this.BurntForestBiomeWeight.ToString();
			dynamicProperties2.Values["Desert"] = this.DesertBiomeWeight.ToString();
			dynamicProperties2.Values["Snow"] = this.SnowBiomeWeight.ToString();
			dynamicProperties2.Values["Wasteland"] = this.WastelandBiomeWeight.ToString();
			dynamicProperties.Save("MapInfo", stream);
		}

		// Token: 0x0600B7D2 RID: 47058 RVA: 0x0044B1A4 File Offset: 0x004493A4
		public void AddPreviewLinePlus(Vector2i pos, Color32 color, int size)
		{
			WorldBuilder.PreviewPoint previewPoint;
			previewPoint.pos = pos;
			previewPoint.color = color;
			previewPoint.size = size;
			this.previewPoints.Add(previewPoint);
		}

		// Token: 0x0600B7D3 RID: 47059 RVA: 0x0044B1D8 File Offset: 0x004493D8
		public void PreviewTextureUpdateBiomes(Color32[] previewColors)
		{
			Color32 color = new Color32(0, 0, 0, byte.MaxValue);
			int num = 0;
			int num2 = this.WorldSize / this.BiomeSize;
			for (int i = 0; i < this.BiomeSize; i++)
			{
				int num3 = num;
				for (int j = 0; j < this.BiomeSize; j++)
				{
					Color32 color2 = this.biomeDest[j + i * this.BiomeSize];
					color.r = color2.r / 2;
					color.g = color2.g / 2;
					color.b = color2.b / 2;
					for (int k = 0; k < num2; k++)
					{
						int num4 = num3 + k * this.WorldSize;
						for (int l = 0; l < num2; l++)
						{
							previewColors[num4 + l] = color;
						}
					}
					num3 += num2;
				}
				num += num2 * this.WorldSize;
			}
		}

		// Token: 0x0600B7D4 RID: 47060 RVA: 0x0044B2CF File Offset: 0x004494CF
		public IEnumerator PreviewTextureUpdateFinal(Color32[] previewColors)
		{
			yield return this.SetMessage(Localization.Get("xuiRwgCreatingPreview", false, null), true, false);
			MicroStopwatch msReset = new MicroStopwatch(true);
			if (this.Townships != null)
			{
				StampGroup roadLayer = new StampGroup("Road Layer");
				foreach (Township township in this.Townships)
				{
					if (township.Streets.Count > 0)
					{
						foreach (StreetTile streetTile in township.Streets.Values)
						{
							if (streetTile.Township != null)
							{
								roadLayer.Stamps.Add(streetTile.GetStamp());
							}
						}
					}
					if (msReset.ElapsedMilliseconds > 500L)
					{
						yield return null;
						msReset.ResetAndRestart();
					}
				}
				List<Township>.Enumerator enumerator = default(List<Township>.Enumerator);
				this.StampManager.DrawStampGroup(roadLayer, previewColors, this.WorldSize, 1f);
				roadLayer = null;
			}
			yield return null;
			msReset.ResetAndRestart();
			Color32 waterColor = new Color32(0, 0, byte.MaxValue, byte.MaxValue);
			Color32 radColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			int num3;
			for (int i = 0; i < this.roadDest.Length; i = num3 + 1)
			{
				int num = i % this.WorldSize;
				int num2 = i / this.WorldSize;
				if (this.roadDest[i].a > 0)
				{
					previewColors[i] = this.roadDest[i];
				}
				if (this.data.GetWater(i) > 0)
				{
					previewColors[i] = waterColor;
				}
				if (this.GetRad(num, num2) > 0)
				{
					previewColors[i] = radColor;
				}
				if (i % 50000 == 0 && msReset.ElapsedMilliseconds > 500L)
				{
					yield return null;
					msReset.ResetAndRestart();
				}
				num3 = i;
			}
			Color32 color = new Color32(200, 200, byte.MaxValue, byte.MaxValue);
			Color32 color2 = new Color32(0, 0, 50, byte.MaxValue);
			for (int j = 0; j < this.playerSpawns.Count; j++)
			{
				WorldBuilder.PlayerSpawn playerSpawn = this.playerSpawns[j];
				int num4 = (int)playerSpawn.Position.x + (int)playerSpawn.Position.z * this.WorldSize;
				previewColors[num4 - this.WorldSize - 1] = color2;
				previewColors[num4 - this.WorldSize] = color;
				previewColors[num4 - this.WorldSize + 1] = color2;
				previewColors[num4 - 1] = color;
				previewColors[num4] = color2;
				previewColors[num4 + 1] = color;
				previewColors[num4 + this.WorldSize - 1] = color2;
				previewColors[num4 + this.WorldSize] = color;
				previewColors[num4 + this.WorldSize + 1] = color2;
			}
			int num5 = previewColors.Length;
			for (int k = 0; k < this.previewPoints.Count; k++)
			{
				WorldBuilder.PreviewPoint previewPoint = this.previewPoints[k];
				int num6 = previewPoint.pos.x + previewPoint.pos.y * this.WorldSize;
				int num7 = previewPoint.size / 2;
				int num8 = num6 - num7;
				for (int l = 0; l < previewPoint.size; l++)
				{
					previewColors[num8 + l] = previewPoint.color;
				}
				num8 = num6 - num7 * this.WorldSize;
				for (int m = 0; m < previewPoint.size; m++)
				{
					int num9 = num8 + m * this.WorldSize;
					if ((ulong)num9 < (ulong)((long)num5))
					{
						previewColors[num9] = previewPoint.color;
					}
				}
			}
			this.previewPoints.Clear();
			yield return null;
			yield break;
			yield break;
		}

		// Token: 0x0600B7D5 RID: 47061 RVA: 0x0044B2E5 File Offset: 0x004494E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public StreetTile GetStreetTileGrid(Vector2i pos)
		{
			return this.GetStreetTileGrid(pos.x, pos.y);
		}

		// Token: 0x0600B7D6 RID: 47062 RVA: 0x0044B2F9 File Offset: 0x004494F9
		public StreetTile GetStreetTileGrid(int x, int y)
		{
			if ((ulong)x >= (ulong)((long)this.StreetTileMapWidth))
			{
				return null;
			}
			if ((ulong)y >= (ulong)((long)this.StreetTileMapWidth))
			{
				return null;
			}
			return this.StreetTileMap[x + y * this.StreetTileMapWidth];
		}

		// Token: 0x0600B7D7 RID: 47063 RVA: 0x0044B326 File Offset: 0x00449526
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public StreetTile GetStreetTileWorld(Vector2i pos)
		{
			return this.GetStreetTileWorld(pos.x, pos.y);
		}

		// Token: 0x0600B7D8 RID: 47064 RVA: 0x0044B33A File Offset: 0x0044953A
		public StreetTile GetStreetTileWorld(int x, int y)
		{
			x /= 150;
			if ((ulong)x >= (ulong)((long)this.StreetTileMapWidth))
			{
				return null;
			}
			y /= 150;
			if ((ulong)y >= (ulong)((long)this.StreetTileMapWidth))
			{
				return null;
			}
			return this.StreetTileMap[x + y * this.StreetTileMapWidth];
		}

		// Token: 0x0600B7D9 RID: 47065 RVA: 0x0044B379 File Offset: 0x00449579
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetHeight(Vector2i pos)
		{
			return this.GetHeight(pos.x, pos.y);
		}

		// Token: 0x0600B7DA RID: 47066 RVA: 0x0044B38D File Offset: 0x0044958D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetHeight(int x, int y)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return 0f;
			}
			return this.data.HeightMap[x + y * this.WorldSize];
		}

		// Token: 0x0600B7DB RID: 47067 RVA: 0x0044B3C5 File Offset: 0x004495C5
		[BurstCompile(CompileSynchronously = true)]
		public static void AdjustHeights(ref NativeArray<float> _src, float _min)
		{
			WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.Invoke(ref _src, _min);
		}

		// Token: 0x0600B7DC RID: 47068 RVA: 0x0044B3CE File Offset: 0x004495CE
		public void SetHeight(int x, int y, float height)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return;
			}
			this.data.HeightMap[x + y * this.WorldSize] = height;
		}

		// Token: 0x0600B7DD RID: 47069 RVA: 0x0044B402 File Offset: 0x00449602
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetHeightTrusted(int x, int y, float height)
		{
			this.data.HeightMap[x + y * this.WorldSize] = height;
		}

		// Token: 0x0600B7DE RID: 47070 RVA: 0x0044B41F File Offset: 0x0044961F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TerrainType GetTerrainType(Vector2i pos)
		{
			return this.GetTerrainType(pos.x, pos.y);
		}

		// Token: 0x0600B7DF RID: 47071 RVA: 0x0044B434 File Offset: 0x00449634
		public TerrainType GetTerrainType(int x, int y)
		{
			x /= 256;
			if ((ulong)x >= (ulong)((long)this.terrainTypeMap.width))
			{
				return TerrainType.none;
			}
			y /= 256;
			if ((ulong)y >= (ulong)((long)this.terrainTypeMap.width))
			{
				return TerrainType.none;
			}
			return this.terrainTypeMap.Get(x, y);
		}

		// Token: 0x0600B7E0 RID: 47072 RVA: 0x0044B48C File Offset: 0x0044968C
		public BiomeType GetBiome(Vector2i pos)
		{
			return this.GetBiome(pos.x, pos.y);
		}

		// Token: 0x0600B7E1 RID: 47073 RVA: 0x0044B4A0 File Offset: 0x004496A0
		public BiomeType GetBiome(int x, int y)
		{
			int num = x / 8 + y / 8 * this.BiomeSize;
			if ((ulong)num >= (ulong)((long)(this.BiomeSize * this.BiomeSize)))
			{
				return BiomeType.forest;
			}
			Color32 color = this.biomeDest[num];
			BiomeType biomeType = BiomeType.forest;
			if (color.g == WorldBuilderConstants.burntForestCol.g)
			{
				biomeType = BiomeType.burntForest;
			}
			else if (color.g == WorldBuilderConstants.desertCol.g)
			{
				biomeType = BiomeType.desert;
			}
			else if (color.g == WorldBuilderConstants.snowCol.g)
			{
				biomeType = BiomeType.snow;
			}
			else if (color.g == WorldBuilderConstants.wastelandCol.g)
			{
				biomeType = BiomeType.wasteland;
			}
			return biomeType;
		}

		// Token: 0x0600B7E2 RID: 47074 RVA: 0x0044B536 File Offset: 0x00449736
		public void SetWater(int x, int y, byte height)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return;
			}
			this.data.waterDest[x + y * this.WorldSize] = (float)height;
		}

		// Token: 0x0600B7E3 RID: 47075 RVA: 0x0044B56B File Offset: 0x0044976B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte GetRad(int x, int y)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return 0;
			}
			return this.radDest[x / 32 + y / 32 * this.RadSize].r;
		}

		// Token: 0x0600B7E4 RID: 47076 RVA: 0x0044B5A5 File Offset: 0x004497A5
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializePrefabs(Stream stream)
		{
			this.PrefabManager.SavePrefabData(stream);
			if (this.PreviewWindow == null)
			{
				this.PrefabManager.UsedPrefabsWorld.Clear();
			}
		}

		// Token: 0x0600B7E5 RID: 47077 RVA: 0x0044B5CC File Offset: 0x004497CC
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializeRawHeightmap(Stream stream)
		{
			HeightMapUtils.SaveHeightMapRAW(stream, this.data.HeightMap.ToArray(), -1f);
		}

		// Token: 0x0600B7E6 RID: 47078 RVA: 0x0044B5EC File Offset: 0x004497EC
		[PublicizedFrom(EAccessModifier.Private)]
		public void DrawRoads(Color32[] dest)
		{
			this.SetTaskMessage(this.messageDrawRoads);
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			byte[] array = new byte[this.WorldSize * this.WorldSize];
			for (int i = 0; i < this.wildernessPaths.Count; i++)
			{
				this.wildernessPaths[i].DrawPathToRoadIds(array);
				this.SetTaskMessage(string.Format(this.messageDrawRoadsWilderness, 100 * i / this.wildernessPaths.Count));
			}
			for (int j = 0; j < this.highwayPaths.Count; j++)
			{
				this.highwayPaths[j].DrawPathToRoadIds(array);
				this.SetTaskMessage(string.Format(this.messageDrawRoadsProgress, 100 * j / this.highwayPaths.Count));
			}
			this.PathShared.ConvertIdsToColors(array, dest);
			Log.Out(string.Format("DrawRoads in {0}", (float)microStopwatch.ElapsedMilliseconds * 0.001f));
		}

		// Token: 0x0600B7E7 RID: 47079 RVA: 0x0044B6EC File Offset: 0x004498EC
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializePlayerSpawns(Stream stream)
		{
			using (StreamWriter streamWriter = new StreamWriter(stream, SdEncoding.UTF8NoBOM, 1024, true))
			{
				streamWriter.WriteLine("<spawnpoints>");
				if (this.playerSpawns != null)
				{
					for (int i = 0; i < this.playerSpawns.Count; i++)
					{
						WorldBuilder.PlayerSpawn playerSpawn = this.playerSpawns[i];
						streamWriter.WriteLine(string.Format("    <spawnpoint position=\"{0},{1},{2}\" rotation=\"0,{3},0\"/>", new object[]
						{
							(playerSpawn.Position.x - (float)this.WorldSize / 2f).ToCultureInvariantString(),
							playerSpawn.Position.y.ToCultureInvariantString(),
							(playerSpawn.Position.z - (float)this.WorldSize / 2f).ToCultureInvariantString(),
							playerSpawn.Rotation
						}));
					}
				}
				streamWriter.WriteLine("</spawnpoints>");
			}
		}

		// Token: 0x0600B7E8 RID: 47080 RVA: 0x0044B7EC File Offset: 0x004499EC
		public void SetTaskMessage(string _message)
		{
			this.taskMessage = _message;
		}

		// Token: 0x0600B7E9 RID: 47081 RVA: 0x0044B7F5 File Offset: 0x004499F5
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator TaskMessageUpdate()
		{
			yield return this.SetMessage(this.taskMessage, false, false);
			yield break;
		}

		// Token: 0x0600B7EA RID: 47082 RVA: 0x0044B804 File Offset: 0x00449A04
		public IEnumerator SetMessage(string _message, bool _logToConsole = false, bool _ignoreCancel = false)
		{
			string text = "\n";
			if (_message != null)
			{
				_message = string.Format("{0} {1}:{2:00}{3}{4}", new object[]
				{
					Localization.Get("xuiTime", false, null),
					this.totalMS.Elapsed.Minutes,
					this.totalMS.Elapsed.Seconds,
					text,
					_message
				});
			}
			if (!GameManager.IsDedicatedServer)
			{
				if (_message != null)
				{
					if (!_ignoreCancel && this.IsCanceled)
					{
						_message = "Canceling...";
					}
					if (this.PreviewWindow != null)
					{
						if (_message != this.setMessageLast)
						{
							this.setMessageLast = _message;
							XUiC_WorldGenerationWindow.Instance.StatusText = _message;
						}
					}
					else if (!XUiC_ProgressWindow.IsWindowOpen())
					{
						XUiC_ProgressWindow.Open(LocalPlayerUI.primaryUI, _message, null, false, true, true);
					}
					else if (_message != this.setMessageLast)
					{
						this.setMessageLast = _message;
						XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, _message, true);
					}
				}
				else
				{
					this.setMessageLast = string.Empty;
					XUiC_ProgressWindow.Close(LocalPlayerUI.primaryUI);
				}
				yield return this.endOfFrameHandle;
			}
			if (_logToConsole && _message != null)
			{
				Log.Out("WorldGenerator:" + _message.Replace("\n", ": "));
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600B7EB RID: 47083 RVA: 0x0044B828 File Offset: 0x00449A28
		public bool IsMessageElapsed()
		{
			if (this.messageMS.ElapsedMilliseconds > 600L)
			{
				this.messageMS.ResetAndRestart();
				return true;
			}
			return false;
		}

		// Token: 0x0600B7EC RID: 47084 RVA: 0x0044B84C File Offset: 0x00449A4C
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i getRotatedPoint(int x, int y, int cx, int cy, int angle)
		{
			return new Vector2i(Mathf.RoundToInt((float)((double)(x - cx) * Math.Cos((double)angle) - (double)(y - cy) * Math.Sin((double)angle) + (double)cx)), Mathf.RoundToInt((float)((double)(x - cx) * Math.Sin((double)angle) + (double)(y - cy) * Math.Cos((double)angle) + (double)cy)));
		}

		// Token: 0x0600B7ED RID: 47085 RVA: 0x0044B8AC File Offset: 0x00449AAC
		[PublicizedFrom(EAccessModifier.Private)]
		public void GetTerrainProperties(string biomeTypeName, string terrainTypeName, string comboTypeName, out Vector2 _scaleMinMax, out int _clusterCount, out int _clusterRadius, out float _clusterStrength, out bool useBiomeMask, out float biomeCutoff)
		{
			_scaleMinMax = Vector2.one;
			string text = this.thisWorldProperties.GetString(comboTypeName, "scale");
			if (text == string.Empty)
			{
				text = this.thisWorldProperties.GetString(terrainTypeName, "scale");
			}
			if (text != string.Empty)
			{
				_scaleMinMax = StringParsers.ParseVector2(text);
			}
			_scaleMinMax *= 0.5f;
			_clusterCount = 3;
			_clusterRadius = 85;
			_clusterStrength = 1f;
			text = this.thisWorldProperties.GetString(comboTypeName, "clusters");
			if (text == string.Empty)
			{
				text = this.thisWorldProperties.GetString(terrainTypeName, "clusters");
			}
			if (text != string.Empty)
			{
				Vector3 vector = StringParsers.ParseVector3(text, 0, -1);
				_clusterCount = (int)vector.x;
				_clusterRadius = (int)(256f * vector.y);
				_clusterStrength = vector.z;
			}
			useBiomeMask = false;
			text = this.thisWorldProperties.GetString(comboTypeName, "use_biome_mask");
			if (text == string.Empty)
			{
				text = this.thisWorldProperties.GetString(terrainTypeName, "use_biome_mask");
			}
			if (text != string.Empty)
			{
				useBiomeMask = StringParsers.ParseBool(text, 0, -1, true);
			}
			biomeCutoff = 0.1f;
			text = this.thisWorldProperties.GetString(comboTypeName, "biome_mask_min");
			if (text == string.Empty)
			{
				text = this.thisWorldProperties.GetString(terrainTypeName, "biome_mask_min");
			}
			if (text != string.Empty)
			{
				biomeCutoff = StringParsers.ParseFloat(text, 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600B7EE RID: 47086 RVA: 0x0044BA47 File Offset: 0x00449C47
		public static string GetGeneratedWorldName(string _worldSeedName, int _worldSize = 8192)
		{
			return RandomCountyNameGenerator.GetName(_worldSeedName.GetHashCode() + _worldSize);
		}

		// Token: 0x0600B7EF RID: 47087 RVA: 0x0044BA58 File Offset: 0x00449C58
		[PublicizedFrom(EAccessModifier.Private)]
		public static int distanceSqr(Vector2i pointA, Vector2i pointB)
		{
			Vector2i vector2i = pointA - pointB;
			return vector2i.x * vector2i.x + vector2i.y * vector2i.y;
		}

		// Token: 0x0600B7F0 RID: 47088 RVA: 0x0044BA88 File Offset: 0x00449C88
		[PublicizedFrom(EAccessModifier.Private)]
		public static float distanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x0600B7F1 RID: 47089 RVA: 0x0044BAB8 File Offset: 0x00449CB8
		public int GetCount(string _name, WorldBuilder.GenerationSelections _selection, GameRandom _rand = null)
		{
			float num = -1f;
			float num2 = 0f;
			float num3 = 0f;
			this.thisWorldProperties.ParseVec(_name, "count", ref num, ref num2, ref num3);
			if (num < 0f)
			{
				return -1;
			}
			float num4 = num;
			if (_selection == WorldBuilder.GenerationSelections.Default)
			{
				num4 = num2;
			}
			else if (_selection == WorldBuilder.GenerationSelections.Many)
			{
				num4 = num3;
			}
			int num5 = (int)num4;
			if (_rand != null && _rand.RandomFloat < num4 - (float)num5)
			{
				num5++;
			}
			return num5;
		}

		// Token: 0x0600B7F2 RID: 47090 RVA: 0x0044BB24 File Offset: 0x00449D24
		[PublicizedFrom(EAccessModifier.Private)]
		public void TestGenerateHeights()
		{
			for (int i = 0; i < this.WorldSize; i++)
			{
				float num = 0f;
				for (int j = 0; j < this.WorldSize; j++)
				{
					int num2 = j + i * this.WorldSize;
					this.data.HeightMap[num2] = num;
					if ((j & 3) == 3)
					{
						num += 2f;
						if (num > 255f)
						{
							num = 0f;
						}
					}
				}
			}
		}

		// Token: 0x0600B7FD RID: 47101 RVA: 0x0044BF28 File Offset: 0x0044A128
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearWaterUnderTerrain$BurstManaged(ref WorldBuilder.Data _data, ref NativeArray<float> _terrain, int startX, int endX, int startY, int endY)
		{
			for (int i = startY; i <= endY; i++)
			{
				for (int j = startX; j <= endX; j++)
				{
					int num = j + i * _data.WorldSize;
					if (_terrain[num] - 0.5f > _data.waterDest[num])
					{
						_data.waterDest[num] = 0f;
					}
				}
			}
		}

		// Token: 0x0600B7FE RID: 47102 RVA: 0x0044BF88 File Offset: 0x0044A188
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FinalizeWater$BurstManaged(ref WorldBuilder.Data _data, float _WaterHeight)
		{
			for (int i = 0; i < _data.HeightMap.Length; i++)
			{
				if (_data.HeightMap[i] - 0.5f > _data.waterDest[i])
				{
					_data.waterDest[i] = 0f;
				}
				else
				{
					_data.waterDest[i] = _WaterHeight;
				}
			}
		}

		// Token: 0x0600B7FF RID: 47103 RVA: 0x0044BFEC File Offset: 0x0044A1EC
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SmoothRoadTerrainTask$BurstManaged(ref WorldBuilder.Data _data, ref NativeArray<Color32> roadMask, ref NativeArray<float> _heightMap, int WorldSize)
		{
			int num = WorldSize * WorldSize;
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < WorldSize; i++)
			{
				for (int j = 0; j < WorldSize; j++)
				{
					int num2 = j + i * WorldSize;
					if (_data.poiHeightMask[num2] > 0)
					{
						nativeArray[num2] = 1000;
					}
					else
					{
						int r = (int)roadMask[num2].r;
						if (r + (int)roadMask[num2].g > 0)
						{
							ref NativeArray<float> ptr = ref _heightMap;
							int num3 = num2;
							ptr[num3] += 0.0008f;
							nativeArray[num2] = 200;
							int num4 = 80;
							int num5 = 3;
							int num6 = 30;
							if (r > 0)
							{
								nativeArray[num2] = 255;
								num4 = 60;
								num5 = 6;
								num6 = 8;
							}
							for (int k = 1; k <= num5; k++)
							{
								for (int l = 0; l < 8; l++)
								{
									int num7 = j + WorldBuilder.directions8way[l].x * k;
									if ((ulong)num7 < (ulong)((long)WorldSize))
									{
										int num8 = i + WorldBuilder.directions8way[l].y * k;
										if ((ulong)num8 < (ulong)((long)WorldSize))
										{
											int num9 = num7 + num8 * WorldSize;
											if (num4 > (int)nativeArray[num9])
											{
												nativeArray[num9] = (ushort)num4;
											}
										}
									}
								}
								num4 -= num6;
							}
						}
					}
				}
			}
			int num10 = WorldSize - 1;
			int num11 = WorldSize - 1;
			NativeArray<float> nativeArray2 = new NativeArray<float>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			int num12 = 6;
			while (num12-- > 0)
			{
				_heightMap.CopyTo(nativeArray2);
				for (int m = 1; m < num11; m++)
				{
					int num13 = m * WorldSize;
					for (int n = 1; n < num10; n++)
					{
						int num14 = n + num13;
						if (roadMask[num14].r != 0 && nativeArray[num14] < 1000)
						{
							float num16;
							float num15 = (num16 = nativeArray2[num14]);
							int num17 = num14 - WorldSize - 1;
							float num18 = num15;
							if (nativeArray[num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.25f;
							num18 = num15;
							if (nativeArray[++num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.5f;
							num18 = num15;
							if (nativeArray[++num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.25f;
							num17 = num14 - 1;
							num18 = num15;
							if (nativeArray[num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.5f;
							num17 += 2;
							num18 = num15;
							if (nativeArray[num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.5f;
							num17 = num14 + WorldSize - 1;
							num18 = num15;
							if (nativeArray[num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.25f;
							num18 = num15;
							if (nativeArray[++num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.5f;
							num18 = num15;
							if (nativeArray[++num17] >= 255)
							{
								num18 = nativeArray2[num17];
							}
							num16 += num18 * 0.25f;
							_heightMap[num14] = num16 / 4f;
						}
					}
				}
				_data.messageCnt++;
			}
			_data.messageCnt = 100;
			int num19 = 30;
			while (num19-- > 0)
			{
				_heightMap.CopyTo(nativeArray2);
				for (int num20 = 1; num20 < num11; num20++)
				{
					int num21 = num20 * WorldSize;
					for (int num22 = 1; num22 < num10; num22++)
					{
						int num23 = num22 + num21;
						int num24 = (int)nativeArray[num23];
						if (num24 != 0 && num24 <= 200)
						{
							int num25 = 0;
							float num26 = 0f;
							int num27 = num23 - WorldSize - 1;
							int num28 = (int)(nativeArray[num27] / 2);
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							num28 = (int)nativeArray[++num27];
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							num28 = (int)(nativeArray[++num27] / 2);
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							num27 = num23 - 1;
							num28 = (int)nativeArray[num27];
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							num27 = num23 + 1;
							num28 = (int)nativeArray[num27];
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							num27 = num23 + WorldSize - 1;
							num28 = (int)(nativeArray[num27] / 2);
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							num28 = (int)nativeArray[++num27];
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							num28 = (int)(nativeArray[++num27] / 2);
							num26 += nativeArray2[num27] * (float)num28;
							num25 += num28;
							if (num25 > 0)
							{
								if (num24 < 200)
								{
									float num29 = (float)num24 * 0.005f;
									_heightMap[num23] = _heightMap[num23] * (1f - num29) + num26 / (float)num25 * num29;
								}
								else
								{
									_heightMap[num23] = num26 / (float)num25;
								}
							}
						}
					}
				}
				_data.messageCnt++;
			}
			nativeArray.Dispose();
			nativeArray2.Dispose();
		}

		// Token: 0x0600B800 RID: 47104 RVA: 0x0044C5F4 File Offset: 0x0044A7F4
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AdjustHeights$BurstManaged(ref NativeArray<float> _src, float _min)
		{
			for (int i = 0; i < _src.Length; i++)
			{
				_src[i] = Utils.FastMax(_src[i], _min);
			}
		}

		// Token: 0x04008956 RID: 35158
		public const int HeightMax = 255;

		// Token: 0x04008957 RID: 35159
		[PublicizedFrom(EAccessModifier.Private)]
		public const int groundHeight = 35;

		// Token: 0x04008958 RID: 35160
		public int WaterHeight = 30;

		// Token: 0x04008959 RID: 35161
		public readonly DistrictPlanner DistrictPlanner;

		// Token: 0x0400895A RID: 35162
		public readonly HighwayPlanner HighwayPlanner;

		// Token: 0x0400895B RID: 35163
		public readonly PathingUtils PathingUtils;

		// Token: 0x0400895C RID: 35164
		public readonly PathShared PathShared;

		// Token: 0x0400895D RID: 35165
		public readonly POISmoother POISmoother;

		// Token: 0x0400895E RID: 35166
		public readonly PrefabManager PrefabManager;

		// Token: 0x0400895F RID: 35167
		public readonly StampManager StampManager;

		// Token: 0x04008960 RID: 35168
		public readonly StreetTileShared StreetTileShared;

		// Token: 0x04008961 RID: 35169
		public readonly TownPlanner TownPlanner;

		// Token: 0x04008962 RID: 35170
		public readonly TownshipShared TownshipShared;

		// Token: 0x04008963 RID: 35171
		public readonly WildernessPathPlanner WildernessPathPlanner;

		// Token: 0x04008964 RID: 35172
		public readonly WildernessPlanner WildernessPlanner;

		// Token: 0x04008965 RID: 35173
		public string WorldName;

		// Token: 0x04008966 RID: 35174
		public string WorldSeedName;

		// Token: 0x04008967 RID: 35175
		public string WorldPath;

		// Token: 0x04008968 RID: 35176
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataStorageType StorageLocation;

		// Token: 0x04008969 RID: 35177
		[PublicizedFrom(EAccessModifier.Private)]
		public MicroStopwatch totalMS;

		// Token: 0x0400896A RID: 35178
		public bool IsCanceled;

		// Token: 0x0400896B RID: 35179
		public bool IsFinished;

		// Token: 0x0400896C RID: 35180
		[PublicizedFrom(EAccessModifier.Private)]
		public Color32[] roadDest;

		// Token: 0x0400896D RID: 35181
		public int WorldSize = 8192;

		// Token: 0x0400896E RID: 35182
		public int WorldSizeDistDiv;

		// Token: 0x0400896F RID: 35183
		public const int BiomeSizeDiv = 8;

		// Token: 0x04008970 RID: 35184
		public int BiomeSize;

		// Token: 0x04008971 RID: 35185
		public int RadSize;

		// Token: 0x04008972 RID: 35186
		public int Seed = 12345;

		// Token: 0x04008973 RID: 35187
		public int Plains = 4;

		// Token: 0x04008974 RID: 35188
		public int Hills = 4;

		// Token: 0x04008975 RID: 35189
		public int Mountains = 2;

		// Token: 0x04008976 RID: 35190
		public WorldBuilder.GenerationSelections Canyons = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008977 RID: 35191
		public WorldBuilder.GenerationSelections Craters = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008978 RID: 35192
		public WorldBuilder.GenerationSelections Lakes = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008979 RID: 35193
		public WorldBuilder.GenerationSelections Rivers = WorldBuilder.GenerationSelections.Default;

		// Token: 0x0400897A RID: 35194
		public WorldBuilder.GenerationSelections Towns = WorldBuilder.GenerationSelections.Default;

		// Token: 0x0400897B RID: 35195
		public WorldBuilder.GenerationSelections Wilderness = WorldBuilder.GenerationSelections.Default;

		// Token: 0x0400897C RID: 35196
		public StreetTile[] StreetTileMap;

		// Token: 0x0400897D RID: 35197
		public int StreetTileMapWidth;

		// Token: 0x0400897E RID: 35198
		[PublicizedFrom(EAccessModifier.Private)]
		public DataMap<BiomeType> biomeMap;

		// Token: 0x0400897F RID: 35199
		[PublicizedFrom(EAccessModifier.Private)]
		public DataMap<TerrainType> terrainTypeMap;

		// Token: 0x04008980 RID: 35200
		public List<Rect> waterRects = new List<Rect>();

		// Token: 0x04008981 RID: 35201
		public List<Township> Townships = new List<Township>();

		// Token: 0x04008982 RID: 35202
		public int WildernessPrefabCount;

		// Token: 0x04008983 RID: 35203
		[PublicizedFrom(EAccessModifier.Private)]
		public string worldSizeName;

		// Token: 0x04008984 RID: 35204
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicProperties thisWorldProperties;

		// Token: 0x04008985 RID: 35205
		[PublicizedFrom(EAccessModifier.Private)]
		public const int WorldTileSize = 1024;

		// Token: 0x04008986 RID: 35206
		[PublicizedFrom(EAccessModifier.Private)]
		public const int BiomeTileSize = 256;

		// Token: 0x04008987 RID: 35207
		[PublicizedFrom(EAccessModifier.Private)]
		public const int TerrainTileSize = 256;

		// Token: 0x04008988 RID: 35208
		[PublicizedFrom(EAccessModifier.Private)]
		public const int terrainToBiomeTileScale = 1;

		// Token: 0x04008989 RID: 35209
		[PublicizedFrom(EAccessModifier.Private)]
		public const int RadTileSize = 32;

		// Token: 0x0400898A RID: 35210
		public WorldBuilder.BiomeLayout biomeLayout;

		// Token: 0x0400898B RID: 35211
		public int ForestBiomeWeight = 13;

		// Token: 0x0400898C RID: 35212
		[PublicizedFrom(EAccessModifier.Private)]
		public int BurntForestBiomeWeight = 18;

		// Token: 0x0400898D RID: 35213
		[PublicizedFrom(EAccessModifier.Private)]
		public int DesertBiomeWeight = 22;

		// Token: 0x0400898E RID: 35214
		[PublicizedFrom(EAccessModifier.Private)]
		public int SnowBiomeWeight = 23;

		// Token: 0x0400898F RID: 35215
		[PublicizedFrom(EAccessModifier.Private)]
		public int WastelandBiomeWeight = 24;

		// Token: 0x04008990 RID: 35216
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<BiomeType, Color32> biomeColors = new Dictionary<BiomeType, Color32>();

		// Token: 0x04008991 RID: 35217
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cPlayerSpawnsNeeded = 12;

		// Token: 0x04008992 RID: 35218
		[PublicizedFrom(EAccessModifier.Private)]
		public List<WorldBuilder.PlayerSpawn> playerSpawns;

		// Token: 0x04008993 RID: 35219
		public List<Path> highwayPaths = new List<Path>();

		// Token: 0x04008994 RID: 35220
		public List<Path> wildernessPaths = new List<Path>();

		// Token: 0x04008995 RID: 35221
		public List<Vector2i> TraderCenterPositions = new List<Vector2i>();

		// Token: 0x04008996 RID: 35222
		public List<Vector2i> TraderForestCenterPositions = new List<Vector2i>();

		// Token: 0x04008997 RID: 35223
		[PublicizedFrom(EAccessModifier.Private)]
		public WaitForEndOfFrame endOfFrameHandle = new WaitForEndOfFrame();

		// Token: 0x04008998 RID: 35224
		public XUiC_WorldGenerationPreview PreviewWindow;

		// Token: 0x04008999 RID: 35225
		public WorldBuilder.Data data;

		// Token: 0x0400899A RID: 35226
		[TupleElementNames(new string[] { "langKey", "fileName", "serializer" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ValueTuple<string, string, Action<Stream>>[] threadedSerializers;

		// Token: 0x0400899B RID: 35227
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryStream[] threadedSerializerBuffers;

		// Token: 0x0400899C RID: 35228
		[TupleElementNames(new string[] { "langKey", "fileName", "serializer" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ValueTuple<string, string, Func<Stream, IEnumerator>>[] mainThreadSerializers;

		// Token: 0x0400899D RID: 35229
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryStream[] mainThreadSerializerBuffers;

		// Token: 0x0400899E RID: 35230
		[PublicizedFrom(EAccessModifier.Private)]
		public long serializedTotalSize;

		// Token: 0x0400899F RID: 35231
		public readonly int[] biomeTagBits = new int[]
		{
			FastTags<TagGroup.Poi>.GetBit("forest"),
			FastTags<TagGroup.Poi>.GetBit("burntforest"),
			FastTags<TagGroup.Poi>.GetBit("desert"),
			FastTags<TagGroup.Poi>.GetBit("snow"),
			FastTags<TagGroup.Poi>.GetBit("wasteland"),
			FastTags<TagGroup.Poi>.GetBit("water")
		};

		// Token: 0x040089A0 RID: 35232
		[PublicizedFrom(EAccessModifier.Private)]
		public string creatingTerrainAndBiomeStamps;

		// Token: 0x040089A1 RID: 35233
		[PublicizedFrom(EAccessModifier.Private)]
		public string creatingTerrainStamps;

		// Token: 0x040089A2 RID: 35234
		[PublicizedFrom(EAccessModifier.Private)]
		public string creatingBiomeStamps;

		// Token: 0x040089A3 RID: 35235
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageGeneratingTerrain;

		// Token: 0x040089A4 RID: 35236
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingStampsToMap;

		// Token: 0x040089A5 RID: 35237
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageTerrainGenerationFinished;

		// Token: 0x040089A6 RID: 35238
		public string messageTownPlanning;

		// Token: 0x040089A7 RID: 35239
		public string messageTownPlanningFinished;

		// Token: 0x040089A8 RID: 35240
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageCleaningUpWaterMapData;

		// Token: 0x040089A9 RID: 35241
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingTerrainStampsToMap;

		// Token: 0x040089AA RID: 35242
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingWaterStampsToMap;

		// Token: 0x040089AB RID: 35243
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingTerrainAndWaterStampsToMap;

		// Token: 0x040089AC RID: 35244
		public string messageSmoothingStreetTiles;

		// Token: 0x040089AD RID: 35245
		public string messageWildernessPOIs;

		// Token: 0x040089AE RID: 35246
		public string messageHighways;

		// Token: 0x040089AF RID: 35247
		public string messageHighwaysConnections;

		// Token: 0x040089B0 RID: 35248
		public string messageHighwaysTownExitsSelf;

		// Token: 0x040089B1 RID: 35249
		public string messageHighwaysTownExitsOther;

		// Token: 0x040089B2 RID: 35250
		public string messageHighwaysTownship;

		// Token: 0x040089B3 RID: 35251
		public string messageHighwaysTownExits;

		// Token: 0x040089B4 RID: 35252
		public string messageWildernessPaths;

		// Token: 0x040089B5 RID: 35253
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageDrawRoads;

		// Token: 0x040089B6 RID: 35254
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageDrawRoadsWilderness;

		// Token: 0x040089B7 RID: 35255
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageDrawRoadsProgress;

		// Token: 0x040089B8 RID: 35256
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageSmoothRoadTerrainCount;

		// Token: 0x040089B9 RID: 35257
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageSmoothRoadTerrain;

		// Token: 0x040089BA RID: 35258
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldGenerationPreview.PreviewStep previewStep;

		// Token: 0x040089BB RID: 35259
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldGenerationPreview.PreviewStep previewStepOfTask;

		// Token: 0x040089BC RID: 35260
		[PublicizedFrom(EAccessModifier.Private)]
		public Color32[] biomeDest;

		// Token: 0x040089BD RID: 35261
		[PublicizedFrom(EAccessModifier.Private)]
		public Color32[] radDest;

		// Token: 0x040089BE RID: 35262
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup lowerLayer = new StampGroup("Lower Layer");

		// Token: 0x040089BF RID: 35263
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup terrainLayer = new StampGroup("Top Layer");

		// Token: 0x040089C0 RID: 35264
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup radiationLayer = new StampGroup("Radiation Layer");

		// Token: 0x040089C1 RID: 35265
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup biomeLayer = new StampGroup("Biome Layer");

		// Token: 0x040089C2 RID: 35266
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup waterLayer = new StampGroup("Water Layer");

		// Token: 0x040089C3 RID: 35267
		[PublicizedFrom(EAccessModifier.Private)]
		public int BorderWaterMask;

		// Token: 0x040089C4 RID: 35268
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<WorldBuilder.PreviewPoint> previewPoints = new List<WorldBuilder.PreviewPoint>();

		// Token: 0x040089C5 RID: 35269
		[PublicizedFrom(EAccessModifier.Private)]
		public string taskMessage = string.Empty;

		// Token: 0x040089C6 RID: 35270
		[PublicizedFrom(EAccessModifier.Private)]
		public string setMessageLast = string.Empty;

		// Token: 0x040089C7 RID: 35271
		[PublicizedFrom(EAccessModifier.Private)]
		public MicroStopwatch messageMS = new MicroStopwatch(true);

		// Token: 0x040089C8 RID: 35272
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Vector2i[] directions8way = new Vector2i[]
		{
			Vector2i.up,
			Vector2i.up + Vector2i.right,
			Vector2i.right,
			Vector2i.right + Vector2i.down,
			Vector2i.down,
			Vector2i.down + Vector2i.left,
			Vector2i.left,
			Vector2i.left + Vector2i.up
		};

		// Token: 0x040089C9 RID: 35273
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Vector2i[] directions4way = new Vector2i[]
		{
			Vector2i.up,
			Vector2i.right,
			Vector2i.down,
			Vector2i.left
		};

		// Token: 0x0200171E RID: 5918
		public enum BiomeLayout
		{
			// Token: 0x040089CB RID: 35275
			CenterForest,
			// Token: 0x040089CC RID: 35276
			CenterWasteland,
			// Token: 0x040089CD RID: 35277
			Circle,
			// Token: 0x040089CE RID: 35278
			Circle2,
			// Token: 0x040089CF RID: 35279
			Line
		}

		// Token: 0x0200171F RID: 5919
		public struct Data
		{
			// Token: 0x0600B801 RID: 47105 RVA: 0x0044C628 File Offset: 0x0044A828
			public void Init(int _worldSize)
			{
				this.WorldSize = _worldSize;
				int num = this.WorldSize * this.WorldSize;
				this.HeightMap = new NativeArray<float>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.waterDest = new NativeArray<float>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.PathTileGridWidth = this.WorldSize / 10;
				this.PathTileGrid = new NativeArray<PathTile>(this.PathTileGridWidth * this.PathTileGridWidth, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.StreetTileDataGridWidth = this.WorldSize / 150;
			}

			// Token: 0x0600B802 RID: 47106 RVA: 0x0044C6A1 File Offset: 0x0044A8A1
			public void Cleanup()
			{
				this.HeightMap.Dispose();
				this.PathTileGrid.Dispose();
				this.StreetTileDataGrid.Dispose();
				this.poiHeightMask.Dispose();
				this.waterDest.Dispose();
			}

			// Token: 0x0600B803 RID: 47107 RVA: 0x0044C6DA File Offset: 0x0044A8DA
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public float GetHeight(Vector2i pos)
			{
				return this.GetHeight(pos.x, pos.y);
			}

			// Token: 0x0600B804 RID: 47108 RVA: 0x0044C6EE File Offset: 0x0044A8EE
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public float GetHeight(int x, int y)
			{
				if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
				{
					return 0f;
				}
				return this.HeightMap[x + y * this.WorldSize];
			}

			// Token: 0x0600B805 RID: 47109 RVA: 0x0044C721 File Offset: 0x0044A921
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool InWorldBounds(int x, int y)
			{
				return (ulong)x < (ulong)((long)this.WorldSize) && (ulong)y < (ulong)((long)this.WorldSize);
			}

			// Token: 0x0600B806 RID: 47110 RVA: 0x0044C73C File Offset: 0x0044A93C
			public ref StreetTileData GetStreetTileDataWorld(int x, int y)
			{
				x /= 150;
				y /= 150;
				if ((ulong)x >= (ulong)((long)this.StreetTileDataGridWidth) || (ulong)y >= (ulong)((long)this.StreetTileDataGridWidth))
				{
					x = 0;
					y = 0;
				}
				return UnsafeUtility.ArrayElementAsRef<StreetTileData>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<StreetTileData>(this.StreetTileDataGrid), x + y * this.StreetTileDataGridWidth);
			}

			// Token: 0x0600B807 RID: 47111 RVA: 0x0044C791 File Offset: 0x0044A991
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public byte GetWater(int x, int y)
			{
				if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
				{
					return 0;
				}
				return (byte)this.waterDest[x + y * this.WorldSize];
			}

			// Token: 0x0600B808 RID: 47112 RVA: 0x0044C7C1 File Offset: 0x0044A9C1
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public byte GetWater(int _index)
			{
				if ((ulong)_index >= (ulong)((long)(this.WorldSize * this.WorldSize)))
				{
					return 0;
				}
				return (byte)this.waterDest[_index];
			}

			// Token: 0x040089D0 RID: 35280
			public int WorldSize;

			// Token: 0x040089D1 RID: 35281
			public NativeArray<float> HeightMap;

			// Token: 0x040089D2 RID: 35282
			public NativeArray<float> waterDest;

			// Token: 0x040089D3 RID: 35283
			public int PathTileGridWidth;

			// Token: 0x040089D4 RID: 35284
			public NativeArray<PathTile> PathTileGrid;

			// Token: 0x040089D5 RID: 35285
			public int StreetTileDataGridWidth;

			// Token: 0x040089D6 RID: 35286
			public NativeArray<StreetTileData> StreetTileDataGrid;

			// Token: 0x040089D7 RID: 35287
			public NativeArray<byte> poiHeightMask;

			// Token: 0x040089D8 RID: 35288
			public volatile int messageCnt;
		}

		// Token: 0x02001720 RID: 5920
		public enum SaveDataPromptMode
		{
			// Token: 0x040089DA RID: 35290
			On,
			// Token: 0x040089DB RID: 35291
			OnInsufficientSpace,
			// Token: 0x040089DC RID: 35292
			Off
		}

		// Token: 0x02001721 RID: 5921
		[PublicizedFrom(EAccessModifier.Private)]
		public class BiomeTypeData
		{
			// Token: 0x0600B809 RID: 47113 RVA: 0x0044C7E4 File Offset: 0x0044A9E4
			public BiomeTypeData(BiomeType _type, float _percent, int _totalTiles)
			{
				this.Type = _type;
				this.Percent = _percent;
				this.TileCount = Mathf.FloorToInt(_percent * (float)_totalTiles);
				if (this.Percent > 0f && this.TileCount == 0)
				{
					this.TileCount = 1;
				}
			}

			// Token: 0x040089DD RID: 35293
			public BiomeType Type;

			// Token: 0x040089DE RID: 35294
			public float Percent;

			// Token: 0x040089DF RID: 35295
			public int TileCount;

			// Token: 0x040089E0 RID: 35296
			public Vector2i Center;
		}

		// Token: 0x02001722 RID: 5922
		[PublicizedFrom(EAccessModifier.Private)]
		public struct PreviewPoint
		{
			// Token: 0x040089E1 RID: 35297
			public Vector2i pos;

			// Token: 0x040089E2 RID: 35298
			public Color32 color;

			// Token: 0x040089E3 RID: 35299
			public int size;
		}

		// Token: 0x02001723 RID: 5923
		public enum GenerationSelections
		{
			// Token: 0x040089E5 RID: 35301
			None,
			// Token: 0x040089E6 RID: 35302
			Few,
			// Token: 0x040089E7 RID: 35303
			Default,
			// Token: 0x040089E8 RID: 35304
			Many
		}

		// Token: 0x02001724 RID: 5924
		public struct PlayerSpawn
		{
			// Token: 0x0600B80A RID: 47114 RVA: 0x0044C830 File Offset: 0x0044AA30
			public PlayerSpawn(Vector3 _position, float _yRotation)
			{
				this.Position = _position;
				this.Rotation = _yRotation;
			}

			// Token: 0x0600B80B RID: 47115 RVA: 0x0044C840 File Offset: 0x0044AA40
			public bool IsTooClose(Vector3 _position)
			{
				float num = _position.x - this.Position.x;
				float num2 = _position.z - this.Position.z;
				return num * num + num2 * num2 < 3600f;
			}

			// Token: 0x040089E9 RID: 35305
			[PublicizedFrom(EAccessModifier.Private)]
			public const int cSafeDist = 60;

			// Token: 0x040089EA RID: 35306
			public Vector3 Position;

			// Token: 0x040089EB RID: 35307
			public float Rotation;
		}

		// Token: 0x0200173E RID: 5950
		// (Invoke) Token: 0x0600B887 RID: 47239
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void ClearWaterUnderTerrain_0000B750$PostfixBurstDelegate(ref WorldBuilder.Data _data, ref NativeArray<float> _terrain, int startX, int endX, int startY, int endY);

		// Token: 0x0200173F RID: 5951
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class ClearWaterUnderTerrain_0000B750$BurstDirectCall
		{
			// Token: 0x0600B88A RID: 47242 RVA: 0x0044EE90 File Offset: 0x0044D090
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.ClearWaterUnderTerrain$BurstManaged(ref WorldBuilder.Data, ref NativeArray<float>, int, int, int, int)).MethodHandle, typeof(WorldBuilder.ClearWaterUnderTerrain_0000B750$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B88B RID: 47243 RVA: 0x0044EEBC File Offset: 0x0044D0BC
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B88C RID: 47244 RVA: 0x0044EED4 File Offset: 0x0044D0D4
			public static void Constructor()
			{
				WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.ClearWaterUnderTerrain(ref WorldBuilder.Data, ref NativeArray<float>, int, int, int, int)).MethodHandle);
			}

			// Token: 0x0600B88D RID: 47245 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B88E RID: 47246 RVA: 0x0044EEE5 File Offset: 0x0044D0E5
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static ClearWaterUnderTerrain_0000B750$BurstDirectCall()
			{
				WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B88F RID: 47247 RVA: 0x0044EEEC File Offset: 0x0044D0EC
			public static void Invoke(ref WorldBuilder.Data _data, ref NativeArray<float> _terrain, int startX, int endX, int startY, int endY)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.WorldBuilder/Data&,Unity.Collections.NativeArray`1<System.Single>&,System.Int32,System.Int32,System.Int32,System.Int32), ref _data, ref _terrain, startX, endX, startY, endY, functionPointer);
						return;
					}
				}
				WorldBuilder.ClearWaterUnderTerrain$BurstManaged(ref _data, ref _terrain, startX, endX, startY, endY);
			}

			// Token: 0x04008A5A RID: 35418
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A5B RID: 35419
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001740 RID: 5952
		// (Invoke) Token: 0x0600B891 RID: 47249
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void FinalizeWater_0000B753$PostfixBurstDelegate(ref WorldBuilder.Data _data, float _WaterHeight);

		// Token: 0x02001741 RID: 5953
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FinalizeWater_0000B753$BurstDirectCall
		{
			// Token: 0x0600B894 RID: 47252 RVA: 0x0044EF2B File Offset: 0x0044D12B
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.FinalizeWater$BurstManaged(ref WorldBuilder.Data, float)).MethodHandle, typeof(WorldBuilder.FinalizeWater_0000B753$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B895 RID: 47253 RVA: 0x0044EF58 File Offset: 0x0044D158
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B896 RID: 47254 RVA: 0x0044EF70 File Offset: 0x0044D170
			public static void Constructor()
			{
				WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.FinalizeWater(ref WorldBuilder.Data, float)).MethodHandle);
			}

			// Token: 0x0600B897 RID: 47255 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B898 RID: 47256 RVA: 0x0044EF81 File Offset: 0x0044D181
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FinalizeWater_0000B753$BurstDirectCall()
			{
				WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B899 RID: 47257 RVA: 0x0044EF88 File Offset: 0x0044D188
			public static void Invoke(ref WorldBuilder.Data _data, float _WaterHeight)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.WorldBuilder/Data&,System.Single), ref _data, _WaterHeight, functionPointer);
						return;
					}
				}
				WorldBuilder.FinalizeWater$BurstManaged(ref _data, _WaterHeight);
			}

			// Token: 0x04008A5C RID: 35420
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A5D RID: 35421
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001742 RID: 5954
		// (Invoke) Token: 0x0600B89B RID: 47259
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void SmoothRoadTerrainTask_0000B75A$PostfixBurstDelegate(ref WorldBuilder.Data _data, ref NativeArray<Color32> roadMask, ref NativeArray<float> _heightMap, int WorldSize);

		// Token: 0x02001743 RID: 5955
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class SmoothRoadTerrainTask_0000B75A$BurstDirectCall
		{
			// Token: 0x0600B89E RID: 47262 RVA: 0x0044EFBB File Offset: 0x0044D1BB
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.SmoothRoadTerrainTask$BurstManaged(ref WorldBuilder.Data, ref NativeArray<Color32>, ref NativeArray<float>, int)).MethodHandle, typeof(WorldBuilder.SmoothRoadTerrainTask_0000B75A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B89F RID: 47263 RVA: 0x0044EFE8 File Offset: 0x0044D1E8
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B8A0 RID: 47264 RVA: 0x0044F000 File Offset: 0x0044D200
			public static void Constructor()
			{
				WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.SmoothRoadTerrainTask(ref WorldBuilder.Data, ref NativeArray<Color32>, ref NativeArray<float>, int)).MethodHandle);
			}

			// Token: 0x0600B8A1 RID: 47265 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B8A2 RID: 47266 RVA: 0x0044F011 File Offset: 0x0044D211
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static SmoothRoadTerrainTask_0000B75A$BurstDirectCall()
			{
				WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B8A3 RID: 47267 RVA: 0x0044F018 File Offset: 0x0044D218
			public static void Invoke(ref WorldBuilder.Data _data, ref NativeArray<Color32> roadMask, ref NativeArray<float> _heightMap, int WorldSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.WorldBuilder/Data&,Unity.Collections.NativeArray`1<UnityEngine.Color32>&,Unity.Collections.NativeArray`1<System.Single>&,System.Int32), ref _data, ref roadMask, ref _heightMap, WorldSize, functionPointer);
						return;
					}
				}
				WorldBuilder.SmoothRoadTerrainTask$BurstManaged(ref _data, ref roadMask, ref _heightMap, WorldSize);
			}

			// Token: 0x04008A5E RID: 35422
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A5F RID: 35423
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001744 RID: 5956
		// (Invoke) Token: 0x0600B8A5 RID: 47269
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void AdjustHeights_0000B76D$PostfixBurstDelegate(ref NativeArray<float> _src, float _min);

		// Token: 0x02001745 RID: 5957
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class AdjustHeights_0000B76D$BurstDirectCall
		{
			// Token: 0x0600B8A8 RID: 47272 RVA: 0x0044F04F File Offset: 0x0044D24F
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.AdjustHeights$BurstManaged(ref NativeArray<float>, float)).MethodHandle, typeof(WorldBuilder.AdjustHeights_0000B76D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B8A9 RID: 47273 RVA: 0x0044F07C File Offset: 0x0044D27C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B8AA RID: 47274 RVA: 0x0044F094 File Offset: 0x0044D294
			public static void Constructor()
			{
				WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.AdjustHeights(ref NativeArray<float>, float)).MethodHandle);
			}

			// Token: 0x0600B8AB RID: 47275 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B8AC RID: 47276 RVA: 0x0044F0A5 File Offset: 0x0044D2A5
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static AdjustHeights_0000B76D$BurstDirectCall()
			{
				WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B8AD RID: 47277 RVA: 0x0044F0AC File Offset: 0x0044D2AC
			public static void Invoke(ref NativeArray<float> _src, float _min)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeArray`1<System.Single>&,System.Single), ref _src, _min, functionPointer);
						return;
					}
				}
				WorldBuilder.AdjustHeights$BurstManaged(ref _src, _min);
			}

			// Token: 0x04008A60 RID: 35424
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A61 RID: 35425
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
