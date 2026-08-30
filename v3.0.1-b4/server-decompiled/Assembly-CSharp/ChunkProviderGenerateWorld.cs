using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

// Token: 0x02000B1E RID: 2846
public abstract class ChunkProviderGenerateWorld : ChunkProviderAbstract
{
	// Token: 0x0600562E RID: 22062 RVA: 0x0021116C File Offset: 0x0020F36C
	public ChunkProviderGenerateWorld(string _levelName, PathAbstractions.AbstractedLocation _worldLocation, bool _bClientMode = false)
	{
		this.bClientMode = _bClientMode;
		this.levelName = _levelName;
		this.worldLocation = _worldLocation;
		this.bDecorationsEnabled = true;
	}

	// Token: 0x0600562F RID: 22063 RVA: 0x002111D3 File Offset: 0x0020F3D3
	public override DynamicPrefabDecorator GetDynamicPrefabDecorator()
	{
		return this.prefabDecorator;
	}

	// Token: 0x06005630 RID: 22064 RVA: 0x002111DB File Offset: 0x0020F3DB
	public override EventPrefabs GetEventPrefabs()
	{
		return this.eventPrefabs;
	}

	// Token: 0x06005631 RID: 22065 RVA: 0x002111E3 File Offset: 0x0020F3E3
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

	// Token: 0x06005632 RID: 22066 RVA: 0x002111FC File Offset: 0x0020F3FC
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

	// Token: 0x06005633 RID: 22067 RVA: 0x0021128C File Offset: 0x0020F48C
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

	// Token: 0x06005634 RID: 22068 RVA: 0x002113A0 File Offset: 0x0020F5A0
	public override void ClearCaches()
	{
		if (this.bClientMode)
		{
			return;
		}
		this.m_RegionFileManager.ClearCaches();
	}

	// Token: 0x06005635 RID: 22069 RVA: 0x002113B6 File Offset: 0x0020F5B6
	public HashSetLong ResetAllChunks(ChunkProtectionLevel excludedProtectionLevels, EnumResetUnprotectedChunksGroupingMode _groupingMode = EnumResetUnprotectedChunksGroupingMode.GroupedPOIs)
	{
		return this.m_RegionFileManager.ResetAllChunks(excludedProtectionLevels, _groupingMode);
	}

	// Token: 0x06005636 RID: 22070 RVA: 0x002113C5 File Offset: 0x0020F5C5
	public HashSetLong ResetRegion(int _regionX, int _regionZ, ChunkProtectionLevel excludedProtectionLevels, EnumResetUnprotectedChunksGroupingMode _groupingMode = EnumResetUnprotectedChunksGroupingMode.GroupedPOIs)
	{
		return this.m_RegionFileManager.ResetRegion(_regionX, _regionZ, excludedProtectionLevels, _groupingMode);
	}

	// Token: 0x06005637 RID: 22071 RVA: 0x002113D7 File Offset: 0x0020F5D7
	public void RequestChunkReset(long _chunkKey)
	{
		this.m_RegionFileManager.RequestChunkReset(_chunkKey);
	}

	// Token: 0x06005638 RID: 22072 RVA: 0x002113E5 File Offset: 0x0020F5E5
	public void MainThreadCacheProtectedPositions()
	{
		this.m_RegionFileManager.MainThreadCacheProtectedPositions();
	}

	// Token: 0x06005639 RID: 22073 RVA: 0x002113F2 File Offset: 0x0020F5F2
	public void SaveChunkAgeDebugTexture(float rangeInDays)
	{
		this.m_RegionFileManager.SaveChunkAgeDebugTexture(rangeInDays);
	}

	// Token: 0x0600563A RID: 22074 RVA: 0x00211400 File Offset: 0x0020F600
	public void IterateChunkExpiryTimes(Action<long, ulong> action)
	{
		this.m_RegionFileManager.IterateChunkExpiryTimes(action);
	}

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x0600563B RID: 22075 RVA: 0x0021140E File Offset: 0x0020F60E
	public IReadOnlyCollection<LongSetGroups.Group> ChunkGroups
	{
		get
		{
			return this.m_RegionFileManager.ChunkGroups;
		}
	}

