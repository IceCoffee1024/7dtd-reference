using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using DynamicMusic;
using DynamicMusic.Factories;
using GamePath;
using Platform;
using PrefabVolumes;
using Twitch;
using UnityEngine;

// Token: 0x02000BFD RID: 3069
public class World : WorldBase, IBlockAccess, IChunkAccess, IChunkCallback
{
	// Token: 0x14000084 RID: 132
	// (add) Token: 0x06005C0F RID: 23567 RVA: 0x0023E7CC File Offset: 0x0023C9CC
	// (remove) Token: 0x06005C10 RID: 23568 RVA: 0x0023E804 File Offset: 0x0023CA04
	public event World.OnEntityLoadedDelegate EntityLoadedDelegates;

	// Token: 0x14000085 RID: 133
	// (add) Token: 0x06005C11 RID: 23569 RVA: 0x0023E83C File Offset: 0x0023CA3C
	// (remove) Token: 0x06005C12 RID: 23570 RVA: 0x0023E874 File Offset: 0x0023CA74
	public event World.OnEntityUnloadedDelegate EntityUnloadedDelegates;

	// Token: 0x06005C14 RID: 23572 RVA: 0x0023EA68 File Offset: 0x0023CC68
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

	// Token: 0x06005C15 RID: 23573 RVA: 0x0023EAF2 File Offset: 0x0023CCF2
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
			if (this.worldState.weatherManagerState != null)
			{
				using (PooledBinaryReader pooledBinaryReader6 = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader6.SetBaseStream(this.worldState.weatherManagerState);
					WeatherManager.Load(pooledBinaryReader6, (int)this.worldState.weatherManagerState.Length);
				}
			}
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

	// Token: 0x06005C16 RID: 23574 RVA: 0x0023EB18 File Offset: 0x0023CD18
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

	// Token: 0x06005C17 RID: 23575 RVA: 0x0023EBC5 File Offset: 0x0023CDC5
	public void SaveDecorations()
	{
		DecoManager.Instance.Save();
	}

	// Token: 0x06005C18 RID: 23576 RVA: 0x0023EBD1 File Offset: 0x0023CDD1
	public void SaveWorldState()
	{
		this.worldState.SetFrom(this, this.ChunkCache.ChunkProvider.GetProviderId());
		this.worldState.Save(GameIO.GetSaveGameDir() + "/main.ttw");
	}

	// Token: 0x06005C19 RID: 23577 RVA: 0x0023EC0C File Offset: 0x0023CE0C
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

	// Token: 0x06005C1A RID: 23578 RVA: 0x0023ECF8 File Offset: 0x0023CEF8
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

	// Token: 0x06005C1B RID: 23579 RVA: 0x0023EEE4 File Offset: 0x0023D0E4
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

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x06005C1C RID: 23580 RVA: 0x0023EF16 File Offset: 0x0023D116
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

	// Token: 0x06005C1D RID: 23581 RVA: 0x0023EF2D File Offset: 0x0023D12D
	public long GetNextChunkToProvide()
	{
		return this.m_ChunkManager.GetNextChunkToProvide();
	}

	// Token: 0x06005C1E RID: 23582 RVA: 0x0023EF3A File Offset: 0x0023D13A
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

	// Token: 0x06005C1F RID: 23583 RVA: 0x0023EF6E File Offset: 0x0023D16E
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

	// Token: 0x06005C20 RID: 23584 RVA: 0x0023EF9D File Offset: 0x0023D19D
	public override void RemoveLocalPlayer(EntityPlayerLocal _localPlayer)
	{
		this.m_LocalPlayerEntities.Remove(_localPlayer);
	}

	// Token: 0x06005C21 RID: 23585 RVA: 0x0023EFAC File Offset: 0x0023D1AC
	public override List<EntityPlayerLocal> GetLocalPlayers()
	{
		return this.m_LocalPlayerEntities;
	}

	// Token: 0x06005C22 RID: 23586 RVA: 0x0023EFB4 File Offset: 0x0023D1B4
	public override bool IsLocalPlayer(int _playerId)
	{
		Entity entity = this.GetEntity(_playerId);
		return entity != null && entity is EntityPlayerLocal;
	}

	// Token: 0x06005C23 RID: 23587 RVA: 0x0023EFDD File Offset: 0x0023D1DD
	public override EntityPlayerLocal GetLocalPlayerFromID(int _playerId)
	{
		return this.GetEntity(_playerId) as EntityPlayerLocal;
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x0023EFEC File Offset: 0x0023D1EC
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

	// Token: 0x06005C25 RID: 23589 RVA: 0x0023F061 File Offset: 0x0023D261
	public override Vector3 GetVectorToClosestLocalPlayer(Vector3 _position)
	{
		return this.GetClosestLocalPlayer(_position).GetPosition() - _position;
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x0023F078 File Offset: 0x0023D278
	public override float GetSquaredDistanceToClosestLocalPlayer(Vector3 _position)
	{
		return this.GetVectorToClosestLocalPlayer(_position).sqrMagnitude;
	}

	// Token: 0x06005C27 RID: 23591 RVA: 0x0023F094 File Offset: 0x0023D294
	public override float GetDistanceToClosestLocalPlayer(Vector3 _position)
	{
		return this.GetVectorToClosestLocalPlayer(_position).magnitude;
	}

	// Token: 0x06005C28 RID: 23592 RVA: 0x0023F0B0 File Offset: 0x0023D2B0
	public void SetLocalPlayer(EntityPlayerLocal _thePlayer)
	{
		this.m_LocalPlayerEntity = _thePlayer;
		this.audioManager.AttachLocalPlayer(_thePlayer, this);
		LightManager.AttachLocalPlayer(_thePlayer, this);
		OcclusionManager.Instance.SetSourceDepthCamera(_thePlayer.playerCamera);
	}

	// Token: 0x06005C29 RID: 23593 RVA: 0x0023F0DD File Offset: 0x0023D2DD
	public override EntityPlayerLocal GetPrimaryPlayer()
	{
		return this.m_LocalPlayerEntity;
	}

	// Token: 0x06005C2A RID: 23594 RVA: 0x0023F0E5 File Offset: 0x0023D2E5
	public int GetPrimaryPlayerId()
	{
		if (!(this.m_LocalPlayerEntity != null))
		{
			return -1;
		}
		return this.m_LocalPlayerEntity.entityId;
	}

	// Token: 0x06005C2B RID: 23595 RVA: 0x0023F102 File Offset: 0x0023D302
	public override List<EntityPlayer> GetPlayers()
	{
		return this.Players.list;
	}

	// Token: 0x06005C2C RID: 23596 RVA: 0x0023F110 File Offset: 0x0023D310
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

	// Token: 0x06005C2D RID: 23597 RVA: 0x0023F16C File Offset: 0x0023D36C
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

	// Token: 0x06005C2E RID: 23598 RVA: 0x0023F1C8 File Offset: 0x0023D3C8
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

	// Token: 0x06005C2F RID: 23599 RVA: 0x0023F224 File Offset: 0x0023D424
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

	// Token: 0x06005C30 RID: 23600 RVA: 0x0023F26C File Offset: 0x0023D46C
	public void SetDensity(Vector3i _pos, sbyte _density, bool _bFoceDensity = false)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return;
		}
		chunkCache.SetDensity(_pos, _density, _bFoceDensity);
	}

	// Token: 0x06005C31 RID: 23601 RVA: 0x0023F290 File Offset: 0x0023D490
	public long GetTexture(int _x, int _y, int _z, int channel = 0)
	{
		Chunk chunk = (Chunk)this.ChunkCache.GetChunkFromWorldPos(_x, _y, _z);
		if (chunk != null)
		{
			return chunk.GetTextureFull(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z), channel);
		}
		return 0L;
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x0023F2D4 File Offset: 0x0023D4D4
	public TextureFullArray GetTextureFullArray(int _x, int _y, int _z)
	{
		Chunk chunk = (Chunk)this.ChunkCache.GetChunkFromWorldPos(_x, _y, _z);
		if (chunk != null)
		{
			return chunk.GetTextureFullArray(World.toBlockXZ(_x), World.toBlockY(_y), World.toBlockXZ(_z), true);
		}
		return new TextureFullArray(0L);
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x0023F319 File Offset: 0x0023D519
	public void SetTexture(int _x, int _y, int _z, long _tex, int channel = 0)
	{
		this.ChunkCache.SetTextureFull(new Vector3i(_x, _y, _z), _tex, channel);
	}

	// Token: 0x06005C34 RID: 23604 RVA: 0x0023F334 File Offset: 0x0023D534
	public override byte GetStability(int worldX, int worldY, int worldZ)
	{
		IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunkSync != null)
		{
			return chunkSync.GetStability(World.toBlockXZ(worldX), World.toBlockY(worldY), World.toBlockXZ(worldZ));
		}
		return 0;
	}

