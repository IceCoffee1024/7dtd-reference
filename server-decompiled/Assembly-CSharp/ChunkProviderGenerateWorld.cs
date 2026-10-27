using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

// Token: 0x02000B4D RID: 2893
public abstract class ChunkProviderGenerateWorld : ChunkProviderAbstract
{
	// Token: 0x060056F5 RID: 22261 RVA: 0x00218670 File Offset: 0x00216870
	public ChunkProviderGenerateWorld(string _levelName, PathAbstractions.AbstractedLocation _worldLocation, bool _bClientMode = false)
	{
		this.bClientMode = _bClientMode;
		this.levelName = _levelName;
		this.worldLocation = _worldLocation;
		this.bDecorationsEnabled = true;
	}

	// Token: 0x060056F6 RID: 22262 RVA: 0x002186D7 File Offset: 0x002168D7
	public override DynamicPrefabDecorator GetDynamicPrefabDecorator()
	{
		return this.prefabDecorator;
	}

	// Token: 0x060056F7 RID: 22263 RVA: 0x002186DF File Offset: 0x002168DF
	public override EventPrefabs GetEventPrefabs()
	{
		return this.eventPrefabs;
	}

	// Token: 0x060056F8 RID: 22264 RVA: 0x002186E7 File Offset: 0x002168E7
	public override IEnumerator Init(World _world)
	{
		this.world = _world;
		this.prefabDecorator = new DynamicPrefabDecorator(_world.m_PrefabCache);
		yield return this.prefabDecorator.Load(this.worldLocation.FullPath, false);
		yield return null;
		this.spawnPointManager = new SpawnPointManager(true);
		if (!this.bClientMode)
		{
			yield return null;
			this.spawnPointManager.Load(this.worldLocation.FullPath);
		}
		if (!this.bClientMode)
		{
			this.threadInfo = ThreadManager.StartThread("GenerateChunks", null, new ThreadManager.ThreadFunctionLoopDelegate(this.GenerateChunksThread), null, null, null, true, false);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060056F9 RID: 22265 RVA: 0x00218700 File Offset: 0x00216900
	public override void SaveAll()
	{
		if (this.bClientMode)
		{
			return;
		}
		if (this.world.IsEditor())
		{
			this.GetDynamicPrefabDecorator().Save(this.worldLocation.FullPath);
			this.spawnPointManager.Save(this.worldLocation.FullPath);
			return;
		}
		if (this.m_RegionFileManager != null)
		{
			this.m_RegionFileManager.MakePersistent(this.world.ChunkCache, false);
			this.m_RegionFileManager.WaitSaveDone();
		}
		EventPrefabs eventPrefabs = this.GetEventPrefabs();
		if (eventPrefabs == null)
		{
			return;
		}
		eventPrefabs.Save(true);
	}

	// Token: 0x060056FA RID: 22266 RVA: 0x00218790 File Offset: 0x00216990
	public override void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet)
	{
		if (this.bClientMode)
		{
			return;
		}
		GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
		int i = _activeChunkSet.Offset;
		while (i < _activeChunkSet.Count)
		{
			Chunk chunkSync = this.world.ChunkCache.GetChunkSync(_activeChunkSet.Array[i]);
			if (chunkSync != null && chunkSync.NeedsSaving && !chunkSync.NeedsDecoration && !chunkSync.InProgressDecorating && !chunkSync.NeedsLightCalculation && !chunkSync.InProgressLighting && _curWorldTimeInTicks - chunkSync.SavedInWorldTicks > 400UL && gameRandom.RandomFloat < 0.3f)
			{
				Chunk chunk = chunkSync;
				lock (chunk)
				{
					if (chunkSync.IsLocked)
					{
						goto IL_00E5;
					}
					chunkSync.InProgressSaving = true;
				}
				this.m_RegionFileManager.SaveChunkSnapshot(chunkSync, false);
				count--;
				chunkSync.InProgressSaving = false;
				goto IL_00E1;
			}
			goto IL_00E1;
			IL_00E5:
			i++;
			continue;
			IL_00E1:
			if (count > 0)
			{
				goto IL_00E5;
			}
			break;
		}
	}

	// Token: 0x060056FB RID: 22267 RVA: 0x002188A4 File Offset: 0x00216AA4
	public override void ClearCaches()
	{
		if (this.bClientMode)
		{
			return;
		}
		this.m_RegionFileManager.ClearCaches();
	}

	// Token: 0x060056FC RID: 22268 RVA: 0x002188BA File Offset: 0x00216ABA
	public HashSetLong ResetAllChunks(ChunkProtectionLevel excludedProtectionLevels, EnumResetUnprotectedChunksGroupingMode _groupingMode = EnumResetUnprotectedChunksGroupingMode.GroupedPOIs)
	{
		return this.m_RegionFileManager.ResetAllChunks(excludedProtectionLevels, _groupingMode);
	}

	// Token: 0x060056FD RID: 22269 RVA: 0x002188C9 File Offset: 0x00216AC9
	public HashSetLong ResetRegion(int _regionX, int _regionZ, ChunkProtectionLevel excludedProtectionLevels, EnumResetUnprotectedChunksGroupingMode _groupingMode = EnumResetUnprotectedChunksGroupingMode.GroupedPOIs)
	{
		return this.m_RegionFileManager.ResetRegion(_regionX, _regionZ, excludedProtectionLevels, _groupingMode);
	}

	// Token: 0x060056FE RID: 22270 RVA: 0x002188DB File Offset: 0x00216ADB
	public void RequestChunkReset(long _chunkKey)
	{
		this.m_RegionFileManager.RequestChunkReset(_chunkKey);
	}

	// Token: 0x060056FF RID: 22271 RVA: 0x002188E9 File Offset: 0x00216AE9
	public void MainThreadCacheProtectedPositions()
	{
		this.m_RegionFileManager.MainThreadCacheProtectedPositions();
	}

	// Token: 0x06005700 RID: 22272 RVA: 0x002188F6 File Offset: 0x00216AF6
	public void SaveChunkAgeDebugTexture(float rangeInDays)
	{
		this.m_RegionFileManager.SaveChunkAgeDebugTexture(rangeInDays);
	}

	// Token: 0x06005701 RID: 22273 RVA: 0x00218904 File Offset: 0x00216B04
	public void IterateChunkExpiryTimes(Action<long, ulong> action)
	{
		this.m_RegionFileManager.IterateChunkExpiryTimes(action);
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x06005702 RID: 22274 RVA: 0x00218912 File Offset: 0x00216B12
	public IReadOnlyCollection<LongSetGroups.Group> ChunkGroups
	{
		get
		{
			return this.m_RegionFileManager.ChunkGroups;
		}
	}

	// Token: 0x06005703 RID: 22275 RVA: 0x00218920 File Offset: 0x00216B20
	public override void RequestChunk(int _x, int _y)
	{
		if (this.bClientMode)
		{
			return;
		}
		object syncRoot = ((ICollection)this.m_ChunkQueue.list).SyncRoot;
		lock (syncRoot)
		{
			long num = WorldChunkCache.MakeChunkKey(_x, _y);
			if (this.m_ChunkQueue.hashSet.Contains(num))
			{
				return;
			}
			this.m_ChunkQueue.Add(num);
		}
		this.m_WaitHandle.Set();
	}

	// Token: 0x06005704 RID: 22276 RVA: 0x002189A4 File Offset: 0x00216BA4
	public override HashSetList<long> GetRequestedChunks()
	{
		return this.m_ChunkQueue;
	}

	// Token: 0x06005705 RID: 22277 RVA: 0x002189AC File Offset: 0x00216BAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void generateTerrain(World _world, Chunk _chunk, GameRandom _random)
	{
		this.m_TerrainGenerator.GenerateTerrain(_world, _chunk, _random, Vector3i.zero, Vector3i.zero, false, false);
	}

	// Token: 0x06005706 RID: 22278 RVA: 0x002189C8 File Offset: 0x00216BC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void generateTerrain(World _world, Chunk _chunk, GameRandom _random, Vector3i _areaStart, Vector3i _areaSize, bool _bFillEmptyBlocks, bool _isReset)
	{
		this.m_TerrainGenerator.GenerateTerrain(_world, _chunk, _random, _areaStart, _areaSize, _bFillEmptyBlocks, _isReset);
	}

	// Token: 0x06005707 RID: 22279 RVA: 0x002189E0 File Offset: 0x00216BE0
	public bool GenerateSingleChunk(ChunkCluster cc, long key, bool _forceRebuild = false)
	{
		this.currentGeneratingChunk = key;
		this.chunkGenerationTimer.Restart();
		if (!_forceRebuild && cc.ContainsChunkSync(key))
		{
			this.currentGeneratingChunk = 0L;
			return false;
		}
		Chunk chunk = null;
		if (this.m_RegionFileManager.ContainsChunkSync(key))
		{
			chunk = this.m_RegionFileManager.GetChunkSync(key);
			this.m_RegionFileManager.RemoveChunkSync(key);
		}
		if (_forceRebuild)
		{
			chunk = cc.GetChunkSync(key);
			if (chunk != null)
			{
				chunk.RemoveBlockEntityTransforms();
				chunk.Reset();
			}
		}
		if (_forceRebuild || chunk == null)
		{
			int num = WorldChunkCache.extractX(key);
			int num2 = WorldChunkCache.extractZ(key);
			if (chunk == null)
			{
				chunk = MemoryPools.PoolChunks.AllocSync(true);
			}
			if (chunk != null)
			{
				chunk.X = num;
				chunk.Z = num2;
				GameRandom gameRandom = Utils.RandomFromSeedOnPos(num, num2, this.world.Seed);
				this.generateTerrain(this.world, chunk, gameRandom);
				GameRandomManager.Instance.FreeGameRandom(gameRandom);
				if (!this.bDecorationsEnabled)
				{
					chunk.NeedsDecoration = false;
					chunk.NeedsLightCalculation = false;
					chunk.NeedsRegeneration = true;
				}
				if (this.bDecorationsEnabled)
				{
					chunk.NeedsDecoration = true;
					chunk.NeedsLightCalculation = true;
					if (this.GetDynamicPrefabDecorator() != null)
					{
						this.GetDynamicPrefabDecorator().DecorateChunk(this.world, chunk);
					}
				}
			}
		}
		bool flag = false;
		if (chunk != null)
		{
			if (!_forceRebuild)
			{
				flag = cc.AddChunkSync(chunk, false);
			}
			else
			{
				ReaderWriterLockSlim syncRoot = cc.GetSyncRoot();
				syncRoot.EnterUpgradeableReadLock();
				if (cc.ContainsChunkSync(key))
				{
					cc.RemoveChunkSync(key);
				}
				flag = cc.AddChunkSync(chunk, false);
				syncRoot.ExitUpgradeableReadLock();
			}
			if (flag)
			{
				if (!chunk.NeedsDecoration)
				{
					ChunkProviderGenerateWorld.OnChunkSyncedAndDecorated(chunk);
				}
				this.updateDecorationsWherePossible(chunk);
				if (_forceRebuild)
				{
					chunk.isModified = true;
				}
			}
			else
			{
				MemoryPools.PoolChunks.FreeSync(chunk);
			}
		}
		return flag;
	}

	// Token: 0x06005708 RID: 22280 RVA: 0x00218B85 File Offset: 0x00216D85
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnChunkSyncedAndDecorated(Chunk chunk)
	{
		WaterSimulationNative.Instance.InitializeChunk(chunk);
	}

	// Token: 0x06005709 RID: 22281 RVA: 0x00218B94 File Offset: 0x00216D94
	[PublicizedFrom(EAccessModifier.Private)]
	public int GenerateChunksThread(ThreadManager.ThreadInfo _threadInfo)
	{
		if (_threadInfo.TerminationRequested())
		{
			return -1;
		}
		if (this.m_RegionFileManager == null)
		{
			return 15;
		}
		long num = this.world.GetNextChunkToProvide();
		if (num == 9223372036854775807L)
		{
			num = DynamicMeshThread.GetNextChunkToLoad();
			if (num == 9223372036854775807L)
			{
				return 15;
			}
		}
		ChunkCluster chunkCache = this.world.ChunkCache;
		this.GenerateSingleChunk(chunkCache, num, false);
		return 0;
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x00218BFA File Offset: 0x00216DFA
	[PublicizedFrom(EAccessModifier.Private)]
	public void tryToDecorate(Chunk _chunk)
	{
		if (_chunk == null)
		{
			return;
		}
		if (!_chunk.NeedsDecoration)
		{
			return;
		}
		if (_chunk.IsLocked)
		{
			return;
		}
		this.decorate(_chunk);
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x00218C1C File Offset: 0x00216E1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void decorate(Chunk _chunk)
	{
		int x = _chunk.X;
		int z = _chunk.Z;
		Chunk chunk;
		Chunk chunk2;
		Chunk chunk3;
		if ((chunk = (Chunk)this.world.GetChunkSync(x + 1, z + 1)) == null || (chunk2 = (Chunk)this.world.GetChunkSync(x, z + 1)) == null || (chunk3 = (Chunk)this.world.GetChunkSync(x + 1, z)) == null)
		{
			return;
		}
		chunk.InProgressDecorating = true;
		chunk2.InProgressDecorating = true;
		chunk3.InProgressDecorating = true;
		_chunk.InProgressDecorating = true;
		this.updateDecosAllowedForChunk(_chunk, chunk3, chunk2);
		for (int i = 0; i < this.m_Decorators.Count; i++)
		{
			this.m_Decorators[i].DecorateChunkOverlapping(this.world, _chunk, chunk3, chunk2, chunk, this.world.Seed);
		}
		_chunk.OnDecorated();
		_chunk.ResetStability();
		_chunk.RefreshSunlight();
		_chunk.NeedsDecoration = false;
		_chunk.NeedsLightCalculation = true;
		chunk.InProgressDecorating = false;
		chunk2.InProgressDecorating = false;
		chunk3.InProgressDecorating = false;
		_chunk.InProgressDecorating = false;
		ChunkProviderGenerateWorld.OnChunkSyncedAndDecorated(_chunk);
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x00218D44 File Offset: 0x00216F44
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateDecosAllowedForChunk(Chunk _chunk, Chunk _c10, Chunk _c01)
	{
		Vector3 vector = new Vector3(0f, 0f, 1f);
		Vector3 vector2 = new Vector3(1f, 0f, 0f);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				int terrainHeight = (int)_chunk.GetTerrainHeight(j, i);
				int num = (int)((j < 15) ? _chunk.GetTerrainHeight(j + 1, i) : _c10.GetTerrainHeight(0, i));
				int num2 = (int)((i < 15) ? _chunk.GetTerrainHeight(j, i + 1) : _c01.GetTerrainHeight(j, 0));
				if (terrainHeight >= 253 || num >= 253 || num2 >= 253)
				{
					_chunk.SetDecoAllowedAt(j, i, EnumDecoAllowed.Nothing);
				}
				else
				{
					float num3 = (float)_chunk.GetDensity(j, terrainHeight, i) / -128f;
					float num4 = ((j < 15) ? ((float)_chunk.GetDensity(j + 1, num, i) / -128f) : ((float)_c10.GetDensity(0, num, i) / -128f));
					float num5 = ((i < 15) ? ((float)_chunk.GetDensity(j, num2, i + 1) / -128f) : ((float)_c01.GetDensity(j, num2, 0) / -128f));
					float num6 = (float)_chunk.GetDensity(j, terrainHeight + 1, i) / 127f;
					float num7 = ((j < 15) ? ((float)_chunk.GetDensity(j + 1, num + 1, i) / 127f) : ((float)_c10.GetDensity(0, num + 1, i) / 127f));
					float num8 = ((i < 15) ? ((float)_chunk.GetDensity(j, num2 + 1, i + 1) / 127f) : ((float)_c01.GetDensity(j, num2 + 1, 0) / 127f));
					if (num3 > 0.999f && num6 > 0.999f)
					{
						num3 = 0.5f;
					}
					if (num4 > 0.999f && num7 > 0.999f)
					{
						num4 = 0.5f;
					}
					if (num5 > 0.999f && num8 > 0.999f)
					{
						num5 = 0.5f;
					}
					float num9 = (float)terrainHeight + num3;
					float num10 = (float)num + num4;
					float num11 = (float)num2 + num5;
					vector.y = num11 - num9;
					vector2.y = num10 - num9;
					Vector3 normalized = Vector3.Cross(vector, vector2).normalized;
					_chunk.SetTerrainNormal(j, i, normalized);
					if (normalized.y < 0.55f)
					{
						_chunk.SetDecoAllowedSlopeAt(j, i, EnumDecoAllowedSlope.Steep);
					}
					else if (normalized.y < 0.65f)
					{
						_chunk.SetDecoAllowedSlopeAt(j, i, EnumDecoAllowedSlope.Sloped);
					}
					if (terrainHeight <= 1 || terrainHeight >= 255 || _chunk.IsWater(j, terrainHeight, i) || _chunk.IsWater(j, terrainHeight + 1, i))
					{
						_chunk.SetDecoAllowedAt(j, i, EnumDecoAllowed.Nothing);
					}
				}
			}
		}
	}

	// Token: 0x0600570D RID: 22285 RVA: 0x00218FF0 File Offset: 0x002171F0
	public void UpdateDecorations(Chunk _chunk)
	{
		this.decorate(_chunk);
	}

	// Token: 0x0600570E RID: 22286 RVA: 0x00218FFC File Offset: 0x002171FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateDecorationsWherePossible(Chunk chunk)
	{
		World world = this.world;
		int x = chunk.X;
		int z = chunk.Z;
		this.tryToDecorate(chunk);
		this.tryToDecorate((Chunk)world.GetChunkSync(x - 1, z));
		this.tryToDecorate((Chunk)world.GetChunkSync(x, z - 1));
		this.tryToDecorate((Chunk)world.GetChunkSync(x - 1, z - 1));
	}

	// Token: 0x0600570F RID: 22287 RVA: 0x00219066 File Offset: 0x00217266
	public override void UnloadChunk(Chunk _chunk)
	{
		if (this.bClientMode)
		{
			MemoryPools.PoolChunks.FreeSync(_chunk);
			return;
		}
		this.m_RegionFileManager.AddChunkSync(_chunk, false);
	}

	// Token: 0x06005710 RID: 22288 RVA: 0x0021908C File Offset: 0x0021728C
	public override void ReloadAllChunks()
	{
		object syncRoot = ((ICollection)this.m_ChunkQueue.list).SyncRoot;
		lock (syncRoot)
		{
			this.m_ChunkQueue.Clear();
		}
		this.m_RegionFileManager.Clear();
	}

	// Token: 0x06005711 RID: 22289 RVA: 0x002190E8 File Offset: 0x002172E8
	public List<ChunkProviderParameter> GetParameters()
	{
		return this.m_Parameters;
	}

	// Token: 0x06005712 RID: 22290 RVA: 0x002190F0 File Offset: 0x002172F0
	public override void Update()
	{
		RegionFileManager regionFileManager = this.m_RegionFileManager;
		if (regionFileManager == null)
		{
			return;
		}
		regionFileManager.Update();
	}

	// Token: 0x06005713 RID: 22291 RVA: 0x00219102 File Offset: 0x00217302
	public override void StopUpdate()
	{
		if (this.threadInfo != null)
		{
			this.threadInfo.WaitForEnd(30);
			this.threadInfo = null;
		}
	}

	// Token: 0x06005714 RID: 22292 RVA: 0x00219120 File Offset: 0x00217320
	public override void Cleanup()
	{
		this.StopUpdate();
		if (this.spawnPointManager != null)
		{
			this.spawnPointManager.Cleanup();
		}
		if (this.m_RegionFileManager != null)
		{
			this.m_RegionFileManager.Cleanup();
		}
		MultiBlockManager.Instance.Cleanup();
	}

	// Token: 0x06005715 RID: 22293 RVA: 0x00219158 File Offset: 0x00217358
	public override bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors)
	{
		this.m_RegionFileManager.SetCacheSize(1000);
		new TerrainMapGenerator().GenerateTerrain(this);
		this.m_RegionFileManager.SetCacheSize(0);
		return true;
	}

	// Token: 0x06005716 RID: 22294 RVA: 0x00219182 File Offset: 0x00217382
	public override IBiomeProvider GetBiomeProvider()
	{
		return this.m_BiomeProvider;
	}

	// Token: 0x06005717 RID: 22295 RVA: 0x0021918A File Offset: 0x0021738A
	public override ITerrainGenerator GetTerrainGenerator()
	{
		return this.m_TerrainGenerator;
	}

	// Token: 0x06005718 RID: 22296 RVA: 0x00219192 File Offset: 0x00217392
	public override SpawnPointList GetSpawnPointList()
	{
		return this.spawnPointManager.spawnPointList;
	}

	// Token: 0x06005719 RID: 22297 RVA: 0x0021919F File Offset: 0x0021739F
	public override void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
		this.spawnPointManager.spawnPointList = _spawnPointList;
	}

	// Token: 0x0600571A RID: 22298 RVA: 0x002191B0 File Offset: 0x002173B0
	public override void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset)
	{
		ChunkCluster chunkCache = this.world.ChunkCache;
		foreach (long num in _chunks)
		{
			Chunk chunkSync = chunkCache.GetChunkSync(num);
			if (chunkSync != null)
			{
				GameRandom gameRandom = Utils.RandomFromSeedOnPos(chunkSync.X, chunkSync.Z, this.world.Seed);
				chunkSync.StopStabilityCalculation = _isStopStabilityCalc;
				this.generateTerrain(this.world, chunkSync, gameRandom, _areaStart, _areaSize, _isFillEmptyBlocks, _isReset);
				GameRandomManager.Instance.FreeGameRandom(gameRandom);
				if (_isRegenChunk)
				{
					chunkSync.NeedsRegeneration = true;
				}
			}
		}
	}

