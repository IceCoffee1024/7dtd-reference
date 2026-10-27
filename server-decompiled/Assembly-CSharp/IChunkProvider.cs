using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B63 RID: 2915
public interface IChunkProvider
{
	// Token: 0x060057B2 RID: 22450
	IEnumerator Init(World _worldData);

	// Token: 0x060057B3 RID: 22451
	void Update();

	// Token: 0x060057B4 RID: 22452
	void StopUpdate();

	// Token: 0x060057B5 RID: 22453
	void Cleanup();

	// Token: 0x060057B6 RID: 22454
	void RequestChunk(int _x, int _y);

	// Token: 0x060057B7 RID: 22455
	HashSetList<long> GetRequestedChunks();

	// Token: 0x060057B8 RID: 22456
	void SaveAll();

	// Token: 0x060057B9 RID: 22457
	void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet);

	// Token: 0x060057BA RID: 22458
	void ReloadAllChunks();

	// Token: 0x060057BB RID: 22459
	void ClearCaches();

	// Token: 0x060057BC RID: 22460
	EnumChunkProviderId GetProviderId();

	// Token: 0x060057BD RID: 22461
	void UnloadChunk(Chunk _chunk);

	// Token: 0x060057BE RID: 22462
	DynamicPrefabDecorator GetDynamicPrefabDecorator();

	// Token: 0x060057BF RID: 22463
	EventPrefabs GetEventPrefabs();

	// Token: 0x060057C0 RID: 22464
	SpawnPointList GetSpawnPointList();

	// Token: 0x060057C1 RID: 22465
	void SetSpawnPointList(SpawnPointList _spawnPointList);

	// Token: 0x060057C2 RID: 22466
	bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors);

	// Token: 0x060057C3 RID: 22467
	void SetDecorationsEnabled(bool _bEnable);

	// Token: 0x060057C4 RID: 22468
	bool IsDecorationsEnabled();

	// Token: 0x060057C5 RID: 22469
	bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize);

	// Token: 0x060057C6 RID: 22470
	BoundsInt GetWorldBounds();

	// Token: 0x060057C7 RID: 22471
	Vector2i GetWorldSize();

	// Token: 0x060057C8 RID: 22472
	IBiomeProvider GetBiomeProvider();

	// Token: 0x060057C9 RID: 22473
	ITerrainGenerator GetTerrainGenerator();

	// Token: 0x060057CA RID: 22474
	int GetPOIBlockIdOverride(int x, int z);

	// Token: 0x060057CB RID: 22475
	float GetPOIHeightOverride(int x, int z);

	// Token: 0x060057CC RID: 22476
	IEnumerator FillOccupiedMap(int xStart, int zStart, DecoOccupiedMap occupiedMap, List<PrefabInstance> overridePOIList = null);

	// Token: 0x060057CD RID: 22477
	void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset);

	// Token: 0x060057CE RID: 22478
	ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos);

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x060057CF RID: 22479
	GameUtils.WorldInfo WorldInfo { get; }
}
