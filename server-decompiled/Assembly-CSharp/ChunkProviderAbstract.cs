using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B35 RID: 2869
public abstract class ChunkProviderAbstract : IChunkProvider
{
	// Token: 0x06005709 RID: 22281 RVA: 0x0021573F File Offset: 0x0021393F
	public virtual IEnumerator Init(World _worldData)
	{
		yield return null;
		yield break;
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void StopUpdate()
	{
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x0600570D RID: 22285 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RequestChunk(int _x, int _y)
	{
	}

	// Token: 0x0600570E RID: 22286 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual HashSetList<long> GetRequestedChunks()
	{
		return null;
	}

	// Token: 0x0600570F RID: 22287 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SaveAll()
	{
	}

	// Token: 0x06005710 RID: 22288 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SaveRandomChunks(int count, ulong _curWorldTimeInTicks, ArraySegment<long> _activeChunkSet)
	{
	}

	// Token: 0x06005711 RID: 22289 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReloadAllChunks()
	{
	}

	// Token: 0x06005712 RID: 22290 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClearCaches()
	{
	}

	// Token: 0x06005713 RID: 22291 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.None;
	}

	// Token: 0x06005714 RID: 22292 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UnloadChunk(Chunk _chunk)
	{
	}

	// Token: 0x06005715 RID: 22293 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual DynamicPrefabDecorator GetDynamicPrefabDecorator()
	{
		return null;
	}

	// Token: 0x06005716 RID: 22294 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual EventPrefabs GetEventPrefabs()
	{
		return null;
	}

	// Token: 0x06005717 RID: 22295 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual SpawnPointList GetSpawnPointList()
	{
		return null;
	}

	// Token: 0x06005718 RID: 22296 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
	}

	// Token: 0x06005719 RID: 22297 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] mapColors)
	{
		return false;
	}

	// Token: 0x0600571A RID: 22298 RVA: 0x00215747 File Offset: 0x00213947
	public virtual void SetDecorationsEnabled(bool _bEnable)
	{
		this.bDecorationsEnabled = _bEnable;
	}

	// Token: 0x0600571B RID: 22299 RVA: 0x00215750 File Offset: 0x00213950
	public virtual bool IsDecorationsEnabled()
	{
		return this.bDecorationsEnabled;
	}

	// Token: 0x0600571C RID: 22300 RVA: 0x00215758 File Offset: 0x00213958
	public virtual bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
	{
		_minSize = Vector3i.zero;
		_maxSize = Vector3i.zero;
		return false;
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x00215774 File Offset: 0x00213974
	public virtual BoundsInt GetWorldBounds()
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		return new BoundsInt(vector3i.x, vector3i.y, vector3i.z, vector3i2.x, vector3i2.y, vector3i2.z);
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x002157B8 File Offset: 0x002139B8
	public virtual Vector2i GetWorldSize()
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		this.GetWorldExtent(out vector3i, out vector3i2);
		return new Vector2i(vector3i2.x - vector3i.x + 1, vector3i2.y - vector3i.y + 1);
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual IBiomeProvider GetBiomeProvider()
	{
		return null;
	}

	// Token: 0x06005720 RID: 22304 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual ITerrainGenerator GetTerrainGenerator()
	{
		return null;
	}

	// Token: 0x06005721 RID: 22305 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetPOIBlockIdOverride(int x, int z)
	{
		return 0;
	}

	// Token: 0x06005722 RID: 22306 RVA: 0x0003D066 File Offset: 0x0003B266
	public virtual float GetPOIHeightOverride(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005723 RID: 22307 RVA: 0x002157F3 File Offset: 0x002139F3
	public virtual IEnumerator FillOccupiedMap(int w, int h, DecoOccupiedMap occupiedMap, List<PrefabInstance> overridePOIList = null)
	{
		yield break;
	}

	// Token: 0x06005724 RID: 22308 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _isStopStabilityCalc, bool _isRegenChunk, bool _isFillEmptyBlocks, bool _isReset)
	{
	}

	// Token: 0x06005725 RID: 22309 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual ChunkProtectionLevel GetChunkProtectionLevel(Vector3i worldPos)
	{
		return ChunkProtectionLevel.None;
	}

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x06005726 RID: 22310 RVA: 0x002157FB File Offset: 0x002139FB
	// (set) Token: 0x06005727 RID: 22311 RVA: 0x00215803 File Offset: 0x00213A03
	public GameUtils.WorldInfo WorldInfo
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06005728 RID: 22312 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ChunkProviderAbstract()
	{
	}

	// Token: 0x04004324 RID: 17188
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDecorationsEnabled;
}
