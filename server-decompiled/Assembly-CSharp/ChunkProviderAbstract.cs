using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B64 RID: 2916
public abstract class ChunkProviderAbstract : IChunkProvider
{
	// Token: 0x060057D0 RID: 22480 RVA: 0x0021CC43 File Offset: 0x0021AE43
	public virtual IEnumerator Init(World _worldData)
	{
		yield return null;
		yield break;
	}

	// Token: 0x060057D1 RID: 22481 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060057D2 RID: 22482 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void StopUpdate()
	{
	}

	// Token: 0x060057D3 RID: 22483 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x060057D4 RID: 22484 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RequestChunk(int _x, int _y)
	{
	}

	// Token: 0x060057D5 RID: 22485 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual HashSetList<long> GetRequestedChunks()
	{
		return null;
	}

	// Token: 0x060057D6 RID: 22486 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SaveAll()
	{
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet)
	{
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReloadAllChunks()
	{
	}

	// Token: 0x060057D9 RID: 22489 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClearCaches()
	{
	}

	// Token: 0x060057DA RID: 22490 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.None;
	}

	// Token: 0x060057DB RID: 22491 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UnloadChunk(Chunk _chunk)
	{
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual DynamicPrefabDecorator GetDynamicPrefabDecorator()
	{
		return null;
	}

	// Token: 0x060057DD RID: 22493 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual EventPrefabs GetEventPrefabs()
	{
		return null;
	}

	// Token: 0x060057DE RID: 22494 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual SpawnPointList GetSpawnPointList()
	{
		return null;
	}

	// Token: 0x060057DF RID: 22495 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
	}

	// Token: 0x060057E0 RID: 22496 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors)
	{
		return false;
	}

	// Token: 0x060057E1 RID: 22497 RVA: 0x0021CC4B File Offset: 0x0021AE4B
	public virtual void SetDecorationsEnabled(bool _bEnable)
	{
		this.bDecorationsEnabled = _bEnable;
	}

	// Token: 0x060057E2 RID: 22498 RVA: 0x0021CC54 File Offset: 0x0021AE54
	public virtual bool IsDecorationsEnabled()
	{
		return this.bDecorationsEnabled;
	}

	// Token: 0x060057E3 RID: 22499 RVA: 0x0021CC5C File Offset: 0x0021AE5C
	public virtual bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
	{
		_minSize = Vector3i.zero;
		_maxSize = Vector3i.zero;
		return false;
	}

	// Token: 0x060057E4 RID: 22500 RVA: 0x0021CC78 File Offset: 0x0021AE78
	public virtual BoundsInt GetWorldBounds()
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		return new BoundsInt(vector3i.x, vector3i.y, vector3i.z, vector3i2.x, vector3i2.y, vector3i2.z);
	}

	// Token: 0x060057E5 RID: 22501 RVA: 0x0021CCBC File Offset: 0x0021AEBC
	public virtual Vector2i GetWorldSize()
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		return new Vector2i(vector3i2.x - vector3i.x + 1, vector3i2.y - vector3i.y + 1);
	}

	// Token: 0x060057E6 RID: 22502 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual IBiomeProvider GetBiomeProvider()
	{
		return null;
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual ITerrainGenerator GetTerrainGenerator()
	{
		return null;
	}

	// Token: 0x060057E8 RID: 22504 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetPOIBlockIdOverride(int x, int z)
	{
		return 0;
	}

	// Token: 0x060057E9 RID: 22505 RVA: 0x0003D372 File Offset: 0x0003B572
	public virtual float GetPOIHeightOverride(int x, int z)
	{
		return 0f;
	}

	// Token: 0x060057EA RID: 22506 RVA: 0x0021CCF7 File Offset: 0x0021AEF7
	public virtual IEnumerator FillOccupiedMap(int w, int h, DecoOccupiedMap occupiedMap, List<PrefabInstance> overridePOIList = null)
	{
		yield break;
	}

	// Token: 0x060057EB RID: 22507 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset)
	{
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos)
	{
		return ChunkProtectionLevel.None;
	}

	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x060057ED RID: 22509 RVA: 0x0021CCFF File Offset: 0x0021AEFF
	// (set) Token: 0x060057EE RID: 22510 RVA: 0x0021CD07 File Offset: 0x0021AF07
	public GameUtils.WorldInfo WorldInfo
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x060057EF RID: 22511 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ChunkProviderAbstract()
	{
	}

	// Token: 0x04004422 RID: 17442
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDecorationsEnabled;
}