	// Token: 0x0600571B RID: 22299 RVA: 0x00219260 File Offset: 0x00217460
	public void RemoveChunks(HashSetLong _chunks)
	{
		if (this.m_RegionFileManager != null)
		{
			this.m_RegionFileManager.RemoveChunks(_chunks, true, true);
		}
	}

	// Token: 0x0600571C RID: 22300 RVA: 0x00219278 File Offset: 0x00217478
	public override ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos)
	{
		return this.m_RegionFileManager.GetChunkProtectionLevelForWorldPos(worldPos);
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x00219286 File Offset: 0x00217486
	public void CheckPersistentData()
	{
		this.m_RegionFileManager.CheckPersistentData();
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x00219294 File Offset: 0x00217494
	public void LogCurrentChunkGeneration()
	{
		Log.Error(string.Format("ChunkProvider Currently Generating Data for Chunk - Key: {0}, Pos: {1}/{2}, Duration {3}", new object[]
		{
			this.currentGeneratingChunk,
			WorldChunkCache.extractX(this.currentGeneratingChunk) << 4,
			WorldChunkCache.extractZ(this.currentGeneratingChunk) << 4,
			this.chunkGenerationTimer.Elapsed.TotalSeconds
		}));
	}

	// Token: 0x04004390 RID: 17296
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int cCacheChunks = 0;

	// Token: 0x04004391 RID: 17297
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;

	// Token: 0x04004392 RID: 17298
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly PathAbstractions.AbstractedLocation worldLocation;

	// Token: 0x04004393 RID: 17299
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string levelName;

	// Token: 0x04004394 RID: 17300
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string gameName;

	// Token: 0x04004395 RID: 17301
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicPrefabDecorator prefabDecorator;

	// Token: 0x04004396 RID: 17302
	[PublicizedFrom(EAccessModifier.Protected)]
	public EventPrefabs eventPrefabs;

	// Token: 0x04004397 RID: 17303
	[PublicizedFrom(EAccessModifier.Protected)]
	public SpawnPointManager spawnPointManager;

	// Token: 0x04004398 RID: 17304
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFileManager m_RegionFileManager;

	// Token: 0x04004399 RID: 17305
	[PublicizedFrom(EAccessModifier.Protected)]
	public IBiomeProvider m_BiomeProvider;

	// Token: 0x0400439A RID: 17306
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<ChunkProviderParameter> m_Parameters = new List<ChunkProviderParameter>();

	// Token: 0x0400439B RID: 17307
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<IWorldDecorator> m_Decorators = new List<IWorldDecorator>();

	// Token: 0x0400439C RID: 17308
	[PublicizedFrom(EAccessModifier.Protected)]
	public ITerrainGenerator m_TerrainGenerator;

	// Token: 0x0400439D RID: 17309
	public HashSetList<long> m_ChunkQueue = new HashSetList<long>();

	// Token: 0x0400439E RID: 17310
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent m_WaitHandle = new AutoResetEvent(false);

	// Token: 0x0400439F RID: 17311
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo threadInfo;

	// Token: 0x040043A0 RID: 17312
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch chunkGenerationTimer = new Stopwatch();

	// Token: 0x040043A1 RID: 17313
	[PublicizedFrom(EAccessModifier.Private)]
	public long currentGeneratingChunk;

	// Token: 0x040043A2 RID: 17314
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bClientMode;
}
