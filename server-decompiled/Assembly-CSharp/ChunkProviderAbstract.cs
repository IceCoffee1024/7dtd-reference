using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B59 RID: 2905
public abstract class ChunkProviderAbstract : IChunkProvider
{
	// Token: 0x060057AB RID: 22443 RVA: 0x0021B6E7 File Offset: 0x002198E7
	public virtual IEnumerator Init(World _worldData)
	{
		yield return null;
		yield break;
	}

	// Token: 0x060057AC RID: 22444 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060057AD RID: 22445 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void StopUpdate()
	{
	}

	// Token: 0x060057AE RID: 22446 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x060057AF RID: 22447 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RequestChunk(int _x, int _y)
	{
	}

	// Token: 0x060057B0 RID: 22448 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual HashSetList<long> GetRequestedChunks()
	{
		return null;
	}

	// Token: 0x060057B1 RID: 22449 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SaveAll()
	{
	}

	// Token: 0x060057B2 RID: 22450 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet)
	{
	}

	// Token: 0x060057B3 RID: 22451 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReloadAllChunks()
	{
	}

	// Token: 0x060057B4 RID: 22452 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClearCaches()
	{
	}

	// Token: 0x060057B5 RID: 22453 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.None;
	}

	// Token: 0x060057B6 RID: 22454 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UnloadChunk(Chunk _chunk)
	{
	}

	// Token: 0x060057B7 RID: 22455 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual DynamicPrefabDecorator GetDynamicPrefabDecorator()
	{
		return null;
	}

	// Token: 0x060057B8 RID: 22456 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual EventPrefabs GetEventPrefabs()
	{
		return null;
	}

	// Token: 0x060057B9 RID: 22457 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual SpawnPointList GetSpawnPointList()
	{
		return null;
	}

	// Token: 0x060057BA RID: 22458 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
	}

	// Token: 0x060057BB RID: 22459 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors)
	{
		return false;
	}

	// Token: 0x060057BC RID: 22460 RVA: 0x0021B6EF File Offset: 0x002198EF
	public virtual void SetDecorationsEnabled(bool _bEnable)
	{
		this.bDecorationsEnabled = _bEnable;
	}

	// Token: 0x060057BD RID: 22461 RVA: 0x0021B6F8 File Offset: 0x002198F8
	public virtual bool IsDecorationsEnabled()
	{
		return this.bDecorationsEnabled;
	}

	// Token: 0x060057BE RID: 22462 RVA: 0x0021B700 File Offset: 0x00219900
	public virtual bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
	{
		_minSize = Vector3i.zero;
		_maxSize = Vector3i.zero;
		return false;
	}

	// Token: 0x060057BF RID: 22463 RVA: 0x0021B71C File Offset: 0x0021991C
	public virtual BoundsInt GetWorldBounds()
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		return new BoundsInt(vector3i.x, vector3i.y, vector3i.z, vector3i2.x, vector3i2.y, vector3i2.z);
	}

	// Token: 0x060057C0 RID: 22464 RVA: 0x0021B760 File Offset: 0x00219960
	public virtual Vector2i GetWorldSize()
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		return new Vector2i(vector3i2.x - vector3i.x + 1, vector3i2.y - vector3i.y + 1);
	}

	// Token: 0x060057C1 RID: 22465 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual IBiomeProvider GetBiomeProvider()
	{
		return null;
	}

	// Token: 0x060057C2 RID: 22466 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual ITerrainGenerator GetTerrainGenerator()
	{
		return null;
	}

	// Token: 0x060057C3 RID: 22467 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetPOIBlockIdOverride(int x, int z)
	{
		return 0;
	}

	// Token: 0x060057C4 RID: 22468 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public virtual float GetPOIHeightOverride(int x, int z)
	{
		return 0f;
	}

	// Token: 0x060057C5 RID: 22469 RVA: 0x0021B79B File Offset: 0x0021999B
	public virtual IEnumerator FillOccupiedMap(int w, int h, DecoOccupiedMap occupiedMap, List<PrefabInstance> overridePOIList = null)
	{
		yield break;
	}

	// Token: 0x060057C6 RID: 22470 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset)
	{
	}

	// Token: 0x060057C7 RID: 22471 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos)
	{
		return ChunkProtectionLevel.None;
	}

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x060057C8 RID: 22472 RVA: 0x0021B7A3 File Offset: 0x002199A3
	// (set) Token: 0x060057C9 RID: 22473 RVA: 0x0021B7AB File Offset: 0x002199AB
	public GameUtils.WorldInfo WorldInfo
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x060057CA RID: 22474 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ChunkProviderAbstract()
	{
	}

	// Token: 0x040043FD RID: 17405
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDecorationsEnabled;
}
