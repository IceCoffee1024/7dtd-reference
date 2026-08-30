using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B34 RID: 2868
public interface IChunkProvider
{
	// Token: 0x060056EB RID: 22251
	IEnumerator Init(World _worldData);

	// Token: 0x060056EC RID: 22252
	void Update();

	// Token: 0x060056ED RID: 22253
	void StopUpdate();

	// Token: 0x060056EE RID: 22254
	void Cleanup();

	// Token: 0x060056EF RID: 22255
	void RequestChunk(int _x, int _y);

	// Token: 0x060056F0 RID: 22256
	HashSetList<long> GetRequestedChunks();

	// Token: 0x060056F1 RID: 22257
	void SaveAll();

	// Token: 0x060056F2 RID: 22258
	void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet);

	// Token: 0x060056F3 RID: 22259
	void ReloadAllChunks();

	// Token: 0x060056F4 RID: 22260
	void ClearCaches();

	// Token: 0x060056F5 RID: 22261
	EnumChunkProviderId GetProviderId();

	// Token: 0x060056F6 RID: 22262
	void UnloadChunk(Chunk _chunk);

	// Token: 0x060056F7 RID: 22263
	DynamicPrefabDecorator GetDynamicPrefabDecorator();

	// Token: 0x060056F8 RID: 22264
	EventPrefabs GetEventPrefabs();

	// Token: 0x060056F9 RID: 22265
	SpawnPointList GetSpawnPointList();

	// Token: 0x060056FA RID: 22266
	void SetSpawnPointList(SpawnPointList _spawnPointList);

	// Token: 0x060056FB RID: 22267
	bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors);

	// Token: 0x060056FC RID: 22268
	void SetDecorationsEnabled(bool _bEnable);

	// Token: 0x060056FD RID: 22269
	bool IsDecorationsEnabled();

	// Token: 0x060056FE RID: 22270
	bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize);

	// Token: 0x060056FF RID: 22271
	BoundsInt GetWorldBounds();

	// Token: 0x06005700 RID: 22272
	Vector2i GetWorldSize();

	// Token: 0x06005701 RID: 22273
	IBiomeProvider GetBiomeProvider();

	// Token: 0x06005702 RID: 22274
	ITerrainGenerator GetTerrainGenerator();

	// Token: 0x06005703 RID: 22275
	int GetPOIBlockIdOverride(int x, int z);

	// Token: 0x06005704 RID: 22276
	float GetPOIHeightOverride(int x, int z);

	// Token: 0x06005705 RID: 22277
	IEnumerator FillOccupiedMap(int xStart, int zStart, DecoOccupiedMap occupiedMap, List<PrefabInstance> overridePOIList = null);

	// Token: 0x06005706 RID: 22278
	void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset);

	// Token: 0x06005707 RID: 22279
	ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos);

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06005708 RID: 22280
	GameUtils.WorldInfo WorldInfo { get; }
}