	// Token: 0x0600563C RID: 22076 RVA: 0x0021141C File Offset: 0x0020F61C
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

	// Token: 0x0600563D RID: 22077 RVA: 0x002114A0 File Offset: 0x0020F6A0
	public override HashSetList<long> GetRequestedChunks()
	{
		return this.m_ChunkQueue;
	}

	// Token: 0x0600563E RID: 22078 RVA: 0x002114A8 File Offset: 0x0020F6A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void generateTerrain(World _world, Chunk _chunk, GameRandom _random)
	{
		this.m_TerrainGenerator.GenerateTerrain(_world, _chunk, _random, Vector3i.zero, Vector3i.zero, false, false);
	}

	// Token: 0x0600563F RID: 22079 RVA: 0x002114C4 File Offset: 0x0020F6C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void generateTerrain(World _world, Chunk _chunk, GameRandom _random, Vector3i _areaStart, Vector3i _areaSize, bool _bFillEmptyBlocks, bool _isReset)
	{
		this.m_TerrainGenerator.GenerateTerrain(_world, _chunk, _random, _areaStart, _areaSize, _bFillEmptyBlocks, _isReset);
	}

	// Token: 0x06005640 RID: 22080 RVA: 0x002114DC File Offset: 0x0020F6DC
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

	// Token: 0x06005641 RID: 22081 RVA: 0x00211681 File Offset: 0x0020F881
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnChunkSyncedAndDecorated(Chunk chunk)
	{
		WaterSimulationNative.Instance.InitializeChunk(chunk);
	}

	// Token: 0x06005642 RID: 22082 RVA: 0x00211690 File Offset: 0x0020F890
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

	// Token: 0x06005643 RID: 22083 RVA: 0x002116F6 File Offset: 0x0020F8F6
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

	// Token: 0x06005644 RID: 22084 RVA: 0x00211718 File Offset: 0x0020F918
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

	// Token: 0x06005645 RID: 22085 RVA: 0x00211840 File Offset: 0x0020FA40
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

	// Token: 0x06005646 RID: 22086 RVA: 0x00211AEC File Offset: 0x0020FCEC
	public void UpdateDecorations(Chunk _chunk)
	{
		this.decorate(_chunk);
	}

	// Token: 0x06005647 RID: 22087 RVA: 0x00211AF8 File Offset: 0x0020FCF8
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

	// Token: 0x06005648 RID: 22088 RVA: 0x00211B62 File Offset: 0x0020FD62
	public override void UnloadChunk(Chunk _chunk)
	{
		if (this.bClientMode)
		{
			MemoryPools.PoolChunks.FreeSync(_chunk);
			return;
		}
		this.m_RegionFileManager.AddChunkSync(_chunk, false);
	}

	// Token: 0x06005649 RID: 22089 RVA: 0x00211B88 File Offset: 0x0020FD88
	public override void ReloadAllChunks()
	{
		object syncRoot = ((ICollection)this.m_ChunkQueue.list).SyncRoot;
		lock (syncRoot)
		{
			this.m_ChunkQueue.Clear();
		}
		this.m_RegionFileManager.Clear();
	}

	// Token: 0x0600564A RID: 22090 RVA: 0x00211BE4 File Offset: 0x0020FDE4
	public List<ChunkProviderParameter> GetParameters()
	{
		return this.m_Parameters;
	}

	// Token: 0x0600564B RID: 22091 RVA: 0x00211BEC File Offset: 0x0020FDEC
	public override void Update()
	{
		RegionFileManager regionFileManager = this.m_RegionFileManager;
		if (regionFileManager == null)
		{
			return;
		}
		regionFileManager.Update();
	}

	// Token: 0x0600564C RID: 22092 RVA: 0x00211BFE File Offset: 0x0020FDFE
	public override void StopUpdate()
	{
		if (this.threadInfo != null)
		{
			this.threadInfo.WaitForEnd(30);
			this.threadInfo = null;
		}
	}

	// Token: 0x0600564D RID: 22093 RVA: 0x00211C1C File Offset: 0x0020FE1C
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

