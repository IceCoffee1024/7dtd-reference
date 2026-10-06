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
	// Token: 0x0200176F RID: 5999
	[BurstCompile(CompileSynchronously = true)]
	public class WorldBuilder
	{
		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x0600BA7A RID: 47738 RVA: 0x00451DB3 File Offset: 0x0044FFB3
		public int HalfWorldSize
		{
			get
			{
				return this.WorldSize / 2;
			}
		}

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x0600BA7B RID: 47739 RVA: 0x00451DBD File Offset: 0x0044FFBD
		public Vector3i PrefabWorldOffset
		{
			get
			{
				return new Vector3i(-this.HalfWorldSize, 0, -this.HalfWorldSize);
			}
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x0600BA7C RID: 47740 RVA: 0x00451DD3 File Offset: 0x0044FFD3
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

		// Token: 0x0600BA7D RID: 47741 RVA: 0x00451DF4 File Offset: 0x0044FFF4
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

		// Token: 0x0600BA7E RID: 47742 RVA: 0x00452234 File Offset: 0x00450434
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

		// Token: 0x0600BA7F RID: 47743 RVA: 0x00452453 File Offset: 0x00450653
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

		// Token: 0x0600BA80 RID: 47744 RVA: 0x00452464 File Offset: 0x00450664
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

		// Token: 0x0600BA81 RID: 47745 RVA: 0x00452648 File Offset: 0x00450848
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

		// Token: 0x0600BA82 RID: 47746 RVA: 0x00452697 File Offset: 0x00450897
		public static string GetWorldPath(UserDataStorageType _storageType, string _worldName)
		{
			return GameIO.GetUserGameDataDir(_storageType) + "/GeneratedWorlds/" + _worldName + "/";
		}

		// Token: 0x0600BA83 RID: 47747 RVA: 0x004526AF File Offset: 0x004508AF
		public void SetStorageType(UserDataStorageType _storageType)
		{
			this.StorageLocation = _storageType;
			this.WorldPath = WorldBuilder.GetWorldPath(_storageType, this.WorldName);
		}

		// Token: 0x0600BA84 RID: 47748 RVA: 0x004526CA File Offset: 0x004508CA
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

		// Token: 0x0600BA85 RID: 47749 RVA: 0x004526D9 File Offset: 0x004508D9
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

		// Token: 0x0600BA86 RID: 47750 RVA: 0x004526E8 File Offset: 0x004508E8
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

		// Token: 0x0600BA87 RID: 47751 RVA: 0x004526F7 File Offset: 0x004508F7
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

		// Token: 0x0600BA88 RID: 47752 RVA: 0x00452708 File Offset: 0x00450908
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

		// Token: 0x0600BA89 RID: 47753 RVA: 0x00452980 File Offset: 0x00450B80
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

		// Token: 0x0600BA8A RID: 47754 RVA: 0x004529D7 File Offset: 0x00450BD7
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

		// Token: 0x0600BA8B RID: 47755 RVA: 0x004529E6 File Offset: 0x00450BE6
		public bool CanSaveData()
		{
			return !SdDirectory.Exists(this.WorldPath);
		}

		// Token: 0x0600BA8C RID: 47756 RVA: 0x004529F6 File Offset: 0x00450BF6
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

		// Token: 0x0600BA8D RID: 47757 RVA: 0x00452A23 File Offset: 0x00450C23
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> CalcPlayerSpawnTiles()
		{
			List<StreetTile> list = (from StreetTile st in this.StreetTileMap
				where !st.OverlapsRadiation && !st.AllIsWater && st.Township == null && (st.District == null || st.District.name == "wilderness") && (this.ForestBiomeWeight == 0 || st.BiomeType == BiomeType.forest) && !st.Used
				select st).ToList<StreetTile>();
			list.Sort((StreetTile _t1, StreetTile _t2) => this.CalcClosestTraderDistance(_t1).CompareTo(this.CalcClosestTraderDistance(_t2)));
			return list;
		}

		// Token: 0x0600BA8E RID: 47758 RVA: 0x00452A58 File Offset: 0x00450C58
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

		// Token: 0x0600BA8F RID: 47759 RVA: 0x00452AC0 File Offset: 0x00450CC0
		[PublicizedFrom(EAccessModifier.Private)]
		public List<StreetTile> getWildernessTilesToSmooth()
		{
			return (from StreetTile st in this.StreetTileMap
				where st.NeedsWildernessSmoothing
				select st).ToList<StreetTile>();
		}

		// Token: 0x0600BA90 RID: 47760 RVA: 0x00452AF8 File Offset: 0x00450CF8
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

		// Token: 0x0600BA91 RID: 47761 RVA: 0x00452B90 File Offset: 0x00450D90
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

		// Token: 0x0600BA92 RID: 47762 RVA: 0x00452BF0 File Offset: 0x00450DF0
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

		// Token: 0x0600BA93 RID: 47763 RVA: 0x00452CA4 File Offset: 0x00450EA4
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

		// Token: 0x0600BA94 RID: 47764 RVA: 0x00452E2C File Offset: 0x0045102C
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

		// Token: 0x0600BA95 RID: 47765 RVA: 0x00452FD8 File Offset: 0x004511D8
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

		// Token: 0x0600BA96 RID: 47766 RVA: 0x00453290 File Offset: 0x00451490
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

		// Token: 0x0600BA97 RID: 47767 RVA: 0x00453344 File Offset: 0x00451544
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

		// Token: 0x0600BA98 RID: 47768 RVA: 0x0045341C File Offset: 0x0045161C
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void ClearWaterUnderTerrain(ref WorldBuilder.Data _data, ref NativeArray<float> _terrain, int startX, int endX, int startY, int endY)
		{
			WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.Invoke(ref _data, ref _terrain, startX, endX, startY, endY);
		}

		// Token: 0x0600BA99 RID: 47769 RVA: 0x0045342C File Offset: 0x0045162C
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

		// Token: 0x0600BA9A RID: 47770 RVA: 0x004535F4 File Offset: 0x004517F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void FinalizeWater()
		{
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			WorldBuilder.FinalizeWater(ref this.data, (float)this.WaterHeight);
			Log.Out("FinalizeWater in {0}", new object[] { (float)microStopwatch.ElapsedMilliseconds * 0.001f });
		}

		// Token: 0x0600BA9B RID: 47771 RVA: 0x0045363F File Offset: 0x0045183F
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void FinalizeWater(ref WorldBuilder.Data _data, float _WaterHeight)
		{
			WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.Invoke(ref _data, _WaterHeight);
		}

		// Token: 0x0600BA9C RID: 47772 RVA: 0x00453648 File Offset: 0x00451848
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

		// Token: 0x0600BA9D RID: 47773 RVA: 0x00453718 File Offset: 0x00451918
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

		// Token: 0x0600BA9E RID: 47774 RVA: 0x00453E5C File Offset: 0x0045205C
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

		// Token: 0x0600BA9F RID: 47775 RVA: 0x00453FB4 File Offset: 0x004521B4
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

		// Token: 0x0600BAA0 RID: 47776 RVA: 0x004540CC File Offset: 0x004522CC
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

		// Token: 0x0600BAA1 RID: 47777 RVA: 0x00454194 File Offset: 0x00452394
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

		// Token: 0x0600BAA2 RID: 47778 RVA: 0x00454266 File Offset: 0x00452466
		[BurstCompile(CompileSynchronously = true)]
		public static void SmoothRoadTerrainTask(ref WorldBuilder.Data _data, ref NativeArray<Color32> roadMask, ref NativeArray<float> _heightMap, int WorldSize)
		{
			WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.Invoke(ref _data, ref roadMask, ref _heightMap, WorldSize);
		}

		// Token: 0x0600BAA3 RID: 47779 RVA: 0x00454274 File Offset: 0x00452474
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

		// Token: 0x0600BAA4 RID: 47780 RVA: 0x004542FC File Offset: 0x004524FC
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

		// Token: 0x0600BAA5 RID: 47781 RVA: 0x004546E8 File Offset: 0x004528E8
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

		// Token: 0x0600BAA6 RID: 47782 RVA: 0x00454868 File Offset: 0x00452A68
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

		// Token: 0x0600BAA7 RID: 47783 RVA: 0x00454C90 File Offset: 0x00452E90
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

		// Token: 0x0600BAA8 RID: 47784 RVA: 0x0045501C File Offset: 0x0045321C
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

		// Token: 0x0600BAA9 RID: 47785 RVA: 0x004550A0 File Offset: 0x004532A0
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

		// Token: 0x0600BAAA RID: 47786 RVA: 0x0045515C File Offset: 0x0045335C
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializeRWGTTW(Stream stream)
		{
			World world = new World();
			WorldState worldState = new WorldState();
			worldState.SetFrom(world, EnumChunkProviderId.ChunkDataDriven);
			worldState.ResetDynamicData();
			worldState.Save(stream);
		}

		// Token: 0x0600BAAB RID: 47787 RVA: 0x0045518C File Offset: 0x0045338C
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

		// Token: 0x0600BAAC RID: 47788 RVA: 0x0045543C File Offset: 0x0045363C
		public void AddPreviewLinePlus(Vector2i pos, Color32 color, int size)
		{
			WorldBuilder.PreviewPoint previewPoint;
			previewPoint.pos = pos;
			previewPoint.color = color;
			previewPoint.size = size;
			this.previewPoints.Add(previewPoint);
		}

		// Token: 0x0600BAAD RID: 47789 RVA: 0x00455470 File Offset: 0x00453670
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

		// Token: 0x0600BAAE RID: 47790 RVA: 0x00455567 File Offset: 0x00453767
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

		// Token: 0x0600BAAF RID: 47791 RVA: 0x0045557D File Offset: 0x0045377D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public StreetTile GetStreetTileGrid(Vector2i pos)
		{
			return this.GetStreetTileGrid(pos.x, pos.y);
		}

		// Token: 0x0600BAB0 RID: 47792 RVA: 0x00455591 File Offset: 0x00453791
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

		// Token: 0x0600BAB1 RID: 47793 RVA: 0x004555BE File Offset: 0x004537BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public StreetTile GetStreetTileWorld(Vector2i pos)
		{
			return this.GetStreetTileWorld(pos.x, pos.y);
		}

		// Token: 0x0600BAB2 RID: 47794 RVA: 0x004555D2 File Offset: 0x004537D2
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

		// Token: 0x0600BAB3 RID: 47795 RVA: 0x00455611 File Offset: 0x00453811
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetHeight(Vector2i pos)
		{
			return this.GetHeight(pos.x, pos.y);
		}

		// Token: 0x0600BAB4 RID: 47796 RVA: 0x00455625 File Offset: 0x00453825
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetHeight(int x, int y)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return 0f;
			}
			return this.data.HeightMap[x + y * this.WorldSize];
		}

		// Token: 0x0600BAB5 RID: 47797 RVA: 0x0045565D File Offset: 0x0045385D
		[BurstCompile(CompileSynchronously = true)]
		public static void AdjustHeights(ref NativeArray<float> _src, float _min)
		{
			WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.Invoke(ref _src, _min);
		}

		// Token: 0x0600BAB6 RID: 47798 RVA: 0x00455666 File Offset: 0x00453866
		public void SetHeight(int x, int y, float height)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return;
			}
			this.data.HeightMap[x + y * this.WorldSize] = height;
		}

		// Token: 0x0600BAB7 RID: 47799 RVA: 0x0045569A File Offset: 0x0045389A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetHeightTrusted(int x, int y, float height)
		{
			this.data.HeightMap[x + y * this.WorldSize] = height;
		}

		// Token: 0x0600BAB8 RID: 47800 RVA: 0x004556B7 File Offset: 0x004538B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TerrainType GetTerrainType(Vector2i pos)
		{
			return this.GetTerrainType(pos.x, pos.y);
		}

		// Token: 0x0600BAB9 RID: 47801 RVA: 0x004556CC File Offset: 0x004538CC
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

		// Token: 0x0600BABA RID: 47802 RVA: 0x00455724 File Offset: 0x00453924
		public BiomeType GetBiome(Vector2i pos)
		{
			return this.GetBiome(pos.x, pos.y);
		}

		// Token: 0x0600BABB RID: 47803 RVA: 0x00455738 File Offset: 0x00453938
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

		// Token: 0x0600BABC RID: 47804 RVA: 0x004557CE File Offset: 0x004539CE
		public void SetWater(int x, int y, byte height)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return;
			}
			this.data.waterDest[x + y * this.WorldSize] = (float)height;
		}

		// Token: 0x0600BABD RID: 47805 RVA: 0x00455803 File Offset: 0x00453A03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte GetRad(int x, int y)
		{
			if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
			{
				return 0;
			}
			return this.radDest[x / 32 + y / 32 * this.RadSize].r;
		}

		// Token: 0x0600BABE RID: 47806 RVA: 0x0045583D File Offset: 0x00453A3D
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializePrefabs(Stream stream)
		{
			this.PrefabManager.SavePrefabData(stream);
			if (this.PreviewWindow == null)
			{
				this.PrefabManager.UsedPrefabsWorld.Clear();
			}
		}

		// Token: 0x0600BABF RID: 47807 RVA: 0x00455864 File Offset: 0x00453A64
		[PublicizedFrom(EAccessModifier.Private)]
		public void serializeRawHeightmap(Stream stream)
		{
			HeightMapUtils.SaveHeightMapRAW(stream, this.data.HeightMap.ToArray(), -1f);
		}

		// Token: 0x0600BAC0 RID: 47808 RVA: 0x00455884 File Offset: 0x00453A84
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

		// Token: 0x0600BAC1 RID: 47809 RVA: 0x00455984 File Offset: 0x00453B84
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

		// Token: 0x0600BAC2 RID: 47810 RVA: 0x00455A84 File Offset: 0x00453C84
		public void SetTaskMessage(string _message)
		{
			this.taskMessage = _message;
		}

		// Token: 0x0600BAC3 RID: 47811 RVA: 0x00455A8D File Offset: 0x00453C8D
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator TaskMessageUpdate()
		{
			yield return this.SetMessage(this.taskMessage, false, false);
			yield break;
		}

		// Token: 0x0600BAC4 RID: 47812 RVA: 0x00455A9C File Offset: 0x00453C9C
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

		// Token: 0x0600BAC5 RID: 47813 RVA: 0x00455AC0 File Offset: 0x00453CC0
		public bool IsMessageElapsed()
		{
			if (this.messageMS.ElapsedMilliseconds > 600L)
			{
				this.messageMS.ResetAndRestart();
				return true;
			}
			return false;
		}

		// Token: 0x0600BAC6 RID: 47814 RVA: 0x00455AE4 File Offset: 0x00453CE4
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i getRotatedPoint(int x, int y, int cx, int cy, int angle)
		{
			return new Vector2i(Mathf.RoundToInt((float)((double)(x - cx) * Math.Cos((double)angle) - (double)(y - cy) * Math.Sin((double)angle) + (double)cx)), Mathf.RoundToInt((float)((double)(x - cx) * Math.Sin((double)angle) + (double)(y - cy) * Math.Cos((double)angle) + (double)cy)));
		}

		// Token: 0x0600BAC7 RID: 47815 RVA: 0x00455B44 File Offset: 0x00453D44
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

		// Token: 0x0600BAC8 RID: 47816 RVA: 0x00455CDF File Offset: 0x00453EDF
		public static string GetGeneratedWorldName(string _worldSeedName, int _worldSize = 8192)
		{
			return RandomCountyNameGenerator.GetName(_worldSeedName.GetHashCode() + _worldSize);
		}

		// Token: 0x0600BAC9 RID: 47817 RVA: 0x00455CF0 File Offset: 0x00453EF0
		[PublicizedFrom(EAccessModifier.Private)]
		public static int distanceSqr(Vector2i pointA, Vector2i pointB)
		{
			Vector2i vector2i = pointA - pointB;
			return vector2i.x * vector2i.x + vector2i.y * vector2i.y;
		}

		// Token: 0x0600BACA RID: 47818 RVA: 0x00455D20 File Offset: 0x00453F20
		[PublicizedFrom(EAccessModifier.Private)]
		public static float distanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x0600BACB RID: 47819 RVA: 0x00455D50 File Offset: 0x00453F50
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

		// Token: 0x0600BACC RID: 47820 RVA: 0x00455DBC File Offset: 0x00453FBC
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

		// Token: 0x0600BAD7 RID: 47831 RVA: 0x004561C0 File Offset: 0x004543C0
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

		// Token: 0x0600BAD8 RID: 47832 RVA: 0x00456220 File Offset: 0x00454420
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

		// Token: 0x0600BAD9 RID: 47833 RVA: 0x00456284 File Offset: 0x00454484
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

		// Token: 0x0600BADA RID: 47834 RVA: 0x0045688C File Offset: 0x00454A8C
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

		// Token: 0x04008B37 RID: 35639
		public const int HeightMax = 255;

		// Token: 0x04008B38 RID: 35640
		[PublicizedFrom(EAccessModifier.Private)]
		public const int groundHeight = 35;

		// Token: 0x04008B39 RID: 35641
		public int WaterHeight = 30;

		// Token: 0x04008B3A RID: 35642
		public readonly DistrictPlanner DistrictPlanner;

		// Token: 0x04008B3B RID: 35643
		public readonly HighwayPlanner HighwayPlanner;

		// Token: 0x04008B3C RID: 35644
		public readonly PathingUtils PathingUtils;

		// Token: 0x04008B3D RID: 35645
		public readonly PathShared PathShared;

		// Token: 0x04008B3E RID: 35646
		public readonly POISmoother POISmoother;

		// Token: 0x04008B3F RID: 35647
		public readonly PrefabManager PrefabManager;

		// Token: 0x04008B40 RID: 35648
		public readonly StampManager StampManager;

		// Token: 0x04008B41 RID: 35649
		public readonly StreetTileShared StreetTileShared;

		// Token: 0x04008B42 RID: 35650
		public readonly TownPlanner TownPlanner;

		// Token: 0x04008B43 RID: 35651
		public readonly TownshipShared TownshipShared;

		// Token: 0x04008B44 RID: 35652
		public readonly WildernessPathPlanner WildernessPathPlanner;

		// Token: 0x04008B45 RID: 35653
		public readonly WildernessPlanner WildernessPlanner;

		// Token: 0x04008B46 RID: 35654
		public string WorldName;

		// Token: 0x04008B47 RID: 35655
		public string WorldSeedName;

		// Token: 0x04008B48 RID: 35656
		public string WorldPath;

		// Token: 0x04008B49 RID: 35657
		[PublicizedFrom(EAccessModifier.Private)]
		public UserDataStorageType StorageLocation;

		// Token: 0x04008B4A RID: 35658
		[PublicizedFrom(EAccessModifier.Private)]
		public MicroStopwatch totalMS;

		// Token: 0x04008B4B RID: 35659
		public bool IsCanceled;

		// Token: 0x04008B4C RID: 35660
		public bool IsFinished;

		// Token: 0x04008B4D RID: 35661
		[PublicizedFrom(EAccessModifier.Private)]
		public Color32[] roadDest;

		// Token: 0x04008B4E RID: 35662
		public int WorldSize = 8192;

		// Token: 0x04008B4F RID: 35663
		public int WorldSizeDistDiv;

		// Token: 0x04008B50 RID: 35664
		public const int BiomeSizeDiv = 8;

		// Token: 0x04008B51 RID: 35665
		public int BiomeSize;

		// Token: 0x04008B52 RID: 35666
		public int RadSize;

		// Token: 0x04008B53 RID: 35667
		public int Seed = 12345;

		// Token: 0x04008B54 RID: 35668
		public int Plains = 4;

		// Token: 0x04008B55 RID: 35669
		public int Hills = 4;

		// Token: 0x04008B56 RID: 35670
		public int Mountains = 2;

		// Token: 0x04008B57 RID: 35671
		public WorldBuilder.GenerationSelections Canyons = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008B58 RID: 35672
		public WorldBuilder.GenerationSelections Craters = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008B59 RID: 35673
		public WorldBuilder.GenerationSelections Lakes = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008B5A RID: 35674
		public WorldBuilder.GenerationSelections Rivers = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008B5B RID: 35675
		public WorldBuilder.GenerationSelections Towns = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008B5C RID: 35676
		public WorldBuilder.GenerationSelections Wilderness = WorldBuilder.GenerationSelections.Default;

		// Token: 0x04008B5D RID: 35677
		public StreetTile[] StreetTileMap;

		// Token: 0x04008B5E RID: 35678
		public int StreetTileMapWidth;

		// Token: 0x04008B5F RID: 35679
		[PublicizedFrom(EAccessModifier.Private)]
		public DataMap<BiomeType> biomeMap;

		// Token: 0x04008B60 RID: 35680
		[PublicizedFrom(EAccessModifier.Private)]
		public DataMap<TerrainType> terrainTypeMap;

		// Token: 0x04008B61 RID: 35681
		public List<Rect> waterRects = new List<Rect>();

		// Token: 0x04008B62 RID: 35682
		public List<Township> Townships = new List<Township>();

		// Token: 0x04008B63 RID: 35683
		public int WildernessPrefabCount;

		// Token: 0x04008B64 RID: 35684
		[PublicizedFrom(EAccessModifier.Private)]
		public string worldSizeName;

		// Token: 0x04008B65 RID: 35685
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicProperties thisWorldProperties;

		// Token: 0x04008B66 RID: 35686
		[PublicizedFrom(EAccessModifier.Private)]
		public const int WorldTileSize = 1024;

		// Token: 0x04008B67 RID: 35687
		[PublicizedFrom(EAccessModifier.Private)]
		public const int BiomeTileSize = 256;

		// Token: 0x04008B68 RID: 35688
		[PublicizedFrom(EAccessModifier.Private)]
		public const int TerrainTileSize = 256;

		// Token: 0x04008B69 RID: 35689
		[PublicizedFrom(EAccessModifier.Private)]
		public const int terrainToBiomeTileScale = 1;

		// Token: 0x04008B6A RID: 35690
		[PublicizedFrom(EAccessModifier.Private)]
		public const int RadTileSize = 32;

		// Token: 0x04008B6B RID: 35691
		public WorldBuilder.BiomeLayout biomeLayout;

		// Token: 0x04008B6C RID: 35692
		public int ForestBiomeWeight = 13;

		// Token: 0x04008B6D RID: 35693
		[PublicizedFrom(EAccessModifier.Private)]
		public int BurntForestBiomeWeight = 18;

		// Token: 0x04008B6E RID: 35694
		[PublicizedFrom(EAccessModifier.Private)]
		public int DesertBiomeWeight = 22;

		// Token: 0x04008B6F RID: 35695
		[PublicizedFrom(EAccessModifier.Private)]
		public int SnowBiomeWeight = 23;

		// Token: 0x04008B70 RID: 35696
		[PublicizedFrom(EAccessModifier.Private)]
		public int WastelandBiomeWeight = 24;

		// Token: 0x04008B71 RID: 35697
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<BiomeType, Color32> biomeColors = new Dictionary<BiomeType, Color32>();

		// Token: 0x04008B72 RID: 35698
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cPlayerSpawnsNeeded = 12;

		// Token: 0x04008B73 RID: 35699
		[PublicizedFrom(EAccessModifier.Private)]
		public List<WorldBuilder.PlayerSpawn> playerSpawns;

		// Token: 0x04008B74 RID: 35700
		public List<Path> highwayPaths = new List<Path>();

		// Token: 0x04008B75 RID: 35701
		public List<Path> wildernessPaths = new List<Path>();

		// Token: 0x04008B76 RID: 35702
		public List<Vector2i> TraderCenterPositions = new List<Vector2i>();

		// Token: 0x04008B77 RID: 35703
		public List<Vector2i> TraderForestCenterPositions = new List<Vector2i>();

		// Token: 0x04008B78 RID: 35704
		[PublicizedFrom(EAccessModifier.Private)]
		public WaitForEndOfFrame endOfFrameHandle = new WaitForEndOfFrame();

		// Token: 0x04008B79 RID: 35705
		public XUiC_WorldGenerationPreview PreviewWindow;

		// Token: 0x04008B7A RID: 35706
		public WorldBuilder.Data data;

		// Token: 0x04008B7B RID: 35707
		[TupleElementNames(new string[] { "langKey", "fileName", "serializer" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ValueTuple<string, string, Action<Stream>>[] threadedSerializers;

		// Token: 0x04008B7C RID: 35708
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryStream[] threadedSerializerBuffers;

		// Token: 0x04008B7D RID: 35709
		[TupleElementNames(new string[] { "langKey", "fileName", "serializer" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ValueTuple<string, string, Func<Stream, IEnumerator>>[] mainThreadSerializers;

		// Token: 0x04008B7E RID: 35710
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MemoryStream[] mainThreadSerializerBuffers;

		// Token: 0x04008B7F RID: 35711
		[PublicizedFrom(EAccessModifier.Private)]
		public long serializedTotalSize;

		// Token: 0x04008B80 RID: 35712
		public readonly int[] biomeTagBits = new int[]
		{
			FastTags<TagGroup.Poi>.GetBit("forest"),
			FastTags<TagGroup.Poi>.GetBit("burntforest"),
			FastTags<TagGroup.Poi>.GetBit("desert"),
			FastTags<TagGroup.Poi>.GetBit("snow"),
			FastTags<TagGroup.Poi>.GetBit("wasteland"),
			FastTags<TagGroup.Poi>.GetBit("water")
		};

		// Token: 0x04008B81 RID: 35713
		[PublicizedFrom(EAccessModifier.Private)]
		public string creatingTerrainAndBiomeStamps;

		// Token: 0x04008B82 RID: 35714
		[PublicizedFrom(EAccessModifier.Private)]
		public string creatingTerrainStamps;

		// Token: 0x04008B83 RID: 35715
		[PublicizedFrom(EAccessModifier.Private)]
		public string creatingBiomeStamps;

		// Token: 0x04008B84 RID: 35716
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageGeneratingTerrain;

		// Token: 0x04008B85 RID: 35717
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingStampsToMap;

		// Token: 0x04008B86 RID: 35718
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageTerrainGenerationFinished;

		// Token: 0x04008B87 RID: 35719
		public string messageTownPlanning;

		// Token: 0x04008B88 RID: 35720
		public string messageTownPlanningFinished;

		// Token: 0x04008B89 RID: 35721
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageCleaningUpWaterMapData;

		// Token: 0x04008B8A RID: 35722
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingTerrainStampsToMap;

		// Token: 0x04008B8B RID: 35723
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingWaterStampsToMap;

		// Token: 0x04008B8C RID: 35724
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageWritingTerrainAndWaterStampsToMap;

		// Token: 0x04008B8D RID: 35725
		public string messageSmoothingStreetTiles;

		// Token: 0x04008B8E RID: 35726
		public string messageWildernessPOIs;

		// Token: 0x04008B8F RID: 35727
		public string messageHighways;

		// Token: 0x04008B90 RID: 35728
		public string messageHighwaysConnections;

		// Token: 0x04008B91 RID: 35729
		public string messageHighwaysTownExitsSelf;

		// Token: 0x04008B92 RID: 35730
		public string messageHighwaysTownExitsOther;

		// Token: 0x04008B93 RID: 35731
		public string messageHighwaysTownship;

		// Token: 0x04008B94 RID: 35732
		public string messageHighwaysTownExits;

		// Token: 0x04008B95 RID: 35733
		public string messageWildernessPaths;

		// Token: 0x04008B96 RID: 35734
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageDrawRoads;

		// Token: 0x04008B97 RID: 35735
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageDrawRoadsWilderness;

		// Token: 0x04008B98 RID: 35736
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageDrawRoadsProgress;

		// Token: 0x04008B99 RID: 35737
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageSmoothRoadTerrainCount;

		// Token: 0x04008B9A RID: 35738
		[PublicizedFrom(EAccessModifier.Private)]
		public string messageSmoothRoadTerrain;

		// Token: 0x04008B9B RID: 35739
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldGenerationPreview.PreviewStep previewStep;

		// Token: 0x04008B9C RID: 35740
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_WorldGenerationPreview.PreviewStep previewStepOfTask;

		// Token: 0x04008B9D RID: 35741
		[PublicizedFrom(EAccessModifier.Private)]
		public Color32[] biomeDest;

		// Token: 0x04008B9E RID: 35742
		[PublicizedFrom(EAccessModifier.Private)]
		public Color32[] radDest;

		// Token: 0x04008B9F RID: 35743
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup lowerLayer = new StampGroup("Lower Layer");

		// Token: 0x04008BA0 RID: 35744
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup terrainLayer = new StampGroup("Top Layer");

		// Token: 0x04008BA1 RID: 35745
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup radiationLayer = new StampGroup("Radiation Layer");

		// Token: 0x04008BA2 RID: 35746
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup biomeLayer = new StampGroup("Biome Layer");

		// Token: 0x04008BA3 RID: 35747
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StampGroup waterLayer = new StampGroup("Water Layer");

		// Token: 0x04008BA4 RID: 35748
		[PublicizedFrom(EAccessModifier.Private)]
		public int BorderWaterMask;

		// Token: 0x04008BA5 RID: 35749
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<WorldBuilder.PreviewPoint> previewPoints = new List<WorldBuilder.PreviewPoint>();

		// Token: 0x04008BA6 RID: 35750
		[PublicizedFrom(EAccessModifier.Private)]
		public string taskMessage = string.Empty;

		// Token: 0x04008BA7 RID: 35751
		[PublicizedFrom(EAccessModifier.Private)]
		public string setMessageLast = string.Empty;

		// Token: 0x04008BA8 RID: 35752
		[PublicizedFrom(EAccessModifier.Private)]
		public MicroStopwatch messageMS = new MicroStopwatch(true);

		// Token: 0x04008BA9 RID: 35753
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

		// Token: 0x04008BAA RID: 35754
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Vector2i[] directions4way = new Vector2i[]
		{
			Vector2i.up,
			Vector2i.right,
			Vector2i.down,
			Vector2i.left
		};

		// Token: 0x02001770 RID: 6000
		public enum BiomeLayout
		{
			// Token: 0x04008BAC RID: 35756
			CenterForest,
			// Token: 0x04008BAD RID: 35757
			CenterWasteland,
			// Token: 0x04008BAE RID: 35758
			Circle,
			// Token: 0x04008BAF RID: 35759
			Circle2,
			// Token: 0x04008BB0 RID: 35760
			Line
		}

		// Token: 0x02001771 RID: 6001
		public struct Data
		{
			// Token: 0x0600BADB RID: 47835 RVA: 0x004568C0 File Offset: 0x00454AC0
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

			// Token: 0x0600BADC RID: 47836 RVA: 0x00456939 File Offset: 0x00454B39
			public void Cleanup()
			{
				this.HeightMap.Dispose();
				this.PathTileGrid.Dispose();
				this.StreetTileDataGrid.Dispose();
				this.poiHeightMask.Dispose();
				this.waterDest.Dispose();
			}

			// Token: 0x0600BADD RID: 47837 RVA: 0x00456972 File Offset: 0x00454B72
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public float GetHeight(Vector2i pos)
			{
				return this.GetHeight(pos.x, pos.y);
			}

			// Token: 0x0600BADE RID: 47838 RVA: 0x00456986 File Offset: 0x00454B86
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public float GetHeight(int x, int y)
			{
				if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
				{
					return 0f;
				}
				return this.HeightMap[x + y * this.WorldSize];
			}

			// Token: 0x0600BADF RID: 47839 RVA: 0x004569B9 File Offset: 0x00454BB9
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool InWorldBounds(int x, int y)
			{
				return (ulong)x < (ulong)((long)this.WorldSize) && (ulong)y < (ulong)((long)this.WorldSize);
			}

			// Token: 0x0600BAE0 RID: 47840 RVA: 0x004569D4 File Offset: 0x00454BD4
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

			// Token: 0x0600BAE1 RID: 47841 RVA: 0x00456A29 File Offset: 0x00454C29
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public byte GetWater(int x, int y)
			{
				if ((ulong)x >= (ulong)((long)this.WorldSize) || (ulong)y >= (ulong)((long)this.WorldSize))
				{
					return 0;
				}
				return (byte)this.waterDest[x + y * this.WorldSize];
			}

			// Token: 0x0600BAE2 RID: 47842 RVA: 0x00456A59 File Offset: 0x00454C59
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public byte GetWater(int _index)
			{
				if ((ulong)_index >= (ulong)((long)(this.WorldSize * this.WorldSize)))
				{
					return 0;
				}
				return (byte)this.waterDest[_index];
			}

			// Token: 0x04008BB1 RID: 35761
			public int WorldSize;

			// Token: 0x04008BB2 RID: 35762
			public NativeArray<float> HeightMap;

			// Token: 0x04008BB3 RID: 35763
			public NativeArray<float> waterDest;

			// Token: 0x04008BB4 RID: 35764
			public int PathTileGridWidth;

			// Token: 0x04008BB5 RID: 35765
			public NativeArray<PathTile> PathTileGrid;

			// Token: 0x04008BB6 RID: 35766
			public int StreetTileDataGridWidth;

			// Token: 0x04008BB7 RID: 35767
			public NativeArray<StreetTileData> StreetTileDataGrid;

			// Token: 0x04008BB8 RID: 35768
			public NativeArray<byte> poiHeightMask;

			// Token: 0x04008BB9 RID: 35769
			public volatile int messageCnt;
		}

		// Token: 0x02001772 RID: 6002
		public enum SaveDataPromptMode
		{
			// Token: 0x04008BBB RID: 35771
			On,
			// Token: 0x04008BBC RID: 35772
			OnInsufficientSpace,
			// Token: 0x04008BBD RID: 35773
			Off
		}

		// Token: 0x02001773 RID: 6003
		[PublicizedFrom(EAccessModifier.Private)]
		public class BiomeTypeData
		{
			// Token: 0x0600BAE3 RID: 47843 RVA: 0x00456A7C File Offset: 0x00454C7C
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

			// Token: 0x04008BBE RID: 35774
			public BiomeType Type;

			// Token: 0x04008BBF RID: 35775
			public float Percent;

			// Token: 0x04008BC0 RID: 35776
			public int TileCount;

			// Token: 0x04008BC1 RID: 35777
			public Vector2i Center;
		}

		// Token: 0x02001774 RID: 6004
		[PublicizedFrom(EAccessModifier.Private)]
		public struct PreviewPoint
		{
			// Token: 0x04008BC2 RID: 35778
			public Vector2i pos;

			// Token: 0x04008BC3 RID: 35779
			public Color32 color;

			// Token: 0x04008BC4 RID: 35780
			public int size;
		}

		// Token: 0x02001775 RID: 6005
		public enum GenerationSelections
		{
			// Token: 0x04008BC6 RID: 35782
			None,
			// Token: 0x04008BC7 RID: 35783
			Few,
			// Token: 0x04008BC8 RID: 35784
			Default,
			// Token: 0x04008BC9 RID: 35785
			Many
		}

		// Token: 0x02001776 RID: 6006
		public struct PlayerSpawn
		{
			// Token: 0x0600BAE4 RID: 47844 RVA: 0x00456AC8 File Offset: 0x00454CC8
			public PlayerSpawn(Vector3 _position, float _yRotation)
			{
				this.Position = _position;
				this.Rotation = _yRotation;
			}

			// Token: 0x0600BAE5 RID: 47845 RVA: 0x00456AD8 File Offset: 0x00454CD8
			public bool IsTooClose(Vector3 _position)
			{
				float num = _position.x - this.Position.x;
				float num2 = _position.z - this.Position.z;
				return num * num + num2 * num2 < 3600f;
			}

			// Token: 0x04008BCA RID: 35786
			[PublicizedFrom(EAccessModifier.Private)]
			public const int cSafeDist = 60;

			// Token: 0x04008BCB RID: 35787
			public Vector3 Position;

			// Token: 0x04008BCC RID: 35788
			public float Rotation;
		}

		// Token: 0x02001790 RID: 6032
		// (Invoke) Token: 0x0600BB61 RID: 47969
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void ClearWaterUnderTerrain_0000BA2A$PostfixBurstDelegate(ref WorldBuilder.Data _data, ref NativeArray<float> _terrain, int startX, int endX, int startY, int endY);

		// Token: 0x02001791 RID: 6033
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class ClearWaterUnderTerrain_0000BA2A$BurstDirectCall
		{
			// Token: 0x0600BB64 RID: 47972 RVA: 0x00459128 File Offset: 0x00457328
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.ClearWaterUnderTerrain$BurstManaged(ref WorldBuilder.Data, ref NativeArray<float>, int, int, int, int)).MethodHandle, typeof(WorldBuilder.ClearWaterUnderTerrain_0000BA2A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BB65 RID: 47973 RVA: 0x00459154 File Offset: 0x00457354
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BB66 RID: 47974 RVA: 0x0045916C File Offset: 0x0045736C
			public static void Constructor()
			{
				WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.ClearWaterUnderTerrain(ref WorldBuilder.Data, ref NativeArray<float>, int, int, int, int)).MethodHandle);
			}

			// Token: 0x0600BB67 RID: 47975 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BB68 RID: 47976 RVA: 0x0045917D File Offset: 0x0045737D
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static ClearWaterUnderTerrain_0000BA2A$BurstDirectCall()
			{
				WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BB69 RID: 47977 RVA: 0x00459184 File Offset: 0x00457384
			public static void Invoke(ref WorldBuilder.Data _data, ref NativeArray<float> _terrain, int startX, int endX, int startY, int endY)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.WorldBuilder/Data&,Unity.Collections.NativeArray`1<System.Single>&,System.Int32,System.Int32,System.Int32,System.Int32), ref _data, ref _terrain, startX, endX, startY, endY, functionPointer);
						return;
					}
				}
				WorldBuilder.ClearWaterUnderTerrain$BurstManaged(ref _data, ref _terrain, startX, endX, startY, endY);
			}

			// Token: 0x04008C3B RID: 35899
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008C3C RID: 35900
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001792 RID: 6034
		// (Invoke) Token: 0x0600BB6B RID: 47979
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void FinalizeWater_0000BA2D$PostfixBurstDelegate(ref WorldBuilder.Data _data, float _WaterHeight);

		// Token: 0x02001793 RID: 6035
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FinalizeWater_0000BA2D$BurstDirectCall
		{
			// Token: 0x0600BB6E RID: 47982 RVA: 0x004591C3 File Offset: 0x004573C3
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.FinalizeWater$BurstManaged(ref WorldBuilder.Data, float)).MethodHandle, typeof(WorldBuilder.FinalizeWater_0000BA2D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BB6F RID: 47983 RVA: 0x004591F0 File Offset: 0x004573F0
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BB70 RID: 47984 RVA: 0x00459208 File Offset: 0x00457408
			public static void Constructor()
			{
				WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.FinalizeWater(ref WorldBuilder.Data, float)).MethodHandle);
			}

			// Token: 0x0600BB71 RID: 47985 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BB72 RID: 47986 RVA: 0x00459219 File Offset: 0x00457419
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FinalizeWater_0000BA2D$BurstDirectCall()
			{
				WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BB73 RID: 47987 RVA: 0x00459220 File Offset: 0x00457420
			public static void Invoke(ref WorldBuilder.Data _data, float _WaterHeight)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.WorldBuilder/Data&,System.Single), ref _data, _WaterHeight, functionPointer);
						return;
					}
				}
				WorldBuilder.FinalizeWater$BurstManaged(ref _data, _WaterHeight);
			}

			// Token: 0x04008C3D RID: 35901
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008C3E RID: 35902
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001794 RID: 6036
		// (Invoke) Token: 0x0600BB75 RID: 47989
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void SmoothRoadTerrainTask_0000BA34$PostfixBurstDelegate(ref WorldBuilder.Data _data, ref NativeArray<Color32> roadMask, ref NativeArray<float> _heightMap, int WorldSize);

		// Token: 0x02001795 RID: 6037
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class SmoothRoadTerrainTask_0000BA34$BurstDirectCall
		{
			// Token: 0x0600BB78 RID: 47992 RVA: 0x00459253 File Offset: 0x00457453
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.SmoothRoadTerrainTask$BurstManaged(ref WorldBuilder.Data, ref NativeArray<Color32>, ref NativeArray<float>, int)).MethodHandle, typeof(WorldBuilder.SmoothRoadTerrainTask_0000BA34$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BB79 RID: 47993 RVA: 0x00459280 File Offset: 0x00457480
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BB7A RID: 47994 RVA: 0x00459298 File Offset: 0x00457498
			public static void Constructor()
			{
				WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.SmoothRoadTerrainTask(ref WorldBuilder.Data, ref NativeArray<Color32>, ref NativeArray<float>, int)).MethodHandle);
			}

			// Token: 0x0600BB7B RID: 47995 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BB7C RID: 47996 RVA: 0x004592A9 File Offset: 0x004574A9
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static SmoothRoadTerrainTask_0000BA34$BurstDirectCall()
			{
				WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BB7D RID: 47997 RVA: 0x004592B0 File Offset: 0x004574B0
			public static void Invoke(ref WorldBuilder.Data _data, ref NativeArray<Color32> roadMask, ref NativeArray<float> _heightMap, int WorldSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.WorldBuilder/Data&,Unity.Collections.NativeArray`1<UnityEngine.Color32>&,Unity.Collections.NativeArray`1<System.Single>&,System.Int32), ref _data, ref roadMask, ref _heightMap, WorldSize, functionPointer);
						return;
					}
				}
				WorldBuilder.SmoothRoadTerrainTask$BurstManaged(ref _data, ref roadMask, ref _heightMap, WorldSize);
			}

			// Token: 0x04008C3F RID: 35903
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008C40 RID: 35904
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001796 RID: 6038
		// (Invoke) Token: 0x0600BB7F RID: 47999
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void AdjustHeights_0000BA47$PostfixBurstDelegate(ref NativeArray<float> _src, float _min);

		// Token: 0x02001797 RID: 6039
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class AdjustHeights_0000BA47$BurstDirectCall
		{
			// Token: 0x0600BB82 RID: 48002 RVA: 0x004592E7 File Offset: 0x004574E7
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.Pointer == 0)
				{
					WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.DeferredCompilation, methodof(WorldBuilder.AdjustHeights$BurstManaged(ref NativeArray<float>, float)).MethodHandle, typeof(WorldBuilder.AdjustHeights_0000BA47$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BB83 RID: 48003 RVA: 0x00459314 File Offset: 0x00457514
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BB84 RID: 48004 RVA: 0x0045932C File Offset: 0x0045752C
			public static void Constructor()
			{
				WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldBuilder.AdjustHeights(ref NativeArray<float>, float)).MethodHandle);
			}

			// Token: 0x0600BB85 RID: 48005 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BB86 RID: 48006 RVA: 0x0045933D File Offset: 0x0045753D
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static AdjustHeights_0000BA47$BurstDirectCall()
			{
				WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BB87 RID: 48007 RVA: 0x00459344 File Offset: 0x00457544
			public static void Invoke(ref NativeArray<float> _src, float _min)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeArray`1<System.Single>&,System.Single), ref _src, _min, functionPointer);
						return;
					}
				}
				WorldBuilder.AdjustHeights$BurstManaged(ref _src, _min);
			}

			// Token: 0x04008C41 RID: 35905
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008C42 RID: 35906
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