	// Token: 0x06005C35 RID: 23605 RVA: 0x0023F371 File Offset: 0x0023D571
	public override byte GetStability(Vector3i _pos)
	{
		return this.GetStability(_pos.x, _pos.y, _pos.z);
	}

	// Token: 0x06005C36 RID: 23606 RVA: 0x0023F38C File Offset: 0x0023D58C
	public override void SetStability(int worldX, int worldY, int worldZ, byte stab)
	{
		IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunkSync != null)
		{
			chunkSync.SetStability(World.toBlockXZ(worldX), World.toBlockY(worldY), World.toBlockXZ(worldZ), stab);
		}
	}

	// Token: 0x06005C37 RID: 23607 RVA: 0x0023F3C9 File Offset: 0x0023D5C9
	public override void SetStability(Vector3i _pos, byte stab)
	{
		this.SetStability(_pos.x, _pos.y, _pos.z, stab);
	}

	// Token: 0x06005C38 RID: 23608 RVA: 0x0023F3E4 File Offset: 0x0023D5E4
	public override byte GetHeight(int worldX, int worldZ)
	{
		IChunk chunkSync = this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunkSync != null)
		{
			return chunkSync.GetHeight(World.toBlockXZ(worldX), World.toBlockXZ(worldZ));
		}
		return 0;
	}

	// Token: 0x06005C39 RID: 23609 RVA: 0x0023F41C File Offset: 0x0023D61C
	public byte GetTerrainHeight(int worldX, int worldZ)
	{
		Chunk chunk = (Chunk)this.GetChunkSync(World.toChunkXZ(worldX), World.toChunkXZ(worldZ));
		if (chunk != null)
		{
			return chunk.GetTerrainHeight(World.toBlockXZ(worldX), World.toBlockXZ(worldZ));
		}
		return 0;
	}

	// Token: 0x06005C3A RID: 23610 RVA: 0x0023F458 File Offset: 0x0023D658
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

	// Token: 0x06005C3B RID: 23611 RVA: 0x0023F494 File Offset: 0x0023D694
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

	// Token: 0x06005C3C RID: 23612 RVA: 0x0023F510 File Offset: 0x0023D710
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

	// Token: 0x06005C3D RID: 23613 RVA: 0x0023F552 File Offset: 0x0023D752
	public override bool IsWater(Vector3i _pos)
	{
		return this.IsWater(_pos.x, _pos.y, _pos.z);
	}

	// Token: 0x06005C3E RID: 23614 RVA: 0x0023F56C File Offset: 0x0023D76C
	public override bool IsWater(Vector3 _pos)
	{
		return this.IsWater(World.worldToBlockPos(_pos));
	}

	// Token: 0x06005C3F RID: 23615 RVA: 0x0023F57C File Offset: 0x0023D77C
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

	// Token: 0x06005C40 RID: 23616 RVA: 0x0023F5C0 File Offset: 0x0023D7C0
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

	// Token: 0x06005C41 RID: 23617 RVA: 0x0023F6A0 File Offset: 0x0023D8A0
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

	// Token: 0x06005C42 RID: 23618 RVA: 0x0023F730 File Offset: 0x0023D930
	public bool FindRandomSpawnPointNearPlayer(Entity _entityPlayer, int maxLightValue, out int x, out int y, out int z, int maxDistance)
	{
		return this.FindRandomSpawnPointNearPosition(_entityPlayer.GetPosition(), maxLightValue, out x, out y, out z, new Vector3((float)maxDistance, (float)maxDistance, (float)maxDistance), true, false);
	}

	// Token: 0x06005C43 RID: 23619 RVA: 0x0023F760 File Offset: 0x0023D960
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

	// Token: 0x06005C44 RID: 23620 RVA: 0x0023F89C File Offset: 0x0023DA9C
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

	// Token: 0x06005C45 RID: 23621 RVA: 0x0023FA18 File Offset: 0x0023DC18
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

	// Token: 0x06005C46 RID: 23622 RVA: 0x0023FAA4 File Offset: 0x0023DCA4
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

	// Token: 0x06005C47 RID: 23623 RVA: 0x0023FCE8 File Offset: 0x0023DEE8
	public bool GetMobRandomSpawnPosWithWater(Vector3 _targetPos, int _minRange, int _maxRange, int _minPlayerRange, bool _checkBedrolls, out Vector3 _position)
	{
		return this.GetRandomSpawnPositionMinMaxToPosition(_targetPos, _minRange, _maxRange, _minPlayerRange, _checkBedrolls, out _position, -1, true, 20, false, EnumLandClaimOwner.None, false) || this.GetRandomSpawnPositionMinMaxToPosition(_targetPos, _minRange, _maxRange, _minPlayerRange, _checkBedrolls, out _position, -1, false, 20, false, EnumLandClaimOwner.None, false);
	}

	// Token: 0x06005C48 RID: 23624 RVA: 0x0023FD2C File Offset: 0x0023DF2C
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

	// Token: 0x06005C49 RID: 23625 RVA: 0x0023FFC0 File Offset: 0x0023E1C0
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

	// Token: 0x06005C4A RID: 23626 RVA: 0x002401A0 File Offset: 0x0023E3A0
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

	// Token: 0x06005C4B RID: 23627 RVA: 0x002401EC File Offset: 0x0023E3EC
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

	// Token: 0x06005C4C RID: 23628 RVA: 0x00240398 File Offset: 0x0023E598
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

	// Token: 0x06005C4D RID: 23629 RVA: 0x002403F4 File Offset: 0x0023E5F4
	public bool IsInPlayfield(Chunk _c)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		return !chunkCache.IsFixedSize || this.IsEditor() || (_c.X > chunkCache.ChunkMinPos.x && _c.Z > chunkCache.ChunkMinPos.y && _c.X < chunkCache.ChunkMaxPos.x && _c.Z < chunkCache.ChunkMaxPos.y);
	}

	// Token: 0x06005C4E RID: 23630 RVA: 0x0024046C File Offset: 0x0023E66C
	public override BlockValue GetBlock(int x, int y, int z)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return BlockValue.Air;
		}
		return chunkCache.GetBlock(x, y, z);
	}

	// Token: 0x06005C4F RID: 23631 RVA: 0x00240492 File Offset: 0x0023E692
	public WaterValue GetWater(int _x, int _y, int _z)
	{
		return this.GetWater(new Vector3i(_x, _y, _z));
	}

	// Token: 0x06005C50 RID: 23632 RVA: 0x002404A4 File Offset: 0x0023E6A4
	public WaterValue GetWater(Vector3i _pos)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return WaterValue.Empty;
		}
		return chunkCache.GetWater(_pos);
	}

	// Token: 0x06005C51 RID: 23633 RVA: 0x002404C8 File Offset: 0x0023E6C8
	public float GetWaterPercent(Vector3i _pos)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return 0f;
		}
		return chunkCache.GetWater(_pos).GetMassPercent();
	}

	// Token: 0x06005C52 RID: 23634 RVA: 0x002404F4 File Offset: 0x0023E6F4
	public override PropValue GetProp(int chunkX, int chunkZ, int propId)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return PropValue.AIR;
		}
		return chunkCache.GetProp(chunkX, chunkZ, propId);
	}

	// Token: 0x06005C53 RID: 23635 RVA: 0x0024051C File Offset: 0x0023E71C
	public override PropValue GetProp(long chunkKey, int propId)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return PropValue.AIR;
		}
		return chunkCache.GetProp(chunkKey, propId);
	}

	// Token: 0x06005C54 RID: 23636 RVA: 0x00240541 File Offset: 0x0023E741
	public void HandleWaterLevelChanged(Vector3i _pos, float _waterPercent)
	{
		GameLightManager instance = GameLightManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.HandleWaterLevelChanged();
	}

	// Token: 0x06005C55 RID: 23637 RVA: 0x00240552 File Offset: 0x0023E752
	public BiomeDefinition GetBiome(string _name)
	{
		return this.Biomes.GetBiome(_name);
	}

	// Token: 0x06005C56 RID: 23638 RVA: 0x00240560 File Offset: 0x0023E760
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

	// Token: 0x06005C57 RID: 23639 RVA: 0x002405A0 File Offset: 0x0023E7A0
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

	// Token: 0x06005C58 RID: 23640 RVA: 0x002405D8 File Offset: 0x0023E7D8
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

	// Token: 0x06005C59 RID: 23641 RVA: 0x002405FC File Offset: 0x0023E7FC
	public override IChunk GetChunkSync(long _key)
	{
		ChunkCluster chunkCache = this.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		return chunkCache.GetChunkSync(_key);
	}

	// Token: 0x06005C5A RID: 23642 RVA: 0x0024061C File Offset: 0x0023E81C
	public bool IsChunkAreaLoaded(Vector3 _position)
	{
		return this.IsChunkAreaLoaded(Utils.Fastfloor(_position.x), 0, Utils.Fastfloor(_position.z));
	}

	// Token: 0x06005C5B RID: 23643 RVA: 0x0024063C File Offset: 0x0023E83C
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

	// Token: 0x06005C5C RID: 23644 RVA: 0x00240695 File Offset: 0x0023E895
	public bool IsChunkAreaCollidersLoaded(Vector3 _position)
	{
		return this.IsChunkAreaCollidersLoaded(Utils.Fastfloor(_position.x), Utils.Fastfloor(_position.z));
	}

	// Token: 0x06005C5D RID: 23645 RVA: 0x002406B4 File Offset: 0x0023E8B4
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

	// Token: 0x06005C5E RID: 23646 RVA: 0x0020D7EB File Offset: 0x0020B9EB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toChunkXZ(int _v)
	{
		return _v >> 4;
	}

	// Token: 0x06005C5F RID: 23647 RVA: 0x00240721 File Offset: 0x0023E921
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i toChunkXZ(Vector2i _v)
	{
		return new Vector2i(_v.x >> 4, _v.y >> 4);
	}

	// Token: 0x06005C60 RID: 23648 RVA: 0x00240738 File Offset: 0x0023E938
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i toChunkXZ(Vector3 _v)
	{
		return new Vector2i(Utils.Fastfloor(_v.x) >> 4, Utils.Fastfloor(_v.z) >> 4);
	}

	// Token: 0x06005C61 RID: 23649 RVA: 0x00240759 File Offset: 0x0023E959
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i toChunkXZ(Vector3i _v)
	{
		return new Vector2i(_v.x >> 4, _v.z >> 4);
	}

	// Token: 0x06005C62 RID: 23650 RVA: 0x00240770 File Offset: 0x0023E970
	public static Vector3i toChunkXYZCube(Vector3 _v)
	{
		return new Vector3i(Utils.Fastfloor(_v.x) >> 4, Utils.Fastfloor(_v.y) >> 4, Utils.Fastfloor(_v.z) >> 4);
	}

	// Token: 0x06005C63 RID: 23651 RVA: 0x0024079E File Offset: 0x0023E99E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i toChunkXYZ(Vector3i _v)
	{
		return new Vector3i(_v.x >> 4, _v.y >> 8, _v.z >> 4);
	}

	// Token: 0x06005C64 RID: 23652 RVA: 0x002407BD File Offset: 0x0023E9BD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toChunkY(int _v)
	{
		return _v >> 8;
	}

	// Token: 0x06005C65 RID: 23653 RVA: 0x002407C2 File Offset: 0x0023E9C2
	public static Vector3 toChunkXyzWorldPos(Vector3i _v)
	{
		return new Vector3((float)(_v.x & -16), (float)(_v.y & -256), (float)(_v.z & -16));
	}

	// Token: 0x06005C66 RID: 23654 RVA: 0x002407EA File Offset: 0x0023E9EA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i toBlock(Vector3i _p)
	{
		_p.x &= 15;
		_p.y &= 255;
		_p.z &= 15;
		return _p;
	}

	// Token: 0x06005C67 RID: 23655 RVA: 0x00240820 File Offset: 0x0023EA20
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i toBlock(int _x, int _y, int _z)
	{
		return new Vector3i(_x & 15, _y & 255, _z & 15);
	}

	// Token: 0x06005C68 RID: 23656 RVA: 0x0020D7E5 File Offset: 0x0020B9E5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toBlockXZ(int _v)
	{
		return _v & 15;
	}

	// Token: 0x06005C69 RID: 23657 RVA: 0x00240836 File Offset: 0x0023EA36
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int toBlockY(int _v)
	{
		return _v & 255;
	}

	// Token: 0x06005C6A RID: 23658 RVA: 0x00240840 File Offset: 0x0023EA40
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

	// Token: 0x06005C6B RID: 23659 RVA: 0x00240894 File Offset: 0x0023EA94
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 blockToTransformPos(Vector3i _blockPos)
	{
		return new Vector3((float)_blockPos.x + 0.5f, (float)_blockPos.y, (float)_blockPos.z + 0.5f);
	}

	// Token: 0x06005C6C RID: 23660 RVA: 0x002408BC File Offset: 0x0023EABC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 blockToTransformPos(WorldBase _world, PropRef _propRef)
	{
		IChunk chunkSync = _world.GetChunkSync(_propRef.ChunkPos);
		PropValue prop = chunkSync.GetProp(_propRef);
		return chunkSync.GetWorldPos().ToVector3CenterXZ() + prop.transform.position;
	}

	// Token: 0x06005C6D RID: 23661 RVA: 0x002408FA File Offset: 0x0023EAFA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i worldToBlockPos(Vector3 _worldPos)
	{
		return new Vector3i(Utils.Fastfloor(_worldPos.x), Utils.Fastfloor(_worldPos.y), Utils.Fastfloor(_worldPos.z));
	}

	// Token: 0x06005C6E RID: 23662 RVA: 0x00240922 File Offset: 0x0023EB22
	public override void SetBlocksRPC(List<BlockChangeInfo> _blockChangeInfo)
	{
		this.gameManager.SetBlocksRPC(_blockChangeInfo, null);
	}

	// Token: 0x06005C6F RID: 23663 RVA: 0x00240931 File Offset: 0x0023EB31
	public BlockValue SetBlock(Vector3i _blockPos, BlockValue _blockValue, bool bNotify, bool updateLight)
	{
		return this.ChunkCache.SetBlock(_blockPos, _blockValue, bNotify, updateLight);
	}

	// Token: 0x06005C70 RID: 23664 RVA: 0x00240948 File Offset: 0x0023EB48
	public override void SetPropsRPC(List<PropChangeInfo> _changes)
	{
		this.gameManager.SetPropsRPC(_changes, null);
	}

	// Token: 0x06005C71 RID: 23665 RVA: 0x00240957 File Offset: 0x0023EB57
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsDaytime()
	{
		return !this.IsDark();
	}

	// Token: 0x06005C72 RID: 23666 RVA: 0x00240964 File Offset: 0x0023EB64
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

	// Token: 0x06005C73 RID: 23667 RVA: 0x002409B0 File Offset: 0x0023EBB0
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

	// Token: 0x06005C74 RID: 23668 RVA: 0x00240A0C File Offset: 0x0023EC0C
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

	// Token: 0x06005C75 RID: 23669 RVA: 0x00240A38 File Offset: 0x0023EC38
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

	// Token: 0x06005C76 RID: 23670 RVA: 0x00240A94 File Offset: 0x0023EC94
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

	// Token: 0x06005C77 RID: 23671 RVA: 0x00240CBA File Offset: 0x0023EEBA
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

	// Token: 0x06005C78 RID: 23672 RVA: 0x00240CFA File Offset: 0x0023EEFA
	public void UncullChunk(Chunk _c)
	{
		if (_c.IsInternalBlocksCulled)
		{
			this.chunksToUncull.Add(_c);
		}
	}

	// Token: 0x06005C79 RID: 23673 RVA: 0x00240D10 File Offset: 0x0023EF10
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

	// Token: 0x06005C7A RID: 23674 RVA: 0x00240DDC File Offset: 0x0023EFDC
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

	// Token: 0x06005C7B RID: 23675 RVA: 0x00240FDC File Offset: 0x0023F1DC
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

	// Token: 0x06005C7C RID: 23676 RVA: 0x00241080 File Offset: 0x0023F280
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

	// Token: 0x06005C7D RID: 23677 RVA: 0x0024165C File Offset: 0x0023F85C
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

	// Token: 0x06005C7E RID: 23678 RVA: 0x00241A54 File Offset: 0x0023FC54
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

	// Token: 0x06005C7F RID: 23679 RVA: 0x00241B28 File Offset: 0x0023FD28
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

	// Token: 0x06005C80 RID: 23680 RVA: 0x00241BFC File Offset: 0x0023FDFC
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

	// Token: 0x06005C81 RID: 23681 RVA: 0x00241CD0 File Offset: 0x0023FED0
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

	// Token: 0x06005C82 RID: 23682 RVA: 0x00241D90 File Offset: 0x0023FF90
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

	// Token: 0x06005C83 RID: 23683 RVA: 0x00241E50 File Offset: 0x00240050
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

	// Token: 0x06005C84 RID: 23684 RVA: 0x00241EE8 File Offset: 0x002400E8
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

	// Token: 0x06005C85 RID: 23685 RVA: 0x00241F8C File Offset: 0x0024018C
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

	// Token: 0x06005C86 RID: 23686 RVA: 0x00241FD0 File Offset: 0x002401D0
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

	// Token: 0x06005C87 RID: 23687 RVA: 0x00242030 File Offset: 0x00240230
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

	// Token: 0x06005C88 RID: 23688 RVA: 0x002420C4 File Offset: 0x002402C4
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

	// Token: 0x06005C89 RID: 23689 RVA: 0x002421DD File Offset: 0x002403DD
	public void TickEntitiesFlush()
	{
		this.TickEntitiesSlice(this.tickEntityList.Count);
	}

	// Token: 0x06005C8A RID: 23690 RVA: 0x002421F0 File Offset: 0x002403F0
	public void TickEntitiesSlice()
	{
		this.TickEntitiesSlice(this.tickEntitySliceCount);
	}

	// Token: 0x06005C8B RID: 23691 RVA: 0x00242200 File Offset: 0x00240400
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

	// Token: 0x06005C8C RID: 23692 RVA: 0x00242260 File Offset: 0x00240460
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

	// Token: 0x06005C8D RID: 23693 RVA: 0x002423F8 File Offset: 0x002405F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickEntityRemove(Entity e)
	{
		int num = this.tickEntityList.IndexOf(e);
		if (num >= this.tickEntityIndex)
		{
			this.tickEntityList[num] = null;
		}
	}

	// Token: 0x06005C8E RID: 23694 RVA: 0x00242428 File Offset: 0x00240628
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

	// Token: 0x06005C8F RID: 23695 RVA: 0x002426D8 File Offset: 0x002408D8
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

	// Token: 0x06005C90 RID: 23696 RVA: 0x0024270C File Offset: 0x0024090C
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

	// Token: 0x06005C91 RID: 23697 RVA: 0x00242764 File Offset: 0x00240964
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

	// Token: 0x06005C92 RID: 23698 RVA: 0x00242794 File Offset: 0x00240994
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

	// Token: 0x06005C93 RID: 23699 RVA: 0x00242A04 File Offset: 0x00240C04
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

	// Token: 0x06005C94 RID: 23700 RVA: 0x00242A38 File Offset: 0x00240C38
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

	// Token: 0x06005C95 RID: 23701 RVA: 0x00242A80 File Offset: 0x00240C80
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

	// Token: 0x06005C96 RID: 23702 RVA: 0x00242C70 File Offset: 0x00240E70
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

	// Token: 0x06005C97 RID: 23703 RVA: 0x00242DF0 File Offset: 0x00240FF0
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

	// Token: 0x06005C98 RID: 23704 RVA: 0x00242F08 File Offset: 0x00241108
	public void RefreshEntitiesOnMap()
	{
		foreach (Entity entity in this.Entities.list)
		{
			this.RemoveEntityFromMap(entity, EnumRemoveEntityReason.Undef);
			this.AddEntityToMap(entity);
		}
	}

	// Token: 0x06005C99 RID: 23705 RVA: 0x00242F68 File Offset: 0x00241168
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

	// Token: 0x06005C9A RID: 23706 RVA: 0x00243020 File Offset: 0x00241220
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

	// Token: 0x06005C9B RID: 23707 RVA: 0x002430DC File Offset: 0x002412DC
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

	// Token: 0x06005C9C RID: 23708 RVA: 0x002431B8 File Offset: 0x002413B8
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

	// Token: 0x06005C9D RID: 23709 RVA: 0x00243324 File Offset: 0x00241524
	public override void AddFallingBlocks(IList<Vector3i> _list)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			this.AddFallingBlock(_list[i], false);
		}
	}

	// Token: 0x06005C9E RID: 23710 RVA: 0x00243350 File Offset: 0x00241550
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

	// Token: 0x06005C9F RID: 23711 RVA: 0x002433C4 File Offset: 0x002415C4
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

	// Token: 0x06005CA0 RID: 23712 RVA: 0x00243650 File Offset: 0x00241850
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

	// Token: 0x06005CA1 RID: 23713 RVA: 0x00243A2C File Offset: 0x00241C2C
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

	// Token: 0x06005CA2 RID: 23714 RVA: 0x00243B78 File Offset: 0x00241D78
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

	// Token: 0x06005CA3 RID: 23715 RVA: 0x00243BB8 File Offset: 0x00241DB8
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

	// Token: 0x06005CA4 RID: 23716 RVA: 0x00243C14 File Offset: 0x00241E14
	public override IGameManager GetGameManager()
	{
		return this.gameManager;
	}

	// Token: 0x06005CA5 RID: 23717 RVA: 0x00243C1C File Offset: 0x00241E1C
	public override Manager GetAudioManager()
	{
		return this.audioManager;
	}

	// Token: 0x06005CA6 RID: 23718 RVA: 0x00243C24 File Offset: 0x00241E24
	public override AIDirector GetAIDirector()
	{
		return this.aiDirector;
	}

	// Token: 0x06005CA7 RID: 23719 RVA: 0x00243C2C File Offset: 0x00241E2C
	public override bool IsRemote()
	{
		return !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
	}

	// Token: 0x06005CA8 RID: 23720 RVA: 0x00243C3C File Offset: 0x00241E3C
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

	// Token: 0x06005CA9 RID: 23721 RVA: 0x00243CD6 File Offset: 0x00241ED6
	public EntityPlayer GetClosestPlayer(Entity _entity, float _distMax, bool _isDead)
	{
		return this.GetClosestPlayer(_entity.position, _distMax, _isDead);
	}

	// Token: 0x06005CAA RID: 23722 RVA: 0x00243CE8 File Offset: 0x00241EE8
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

	// Token: 0x06005CAB RID: 23723 RVA: 0x00243D70 File Offset: 0x00241F70
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

	// Token: 0x06005CAC RID: 23724 RVA: 0x00243E1C File Offset: 0x0024201C
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

	// Token: 0x06005CAD RID: 23725 RVA: 0x00243E86 File Offset: 0x00242086
	public override WorldBlockTicker GetWBT()
	{
		return this.worldBlockTicker;
	}

	// Token: 0x06005CAE RID: 23726 RVA: 0x00243E8E File Offset: 0x0024208E
	public override bool IsOpenSkyAbove(int _x, int _y, int _z)
	{
		return this.ChunkCache == null || ((Chunk)this.GetChunkSync(_x >> 4, _z >> 4)).IsOpenSkyAbove(_x & 15, _y, _z & 15);
	}

	// Token: 0x06005CAF RID: 23727 RVA: 0x0005AB36 File Offset: 0x00058D36
	public override bool IsEditor()
	{
		return GameManager.Instance.IsEditMode();
	}

	// Token: 0x06005CB0 RID: 23728 RVA: 0x00243EB9 File Offset: 0x002420B9
	public int GetGameMode()
	{
		return GameStats.GetInt(EnumGameStats.GameModeId);
	}

	// Token: 0x06005CB1 RID: 23729 RVA: 0x00243EC1 File Offset: 0x002420C1
	public SpawnManagerDynamic GetDynamiceSpawnManager()
	{
		return this.dynamicSpawnManager;
	}

	// Token: 0x06005CB2 RID: 23730 RVA: 0x00243ECC File Offset: 0x002420CC
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

	// Token: 0x06005CB3 RID: 23731 RVA: 0x00244004 File Offset: 0x00242204
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

	// Token: 0x06005CB4 RID: 23732 RVA: 0x002440FE File Offset: 0x002422FE
	public override bool CanPickupBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative)
	{
		return (World.SandboxUseTraderArea != TraderAreaStates.Default || !this.IsWithinTraderArea(blockPos)) && this.CanPlaceBlockAt(blockPos, lpRelative, false);
	}

	// Token: 0x06005CB5 RID: 23733 RVA: 0x0024411C File Offset: 0x0024231C
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

	// Token: 0x06005CB6 RID: 23734 RVA: 0x00244238 File Offset: 0x00242438
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

	// Token: 0x06005CB7 RID: 23735 RVA: 0x002443BC File Offset: 0x002425BC
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

	// Token: 0x06005CB8 RID: 23736 RVA: 0x0024450A File Offset: 0x0024270A
	public override bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative)
	{
		return this.IsMyLandProtectedBlock(worldBlockPos, lpRelative, World.SandboxUseTraderArea == TraderAreaStates.Claimable);
	}

	// Token: 0x06005CB9 RID: 23737 RVA: 0x0024451C File Offset: 0x0024271C
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

	// Token: 0x06005CBA RID: 23738 RVA: 0x0024461C File Offset: 0x0024281C
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

	// Token: 0x06005CBB RID: 23739 RVA: 0x00244724 File Offset: 0x00242924
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

	// Token: 0x06005CBC RID: 23740 RVA: 0x00244840 File Offset: 0x00242A40
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

	// Token: 0x06005CBD RID: 23741 RVA: 0x00244940 File Offset: 0x00242B40
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

	// Token: 0x06005CBE RID: 23742 RVA: 0x00244A20 File Offset: 0x00242C20
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

	// Token: 0x06005CBF RID: 23743 RVA: 0x00244B24 File Offset: 0x00242D24
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

	// Token: 0x06005CC0 RID: 23744 RVA: 0x00244C74 File Offset: 0x00242E74
	public bool IsLandProtectionValidForPlayer(PersistentPlayerData ppData)
	{
		double num = (double)GameStats.GetInt(EnumGameStats.LandClaimExpiryTime) * 24.0;
		return ppData.OfflineHours <= num;
	}

	// Token: 0x06005CC1 RID: 23745 RVA: 0x00244CA0 File Offset: 0x00242EA0
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

	// Token: 0x06005CC2 RID: 23746 RVA: 0x00244DAC File Offset: 0x00242FAC
	public float GetDecorationOffsetY(Vector3i _blockPos)
	{
		sbyte density = base.GetDensity(_blockPos);
		sbyte density2 = base.GetDensity(_blockPos - Vector3i.up);
		return MarchingCubes.GetDecorationOffsetY(density, density2);
	}

	// Token: 0x06005CC3 RID: 23747 RVA: 0x00244DD8 File Offset: 0x00242FD8
	public EnumDecoAllowed GetDecoAllowedAt(int _x, int _z)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_x, _z);
		if (chunk != null)
		{
			return chunk.GetDecoAllowedAt(World.toBlockXZ(_x), World.toBlockXZ(_z));
		}
		return EnumDecoAllowed.Nothing;
	}

	// Token: 0x06005CC4 RID: 23748 RVA: 0x00244E10 File Offset: 0x00243010
	public void SetDecoAllowedAt(int _x, int _z, EnumDecoAllowed _decoAllowed)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_x, _z);
		if (chunk != null)
		{
			chunk.SetDecoAllowedAt(World.toBlockXZ(_x), World.toBlockXZ(_z), _decoAllowed);
		}
	}

	// Token: 0x06005CC5 RID: 23749 RVA: 0x00244E44 File Offset: 0x00243044
	public Vector3 GetTerrainNormalAt(int _x, int _z)
	{
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(_x, _z);
		if (chunk != null)
		{
			return chunk.GetTerrainNormal(World.toBlockXZ(_x), World.toBlockXZ(_z));
		}
		return Vector3.zero;
	}

	// Token: 0x06005CC6 RID: 23750 RVA: 0x00244E7A File Offset: 0x0024307A
	public bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
	{
		return this.ChunkCache.ChunkProvider.GetWorldExtent(out _minSize, out _maxSize);
	}

	// Token: 0x06005CC7 RID: 23751 RVA: 0x00244E90 File Offset: 0x00243090
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

	// Token: 0x06005CC8 RID: 23752 RVA: 0x00244EF4 File Offset: 0x002430F4
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

	// Token: 0x06005CC9 RID: 23753 RVA: 0x00244F4C File Offset: 0x0024314C
	public bool CanMobsSpawnAtPos(Vector3 _pos)
	{
		Vector3i vector3i = World.worldToBlockPos(_pos);
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
		return chunk != null && chunk.CanMobsSpawnAtPos(World.toBlockXZ(vector3i.x), World.toBlockY(vector3i.y), World.toBlockXZ(vector3i.z), false, true);
	}

	// Token: 0x06005CCA RID: 23754 RVA: 0x00244F9C File Offset: 0x0024319C
	public bool CanSleeperSpawnAtPos(Vector3 _pos, bool _checkBelow)
	{
		Vector3i vector3i = World.worldToBlockPos(_pos);
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
		return chunk != null && chunk.CanSleeperSpawnAtPos(World.toBlockXZ(vector3i.x), World.toBlockY(vector3i.y), World.toBlockXZ(vector3i.z), _checkBelow);
	}

	// Token: 0x06005CCB RID: 23755 RVA: 0x00244FEC File Offset: 0x002431EC
	public bool CanPlayersSpawnAtPos(Vector3 _pos, bool _bAllowToSpawnOnAirPos = false)
	{
		Vector3i vector3i = World.worldToBlockPos(_pos);
		Chunk chunk = (Chunk)base.GetChunkFromWorldPos(vector3i);
		return chunk != null && chunk.CanPlayersSpawnAtPos(World.toBlockXZ(vector3i.x), World.toBlockY(vector3i.y), World.toBlockXZ(vector3i.z), _bAllowToSpawnOnAirPos);
	}

	// Token: 0x06005CCC RID: 23756 RVA: 0x0024503C File Offset: 0x0024323C
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

	// Token: 0x06005CCD RID: 23757 RVA: 0x00245074 File Offset: 0x00243274
	public void OnChunkAdded(Chunk _c)
	{
		List<long> list = this.newlyLoadedChunksThisUpdate;
		lock (list)
		{
			this.newlyLoadedChunksThisUpdate.Add(_c.Key);
		}
	}

	// Token: 0x06005CCE RID: 23758 RVA: 0x002450C0 File Offset: 0x002432C0
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

	// Token: 0x06005CCF RID: 23759 RVA: 0x00245130 File Offset: 0x00243330
	public void OnChunkBeforeSave(Chunk _c)
	{
		if (this.worldBlockTicker != null)
		{
			this.worldBlockTicker.OnChunkBeforeSave(_c);
		}
	}

	// Token: 0x06005CD0 RID: 23760 RVA: 0x00245148 File Offset: 0x00243348
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

	// Token: 0x06005CD1 RID: 23761 RVA: 0x00245200 File Offset: 0x00243400
	public override ulong GetWorldTime()
	{
		return this.worldTime;
	}

	// Token: 0x06005CD2 RID: 23762 RVA: 0x00245208 File Offset: 0x00243408
	public override WorldCreationData GetWorldCreationData()
	{
		return this.wcd;
	}

	// Token: 0x06005CD3 RID: 23763 RVA: 0x00245210 File Offset: 0x00243410
	public bool IsEntityInRange(int _entityId, int _refEntity, int _range)
	{
		Entity entity;
		Entity entity2;
		return _entityId == _refEntity || (this.Entities.dict.TryGetValue(_entityId, out entity) && this.Entities.dict.TryGetValue(_refEntity, out entity2) && entity.GetDistanceSq(entity2) <= (float)(_range * _range));
	}

	// Token: 0x06005CD4 RID: 23764 RVA: 0x00245260 File Offset: 0x00243460
	public bool IsEntityInRange(int _entityId, Vector3 _position, int _range)
	{
		Entity entity;
		return this.Entities.dict.TryGetValue(_entityId, out entity) && entity.GetDistanceSq(_position) <= (float)(_range * _range);
	}

	// Token: 0x06005CD5 RID: 23765 RVA: 0x00245294 File Offset: 0x00243494
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

	// Token: 0x06005CD6 RID: 23766 RVA: 0x00245360 File Offset: 0x00243560
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

	// Token: 0x06005CD7 RID: 23767 RVA: 0x00245470 File Offset: 0x00243670
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

	// Token: 0x06005CD8 RID: 23768 RVA: 0x00245568 File Offset: 0x00243768
	public bool IsPositionRadiated(Vector3 position)
	{
		IChunkProvider chunkProvider = this.ChunkCache.ChunkProvider;
		IBiomeProvider biomeProvider;
		return chunkProvider != null && (biomeProvider = chunkProvider.GetBiomeProvider()) != null && biomeProvider.GetRadiationAt((int)position.x, (int)position.z) > 0f;
	}

	// Token: 0x06005CD9 RID: 23769 RVA: 0x002455AB File Offset: 0x002437AB
	public bool IsPositionWithinPOI(Vector3 position, int offset)
	{
		return this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().GetPrefabFromWorldPosInsideWithOffset((int)position.x, (int)position.z, offset) != null;
	}

	// Token: 0x06005CDA RID: 23770 RVA: 0x002455D4 File Offset: 0x002437D4
	public PrefabInstance GetPOIAtPosition(Vector3 _position, FastTags<TagGroup.Poi>? _excludeTags = null, FastTags<TagGroup.Poi>? _requiredTags = null)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator == null)
		{
			return null;
		}
		return dynamicPrefabDecorator.GetPrefabAtPosition(_position, _excludeTags, _requiredTags);
	}

	// Token: 0x06005CDB RID: 23771 RVA: 0x00245600 File Offset: 0x00243800
	public void GetPOIsAtXZ(int _xMin, int _xMax, int _zMin, int _zMax, List<PrefabInstance> _list)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator != null)
		{
			dynamicPrefabDecorator.GetPrefabsAtXZ(_xMin, _xMax, _zMin, _zMax, _list);
		}
	}

	// Token: 0x06005CDC RID: 23772 RVA: 0x00245630 File Offset: 0x00243830
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

	// Token: 0x06005CDD RID: 23773 RVA: 0x00245724 File Offset: 0x00243924
	public Vector3 ClampToValidWorldPosForMap(Vector2 position)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		float num = Mathf.Clamp(position.x, (float)vector3i.x, (float)vector3i2.x);
		float num2 = Mathf.Clamp(position.y, (float)vector3i.z, (float)vector3i2.z);
		return new Vector2(num, num2);
	}

	// Token: 0x06005CDE RID: 23774 RVA: 0x0024577A File Offset: 0x0024397A
	public void ObjectOnMapAdd(MapObject _mo)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.Add(_mo);
		}
	}

	// Token: 0x06005CDF RID: 23775 RVA: 0x00245790 File Offset: 0x00243990
	public void ObjectOnMapRemove(EnumMapObjectType _type, int _key)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.Remove(_type, _key);
		}
	}

	// Token: 0x06005CE0 RID: 23776 RVA: 0x002457A7 File Offset: 0x002439A7
	public void ObjectOnMapRemove(EnumMapObjectType _type, Vector3 _position)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.RemoveByPosition(_type, _position);
		}
	}

	// Token: 0x06005CE1 RID: 23777 RVA: 0x002457BE File Offset: 0x002439BE
	public void ObjectOnMapRemove(EnumMapObjectType _type)
	{
		if (this.objectsOnMap != null)
		{
			this.objectsOnMap.RemoveByType(_type);
		}
	}

	// Token: 0x06005CE2 RID: 23778 RVA: 0x002457D4 File Offset: 0x002439D4
	public List<MapObject> GetObjectOnMapList(EnumMapObjectType _type)
	{
		if (this.objectsOnMap != null)
		{
			return this.objectsOnMap.GetList(_type);
		}
		return new List<MapObject>();
	}

	// Token: 0x06005CE3 RID: 23779 RVA: 0x002457F0 File Offset: 0x002439F0
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

	// Token: 0x06005CE4 RID: 23780 RVA: 0x00245894 File Offset: 0x00243A94
	public static void SetWorldAreas(List<TraderArea> _traders)
	{
		World.traderAreas = _traders;
	}

	// Token: 0x06005CE5 RID: 23781 RVA: 0x0024589C File Offset: 0x00243A9C
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

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x06005CE6 RID: 23782 RVA: 0x002458F8 File Offset: 0x00243AF8
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

	// Token: 0x06005CE7 RID: 23783 RVA: 0x00245921 File Offset: 0x00243B21
	public bool IsWithinTraderArea(Vector3i _worldBlockPos)
	{
		return this.GetTraderAreaAt(_worldBlockPos) != null;
	}

	// Token: 0x06005CE8 RID: 23784 RVA: 0x00245930 File Offset: 0x00243B30
	public bool IsWithinTraderPlacingProtection(Vector3i _worldBlockPos)
	{
		if (World.SandboxUseTraderArea != TraderAreaStates.Default)
		{
			return false;
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		return dynamicPrefabDecorator != null && dynamicPrefabDecorator.GetTraderAtPosition(_worldBlockPos, 2) != null;
	}

	// Token: 0x06005CE9 RID: 23785 RVA: 0x00245968 File Offset: 0x00243B68
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

	// Token: 0x06005CEA RID: 23786 RVA: 0x002459C4 File Offset: 0x00243BC4
	public bool IsWithinTraderArea(Vector3i _minPos, Vector3i _maxPos)
	{
		if (World.SandboxUseTraderArea == TraderAreaStates.Claimable)
		{
			return false;
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		return dynamicPrefabDecorator != null && dynamicPrefabDecorator.IsWithinTraderArea(_minPos, _maxPos);
	}

	// Token: 0x06005CEB RID: 23787 RVA: 0x002459FC File Offset: 0x00243BFC
	public TraderArea GetTraderAreaAt(Vector3i _pos)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator != null)
		{
			return dynamicPrefabDecorator.GetTraderAtPosition(_pos, 0);
		}
		return null;
	}

	// Token: 0x06005CEC RID: 23788 RVA: 0x00245A28 File Offset: 0x00243C28
	public TraderArea GetTraderAreaOuterAt(Vector3i _pos, int offset)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = this.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator != null)
		{
			return dynamicPrefabDecorator.GetTraderOuterAtPosition(_pos, 3);
		}
		return null;
	}

	// Token: 0x06005CED RID: 23789 RVA: 0x00245A54 File Offset: 0x00243C54
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

	// Token: 0x06005CEE RID: 23790 RVA: 0x00245AE8 File Offset: 0x00243CE8
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

	// Token: 0x06005CEF RID: 23791 RVA: 0x00245B40 File Offset: 0x00243D40
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

	// Token: 0x06005CF0 RID: 23792 RVA: 0x00245BA4 File Offset: 0x00243DA4
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

	// Token: 0x06005CF1 RID: 23793 RVA: 0x00245C38 File Offset: 0x00243E38
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

	// Token: 0x06005CF2 RID: 23794 RVA: 0x00245CD8 File Offset: 0x00243ED8
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

	// Token: 0x06005CF3 RID: 23795 RVA: 0x00245D84 File Offset: 0x00243F84
	public void WriteSleeperVolumes(PooledBinaryWriter _bw)
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

	// Token: 0x06005CF4 RID: 23796 RVA: 0x00245E34 File Offset: 0x00244034
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadSleeperVolumes(int _version, PooledBinaryReader _br)
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

	// Token: 0x06005CF5 RID: 23797 RVA: 0x00245FBC File Offset: 0x002441BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupSleeperVolumes()
	{
		foreach (SleeperVolume sleeperVolume in this.sleeperVolumes.Values)
		{
			sleeperVolume.AddToPrefabInstance();
		}
	}

	// Token: 0x06005CF6 RID: 23798 RVA: 0x00246014 File Offset: 0x00244214
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

	// Token: 0x06005CF7 RID: 23799 RVA: 0x00246094 File Offset: 0x00244294
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

	// Token: 0x06005CF8 RID: 23800 RVA: 0x00246118 File Offset: 0x00244318
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

	// Token: 0x06005CF9 RID: 23801 RVA: 0x002461B8 File Offset: 0x002443B8
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

	// Token: 0x06005CFA RID: 23802 RVA: 0x0024624C File Offset: 0x0024444C
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

	// Token: 0x06005CFB RID: 23803 RVA: 0x002462F4 File Offset: 0x002444F4
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

	// Token: 0x06005CFC RID: 23804 RVA: 0x0024639C File Offset: 0x0024459C
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

	// Token: 0x06005CFD RID: 23805 RVA: 0x0024641C File Offset: 0x0024461C
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

	// Token: 0x06005CFE RID: 23806 RVA: 0x00246474 File Offset: 0x00244674
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

	// Token: 0x06005CFF RID: 23807 RVA: 0x002464D8 File Offset: 0x002446D8
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

	// Token: 0x06005D00 RID: 23808 RVA: 0x0024656C File Offset: 0x0024476C
	public void WriteTriggerVolumes(PooledBinaryWriter _bw)
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

	// Token: 0x06005D01 RID: 23809 RVA: 0x0024661C File Offset: 0x0024481C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadTriggerVolumes(int _version, PooledBinaryReader _br)
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

	// Token: 0x06005D02 RID: 23810 RVA: 0x002467A4 File Offset: 0x002449A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupTriggerVolumes()
	{
		foreach (TriggerVolume triggerVolume in this.triggerVolumes.Values)
		{
			triggerVolume.AddToPrefabInstance();
		}
	}

	// Token: 0x06005D03 RID: 23811 RVA: 0x002467FC File Offset: 0x002449FC
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

	// Token: 0x06005D04 RID: 23812 RVA: 0x002468A0 File Offset: 0x00244AA0
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

	// Token: 0x06005D05 RID: 23813 RVA: 0x0024696C File Offset: 0x00244B6C
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

	// Token: 0x06005D06 RID: 23814 RVA: 0x00246A14 File Offset: 0x00244C14
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

	// Token: 0x06005D07 RID: 23815 RVA: 0x00246AC0 File Offset: 0x00244CC0
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

	// Token: 0x06005D08 RID: 23816 RVA: 0x00246B18 File Offset: 0x00244D18
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

	// Token: 0x06005D09 RID: 23817 RVA: 0x00246B7C File Offset: 0x00244D7C
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

	// Token: 0x06005D0A RID: 23818 RVA: 0x00246C20 File Offset: 0x00244E20
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

	// Token: 0x06005D0B RID: 23819 RVA: 0x00246CA8 File Offset: 0x00244EA8
	public void WriteWallVolumes(PooledBinaryWriter _bw)
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

	// Token: 0x06005D0C RID: 23820 RVA: 0x00246D58 File Offset: 0x00244F58
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReadWallVolumes(int _version, PooledBinaryReader _br)
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

	// Token: 0x06005D0D RID: 23821 RVA: 0x00246EE0 File Offset: 0x002450E0
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

	// Token: 0x06005D0E RID: 23822 RVA: 0x00246F68 File Offset: 0x00245168
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupWallVolumes()
	{
		foreach (WallVolume wallVolume in this.wallVolumes.Values)
		{
			wallVolume.AddToPrefabInstance();
		}
	}

	// Token: 0x06005D0F RID: 23823 RVA: 0x00246FC0 File Offset: 0x002451C0
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

	// Token: 0x06005D10 RID: 23824 RVA: 0x00247090 File Offset: 0x00245290
	public void AddBlockData(Vector3i v3i, object bd)
	{
		this.blockData.Add(v3i, bd);
	}

	// Token: 0x06005D11 RID: 23825 RVA: 0x002470A0 File Offset: 0x002452A0
	public object GetBlockData(Vector3i v3i)
	{
		object obj;
		if (!this.blockData.TryGetValue(v3i, out obj))
		{
			obj = null;
		}
		return obj;
	}

	// Token: 0x06005D12 RID: 23826 RVA: 0x002470C0 File Offset: 0x002452C0
	public void ClearBlockData(Vector3i v3i)
	{
		this.blockData.Remove(v3i);
	}

	// Token: 0x06005D13 RID: 23827 RVA: 0x002470CF File Offset: 0x002452CF
	public void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _bStopStabilityUpdate, bool _bRegenerateChunk, bool _bFillEmptyBlocks, bool _isReset = false)
	{
		this.ChunkCache.ChunkProvider.RebuildTerrain(_chunks, _areaStart, _areaSize, _bStopStabilityUpdate, _bRegenerateChunk, _bFillEmptyBlocks, _isReset);
	}

	// Token: 0x06005D14 RID: 23828 RVA: 0x002470EC File Offset: 0x002452EC
	public override GameRandom GetGameRandom()
	{
		return this.rand;
	}

	// Token: 0x06005D15 RID: 23829 RVA: 0x002470F4 File Offset: 0x002452F4
	public float RandomRange(float _min, float _max)
	{
		return this.rand.RandomFloat * (_max - _min) + _min;
	}

	// Token: 0x06005D16 RID: 23830 RVA: 0x00247108 File Offset: 0x00245308
	[PublicizedFrom(EAccessModifier.Private)]
	public void DuskDawnInit()
	{
		ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
		this.DuskHour = valueTuple.Item1;
		this.DawnHour = valueTuple.Item2;
	}

	// Token: 0x06005D17 RID: 23831 RVA: 0x0024713A File Offset: 0x0024533A
	public void SetTime(ulong _time)
	{
		this.worldTime = _time;
		if (this.m_WorldEnvironment)
		{
			this.m_WorldEnvironment.WorldTimeChanged();
		}
	}

	// Token: 0x06005D18 RID: 23832 RVA: 0x0024715B File Offset: 0x0024535B
	public void SetTimeJump(ulong _time, bool _isSeek = false)
	{
		this.SetTime(_time);
		SkyManager.bUpdateSunMoonNow = true;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.aiDirector.BloodMoonComponent.TimeChanged(_isSeek);
		}
	}

	// Token: 0x06005D19 RID: 23833 RVA: 0x00247187 File Offset: 0x00245387
	public bool IsWorldEvent(World.WorldEvent _event)
	{
		return _event == World.WorldEvent.BloodMoon && this.isEventBloodMoon;
	}

	// Token: 0x06005D1A RID: 23834 RVA: 0x00247194 File Offset: 0x00245394
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

	// Token: 0x06005D1B RID: 23835 RVA: 0x002472C4 File Offset: 0x002454C4
	public override void AddPendingDowngradeBlock(BlockValueRef _bvRef)
	{
		this.pendingUpgradeDowngradeBlocks.Add(_bvRef);
	}

	// Token: 0x06005D1C RID: 23836 RVA: 0x002472D3 File Offset: 0x002454D3
	public override bool TryRetrieveAndRemovePendingDowngradeBlock(BlockValueRef _bvRef)
	{
		if (this.pendingUpgradeDowngradeBlocks.Contains(_bvRef))
		{
			this.pendingUpgradeDowngradeBlocks.Remove(_bvRef);
			return true;
		}
		return false;
	}

	// Token: 0x06005D1D RID: 23837 RVA: 0x002472F3 File Offset: 0x002454F3
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

	// Token: 0x06005D1E RID: 23838 RVA: 0x00247320 File Offset: 0x00245520
	public bool IsRandomWorld()
	{
		bool flag = false;
		if (this.ChunkCache.ChunkProvider != null && this.ChunkCache.ChunkProvider.WorldInfo != null)
		{
			flag = this.ChunkCache.ChunkProvider.WorldInfo.RandomGeneratedWorld;
		}
		return flag;
	}

	// Token: 0x040047DB RID: 18395
	public const int cCollisionBlocks = 5;

	// Token: 0x040047DE RID: 18398
	public ulong worldTime;

	// Token: 0x040047DF RID: 18399
	public int DawnHour;

	// Token: 0x040047E0 RID: 18400
	public int DuskHour;

	// Token: 0x040047E1 RID: 18401
	public float Gravity = 0.08f;

	// Token: 0x040047E2 RID: 18402
	public DictionaryList<int, Entity> Entities = new DictionaryList<int, Entity>();

	// Token: 0x040047E3 RID: 18403
	public DictionaryList<int, EntityPlayer> Players = new DictionaryList<int, EntityPlayer>();

	// Token: 0x040047E4 RID: 18404
	public List<EntityAlive> EntityAlives = new List<EntityAlive>();

	// Token: 0x040047E5 RID: 18405
	public EntityAsyncManager entityAsyncManager;

	// Token: 0x040047E6 RID: 18406
	public NetEntityDistribution entityDistributer;

	// Token: 0x040047E7 RID: 18407
	public NetEntityPackageQueue netEntityPackageQueue;

	// Token: 0x040047E8 RID: 18408
	public AIDirector aiDirector;

	// Token: 0x040047E9 RID: 18409
	public Manager audioManager;

	// Token: 0x040047EA RID: 18410
	public Conductor dmsConductor;

	// Token: 0x040047EB RID: 18411
	public IGameManager gameManager;

	// Token: 0x040047EC RID: 18412
	public int Seed;

	// Token: 0x040047ED RID: 18413
	public WorldBiomes Biomes;

	// Token: 0x040047EE RID: 18414
	public SpawnManagerBiomes biomeSpawnManager;

	// Token: 0x040047EF RID: 18415
	public BiomeIntensity LocalPlayerBiomeIntensityStandingOn = BiomeIntensity.Default;

	// Token: 0x040047F0 RID: 18416
	public WorldCreationData wcd;

	// Token: 0x040047F1 RID: 18417
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldState worldState;

	// Token: 0x040047F2 RID: 18418
	public ChunkManager m_ChunkManager;

	// Token: 0x040047F3 RID: 18419
	public SharedChunkObserverCache m_SharedChunkObserverCache;

	// Token: 0x040047F4 RID: 18420
	public readonly PrefabCache m_PrefabCache = new PrefabCache();

	// Token: 0x040047F5 RID: 18421
	public EventPrefabsClient m_EventPrefabsClient;

	// Token: 0x040047F6 RID: 18422
	public WorldEnvironment m_WorldEnvironment;

	// Token: 0x040047F7 RID: 18423
	public BiomeAtmosphereEffects BiomeAtmosphereEffects;

	// Token: 0x040047F8 RID: 18424
	public FlatAreaManager FlatAreaManager;

	// Token: 0x040047F9 RID: 18425
	public static bool IsSplatMapAvailable;

	// Token: 0x040047FA RID: 18426
	public List<SSpawnedEntity> Last4Spawned = new List<SSpawnedEntity>();

	// Token: 0x040047FB RID: 18427
	public int playerEntityUpdateCount;

	// Token: 0x040047FC RID: 18428
	public int clientLastEntityId;

	// Token: 0x040047FD RID: 18429
	public Transform EntitiesTransform;

	// Token: 0x040047FE RID: 18430
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom rand;

	// Token: 0x040047FF RID: 18431
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUnityTerrainConfigured;

	// Token: 0x04004800 RID: 18432
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayerLocal> m_LocalPlayerEntities = new List<EntityPlayerLocal>();

	// Token: 0x04004801 RID: 18433
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal m_LocalPlayerEntity;

	// Token: 0x04004802 RID: 18434
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> entitiesWithinAABBExcludingEntity = new List<Entity>();

	// Token: 0x04004803 RID: 18435
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> livingEntitiesWithinAABBExcludingEntity = new List<EntityAlive>();

	// Token: 0x04004804 RID: 18436
	[PublicizedFrom(EAccessModifier.Private)]
	public MapObjectManager objectsOnMap;

	// Token: 0x04004805 RID: 18437
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnManagerDynamic dynamicSpawnManager;

	// Token: 0x04004806 RID: 18438
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBlockTicker worldBlockTicker;

	// Token: 0x04004807 RID: 18439
	public const int SleeperVolumeWorldStateSaveVersion = 1;

	// Token: 0x04004808 RID: 18440
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextSleeperVolumeId;

	// Token: 0x04004809 RID: 18441
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, SleeperVolume> sleeperVolumes = new Dictionary<int, SleeperVolume>();

	// Token: 0x0400480A RID: 18442
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<VolumeKey, int> sleeperVolumeMap = new Dictionary<VolumeKey, int>();

	// Token: 0x0400480B RID: 18443
	public const int TriggerVolumeWorldStateSaveVersion = 1;

	// Token: 0x0400480C RID: 18444
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextTriggerVolumeId;

	// Token: 0x0400480D RID: 18445
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, TriggerVolume> triggerVolumes = new Dictionary<int, TriggerVolume>();

	// Token: 0x0400480E RID: 18446
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<VolumeKey, int> triggerVolumeMap = new Dictionary<VolumeKey, int>();

	// Token: 0x0400480F RID: 18447
	public const int WallVolumeWorldStateSaveVersion = 1;

	// Token: 0x04004810 RID: 18448
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextWallVolumeId;

	// Token: 0x04004811 RID: 18449
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, WallVolume> wallVolumes = new Dictionary<int, WallVolume>();

	// Token: 0x04004812 RID: 18450
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<VolumeKey, int> wallVolumeMap = new Dictionary<VolumeKey, int>();

	// Token: 0x04004813 RID: 18451
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector3i, object> blockData = new Dictionary<Vector3i, object>();

	// Token: 0x04004814 RID: 18452
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> newlyLoadedChunksThisUpdate = new List<long>();

	// Token: 0x04004815 RID: 18453
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, ulong> areaMasterChunksToLock = new Dictionary<long, ulong>();

	// Token: 0x04004816 RID: 18454
	public TriggerManager triggerManager;

	// Token: 0x04004817 RID: 18455
	public static bool BiomeProgressionEnabled = true;

	// Token: 0x04004818 RID: 18456
	public static bool TemperatureSurvival = true;

	// Token: 0x04004819 RID: 18457
	public static bool MapEnabled = true;

	// Token: 0x0400481A RID: 18458
	public static int BloodMoonWarningHour = 8;

	// Token: 0x0400481B RID: 18459
	public static float StormFrequency = 1f;

	// Token: 0x0400481C RID: 18460
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

	// Token: 0x0400481D RID: 18461
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] supportOffsets = new int[]
	{
		0, 1, 1, 1, 1, 0, 1, -1, 0, -1,
		-1, -1, -1, 0, -1, 1
	};

	// Token: 0x0400481E RID: 18462
	[PublicizedFrom(EAccessModifier.Private)]
	public MicroStopwatch msUnculling = new MicroStopwatch();

	// Token: 0x0400481F RID: 18463
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<Chunk> chunksToUncull = new HashSetList<Chunk>();

	// Token: 0x04004820 RID: 18464
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<Chunk> chunksToRegenerate = new HashSetList<Chunk>();

	// Token: 0x04004821 RID: 18465
	[PublicizedFrom(EAccessModifier.Private)]
	public World.ClipBlock[] _clipBlocks = new World.ClipBlock[32];

	// Token: 0x04004822 RID: 18466
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds[] _clipBounds = new Bounds[16];

	// Token: 0x04004823 RID: 18467
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCollCacheSize = 50;

	// Token: 0x04004824 RID: 18468
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue[,,] collBlockCache = new BlockValue[50, 50, 50];

	// Token: 0x04004825 RID: 18469
	[PublicizedFrom(EAccessModifier.Private)]
	public sbyte[,,] collDensityCache = new sbyte[50, 50, 50];

	// Token: 0x04004826 RID: 18470
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickEntityFrameCount;

	// Token: 0x04004827 RID: 18471
	[PublicizedFrom(EAccessModifier.Private)]
	public float tickEntityFrameCountAverage = 1f;

	// Token: 0x04004828 RID: 18472
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> tickEntityList = new List<Entity>();

	// Token: 0x04004829 RID: 18473
	[PublicizedFrom(EAccessModifier.Private)]
	public float tickEntityPartialTicks;

	// Token: 0x0400482A RID: 18474
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickEntityIndex;

	// Token: 0x0400482B RID: 18475
	[PublicizedFrom(EAccessModifier.Private)]
	public int tickEntitySliceCount;

	// Token: 0x0400482C RID: 18476
	[PublicizedFrom(EAccessModifier.Protected)]
	public Queue<Vector3i> fallingBlocks = new Queue<Vector3i>();

	// Token: 0x0400482D RID: 18477
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<Vector3i> fallingBlockSet = new HashSet<Vector3i>();

	// Token: 0x0400482E RID: 18478
	[PublicizedFrom(EAccessModifier.Protected)]
	public Queue<List<Vector3i>> fallingGroups = new Queue<List<Vector3i>>();

	// Token: 0x0400482F RID: 18479
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<Vector3i> groupedBlocks = new HashSet<Vector3i>();

	// Token: 0x04004830 RID: 18480
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<Vector3i> processedBlocks = new HashSet<Vector3i>();

	// Token: 0x04004831 RID: 18481
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> resetTempPositions = new List<Vector3i>();

	// Token: 0x04004832 RID: 18482
	[PublicizedFrom(EAccessModifier.Private)]
	public List<List<Vector3i>> resetTempGroups = new List<List<Vector3i>>();

	// Token: 0x04004833 RID: 18483
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Chunk> m_lpChunkList = new List<Chunk>();

	// Token: 0x04004834 RID: 18484
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWorldNavExtent = 2900;

	// Token: 0x04004835 RID: 18485
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWorldRWGBorder = 90;

	// Token: 0x04004836 RID: 18486
	public const float cEdgeHard = 50f;

	// Token: 0x04004837 RID: 18487
	public const float cEdgeSoft = 80f;

	// Token: 0x04004838 RID: 18488
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cEdgeMinWorldSize = 1024;

	// Token: 0x04004839 RID: 18489
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<TraderArea> traderAreas;

	// Token: 0x0400483A RID: 18490
	public static TraderAreaStates SandboxUseTraderArea = TraderAreaStates.Default;

	// Token: 0x0400483B RID: 18491
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTraderPlacingProtection = 2;

	// Token: 0x0400483C RID: 18492
	public bool isEventBloodMoon;

	// Token: 0x0400483D RID: 18493
	public ulong eventWorldTime;

	// Token: 0x0400483E RID: 18494
	public int WorldDay;

	// Token: 0x0400483F RID: 18495
	public int WorldHour;

	// Token: 0x04004840 RID: 18496
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<BlockValueRef> pendingUpgradeDowngradeBlocks = new HashSet<BlockValueRef>();

	// Token: 0x02000BFE RID: 3070
	// (Invoke) Token: 0x06005D21 RID: 23841
	public delegate void OnEntityLoadedDelegate(Entity _entity);

	// Token: 0x02000BFF RID: 3071
	// (Invoke) Token: 0x06005D25 RID: 23845
	public delegate void OnEntityUnloadedDelegate(Entity _entity, EnumRemoveEntityReason _reason);

	// Token: 0x02000C00 RID: 3072
	[PublicizedFrom(EAccessModifier.Private)]
	public class ClipBlock
	{
		// Token: 0x06005D28 RID: 23848 RVA: 0x0024745F File Offset: 0x0024565F
		public static void ResetStorage()
		{
			World.ClipBlock._storageIndex = 0;
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x00247468 File Offset: 0x00245668
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

		// Token: 0x06005D2A RID: 23850 RVA: 0x002474B0 File Offset: 0x002456B0
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

		// Token: 0x04004841 RID: 18497
		public const int kMaxBlocks = 32;

		// Token: 0x04004842 RID: 18498
		public BlockValue value;

		// Token: 0x04004843 RID: 18499
		public Vector3 pos;

		// Token: 0x04004844 RID: 18500
		public Block block;

		// Token: 0x04004845 RID: 18501
		public Vector3 bmins;

		// Token: 0x04004846 RID: 18502
		public Vector3 bmaxs;

		// Token: 0x04004847 RID: 18503
		[PublicizedFrom(EAccessModifier.Private)]
		public static int _storageIndex = 0;

		// Token: 0x04004848 RID: 18504
		[PublicizedFrom(EAccessModifier.Private)]
		public static World.ClipBlock[] _storage = new World.ClipBlock[32];
	}

	// Token: 0x02000C01 RID: 3073
	public struct GroupBounds
	{
		// Token: 0x06005D2D RID: 23853 RVA: 0x0024753C File Offset: 0x0024573C
		public bool IsWithinSize(int maxSize)
		{
			return this.max.x - this.min.x + 1 <= maxSize && this.max.y - this.min.y + 1 <= maxSize && this.max.z - this.min.z + 1 <= maxSize;
		}

		// Token: 0x04004849 RID: 18505
		public Vector3i min;

		// Token: 0x0400484A RID: 18506
		public Vector3i max;
	}

	// Token: 0x02000C02 RID: 3074
	public enum WorldEvent
	{
		// Token: 0x0400484C RID: 18508
		BloodMoon
	}
}
