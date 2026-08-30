using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Audio;
using DynamicMusic;
using DynamicMusic.Factories;
using GamePath;
using Platform;
using PrefabVolumes;
using Twitch;
using UnityEngine;

// Token: 0x02000BD9 RID: 3033
public class World : WorldBase, IBlockAccess, IChunkAccess, IChunkCallback
{
	// Token: 0x14000084 RID: 132
	// (add) Token: 0x06005B6D RID: 23405 RVA: 0x002387B4 File Offset: 0x002369B4
	// (remove) Token: 0x06005B6E RID: 23406 RVA: 0x002387EC File Offset: 0x002369EC
	public event World.OnEntityLoadedDelegate EntityLoadedDelegates;

	// Token: 0x14000085 RID: 133
	// (add) Token: 0x06005B6F RID: 23407 RVA: 0x00238824 File Offset: 0x00236A24
	// (remove) Token: 0x06005B70 RID: 23408 RVA: 0x0023885C File Offset: 0x00236A5C
	public event World.OnEntityUnloadedDelegate EntityUnloadedDelegates;

	// Token: 0x06005B72 RID: 23410 RVA: 0x00238A50 File Offset: 0x00236C50
	public virtual void Init(IGameManager _gameManager, WorldBiomes _biomes)
	{
		this.gameManager = _gameManager;
		this.m_ChunkManager = new ChunkManager();
		this.m_ChunkManager.Init(this);
		this.m_SharedChunkObserverCache = new SharedChunkObserverCache(this.m_ChunkManager, 3, new NoThreadingSemantics());
		LightManager.Init();
		this.triggerManager = new TriggerManager();
		this.Biomes = _biomes;
		if (_biomes != null)
		{
			this.biomeSpawnManager = new SpawnManagerBiomes(this);
		}
		this.audioManager = Manager.Instance;
		this.BiomeAtmosphereEffects = new BiomeAtmosphereEffects();
		this.BiomeAtmosphereEffects.Init(this);
	}

