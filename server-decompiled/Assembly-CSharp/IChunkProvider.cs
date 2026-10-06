using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B58 RID: 2904
public interface IChunkProvider
{
	// Token: 0x0600578D RID: 22413
	IEnumerator Init(World _worldData);

	// Token: 0x0600578E RID: 22414
	void Update();

	// Token: 0x0600578F RID: 22415
	void StopUpdate();

	// Token: 0x06005790 RID: 22416
	void Cleanup();

	// Token: 0x06005791 RID: 22417
	void RequestChunk(int _x, int _y);

	// Token: 0x06005792 RID: 22418
	HashSetList<long> GetRequestedChunks();

	// Token: 0x06005793 RID: 22419
	void SaveAll();

	// Token: 0x06005794 RID: 22420
	void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet);

	// Token: 0x06005795 RID: 22421
	void ReloadAllChunks();

	// Token: 0x06005796 RID: 22422
	void ClearCaches();

	// Token: 0x06005797 RID: 22423
	EnumChunkProviderId GetProviderId();

	// Token: 0x06005798 RID: 22424
	void UnloadChunk(Chunk _chunk);

	// Token: 0x06005799 RID: 22425
	DynamicPrefabDecorator GetDynamicPrefabDecorator();

	// Token: 0x0600579A RID: 22426
	EventPrefabs GetEventPrefabs();

	// Token: 0x0600579B RID: 22427
	SpawnPointList GetSpawnPointList();

	// Token: 0x0600579C RID: 22428
	void SetSpawnPointList(SpawnPointList _spawnPointList);

	// Token: 0x0600579D RID: 22429
	bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors);

	// Token: 0x0600579E RID: 22430
	void SetDecorationsEnabled(bool _bEnable);

	// Token: 0x0600579F RID: 22431
	bool IsDecorationsEnabled();

	// Token: 0x060057A0 RID: 22432
	bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize);

	// Token: 0x060057A1 RID: 22433
	BoundsInt GetWorldBounds();

	// Token: 0x060057A2 RID: 22434
	Vector2i GetWorldSize();

	// Token: 0x060057A3 RID: 22435
	IBiomeProvider GetBiomeProvider();

	// Token: 0x060057A4 RID: 22436
	ITerrainGenerator GetTerrainGenerator();

	// Token: 0x060057A5 RID: 22437
	int GetPOIBlockIdOverride(int x, int z);

	// Token: 0x060057A6 RID: 22438
	float GetPOIHeightOverride(int x, int z);

	// Token: 0x060057A7 RID: 22439
	IEnumerator FillOccupiedMap(int xStart, int zStart, DecoOccupiedMap occupiedMap, List<PrefabInstance> overridePOIList = null);

	// Token: 0x060057A8 RID: 22440
	void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset);

	// Token: 0x060057A9 RID: 22441
	ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos);

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x060057AA RID: 22442
	GameUtils.WorldInfo WorldInfo { get; }
}