	// Token: 0x0600564E RID: 22094 RVA: 0x00211C54 File Offset: 0x0020FE54
	public override bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors)
	{
		this.m_RegionFileManager.SetCacheSize(1000);
		new TerrainMapGenerator().GenerateTerrain(this);
		this.m_RegionFileManager.SetCacheSize(0);
		return true;
	}

	// Token: 0x0600564F RID: 22095 RVA: 0x00211C7E File Offset: 0x0020FE7E
	public override IBiomeProvider GetBiomeProvider()
	{
		return this.m_BiomeProvider;
	}

	// Token: 0x06005650 RID: 22096 RVA: 0x00211C86 File Offset: 0x0020FE86
	public override ITerrainGenerator GetTerrainGenerator()
	{
		return this.m_TerrainGenerator;
	}

	// Token: 0x06005651 RID: 22097 RVA: 0x00211C8E File Offset: 0x0020FE8E
	public override SpawnPointList GetSpawnPointList()
	{
		return this.spawnPointManager.spawnPointList;
	}

	// Token: 0x06005652 RID: 22098 RVA: 0x00211C9B File Offset: 0x0020FE9B
	public override void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
		this.spawnPointManager.spawnPointList = _spawnPointList;
	}

	// Token: 0x06005653 RID: 22099 RVA: 0x00211CAC File Offset: 0x0020FEAC
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

	// Token: 0x06005654 RID: 22100 RVA: 0x00211D5C File Offset: 0x0020FF5C
	public void RemoveChunks(HashSetLong _chunks)
	{
		if (this.m_RegionFileManager != null)
		{
			this.m_RegionFileManager.RemoveChunks(_chunks, true, true);
		}
	}

	// Token: 0x06005655 RID: 22101 RVA: 0x00211D74 File Offset: 0x0020FF74
	public override ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos)
	{
		return this.m_RegionFileManager.GetChunkProtectionLevelForWorldPos(worldPos);
	}

	// Token: 0x06005656 RID: 22102 RVA: 0x00211D82 File Offset: 0x0020FF82
	public void CheckPersistentData()
	{
		this.m_RegionFileManager.CheckPersistentData();
	}

	// Token: 0x06005657 RID: 22103 RVA: 0x00211D90 File Offset: 0x0020FF90
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

	// Token: 0x04004292 RID: 17042
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int cCacheChunks = 0;

	// Token: 0x04004293 RID: 17043
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;

	// Token: 0x04004294 RID: 17044
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly PathAbstractions.AbstractedLocation worldLocation;

	// Token: 0x04004295 RID: 17045
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string levelName;

	// Token: 0x04004296 RID: 17046
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string gameName;

	// Token: 0x04004297 RID: 17047
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicPrefabDecorator prefabDecorator;

	// Token: 0x04004298 RID: 17048
	[PublicizedFrom(EAccessModifier.Protected)]
	public EventPrefabs eventPrefabs;

	// Token: 0x04004299 RID: 17049
	[PublicizedFrom(EAccessModifier.Protected)]
	public SpawnPointManager spawnPointManager;

	// Token: 0x0400429A RID: 17050
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFileManager m_RegionFileManager;

	// Token: 0x0400429B RID: 17051
	[PublicizedFrom(EAccessModifier.Protected)]
	public IBiomeProvider m_BiomeProvider;

	// Token: 0x0400429C RID: 17052
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<ChunkProviderParameter> m_Parameters = new List<ChunkProviderParameter>();

	// Token: 0x0400429D RID: 17053
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<IWorldDecorator> m_Decorators = new List<IWorldDecorator>();

	// Token: 0x0400429E RID: 17054
	[PublicizedFrom(EAccessModifier.Protected)]
	public ITerrainGenerator m_TerrainGenerator;

	// Token: 0x0400429F RID: 17055
	public HashSetList<long> m_ChunkQueue = new HashSetList<long>();

	// Token: 0x040042A0 RID: 17056
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent m_WaitHandle = new AutoResetEvent(false);

	// Token: 0x040042A1 RID: 17057
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo threadInfo;

	// Token: 0x040042A2 RID: 17058
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch chunkGenerationTimer = new Stopwatch();

	// Token: 0x040042A3 RID: 17059
	[PublicizedFrom(EAccessModifier.Private)]
	public long currentGeneratingChunk;

	// Token: 0x040042A4 RID: 17060
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bClientMode;
}
