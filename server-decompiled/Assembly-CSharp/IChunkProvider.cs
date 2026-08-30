using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
public interface IChunkProvider
{
	// Token: 0x06005773 RID: 22387
	IEnumerator Init(World _worldData);

	// Token: 0x06005774 RID: 22388
	void Update();

	// Token: 0x06005775 RID: 22389
	void StopUpdate();

	// Token: 0x06005776 RID: 22390
	void Cleanup();

	// Token: 0x06005777 RID: 22391
	void RequestChunk(int _x, int _y);

	// Token: 0x06005778 RID: 22392
	HashSetList<long> GetRequestedChunks();

	// Token: 0x06005779 RID: 22393
	void SaveAll();

	// Token: 0x0600577A RID: 22394
	void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet);

	// Token: 0x0600577B RID: 22395
	void ReloadAllChunks();

	// Token: 0x0600577C RID: 22396
	void ClearCaches();

	// Token: 0x0600577D RID: 22397
	EnumChunkProviderId GetProviderId();

	// Token: 0x0600577E RID: 22398
	void UnloadChunk(Chunk _chunk);

	// Token: 0x0600577F RID: 22399
	DynamicPrefabDecorator GetDynamicPrefabDecorator();

	// Token: 0x06005780 RID: 22400
	EventPrefabs GetEventPrefabs();

	// Token: 0x06005781 RID: 22401
	SpawnPointList GetSpawnPointList();

	// Token: 0x06005782 RID: 22402
	void SetSpawnPointList(SpawnPointList _spawnPointList);

	// Token: 0x06005783 RID: 22403
	bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors);

	// Token: 0x06005784 RID: 22404
	void SetDecorationsEnabled(bool _bEnable);

	// Token: 0x06005785 RID: 22405
	bool IsDecorationsEnabled();

	// Token: 0x06005786 RID: 22406
	bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize);

	// Token: 0x06005787 RID: 22407
	BoundsInt GetWorldBounds();

	// Token: 0x06005788 RID: 22408
	Vector2i GetWorldSize();

	// Token: 0x06005789 RID: 22409
	IBiomeProvider GetBiomeProvider();

	// Token: 0x0600578A RID: 22410
	ITerrainGenerator GetTerrainGenerator();

	// Token: 0x0600578B RID: 22411
	int GetPOIBlockIdOverride(int x, int z);

	// Token: 0x0600578C RID: 22412
	float GetPOIHeightOverride(int x, int z);

	// Token: 0x0600578D RID: 22413
	IEnumerator FillOccupiedMap(int xStart, int zStart, DecoOccupiedMap occupiedMap, List<PrefabInstance> overridePOIList = null);

	// Token: 0x0600578E RID: 22414
	void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset);

	// Token: 0x0600578F RID: 22415
	ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos);

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x06005790 RID: 22416
	GameUtils.WorldInfo WorldInfo { get; }
}