	// Token: 0x06005B73 RID: 23411 RVA: 0x00238ADA File Offset: 0x00236CDA
	public IEnumerator LoadWorld(string _levelName, PathAbstractions.AbstractedLocation _worldLocation, bool _fixedSizeCC = false)
	{
		Log.Out(string.Format("World.Load: {0} {1}", _levelName, _worldLocation));
		GamePrefs.Set(EnumGamePrefs.GameWorld, _levelName);
		World.IsSplatMapAvailable = GameManager.IsSplatMapAvailable();
		this.DuskDawnInit();
		this.wcd = new WorldCreationData(_levelName);
		this.worldState = new WorldState();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && _worldLocation.Type != PathAbstractions.EAbstractedLocationType.None)
		{
			string text;
			if (this.IsEditor())
			{
				text = _worldLocation.FullPath + "/main.ttw";
			}
			else
			{
				text = GameIO.GetSaveGameDir() + "/main.ttw";
				if (!SdFile.Exists(text))
				{
					if (!SdDirectory.Exists(GameIO.GetSaveGameDir()))
					{
						SdDirectory.CreateDirectory(GameIO.GetSaveGameDir());
					}
					Log.Out("Loading base world file header...");
					this.worldState.Load(_worldLocation.FullPath + "/main.ttw", false, true, false);
					this.worldState.GenerateNewGuid();
					this.Seed = GamePrefs.GetString(EnumGamePrefs.GameName).GetHashCode();
					this.worldState.SetFrom(this, this.worldState.providerId);
					this.worldState.worldTime = 7000UL;
					this.worldState.saveDataLimit = SaveDataLimit.GetLimitFromPref();
					this.worldState.Save(text);
				}
			}
			if (!this.worldState.Load(text, true, false, !this.IsEditor()))
			{
				Log.Error("Could not load file '" + text + "'!");
			}
			else
			{
				this.Seed = this.worldState.seed;
			}
		}
		this.wcd.Apply(this, this.worldState);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			this.Seed = GamePrefs.GetString(EnumGamePrefs.GameNameClient).GetHashCode();
		}
		GameRandomManager.Instance.SetBaseSeed(this.Seed);
		this.rand = GameRandomManager.Instance.CreateGameRandom();
		this.rand.SetLock();
		this.worldTime = ((!this.IsEditor()) ? this.worldState.worldTime : 12000UL);
		GameTimer.Instance.ticks = this.worldState.timeInTicks;
		EntityFactory.nextEntityID = this.worldState.nextEntityID;
		if (PlatformOptimizations.LimitedSaveData && this.worldState.saveDataLimit < 0L)
		{
			GameUtils.WorldInfo worldInfo = GameUtils.WorldInfo.LoadWorldInfo(_worldLocation);
			SaveDataLimit.SetLimitToPref(SaveDataLimitType.VeryLong.CalculateTotalSize(worldInfo.WorldSize));
		}
		else
		{
			SaveDataLimit.SetLimitToPref(this.worldState.saveDataLimit);
		}
		this.clientLastEntityId = -2;
		if (_worldLocation.Type != PathAbstractions.EAbstractedLocationType.None)
		{
			this.EntitiesTransform = GameObject.Find("/Entities").transform;
			EntityFactory.Init(this.EntitiesTransform);
		}
		this.netEntityPackageQueue = new NetEntityPackageQueue(this);
		this.entityAsyncManager = new EntityAsyncManager(this.netEntityPackageQueue);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.dynamicSpawnManager = new SpawnManagerDynamic(this, null);
			if (this.worldState.dynamicSpawnerState != null && this.worldState.dynamicSpawnerState.Length > 0L)
			{
				using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader.SetBaseStream(this.worldState.dynamicSpawnerState);
					this.dynamicSpawnManager.Read(pooledBinaryReader);
				}
			}
			this.entityDistributer = new NetEntityDistribution(this, 0);
			this.worldBlockTicker = new WorldBlockTicker(this);
			this.aiDirector = new AIDirector(this);
			if (this.worldState.aiDirectorState != null)
			{
				using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader2.SetBaseStream(this.worldState.aiDirectorState);
					this.aiDirector.Load(pooledBinaryReader2);
				}
			}
			if (this.worldState.sleeperVolumeState != null)
			{
				using (PooledBinaryReader pooledBinaryReader3 = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader3.SetBaseStream(this.worldState.sleeperVolumeState);
					this.ReadSleeperVolumes(this.worldState.sleeperVolumesSaveVersion, pooledBinaryReader3);
					goto IL_046D;
				}
			}
			this.nextSleeperVolumeId = 0;
			this.sleeperVolumeMap.Clear();
			this.sleeperVolumes.Clear();
			IL_046D:
			if (this.worldState.triggerVolumeState != null)
			{
				using (PooledBinaryReader pooledBinaryReader4 = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader4.SetBaseStream(this.worldState.triggerVolumeState);
					this.ReadTriggerVolumes(this.worldState.triggerVolumesSaveVersion, pooledBinaryReader4);
					goto IL_04C5;
				}
			}
			this.triggerVolumes.Clear();
			IL_04C5:
			if (this.worldState.wallVolumeState != null)
			{
				using (PooledBinaryReader pooledBinaryReader5 = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader5.SetBaseStream(this.worldState.wallVolumeState);
					this.ReadWallVolumes(this.worldState.wallVolumesSaveVersion, pooledBinaryReader5);
					goto IL_051D;
				}
			}
			this.wallVolumes.Clear();
			IL_051D:
			SleeperVolume.WorldInit();
		}
		DecoManager.Instance.IsEnabled = _levelName != "Empty";
		yield return null;
		ChunkCluster cc = null;
		yield return this.CreateChunkCluster(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? this.worldState.providerId : EnumChunkProviderId.NetworkClient, GamePrefs.GetString(EnumGamePrefs.GameWorld), _worldLocation, _fixedSizeCC, delegate(ChunkCluster _cluster)
		{
			cc = _cluster;
		});
		yield return null;
		XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadWorldEnvironment", false, null), true);
		string text2 = "WorldEnvironment";
		if (this.wcd.Properties.Contains("WorldEnvironment", "Class"))
		{
			text2 = this.wcd.Properties.GetString("WorldEnvironment", "Class");
		}
		GameObject gameObject = new GameObject("WorldEnvironment");
		this.m_WorldEnvironment = gameObject.AddComponent(Type.GetType(text2)) as WorldEnvironment;
		this.m_WorldEnvironment.Init(this.wcd, this);
		DynamicPrefabDecorator decorator = cc.ChunkProvider.GetDynamicPrefabDecorator();
		SelectionBoxManager.Instance.CategoryDynamicPrefab.SetCallback(decorator);
		if (GameManager.Instance.IsEditMode() && !PrefabEditModeManager.Instance.IsActive())
		{
			SelectionBoxManager.Instance.CategoryDynamicPrefab.SetVisible(true);
		}
		if (DecoManager.Instance.IsEnabled)
		{
			IChunkProvider chunkProvider = this.ChunkCache.ChunkProvider;
			yield return DecoManager.Instance.OnWorldLoaded(chunkProvider.GetWorldSize().x, chunkProvider.GetWorldSize().y, this, chunkProvider);
			this.m_WorldEnvironment.CreateUnityTerrain();
		}
		EventPrefabs eventPrefabs = this.ChunkCache.ChunkProvider.GetEventPrefabs();
		if (eventPrefabs != null)
		{
			eventPrefabs.Load();
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			this.m_EventPrefabsClient = new EventPrefabsClient(this.m_PrefabCache, decorator);
		}
		if (!this.IsEditor())
		{
			(this.dmsConductor = Factory.CreateConductor()).Init(true);
			if (!GameManager.IsDedicatedServer)
			{
				yield return this.dmsConductor.PreloadRoutine();
			}
		}
		if (!GameManager.IsDedicatedServer)
		{
			foreach (IEntitlementValidator entitlementValidator in PlatformManager.MultiPlatform.EntitlementValidators)
			{
				TwitchEntitlementManager twitchEntitlementManager = entitlementValidator as TwitchEntitlementManager;
				if (twitchEntitlementManager != null)
				{
					twitchEntitlementManager.Init();
				}
			}
		}
		this.SetupTraders();
		this.SetupSleeperVolumes();
		this.SetupTriggerVolumes();
		this.SetupWallVolumes();
		if (!GameManager.IsDedicatedServer && GameManager.IsSplatMapAvailable())
		{
			if (UnityDistantTerrainTest.Instance == null)
			{
				UnityDistantTerrainTest.Create();
			}
			if (!this.isUnityTerrainConfigured)
			{
				this.isUnityTerrainConfigured = true;
				ChunkProviderGenerateWorldFromRaw chunkProviderGenerateWorldFromRaw = this.ChunkCache.ChunkProvider as ChunkProviderGenerateWorldFromRaw;
				UnityDistantTerrainTest instance = UnityDistantTerrainTest.Instance;
				if (chunkProviderGenerateWorldFromRaw != null)
				{
					instance.HeightMap = chunkProviderGenerateWorldFromRaw.heightData;
					instance.hmWidth = chunkProviderGenerateWorldFromRaw.GetWorldSize().x;
					instance.hmHeight = chunkProviderGenerateWorldFromRaw.GetWorldSize().y;
					instance.TerrainMaterial = MeshDescription.meshes[5].materialDistant;
					instance.TerrainMaterial.renderQueue = 2490;
					instance.WaterMaterial = MeshDescription.meshes[1].materialDistant;
					instance.WaterMaterial.SetVector("_WorldDim", new Vector4((float)chunkProviderGenerateWorldFromRaw.GetWorldSize().x, (float)chunkProviderGenerateWorldFromRaw.GetWorldSize().y, 0f, 0f));
					chunkProviderGenerateWorldFromRaw.GetWaterChunks16x16(out instance.WaterChunks16x16Width, out instance.WaterChunks16x16);
					instance.LoadTerrain();
				}
			}
		}
		yield break;
	}

	// Token: 0x06005B74 RID: 23412 RVA: 0x00238B00 File Offset: 0x00236D00
	public void Save()
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache != null)
		{
			chunkCache.Save();
		}
		this.worldState.SetFrom(this, this.ChunkCache.ChunkProvider.GetProviderId());
		if (this.IsEditor())
		{
			this.worldState.ResetDynamicData();
			this.worldState.nextEntityID = 171;
			this.worldState.Save(PathAbstractions.Contextual.FindActiveWorldLocation().FullPath + "/main.ttw");
		}
		else
		{
			this.worldState.Save(GameIO.GetSaveGameDir() + "/main.ttw");
		}
		this.SaveDecorations();
		SaveDataUtils.SaveDataManager.CommitAsync();
	}

	// Token: 0x06005B75 RID: 23413 RVA: 0x00238BAD File Offset: 0x00236DAD
	public void SaveDecorations()
	{
		DecoManager.Instance.Save();
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x00238BB9 File Offset: 0x00236DB9
	public void SaveWorldState()
	{
		this.worldState.SetFrom(this, this.ChunkCache.ChunkProvider.GetProviderId());
		this.worldState.Save(GameIO.GetSaveGameDir() + "/main.ttw");
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x00238BF4 File Offset: 0x00236DF4
	public virtual void UnloadWorld(bool _bUnloadRespawnableEntities)
	{
		Log.Out("World.Unload");
		if (this.m_WorldEnvironment != null)
		{
			this.m_WorldEnvironment.Cleanup();
			global::UnityEngine.Object.Destroy(this.m_WorldEnvironment.gameObject);
			this.m_WorldEnvironment = null;
		}
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache != null)
		{
			chunkCache.Cleanup();
		}
		this.ChunkCache = null;
		this.UnloadEntities(this.Entities.list, true);
		EntityFactory.Cleanup();
		if (BlockToolSelection.Instance != null)
		{
			BlockToolSelection.Instance.SelectionActive = false;
		}
		SelectionBoxManager.Instance.CategoryDynamicPrefab.Clear();
		SelectionBoxManager.Instance.CategoryTraderTeleport.Clear();
		SelectionBoxManager.Instance.CategorySleeperVolume.Clear();
		SelectionBoxManager.Instance.CategoryTriggerVolume.Clear();
		DecoManager.Instance.OnWorldUnloaded();
		Block.OnWorldUnloaded();
		if (UnityDistantTerrainTest.Instance != null)
		{
			UnityDistantTerrainTest.Instance.Cleanup();
			this.isUnityTerrainConfigured = false;
		}
	}

	// Token: 0x06005B78 RID: 23416 RVA: 0x00238CE0 File Offset: 0x00236EE0
	public virtual void Cleanup()
	{
		Log.Out("World.Cleanup");
		if (this.m_PrefabCache != null)
		{
			this.m_PrefabCache.Clear();
		}
		if (this.m_ChunkManager != null)
		{
			this.m_ChunkManager.Cleanup();
			this.m_ChunkManager = null;
		}
		if (this.audioManager != null)
		{
			this.audioManager.Dispose();
			Manager.CleanUp();
			this.audioManager = null;
		}
		if (this.dmsConductor != null)
		{
			this.dmsConductor.CleanUp();
			this.dmsConductor.OnWorldExit();
			this.dmsConductor = null;
		}
		LightManager.Dispose();
		for (int i = 0; i < this.Entities.list.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.Entities.list[i].RootTransform.gameObject);
		}
		this.Entities.Clear();
		this.EntityAlives.Clear();
		if (this.Biomes != null)
		{
			this.Biomes.Cleanup();
		}
		if (this.entityAsyncManager != null)
		{
			this.entityAsyncManager.Cleanup();
			this.entityAsyncManager = null;
		}
		if (this.entityDistributer != null)
		{
			this.entityDistributer.Cleanup();
			this.entityDistributer = null;
		}
		if (this.netEntityPackageQueue != null)
		{
			this.netEntityPackageQueue.Cleanup();
			this.netEntityPackageQueue = null;
		}
		if (this.biomeSpawnManager != null)
		{
			this.biomeSpawnManager.Cleanup();
			this.biomeSpawnManager = null;
		}
		this.dynamicSpawnManager = null;
		if (this.worldBlockTicker != null)
		{
			this.worldBlockTicker.Cleanup();
			this.worldBlockTicker = null;
		}
		BlockShapeNew.Cleanup();
		this.Biomes = null;
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.Clear();
		}
		this.m_LocalPlayerEntity = null;
		this.aiDirector = null;
		PathFinderThread.Instance = null;
		this.wcd = null;
		this.worldState = null;
		this.BiomeAtmosphereEffects = null;
		DynamicMeshUnity.ClearCachedDynamicMeshChunksList();
		if (this.FlatAreaManager != null)
		{
			this.FlatAreaManager.Cleanup();
		}
		if (this.m_EventPrefabsClient != null)
		{
			this.m_EventPrefabsClient = null;
		}
	}

	// Token: 0x06005B79 RID: 23417 RVA: 0x00238ECC File Offset: 0x002370CC
	public void ClearCaches()
	{
		this.m_ChunkManager.FreePools();
		PathPoint.CompactPool();
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache != null)
		{
			chunkCache.ChunkProvider.ClearCaches();
		}
	}

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06005B7A RID: 23418 RVA: 0x00238EFE File Offset: 0x002370FE
	public string Guid
	{
		get
		{
			if (this.worldState != null)
			{
				return this.worldState.Guid;
			}
			return null;
		}
	}

	// Token: 0x06005B7B RID: 23419 RVA: 0x00238F15 File Offset: 0x00237115
	public long GetNextChunkToProvide()
	{
		return this.m_ChunkManager.GetNextChunkToProvide();
	}

	// Token: 0x06005B7C RID: 23420 RVA: 0x00238F22 File Offset: 0x00237122
	public virtual IEnumerator CreateChunkCluster(EnumChunkProviderId _chunkProviderId, string _clusterName, PathAbstractions.AbstractedLocation _worldLocation, bool _bFixedSize, Action<ChunkCluster> _resultHandler)
	{
		ChunkCluster cc = new ChunkCluster(this, _clusterName);
		this.ChunkCache = cc;
		cc.IsFixedSize = _bFixedSize;
		cc.AddChunkCallback(this);
		WaterSimulationNative.Instance.Init(cc);
		yield return cc.Init(_chunkProviderId, _worldLocation);
		_resultHandler(cc);
		yield break;
	}

	// Token: 0x06005B7D RID: 23421 RVA: 0x00238F56 File Offset: 0x00237156
	public override void AddLocalPlayer(EntityPlayerLocal _localPlayer)
	{
		if (!this.m_LocalPlayerEntities.Contains(_localPlayer))
		{
			this.m_LocalPlayerEntities.Add(_localPlayer);
		}
		if (this.objectsOnMap == null)
		{
			this.objectsOnMap = new MapObjectManager();
		}
	}

	// Token: 0x06005B7E RID: 23422 RVA: 0x00238F85 File Offset: 0x00237185
	public override void RemoveLocalPlayer(EntityPlayerLocal _localPlayer)
	{
		this.m_LocalPlayerEntities.Remove(_localPlayer);
	}

	// Token: 0x06005B7F RID: 23423 RVA: 0x00238F94 File Offset: 0x00237194
	public override List<EntityPlayerLocal> GetLocalPlayers()
	{
		return this.m_LocalPlayerEntities;
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x00238F9C File Offset: 0x0023719C
	public override bool IsLocalPlayer(int _playerId)
	{
		Entity entity = this.GetEntity(_playerId);
		return entity != null && entity is EntityPlayerLocal;
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x00238FC5 File Offset: 0x002371C5
	public override EntityPlayerLocal GetLocalPlayerFromID(int _playerId)
	{
		return this.GetEntity(_playerId) as EntityPlayerLocal;
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x00238FD4 File Offset: 0x002371D4
	public override EntityPlayerLocal GetClosestLocalPlayer(Vector3 _position)
	{
		EntityPlayerLocal entityPlayerLocal = this.GetPrimaryPlayer();
		if (this.m_LocalPlayerEntities.Count > 1)
		{
			float num = float.MaxValue;
			for (int i = 0; i < this.m_LocalPlayerEntities.Count; i++)
			{
				float sqrMagnitude = (this.m_LocalPlayerEntities[i].GetPosition() - _position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					entityPlayerLocal = this.m_LocalPlayerEntities[i];
				}
			}
		}
		return entityPlayerLocal;
	}

	// Token: 0x06005B83 RID: 23427 RVA: 0x00239049 File Offset: 0x00237249
	public override Vector3 GetVectorToClosestLocalPlayer(Vector3 _position)
	{
		return this.GetClosestLocalPlayer(_position).GetPosition() - _position;
	}

	// Token: 0x06005B84 RID: 23428 RVA: 0x00239060 File Offset: 0x00237260
	public override float GetSquaredDistanceToClosestLocalPlayer(Vector3 _position)
	{
		return this.GetVectorToClosestLocalPlayer(_position).sqrMagnitude;
	}

	// Token: 0x06005B85 RID: 23429 RVA: 0x0023907C File Offset: 0x0023727C
	public override float GetDistanceToClosestLocalPlayer(Vector3 _position)
	{
		return this.GetVectorToClosestLocalPlayer(_position).magnitude;
	}

	// Token: 0x06005B86 RID: 23430 RVA: 0x00239098 File Offset: 0x00237298
	public void SetLocalPlayer(EntityPlayerLocal _thePlayer)
	{
		this.m_LocalPlayerEntity = _thePlayer;
		this.audioManager.AttachLocalPlayer(_thePlayer, this);
		LightManager.AttachLocalPlayer(_thePlayer, this);
		OcclusionManager.Instance.SetSourceDepthCamera(_thePlayer.playerCamera);
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x002390C5 File Offset: 0x002372C5
	public override EntityPlayerLocal GetPrimaryPlayer()
	{
		return this.m_LocalPlayerEntity;
	}

	// Token: 0x06005B88 RID: 23432 RVA: 0x002390CD File Offset: 0x002372CD
	public int GetPrimaryPlayerId()
	{
		if (!(this.m_LocalPlayerEntity != null))
		{
			return -1;
		}
		return this.m_LocalPlayerEntity.entityId;
	}

	// Token: 0x06005B89 RID: 23433 RVA: 0x002390EA File Offset: 0x002372EA
	public override List<EntityPlayer> GetPlayers()
	{
		return this.Players.list;
	}

	// Token: 0x06005B8A RID: 23434 RVA: 0x002390F8 File Offset: 0x002372F8
	public void GetSunAndBlockColors(Vector3i _worldBlockPos, out byte sunLight, out byte blockLight)
	{
		sunLight = 0;
		blockLight = 0;
		IChunk chunkFromWorldPos = base.GetChunkFromWorldPos(_worldBlockPos);
		if (chunkFromWorldPos != null)
		{
			int num = World.toBlockXZ(_worldBlockPos.x);
			int num2 = World.toBlockY(_worldBlockPos.y);
			int num3 = World.toBlockXZ(_worldBlockPos.z);
			sunLight = chunkFromWorldPos.GetLight(num, num2, num3, Chunk.LIGHT_TYPE.SUN);
			blockLight = chunkFromWorldPos.GetLight(num, num2, num3, Chunk.LIGHT_TYPE.BLOCK);
		}
	}

	// Token: 0x06005B8B RID: 23435 RVA: 0x00239154 File Offset: 0x00237354
	public override float GetLightBrightness(Vector3i blockPos)
	{
		IChunk chunkFromWorldPos = base.GetChunkFromWorldPos(blockPos);
		if (chunkFromWorldPos != null)
		{
			int num = World.toBlockXZ(blockPos.x);
			int num2 = World.toBlockY(blockPos.y);
			int num3 = World.toBlockXZ(blockPos.z);
			return chunkFromWorldPos.GetLightBrightness(num, num2, num3, 0);
		}
		if (!this.IsDaytime())
		{
			return 0.1f;
		}
		return 0.65f;
	}

	// Token: 0x06005B8C RID: 23436 RVA: 0x002391B0 File Offset: 0x002373B0
	public override int GetBlockLightValue(Vector3i blockPos)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return (int)MarchingCubes.DensityAir;
		}
		IChunk chunkFromWorldPos = chunkCache.GetChunkFromWorldPos(blockPos);
		if (chunkFromWorldPos != null)
		{
			int num = World.toBlockXZ(blockPos.x);
			int num2 = World.toBlockY(blockPos.y);
			int num3 = World.toBlockXZ(blockPos.z);
			return chunkFromWorldPos.GetLightValue(num, num2, num3, 0);
		}
		return 0;
	}

	// Token: 0x06005B8D RID: 23437 RVA: 0x0023920C File Offset: 0x0023740C
	public override sbyte GetDensity(int _x, int _y, int _z)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return MarchingCubes.DensityAir;
		}
		IChunk chunkFromWorldPos = chunkCache.GetChunkFromWorldPos(_x, _y, _z);
		if (chunkFromWorldPos != null)
		{
			return chunkFromWorldPos.GetDensity(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z));
		}
		return MarchingCubes.DensityAir;
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x00239254 File Offset: 0x00237454
	public void SetDensity(Vector3i _pos, sbyte _density, bool _bFoceDensity = false)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		chunkCache.SetDensity(_pos, _density, _bFoceDensity);
	}

	// Token: 0x06005B8F RID: 23439 RVA: 0x00239278 File Offset: 0x00237478
	public long GetTexture(int _x, int _y, int _z, int channel = 0)
	{
		Chunk chunk = (Chunk)this.ChunkCache.GetChunkFromWorldPos(_x, _y, _z);
		if (chunk != null)
		{
			return chunk.GetTextureFull(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z), channel);
		}
		return 0L;
	}

	// Token: 0x06005B90 RID: 23440 RVA: 0x002392BC File Offset: 0x002374BC
	public TextureFullArray GetTextureFullArray(int _x, int _y, int _z)
	{
		Chunk chunk = (Chunk)this.ChunkCache.GetChunkFromWorldPos(_x, _y, _z);
		if (chunk != null)
		{
			return chunk.GetTextureFullArray(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z), true);
		}
		return new TextureFullArray(0L);
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x00239301 File Offset: 0x00237501
	public void SetTexture(int _x, int _y, int _z, long _tex, int channel = 0)
	{
		this.ChunkCache.SetTextureFull(new Vector3i(_x, _y, _z), _tex, channel);
	}

	// Token: 0x06005B92 RID: 23442 RVA: 0x0023931C File Offset: 0x0023751C
	public override byte GetStability(int worldX, int worldY, int worldZ)
	{
		IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunkSync != null)
		{
			return chunkSync.GetStability(World.toBlockXZ(worldX), World.toBlockY(worldY), World.toBlockXZ(worldZ));
		}
		return 0;
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x00239359 File Offset: 0x00237559
	public override byte GetStability(Vector3i _pos)
	{
		return this.GetStability(_pos.x, _pos.y, _pos.z);
	}

	// Token: 0x06005B94 RID: 23444 RVA: 0x00239374 File Offset: 0x00237574
	public override void SetStability(int worldX, int worldY, int worldZ, byte stab)
	{
		IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunkSync != null)
		{
			chunkSync.SetStability(World.toBlockXZ(worldX), World.toBlockY(worldY), World.toBlockXZ(worldZ), stab);
		}
	}

	// Token: 0x06005B95 RID: 23445 RVA: 0x002393B1 File Offset: 0x002375B1
	public override void SetStability(Vector3i _pos, byte stab)
	{
		this.SetStability(_pos.x, _pos.y, _pos.z, stab);
	}

	// Token: 0x06005B96 RID: 23446 RVA: 0x002393CC File Offset: 0x002375CC
	public override byte GetHeight(int worldX, int worldZ)
	{
		IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunkSync != null)
		{
			return chunkSync.GetHeight(World.toBlockXZ(worldX), World.toBlockXZ(worldZ));
		}
		return 0;
	}

	// Token: 0x06005B97 RID: 23447 RVA: 0x00239404 File Offset: 0x00237604
	public byte GetTerrainHeight(int worldX, int worldZ)
	{
		Chunk chunk = (Chunk)this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunk != null)
		{
			return chunk.GetTerrainHeight(World.toBlockXZ(worldX), World.toBlockXZ(worldZ));
		}
		return 0;
	}

	// Token: 0x06005B98 RID: 23448 RVA: 0x00239440 File Offset: 0x00237640
	public float GetHeightAt(float worldX, float worldZ)
	{
		IChunkProvider chunkProvider = this.ChunkCache.ChunkProvider;
		ITerrainGenerator terrainGenerator = ((chunkProvider != null) ? chunkProvider.GetTerrainGenerator() : null);
		if (terrainGenerator != null)
		{
			return terrainGenerator.GetTerrainHeightAt((int)worldX, (int)worldZ);
		}
		return 0f;
	}

	// Token: 0x06005B99 RID: 23449 RVA: 0x0023947C File Offset: 0x0023767C
	public bool GetWaterAt(float worldX, float worldZ)
	{
		ChunkProviderGenerateWorldFromRaw chunkProviderGenerateWorldFromRaw = this.ChunkCache.ChunkProvider as ChunkProviderGenerateWorldFromRaw;
		if (chunkProviderGenerateWorldFromRaw == null)
		{
			return false;
		}
		WorldDecoratorPOIFromImage poiFromImage = chunkProviderGenerateWorldFromRaw.poiFromImage;
		if (poiFromImage == null)
		{
			return false;
		}
		if (!poiFromImage.m_Poi.Contains((int)worldX, (int)worldZ))
		{
			return false;
		}
		byte data = poiFromImage.m_Poi.GetData((int)worldX, (int)worldZ);
		if (data == 0)
		{
			return false;
		}
		PoiMapElement poiForColor = this.Biomes.getPoiForColor((uint)data);
		return poiForColor != null && poiForColor.m_BlockValue.type == 240;
	}

	// Token: 0x06005B9A RID: 23450 RVA: 0x002394F8 File Offset: 0x002376F8
	public override bool IsWater(int _x, int _y, int _z)
	{
		if (_y < 256)
		{
			IChunk chunkFromWorldPos = base.GetChunkFromWorldPos(_x, _y, _z);
			if (chunkFromWorldPos != null)
			{
				_x &= 15;
				_y &= 255;
				_z &= 15;
				return chunkFromWorldPos.IsWater(_x, _y, _z);
			}
		}
		return false;
	}

	// Token: 0x06005B9B RID: 23451 RVA: 0x0023953A File Offset: 0x0023773A
	public override bool IsWater(Vector3i _pos)
	{
		return this.IsWater(_pos.x, _pos.y, _pos.z);
	}

	// Token: 0x06005B9C RID: 23452 RVA: 0x00239554 File Offset: 0x00237754
	public override bool IsWater(Vector3 _pos)
	{
		return this.IsWater(World.worldToBlockPos(_pos));
	}

	// Token: 0x06005B9D RID: 23453 RVA: 0x00239564 File Offset: 0x00237764
	public override bool IsAir(int _x, int _y, int _z)
	{
		if (_y < 256)
		{
			IChunk chunkFromWorldPos = base.GetChunkFromWorldPos(_x, _y, _z);
			if (chunkFromWorldPos != null)
			{
				_x &= 15;
				_y &= 255;
				_z &= 15;
				return chunkFromWorldPos.IsAir(_x, _y, _z);
			}
		}
		return true;
	}

	// Token: 0x06005B9E RID: 23454 RVA: 0x002395A8 File Offset: 0x002377A8
	public bool CheckForLevelNearbyHeights(float worldX, float worldZ, int distance)
	{
		IChunkProvider chunkProvider = this.ChunkCache.ChunkProvider;
		ITerrainGenerator terrainGenerator = ((chunkProvider != null) ? chunkProvider.GetTerrainGenerator() : null);
		if (terrainGenerator != null)
		{
			float num = terrainGenerator.GetTerrainHeightAt((int)worldX, (int)worldZ);
			float num2 = num;
			float num3 = num;
			num = terrainGenerator.GetTerrainHeightAt((int)worldX + distance, (int)worldZ);
			if (num < num2)
			{
				num2 = num;
			}
			else if (num > num3)
			{
				num3 = num;
			}
			num = terrainGenerator.GetTerrainHeightAt((int)worldX - distance, (int)worldZ);
			if (num < num2)
			{
				num2 = num;
			}
			else if (num > num3)
			{
				num3 = num;
			}
			num = terrainGenerator.GetTerrainHeightAt((int)worldX, (int)worldZ + distance);
			if (num < num2)
			{
				num2 = num;
			}
			else if (num > num3)
			{
				num3 = num;
			}
			num = terrainGenerator.GetTerrainHeightAt((int)worldX, (int)worldZ - distance);
			if (num < num2)
			{
				num2 = num;
			}
			else if (num > num3)
			{
				num3 = num;
			}
			return Mathf.Abs(num3 - num2) <= 2f;
		}
		return false;
	}

	// Token: 0x06005B9F RID: 23455 RVA: 0x00239688 File Offset: 0x00237888
	public bool FindRandomSpawnPointNearRandomPlayer(int maxLightValue, out int x, out int y, out int z)
	{
		if (this.Players.list.Count == 0)
		{
			x = (y = (z = 0));
			return false;
		}
		Entity entity = null;
		int num = this.GetGameRandom().RandomRange(this.Players.list.Count);
		for (int i = 0; i < this.Players.list.Count; i++)
		{
			entity = this.Players.list[i];
			if (num-- == 0)
			{
				break;
			}
		}
		return this.FindRandomSpawnPointNearPlayer(entity, maxLightValue, out x, out y, out z, 32);
	}

	// Token: 0x06005BA0 RID: 23456 RVA: 0x00239718 File Offset: 0x00237918
	public bool FindRandomSpawnPointNearPlayer(Entity _entityPlayer, int maxLightValue, out int x, out int y, out int z, int maxDistance)
	{
		return this.FindRandomSpawnPointNearPosition(_entityPlayer.GetPosition(), maxLightValue, out x, out y, out z, new Vector3((float)maxDistance, (float)maxDistance, (float)maxDistance), true, false);
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x00239748 File Offset: 0x00237948
	public bool FindRandomSpawnPointNearPositionUnderground(Vector3 _pos, int maxLightValue, out int x, out int y, out int z, Vector3 maxDistance)
	{
		x = (y = (z = 0));
		for (int i = 0; i < 5; i++)
		{
			x = Utils.Fastfloor(_pos.x + this.RandomRange(-maxDistance.x / 2f, maxDistance.x / 2f));
			z = Utils.Fastfloor(_pos.z + this.RandomRange(-maxDistance.z / 2f, maxDistance.z / 2f));
			Chunk chunk = (Chunk)base.GetChunkFromWorldPos(x, z);
			if (chunk != null && this.IsInPlayfield(chunk))
			{
				int num = World.toBlockXZ(x);
				int num2 = World.toBlockXZ(z);
				int num3 = Utils.Fastfloor(_pos.y - maxDistance.y / 2f);
				int num4 = Utils.Fastfloor(_pos.y + maxDistance.y / 2f);
				int num5 = (int)_pos.y;
				if (num5 >= num3 && num5 <= num4 && chunk.CanMobsSpawnAtPos(num, num5, num2, false, true))
				{
					y = num5;
					return true;
				}
				if (chunk.FindSpawnPointAtXZ(num, num2, out y, maxLightValue, 0, num3, num4, false))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005BA2 RID: 23458 RVA: 0x00239884 File Offset: 0x00237A84
	public bool FindRandomSpawnPointNearPosition(Vector3 _pos, int maxLightValue, out int x, out int y, out int z, Vector3 maxDistance, bool _bOnGround, bool _bIgnoreCanMobsSpawnOn = false)
	{
		x = (y = (z = 0));
		for (int i = 0; i < 5; i++)
		{
			x = Utils.Fastfloor(_pos.x + this.RandomRange(-maxDistance.x / 2f, maxDistance.x / 2f));
			z = Utils.Fastfloor(_pos.z + this.RandomRange(-maxDistance.z / 2f, maxDistance.z / 2f));
			Chunk chunk = (Chunk)base.GetChunkFromWorldPos(x, z);
			if (chunk != null && this.IsInPlayfield(chunk))
			{
				if (!_bOnGround)
				{
					y = Utils.Fastfloor(_pos.y + this.RandomRange(-maxDistance.y / 2f, maxDistance.y / 2f));
					return true;
				}
				int num = World.toBlockXZ(x);
				int num2 = World.toBlockXZ(z);
				int num3 = Utils.Fastfloor(_pos.y - maxDistance.y / 2f);
				int num4 = Utils.Fastfloor(_pos.y + maxDistance.y / 2f);
				int num5 = (int)(chunk.GetHeight(num, num2) + 1);
				if (num5 >= num3 && num5 <= num4 && chunk.CanMobsSpawnAtPos(num, num5, num2, _bIgnoreCanMobsSpawnOn, true))
				{
					y = num5;
					return true;
				}
				if (chunk.FindSpawnPointAtXZ(num, num2, out y, maxLightValue, 0, num3, num4, _bIgnoreCanMobsSpawnOn))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005BA3 RID: 23459 RVA: 0x00239A00 File Offset: 0x00237C00
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPositionInRangeOfBedrolls(Vector3 _position)
	{
		int num = GamePrefs.GetInt(EnumGamePrefs.BedrollDeadZoneSize);
		num *= num;
		for (int i = 0; i < this.Players.list.Count; i++)
		{
			EntityBedrollPositionList spawnPoints = this.Players.list[i].SpawnPoints;
			int count = spawnPoints.Count;
			for (int j = 0; j < count; j++)
			{
				if ((spawnPoints[j].ToVector3() - _position).sqrMagnitude < (float)num)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x00239A8C File Offset: 0x00237C8C
	public bool GetRandomSpawnPositionMinMaxToRandomPlayer(int _minRange, int _maxRange, bool _bConsiderBedrolls, out EntityPlayer _player, out Vector3 _position)
	{
		_position = Vector3.zero;
		_player = null;
		if (this.Players.list.Count == 0)
		{
			return false;
		}
		if (_maxRange - _minRange <= 0)
		{
			return false;
		}
		int num = this.rand.RandomRange(this.Players.list.Count);
		for (int i = 0; i < this.Players.list.Count; i++)
		{
			if (num-- == 0)
			{
				_player = this.Players.list[i];
				break;
			}
		}
		int num2 = _minRange * _minRange;
		for (int j = 0; j < 10; j++)
		{
			Vector2 vector = Vector2.zero;
			do
			{
				vector = this.rand.RandomInsideUnitCircle * (float)(_maxRange - _minRange);
			}
			while ((double)vector.sqrMagnitude < 0.01);
			vector += vector * ((float)_minRange / vector.magnitude);
			_position = _player.GetPosition() + new Vector3(vector.x, 0f, vector.y);
			Vector3i vector3i = World.worldToBlockPos(_position);
			Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
			if (chunk != null)
			{
				int num3 = World.toBlockXZ(vector3i.x);
				int num4 = World.toBlockXZ(vector3i.z);
				vector3i.y = (int)(chunk.GetHeight(num3, num4) + 1);
				_position.y = (float)vector3i.y;
				if ((!_bConsiderBedrolls || !this.isPositionInRangeOfBedrolls(vector3i.ToVector3())) && chunk.CanMobsSpawnAtPos(num3, Utils.Fastfloor(_position.y), num4, false, true))
				{
					bool flag = true;
					for (int k = 0; k < this.Players.list.Count; k++)
					{
						EntityPlayer entityPlayer = this.Players.list[k];
						if (entityPlayer.GetDistanceSq(_position) < (float)num2)
						{
							flag = false;
							break;
						}
						if (entityPlayer.CanSee(_position))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						_position = vector3i.ToVector3() + new Vector3(0.5f, this.GetTerrainOffset(vector3i), 0.5f);
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06005BA5 RID: 23461 RVA: 0x00239CD0 File Offset: 0x00237ED0
	public bool GetMobRandomSpawnPosWithWater(Vector3 _targetPos, int _minRange, int _maxRange, int _minPlayerRange, bool _checkBedrolls, out Vector3 _position)
	{
		return this.GetRandomSpawnPositionMinMaxToPosition(_targetPos, _minRange, _maxRange, _minPlayerRange, _checkBedrolls, out _position, -1, true, 20, false, EnumLandClaimOwner.None, false) || this.GetRandomSpawnPositionMinMaxToPosition(_targetPos, _minRange, _maxRange, _minPlayerRange, _checkBedrolls, out _position, -1, false, 20, false, EnumLandClaimOwner.None, false);
	}

	// Token: 0x06005BA6 RID: 23462 RVA: 0x00239D14 File Offset: 0x00237F14
	public bool GetRandomSpawnPositionMinMaxToPosition(Vector3 _targetPos, int _minRange, int _maxRange, int _minPlayerRange, bool _checkBedrolls, out Vector3 _position, int _forPlayerEntityId = -1, bool _checkWater = true, int _retryCount = 50, bool _checkLandClaim = false, EnumLandClaimOwner _maxLandClaimType = EnumLandClaimOwner.None, bool _useSquareRadius = false)
	{
		_position = Vector3.zero;
		int num = _maxRange - _minRange;
		if (num <= 0)
		{
			return false;
		}
		PersistentPlayerData persistentPlayerData = (_checkLandClaim ? GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(_forPlayerEntityId) : null);
		for (int i = 0; i < _retryCount; i++)
		{
			if (_useSquareRadius)
			{
				int num2;
				int num3;
				do
				{
					num2 = this.rand.RandomRange(-_maxRange, _maxRange + 1);
					num3 = this.rand.RandomRange(-_maxRange, _maxRange + 1);
				}
				while (Mathf.Abs(num2) < _minRange && Mathf.Abs(num3) < _minRange);
				_position.x = _targetPos.x + (float)num2;
				_position.y = _targetPos.y;
				_position.z = _targetPos.z + (float)num3;
			}
			else
			{
				Vector2 vector;
				do
				{
					vector = this.rand.RandomInsideUnitCircle * (float)num;
				}
				while (vector.sqrMagnitude < 0.01f);
				vector += vector * ((float)_minRange / vector.magnitude);
				_position.x = _targetPos.x + vector.x;
				_position.y = _targetPos.y;
				_position.z = _targetPos.z + vector.y;
			}
			Vector3i vector3i = World.worldToBlockPos(_position);
			Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
			if (chunk != null)
			{
				int num4 = World.toBlockXZ(vector3i.x);
				int num5 = World.toBlockXZ(vector3i.z);
				vector3i.y = (int)(chunk.GetHeight(num4, num5) + 1);
				_position.y = (float)vector3i.y;
				if (!_checkBedrolls || !this.isPositionInRangeOfBedrolls(vector3i.ToVector3()))
				{
					if (_forPlayerEntityId == -1)
					{
						if (!chunk.CanMobsSpawnAtPos(num4, Utils.Fastfloor(_position.y), num5, false, _checkWater))
						{
							goto IL_026E;
						}
					}
					else if (!chunk.CanPlayersSpawnAtPos(num4, Utils.Fastfloor(_position.y), num5, false) || !chunk.IsPositionOnTerrain(num4, vector3i.y, num5) || this.GetPOIAtPosition(_position, null, null) != null || (_checkWater && chunk.IsWater(num4, vector3i.y - 1, num5)) || (_checkLandClaim && this.GetLandClaimOwner(vector3i, persistentPlayerData) > _maxLandClaimType))
					{
						goto IL_026E;
					}
					if (this.isPositionFarFromPlayers(_position, _minPlayerRange))
					{
						_position = vector3i.ToVector3() + new Vector3(0.5f, this.GetTerrainOffset(vector3i), 0.5f);
						return true;
					}
				}
			}
			IL_026E:;
		}
		_position = Vector3.zero;
		return false;
	}

	// Token: 0x06005BA7 RID: 23463 RVA: 0x00239FA8 File Offset: 0x002381A8
	public bool GetRandomSpawnPositionInAreaMinMaxToPlayers(Rect _area, int _minDistance, int UNUSED_maxDistance, bool _checkBedrolls, out Vector3 _position, out Chunk _chunk)
	{
		_position = Vector3.zero;
		_chunk = null;
		if (this.Players.list.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < 10; i++)
		{
			_position.x = _area.x + this.RandomRange(0f, _area.width - 1f);
			_position.y = 0f;
			_position.z = _area.y + this.RandomRange(0f, _area.height - 1f);
			Vector3i vector3i = World.worldToBlockPos(_position);
			Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
			if (chunk != null)
			{
				int num = World.toBlockXZ(vector3i.x);
				int num2 = World.toBlockXZ(vector3i.z);
				vector3i.y = (int)(chunk.GetHeight(num, num2) + 1);
				_position.y = (float)vector3i.y;
				if ((!_checkBedrolls || !this.isPositionInRangeOfBedrolls(vector3i.ToVector3())) && chunk.CanMobsSpawnAtPos(num, Utils.Fastfloor(_position.y), num2, false, true))
				{
					bool flag = this.isPositionFarFromPlayers(_position, _minDistance);
					if (flag)
					{
						for (int j = 0; j < this.Players.list.Count; j++)
						{
							EntityPlayer entityPlayer = this.Players.list[j];
							if ((_position - entityPlayer.position).sqrMagnitude < 2500f && entityPlayer.IsInViewCone(_position))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							_position = vector3i.ToVector3() + new Vector3(0.5f, this.GetTerrainOffset(vector3i), 0.5f);
							_chunk = chunk;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06005BA8 RID: 23464 RVA: 0x0023A188 File Offset: 0x00238388
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPositionFarFromPlayers(Vector3 _position, int _minDistance)
	{
		int num = _minDistance * _minDistance;
		for (int i = 0; i < this.Players.list.Count; i++)
		{
			if (this.Players.list[i].GetDistanceSq(_position) < (float)num)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005BA9 RID: 23465 RVA: 0x0023A1D4 File Offset: 0x002383D4
	public Vector3 FindSupportingBlockPos(Vector3 pos)
	{
		Vector3i vector3i = World.worldToBlockPos(pos);
		BlockValue blockValue = base.GetBlock(vector3i);
		Block block = blockValue.Block;
		if (block.IsMovementBlocked(this, vector3i, blockValue, BlockFace.Top))
		{
			return pos;
		}
		if (block.IsElevator())
		{
			return pos;
		}
		vector3i.y++;
		blockValue = base.GetBlock(vector3i);
		block = blockValue.Block;
		if (block.IsElevator((int)blockValue.rotation))
		{
			return pos;
		}
		vector3i.y -= 2;
		blockValue = base.GetBlock(vector3i);
		block = blockValue.Block;
		if (!block.IsElevator() && !block.IsMovementBlocked(this, vector3i, blockValue, BlockFace.Top))
		{
			Vector3 vector = new Vector3((float)vector3i.x + 0.5f, pos.y, (float)vector3i.z + 0.5f);
			Vector3 vector2 = pos - vector;
			int num = Mathf.RoundToInt((Mathf.Atan2(vector2.x, vector2.z) * 57.29578f + 22.5f) / 45f) & 7;
			int[] array = World.supportOrder[num];
			Vector3i vector3i2;
			vector3i2.y = vector3i.y;
			for (int i = 0; i < 8; i++)
			{
				int num2 = array[i] * 2;
				vector3i2.x = vector3i.x + World.supportOffsets[num2];
				vector3i2.z = vector3i.z + World.supportOffsets[num2 + 1];
				blockValue = base.GetBlock(vector3i2);
				block = blockValue.Block;
				if (block.IsMovementBlocked(this, vector3i2, blockValue, BlockFace.Top))
				{
					pos.x = (float)vector3i2.x + 0.5f;
					pos.z = (float)vector3i2.z + 0.5f;
					break;
				}
			}
		}
		return pos;
	}

	// Token: 0x06005BAA RID: 23466 RVA: 0x0023A380 File Offset: 0x00238580
	public float GetTerrainOffset(Vector3i _blockPos)
	{
		float num = 0f;
		if (base.GetBlock(_blockPos - Vector3i.up).Block.shape.IsTerrain())
		{
			sbyte density = base.GetDensity(_blockPos);
			sbyte density2 = base.GetDensity(_blockPos - Vector3i.up);
			num = MarchingCubes.GetDecorationOffsetY(density, density2);
		}
		return num;
	}

	// Token: 0x06005BAB RID: 23467 RVA: 0x0023A3DC File Offset: 0x002385DC
	public bool IsInPlayfield(Chunk _c)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		return !chunkCache.IsFixedSize || this.IsEditor() || (_c.X > chunkCache.ChunkMinPos.x && _c.Z > chunkCache.ChunkMinPos.y && _c.X < chunkCache.ChunkMaxPos.x && _c.Z < chunkCache.ChunkMaxPos.y);
	}

	// Token: 0x06005BAC RID: 23468 RVA: 0x0023A454 File Offset: 0x00238654
	public override BlockValue GetBlock(int x, int y, int z)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return BlockValue.Air;
		}
		return chunkCache.GetBlock(x, y, z);
	}

	// Token: 0x06005BAD RID: 23469 RVA: 0x0023A47A File Offset: 0x0023867A
	public WaterValue GetWater(int _x, int _y, int _z)
	{
		return this.GetWater(new Vector3i(_x, _y, _z));
	}

	// Token: 0x06005BAE RID: 23470 RVA: 0x0023A48C File Offset: 0x0023868C
	public WaterValue GetWater(Vector3i _pos)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return WaterValue.Empty;
		}
		return chunkCache.GetWater(_pos);
	}

	// Token: 0x06005BAF RID: 23471 RVA: 0x0023A4B0 File Offset: 0x002386B0
	public float GetWaterPercent(Vector3i _pos)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return 0f;
		}
		return chunkCache.GetWater(_pos).GetMassPercent();
	}

	// Token: 0x06005BB0 RID: 23472 RVA: 0x0023A4DC File Offset: 0x002386DC
	public override PropValue GetProp(int chunkX, int chunkZ, int propId)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return PropValue.AIR;
		}
		return chunkCache.GetProp(chunkX, chunkZ, propId);
	}

	// Token: 0x06005BB1 RID: 23473 RVA: 0x0023A504 File Offset: 0x00238704
	public override PropValue GetProp(long chunkKey, int propId)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return PropValue.AIR;
		}
		return chunkCache.GetProp(chunkKey, propId);
	}

	// Token: 0x06005BB2 RID: 23474 RVA: 0x0023A529 File Offset: 0x00238729
	public void HandleWaterLevelChanged(Vector3i _pos, float _waterPercent)
	{
		GameLightManager instance = GameLightManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.HandleWaterLevelChanged();
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x0023A53A File Offset: 0x0023873A
	public BiomeDefinition GetBiome(string _name)
	{
		return this.Biomes.GetBiome(_name);
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x0023A548 File Offset: 0x00238748
	public BiomeDefinition GetBiome(int _x, int _z)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_x, _z);
		if (chunk != null)
		{
			byte biomeId = chunk.GetBiomeId(World.toBlockXZ(_x), World.toBlockXZ(_z));
			return this.Biomes.GetBiome(biomeId);
		}
		return null;
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x0023A588 File Offset: 0x00238788
	public BiomeDefinition GetBiomeInWorld(int _x, int _z)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		IChunkProvider chunkProvider = ((chunkCache != null) ? chunkCache.ChunkProvider : null);
		if (chunkProvider != null)
		{
			IBiomeProvider biomeProvider = chunkProvider.GetBiomeProvider();
			if (biomeProvider != null)
			{
				return biomeProvider.GetBiomeAt(_x, _z);
			}
		}
		return null;
	}

	// Token: 0x06005BB6 RID: 23478 RVA: 0x0023A5C0 File Offset: 0x002387C0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override IChunk GetChunkSync(int chunkX, int chunkZ)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		return chunkCache.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x0023A5E4 File Offset: 0x002387E4
	public override IChunk GetChunkSync(long _key)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		return chunkCache.GetChunkSync(_key);
	}

	// Token: 0x06005BB8 RID: 23480 RVA: 0x0023A604 File Offset: 0x00238804
	public bool IsChunkAreaLoaded(Vector3 _position)
	{
		return this.IsChunkAreaLoaded(Utils.Fastfloor(_position.x), 0, Utils.Fastfloor(_position.z));
	}

	// Token: 0x06005BB9 RID: 23481 RVA: 0x0023A624 File Offset: 0x00238824
	public bool IsChunkAreaLoaded(int _blockPosX, int _, int _blockPosZ)
	{
		int num = World.toChunkXZ(_blockPosX - 8);
		int num2 = World.toChunkXZ(_blockPosZ - 8);
		int num3 = World.toChunkXZ(_blockPosX + 8);
		int num4 = World.toChunkXZ(_blockPosZ + 8);
		for (int i = num; i <= num3; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				if (this.GetChunkSync(i, j) == null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06005BBA RID: 23482 RVA: 0x0023A67D File Offset: 0x0023887D
	public bool IsChunkAreaCollidersLoaded(Vector3 _position)
	{
		return this.IsChunkAreaCollidersLoaded(Utils.Fastfloor(_position.x), Utils.Fastfloor(_position.z));
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x0023A69C File Offset: 0x0023889C
	public bool IsChunkAreaCollidersLoaded(int _blockPosX, int _blockPosZ)
	{
		int num = World.toChunkXZ(_blockPosX - 8);
		int num2 = World.toChunkXZ(_blockPosZ - 8);
		int num3 = World.toChunkXZ(_blockPosX + 8);
		int num4 = World.toChunkXZ(_blockPosZ + 8);
		for (int i = num; i <= num3; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				Chunk chunk = (Chunk)this.GetChunkSync(i, j);
				if (chunk == null || !chunk.IsCollisionMeshGenerated)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x0020785F File Offset: 0x00205A5F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toChunkXZ(int _v)
	{
		return _v >> 4;
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x0023A709 File Offset: 0x00238909
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i toChunkXZ(Vector2i _v)
	{
		return new Vector2i(_v.x >> 4, _v.y >> 4);
	}

	// Token: 0x06005BBE RID: 23486 RVA: 0x0023A720 File Offset: 0x00238920
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i toChunkXZ(Vector3 _v)
	{
		return new Vector2i(Utils.Fastfloor(_v.x) >> 4, Utils.Fastfloor(_v.z) >> 4);
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x0023A741 File Offset: 0x00238941
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i toChunkXZ(Vector3i _v)
	{
		return new Vector2i(_v.x >> 4, _v.z >> 4);
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x0023A758 File Offset: 0x00238958
	public static Vector3i toChunkXYZCube(Vector3 _v)
	{
		return new Vector3i(Utils.Fastfloor(_v.x) >> 4, Utils.Fastfloor(_v.y) >> 4, Utils.Fastfloor(_v.z) >> 4);
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x0023A786 File Offset: 0x00238986
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i toChunkXYZ(Vector3i _v)
	{
		return new Vector3i(_v.x >> 4, _v.y >> 8, _v.z >> 4);
	}

	// Token: 0x06005BC2 RID: 23490 RVA: 0x0023A7A5 File Offset: 0x002389A5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toChunkY(int _v)
	{
		return _v >> 8;
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x0023A7AA File Offset: 0x002389AA
	public static Vector3 toChunkXyzWorldPos(Vector3i _v)
	{
		return new Vector3((float)(_v.x & -16), (float)(_v.y & -256), (float)(_v.z & -16));
	}

	// Token: 0x06005BC4 RID: 23492 RVA: 0x0023A7D2 File Offset: 0x002389D2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i toBlock(Vector3i _p)
	{
		_p.x &= 15;
		_p.y &= 255;
		_p.z &= 15;
		return _p;
	}

	// Token: 0x06005BC5 RID: 23493 RVA: 0x0023A808 File Offset: 0x00238A08
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i toBlock(int _x, int _y, int _z)
	{
		return new Vector3i(_x & 15, _y & 255, _z & 15);
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x00207859 File Offset: 0x00205A59
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toBlockXZ(int _v)
	{
		return _v & 15;
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x0023A81E File Offset: 0x00238A1E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toBlockY(int _v)
	{
		return _v & 255;
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x0023A828 File Offset: 0x00238A28
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 blockToTransformPos(WorldBase _world, BlockValueRef _bvRef)
	{
		Vector3 vector;
		switch (_bvRef.Type)
		{
		case BlockValueRefType.None:
			vector = Vector3.zero;
			break;
		case BlockValueRefType.Block:
			vector = World.blockToTransformPos(_bvRef.BlockPosition);
			break;
		case BlockValueRefType.Prop:
			vector = World.blockToTransformPos(_world, _bvRef.PropReference);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return vector;
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x0023A87C File Offset: 0x00238A7C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 blockToTransformPos(Vector3i _blockPos)
	{
		return new Vector3((float)_blockPos.x + 0.5f, (float)_blockPos.y, (float)_blockPos.z + 0.5f);
	}

	// Token: 0x06005BCA RID: 23498 RVA: 0x0023A8A4 File Offset: 0x00238AA4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 blockToTransformPos(WorldBase _world, PropRef _propRef)
	{
		IChunk chunkSync = _world.GetChunkSync(_propRef.ChunkPos);
		PropValue prop = chunkSync.GetProp(_propRef);
		return chunkSync.GetWorldPos().ToVector3CenterXZ() + prop.transform.position;
	}

	// Token: 0x06005BCB RID: 23499 RVA: 0x0023A8E2 File Offset: 0x00238AE2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i worldToBlockPos(Vector3 _worldPos)
	{
		return new Vector3i(Utils.Fastfloor(_worldPos.x), Utils.Fastfloor(_worldPos.y), Utils.Fastfloor(_worldPos.z));
	}

	// Token: 0x06005BCC RID: 23500 RVA: 0x0023A90A File Offset: 0x00238B0A
	public override void SetBlocksRPC(List<BlockChangeInfo> _blockChangeInfo)
	{
		this.gameManager.SetBlocksRPC(_blockChangeInfo, null);
	}

	// Token: 0x06005BCD RID: 23501 RVA: 0x0023A919 File Offset: 0x00238B19
	public BlockValue SetBlock(Vector3i _blockPos, BlockValue _blockValue, bool bNotify, bool updateLight)
	{
		return this.ChunkCache.SetBlock(_blockPos, _blockValue, bNotify, updateLight);
	}

	// Token: 0x06005BCE RID: 23502 RVA: 0x0023A930 File Offset: 0x00238B30
	public override void SetPropsRPC(List<PropChangeInfo> _changes)
	{
		this.gameManager.SetPropsRPC(_changes, null);
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x0023A93F File Offset: 0x00238B3F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsDaytime()
	{
		return !this.IsDark();
	}

	// Token: 0x06005BD0 RID: 23504 RVA: 0x0023A94C File Offset: 0x00238B4C
	public bool IsDark()
	{
		if (SkyManager.isAllTimeDay)
		{
			return false;
		}
		if (SkyManager.isAllTimeNight)
		{
			return true;
		}
		float num = this.worldTime % 24000UL / 1000f;
		return num < (float)this.DawnHour || num > (float)this.DuskHour;
	}

	// Token: 0x06005BD1 RID: 23505 RVA: 0x0023A998 File Offset: 0x00238B98
	public override TileEntity GetTileEntity(Vector3i _pos)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		Chunk chunk = (Chunk)chunkCache.GetChunkFromWorldPos(_pos);
		if (chunk == null)
		{
			return null;
		}
		Vector3i vector3i = new Vector3i(World.toBlockXZ(_pos.x), World.toBlockY(_pos.y), World.toBlockXZ(_pos.z));
		return chunk.GetTileEntity(vector3i);
	}

	// Token: 0x06005BD2 RID: 23506 RVA: 0x0023A9F4 File Offset: 0x00238BF4
	public void RemoveTileEntity(TileEntity _te)
	{
		Chunk chunk = _te.GetChunk();
		if (chunk != null)
		{
			chunk.RemoveTileEntity(this, _te);
			return;
		}
		Log.Error("RemoveTileEntity: chunk not found!");
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x0023AA20 File Offset: 0x00238C20
	public BlockTrigger GetBlockTrigger(Vector3i _pos)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		Chunk chunk = (Chunk)chunkCache.GetChunkFromWorldPos(_pos);
		if (chunk == null)
		{
			return null;
		}
		Vector3i vector3i = new Vector3i(World.toBlockXZ(_pos.x), World.toBlockY(_pos.y), World.toBlockXZ(_pos.z));
		return chunk.GetBlockTrigger(vector3i);
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x0023AA7C File Offset: 0x00238C7C
	public void OnUpdateTick(float _partialTicks, ArraySegment<long> _activeChunks)
	{
		this.updateChunkAddedRemovedCallbacks();
		this.WorldEventUpdateTime();
		WaterSplashCubes.Update();
		DecoManager.Instance.UpdateTick(this);
		MultiBlockManager.Instance.MainThreadUpdate();
		if (!this.IsEditor())
		{
			this.dmsConductor.Update();
		}
		this.checkPOIUnculling();
		this.updateChunksToUncull();
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		this.worldBlockTicker.Tick(_activeChunks, this.m_LocalPlayerEntity, this.rand);
		if (GameTimer.Instance.ticks % 20UL == 0UL)
		{
			bool @bool = GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
			ChunkCluster chunkCache = this.ChunkCache;
			bool flag = GameTimer.Instance.ticks % 40UL == 0UL;
			for (int i = _activeChunks.Offset; i < _activeChunks.Count; i++)
			{
				long num = _activeChunks.Array[i];
				if ((!flag || num % 2L == 0L) && (flag || num % 2L != 0L))
				{
					Chunk chunk = ((chunkCache != null) ? chunkCache.GetChunkSync(num) : null);
					if (chunk != null)
					{
						if (chunk.NeedsTicking)
						{
							chunk.UpdateTick(this, @bool);
						}
						if (!this.IsEditor() && chunk.IsAreaMaster() && chunk.IsAreaMasterDominantBiomeInitialized(chunkCache))
						{
							ChunkAreaBiomeSpawnData chunkBiomeSpawnData = chunk.GetChunkBiomeSpawnData();
							if (chunkBiomeSpawnData != null && chunkBiomeSpawnData.IsSpawnNeeded(this.Biomes, this.worldTime) && chunk.IsAreaMasterCornerChunksLoaded(chunkCache))
							{
								if (this.areaMasterChunksToLock.ContainsKey(chunk.Key))
								{
									chunk.isModified |= chunkBiomeSpawnData.DelayAllEnemySpawningUntil(this.areaMasterChunksToLock[chunk.Key], this.Biomes);
									this.areaMasterChunksToLock.Remove(chunk.Key);
								}
								else
								{
									this.biomeSpawnManager.Update(string.Empty, @bool, chunkBiomeSpawnData);
								}
							}
						}
					}
				}
			}
		}
		if (GameTimer.Instance.ticks % 16UL == 0UL && GamePrefs.GetString(EnumGamePrefs.DynamicSpawner).Length > 0)
		{
			this.dynamicSpawnManager.Update(GamePrefs.GetString(EnumGamePrefs.DynamicSpawner), GameStats.GetBool(EnumGameStats.IsSpawnEnemies), null);
		}
		this.aiDirector.Tick((double)(_partialTicks / 20f));
		this.TickSleeperVolumes();
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x0023ACA2 File Offset: 0x00238EA2
	public bool UncullPOI(PrefabInstance _pi)
	{
		if (_pi.AddChunksToUncull(this, this.chunksToUncull))
		{
			Log.Out("Unculling POI {0} {1}", new object[]
			{
				_pi.location.Name,
				_pi.boundingBoxPosition
			});
			return true;
		}
		return false;
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x0023ACE2 File Offset: 0x00238EE2
	public void UncullChunk(Chunk _c)
	{
		if (_c.IsInternalBlocksCulled)
		{
			this.chunksToUncull.Add(_c);
		}
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x0023ACF8 File Offset: 0x00238EF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkPOIUnculling()
	{
		if (GameTimer.Instance.ticks % 38UL != 0UL || GameStats.GetInt(EnumGameStats.OptionsPOICulling) == 0)
		{
			return;
		}
		List<EntityPlayer> list = GameManager.Instance.World.Players.list;
		for (int i = 0; i < list.Count; i++)
		{
			EntityPlayer entityPlayer = list[i];
			if (entityPlayer.Spawned)
			{
				Dictionary<int, PrefabInstance> prefabsAroundNear = entityPlayer.GetPrefabsAroundNear();
				if (prefabsAroundNear != null)
				{
					foreach (KeyValuePair<int, PrefabInstance> keyValuePair in prefabsAroundNear)
					{
						PrefabInstance value = keyValuePair.Value;
						if (value.Overlaps(entityPlayer.position, 6f))
						{
							this.UncullPOI(value);
						}
					}
				}
			}
		}
	}

	// Token: 0x06005BD8 RID: 23512 RVA: 0x0023ADC4 File Offset: 0x00238FC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateChunksToUncull()
	{
		if (this.chunksToUncull.list.Count == 0)
		{
			return;
		}
		this.msUnculling.ResetAndRestart();
		this.chunksToRegenerate.Clear();
		for (int i = this.chunksToUncull.list.Count - 1; i >= 0; i--)
		{
			Chunk chunk = this.chunksToUncull.list[i];
			if (chunk.InProgressUnloading)
			{
				this.chunksToUncull.Remove(chunk);
			}
			else
			{
				BlockFaceFlag blockFaceFlag = chunk.RestoreCulledBlocks(this);
				this.chunksToUncull.Remove(chunk);
				if (!this.chunksToRegenerate.hashSet.Contains(chunk))
				{
					this.chunksToRegenerate.Add(chunk);
				}
				Chunk chunk2;
				if ((blockFaceFlag & BlockFaceFlag.West) != BlockFaceFlag.None && (chunk2 = (Chunk)this.GetChunkSync(chunk.X - 1, chunk.Z)) != null && !this.chunksToRegenerate.hashSet.Contains(chunk2))
				{
					this.chunksToRegenerate.Add(chunk2);
				}
				if ((blockFaceFlag & BlockFaceFlag.East) != BlockFaceFlag.None && (chunk2 = (Chunk)this.GetChunkSync(chunk.X + 1, chunk.Z)) != null && !this.chunksToRegenerate.hashSet.Contains(chunk2))
				{
					this.chunksToRegenerate.Add(chunk2);
				}
				if ((blockFaceFlag & BlockFaceFlag.North) != BlockFaceFlag.None && (chunk2 = (Chunk)this.GetChunkSync(chunk.X, chunk.Z + 1)) != null && !this.chunksToRegenerate.hashSet.Contains(chunk2))
				{
					this.chunksToRegenerate.Add(chunk2);
				}
				if ((blockFaceFlag & BlockFaceFlag.South) != BlockFaceFlag.None && (chunk2 = (Chunk)this.GetChunkSync(chunk.X, chunk.Z - 1)) != null && !this.chunksToRegenerate.hashSet.Contains(chunk2))
				{
					this.chunksToRegenerate.Add(chunk2);
				}
				if (this.msUnculling.ElapsedMilliseconds > 5L)
				{
					break;
				}
			}
		}
		for (int j = this.chunksToRegenerate.list.Count - 1; j >= 0; j--)
		{
			this.chunksToRegenerate.list[j].NeedsRegeneration = true;
		}
	}

	// Token: 0x06005BD9 RID: 23513 RVA: 0x0023AFC4 File Offset: 0x002391C4
	public Vector3[] GetRandomSpawnPointPositions(int _count)
	{
		Vector3[] array = new Vector3[_count];
		List<Chunk> chunkArrayCopySync = this.ChunkCache.GetChunkArrayCopySync();
		int count = chunkArrayCopySync.Count;
		while (_count > 0)
		{
			for (int i = 0; i < chunkArrayCopySync.Count; i++)
			{
				Chunk chunk = chunkArrayCopySync[i];
				if (this.GetGameRandom().RandomRange(count) == 1)
				{
					Chunk[] array2 = new Chunk[8];
					if (this.ChunkCache.GetNeighborChunks(chunk, array2))
					{
						int num;
						int num2;
						int num3;
						if (chunk.FindRandomTopSoilPoint(this, out num, out num2, out num3, 5))
						{
							array[array.Length - _count] = new Vector3((float)num, (float)num2, (float)num3);
							_count--;
						}
						if (_count == 0)
						{
							break;
						}
					}
				}
			}
		}
		return array;
	}

	// Token: 0x06005BDA RID: 23514 RVA: 0x0023B068 File Offset: 0x00239268
	public Vector3 ClipBoundsMove(Entity _entity, Bounds _aabb, Vector3 move, Vector3 expandDir, float stepHeight)
	{
		if (stepHeight > 0f)
		{
			move.y = stepHeight;
		}
		Bounds bounds = BoundsUtils.ExpandDirectional(_aabb, expandDir);
		int num = Utils.Fastfloor(bounds.min.x - 0.5f);
		int num2 = Utils.Fastfloor(bounds.max.x + 1.5f);
		int num3 = Utils.Fastfloor(bounds.min.y - 0.5f);
		int num4 = Utils.Fastfloor(bounds.max.y + 1f);
		int num5 = Utils.Fastfloor(bounds.min.z - 0.5f);
		int num6 = Utils.Fastfloor(bounds.max.z + 1.5f);
		World.ClipBlock.ResetStorage();
		int num7 = 0;
		int num8 = 0;
		Chunk chunk = null;
		Vector3 vector = default(Vector3);
		for (int i = num; i < num2; i++)
		{
			vector.x = (float)i;
			int j = num5;
			while (j < num6)
			{
				vector.z = (float)j;
				if (chunk != null && chunk.X == World.toChunkXZ(i) && chunk.Z == World.toChunkXZ(j))
				{
					goto IL_0145;
				}
				chunk = (Chunk)base.GetChunkFromWorldPos(i, j);
				if (chunk != null)
				{
					if (!this.IsInPlayfield(chunk))
					{
						this._clipBounds[num8++] = chunk.GetAABB();
						goto IL_0145;
					}
					goto IL_0145;
				}
				IL_01B0:
				j++;
				continue;
				IL_0145:
				for (int k = num3; k < num4; k++)
				{
					if (k > 0 && k < 256)
					{
						BlockValue block = this.GetBlock(i, k, j);
						Block block2 = block.Block;
						if (block2.IsCollideMovement)
						{
							float num9 = 0f;
							vector.y = (float)k;
							this._clipBlocks[num7++] = World.ClipBlock.New(block, block2, num9, vector, _aabb);
						}
					}
				}
				goto IL_01B0;
			}
		}
		Vector3 min = _aabb.min;
		Vector3 max = _aabb.max;
		if (move.y != 0f && num7 > 0)
		{
			for (int l = 0; l < num7; l++)
			{
				World.ClipBlock clipBlock = this._clipBlocks[l];
				IList<Bounds> clipBoundsList = clipBlock.block.GetClipBoundsList(clipBlock.value, clipBlock.pos);
				move.y = BoundsUtils.ClipBoundsMoveY(clipBlock.bmins, clipBlock.bmaxs, move.y, clipBoundsList, clipBoundsList.Count);
				if (move.y == 0f)
				{
					break;
				}
			}
		}
		if (move.y != 0f)
		{
			if (num8 > 0)
			{
				move.y = BoundsUtils.ClipBoundsMoveY(min, max, move.y, this._clipBounds, num8);
			}
			min.y += move.y;
			max.y += move.y;
			for (int m = 0; m < num7; m++)
			{
				World.ClipBlock clipBlock2 = this._clipBlocks[m];
				clipBlock2.bmins.y = clipBlock2.bmins.y + move.y;
				clipBlock2.bmaxs.y = clipBlock2.bmaxs.y + move.y;
			}
		}
		if (move.x != 0f && num7 > 0)
		{
			for (int n = 0; n < num7; n++)
			{
				World.ClipBlock clipBlock3 = this._clipBlocks[n];
				IList<Bounds> clipBoundsList2 = clipBlock3.block.GetClipBoundsList(clipBlock3.value, clipBlock3.pos);
				move.x = BoundsUtils.ClipBoundsMoveX(clipBlock3.bmins, clipBlock3.bmaxs, move.x, clipBoundsList2, clipBoundsList2.Count);
				if (move.x == 0f)
				{
					break;
				}
			}
		}
		if (move.x != 0f)
		{
			if (num8 > 0)
			{
				move.x = BoundsUtils.ClipBoundsMoveX(min, max, move.x, this._clipBounds, num8);
			}
			min.x += move.x;
			max.x += move.x;
			for (int num10 = 0; num10 < num7; num10++)
			{
				World.ClipBlock clipBlock4 = this._clipBlocks[num10];
				clipBlock4.bmins.x = clipBlock4.bmins.x + move.x;
				clipBlock4.bmaxs.x = clipBlock4.bmaxs.x + move.x;
			}
		}
		if (move.z != 0f && num7 > 0)
		{
			for (int num11 = 0; num11 < num7; num11++)
			{
				World.ClipBlock clipBlock5 = this._clipBlocks[num11];
				IList<Bounds> clipBoundsList3 = clipBlock5.block.GetClipBoundsList(clipBlock5.value, clipBlock5.pos);
				move.z = BoundsUtils.ClipBoundsMoveZ(clipBlock5.bmins, clipBlock5.bmaxs, move.z, clipBoundsList3, clipBoundsList3.Count);
				if (move.z == 0f)
				{
					break;
				}
			}
		}
		if (move.z != 0f)
		{
			if (num8 > 0)
			{
				move.z = BoundsUtils.ClipBoundsMoveZ(min, max, move.z, this._clipBounds, num8);
			}
			min.z += move.z;
			max.z += move.z;
			for (int num12 = 0; num12 < num7; num12++)
			{
				World.ClipBlock clipBlock6 = this._clipBlocks[num12];
				clipBlock6.bmins.z = clipBlock6.bmins.z + move.z;
				clipBlock6.bmaxs.z = clipBlock6.bmaxs.z + move.z;
			}
		}
		if (stepHeight > 0f)
		{
			stepHeight = -stepHeight;
			if (num7 > 0)
			{
				for (int num13 = 0; num13 < num7; num13++)
				{
					World.ClipBlock clipBlock7 = this._clipBlocks[num13];
					IList<Bounds> clipBoundsList4 = clipBlock7.block.GetClipBoundsList(clipBlock7.value, clipBlock7.pos);
					stepHeight = BoundsUtils.ClipBoundsMoveY(clipBlock7.bmins, clipBlock7.bmaxs, stepHeight, clipBoundsList4, clipBoundsList4.Count);
					if (stepHeight == 0f)
					{
						break;
					}
				}
			}
			if (stepHeight != 0f && num8 > 0)
			{
				stepHeight = BoundsUtils.ClipBoundsMoveY(min, max, stepHeight, this._clipBounds, num8);
			}
			move.y += stepHeight;
		}
		return move;
	}

	// Token: 0x06005BDB RID: 23515 RVA: 0x0023B644 File Offset: 0x00239844
	public List<Bounds> GetCollidingBounds(Entity _entity, Bounds _aabb, List<Bounds> collidingBoundingBoxes)
	{
		int num = Utils.Fastfloor(_aabb.min.x - 0.5f);
		int num2 = Utils.Fastfloor(_aabb.max.x + 0.5f);
		int num3 = Utils.Fastfloor(_aabb.min.y - 1f);
		int num4 = Utils.Fastfloor(_aabb.max.y + 1f);
		int num5 = Utils.Fastfloor(_aabb.min.z - 0.5f);
		int num6 = Utils.Fastfloor(_aabb.max.z + 0.5f);
		Chunk chunk = null;
		int i = num - 1;
		int num7 = 0;
		while (i <= num2 + 1)
		{
			if (num7 >= 50)
			{
				Log.Warning(string.Format("1BB exceeded size {0}: BB={1}", 50, _aabb.ToCultureInvariantString()));
				return collidingBoundingBoxes;
			}
			int j = num5 - 1;
			int num8 = 0;
			while (j <= num6 + 1)
			{
				if (num8 >= 50)
				{
					Log.Warning(string.Format("2BB exceeded size {0}: BB={1}", 50, _aabb.ToCultureInvariantString()));
					return collidingBoundingBoxes;
				}
				if (chunk != null && chunk.X == World.toChunkXZ(i) && chunk.Z == World.toChunkXZ(j))
				{
					goto IL_014B;
				}
				chunk = (Chunk)base.GetChunkFromWorldPos(i, j);
				if (chunk != null)
				{
					if (!this.IsInPlayfield(chunk))
					{
						collidingBoundingBoxes.Add(chunk.GetAABB());
						goto IL_014B;
					}
					goto IL_014B;
				}
				IL_01EB:
				j++;
				num8++;
				continue;
				IL_014B:
				int num9 = World.toBlockXZ(i);
				int num10 = World.toBlockXZ(j);
				int k = num3;
				int num11 = 0;
				while (k < num4)
				{
					if (k > 0 && k < 255)
					{
						BlockValue block = chunk.GetBlock(num9, k, num10);
						if (num11 >= 50)
						{
							Log.Warning(string.Format("3BB exceeded size {0}: BB={1}", 50, _aabb.ToCultureInvariantString()));
							return collidingBoundingBoxes;
						}
						this.collBlockCache[num7, num11, num8] = block;
						this.collDensityCache[num7, num11, num8] = chunk.GetDensity(num9, k, num10);
					}
					k++;
					num11++;
				}
				goto IL_01EB;
			}
			i++;
			num7++;
		}
		int l = num;
		int num12 = 0;
		while (l <= num2)
		{
			if (num12 >= 50)
			{
				Log.Warning(string.Format("4BB exceeded size {0}: BB={1}", 50, _aabb.ToCultureInvariantString()));
				return collidingBoundingBoxes;
			}
			int m = num5;
			int num13 = 0;
			while (m <= num6)
			{
				if (num13 >= 50)
				{
					Log.Warning(string.Format("5BB exceeded size {0}: BB={1}", 50, _aabb.ToCultureInvariantString()));
					return collidingBoundingBoxes;
				}
				int n = num3;
				int num14 = 0;
				while (n < num4)
				{
					if (n > 0 && n < 255)
					{
						if (num14 >= 50)
						{
							Log.Warning(string.Format("6BB exceeded size {0}: BB={1}", 50, _aabb.ToCultureInvariantString()));
							return collidingBoundingBoxes;
						}
						BlockValue blockValue = this.collBlockCache[num12 + 1, num14, num13 + 1];
						Block block2 = blockValue.Block;
						if (block2.IsCollideMovement)
						{
							float num15 = 0f;
							if (block2.shape.IsTerrain())
							{
								num15 = MarchingCubes.GetDecorationOffsetY(this.collDensityCache[num12 + 1, num14 + 1, num13 + 1], this.collDensityCache[num12 + 1, num14, num13 + 1]);
							}
							block2.GetCollidingAABB(blockValue, l, n, m, num15, _aabb, collidingBoundingBoxes);
						}
					}
					n++;
					num14++;
				}
				m++;
				num13++;
			}
			l++;
			num12++;
		}
		Bounds bounds = _aabb;
		bounds.Expand(0.25f);
		List<Entity> entitiesInBounds = this.GetEntitiesInBounds(_entity, bounds);
		for (int num16 = 0; num16 < entitiesInBounds.Count; num16++)
		{
			Bounds bounds2 = entitiesInBounds[num16].getBoundingBox();
			if (bounds2.Intersects(_aabb))
			{
				collidingBoundingBoxes.Add(bounds2);
			}
			bounds2 = _entity.getBoundingBox();
			if (bounds2.Intersects(_aabb))
			{
				collidingBoundingBoxes.Add(bounds2);
			}
		}
		return collidingBoundingBoxes;
	}

	// Token: 0x06005BDC RID: 23516 RVA: 0x0023BA3C File Offset: 0x00239C3C
	public List<Entity> GetEntitiesInBounds(Entity _excludeEntity, Bounds _aabbOfEntity)
	{
		this.entitiesWithinAABBExcludingEntity.Clear();
		int num = Utils.Fastfloor((_aabbOfEntity.min.x - 5f) / 16f);
		int num2 = Utils.Fastfloor((_aabbOfEntity.max.x + 5f) / 16f);
		int num3 = Utils.Fastfloor((_aabbOfEntity.min.z - 5f) / 16f);
		int num4 = Utils.Fastfloor((_aabbOfEntity.max.z + 5f) / 16f);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunkSync = this.ChunkCache.GetChunkSync(i, j);
				if (chunkSync != null)
				{
					chunkSync.GetEntitiesInBounds(_excludeEntity, _aabbOfEntity, this.entitiesWithinAABBExcludingEntity, true);
				}
			}
		}
		return this.entitiesWithinAABBExcludingEntity;
	}

	// Token: 0x06005BDD RID: 23517 RVA: 0x0023BB10 File Offset: 0x00239D10
	public List<Entity> GetEntitiesInBounds(Entity _excludeEntity, Bounds _aabbOfEntity, bool _isAlive)
	{
		this.entitiesWithinAABBExcludingEntity.Clear();
		int num = Utils.Fastfloor((_aabbOfEntity.min.x - 5f) / 16f);
		int num2 = Utils.Fastfloor((_aabbOfEntity.max.x + 5f) / 16f);
		int num3 = Utils.Fastfloor((_aabbOfEntity.min.z - 5f) / 16f);
		int num4 = Utils.Fastfloor((_aabbOfEntity.max.z + 5f) / 16f);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunkSync = this.ChunkCache.GetChunkSync(i, j);
				if (chunkSync != null)
				{
					chunkSync.GetEntitiesInBounds(_excludeEntity, _aabbOfEntity, this.entitiesWithinAABBExcludingEntity, _isAlive);
				}
			}
		}
		return this.entitiesWithinAABBExcludingEntity;
	}

	// Token: 0x06005BDE RID: 23518 RVA: 0x0023BBE4 File Offset: 0x00239DE4
	public List<EntityAlive> GetLivingEntitiesInBounds(EntityAlive _excludeEntity, Bounds _aabbOfEntity)
	{
		this.livingEntitiesWithinAABBExcludingEntity.Clear();
		int num = Utils.Fastfloor((_aabbOfEntity.min.x - 5f) / 16f);
		int num2 = Utils.Fastfloor((_aabbOfEntity.max.x + 5f) / 16f);
		int num3 = Utils.Fastfloor((_aabbOfEntity.min.z - 5f) / 16f);
		int num4 = Utils.Fastfloor((_aabbOfEntity.max.z + 5f) / 16f);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Chunk chunkSync = this.ChunkCache.GetChunkSync(i, j);
				if (chunkSync != null)
				{
					chunkSync.GetLivingEntitiesInBounds(_excludeEntity, _aabbOfEntity, this.livingEntitiesWithinAABBExcludingEntity);
				}
			}
		}
		return this.livingEntitiesWithinAABBExcludingEntity;
	}

	// Token: 0x06005BDF RID: 23519 RVA: 0x0023BCB8 File Offset: 0x00239EB8
	public void GetEntitiesInBounds(FastTags<TagGroup.Global> _tags, Bounds _bb, List<Entity> _list)
	{
		int num = Utils.Fastfloor((_bb.min.x - 5f) / 16f);
		int num2 = Utils.Fastfloor((_bb.max.x + 5f) / 16f);
		int num3 = Utils.Fastfloor((_bb.min.z - 5f) / 16f);
		int num4 = Utils.Fastfloor((_bb.max.z + 5f) / 16f);
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				Chunk chunk = (Chunk)this.GetChunkSync(j, i);
				if (chunk != null)
				{
					chunk.GetEntitiesInBounds(_tags, _bb, _list);
				}
			}
		}
	}

	// Token: 0x06005BE0 RID: 23520 RVA: 0x0023BD78 File Offset: 0x00239F78
	public List<Entity> GetEntitiesInBounds(Type _class, Bounds _bb, List<Entity> _list)
	{
		int num = Utils.Fastfloor((_bb.min.x - 5f) / 16f);
		int num2 = Utils.Fastfloor((_bb.max.x + 5f) / 16f);
		int num3 = Utils.Fastfloor((_bb.min.z - 5f) / 16f);
		int num4 = Utils.Fastfloor((_bb.max.z + 5f) / 16f);
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				Chunk chunk = (Chunk)this.GetChunkSync(j, i);
				if (chunk != null)
				{
					chunk.GetEntitiesInBounds(_class, _bb, _list);
				}
			}
		}
		return _list;
	}

	// Token: 0x06005BE1 RID: 23521 RVA: 0x0023BE38 File Offset: 0x0023A038
	public void GetEntitiesAround(EntityFlags _mask, Vector3 _pos, float _radius, List<Entity> _list)
	{
		int num = Utils.Fastfloor((_pos.x - _radius) / 16f);
		int num2 = Utils.Fastfloor((_pos.x + _radius) / 16f);
		int num3 = Utils.Fastfloor((_pos.z - _radius) / 16f);
		int num4 = Utils.Fastfloor((_pos.z + _radius) / 16f);
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				Chunk chunk = (Chunk)this.GetChunkSync(j, i);
				if (chunk != null)
				{
					chunk.GetEntitiesAround(_mask, _pos, _radius, _list);
				}
			}
		}
	}

	// Token: 0x06005BE2 RID: 23522 RVA: 0x0023BED0 File Offset: 0x0023A0D0
	public void GetEntitiesAround(EntityFlags _flags, EntityFlags _mask, Vector3 _pos, float _radius, List<Entity> _list)
	{
		_flags &= _mask;
		int num = Utils.Fastfloor((_pos.x - _radius) / 16f);
		int num2 = Utils.Fastfloor((_pos.x + _radius) / 16f);
		int num3 = Utils.Fastfloor((_pos.z - _radius) / 16f);
		int num4 = Utils.Fastfloor((_pos.z + _radius) / 16f);
		for (int i = num3; i <= num4; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				Chunk chunk = (Chunk)this.GetChunkSync(j, i);
				if (chunk != null)
				{
					chunk.GetEntitiesAround(_flags, _mask, _pos, _radius, _list);
				}
			}
		}
	}

	// Token: 0x06005BE3 RID: 23523 RVA: 0x0023BF74 File Offset: 0x0023A174
	public int GetEntityAliveCount(EntityFlags _flags, EntityFlags _mask)
	{
		int num = 0;
		int count = this.EntityAlives.Count;
		for (int i = 0; i < count; i++)
		{
			if ((this.EntityAlives[i].entityFlags & _mask) == _flags)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06005BE4 RID: 23524 RVA: 0x0023BFB8 File Offset: 0x0023A1B8
	public void GetPlayersAround(Vector3 _pos, float _radius, List<EntityPlayer> _list)
	{
		float num = _radius * _radius;
		for (int i = this.Players.list.Count - 1; i >= 0; i--)
		{
			EntityPlayer entityPlayer = this.Players.list[i];
			if ((entityPlayer.position - _pos).sqrMagnitude <= num)
			{
				_list.Add(entityPlayer);
			}
		}
	}

	// Token: 0x06005BE5 RID: 23525 RVA: 0x0023C018 File Offset: 0x0023A218
	public void SetEntitiesVisibleNearToLocalPlayer()
	{
		EntityPlayerLocal primaryPlayer = this.GetPrimaryPlayer();
		if (primaryPlayer == null)
		{
			return;
		}
		bool aimingGun = primaryPlayer.AimingGun;
		Vector3 vector = primaryPlayer.cameraTransform.position + Origin.position;
		for (int i = this.Entities.list.Count - 1; i >= 0; i--)
		{
			Entity entity = this.Entities.list[i];
			if (entity != primaryPlayer)
			{
				entity.VisiblityCheck((entity.position - vector).sqrMagnitude, aimingGun);
			}
		}
	}

	// Token: 0x06005BE6 RID: 23526 RVA: 0x0023C0AC File Offset: 0x0023A2AC
	public void TickEntities(float _partialTicks)
	{
		int frameCount = Time.frameCount;
		int num = frameCount - this.tickEntityFrameCount;
		if (num <= 0)
		{
			num = 1;
		}
		this.tickEntityFrameCount = frameCount;
		this.tickEntityFrameCountAverage = this.tickEntityFrameCountAverage * 0.8f + (float)num * 0.2f;
		this.tickEntityPartialTicks = _partialTicks;
		this.tickEntityIndex = 0;
		this.tickEntityList.Clear();
		Entity primaryPlayer = this.GetPrimaryPlayer();
		int count = this.Entities.list.Count;
		for (int i = 0; i < count; i++)
		{
			Entity entity = this.Entities.list[i];
			if (entity != primaryPlayer)
			{
				this.tickEntityList.Add(entity);
			}
		}
		if (primaryPlayer)
		{
			this.TickEntity(primaryPlayer, _partialTicks);
		}
		this.EntityActivityUpdate();
		int num2 = (int)(this.tickEntityFrameCountAverage + 0.4f) - 1;
		if (num2 <= 0)
		{
			this.TickEntitiesFlush();
			return;
		}
		int num3 = (this.tickEntityList.Count - 25) / (num2 + 1);
		if (num3 < 0)
		{
			num3 = 0;
		}
		this.tickEntitySliceCount = (this.tickEntityList.Count - num3) / num2 + 1;
	}

	// Token: 0x06005BE7 RID: 23527 RVA: 0x0023C1C5 File Offset: 0x0023A3C5
	public void TickEntitiesFlush()
	{
		this.TickEntitiesSlice(this.tickEntityList.Count);
	}

	// Token: 0x06005BE8 RID: 23528 RVA: 0x0023C1D8 File Offset: 0x0023A3D8
	public void TickEntitiesSlice()
	{
		this.TickEntitiesSlice(this.tickEntitySliceCount);
	}

	// Token: 0x06005BE9 RID: 23529 RVA: 0x0023C1E8 File Offset: 0x0023A3E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickEntitiesSlice(int count)
	{
		int num = Utils.FastMin(this.tickEntityIndex + count, this.tickEntityList.Count);
		for (int i = this.tickEntityIndex; i < num; i++)
		{
			Entity entity = this.tickEntityList[i];
			if (entity)
			{
				this.TickEntity(entity, this.tickEntityPartialTicks);
			}
		}
		this.tickEntityIndex = num;
	}

	// Token: 0x06005BEA RID: 23530 RVA: 0x0023C248 File Offset: 0x0023A448
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickEntity(Entity e, float _partialTicks)
	{
		e.SetLastTickPos(e.position);
		e.OnUpdatePosition(_partialTicks);
		e.CheckPosition();
		if (e.IsSpawned() && !e.IsMarkedForUnload())
		{
			Chunk chunk = (Chunk)this.GetChunkSync(e.chunkPosAddedEntityTo.x, e.chunkPosAddedEntityTo.z);
			bool flag = false;
			if (chunk != null)
			{
				if (!chunk.hasEntities)
				{
					flag = true;
				}
				else
				{
					chunk.AdJustEntityTracking(e);
				}
			}
			int num = World.toChunkXZ(Utils.Fastfloor(e.position.x));
			int num2 = World.toChunkXZ(Utils.Fastfloor(e.position.z));
			if (flag || !e.addedToChunk || e.chunkPosAddedEntityTo.x != num || e.chunkPosAddedEntityTo.z != num2)
			{
				if (e.addedToChunk && chunk != null)
				{
					chunk.RemoveEntityFromChunk(e);
				}
				chunk = (Chunk)this.GetChunkSync(num, num2);
				if (chunk != null)
				{
					e.addedToChunk = true;
					chunk.AddEntityToChunk(e);
				}
				else
				{
					e.addedToChunk = false;
				}
			}
			if (e is EntityPlayer || e is EntityDrone || this.IsChunkAreaLoaded(e.position))
			{
				if (e.CanUpdateEntity())
				{
					e.OnUpdateEntity();
				}
				else if (e is EntityAlive)
				{
					((EntityAlive)e).CheckDespawn();
				}
			}
			else
			{
				EntityAlive entityAlive = e as EntityAlive;
				if (entityAlive != null)
				{
					entityAlive.SetAttackTarget(null, 0);
					entityAlive.CheckDespawn();
				}
			}
		}
		if (e.IsMarkedForUnload() && !e.isEntityRemote && !e.bWillRespawn)
		{
			this.unloadEntity(e, e.IsDespawned ? EnumRemoveEntityReason.Despawned : EnumRemoveEntityReason.Killed);
		}
	}

	// Token: 0x06005BEB RID: 23531 RVA: 0x0023C3E0 File Offset: 0x0023A5E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickEntityRemove(Entity e)
	{
		int num = this.tickEntityList.IndexOf(e);
		if (num >= this.tickEntityIndex)
		{
			this.tickEntityList[num] = null;
		}
	}

	// Token: 0x06005BEC RID: 23532 RVA: 0x0023C410 File Offset: 0x0023A610
	public void EntityActivityUpdate()
	{
		List<EntityPlayer> list = this.Players.list;
		if (list.Count == 0)
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			list[i].aiClosest.Clear();
		}
		int count = this.EntityAlives.Count;
		for (int j = 0; j < count; j++)
		{
			EntityAlive entityAlive = this.EntityAlives[j];
			EntityPlayer closestPlayer = this.GetClosestPlayer(entityAlive.position, -1f, false);
			if (closestPlayer)
			{
				closestPlayer.aiClosest.Add(entityAlive);
				entityAlive.aiClosestPlayer = closestPlayer;
				entityAlive.aiClosestPlayerDistSq = (closestPlayer.position - entityAlive.position).sqrMagnitude;
			}
			else
			{
				entityAlive.aiClosestPlayer = null;
				entityAlive.aiClosestPlayerDistSq = float.MaxValue;
			}
		}
		Vector3 vector = Vector3.zero;
		float num = 0f;
		if (this.m_LocalPlayerEntity)
		{
			vector = this.m_LocalPlayerEntity.cameraTransform.position + Origin.position;
			this.m_LocalPlayerEntity.emodel.ClothSimOn(!this.m_LocalPlayerEntity.AttachedToEntity, false);
			num = 625f;
			if (this.m_LocalPlayerEntity.AimingGun)
			{
				num = 3025f;
			}
		}
		int num2 = Utils.FastClamp(60 / list.Count, 4, 20);
		for (int k = list.Count - 1; k >= 0; k--)
		{
			EntityPlayer entityPlayer = list[k];
			entityPlayer.aiClosest.Sort((EntityAlive e1, EntityAlive e2) => e1.aiClosestPlayerDistSq.CompareTo(e2.aiClosestPlayerDistSq));
			for (int l = 0; l < entityPlayer.aiClosest.Count; l++)
			{
				EntityAlive entityAlive2 = entityPlayer.aiClosest[l];
				if (l < num2 || entityAlive2.aiClosestPlayerDistSq < 64f)
				{
					entityAlive2.aiActiveScale = 1f;
					bool flag = entityAlive2.aiClosestPlayerDistSq < 36f;
					entityAlive2.emodel.JiggleOn(flag);
				}
				else
				{
					float num3 = ((entityAlive2.aiClosestPlayerDistSq < 225f) ? 0.3f : 0.1f);
					entityAlive2.aiActiveScale = num3;
					entityAlive2.emodel.JiggleOn(false);
				}
			}
			if (entityPlayer != this.m_LocalPlayerEntity)
			{
				bool flag2 = !entityPlayer.AttachedToEntity && (entityPlayer.position - vector).sqrMagnitude < num;
				entityPlayer.emodel.ClothSimOn(flag2, false);
			}
		}
	}

	// Token: 0x06005BED RID: 23533 RVA: 0x0023C6C0 File Offset: 0x0023A8C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void addToChunk(Entity e)
	{
		if (!e.addedToChunk)
		{
			Chunk chunk = (Chunk)base.GetChunkFromWorldPos(e.GetBlockPosition());
			if (chunk != null)
			{
				chunk.AddEntityToChunk(e);
			}
		}
	}

	// Token: 0x06005BEE RID: 23534 RVA: 0x0023C6F4 File Offset: 0x0023A8F4
	public override void UnloadEntities(List<Entity> _entityList, bool _forceUnload = false)
	{
		for (int i = _entityList.Count - 1; i >= 0; i--)
		{
			Entity entity = _entityList[i];
			if (_forceUnload || (!entity.bWillRespawn && (!(entity.AttachedMainEntity != null) || !entity.AttachedMainEntity.bWillRespawn)))
			{
				this.unloadEntity(entity, EnumRemoveEntityReason.Unloaded);
			}
		}
	}

	// Token: 0x06005BEF RID: 23535 RVA: 0x0023C74C File Offset: 0x0023A94C
	public override Entity RemoveEntity(int _entityId, EnumRemoveEntityReason _reason)
	{
		Entity entity = this.GetEntity(_entityId);
		if (entity != null)
		{
			entity.MarkToUnload();
			this.unloadEntity(entity, _reason);
		}
		return entity;
	}

	// Token: 0x06005BF0 RID: 23536 RVA: 0x0023C77C File Offset: 0x0023A97C
	public void unloadEntity(Entity _e, EnumRemoveEntityReason _reason)
	{
		EnumRemoveEntityReason unloadReason = _e.unloadReason;
		_e.unloadReason = _reason;
		if (!this.Entities.dict.ContainsKey(_e.entityId))
		{
			Log.Warning("{0} World unloadEntity !dict {1}, {2}, was {3}", new object[]
			{
				GameManager.frameCount,
				_e,
				_reason,
				unloadReason
			});
			return;
		}
		if (this.EntityUnloadedDelegates != null)
		{
			this.EntityUnloadedDelegates(_e, _reason);
		}
		if (_e.NavObject != null)
		{
			if (_reason == EnumRemoveEntityReason.Unloaded && _e is EntitySupplyCrate)
			{
				_e.NavObject.TrackedPosition = _e.position;
			}
			else
			{
				NavObjectManager.Instance.UnRegisterNavObject(_e.NavObject);
			}
		}
		_e.OnEntityUnload();
		this.Entities.Remove(_e.entityId);
		this.TickEntityRemove(_e);
		EntityAlive entityAlive = _e as EntityAlive;
		if (entityAlive)
		{
			this.EntityAlives.Remove(entityAlive);
		}
		this.RemoveEntityFromMap(_e, _reason);
		if (_e.addedToChunk && _e.IsMarkedForUnload())
		{
			Chunk chunk = (Chunk)this.GetChunkSync(_e.chunkPosAddedEntityTo.x, _e.chunkPosAddedEntityTo.z);
			if (chunk != null && !chunk.InProgressUnloading)
			{
				chunk.RemoveEntityFromChunk(_e);
			}
		}
		if (!this.IsRemote())
		{
			if (VehicleManager.Instance != null)
			{
				EntityVehicle entityVehicle = _e as EntityVehicle;
				if (entityVehicle)
				{
					VehicleManager.Instance.RemoveTrackedVehicle(entityVehicle, _reason);
				}
			}
			if (DroneManager.Instance != null)
			{
				EntityDrone entityDrone = _e as EntityDrone;
				if (entityDrone)
				{
					DroneManager.Instance.RemoveTrackedDrone(entityDrone, _reason);
				}
			}
			if (TurretTracker.Instance != null)
			{
				EntityTurret entityTurret = _e as EntityTurret;
				if (entityTurret)
				{
					TurretTracker.Instance.RemoveTrackedTurret(entityTurret, _reason);
				}
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.entityDistributer.Remove(_e, _reason);
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && _e is EntityAlive && PathFinderThread.Instance != null)
		{
			PathFinderThread.Instance.RemovePathsFor(_e.entityId);
		}
		if (_e is EntityPlayer)
		{
			this.Players.Remove(_e.entityId);
			this.gameManager.HandlePersistentPlayerDisconnected(_e.entityId);
			this.playerEntityUpdateCount++;
			NavObjectManager.Instance.UnRegisterNavObjectByOwnerEntity(_e, "sleeping_bag");
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.aiDirector.RemoveEntity(_e);
		}
		this.audioManager.EntityRemovedFromWorld(_e, this);
		WeatherManager.EntityRemovedFromWorld(_e);
		LightManager.EntityRemovedFromWorld(_e, this);
	}

	// Token: 0x06005BF1 RID: 23537 RVA: 0x0023C9EC File Offset: 0x0023ABEC
	public override Entity GetEntity(int _entityId)
	{
		EntityAsyncManager entityAsyncManager = this.entityAsyncManager;
		if (entityAsyncManager != null)
		{
			entityAsyncManager.EnsureEntity(_entityId);
		}
		Entity entity;
		this.Entities.dict.TryGetValue(_entityId, out entity);
		return entity;
	}

	// Token: 0x06005BF2 RID: 23538 RVA: 0x0023CA20 File Offset: 0x0023AC20
	public override void ChangeClientEntityIdToServer(int _clientEntityId, int _serverEntityId)
	{
		Entity entity = this.GetEntity(_clientEntityId);
		if (entity)
		{
			this.Entities.Remove(_clientEntityId);
			entity.entityId = _serverEntityId;
			entity.clientEntityId = 0;
			this.Entities.Add(_serverEntityId, entity);
		}
	}

	// Token: 0x06005BF3 RID: 23539 RVA: 0x0023CA68 File Offset: 0x0023AC68
	public void SpawnEntityInWorld(Entity _entity)
	{
		if (_entity == null)
		{
			Log.Warning("Ignore spawning of empty entity");
			return;
		}
		if (this.EntityLoadedDelegates != null)
		{
			this.EntityLoadedDelegates(_entity);
		}
		this.AddEntityToMap(_entity);
		this.Entities.Add(_entity.entityId, _entity);
		this.addToChunk(_entity);
		EntityPlayer entityPlayer = _entity as EntityPlayer;
		EntityAlive entityAlive = ((!entityPlayer) ? (_entity as EntityAlive) : null);
		if (entityAlive)
		{
			this.EntityAlives.Add(entityAlive);
		}
		if (!this.IsRemote())
		{
			EntityVehicle entityVehicle = _entity as EntityVehicle;
			if (entityVehicle != null && VehicleManager.Instance != null)
			{
				VehicleManager.Instance.AddTrackedVehicle(entityVehicle);
			}
			EntityDrone entityDrone = _entity as EntityDrone;
			if (entityDrone != null && DroneManager.Instance != null)
			{
				DroneManager.Instance.AddTrackedDrone(entityDrone);
			}
			EntityTurret entityTurret = _entity as EntityTurret;
			if (entityTurret != null)
			{
				if (TurretTracker.Instance != null)
				{
					TurretTracker.Instance.AddTrackedTurret(entityTurret);
				}
				if (entityTurret.OriginalItemValue.ItemClass == null)
				{
					entityTurret.InitDynamicSpawn();
				}
			}
		}
		if (this.audioManager != null)
		{
			this.audioManager.EntityAddedToWorld(_entity, this);
		}
		WeatherManager.EntityAddedToWorld(_entity);
		LightManager.EntityAddedToWorld(_entity, this);
		_entity.OnAddedToWorld();
		if (_entity.position.y < 1f)
		{
			Log.Warning(string.Concat(new string[]
			{
				"Spawned entity with wrong pos: ",
				(_entity != null) ? _entity.ToString() : null,
				" id=",
				_entity.entityId.ToString(),
				" pos=",
				_entity.position.ToCultureInvariantString()
			}));
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.entityDistributer.Add(_entity);
		}
		if (entityPlayer)
		{
			this.Players.Add(_entity.entityId, entityPlayer);
			this.playerEntityUpdateCount++;
		}
		else if (entityAlive)
		{
			entityAlive.Spawned = true;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.aiDirector.AddEntity(_entity);
		}
	}

	// Token: 0x06005BF4 RID: 23540 RVA: 0x0023CC58 File Offset: 0x0023AE58
	public void AddEntityToMap(Entity _entity)
	{
		if (_entity == null)
		{
			return;
		}
		if (_entity.HasUIIcon() && _entity.GetMapObjectType() == EnumMapObjectType.Entity)
		{
			EntityVehicle entityVehicle = _entity as EntityVehicle;
			if (entityVehicle != null)
			{
				EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				if (primaryPlayer != null)
				{
					LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(primaryPlayer);
					if (uiforPlayer != null && uiforPlayer.xui != null && uiforPlayer.xui.GetWindow("mapArea") != null)
					{
						((XUiC_MapArea)uiforPlayer.xui.GetWindow("mapArea").Controller).RefreshVehiclePositionWaypoint(entityVehicle, false);
						return;
					}
				}
			}
			else
			{
				EntityDrone entityDrone = _entity as EntityDrone;
				if (entityDrone != null)
				{
					EntityPlayerLocal primaryPlayer2 = GameManager.Instance.World.GetPrimaryPlayer();
					if (primaryPlayer2 != null)
					{
						LocalPlayerUI uiforPlayer2 = LocalPlayerUI.GetUIForPlayer(primaryPlayer2);
						if (uiforPlayer2 != null && uiforPlayer2.xui != null && uiforPlayer2.xui.GetWindow("mapArea") != null)
						{
							((XUiC_MapArea)uiforPlayer2.xui.GetWindow("mapArea").Controller).RefreshDronePositionWaypoint(entityDrone, false);
							return;
						}
					}
				}
				else
				{
					if (_entity is EntityEnemy || _entity is EntityEnemyAnimal)
					{
						this.ObjectOnMapAdd(new MapObjectZombie(_entity));
						return;
					}
					if (_entity is EntityAnimal)
					{
						this.ObjectOnMapAdd(new MapObjectAnimal(_entity));
						return;
					}
					this.ObjectOnMapAdd(new MapObject(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false));
				}
			}
		}
	}

	// Token: 0x06005BF5 RID: 23541 RVA: 0x0023CDD8 File Offset: 0x0023AFD8
	public void RemoveEntityFromMap(Entity _entity, EnumRemoveEntityReason _reason)
	{
		if (_entity == null)
		{
			return;
		}
		EnumMapObjectType mapObjectType = _entity.GetMapObjectType();
		if (mapObjectType == EnumMapObjectType.SupplyDrop)
		{
			if (_reason == EnumRemoveEntityReason.Killed)
			{
				this.ObjectOnMapRemove(_entity.GetMapObjectType(), _entity.entityId);
				return;
			}
		}
		else
		{
			EntityPlayerLocal primaryPlayer = this.GetPrimaryPlayer();
			XUiC_MapArea xuiC_MapArea = null;
			if (primaryPlayer != null)
			{
				LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(primaryPlayer);
				if (uiforPlayer != null)
				{
					XUi xui = uiforPlayer.xui;
					object obj;
					if (xui == null)
					{
						obj = null;
					}
					else
					{
						XUiV_Window window = xui.GetWindow("mapArea");
						obj = ((window != null) ? window.Controller : null);
					}
					xuiC_MapArea = (XUiC_MapArea)obj;
				}
			}
			EntityVehicle entityVehicle = _entity as EntityVehicle;
			if (entityVehicle != null)
			{
				if (_reason == EnumRemoveEntityReason.Unloaded)
				{
					if (entityVehicle.GetOwner() != null && entityVehicle.LocalPlayerIsOwner() && xuiC_MapArea != null)
					{
						xuiC_MapArea.RefreshVehiclePositionWaypoint(entityVehicle, true);
					}
				}
				else if ((_reason == EnumRemoveEntityReason.Killed || _reason == EnumRemoveEntityReason.Despawned) && xuiC_MapArea != null)
				{
					xuiC_MapArea.RemoveVehicleLastKnownWaypoint(entityVehicle);
				}
			}
			else
			{
				EntityDrone entityDrone = _entity as EntityDrone;
				if (entityDrone != null)
				{
					if (_reason == EnumRemoveEntityReason.Unloaded)
					{
						if (entityDrone.LocalPlayerIsOwner() && xuiC_MapArea != null)
						{
							xuiC_MapArea.RefreshDronePositionWaypoint(entityDrone, true);
						}
					}
					else if ((_reason == EnumRemoveEntityReason.Killed || _reason == EnumRemoveEntityReason.Despawned) && xuiC_MapArea != null)
					{
						xuiC_MapArea.RemoveDronePositionWaypoint(entityDrone.entityId);
					}
				}
			}
			this.ObjectOnMapRemove(mapObjectType, _entity.entityId);
		}
	}

	// Token: 0x06005BF6 RID: 23542 RVA: 0x0023CEF0 File Offset: 0x0023B0F0
	public void RefreshEntitiesOnMap()
	{
		foreach (Entity entity in this.Entities.list)
		{
			this.RemoveEntityFromMap(entity, EnumRemoveEntityReason.Undef);
			this.AddEntityToMap(entity);
		}
	}

	// Token: 0x06005BF7 RID: 23543 RVA: 0x0023CF50 File Offset: 0x0023B150
	public void LockAreaMasterChunksAround(Vector3i _blockPos, ulong _worldTimeToLock)
	{
		for (int i = -2; i <= 2; i++)
		{
			for (int j = -2; j <= 2; j++)
			{
				Vector3i vector3i = Chunk.ToAreaMasterChunkPos(new Vector3i(_blockPos.x + i * 80, 0, _blockPos.z + j * 80));
				Chunk chunk = (Chunk)this.GetChunkSync(vector3i.x, vector3i.z);
				if (chunk != null && chunk.GetChunkBiomeSpawnData() != null)
				{
					chunk.isModified |= chunk.GetChunkBiomeSpawnData().DelayAllEnemySpawningUntil(_worldTimeToLock, this.Biomes);
				}
				else
				{
					this.areaMasterChunksToLock[WorldChunkCache.MakeChunkKey(vector3i.x, vector3i.z)] = _worldTimeToLock;
				}
			}
		}
	}

	// Token: 0x06005BF8 RID: 23544 RVA: 0x0023D008 File Offset: 0x0023B208
	public bool IsWaterInBounds(Bounds _aabb)
	{
		Vector3 min = _aabb.min;
		Vector3 max = _aabb.max;
		int num = Utils.Fastfloor(min.x);
		int num2 = Utils.Fastfloor(max.x + 1f);
		int num3 = Utils.Fastfloor(min.y);
		int num4 = Utils.Fastfloor(max.y + 1f);
		int num5 = Utils.Fastfloor(min.z);
		int num6 = Utils.Fastfloor(max.z + 1f);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				for (int k = num5; k < num6; k++)
				{
					if (this.IsWater(i, j, k))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06005BF9 RID: 23545 RVA: 0x0023D0C4 File Offset: 0x0023B2C4
	public bool IsMaterialInBounds(Bounds _aabb, MaterialBlock _material)
	{
		int num = Utils.Fastfloor(_aabb.min.x);
		int num2 = Utils.Fastfloor(_aabb.max.x + 1f);
		int num3 = Utils.Fastfloor(_aabb.min.y);
		int num4 = Utils.Fastfloor(_aabb.max.y + 1f);
		int num5 = Utils.Fastfloor(_aabb.min.z);
		int num6 = Utils.Fastfloor(_aabb.max.z + 1f);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				for (int k = num5; k < num6; k++)
				{
					if (this.GetBlock(i, j, k).Block.blockMaterial == _material)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06005BFA RID: 23546 RVA: 0x0023D1A0 File Offset: 0x0023B3A0
	public void ClearFallingBlocksForChunks(HashSetLong chunks)
	{
		this.resetTempPositions.Clear();
		while (this.fallingBlocks.Count > 0)
		{
			Vector3i vector3i = this.fallingBlocks.Dequeue();
			long num = WorldChunkCache.MakeChunkKey(World.toChunkXZ(vector3i.x), World.toChunkXZ(vector3i.z));
			if (!chunks.Contains(num))
			{
				this.resetTempPositions.Add(vector3i);
			}
			else
			{
				this.fallingBlockSet.Remove(vector3i);
			}
		}
		this.fallingBlocks = new Queue<Vector3i>(this.resetTempPositions);
		EntityFallingBlock.ClearFallingBlocksForChunks(chunks);
		if (EntityFallingBlocks.Enabled)
		{
			this.resetTempGroups.Clear();
			while (this.fallingGroups.Count > 0)
			{
				List<Vector3i> list = this.fallingGroups.Dequeue();
				Vector3i vector3i2 = list[0];
				long num2 = WorldChunkCache.MakeChunkKey(World.toChunkXZ(vector3i2.x), World.toChunkXZ(vector3i2.z));
				if (!chunks.Contains(num2))
				{
					this.resetTempGroups.Add(list);
				}
				else
				{
					foreach (Vector3i vector3i3 in list)
					{
						this.fallingBlockSet.Remove(vector3i3);
						this.groupedBlocks.Remove(vector3i3);
						this.processedBlocks.Remove(vector3i3);
					}
				}
			}
			EntityFallingBlocks.ClearFallingBlocksForChunks(chunks);
		}
	}

	// Token: 0x06005BFB RID: 23547 RVA: 0x0023D30C File Offset: 0x0023B50C
	public override void AddFallingBlocks(IList<Vector3i> _list)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			this.AddFallingBlock(_list[i], false);
		}
	}

	// Token: 0x06005BFC RID: 23548 RVA: 0x0023D338 File Offset: 0x0023B538
	public void AddFallingBlock(Vector3i _blockPos, bool includeOversized = false)
	{
		if (!this.fallingBlockSet.Contains(_blockPos))
		{
			BlockValue block = base.GetBlock(_blockPos);
			if (block.ischild || block.Block.StabilityIgnore || block.isair || (!includeOversized && block.Block.isOversized))
			{
				return;
			}
			DynamicMeshManager.AddFallingBlockObserver(_blockPos);
			this.fallingBlocks.Enqueue(_blockPos);
			this.fallingBlockSet.Add(_blockPos);
		}
	}

	// Token: 0x06005BFD RID: 23549 RVA: 0x0023D3AC File Offset: 0x0023B5AC
	public void LetBlocksFall()
	{
		if (this.fallingBlocks.Count == 0)
		{
			return;
		}
		if (EntityFallingBlocks.Enabled)
		{
			this.GroupFallingBlocks();
		}
		int num = 0;
		Vector3i zero = Vector3i.zero;
		while (this.fallingGroups.Count > 0)
		{
			if (num >= 2)
			{
				break;
			}
			List<Vector3i> list = this.fallingGroups.Dequeue();
			this.CreateFallingBlockGroup(list);
			num++;
			foreach (Vector3i vector3i in list)
			{
				this.processedBlocks.Add(vector3i);
				this.fallingBlockSet.Remove(vector3i);
			}
		}
		while (this.fallingBlocks.Count > 0 && num < 2)
		{
			Vector3i vector3i2 = this.fallingBlocks.Dequeue();
			if (!this.processedBlocks.Contains(vector3i2) && !this.groupedBlocks.Contains(vector3i2))
			{
				if (zero.Equals(vector3i2))
				{
					this.fallingBlocks.Enqueue(vector3i2);
					break;
				}
				this.fallingBlockSet.Remove(vector3i2);
				BlockValue block = this.GetBlock(vector3i2.x, vector3i2.y, vector3i2.z);
				if (!block.isair)
				{
					TextureFullArray textureFullArray = this.GetTextureFullArray(vector3i2.x, vector3i2.y, vector3i2.z);
					SignCanvas.CanvasState canvasState = null;
					TileEntity tileEntity = this.GetTileEntity(vector3i2);
					TEFeatureCanvas tefeatureCanvas;
					if (tileEntity != null && tileEntity.TryGetSelfOrFeature<TEFeatureCanvas>(out tefeatureCanvas) && tefeatureCanvas.Canvas != null)
					{
						canvasState = tefeatureCanvas.Canvas.State.Clone();
					}
					Block block2 = block.Block;
					block2.OnBlockStartsToFall(this, vector3i2, block);
					DynamicMeshManager.ChunkChanged(vector3i2, -1, block.type);
					if (block2.ShowModelOnFall())
					{
						Vector3 vector = new Vector3((float)vector3i2.x + 0.5f + this.RandomRange(-0.1f, 0.1f), (float)vector3i2.y + 0.5f, (float)vector3i2.z + 0.5f + this.RandomRange(-0.1f, 0.1f));
						EntityFallingBlock entityFallingBlock = (EntityFallingBlock)EntityFactory.CreateEntity(EntityClass.FromString("fallingBlock"), -1, new BlockValue[] { block }, new TextureFullArray[] { textureFullArray }, 1, vector, Vector3.zero, -1f, -1, -1, "");
						if (canvasState != null)
						{
							entityFallingBlock.SetCanvasState(canvasState);
						}
						this.SpawnEntityInWorld(entityFallingBlock);
						num++;
					}
				}
			}
		}
		this.processedBlocks.Clear();
	}

	// Token: 0x06005BFE RID: 23550 RVA: 0x0023D638 File Offset: 0x0023B838
	[PublicizedFrom(EAccessModifier.Private)]
	public void GroupFallingBlocks()
	{
		HashSet<Vector3i> hashSet = new HashSet<Vector3i>();
		List<Vector3i> list = new List<Vector3i>();
		Queue<Vector3i> queue = new Queue<Vector3i>();
		HashSet<Vector3i> hashSet2 = new HashSet<Vector3i>();
		foreach (Vector3i vector3i in this.fallingBlockSet)
		{
			if (!this.groupedBlocks.Contains(vector3i) && !hashSet.Contains(vector3i))
			{
				BlockValue block = this.GetBlock(vector3i.x, vector3i.y, vector3i.z);
				if (!block.isair && !block.Block.shape.IsTerrain())
				{
					list.Clear();
					queue.Clear();
					hashSet2.Clear();
					queue.Enqueue(vector3i);
					hashSet2.Add(vector3i);
					World.GroupBounds groupBounds = new World.GroupBounds
					{
						min = vector3i,
						max = vector3i
					};
					while (queue.Count > 0)
					{
						Vector3i vector3i2 = queue.Dequeue();
						list.Add(vector3i2);
						hashSet.Add(vector3i2);
						foreach (Vector3i vector3i3 in new Vector3i[]
						{
							new Vector3i(vector3i2.x + 1, vector3i2.y, vector3i2.z),
							new Vector3i(vector3i2.x - 1, vector3i2.y, vector3i2.z),
							new Vector3i(vector3i2.x, vector3i2.y + 1, vector3i2.z),
							new Vector3i(vector3i2.x, vector3i2.y - 1, vector3i2.z),
							new Vector3i(vector3i2.x, vector3i2.y, vector3i2.z + 1),
							new Vector3i(vector3i2.x, vector3i2.y, vector3i2.z - 1)
						})
						{
							if (this.fallingBlockSet.Contains(vector3i3) && !hashSet2.Contains(vector3i3) && !this.groupedBlocks.Contains(vector3i3))
							{
								World.GroupBounds groupBounds2 = groupBounds;
								groupBounds2.min.x = Math.Min(groupBounds2.min.x, vector3i3.x);
								groupBounds2.min.y = Math.Min(groupBounds2.min.y, vector3i3.y);
								groupBounds2.min.z = Math.Min(groupBounds2.min.z, vector3i3.z);
								groupBounds2.max.x = Math.Max(groupBounds2.max.x, vector3i3.x);
								groupBounds2.max.y = Math.Max(groupBounds2.max.y, vector3i3.y);
								groupBounds2.max.z = Math.Max(groupBounds2.max.z, vector3i3.z);
								if (groupBounds2.IsWithinSize(EntityFallingBlocks.MaxGroupSize))
								{
									groupBounds = groupBounds2;
									queue.Enqueue(vector3i3);
									hashSet2.Add(vector3i3);
								}
							}
						}
					}
					if (list.Count > 1)
					{
						this.fallingGroups.Enqueue(new List<Vector3i>(list));
						foreach (Vector3i vector3i4 in list)
						{
							this.groupedBlocks.Add(vector3i4);
						}
					}
				}
			}
		}
	}

	// Token: 0x06005BFF RID: 23551 RVA: 0x0023DA14 File Offset: 0x0023BC14
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateFallingBlockGroup(List<Vector3i> groupBlocks)
	{
		BlockValue[] blockValues = this.GetBlockValues(groupBlocks);
		TextureFullArray[] blockTextureFullArrays = this.GetBlockTextureFullArrays(groupBlocks);
		Vector3i vector3i = groupBlocks[0];
		BlockValue block = base.GetBlock(vector3i);
		foreach (Vector3i vector3i2 in groupBlocks)
		{
			BlockValue block2 = base.GetBlock(vector3i2);
			block2.Block.OnBlockStartsToFall(this, vector3i2, block2);
			DynamicMeshManager.ChunkChanged(vector3i2, -1, block2.type);
			this.groupedBlocks.Remove(vector3i2);
		}
		if (block.Block.ShowModelOnFall())
		{
			Vector3 vector = new Vector3((float)vector3i.x + 0.5f + this.RandomRange(-0.1f, 0.1f), (float)vector3i.y + 0.5f, (float)vector3i.z + 0.5f + this.RandomRange(-0.1f, 0.1f));
			EntityFallingBlocks entityFallingBlocks = (EntityFallingBlocks)EntityFactory.CreateEntity(EntityClass.FromString("fallingBlocks"), -1, blockValues, blockTextureFullArrays, groupBlocks.Count, vector, Vector3.zero, -1f, -1, -1, "");
			entityFallingBlocks.SetBlockGroupData(groupBlocks.ToArray(), blockValues);
			this.SpawnEntityInWorld(entityFallingBlocks);
		}
	}

	// Token: 0x06005C00 RID: 23552 RVA: 0x0023DB60 File Offset: 0x0023BD60
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue[] GetBlockValues(List<Vector3i> groupBlocks)
	{
		BlockValue[] array = new BlockValue[groupBlocks.Count];
		for (int i = 0; i < groupBlocks.Count; i++)
		{
			array[i] = base.GetBlock(groupBlocks[i]);
		}
		return array;
	}

	// Token: 0x06005C01 RID: 23553 RVA: 0x0023DBA0 File Offset: 0x0023BDA0
	[PublicizedFrom(EAccessModifier.Private)]
	public TextureFullArray[] GetBlockTextureFullArrays(List<Vector3i> groupBlocks)
	{
		TextureFullArray[] array = new TextureFullArray[groupBlocks.Count];
		for (int i = 0; i < groupBlocks.Count; i++)
		{
			array[i] = this.GetTextureFullArray(groupBlocks[i].x, groupBlocks[i].y, groupBlocks[i].z);
		}
		return array;
	}

	// Token: 0x06005C02 RID: 23554 RVA: 0x0023DBFC File Offset: 0x0023BDFC
	public override IGameManager GetGameManager()
	{
		return this.gameManager;
	}

	// Token: 0x06005C03 RID: 23555 RVA: 0x0023DC04 File Offset: 0x0023BE04
	public override Manager GetAudioManager()
	{
		return this.audioManager;
	}

	// Token: 0x06005C04 RID: 23556 RVA: 0x0023DC0C File Offset: 0x0023BE0C
	public override AIDirector GetAIDirector()
	{
		return this.aiDirector;
	}

	// Token: 0x06005C05 RID: 23557 RVA: 0x0023DC14 File Offset: 0x0023BE14
	public override bool IsRemote()
	{
		return !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
	}

	// Token: 0x06005C06 RID: 23558 RVA: 0x0023DC24 File Offset: 0x0023BE24
	public EntityPlayer GetClosestPlayer(float _x, float _y, float _z, int _notFromThisTeam, double _maxDistance)
	{
		float num = -1f;
		EntityPlayer entityPlayer = null;
		for (int i = 0; i < this.Players.list.Count; i++)
		{
			EntityPlayer entityPlayer2 = this.Players.list[i];
			if (!entityPlayer2.IsDead() && entityPlayer2.Spawned && (_notFromThisTeam == 0 || entityPlayer2.TeamNumber != _notFromThisTeam))
			{
				float distanceSq = entityPlayer2.GetDistanceSq(new Vector3(_x, _y, _z));
				if ((_maxDistance < 0.0 || (double)distanceSq < _maxDistance * _maxDistance) && (num == -1f || distanceSq < num))
				{
					num = distanceSq;
					entityPlayer = entityPlayer2;
				}
			}
		}
		return entityPlayer;
	}

	// Token: 0x06005C07 RID: 23559 RVA: 0x0023DCBE File Offset: 0x0023BEBE
	public EntityPlayer GetClosestPlayer(Entity _entity, float _distMax, bool _isDead)
	{
		return this.GetClosestPlayer(_entity.position, _distMax, _isDead);
	}

	// Token: 0x06005C08 RID: 23560 RVA: 0x0023DCD0 File Offset: 0x0023BED0
	public EntityPlayer GetClosestPlayer(Vector3 _pos, float _distMax, bool _isDead)
	{
		if (_distMax < 0f)
		{
			_distMax = float.MaxValue;
		}
		float num = _distMax * _distMax;
		EntityPlayer entityPlayer = null;
		float num2 = float.MaxValue;
		for (int i = this.Players.list.Count - 1; i >= 0; i--)
		{
			EntityPlayer entityPlayer2 = this.Players.list[i];
			if (entityPlayer2.IsDead() == _isDead && entityPlayer2.Spawned)
			{
				float distanceSq = entityPlayer2.GetDistanceSq(_pos);
				if (distanceSq < num2 && distanceSq <= num)
				{
					num2 = distanceSq;
					entityPlayer = entityPlayer2;
				}
			}
		}
		return entityPlayer;
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x0023DD58 File Offset: 0x0023BF58
	public EntityPlayer GetClosestPlayerSeen(EntityAlive _entity, float _distMax, float lightMin)
	{
		Vector3 position = _entity.position;
		if (_distMax < 0f)
		{
			_distMax = float.MaxValue;
		}
		float num = _distMax * _distMax;
		EntityPlayer entityPlayer = null;
		float num2 = float.MaxValue;
		for (int i = this.Players.list.Count - 1; i >= 0; i--)
		{
			EntityPlayer entityPlayer2 = this.Players.list[i];
			if (!entityPlayer2.IsDead() && entityPlayer2.Spawned)
			{
				float distanceSq = entityPlayer2.GetDistanceSq(position);
				if (distanceSq < num2 && distanceSq <= num && entityPlayer2.Stealth.lightLevel >= lightMin && _entity.CanSee(entityPlayer2))
				{
					num2 = distanceSq;
					entityPlayer = entityPlayer2;
				}
			}
		}
		return entityPlayer;
	}

	// Token: 0x06005C0A RID: 23562 RVA: 0x0023DE04 File Offset: 0x0023C004
	public bool IsPlayerAliveAndNear(Vector3 _pos, float _distMax)
	{
		float num = _distMax * _distMax;
		for (int i = this.Players.list.Count - 1; i >= 0; i--)
		{
			EntityPlayer entityPlayer = this.Players.list[i];
			if (!entityPlayer.IsDead() && entityPlayer.Spawned && (entityPlayer.position - _pos).sqrMagnitude <= num)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x0023DE6E File Offset: 0x0023C06E
	public override WorldBlockTicker GetWBT()
	{
		return this.worldBlockTicker;
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x0023DE76 File Offset: 0x0023C076
	public override bool IsOpenSkyAbove(int _x, int _y, int _z)
	{
		return this.ChunkCache == null || ((Chunk)this.GetChunkSync(_x >> 4, _z >> 4)).IsOpenSkyAbove(_x & 15, _y, _z & 15);
	}

	// Token: 0x06005C0D RID: 23565 RVA: 0x0005A6B6 File Offset: 0x000588B6
	public override bool IsEditor()
	{
		return GameManager.Instance.IsEditMode();
	}

	// Token: 0x06005C0E RID: 23566 RVA: 0x0023DEA1 File Offset: 0x0023C0A1
	public int GetGameMode()
	{
		return GameStats.GetInt(EnumGameStats.GameModeId);
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x0023DEA9 File Offset: 0x0023C0A9
	public SpawnManagerDynamic GetDynamiceSpawnManager()
	{
		return this.dynamicSpawnManager;
	}

	// Token: 0x06005C10 RID: 23568 RVA: 0x0023DEB4 File Offset: 0x0023C0B4
	public override bool CanPlaceLandProtectionBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative)
	{
		if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
		{
			return true;
		}
		if (this.InBoundsForPlayersPercent(blockPos.ToVector3CenterXZ()) < 0.5f)
		{
			return false;
		}
		this.m_lpChunkList.Clear();
		int num = GameStats.GetInt(EnumGameStats.LandClaimSize) - 1;
		int num2 = GameStats.GetInt(EnumGameStats.LandClaimDeadZone) + num;
		int num3 = num2 / 16 + 1;
		int num4 = num2 / 16 + 1;
		for (int i = -num3; i <= num3; i++)
		{
			int num5 = blockPos.x + i * 16;
			for (int j = -num4; j <= num4; j++)
			{
				int num6 = blockPos.z + j * 16;
				Chunk chunk = (Chunk)base.GetChunkFromWorldPos(new Vector3i(num5, blockPos.y, num6));
				if (chunk != null && !this.m_lpChunkList.Contains(chunk))
				{
					this.m_lpChunkList.Add(chunk);
					if (this.IsLandProtectedBlock(chunk, blockPos, lpRelative, num, num2, true))
					{
						this.m_lpChunkList.Clear();
						return false;
					}
				}
			}
		}
		int num7 = num2 / 2;
		Vector3i vector3i = new Vector3i(num7, num7, num7);
		Vector3i vector3i2 = blockPos - vector3i;
		Vector3i vector3i3 = blockPos + vector3i;
		if (this.IsWithinTraderArea(vector3i2, vector3i3))
		{
			return false;
		}
		this.m_lpChunkList.Clear();
		return true;
	}

	// Token: 0x06005C11 RID: 23569 RVA: 0x0023DFEC File Offset: 0x0023C1EC
	public bool IsEmptyPosition(Vector3i blockPos)
	{
		if (this.IsWithinTraderArea(blockPos))
		{
			return false;
		}
		if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
		{
			return true;
		}
		this.m_lpChunkList.Clear();
		int @int = GameStats.GetInt(EnumGameStats.LandClaimSize);
		int num = (@int - 1) / 2;
		int num2 = @int / 16 + 1;
		int num3 = @int / 16 + 1;
		int num4 = blockPos.x - num;
		int num5 = blockPos.z - num;
		for (int i = -num2; i <= num2; i++)
		{
			int num6 = num4 + i * 16;
			for (int j = -num3; j <= num3; j++)
			{
				int num7 = num5 + j * 16;
				Chunk chunk = (Chunk)base.GetChunkFromWorldPos(new Vector3i(num6, blockPos.y, num7));
				if (chunk != null && !this.m_lpChunkList.Contains(chunk))
				{
					this.m_lpChunkList.Add(chunk);
					if (this.IsLandProtectedBlock(chunk, blockPos, null, num, num, false))
					{
						this.m_lpChunkList.Clear();
						return false;
					}
				}
			}
		}
		this.m_lpChunkList.Clear();
		return true;
	}

	// Token: 0x06005C12 RID: 23570 RVA: 0x0023E0E6 File Offset: 0x0023C2E6
	public override bool CanPickupBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative)
	{
		return (World.SandboxUseTraderArea != TraderAreaStates.Default || !this.IsWithinTraderArea(blockPos)) && this.CanPlaceBlockAt(blockPos, lpRelative, false);
	}

	// Token: 0x06005C13 RID: 23571 RVA: 0x0023E104 File Offset: 0x0023C304
	public override bool CanPlaceBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative, bool traderAllowed = false)
	{
		if (!traderAllowed && World.SandboxUseTraderArea == TraderAreaStates.Default && this.IsWithinTraderArea(blockPos))
		{
			return false;
		}
		if (this.InBoundsForPlayersPercent(blockPos.ToVector3CenterXZ()) < 0.5f)
		{
			return false;
		}
		if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
		{
			return true;
		}
		this.m_lpChunkList.Clear();
		int @int = GameStats.GetInt(EnumGameStats.LandClaimSize);
		int num = (@int - 1) / 2;
		int num2 = @int / 16 + 1;
		int num3 = @int / 16 + 1;
		int num4 = blockPos.x - num;
		int num5 = blockPos.z - num;
		for (int i = -num2; i <= num2; i++)
		{
			int num6 = num4 + i * 16;
			for (int j = -num3; j <= num3; j++)
			{
				int num7 = num5 + j * 16;
				Chunk chunk = (Chunk)base.GetChunkFromWorldPos(new Vector3i(num6, blockPos.y, num7));
				if (chunk != null && !this.m_lpChunkList.Contains(chunk))
				{
					this.m_lpChunkList.Add(chunk);
					if (this.IsLandProtectedBlock(chunk, blockPos, lpRelative, num, num, false))
					{
						this.m_lpChunkList.Clear();
						return false;
					}
				}
			}
		}
		this.m_lpChunkList.Clear();
		return true;
	}

	// Token: 0x06005C14 RID: 23572 RVA: 0x0023E220 File Offset: 0x0023C420
	public override float GetLandProtectionHardnessModifier(Vector3i blockPos, EntityAlive lpRelative, PersistentPlayerData ppData)
	{
		if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
		{
			return 1f;
		}
		if (lpRelative is EntityEnemy || lpRelative == null)
		{
			return 1f;
		}
		float num = 1f;
		BlockValue block = base.GetBlock(blockPos);
		if (!block.Equals(BlockValue.Air))
		{
			num = block.Block.LPHardnessScale;
			if (num == 0f)
			{
				return 1f;
			}
		}
		this.m_lpChunkList.Clear();
		int @int = GameStats.GetInt(EnumGameStats.LandClaimSize);
		int num2 = (@int - 1) / 2;
		int num3 = @int / 16 + 1;
		int num4 = @int / 16 + 1;
		int num5 = blockPos.x - num2;
		int num6 = blockPos.z - num2;
		float num7 = 1f;
		for (int i = -num3; i <= num3; i++)
		{
			int num8 = num5 + i * 16;
			for (int j = -num4; j <= num4; j++)
			{
				int num9 = num6 + j * 16;
				Chunk chunk = (Chunk)base.GetChunkFromWorldPos(new Vector3i(num8, blockPos.y, num9));
				if (chunk != null && !this.m_lpChunkList.Contains(chunk))
				{
					this.m_lpChunkList.Add(chunk);
					float landProtectionHardnessModifier = this.GetLandProtectionHardnessModifier(chunk, blockPos, ppData, num2);
					if (landProtectionHardnessModifier < 1f)
					{
						this.m_lpChunkList.Clear();
						return landProtectionHardnessModifier;
					}
					num7 = Math.Max(num7, landProtectionHardnessModifier);
				}
			}
		}
		this.m_lpChunkList.Clear();
		if (num7 > 1f)
		{
			if (lpRelative is EntityVehicle)
			{
				num7 *= 2f;
			}
			return num7 * num;
		}
		return num7;
	}

	// Token: 0x06005C15 RID: 23573 RVA: 0x0023E3A4 File Offset: 0x0023C5A4
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetLandProtectionHardnessModifier(Chunk chunk, Vector3i blockPos, PersistentPlayerData lpRelative, int halfClaimSize)
	{
		float num = 1f;
		PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
		List<Vector3i> list = chunk.IndexedBlocks["lpblock"];
		if (list != null)
		{
			Vector3i worldPos = chunk.GetWorldPos();
			for (int i = 0; i < list.Count; i++)
			{
				Vector3i vector3i = list[i] + worldPos;
				TEFeatureLandClaim selfOrFeature = this.GetTileEntity(vector3i).GetSelfOrFeature<TEFeatureLandClaim>();
				if (selfOrFeature != null && selfOrFeature.IsPrimary())
				{
					PersistentPlayerData landProtectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(vector3i);
					if (landProtectionBlockOwner != null && (lpRelative == null || (landProtectionBlockOwner != lpRelative && (blockPos == vector3i || !landProtectionBlockOwner.IsAlly(lpRelative)))))
					{
						int num2 = Math.Abs(vector3i.x - blockPos.x);
						int num3 = Math.Abs(vector3i.z - blockPos.z);
						if (num2 <= halfClaimSize && num3 <= halfClaimSize)
						{
							float landProtectionHardnessModifierForPlayer = this.GetLandProtectionHardnessModifierForPlayer(landProtectionBlockOwner);
							if (landProtectionHardnessModifierForPlayer < 1f)
							{
								return landProtectionHardnessModifierForPlayer;
							}
							num = Mathf.Max(num, landProtectionHardnessModifierForPlayer);
							if (lpRelative != null)
							{
								EntityPlayer entityPlayer = this.GetEntity(lpRelative.EntityId) as EntityPlayer;
								num = EffectManager.GetValue(PassiveEffects.LandClaimDamageModifier, entityPlayer.inventory.holdingItemItemValue, num, entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06005C16 RID: 23574 RVA: 0x0023E4F2 File Offset: 0x0023C6F2
	public override bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative)
	{
		return this.IsMyLandProtectedBlock(worldBlockPos, lpRelative, World.SandboxUseTraderArea == TraderAreaStates.Claimable);
	}

	// Token: 0x06005C17 RID: 23575 RVA: 0x0023E504 File Offset: 0x0023C704
	public override bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative, bool traderAllowed = false)
	{
		if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
		{
			return true;
		}
		if (!traderAllowed && this.IsWithinTraderArea(worldBlockPos))
		{
			return false;
		}
		this.m_lpChunkList.Clear();
		int @int = GameStats.GetInt(EnumGameStats.LandClaimSize);
		int num = (@int - 1) / 2;
		int num2 = @int / 16 + 1;
		int num3 = @int / 16 + 1;
		int num4 = worldBlockPos.x - num;
		int num5 = worldBlockPos.z - num;
		for (int i = -num2; i <= num2; i++)
		{
			int num6 = num4 + i * 16;
			for (int j = -num3; j <= num3; j++)
			{
				int num7 = num5 + j * 16;
				Chunk chunk = (Chunk)base.GetChunkFromWorldPos(new Vector3i(num6, worldBlockPos.y, num7));
				if (chunk != null && !this.m_lpChunkList.Contains(chunk))
				{
					this.m_lpChunkList.Add(chunk);
					if (this.IsMyLandClaimInChunk(chunk, worldBlockPos, lpRelative, num, num, false))
					{
						this.m_lpChunkList.Clear();
						return true;
					}
				}
			}
		}
		this.m_lpChunkList.Clear();
		return false;
	}

	// Token: 0x06005C18 RID: 23576 RVA: 0x0023E604 File Offset: 0x0023C804
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsLandProtectedBlock(Chunk chunk, Vector3i blockPos, PersistentPlayerData lpRelative, int claimSize, int deadZone, bool forKeystone)
	{
		PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
		List<Vector3i> list = chunk.IndexedBlocks["lpblock"];
		if (list != null)
		{
			Vector3i worldPos = chunk.GetWorldPos();
			for (int i = 0; i < list.Count; i++)
			{
				Vector3i vector3i = list[i] + worldPos;
				TEFeatureLandClaim selfOrFeature = this.GetTileEntity(vector3i).GetSelfOrFeature<TEFeatureLandClaim>();
				if (selfOrFeature != null && selfOrFeature.IsPrimary())
				{
					int num = Math.Abs(vector3i.x - blockPos.x);
					int num2 = Math.Abs(vector3i.z - blockPos.z);
					if (num <= deadZone && num2 <= deadZone)
					{
						PersistentPlayerData landProtectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(vector3i);
						if (landProtectionBlockOwner != null)
						{
							bool flag = this.IsLandProtectionValidForPlayer(landProtectionBlockOwner);
							if (flag && lpRelative != null)
							{
								if (lpRelative == landProtectionBlockOwner)
								{
									flag = false;
								}
								else if (landProtectionBlockOwner.IsAlly(lpRelative))
								{
									flag = num <= claimSize && num2 <= claimSize && forKeystone;
								}
							}
							if (flag)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06005C19 RID: 23577 RVA: 0x0023E70C File Offset: 0x0023C90C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsMyLandClaimInChunk(Chunk chunk, Vector3i blockPos, PersistentPlayerData lpRelative, int claimSize, int deadZone, bool forKeystone)
	{
		PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
		List<Vector3i> list = chunk.IndexedBlocks["lpblock"];
		if (list != null)
		{
			Vector3i worldPos = chunk.GetWorldPos();
			for (int i = 0; i < list.Count; i++)
			{
				Vector3i vector3i = list[i] + worldPos;
				TEFeatureLandClaim selfOrFeature = this.GetTileEntity(vector3i).GetSelfOrFeature<TEFeatureLandClaim>();
				if (selfOrFeature != null && selfOrFeature.IsPrimary())
				{
					int num = Math.Abs(vector3i.x - blockPos.x);
					int num2 = Math.Abs(vector3i.z - blockPos.z);
					if (num <= deadZone && num2 <= deadZone)
					{
						PersistentPlayerData landProtectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(vector3i);
						if (landProtectionBlockOwner != null)
						{
							bool flag = this.IsLandProtectionValidForPlayer(landProtectionBlockOwner);
							if (flag && lpRelative != null)
							{
								if (lpRelative == landProtectionBlockOwner)
								{
									flag = num <= claimSize && num2 <= claimSize;
								}
								else
								{
									flag = landProtectionBlockOwner.IsAlly(lpRelative) && (num <= claimSize && num2 <= claimSize && forKeystone);
								}
							}
							if (flag)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06005C1A RID: 23578 RVA: 0x0023E828 File Offset: 0x0023CA28
	public override EnumLandClaimOwner GetLandClaimOwner(Vector3i worldBlockPos, PersistentPlayerData lpRelative)
	{
		if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
		{
			return EnumLandClaimOwner.Self;
		}
		if (this.IsWithinTraderArea(worldBlockPos))
		{
			return EnumLandClaimOwner.None;
		}
		this.m_lpChunkList.Clear();
		int @int = GameStats.GetInt(EnumGameStats.LandClaimSize);
		int num = (@int - 1) / 2;
		int num2 = @int / 16 + 1;
		int num3 = @int / 16 + 1;
		int num4 = worldBlockPos.x - num;
		int num5 = worldBlockPos.z - num;
		for (int i = -num2; i <= num2; i++)
		{
			int num6 = num4 + i * 16;
			for (int j = -num3; j <= num3; j++)
			{
				int num7 = num5 + j * 16;
				Chunk chunk = (Chunk)base.GetChunkFromWorldPos(new Vector3i(num6, worldBlockPos.y, num7));
				if (chunk != null && !this.m_lpChunkList.Contains(chunk))
				{
					this.m_lpChunkList.Add(chunk);
					EnumLandClaimOwner landClaimOwner = this.GetLandClaimOwner(chunk, worldBlockPos, lpRelative, num, num, false);
					if (landClaimOwner != EnumLandClaimOwner.None)
					{
						this.m_lpChunkList.Clear();
						return landClaimOwner;
					}
				}
			}
		}
		this.m_lpChunkList.Clear();
		return EnumLandClaimOwner.None;
	}

	// Token: 0x06005C1B RID: 23579 RVA: 0x0023E928 File Offset: 0x0023CB28
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumLandClaimOwner GetLandClaimOwner(Chunk chunk, Vector3i blockPos, PersistentPlayerData lpRelative, int claimSize, int deadZone, bool forKeystone)
	{
		PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
		List<Vector3i> list = chunk.IndexedBlocks["lpblock"];
		if (list != null)
		{
			Vector3i worldPos = chunk.GetWorldPos();
			for (int i = 0; i < list.Count; i++)
			{
				Vector3i vector3i = list[i] + worldPos;
				TEFeatureLandClaim selfOrFeature = this.GetTileEntity(vector3i).GetSelfOrFeature<TEFeatureLandClaim>();
				if (selfOrFeature != null && selfOrFeature.IsPrimary())
				{
					int num = Math.Abs(vector3i.x - blockPos.x);
					int num2 = Math.Abs(vector3i.z - blockPos.z);
					if (num <= deadZone && num2 <= deadZone)
					{
						PersistentPlayerData landProtectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(vector3i);
						if (landProtectionBlockOwner != null && this.IsLandProtectionValidForPlayer(landProtectionBlockOwner))
						{
							if (lpRelative == null)
							{
								return EnumLandClaimOwner.Other;
							}
							if (lpRelative == landProtectionBlockOwner)
							{
								return EnumLandClaimOwner.Self;
							}
							if (landProtectionBlockOwner.IsAlly(lpRelative))
							{
								return EnumLandClaimOwner.Ally;
							}
							return EnumLandClaimOwner.Other;
						}
					}
				}
			}
		}
		return EnumLandClaimOwner.None;
	}

	// Token: 0x06005C1C RID: 23580 RVA: 0x0023EA08 File Offset: 0x0023CC08
	public bool GetLandClaimOwnerInParty(EntityPlayer player, PersistentPlayerData lpRelative)
	{
		if (GameStats.GetInt(EnumGameStats.GameModeId) != 1)
		{
			return false;
		}
		Vector3i blockPosition = player.GetBlockPosition();
		if (this.IsWithinTraderArea(blockPosition))
		{
			return false;
		}
		this.m_lpChunkList.Clear();
		int @int = GameStats.GetInt(EnumGameStats.LandClaimSize);
		int num = (@int - 1) / 2;
		int num2 = @int / 16 + 1;
		int num3 = @int / 16 + 1;
		int num4 = blockPosition.x - num;
		int num5 = blockPosition.z - num;
		for (int i = -num2; i <= num2; i++)
		{
			int num6 = num4 + i * 16;
			for (int j = -num3; j <= num3; j++)
			{
				int num7 = num5 + j * 16;
				Chunk chunk = (Chunk)base.GetChunkFromWorldPos(new Vector3i(num6, blockPosition.y, num7));
				if (chunk != null && !this.m_lpChunkList.Contains(chunk))
				{
					this.m_lpChunkList.Add(chunk);
					if (this.GetLandClaimOwnerInParty(chunk, player, blockPosition, lpRelative, num, num))
					{
						this.m_lpChunkList.Clear();
						return true;
					}
				}
			}
		}
		this.m_lpChunkList.Clear();
		return false;
	}

	// Token: 0x06005C1D RID: 23581 RVA: 0x0023EB0C File Offset: 0x0023CD0C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool GetLandClaimOwnerInParty(Chunk chunk, EntityPlayer player, Vector3i blockPos, PersistentPlayerData lpRelative, int claimSize, int deadZone)
	{
		PersistentPlayerList persistentPlayerList = this.gameManager.GetPersistentPlayerList();
		bool flag = player.Party != null;
		List<Vector3i> list = chunk.IndexedBlocks["lpblock"];
		if (list != null)
		{
			Vector3i worldPos = chunk.GetWorldPos();
			for (int i = 0; i < list.Count; i++)
			{
				Vector3i vector3i = list[i] + worldPos;
				TEFeatureLandClaim selfOrFeature = this.GetTileEntity(vector3i).GetSelfOrFeature<TEFeatureLandClaim>();
				if (selfOrFeature != null && selfOrFeature.IsPrimary())
				{
					int num = Math.Abs(vector3i.x - blockPos.x);
					int num2 = Math.Abs(vector3i.z - blockPos.z);
					if (num <= deadZone && num2 <= deadZone)
					{
						PersistentPlayerData landProtectionBlockOwner = persistentPlayerList.GetLandProtectionBlockOwner(vector3i);
						if (landProtectionBlockOwner != null)
						{
							if (lpRelative == null && player != null && landProtectionBlockOwner.EntityId == player.entityId)
							{
								lpRelative = landProtectionBlockOwner;
							}
							if (this.IsLandProtectionValidForPlayer(landProtectionBlockOwner) && lpRelative != null)
							{
								if (lpRelative == landProtectionBlockOwner)
								{
									if (num <= claimSize && num2 <= claimSize)
									{
										return true;
									}
								}
								else if (flag && landProtectionBlockOwner.IsAlly(lpRelative) && player.Party.ContainsMember(landProtectionBlockOwner.EntityId) && num <= claimSize && num2 <= claimSize)
								{
									return true;
								}
							}
							return false;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06005C1E RID: 23582 RVA: 0x0023EC5C File Offset: 0x0023CE5C
	public bool IsLandProtectionValidForPlayer(PersistentPlayerData ppData)
	{
		double num = (double)GameStats.GetInt(EnumGameStats.LandClaimExpiryTime) * 24.0;
		return ppData.OfflineHours <= num;
	}

	// Token: 0x06005C1F RID: 23583 RVA: 0x0023EC88 File Offset: 0x0023CE88
	public float GetLandProtectionHardnessModifierForPlayer(PersistentPlayerData ppData)
	{
		float num = (float)GameStats.GetInt(EnumGameStats.LandClaimOnlineDurabilityModifier);
		if (ppData.EntityId != -1)
		{
			return num;
		}
		double offlineHours = ppData.OfflineHours;
		double offlineMinutes = ppData.OfflineMinutes;
		float num2 = (float)GameStats.GetInt(EnumGameStats.LandClaimOfflineDelay);
		if (num2 != 0f && offlineMinutes <= (double)num2)
		{
			return num;
		}
		double num3 = (double)GameStats.GetInt(EnumGameStats.LandClaimExpiryTime) * 24.0;
		if (offlineHours > num3)
		{
			return 1f;
		}
		EnumLandClaimDecayMode @int = (EnumLandClaimDecayMode)GameStats.GetInt(EnumGameStats.LandClaimDecayMode);
		float num4 = (float)GameStats.GetInt(EnumGameStats.LandClaimOfflineDurabilityModifier);
		if (num4 == 0f)
		{
			return 0f;
		}
		if (@int == EnumLandClaimDecayMode.DecaySlowly)
		{
			double num5 = (offlineHours - 24.0) / (num3 - 24.0);
			return Mathf.Max(1f, (float)(1.0 - num5) * num4);
		}
		if (@int == EnumLandClaimDecayMode.BuffedUntilExpired)
		{
			return num4;
		}
		double num6 = (offlineHours - 24.0) / (num3 - 24.0);
		return Mathf.Max(1f, (float)((1.0 - num6) * (1.0 - num6)) * num4);
	}

	// Token: 0x06005C20 RID: 23584 RVA: 0x0023ED94 File Offset: 0x0023CF94
	public float GetDecorationOffsetY(Vector3i _blockPos)
	{
		sbyte density = base.GetDensity(_blockPos);
		sbyte density2 = base.GetDensity(_blockPos - Vector3i.up);
		return MarchingCubes.GetDecorationOffsetY(density, density2);
	}

	// Token: 0x06005C21 RID: 23585 RVA: 0x0023EDC0 File Offset: 0x0023CFC0
	public EnumDecoAllowed GetDecoAllowedAt(int _x, int _z)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_x, _z);
		if (chunk != null)
		{
			return chunk.GetDecoAllowedAt(World.toBlockXZ(_x), World.toBlockXZ(_z));
		}
		return EnumDecoAllowed.Nothing;
	}

	// Token: 0x06005C22 RID: 23586 RVA: 0x0023EDF8 File Offset: 0x0023CFF8
	public void SetDecoAllowedAt(int _x, int _z, EnumDecoAllowed _decoAllowed)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_x, _z);
		if (chunk != null)
		{
			chunk.SetDecoAllowedAt(World.toBlockXZ(_x), World.toBlockXZ(_z), _decoAllowed);
		}
	}

	// Token: 0x06005C23 RID: 23587 RVA: 0x0023EE2C File Offset: 0x0023D02C
	public Vector3 GetTerrainNormalAt(int _x, int _z)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_x, _z);
		if (chunk != null)
		{
			return chunk.GetTerrainNormal(World.toBlockXZ(_x), World.toBlockXZ(_z));
		}
		return Vector3.zero;
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x0023EE62 File Offset: 0x0023D062
	public bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
	{
		return this.ChunkCache.ChunkProvider.GetWorldExtent(out _minSize, out _maxSize);
	}

	// Token: 0x06005C25 RID: 23589 RVA: 0x0023EE78 File Offset: 0x0023D078
	public virtual bool IsPositionAvailable(Vector3 _position)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return false;
		}
		Vector3i vector3i = World.worldToBlockPos(_position);
		for (int i = 0; i < Vector3i.MIDDLE_AND_HORIZONTAL_DIRECTIONS_DIAGONAL.Length; i++)
		{
			Vector3i vector3i2 = Vector3i.MIDDLE_AND_HORIZONTAL_DIRECTIONS_DIAGONAL[i] * 16;
			IChunk chunkFromWorldPos = chunkCache.GetChunkFromWorldPos(vector3i + vector3i2);
			if (chunkFromWorldPos == null || !chunkFromWorldPos.GetAvailable())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x0023EEDC File Offset: 0x0023D0DC
	public bool GetBiomeIntensity(Vector3i _position, out BiomeIntensity _biomeIntensity)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_position);
		if (chunk != null && !chunk.NeedsLightCalculation)
		{
			_biomeIntensity = chunk.GetBiomeIntensity(World.toBlockXZ(_position.x), World.toBlockXZ(_position.z));
			return true;
		}
		_biomeIntensity = BiomeIntensity.Default;
		return false;
	}

	// Token: 0x06005C27 RID: 23591 RVA: 0x0023EF34 File Offset: 0x0023D134
	public bool CanMobsSpawnAtPos(Vector3 _pos)
	{
		Vector3i vector3i = World.worldToBlockPos(_pos);
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
		return chunk != null && chunk.CanMobsSpawnAtPos(World.toBlockXZ(vector3i.x), World.toBlockY(vector3i.y), World.toBlockXZ(vector3i.z), false, true);
	}

	// Token: 0x06005C28 RID: 23592 RVA: 0x0023EF84 File Offset: 0x0023D184
	public bool CanSleeperSpawnAtPos(Vector3 _pos, bool _checkBelow)
	{
		Vector3i vector3i = World.worldToBlockPos(_pos);
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
		return chunk != null && chunk.CanSleeperSpawnAtPos(World.toBlockXZ(vector3i.x), World.toBlockY(vector3i.y), World.toBlockXZ(vector3i.z), _checkBelow);
	}

	// Token: 0x06005C29 RID: 23593 RVA: 0x0023EFD4 File Offset: 0x0023D1D4
	public bool CanPlayersSpawnAtPos(Vector3 _pos, bool _bAllowToSpawnOnAirPos = false)
	{
		Vector3i vector3i = World.worldToBlockPos(_pos);
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
		return chunk != null && chunk.CanPlayersSpawnAtPos(World.toBlockXZ(vector3i.x), World.toBlockY(vector3i.y), World.toBlockXZ(vector3i.z), _bAllowToSpawnOnAirPos);
	}

	// Token: 0x06005C2A RID: 23594 RVA: 0x0023F024 File Offset: 0x0023D224
	public void CheckEntityCollisionWithBlocks(Entity _entity)
	{
		if (!_entity.CanCollideWithBlocks())
		{
			return;
		}
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		if (chunkCache.Overlaps(_entity.boundingBox))
		{
			chunkCache.CheckCollisionWithBlocks(_entity);
		}
	}

	// Token: 0x06005C2B RID: 23595 RVA: 0x0023F05C File Offset: 0x0023D25C
	public void OnChunkAdded(Chunk _c)
	{
		List<long> list = this.newlyLoadedChunksThisUpdate;
		lock (list)
		{
			this.newlyLoadedChunksThisUpdate.Add(_c.Key);
		}
	}

	// Token: 0x06005C2C RID: 23596 RVA: 0x0023F0A8 File Offset: 0x0023D2A8
	public void OnChunkBeforeRemove(Chunk _c)
	{
		List<long> list = this.newlyLoadedChunksThisUpdate;
		lock (list)
		{
			this.newlyLoadedChunksThisUpdate.Remove(_c.Key);
		}
		if (this.worldBlockTicker != null)
		{
			this.worldBlockTicker.OnChunkRemoved(_c);
		}
		GameManager.Instance.prefabLODManager.TriggerUpdate();
	}

	// Token: 0x06005C2D RID: 23597 RVA: 0x0023F118 File Offset: 0x0023D318
	public void OnChunkBeforeSave(Chunk _c)
	{
		if (this.worldBlockTicker != null)
		{
			this.worldBlockTicker.OnChunkBeforeSave(_c);
		}
	}

	// Token: 0x06005C2E RID: 23598 RVA: 0x0023F130 File Offset: 0x0023D330
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateChunkAddedRemovedCallbacks()
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		List<long> list = this.newlyLoadedChunksThisUpdate;
		lock (list)
		{
			for (int i = this.newlyLoadedChunksThisUpdate.Count - 1; i >= 0; i--)
			{
				long num = this.newlyLoadedChunksThisUpdate[i];
				Chunk chunkSync = chunkCache.GetChunkSync(num);
				if (chunkSync != null && !chunkSync.NeedsDecoration)
				{
					chunkSync.OnLoad(this);
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						this.worldBlockTicker.OnChunkAdded(this, chunkSync, this.rand);
					}
					this.newlyLoadedChunksThisUpdate.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x06005C2F RID: 23599 RVA: 0x0023F1E8 File Offset: 0x0023D3E8
	public override ulong GetWorldTime()
	{
		return this.worldTime;
	}

	// Token: 0x06005C30 RID: 23600 RVA: 0x0023F1F0 File Offset: 0x0023D3F0
	public override WorldCreationData GetWorldCreationData()
	{
		return this.wcd;
	}

	// Token: 0x06005C31 RID: 23601 RVA: 0x0023F1F8 File Offset: 0x0023D3F8
	public bool IsEntityInRange(int _entityId, int _refEntity, int _range)
	{
		Entity entity;
		Entity entity2;
		return _entityId == _refEntity || (this.Entities.dict.TryGetValue(_entityId, out entity) && this.Entities.dict.TryGetValue(_refEntity, out entity2) && entity.GetDistanceSq(entity2) <= (float)(_range * _range));
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x0023F248 File Offset: 0x0023D448
	public bool IsEntityInRange(int _entityId, Vector3 _position, int _range)
	{
		Entity entity;
		return this.Entities.dict.TryGetValue(_entityId, out entity) && entity.GetDistanceSq(_position) <= (float)(_range * _range);
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x0023F27C File Offset: 0x0023D47C
	public bool IsPositionInBounds(Vector3 position)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Navezgane")
		{
			vector3i = new Vector3i(-2900, vector3i.y, -2900);
			vector3i2 = new Vector3i(2900, vector3i2.y, 2900);
		}
		else if (!GameUtils.IsPlaytesting())
		{
			vector3i = new Vector3i(vector3i.x + 90, vector3i.y, vector3i.z + 90);
			vector3i2 = new Vector3i(vector3i2.x - 90, vector3i2.y, vector3i2.z - 90);
		}
		Vector3Int vector3Int = vector3i;
		Vector3Int vector3Int2 = vector3i2;
		BoundsInt boundsInt = new BoundsInt(vector3Int, vector3Int2 - vector3Int);
		return boundsInt.Contains(Vector3Int.RoundToInt(position));
	}

	// Token: 0x06005C34 RID: 23604 RVA: 0x0023F348 File Offset: 0x0023D548
	public float InBoundsForPlayersPercent(Vector3 _pos)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		if (vector3i2.x - vector3i.x < 1024)
		{
			return 1f;
		}
		Vector2 vector;
		vector.x = (float)(vector3i.x + vector3i2.x) * 0.5f;
		vector.y = (float)(vector3i.z + vector3i2.z) * 0.5f;
		float num;
		if (_pos.x < vector.x)
		{
			num = (_pos.x - ((float)vector3i.x + 50f)) / 80f;
		}
		else
		{
			num = ((float)vector3i2.x - 50f - _pos.x) / 80f;
		}
		num = Utils.FastClamp01(num);
		float num2;
		if (_pos.z < vector.y)
		{
			num2 = (_pos.z - ((float)vector3i.z + 50f)) / 80f;
		}
		else
		{
			num2 = ((float)vector3i2.z - 50f - _pos.z) / 80f;
		}
		num2 = Utils.FastClamp01(num2);
		return Utils.FastMin(num, num2);
	}

	// Token: 0x06005C35 RID: 23605 RVA: 0x0023F458 File Offset: 0x0023D658
	public bool AdjustBoundsForPlayers(ref Vector3 _pos, float _padPercent)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		if (vector3i2.x - vector3i.x < 1024)
		{
			return false;
		}
		if (vector3i2.x == 0)
		{
			return false;
		}
		int num = (int)(50f + 80f * _padPercent);
		vector3i.x += num;
		vector3i.z += num;
		vector3i2.x -= num;
		vector3i2.z -= num;
		bool flag = false;
		if (_pos.x < (float)vector3i.x)
		{
			_pos.x = (float)vector3i.x;
			flag = true;
		}
		else if (_pos.x > (float)vector3i2.x)
		{
			_pos.x = (float)vector3i2.x;
			flag = true;
		}
		if (_pos.z < (float)vector3i.z)
		{
			_pos.z = (float)vector3i.z;
			flag = true;
		}
		else if (_pos.z > (float)vector3i2.z)
		{
			_pos.z = (float)vector3i2.z;
			flag = true;
		}
		return flag;
	}

	// Token: 0x06005C36 RID: 23606 RVA: 0x0023F550 File Offset: 0x0023D750
	public bool IsPositionRadiated(Vector3 position)
	{
		IChunkProvider chunkProvider = this.ChunkCache.ChunkProvider;
		IBiomeProvider biomeProvider;
		return chunkProvider != null && (biomeProvider = chunkProvider.GetBiomeProvider()) != null && biomeProvider.GetRadiationAt((int)position.x, (int)position.z) > 0f;
	}

	// Token: 0x06005C37 RID: 23607 RVA: 0x0023F593 File Offset: 0x0023D793
	public bool IsPositionWithinPOI(Vector3 position, int offset)
	{
		return this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().GetPrefabFromWorldPosInsideWithOffset((int)position.x, (int)position.z, offset) != null;
	}

	// Token: 0x06005C38 RID: 23608 RVA: 0x0023F5BC File Offset: 0x0023D7BC
	public PrefabInstance GetPOIAtPosition(Vector3 _position, FastTags<TagGroup.Poi>? _excludeTags = null, FastTags<TagGroup.Poi>? _requiredTags = null)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator == null)
		{
			return null;
		}
		return dynamicPrefabDecorator.GetPrefabAtPosition(_position, _excludeTags, _requiredTags);
	}

	// Token: 0x06005C39 RID: 23609 RVA: 0x0023F5E8 File Offset: 0x0023D7E8
	public void GetPOIsAtXZ(int _xMin, int _xMax, int _zMin, int _zMax, List<PrefabInstance> _list)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator != null)
		{
			dynamicPrefabDecorator.GetPrefabsAtXZ(_xMin, _xMax, _zMin, _zMax, _list);
		}
	}

	// Token: 0x06005C3A RID: 23610 RVA: 0x0023F618 File Offset: 0x0023D818
	public Vector3 ClampToValidWorldPos(Vector3 position)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Navezgane")
		{
			vector3i = new Vector3i(-2900, vector3i.y, -2900);
			vector3i2 = new Vector3i(2900, vector3i2.y, 2900);
		}
		else if (!GameUtils.IsPlaytesting())
		{
			vector3i = new Vector3i(vector3i.x + 90, vector3i.y, vector3i.z + 90);
			vector3i2 = new Vector3i(vector3i2.x - 90, vector3i2.y, vector3i2.z - 90);
		}
		float num = Mathf.Clamp(position.x, (float)vector3i.x, (float)vector3i2.x);
		float num2 = Mathf.Clamp(position.y, (float)vector3i.y, (float)vector3i2.y);
		float num3 = Mathf.Clamp(position.z, (float)vector3i.z, (float)vector3i2.z);
		return new Vector3(num, num2, num3);
	}

	// Token: 0x06005C3B RID: 23611 RVA: 0x0023F70C File Offset: 0x0023D90C
	public Vector3 ClampToValidWorldPosForMap(Vector2 position)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		float num = Mathf.Clamp(position.x, (float)vector3i.x, (float)vector3i2.x);
		float num2 = Mathf.Clamp(position.y, (float)vector3i.z, (float)vector3i2.z);
		return new Vector2(num, num2);
	}

	// Token: 0x06005C3C RID: 23612 RVA: 0x0023F762 File Offset: 0x0023D962
	public void ObjectOnMapAdd(MapObject _mo)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.Add(_mo);
		}
	}

	// Token: 0x06005C3D RID: 23613 RVA: 0x0023F778 File Offset: 0x0023D978
	public void ObjectOnMapRemove(EnumMapObjectType _type, int _key)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.Remove(_type, _key);
		}
	}

	// Token: 0x06005C3E RID: 23614 RVA: 0x0023F78F File Offset: 0x0023D98F
	public void ObjectOnMapRemove(EnumMapObjectType _type, Vector3 _position)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.RemoveByPosition(_type, _position);
		}
	}

	// Token: 0x06005C3F RID: 23615 RVA: 0x0023F7A6 File Offset: 0x0023D9A6
	public void ObjectOnMapRemove(EnumMapObjectType _type)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.RemoveByType(_type);
		}
	}

	// Token: 0x06005C40 RID: 23616 RVA: 0x0023F7BC File Offset: 0x0023D9BC
	public List<MapObject> GetObjectOnMapList(EnumMapObjectType _type)
	{
		if (this.objectsOnMap != null)
		{
			return this.objectsOnMap.GetList(_type);
		}
		return new List<MapObject>();
	}

	// Token: 0x06005C41 RID: 23617 RVA: 0x0023F7D8 File Offset: 0x0023D9D8
	public void DebugAddSpawnedEntity(Entity entity)
	{
		if (this.GetPrimaryPlayer() == null || !(entity is EntityAlive))
		{
			return;
		}
		EntityAlive entityAlive = (EntityAlive)entity;
		SSpawnedEntity sspawnedEntity = default(SSpawnedEntity);
		sspawnedEntity.distanceToLocalPlayer = (entityAlive.GetPosition() - this.GetPrimaryPlayer().GetPosition()).magnitude;
		sspawnedEntity.name = entityAlive.EntityName;
		sspawnedEntity.pos = entityAlive.GetPosition();
		sspawnedEntity.timeSpawned = Time.time;
		this.Last4Spawned.Add(sspawnedEntity);
		if (this.Last4Spawned.Count > 4)
		{
			this.Last4Spawned.RemoveAt(0);
		}
	}

	// Token: 0x06005C42 RID: 23618 RVA: 0x0023F87C File Offset: 0x0023DA7C
	public static void SetWorldAreas(List<TraderArea> _traders)
	{
		World.traderAreas = _traders;
	}

	// Token: 0x06005C43 RID: 23619 RVA: 0x0023F884 File Offset: 0x0023DA84
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupTraders()
	{
		if (World.traderAreas != null)
		{
			DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator != null)
			{
				dynamicPrefabDecorator.ClearTraders();
				for (int i = 0; i < World.traderAreas.Count; i++)
				{
					dynamicPrefabDecorator.AddTrader(World.traderAreas[i]);
				}
			}
			World.traderAreas = null;
		}
	}

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06005C44 RID: 23620 RVA: 0x0023F8E0 File Offset: 0x0023DAE0
	public List<TraderArea> TraderAreas
	{
		get
		{
			DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator == null)
			{
				return null;
			}
			return dynamicPrefabDecorator.GetTraderAreas();
		}
	}

	// Token: 0x06005C45 RID: 23621 RVA: 0x0023F909 File Offset: 0x0023DB09
	public bool IsWithinTraderArea(Vector3i _worldBlockPos)
	{
		return this.GetTraderAreaAt(_worldBlockPos) != null;
	}

	// Token: 0x06005C46 RID: 23622 RVA: 0x0023F918 File Offset: 0x0023DB18
	public bool IsWithinTraderPlacingProtection(Vector3i _worldBlockPos)
	{
		if (World.SandboxUseTraderArea != TraderAreaStates.Default)
		{
			return false;
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		return dynamicPrefabDecorator != null && dynamicPrefabDecorator.GetTraderAtPosition(_worldBlockPos, 2) != null;
	}

	// Token: 0x06005C47 RID: 23623 RVA: 0x0023F950 File Offset: 0x0023DB50
	public bool IsWithinTraderPlacingProtection(Bounds _bounds)
	{
		if (World.SandboxUseTraderArea != TraderAreaStates.Default)
		{
			return false;
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator != null)
		{
			_bounds.Expand(4f);
			Vector3i vector3i = World.worldToBlockPos(_bounds.min);
			Vector3i vector3i2 = World.worldToBlockPos(_bounds.max);
			return dynamicPrefabDecorator.IsWithinTraderArea(vector3i, vector3i2);
		}
		return false;
	}

	// Token: 0x06005C48 RID: 23624 RVA: 0x0023F9AC File Offset: 0x0023DBAC
	public bool IsWithinTraderArea(Vector3i _minPos, Vector3i _maxPos)
	{
		if (World.SandboxUseTraderArea == TraderAreaStates.Claimable)
		{
			return false;
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		return dynamicPrefabDecorator != null && dynamicPrefabDecorator.IsWithinTraderArea(_minPos, _maxPos);
	}

	// Token: 0x06005C49 RID: 23625 RVA: 0x0023F9E4 File Offset: 0x0023DBE4
	public TraderArea GetTraderAreaAt(Vector3i _pos)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator != null)
		{
			return dynamicPrefabDecorator.GetTraderAtPosition(_pos, 0);
		}
		return null;
	}

	// Token: 0x06005C4A RID: 23626 RVA: 0x0023FA10 File Offset: 0x0023DC10
	public override int AddSleeperVolume(SleeperVolume _sleeperVolume)
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		int num2;
		lock (dictionary)
		{
			int num = this.nextSleeperVolumeId;
			VolumeKey volumeKey = new VolumeKey(_sleeperVolume);
			if (!this.sleeperVolumeMap.TryAdd(volumeKey, num))
			{
				Log.Error(string.Format("SleeperVolume already exists at {0}", volumeKey));
				num2 = -1;
			}
			else
			{
				this.sleeperVolumes.Add(num, _sleeperVolume);
				this.nextSleeperVolumeId++;
				num2 = num;
			}
		}
		return num2;
	}

	// Token: 0x06005C4B RID: 23627 RVA: 0x0023FAA4 File Offset: 0x0023DCA4
	public override int FindSleeperVolume(Vector3i mins, Vector3i maxs)
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		int num2;
		lock (dictionary)
		{
			int num;
			if (!this.sleeperVolumeMap.TryGetValue(new VolumeKey(mins, maxs), out num))
			{
				num2 = -1;
			}
			else
			{
				num2 = num;
			}
		}
		return num2;
	}

	// Token: 0x06005C4C RID: 23628 RVA: 0x0023FAFC File Offset: 0x0023DCFC
	public override SleeperVolume GetSleeperVolume(int index)
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		SleeperVolume sleeperVolume2;
		lock (dictionary)
		{
			SleeperVolume sleeperVolume;
			if (!this.sleeperVolumes.TryGetValue(index, out sleeperVolume))
			{
				throw new Exception(string.Format("SleeperVolume id {0} not found", index));
			}
			sleeperVolume2 = sleeperVolume;
		}
		return sleeperVolume2;
	}

	// Token: 0x06005C4D RID: 23629 RVA: 0x0023FB60 File Offset: 0x0023DD60
	public void GetAllSleeperVolumes(List<ValueTuple<int, SleeperVolume>> _volumes)
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		lock (dictionary)
		{
			foreach (KeyValuePair<int, SleeperVolume> keyValuePair in this.sleeperVolumes)
			{
				int num;
				SleeperVolume sleeperVolume;
				keyValuePair.Deconstruct(out num, out sleeperVolume);
				int num2 = num;
				SleeperVolume sleeperVolume2 = sleeperVolume;
				_volumes.Add(new ValueTuple<int, SleeperVolume>(num2, sleeperVolume2));
			}
		}
	}

	// Token: 0x06005C4E RID: 23630 RVA: 0x0023FBF4 File Offset: 0x0023DDF4
	public void CheckSleeperVolumeTouching(EntityPlayer _player)
	{
		if (!GameStats.GetBool(EnumGameStats.IsSpawnEnemies))
		{
			return;
		}
		Vector3i blockPosition = _player.GetBlockPosition();
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(blockPosition);
		if (chunk != null)
		{
			List<int> list = chunk.GetSleeperVolumes();
			Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
			lock (dictionary)
			{
				for (int i = 0; i < list.Count; i++)
				{
					int num = list[i];
					SleeperVolume sleeperVolume;
					if (this.sleeperVolumes.TryGetValue(num, out sleeperVolume))
					{
						sleeperVolume.CheckTouching(this, _player);
					}
				}
			}
		}
	}

	// Token: 0x06005C4F RID: 23631 RVA: 0x0023FC94 File Offset: 0x0023DE94
	public void CheckSleeperVolumeNoise(Vector3 position)
	{
		if (!GameStats.GetBool(EnumGameStats.IsSpawnEnemies))
		{
			return;
		}
		position.y += 0.1f;
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(World.worldToBlockPos(position));
		if (chunk != null)
		{
			List<int> list = chunk.GetSleeperVolumes();
			Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
			lock (dictionary)
			{
				for (int i = 0; i < list.Count; i++)
				{
					int num = list[i];
					SleeperVolume sleeperVolume;
					if (this.sleeperVolumes.TryGetValue(num, out sleeperVolume))
					{
						sleeperVolume.CheckNoise(this, position);
					}
				}
			}
		}
	}

	// Token: 0x06005C50 RID: 23632 RVA: 0x0023FD40 File Offset: 0x0023DF40
	public void WriteSleeperVolumes(BinaryWriter _bw)
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		lock (dictionary)
		{
			_bw.Write(this.sleeperVolumes.Count);
			foreach (KeyValuePair<int, SleeperVolume> keyValuePair in this.sleeperVolumes)
			{
				int num;
				SleeperVolume sleeperVolume;
				keyValuePair.Deconstruct(out num, out sleeperVolume);
				int num2 = num;
				SleeperVolume sleeperVolume2 = sleeperVolume;
				_bw.Write(num2);
				sleeperVolume2.Write(_bw);
			}
			_bw.Write(this.nextSleeperVolumeId);
		}
	}

	// Token: 0x06005C51 RID: 23633 RVA: 0x0023FDF0 File Offset: 0x0023DFF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadSleeperVolumes(int _version, BinaryReader _br)
	{
		this.sleeperVolumeMap.Clear();
		this.sleeperVolumes.Clear();
		int num = _br.ReadInt32();
		if (_version < 1)
		{
			for (int i = 0; i < num; i++)
			{
				SleeperVolume sleeperVolume = SleeperVolume.Read(_br);
				VolumeKey volumeKey = new VolumeKey(sleeperVolume);
				if (this.sleeperVolumeMap.ContainsKey(volumeKey))
				{
					Log.Error("ReadSleeperVolumes #{0} dup key ({1}) ({2}) {3}", new object[]
					{
						i,
						volumeKey.boxMin,
						volumeKey.boxMax,
						volumeKey.GetHashCode()
					});
				}
				else
				{
					this.sleeperVolumes.Add(i, sleeperVolume);
					this.sleeperVolumeMap.Add(volumeKey, i);
				}
			}
			this.nextSleeperVolumeId = num;
			return;
		}
		for (int j = 0; j < num; j++)
		{
			int num2 = _br.ReadInt32();
			SleeperVolume sleeperVolume2 = SleeperVolume.Read(_br);
			VolumeKey volumeKey2 = new VolumeKey(sleeperVolume2);
			if (this.sleeperVolumeMap.ContainsKey(volumeKey2))
			{
				Log.Error("ReadSleeperVolumes #{0} dup key ({1}) ({2}) {3}", new object[]
				{
					j,
					volumeKey2.boxMin,
					volumeKey2.boxMax,
					volumeKey2.GetHashCode()
				});
			}
			else
			{
				this.sleeperVolumes.Add(num2, sleeperVolume2);
				this.sleeperVolumeMap.Add(volumeKey2, num2);
			}
		}
		this.nextSleeperVolumeId = _br.ReadInt32();
	}

	// Token: 0x06005C52 RID: 23634 RVA: 0x0023FF78 File Offset: 0x0023E178
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupSleeperVolumes()
	{
		foreach (SleeperVolume sleeperVolume in this.sleeperVolumes.Values)
		{
			sleeperVolume.AddToPrefabInstance();
		}
	}

	// Token: 0x06005C53 RID: 23635 RVA: 0x0023FFD0 File Offset: 0x0023E1D0
	public void NotifySleeperVolumesEntityDied(EntityAlive entity)
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		lock (dictionary)
		{
			foreach (SleeperVolume sleeperVolume in this.sleeperVolumes.Values)
			{
				sleeperVolume.EntityDied(entity);
			}
		}
	}

	// Token: 0x06005C54 RID: 23636 RVA: 0x00240050 File Offset: 0x0023E250
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickSleeperVolumes()
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		lock (dictionary)
		{
			SleeperVolume.TickSpawnCount = 0;
			foreach (SleeperVolume sleeperVolume in this.sleeperVolumes.Values)
			{
				sleeperVolume.Tick(this);
			}
		}
	}

	// Token: 0x06005C55 RID: 23637 RVA: 0x002400D4 File Offset: 0x0023E2D4
	public void RemoveSleeperVolumesFor(PrefabInstance _prefabInstance)
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		lock (dictionary)
		{
			foreach (SleeperVolume sleeperVolume in _prefabInstance.sleeperVolumes)
			{
				sleeperVolume.DespawnAndReset(this);
				int num;
				if (this.sleeperVolumeMap.Remove(new VolumeKey(sleeperVolume), out num))
				{
					this.sleeperVolumes.Remove(num);
				}
			}
		}
	}

	// Token: 0x06005C56 RID: 23638 RVA: 0x00240174 File Offset: 0x0023E374
	public override int AddTriggerVolume(TriggerVolume _triggerVolume)
	{
		Dictionary<int, TriggerVolume> dictionary = this.triggerVolumes;
		int num2;
		lock (dictionary)
		{
			int num = this.nextTriggerVolumeId;
			VolumeKey volumeKey = new VolumeKey(_triggerVolume);
			if (!this.triggerVolumeMap.TryAdd(volumeKey, num))
			{
				Log.Error(string.Format("TriggerVolume already exists at {0}", volumeKey));
				num2 = -1;
			}
			else
			{
				this.triggerVolumes.Add(num, _triggerVolume);
				this.nextTriggerVolumeId++;
				num2 = num;
			}
		}
		return num2;
	}

	// Token: 0x06005C57 RID: 23639 RVA: 0x00240208 File Offset: 0x0023E408
	public override void ResetTriggerVolumes(long chunkKey)
	{
		Vector2i vector2i = WorldChunkCache.extractXZ(chunkKey);
		Bounds bounds = Chunk.CalculateAABB(vector2i.x, 0, vector2i.y);
		Dictionary<int, TriggerVolume> dictionary = this.triggerVolumes;
		lock (dictionary)
		{
			foreach (TriggerVolume triggerVolume in this.triggerVolumes.Values)
			{
				if (triggerVolume.Intersects(bounds))
				{
					triggerVolume.Reset();
				}
			}
		}
	}

	// Token: 0x06005C58 RID: 23640 RVA: 0x002402B0 File Offset: 0x0023E4B0
	public override void ResetSleeperVolumes(long chunkKey)
	{
		Vector2i vector2i = WorldChunkCache.extractXZ(chunkKey);
		Bounds bounds = Chunk.CalculateAABB(vector2i.x, 0, vector2i.y);
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		lock (dictionary)
		{
			foreach (SleeperVolume sleeperVolume in this.sleeperVolumes.Values)
			{
				if (sleeperVolume.Intersects(bounds))
				{
					sleeperVolume.DespawnAndReset(this);
				}
			}
		}
	}

	// Token: 0x06005C59 RID: 23641 RVA: 0x00240358 File Offset: 0x0023E558
	public void ResetSleeperVolumes()
	{
		Dictionary<int, SleeperVolume> dictionary = this.sleeperVolumes;
		lock (dictionary)
		{
			foreach (SleeperVolume sleeperVolume in this.sleeperVolumes.Values)
			{
				sleeperVolume.DespawnAndReset(this);
			}
		}
	}

	// Token: 0x06005C5A RID: 23642 RVA: 0x002403D8 File Offset: 0x0023E5D8
	public override int FindTriggerVolume(Vector3i mins, Vector3i maxs)
	{
		Dictionary<int, TriggerVolume> dictionary = this.triggerVolumes;
		int num2;
		lock (dictionary)
		{
			int num;
			if (!this.triggerVolumeMap.TryGetValue(new VolumeKey(mins, maxs), out num))
			{
				num2 = -1;
			}
			else
			{
				num2 = num;
			}
		}
		return num2;
	}

	// Token: 0x06005C5B RID: 23643 RVA: 0x00240430 File Offset: 0x0023E630
	public override TriggerVolume GetTriggerVolume(int index)
	{
		Dictionary<int, TriggerVolume> dictionary = this.triggerVolumes;
		TriggerVolume triggerVolume2;
		lock (dictionary)
		{
			TriggerVolume triggerVolume;
			if (!this.triggerVolumes.TryGetValue(index, out triggerVolume))
			{
				throw new Exception(string.Format("TriggerVolume id {0} not found", index));
			}
			triggerVolume2 = triggerVolume;
		}
		return triggerVolume2;
	}

	// Token: 0x06005C5C RID: 23644 RVA: 0x00240494 File Offset: 0x0023E694
	public void CheckTriggerVolumeTrigger(EntityPlayer _player)
	{
		Vector3i blockPosition = _player.GetBlockPosition();
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(blockPosition);
		if (chunk != null)
		{
			List<int> list = chunk.GetTriggerVolumes();
			Dictionary<int, TriggerVolume> dictionary = this.triggerVolumes;
			lock (dictionary)
			{
				for (int i = 0; i < list.Count; i++)
				{
					int num = list[i];
					TriggerVolume triggerVolume;
					if (this.triggerVolumes.TryGetValue(num, out triggerVolume))
					{
						triggerVolume.CheckTouching(this, _player);
					}
				}
			}
		}
	}

	// Token: 0x06005C5D RID: 23645 RVA: 0x00240528 File Offset: 0x0023E728
	public void WriteTriggerVolumes(BinaryWriter _bw)
	{
		Dictionary<int, TriggerVolume> dictionary = this.triggerVolumes;
		lock (dictionary)
		{
			_bw.Write(this.triggerVolumes.Count);
			foreach (KeyValuePair<int, TriggerVolume> keyValuePair in this.triggerVolumes)
			{
				int num;
				TriggerVolume triggerVolume;
				keyValuePair.Deconstruct(out num, out triggerVolume);
				int num2 = num;
				TriggerVolume triggerVolume2 = triggerVolume;
				_bw.Write(num2);
				triggerVolume2.Write(_bw);
			}
			_bw.Write(this.nextTriggerVolumeId);
		}
	}

	// Token: 0x06005C5E RID: 23646 RVA: 0x002405D8 File Offset: 0x0023E7D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadTriggerVolumes(int _version, BinaryReader _br)
	{
		this.triggerVolumes.Clear();
		this.triggerVolumeMap.Clear();
		int num = _br.ReadInt32();
		if (_version < 1)
		{
			for (int i = 0; i < num; i++)
			{
				TriggerVolume triggerVolume = TriggerVolume.Read(_br);
				VolumeKey volumeKey = new VolumeKey(triggerVolume);
				if (this.triggerVolumeMap.ContainsKey(volumeKey))
				{
					Log.Error("ReadTriggerVolumes #{0} dup key ({1}) ({2}) {3}", new object[]
					{
						i,
						volumeKey.boxMin,
						volumeKey.boxMax,
						volumeKey.GetHashCode()
					});
				}
				else
				{
					this.triggerVolumeMap.Add(volumeKey, i);
					this.triggerVolumes.Add(i, triggerVolume);
				}
			}
			this.nextTriggerVolumeId = num;
			return;
		}
		for (int j = 0; j < num; j++)
		{
			int num2 = _br.ReadInt32();
			TriggerVolume triggerVolume2 = TriggerVolume.Read(_br);
			VolumeKey volumeKey2 = new VolumeKey(triggerVolume2);
			if (this.triggerVolumeMap.ContainsKey(volumeKey2))
			{
				Log.Error("ReadTriggerVolumes #{0} dup key ({1}) ({2}) {3}", new object[]
				{
					j,
					volumeKey2.boxMin,
					volumeKey2.boxMax,
					volumeKey2.GetHashCode()
				});
			}
			else
			{
				this.triggerVolumes.Add(num2, triggerVolume2);
				this.triggerVolumeMap.Add(volumeKey2, num2);
			}
		}
		this.nextTriggerVolumeId = _br.ReadInt32();
	}

	// Token: 0x06005C5F RID: 23647 RVA: 0x00240760 File Offset: 0x0023E960
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupTriggerVolumes()
	{
		foreach (TriggerVolume triggerVolume in this.triggerVolumes.Values)
		{
			triggerVolume.AddToPrefabInstance();
		}
	}

	// Token: 0x06005C60 RID: 23648 RVA: 0x002407B8 File Offset: 0x0023E9B8
	public void RemoveTriggerVolumesFor(PrefabInstance prefabInstance)
	{
		this.triggerManager.RemoveFromUpdateList(prefabInstance);
		Dictionary<int, TriggerVolume> dictionary = this.triggerVolumes;
		lock (dictionary)
		{
			foreach (TriggerVolume triggerVolume in prefabInstance.triggerVolumes)
			{
				int num;
				if (this.triggerVolumeMap.Remove(new VolumeKey(triggerVolume), out num))
				{
					this.triggerVolumes.Remove(num);
				}
			}
		}
	}

	// Token: 0x06005C61 RID: 23649 RVA: 0x0024085C File Offset: 0x0023EA5C
	public override int AddWallVolume(WallVolume _wallVolume)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		int num2;
		lock (dictionary)
		{
			int num = this.nextWallVolumeId;
			VolumeKey volumeKey = new VolumeKey(_wallVolume);
			if (!this.wallVolumeMap.TryAdd(volumeKey, num))
			{
				Log.Error(string.Format("WallVolume already exists at {0}", volumeKey));
				num2 = -1;
			}
			else
			{
				this.wallVolumes.Add(num, _wallVolume);
				this.nextWallVolumeId++;
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					NetPackageWallVolume package = NetPackageManager.GetPackage<NetPackageWallVolume>();
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package.Setup(num, _wallVolume), false, -1, -1, -1, null, 192, false);
				}
				num2 = num;
			}
		}
		return num2;
	}

	// Token: 0x06005C62 RID: 23650 RVA: 0x00240928 File Offset: 0x0023EB28
	public void AddWallVolumeAt(int _index, WallVolume _wallVolume)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		lock (dictionary)
		{
			if (this.wallVolumes.ContainsKey(_index))
			{
				Log.Error(string.Format("WallVolume already exists with id {0}", _index));
			}
			else
			{
				VolumeKey volumeKey = new VolumeKey(_wallVolume);
				if (this.wallVolumeMap.ContainsKey(volumeKey))
				{
					Log.Error(string.Format("WallVolume already exists at {0}", volumeKey));
				}
				else
				{
					this.wallVolumes.Add(_index, _wallVolume);
					this.wallVolumeMap.Add(volumeKey, _index);
				}
			}
		}
	}

	// Token: 0x06005C63 RID: 23651 RVA: 0x002409D0 File Offset: 0x0023EBD0
	public void RemoveWallVolumeAt(int _index)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		lock (dictionary)
		{
			WallVolume wallVolume;
			if (!this.wallVolumes.Remove(_index, out wallVolume))
			{
				Log.Error(string.Format("RemoveWallVolume invalid index {0}", _index));
			}
			else
			{
				this.wallVolumeMap.Remove(new VolumeKey(wallVolume));
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					NetPackageWallVolumeRemove package = NetPackageManager.GetPackage<NetPackageWallVolumeRemove>();
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package.Setup(_index), false, -1, -1, -1, null, 192, false);
				}
			}
		}
	}

	// Token: 0x06005C64 RID: 23652 RVA: 0x00240A7C File Offset: 0x0023EC7C
	public override int FindWallVolume(Vector3i mins, Vector3i maxs)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		int num2;
		lock (dictionary)
		{
			int num;
			if (!this.wallVolumeMap.TryGetValue(new VolumeKey(mins, maxs), out num))
			{
				num2 = -1;
			}
			else
			{
				num2 = num;
			}
		}
		return num2;
	}

	// Token: 0x06005C65 RID: 23653 RVA: 0x00240AD4 File Offset: 0x0023ECD4
	public override WallVolume GetWallVolume(int index)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		WallVolume wallVolume2;
		lock (dictionary)
		{
			WallVolume wallVolume;
			if (!this.wallVolumes.TryGetValue(index, out wallVolume))
			{
				throw new Exception(string.Format("WallVolume id {0} not found", index));
			}
			wallVolume2 = wallVolume;
		}
		return wallVolume2;
	}

	// Token: 0x06005C66 RID: 23654 RVA: 0x00240B38 File Offset: 0x0023ED38
	public override List<ValueTuple<int, WallVolume>> GetAllWallVolumes()
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		List<ValueTuple<int, WallVolume>> list2;
		lock (dictionary)
		{
			List<ValueTuple<int, WallVolume>> list = new List<ValueTuple<int, WallVolume>>();
			foreach (KeyValuePair<int, WallVolume> keyValuePair in this.wallVolumes)
			{
				int num;
				WallVolume wallVolume;
				keyValuePair.Deconstruct(out num, out wallVolume);
				int num2 = num;
				WallVolume wallVolume2 = wallVolume;
				list.Add(new ValueTuple<int, WallVolume>(num2, wallVolume2));
			}
			list2 = list;
		}
		return list2;
	}

	// Token: 0x06005C67 RID: 23655 RVA: 0x00240BDC File Offset: 0x0023EDDC
	public override bool HasWallVolumes(List<int> _wallVolumeIds)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		bool flag2;
		lock (dictionary)
		{
			foreach (int num in _wallVolumeIds)
			{
				if (!this.wallVolumes.ContainsKey(num))
				{
					return false;
				}
			}
			flag2 = true;
		}
		return flag2;
	}

	// Token: 0x06005C68 RID: 23656 RVA: 0x00240C64 File Offset: 0x0023EE64
	public void WriteWallVolumes(BinaryWriter _bw)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		lock (dictionary)
		{
			_bw.Write(this.wallVolumes.Count);
			foreach (KeyValuePair<int, WallVolume> keyValuePair in this.wallVolumes)
			{
				int num;
				WallVolume wallVolume;
				keyValuePair.Deconstruct(out num, out wallVolume);
				int num2 = num;
				WallVolume wallVolume2 = wallVolume;
				_bw.Write(num2);
				wallVolume2.Write(_bw);
			}
			_bw.Write(this.nextWallVolumeId);
		}
	}

	// Token: 0x06005C69 RID: 23657 RVA: 0x00240D14 File Offset: 0x0023EF14
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadWallVolumes(int _version, BinaryReader _br)
	{
		this.wallVolumes.Clear();
		this.wallVolumeMap.Clear();
		int num = _br.ReadInt32();
		if (_version < 1)
		{
			for (int i = 0; i < num; i++)
			{
				WallVolume wallVolume = WallVolume.Read(_br);
				VolumeKey volumeKey = new VolumeKey(wallVolume);
				if (this.wallVolumeMap.ContainsKey(volumeKey))
				{
					Log.Error("ReadWallVolumes #{0} dup key ({1}) ({2}) {3}", new object[]
					{
						i,
						volumeKey.boxMin,
						volumeKey.boxMax,
						volumeKey.GetHashCode()
					});
				}
				else
				{
					this.wallVolumes.Add(i, wallVolume);
					this.wallVolumeMap.Add(volumeKey, i);
				}
			}
			this.nextWallVolumeId = num;
			return;
		}
		for (int j = 0; j < num; j++)
		{
			int num2 = _br.ReadInt32();
			WallVolume wallVolume2 = WallVolume.Read(_br);
			VolumeKey volumeKey2 = new VolumeKey(wallVolume2);
			if (this.wallVolumeMap.ContainsKey(volumeKey2))
			{
				Log.Error("ReadWallVolumes #{0} dup key ({1}) ({2}) {3}", new object[]
				{
					j,
					volumeKey2.boxMin,
					volumeKey2.boxMax,
					volumeKey2.GetHashCode()
				});
			}
			else
			{
				this.wallVolumes.Add(num2, wallVolume2);
				this.wallVolumeMap.Add(volumeKey2, num2);
			}
		}
		this.nextWallVolumeId = _br.ReadInt32();
	}

	// Token: 0x06005C6A RID: 23658 RVA: 0x00240E9C File Offset: 0x0023F09C
	public void SetWallVolumesForClient(List<ValueTuple<int, WallVolume>> wallVolumeData)
	{
		this.wallVolumes.Clear();
		this.wallVolumeMap.Clear();
		foreach (ValueTuple<int, WallVolume> valueTuple in wallVolumeData)
		{
			int item = valueTuple.Item1;
			WallVolume item2 = valueTuple.Item2;
			this.wallVolumes.Add(item, item2);
			this.wallVolumeMap.Add(new VolumeKey(item2), item);
		}
	}

	// Token: 0x06005C6B RID: 23659 RVA: 0x00240F24 File Offset: 0x0023F124
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupWallVolumes()
	{
		foreach (WallVolume wallVolume in this.wallVolumes.Values)
		{
			wallVolume.AddToPrefabInstance();
		}
	}

	// Token: 0x06005C6C RID: 23660 RVA: 0x00240F7C File Offset: 0x0023F17C
	public void RemoveWallVolumesFor(PrefabInstance prefabInstance)
	{
		Dictionary<int, WallVolume> dictionary = this.wallVolumes;
		lock (dictionary)
		{
			foreach (WallVolume wallVolume in prefabInstance.wallVolumes)
			{
				int num;
				if (this.wallVolumeMap.Remove(new VolumeKey(wallVolume), out num))
				{
					this.wallVolumes.Remove(num);
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						NetPackageWallVolumeRemove package = NetPackageManager.GetPackage<NetPackageWallVolumeRemove>();
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package.Setup(num), false, -1, -1, -1, null, 192, false);
					}
				}
			}
		}
	}

	// Token: 0x06005C6D RID: 23661 RVA: 0x0024104C File Offset: 0x0023F24C
	public void AddBlockData(Vector3i v3i, object bd)
	{
		this.blockData.Add(v3i, bd);
	}

	// Token: 0x06005C6E RID: 23662 RVA: 0x0024105C File Offset: 0x0023F25C
	public object GetBlockData(Vector3i v3i)
	{
		object obj;
		if (!this.blockData.TryGetValue(v3i, out obj))
		{
			obj = null;
		}
		return obj;
	}

	// Token: 0x06005C6F RID: 23663 RVA: 0x0024107C File Offset: 0x0023F27C
	public void ClearBlockData(Vector3i v3i)
	{
		this.blockData.Remove(v3i);
	}

	// Token: 0x06005C70 RID: 23664 RVA: 0x0024108B File Offset: 0x0023F28B
	public void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _bStopStabilityUpdate, bool _bRegenerateChunk, bool _bFillEmptyBlocks, bool _isReset = false)
	{
		this.ChunkCache.ChunkProvider.RebuildTerrain(_chunks, _areaStart, _areaSize, _bStopStabilityUpdate, _bRegenerateChunk, _bFillEmptyBlocks, _isReset);
	}

	// Token: 0x06005C71 RID: 23665 RVA: 0x002410A8 File Offset: 0x0023F2A8
	public override GameRandom GetGameRandom()
	{
		return this.rand;
	}

	// Token: 0x06005C72 RID: 23666 RVA: 0x002410B0 File Offset: 0x0023F2B0
	public float RandomRange(float _min, float _max)
	{
		return this.rand.RandomFloat * (_max - _min) + _min;
	}

	// Token: 0x06005C73 RID: 23667 RVA: 0x002410C4 File Offset: 0x0023F2C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void DuskDawnInit()
	{
		ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
		this.DuskHour = valueTuple.Item1;
		this.DawnHour = valueTuple.Item2;
	}

	// Token: 0x06005C74 RID: 23668 RVA: 0x002410F6 File Offset: 0x0023F2F6
	public void SetTime(ulong _time)
	{
		this.worldTime = _time;
		if (this.m_WorldEnvironment)
		{
			this.m_WorldEnvironment.WorldTimeChanged();
		}
	}

	// Token: 0x06005C75 RID: 23669 RVA: 0x00241117 File Offset: 0x0023F317
	public void SetTimeJump(ulong _time, bool _isSeek = false)
	{
		this.SetTime(_time);
		SkyManager.bUpdateSunMoonNow = true;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.aiDirector.BloodMoonComponent.TimeChanged(_isSeek);
		}
	}

	// Token: 0x06005C76 RID: 23670 RVA: 0x00241143 File Offset: 0x0023F343
	public bool IsWorldEvent(World.WorldEvent _event)
	{
		return _event == World.WorldEvent.BloodMoon && this.isEventBloodMoon;
	}

	// Token: 0x06005C77 RID: 23671 RVA: 0x00241150 File Offset: 0x0023F350
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldEventUpdateTime()
	{
		this.WorldDay = GameUtils.WorldTimeToDays(this.worldTime);
		this.WorldHour = GameUtils.WorldTimeToHours(this.worldTime);
		int num = GameUtils.WorldTimeToDays(this.eventWorldTime);
		int num2 = GameUtils.WorldTimeToHours(this.eventWorldTime);
		if (num == this.WorldDay && num2 == this.WorldHour)
		{
			return;
		}
		this.eventWorldTime = this.worldTime;
		ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
		int item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		bool flag = this.isEventBloodMoon;
		this.isEventBloodMoon = false;
		int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
		if (this.WorldDay == @int)
		{
			if (this.WorldHour >= item)
			{
				this.isEventBloodMoon = true;
			}
		}
		else if (this.WorldDay > 1 && this.WorldDay == @int + 1 && this.WorldHour < item2)
		{
			this.isEventBloodMoon = true;
		}
		if (flag != this.isEventBloodMoon)
		{
			EntityPlayerLocal primaryPlayer = this.GetPrimaryPlayer();
			if (primaryPlayer)
			{
				if (this.isEventBloodMoon && this.WorldHour == item)
				{
					primaryPlayer.BloodMoonParticipation = true;
					return;
				}
				if (!this.isEventBloodMoon && this.WorldHour == item2 && primaryPlayer.BloodMoonParticipation)
				{
					QuestEventManager.Current.BloodMoonSurvived();
					primaryPlayer.BloodMoonParticipation = false;
				}
			}
		}
	}

	// Token: 0x06005C78 RID: 23672 RVA: 0x00241280 File Offset: 0x0023F480
	public override void AddPendingDowngradeBlock(BlockValueRef _bvRef)
	{
		this.pendingUpgradeDowngradeBlocks.Add(_bvRef);
	}

	// Token: 0x06005C79 RID: 23673 RVA: 0x0024128F File Offset: 0x0023F48F
	public override bool TryRetrieveAndRemovePendingDowngradeBlock(BlockValueRef _bvRef)
	{
		if (this.pendingUpgradeDowngradeBlocks.Contains(_bvRef))
		{
			this.pendingUpgradeDowngradeBlocks.Remove(_bvRef);
			return true;
		}
		return false;
	}

	// Token: 0x06005C7A RID: 23674 RVA: 0x002412AF File Offset: 0x0023F4AF
	public IEnumerator ResetPOIS(List<PrefabInstance> prefabInstances, FastTags<TagGroup.Global> questTags, int entityID, int[] sharedWith, QuestClass questClass)
	{
		long playerChunkKey = 0L;
		if (entityID != -1)
		{
			EntityPlayer entityPlayer = this.GetEntity(entityID) as EntityPlayer;
			if (entityPlayer != null)
			{
				Chunk chunk = base.GetChunkFromWorldPos(entityPlayer.GetBlockPosition()) as Chunk;
				if (chunk != null)
				{
					playerChunkKey = chunk.Key;
				}
			}
		}
		int num3;
		for (int i = 0; i < prefabInstances.Count; i = num3 + 1)
		{
			PrefabInstance prefabInstance = prefabInstances[i];
			Log.Out(string.Format("ResetPOIS {0}. Pos: {1}, Rot: {2}", prefabInstance.name, prefabInstance.boundingBoxPosition, prefabInstance.rotation));
			Prefab prefab = prefabInstance.prefab;
			this.triggerManager.RemoveFromUpdateList(prefabInstance);
			prefabInstance.LastQuestClass = questClass;
			yield return prefabInstance.ResetBlocksAndRebuild(this, questTags, playerChunkKey);
			for (int j = 0; j < prefab.SleeperVolumeList.Count; j++)
			{
				PrefabSleeperVolume prefabSleeperVolume = prefab.SleeperVolumeList[j];
				Vector3i startPos = prefabSleeperVolume.startPos;
				Vector3i size = prefabSleeperVolume.size;
				int num = GameManager.Instance.World.FindSleeperVolume(prefabInstance.boundingBoxPosition + startPos, prefabInstance.boundingBoxPosition + startPos + size);
				if (num != -1)
				{
					this.GetSleeperVolume(num).DespawnAndReset(this);
				}
			}
			for (int k = 0; k < prefab.TriggerVolumeList.Count; k++)
			{
				PrefabTriggerVolume prefabTriggerVolume = prefab.TriggerVolumeList[k];
				Vector3i startPos2 = prefabTriggerVolume.startPos;
				Vector3i size2 = prefabTriggerVolume.size;
				int num2 = GameManager.Instance.World.FindTriggerVolume(prefabInstance.boundingBoxPosition + startPos2, prefabInstance.boundingBoxPosition + startPos2 + size2);
				if (num2 != -1)
				{
					this.GetTriggerVolume(num2).Reset();
				}
			}
			this.triggerManager.RefreshTriggers(prefabInstance, questTags);
			if (!prefabInstance.bPrefabCopiedIntoWorld)
			{
				Log.Error("ResetPOIS " + prefabInstance.name + " was not copied into world");
			}
			prefabInstance = null;
			prefab = null;
			num3 = i;
		}
		yield break;
	}

	// Token: 0x06005C7B RID: 23675 RVA: 0x002412DC File Offset: 0x0023F4DC
	public bool IsRandomWorld()
	{
		bool flag = false;
		if (this.ChunkCache.ChunkProvider != null && this.ChunkCache.ChunkProvider.WorldInfo != null)
		{
			flag = this.ChunkCache.ChunkProvider.WorldInfo.RandomGeneratedWorld;
		}
		return flag;
	}

	// Token: 0x04004700 RID: 18176
	public const int cCollisionBlocks = 5;

	// Token: 0x04004703 RID: 18179
	public ulong worldTime;

	// Token: 0x04004704 RID: 18180
	public int DawnHour;

	// Token: 0x04004705 RID: 18181
	public int DuskHour;

	// Token: 0x04004706 RID: 18182
	public float Gravity = 0.08f;

	// Token: 0x04004707 RID: 18183
	public DictionaryList<int, Entity> Entities = new DictionaryList<int, Entity>();

	// Token: 0x04004708 RID: 18184
	public DictionaryList<int, EntityPlayer> Players = new DictionaryList<int, EntityPlayer>();

	// Token: 0x04004709 RID: 18185
	public List<EntityAlive> EntityAlives = new List<EntityAlive>();

	// Token: 0x0400470A RID: 18186
	public EntityAsyncManager entityAsyncManager;

	// Token: 0x0400470B RID: 18187
	public NetEntityDistribution entityDistributer;

	// Token: 0x0400470C RID: 18188
	public NetEntityPackageQueue netEntityPackageQueue;

	// Token: 0x0400470D RID: 18189
	public AIDirector aiDirector;

	// Token: 0x0400470E RID: 18190
	public Manager audioManager;

	// Token: 0x0400470F RID: 18191
	public Conductor dmsConductor;

	// Token: 0x04004710 RID: 18192
	public IGameManager gameManager;

	// Token: 0x04004711 RID: 18193
	public int Seed;

	// Token: 0x04004712 RID: 18194
	public WorldBiomes Biomes;

	// Token: 0x04004713 RID: 18195
	public SpawnManagerBiomes biomeSpawnManager;

	// Token: 0x04004714 RID: 18196
	public BiomeIntensity LocalPlayerBiomeIntensityStandingOn = BiomeIntensity.Default;

	// Token: 0x04004715 RID: 18197
	public WorldCreationData wcd;

	// Token: 0x04004716 RID: 18198
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldState worldState;

	// Token: 0x04004717 RID: 18199
	public ChunkManager m_ChunkManager;

	// Token: 0x04004718 RID: 18200
	public SharedChunkObserverCache m_SharedChunkObserverCache;

	// Token: 0x04004719 RID: 18201
	public readonly PrefabCache m_PrefabCache = new PrefabCache();

	// Token: 0x0400471A RID: 18202
	public EventPrefabsClient m_EventPrefabsClient;

	// Token: 0x0400471B RID: 18203
	public WorldEnvironment m_WorldEnvironment;

	// Token: 0x0400471C RID: 18204
	public BiomeAtmosphereEffects BiomeAtmosphereEffects;

	// Token: 0x0400471D RID: 18205
	public FlatAreaManager FlatAreaManager;

	// Token: 0x0400471E RID: 18206
	public static bool IsSplatMapAvailable;

	// Token: 0x0400471F RID: 18207
	public List<SSpawnedEntity> Last4Spawned = new List<SSpawnedEntity>();

	// Token: 0x04004720 RID: 18208
	public int playerEntityUpdateCount;

	// Token: 0x04004721 RID: 18209
	public int clientLastEntityId;

	// Token: 0x04004722 RID: 18210
	public Transform EntitiesTransform;

	// Token: 0x04004723 RID: 18211
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom rand;

	// Token: 0x04004724 RID: 18212
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUnityTerrainConfigured;

	// Token: 0x04004725 RID: 18213
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayerLocal> m_LocalPlayerEntities = new List<EntityPlayerLocal>();

	// Token: 0x04004726 RID: 18214
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal m_LocalPlayerEntity;

	// Token: 0x04004727 RID: 18215
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> entitiesWithinAABBExcludingEntity = new List<Entity>();

	// Token: 0x04004728 RID: 18216
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> livingEntitiesWithinAABBExcludingEntity = new List<EntityAlive>();

	// Token: 0x04004729 RID: 18217
	[PublicizedFrom(EAccessModifier.Private)]
	public MapObjectManager objectsOnMap;

	// Token: 0x0400472A RID: 18218
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnManagerDynamic dynamicSpawnManager;

	// Token: 0x0400472B RID: 18219
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBlockTicker worldBlockTicker;

	// Token: 0x0400472C RID: 18220
	public const int SleeperVolumeWorldStateSaveVersion = 1;

	// Token: 0x0400472D RID: 18221
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextSleeperVolumeId;

	// Token: 0x0400472E RID: 18222
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, SleeperVolume> sleeperVolumes = new Dictionary<int, SleeperVolume>();

	// Token: 0x0400472F RID: 18223
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<VolumeKey, int> sleeperVolumeMap = new Dictionary<VolumeKey, int>();

	// Token: 0x04004730 RID: 18224
	public const int TriggerVolumeWorldStateSaveVersion = 1;

	// Token: 0x04004731 RID: 18225
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextTriggerVolumeId;

	// Token: 0x04004732 RID: 18226
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, TriggerVolume> triggerVolumes = new Dictionary<int, TriggerVolume>();

	// Token: 0x04004733 RID: 18227
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<VolumeKey, int> triggerVolumeMap = new Dictionary<VolumeKey, int>();

	// Token: 0x04004734 RID: 18228
	public const int WallVolumeWorldStateSaveVersion = 1;

	// Token: 0x04004735 RID: 18229
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextWallVolumeId;

	// Token: 0x04004736 RID: 18230
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, WallVolume> wallVolumes = new Dictionary<int, WallVolume>();

	// Token: 0x04004737 RID: 18231
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<VolumeKey, int> wallVolumeMap = new Dictionary<VolumeKey, int>();

	// Token: 0x04004738 RID: 18232
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector3i, object> blockData = new Dictionary<Vector3i, object>();

	// Token: 0x04004739 RID: 18233
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> newlyLoadedChunksThisUpdate = new List<long>();

	// Token: 0x0400473A RID: 18234
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, ulong> areaMasterChunksToLock = new Dictionary<long, ulong>();

	// Token: 0x0400473B RID: 18235
	public TriggerManager triggerManager;

	// Token: 0x0400473C RID: 18236
	public static bool BiomeProgressionEnabled = true;

	// Token: 0x0400473D RID: 18237
	public static bool TemperatureSurvival = true;

	// Token: 0x0400473E RID: 18238
	public static bool MapEnabled = true;

	// Token: 0x0400473F RID: 18239
	public static int BloodMoonWarningHour = 8;

	// Token: 0x04004740 RID: 18240
	public static float StormFrequency = 1f;

	// Token: 0x04004741 RID: 18241
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[][] supportOrder = new int[][]
	{
		new int[] { 0, 7, 1, 6, 2, 4, 3, 5 },
		new int[] { 0, 2, 1, 7, 3, 4, 6, 5 },
		new int[] { 2, 1, 3, 0, 4, 6, 5, 7 },
		new int[] { 2, 4, 3, 1, 5, 6, 0, 7 },
		new int[] { 4, 3, 5, 2, 6, 0, 7, 1 },
		new int[] { 4, 6, 5, 3, 7, 0, 2, 1 },
		new int[] { 6, 5, 7, 4, 0, 2, 1, 3 },
		new int[] { 6, 0, 7, 5, 1, 2, 4, 3 }
	};

	// Token: 0x04004742 RID: 18242
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] supportOffsets = new int[]
	{
		0, 1, 1, 1, 1, 0, 1, -1, 0, -1,
		-1, -1, -1, 0, -1, 1
	};

	// Token: 0x04004743 RID: 18243
	[PublicizedFrom(EAccessModifier.Private)]
	public MicroStopwatch msUnculling = new MicroStopwatch();

	// Token: 0x04004744 RID: 18244
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<Chunk> chunksToUncull = new HashSetList<Chunk>();

	// Token: 0x04004745 RID: 18245
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<Chunk> chunksToRegenerate = new HashSetList<Chunk>();

	// Token: 0x04004746 RID: 18246
	[PublicizedFrom(EAccessModifier.Private)]
	public World.ClipBlock[] _clipBlocks = new World.ClipBlock[32];

	// Token: 0x04004747 RID: 18247
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds[] _clipBounds = new Bounds[16];

	// Token: 0x04004748 RID: 18248
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCollCacheSize = 50;

	// Token: 0x04004749 RID: 18249
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue[,,] collBlockCache = new BlockValue[50, 50, 50];

	// Token: 0x0400474A RID: 18250
	[PublicizedFrom(EAccessModifier.Private)]
	public sbyte[,,] collDensityCache = new sbyte[50, 50, 50];

	// Token: 0x0400474B RID: 18251
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickEntityFrameCount;

	// Token: 0x0400474C RID: 18252
	[PublicizedFrom(EAccessModifier.Private)]
	public float tickEntityFrameCountAverage = 1f;

	// Token: 0x0400474D RID: 18253
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> tickEntityList = new List<Entity>();

	// Token: 0x0400474E RID: 18254
	[PublicizedFrom(EAccessModifier.Private)]
	public float tickEntityPartialTicks;

	// Token: 0x0400474F RID: 18255
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickEntityIndex;

	// Token: 0x04004750 RID: 18256
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickEntitySliceCount;

	// Token: 0x04004751 RID: 18257
	[PublicizedFrom(EAccessModifier.Protected)]
	public Queue<Vector3i> fallingBlocks = new Queue<Vector3i>();

	// Token: 0x04004752 RID: 18258
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<Vector3i> fallingBlockSet = new HashSet<Vector3i>();

	// Token: 0x04004753 RID: 18259
	[PublicizedFrom(EAccessModifier.Protected)]
	public Queue<List<Vector3i>> fallingGroups = new Queue<List<Vector3i>>();

	// Token: 0x04004754 RID: 18260
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<Vector3i> groupedBlocks = new HashSet<Vector3i>();

	// Token: 0x04004755 RID: 18261
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<Vector3i> processedBlocks = new HashSet<Vector3i>();

	// Token: 0x04004756 RID: 18262
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> resetTempPositions = new List<Vector3i>();

	// Token: 0x04004757 RID: 18263
	[PublicizedFrom(EAccessModifier.Private)]
	public List<List<Vector3i>> resetTempGroups = new List<List<Vector3i>>();

	// Token: 0x04004758 RID: 18264
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Chunk> m_lpChunkList = new List<Chunk>();

	// Token: 0x04004759 RID: 18265
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWorldNavExtent = 2900;

	// Token: 0x0400475A RID: 18266
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWorldRWGBorder = 90;

	// Token: 0x0400475B RID: 18267
	public const float cEdgeHard = 50f;

	// Token: 0x0400475C RID: 18268
	public const float cEdgeSoft = 80f;

	// Token: 0x0400475D RID: 18269
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cEdgeMinWorldSize = 1024;

	// Token: 0x0400475E RID: 18270
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<TraderArea> traderAreas;

	// Token: 0x0400475F RID: 18271
	public static TraderAreaStates SandboxUseTraderArea = TraderAreaStates.Default;

	// Token: 0x04004760 RID: 18272
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTraderPlacingProtection = 2;

	// Token: 0x04004761 RID: 18273
	public bool isEventBloodMoon;

	// Token: 0x04004762 RID: 18274
	public ulong eventWorldTime;

	// Token: 0x04004763 RID: 18275
	public int WorldDay;

	// Token: 0x04004764 RID: 18276
	public int WorldHour;

	// Token: 0x04004765 RID: 18277
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<BlockValueRef> pendingUpgradeDowngradeBlocks = new HashSet<BlockValueRef>();

	// Token: 0x02000BDA RID: 3034
	// (Invoke) Token: 0x06005C7E RID: 23678
	public delegate void OnEntityLoadedDelegate(Entity _entity);

	// Token: 0x02000BDB RID: 3035
	// (Invoke) Token: 0x06005C82 RID: 23682
	public delegate void OnEntityUnloadedDelegate(Entity _entity, EnumRemoveEntityReason _reason);

	// Token: 0x02000BDC RID: 3036
	[PublicizedFrom(EAccessModifier.Private)]
	public class ClipBlock
	{
		// Token: 0x06005C85 RID: 23685 RVA: 0x0024141B File Offset: 0x0023F61B
		public static void ResetStorage()
		{
			World.ClipBlock._storageIndex = 0;
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x00241424 File Offset: 0x0023F624
		public static World.ClipBlock New(BlockValue _value, Block _block, float _yDistort, Vector3 _blockPos, Bounds _bounds)
		{
			World.ClipBlock clipBlock = World.ClipBlock._storage[World.ClipBlock._storageIndex];
			if (clipBlock == null)
			{
				clipBlock = new World.ClipBlock();
				World.ClipBlock._storage[World.ClipBlock._storageIndex] = clipBlock;
			}
			clipBlock.Init(_value, _block, _yDistort, _blockPos, _bounds);
			World.ClipBlock._storageIndex++;
			return clipBlock;
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x0024146C File Offset: 0x0023F66C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Init(BlockValue _value, Block _block, float _yDistort, Vector3 _blockPos, Bounds _bounds)
		{
			this.value = _value;
			this.block = _block;
			this.pos = _blockPos;
			Bounds bounds = _bounds;
			bounds.center -= _blockPos;
			bounds.min -= new Vector3(0f, _yDistort, 0f);
			this.bmins = bounds.min;
			this.bmaxs = bounds.max;
		}

		// Token: 0x04004766 RID: 18278
		public const int kMaxBlocks = 32;

		// Token: 0x04004767 RID: 18279
		public BlockValue value;

		// Token: 0x04004768 RID: 18280
		public Vector3 pos;

		// Token: 0x04004769 RID: 18281
		public Block block;

		// Token: 0x0400476A RID: 18282
		public Vector3 bmins;

		// Token: 0x0400476B RID: 18283
		public Vector3 bmaxs;

		// Token: 0x0400476C RID: 18284
		[PublicizedFrom(EAccessModifier.Private)]
		public static int _storageIndex = 0;

		// Token: 0x0400476D RID: 18285
		[PublicizedFrom(EAccessModifier.Private)]
		public static World.ClipBlock[] _storage = new World.ClipBlock[32];
	}

	// Token: 0x02000BDD RID: 3037
	public struct GroupBounds
	{
		// Token: 0x06005C8A RID: 23690 RVA: 0x002414F8 File Offset: 0x0023F6F8
		public bool IsWithinSize(int maxSize)
		{
			return this.max.x - this.min.x + 1 <= maxSize && this.max.y - this.min.y + 1 <= maxSize && this.max.z - this.min.z + 1 <= maxSize;
		}

		// Token: 0x0400476E RID: 18286
		public Vector3i min;

		// Token: 0x0400476F RID: 18287
		public Vector3i max;
	}

	// Token: 0x02000BDE RID: 3038
	public enum WorldEvent
	{
		// Token: 0x04004771 RID: 18289
		BloodMoon
	}
}
