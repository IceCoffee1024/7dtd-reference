using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BC1 RID: 3009
public class UnityDistantTerrain
{
	// Token: 0x06005AAC RID: 23212 RVA: 0x0022E3F4 File Offset: 0x0022C5F4
	public void Init(GameObject _parent, UnityDistantTerrain.Config _terrainConfig, int _visibleChunks, Material _terrainMaterial, Material _waterMaterial, int _waterChunks16x16Width, byte[] _waterChunks16x16, ITileArea<float[,]> _heights, TileArea<Color32[]> _splat0, TileArea<Color32[]> _splat1, TileArea<Color32[]> _splat2)
	{
		Transform transform = _parent.transform;
		this.terrainConfig = _terrainConfig;
		this.visibleChunks = _visibleChunks;
		this.terrainMaterial = _terrainMaterial;
		this.waterMaterial = _waterMaterial;
		this.terrainHeights = _heights;
		this.splat0Arr = _splat0;
		this.splat1Arr = _splat1;
		this.splat2Arr = _splat2;
		this.waterChunks16x16Width = _waterChunks16x16Width;
		this.waterChunks16x16 = _waterChunks16x16;
		this.cacheParentObj = new GameObject("Cache");
		this.cacheParentT = this.cacheParentObj.transform;
		this.cacheParentT.SetParent(transform, false);
		this.cacheParentObj.SetActive(false);
		this.visibleParentObj = new GameObject("Terrain");
		this.visibleParentT = this.visibleParentObj.transform;
		this.visibleParentT.SetParent(transform, false);
		this.visibleParentT.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
		this.terrainTiles = new TileArea<UnityDistantTerrain.TerrainAndWater>(this.terrainHeights.Config, null);
		for (int i = 0; i < this.visibleChunks * this.visibleChunks; i++)
		{
			this.AddTerrainAndWaterToCache();
		}
		if (this.splat0Arr != null)
		{
			for (int j = 0; j < this.splatMapCache.Length; j++)
			{
				this.splatMapCache[j] = new TileArea<Texture2D>(this.splat0Arr.config, null);
			}
		}
		this.waterPlane = new UnityDistantTerrainWaterPlane(_terrainConfig, _waterMaterial);
		Origin.Add(transform, -1);
		transform.position = -Origin.position;
	}

	// Token: 0x06005AAD RID: 23213 RVA: 0x0022E56C File Offset: 0x0022C76C
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddTerrainAndWaterToCache()
	{
		GameObject gameObject = new GameObject("Terrain");
		gameObject.transform.SetParent(this.cacheParentT, false);
		Terrain terrain = gameObject.AddComponent<Terrain>();
		GameObject gameObject2 = new GameObject("Water");
		gameObject2.transform.SetParent(terrain.transform, false);
		gameObject2.transform.localPosition = new Vector3(0f, 0.25f, 0f);
		gameObject2.AddComponent<MeshFilter>();
		gameObject2.AddComponent<MeshRenderer>();
		this.terrainCache.Add(new UnityDistantTerrain.TerrainAndWater
		{
			terrain = terrain,
			waterPlane = gameObject2
		});
		TerrainData terrainData = new TerrainData();
		terrainData.heightmapResolution = this.terrainConfig.DataTileSize / this.terrainConfig.DataSteps + 1;
		terrainData.size = new Vector3((float)this.terrainConfig.DataTileSize, (float)this.terrainConfig.MaxHeight, (float)this.terrainConfig.DataTileSize);
		if (this.terrainMaterial != null)
		{
			terrain.materialTemplate = this.terrainMaterial;
		}
		terrain.terrainData = terrainData;
		terrain.drawInstanced = false;
		terrain.drawTreesAndFoliage = false;
		terrain.heightmapMaximumLOD = 0;
		terrain.heightmapPixelError = (float)this.terrainConfig.PixelError;
		int num = Mathf.Clamp(GamePrefs.GetInt(EnumGamePrefs.OptionsGfxTerrainQuality), 0, UnityDistantTerrain.basemapDistances.Length - 1);
		float num2 = UnityDistantTerrain.basemapDistances[num];
		terrain.basemapDistance = num2;
		gameObject.SetActive(false);
	}

	// Token: 0x06005AAE RID: 23214 RVA: 0x0022E6D4 File Offset: 0x0022C8D4
	public void Cleanup()
	{
		if (this.visibleParentT)
		{
			Origin.Remove(this.visibleParentT.parent);
		}
		for (int i = 0; i < this.splatMapCache.Length; i++)
		{
			TileArea<Texture2D> tileArea = this.splatMapCache[i];
			if (tileArea != null)
			{
				foreach (KeyValuePair<uint, Texture2D> keyValuePair in tileArea.Data)
				{
					global::UnityEngine.Object.Destroy(keyValuePair.Value);
				}
				this.splatMapCache[i] = null;
			}
		}
		global::UnityEngine.Object.Destroy(this.cacheParentObj);
		global::UnityEngine.Object.Destroy(this.visibleParentObj);
		this.terrainCache.Clear();
		global::UnityEngine.Object.Destroy(this.terrainClipTexture);
		this.terrainClipTexture = null;
		this.visibleVoxelChunks.Clear();
		this.waterPlane.Cleanup();
		this.isAnyTerrainDirty = false;
		this.terrainHeights.Cleanup();
	}

	// Token: 0x06005AAF RID: 23215 RVA: 0x0022E7CC File Offset: 0x0022C9CC
	public void FrameUpdate(EntityPlayerLocal _player)
	{
		this.BuildAroundPos(_player.position);
		this.UpdateChunks();
		this.UpdateTextureApply();
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x0022E7E7 File Offset: 0x0022C9E7
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildAroundPos(Vector3 _position)
	{
		this.tempPositions.Clear();
		this.tempPositions.Add(_position);
		this.BuildAroundPos(this.tempPositions);
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x0022E80C File Offset: 0x0022CA0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildAroundPos(List<Vector3> _positions)
	{
		int chunkWorldSize = this.terrainConfig.ChunkWorldSize;
		float num = 0f;
		int num2 = (this.visibleChunks - 1) / 2;
		int num3 = num2;
		if ((this.visibleChunks & 1) == 0)
		{
			num = (float)chunkWorldSize * -0.5f;
			num3++;
		}
		this.visibleTerrainTilesOfObservers.Clear();
		for (int i = 0; i < _positions.Count; i++)
		{
			int num4 = Utils.Fastfloor((_positions[i].x + num) / (float)chunkWorldSize);
			int num5 = Utils.Fastfloor((_positions[i].z + num) / (float)chunkWorldSize);
			int num6 = num4 - num2;
			int num7 = num4 + num3;
			int num8 = num5 - num2;
			int num9 = num5 + num3;
			for (int j = num6; j <= num7; j++)
			{
				for (int k = num8; k <= num9; k++)
				{
					this.visibleTerrainTilesOfObservers.Add(TileAreaUtils.MakeKey(j, k));
				}
			}
		}
		this.tempKeysToRemove.Clear();
		foreach (KeyValuePair<uint, UnityDistantTerrain.TerrainAndWater> keyValuePair in this.terrainTiles.Data)
		{
			if (!this.visibleTerrainTilesOfObservers.Contains(keyValuePair.Key))
			{
				this.tempKeysToRemove.Add(keyValuePair.Key);
			}
		}
		for (int l = 0; l < this.tempKeysToRemove.Count; l++)
		{
			uint num10 = this.tempKeysToRemove[l];
			UnityDistantTerrain.TerrainAndWater terrainAndWater = this.terrainTiles[num10];
			this.terrainTiles.Remove(num10);
			this.terrainCache.Add(terrainAndWater);
			terrainAndWater.waterPlane.gameObject.SetActive(false);
			terrainAndWater.terrain.gameObject.SetActive(false);
			terrainAndWater.terrain.transform.SetParent(this.cacheParentT, false);
		}
		bool flag = false;
		int num11 = -(this.terrainConfig.DataWidth / this.terrainConfig.DataTileSize) / 2;
		int num12 = this.terrainConfig.DataWidth / this.terrainConfig.DataTileSize / 2 - 1;
		num12 = Utils.FastMax(0, num12);
		int num13 = -(this.terrainConfig.DataHeight / this.terrainConfig.DataTileSize) / 2;
		int num14 = this.terrainConfig.DataHeight / this.terrainConfig.DataTileSize / 2 - 1;
		num14 = Utils.FastMax(0, num14);
		float num15 = (float)chunkWorldSize * 0.5f;
		float num16 = UnityDistantTerrain.pixelErrorDistanceDiv[GamePrefs.GetInt(EnumGamePrefs.OptionsGfxTerrainQuality)];
		foreach (uint num17 in this.visibleTerrainTilesOfObservers)
		{
			int tileXPos = TileAreaUtils.GetTileXPos(num17);
			int tileZPos = TileAreaUtils.GetTileZPos(num17);
			UnityDistantTerrain.TerrainAndWater terrainAndWater2 = this.terrainTiles[num17];
			if (terrainAndWater2 != null)
			{
				float num18 = float.MaxValue;
				for (int m = 0; m < _positions.Count; m++)
				{
					float num19 = _positions[m].x - ((float)(tileXPos * chunkWorldSize) + num15);
					float num20 = _positions[m].z - ((float)(tileZPos * chunkWorldSize) + num15);
					float num21 = num19 * num19 + num20 * num20;
					if (num21 < num18)
					{
						num18 = num21;
					}
				}
				float num22 = 5f;
				if (num18 > 102400f)
				{
					float num23 = Mathf.Sqrt(num18);
					num22 += (num23 - 320f) / num16;
				}
				terrainAndWater2.terrain.heightmapPixelError = (float)((int)num22);
			}
			else if (tileXPos >= num11 && tileXPos <= num12 && tileZPos >= num13 && tileZPos <= num14)
			{
				terrainAndWater2 = this.CreateAndConfigureTerrain(tileXPos, tileZPos);
				terrainAndWater2.terrain.transform.SetParent(this.visibleParentT, false);
				this.terrainTiles[num17] = terrainAndWater2;
				flag = true;
				this.isNeighborsDirty = true;
				break;
			}
		}
		if (!flag && this.isNeighborsDirty)
		{
			this.isNeighborsDirty = false;
			foreach (uint num24 in this.visibleTerrainTilesOfObservers)
			{
				UnityDistantTerrain.TerrainAndWater terrainAndWater3 = this.terrainTiles[num24];
				if (terrainAndWater3 != null)
				{
					int tileXPos2 = TileAreaUtils.GetTileXPos(num24);
					int tileZPos2 = TileAreaUtils.GetTileZPos(num24);
					UnityDistantTerrain.TerrainAndWater terrainAndWater4 = this.terrainTiles[TileAreaUtils.MakeKey(tileXPos2 - 1, tileZPos2)];
					UnityDistantTerrain.TerrainAndWater terrainAndWater5 = this.terrainTiles[TileAreaUtils.MakeKey(tileXPos2, tileZPos2 + 1)];
					UnityDistantTerrain.TerrainAndWater terrainAndWater6 = this.terrainTiles[TileAreaUtils.MakeKey(tileXPos2 + 1, tileZPos2)];
					UnityDistantTerrain.TerrainAndWater terrainAndWater7 = this.terrainTiles[TileAreaUtils.MakeKey(tileXPos2, tileZPos2 - 1)];
					terrainAndWater3.terrain.SetNeighbors((terrainAndWater4 != null) ? terrainAndWater4.terrain : null, (terrainAndWater5 != null) ? terrainAndWater5.terrain : null, (terrainAndWater6 != null) ? terrainAndWater6.terrain : null, (terrainAndWater7 != null) ? terrainAndWater7.terrain : null);
				}
			}
		}
		DynamicMeshManager.UpdateDistantTerrainBounds(this.terrainTiles, this.terrainConfig);
	}

	// Token: 0x06005AB2 RID: 23218 RVA: 0x0022ED58 File Offset: 0x0022CF58
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrain.TerrainAndWater GetFromCacheOrCreate()
	{
		if (this.terrainCache.Count == 0)
		{
			this.AddTerrainAndWaterToCache();
		}
		UnityDistantTerrain.TerrainAndWater terrainAndWater = this.terrainCache[this.terrainCache.Count - 1];
		this.terrainCache.RemoveAt(this.terrainCache.Count - 1);
		return terrainAndWater;
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x0022EDA8 File Offset: 0x0022CFA8
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrain.TerrainAndWater CreateAndConfigureTerrain(int cx, int cz)
	{
		UnityDistantTerrain.TerrainAndWater fromCacheOrCreate = this.GetFromCacheOrCreate();
		Terrain terrain = fromCacheOrCreate.terrain;
		GameObject gameObject = terrain.gameObject;
		gameObject.name = "Terrain " + cx.ToString() + "/" + cz.ToString();
		terrain.transform.localPosition = new Vector3((float)(cx * this.terrainConfig.ChunkWorldSize), 0f, (float)(cz * this.terrainConfig.ChunkWorldSize));
		float[,] array = this.terrainHeights[cx, cz];
		terrain.terrainData.SetHeights(0, 0, array);
		gameObject.SetActive(true);
		if (this.splat0Arr != null)
		{
			Material materialTemplate = terrain.materialTemplate;
			int num = this.terrainConfig.DataTileSize / this.terrainConfig.SplatSteps + 2;
			int num2 = num;
			Texture2D texture2D = this.splatMapCache[0][cx, cz];
			if (texture2D == null)
			{
				texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, true);
				texture2D.SetPixels32(this.splat0Arr[cx, cz]);
				texture2D.filterMode = FilterMode.Bilinear;
				texture2D.wrapMode = TextureWrapMode.Clamp;
				texture2D.name = "Splat0 " + cx.ToString() + "/" + cz.ToString();
				texture2D.Apply(true, true);
				this.splatMapCache[0][cx, cz] = texture2D;
			}
			Texture2D texture2D2 = this.splatMapCache[1][cx, cz];
			if (texture2D2 == null)
			{
				texture2D2 = new Texture2D(num, num2, TextureFormat.RGBA32, true);
				texture2D2.SetPixels32(this.splat1Arr[cx, cz]);
				texture2D2.filterMode = FilterMode.Bilinear;
				texture2D2.wrapMode = TextureWrapMode.Clamp;
				texture2D2.name = "Splat1 " + cx.ToString() + "/" + cz.ToString();
				texture2D2.Apply(true, true);
				this.splatMapCache[1][cx, cz] = texture2D2;
			}
			Texture2D texture2D3 = this.splatMapCache[2][cx, cz];
			if (texture2D3 == null)
			{
				texture2D3 = new Texture2D(num, num2, TextureFormat.RGBA32, true);
				texture2D3.SetPixels32(this.splat2Arr[cx, cz]);
				texture2D3.filterMode = FilterMode.Bilinear;
				texture2D3.wrapMode = TextureWrapMode.Clamp;
				texture2D3.name = "Splat2 " + cx.ToString() + "/" + cz.ToString();
				texture2D3.Apply(true, true);
				this.splatMapCache[2][cx, cz] = texture2D3;
			}
			materialTemplate.SetTexture("_CustomControl0", texture2D);
			materialTemplate.SetTexture("_CustomControl1", texture2D2);
			materialTemplate.SetTexture("_CustomControl2", texture2D3);
		}
		if (this.waterChunks16x16Width != 0)
		{
			this.waterPlane.createDynamicWaterPlane_Step1((cx - this.terrainHeights.Config.tileStart.x) * this.terrainConfig.ChunkWorldSize, (cz - this.terrainHeights.Config.tileStart.y) * this.terrainConfig.ChunkWorldSize, this.terrainConfig.ChunkWorldSize, this.waterChunks16x16Width, this.waterChunks16x16);
			this.waterPlane.createDynamicWaterPlane_Step2(fromCacheOrCreate.waterPlane, terrain.transform.parent, null);
		}
		return fromCacheOrCreate;
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x0022F0C0 File Offset: 0x0022D2C0
	public void OnChunkUpdate(int _chunkX, int _chunkZ, bool _visible)
	{
		int num = this.terrainConfig.DataWidth / 16;
		if (_chunkX < -num / 2 || _chunkX >= num / 2)
		{
			return;
		}
		int num2 = this.terrainConfig.DataHeight / 16;
		if (_chunkZ < -num2 / 2 || _chunkZ >= num2 / 2)
		{
			return;
		}
		uint num3 = TileAreaUtils.MakeKey(_chunkX, _chunkZ);
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo;
		if (!this.dictChunkHeightsArr.TryGetValue(num3, out voxeChunkInfo))
		{
			voxeChunkInfo = new UnityDistantTerrain.VoxeChunkInfo();
			this.dictChunkHeightsArr[num3] = voxeChunkInfo;
		}
		if (_visible)
		{
			this.visibleVoxelChunks.Add(num3);
			voxeChunkInfo.a3 = 1;
		}
		else
		{
			this.visibleVoxelChunks.Remove(num3);
			voxeChunkInfo.a3 = 0;
		}
		voxeChunkInfo.isDirty = true;
		this.isAnyTerrainDirty = true;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < this.posAround.Length; j++)
			{
				this.UpdateVoxelChunkInfo(_chunkX + this.posAround[j].x, _chunkZ + this.posAround[j].z, this.visibleVoxelChunks);
			}
		}
		this.UpdateTerrainTextureData(_chunkX, _chunkZ, _visible);
	}

	// Token: 0x06005AB5 RID: 23221 RVA: 0x0022F1D4 File Offset: 0x0022D3D4
	public void UpdateChunks()
	{
		if (!this.isAnyTerrainDirty)
		{
			return;
		}
		this.isAnyTerrainDirty = false;
		this.toRemove.Clear();
		this.toUpdateTerrain.Clear();
		foreach (KeyValuePair<uint, UnityDistantTerrain.VoxeChunkInfo> keyValuePair in this.dictChunkHeightsArr)
		{
			UnityDistantTerrain.VoxeChunkInfo value = keyValuePair.Value;
			if (value.isDirty)
			{
				int tileXPos = TileAreaUtils.GetTileXPos(keyValuePair.Key);
				int tileZPos = TileAreaUtils.GetTileZPos(keyValuePair.Key);
				if (this.UpdateChunkHeights(tileXPos, tileZPos, value))
				{
					value.isDirty = false;
				}
			}
			if (!value.isDirty && value.IsEmpty())
			{
				this.toRemove.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < this.toRemove.Count; i++)
		{
			this.dictChunkHeightsArr.Remove(this.toRemove[i]);
		}
		foreach (Terrain terrain in this.toUpdateTerrain)
		{
			terrain.terrainData.SyncHeightmap();
		}
		this.toUpdateTerrain.Clear();
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x0022F32C File Offset: 0x0022D52C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool UpdateTerrainTextureData(int chunkX, int chunkZ, bool _bVisible)
	{
		int num = this.terrainConfig.DataWidth / 16;
		int num2 = this.terrainConfig.DataHeight / 16;
		if (this.terrainClipTexture == null)
		{
			this.terrainClipTexture = new Texture2D(num, num2, TextureFormat.RGB24, false);
			this.terrainClipTexture.wrapMode = TextureWrapMode.Clamp;
			this.terrainClipTexture.filterMode = FilterMode.Point;
			this.terrainClipCols = new Color32[num * num2];
			for (int i = 0; i < this.terrainClipCols.Length; i++)
			{
				this.terrainClipCols[i] = this.colNoClip;
			}
			this.waterMaterial.SetTexture("_ClipChunks", this.terrainClipTexture);
		}
		int num3 = num / 2;
		int num4 = num2 / 2;
		foreach (KeyValuePair<uint, UnityDistantTerrain.VoxeChunkInfo> keyValuePair in this.dictChunkHeightsArr)
		{
			UnityDistantTerrain.VoxeChunkInfo value = keyValuePair.Value;
			Color32 color;
			if (value.a0 == 2 && value.a1 == 2 && value.a2 == 2 && value.a3 == 2)
			{
				color = this.colClipTerrainAndWater;
			}
			else if (value.a3 != 0)
			{
				color = this.colClipWater;
			}
			else
			{
				color = this.colNoClip;
			}
			int tileXPos = TileAreaUtils.GetTileXPos(keyValuePair.Key);
			int tileZPos = TileAreaUtils.GetTileZPos(keyValuePair.Key);
			this.terrainClipCols[tileXPos + num3 + (tileZPos + num4) * num2] = color;
		}
		this.isClipTextureDirty = true;
		return true;
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x0022F4BC File Offset: 0x0022D6BC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool UpdateTextureApply()
	{
		if (!this.isClipTextureDirty)
		{
			return false;
		}
		this.isClipTextureDirty = false;
		this.terrainClipTexture.SetPixels32(this.terrainClipCols);
		this.terrainClipTexture.Apply();
		return true;
	}

	// Token: 0x06005AB8 RID: 23224 RVA: 0x0022F4EC File Offset: 0x0022D6EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateVoxelChunkInfo(int _chunkX, int _chunkZ, HashSet<uint> _visibleVoxelChunks)
	{
		int num = this.terrainConfig.DataWidth / 16;
		int num2 = this.terrainConfig.DataHeight / 16;
		if (_chunkX < -num / 2 || _chunkZ < -num2 / 2 || _chunkX >= num / 2 || _chunkZ >= num2 / 2)
		{
			return;
		}
		uint num3 = TileAreaUtils.MakeKey(_chunkX, _chunkZ);
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo;
		if (!this.dictChunkHeightsArr.TryGetValue(num3, out voxeChunkInfo))
		{
			voxeChunkInfo = new UnityDistantTerrain.VoxeChunkInfo();
			this.dictChunkHeightsArr[num3] = voxeChunkInfo;
		}
		int num4 = _chunkX - 1;
		int num5 = _chunkX + 1;
		int num6 = _chunkZ - 1;
		int num7 = _chunkZ + 1;
		bool flag = voxeChunkInfo.a3 > 0 && _visibleVoxelChunks.Contains(TileAreaUtils.MakeKey(num4, _chunkZ));
		if (flag)
		{
			if (voxeChunkInfo.a2 == 0)
			{
				voxeChunkInfo.a2 = 1;
				voxeChunkInfo.isDirty = true;
				this.isAnyTerrainDirty = true;
			}
		}
		else if (voxeChunkInfo.a2 != 0)
		{
			voxeChunkInfo.a2 = 0;
			voxeChunkInfo.isDirty = true;
			this.isAnyTerrainDirty = true;
		}
		bool flag2 = voxeChunkInfo.a3 > 0 && _visibleVoxelChunks.Contains(TileAreaUtils.MakeKey(_chunkX, num6));
		if (flag2)
		{
			if (voxeChunkInfo.a1 == 0)
			{
				voxeChunkInfo.a1 = 1;
				voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
			}
		}
		else if (voxeChunkInfo.a1 != 0)
		{
			voxeChunkInfo.a1 = 0;
			voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
		}
		if (voxeChunkInfo.a3 > 0 && flag2 && flag && _visibleVoxelChunks.Contains(TileAreaUtils.MakeKey(num4, num6)))
		{
			if (voxeChunkInfo.a0 == 0)
			{
				voxeChunkInfo.a0 = 1;
				voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
			}
		}
		else if (voxeChunkInfo.a0 != 0)
		{
			voxeChunkInfo.a0 = 0;
			voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
		}
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo2;
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo3;
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo4;
		if (voxeChunkInfo.a0 >= 1 && voxeChunkInfo.a1 > 0 && voxeChunkInfo.a2 > 0 && voxeChunkInfo.a3 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num4, _chunkZ), out voxeChunkInfo2) && voxeChunkInfo2.a1 > 0 && voxeChunkInfo2.a3 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num4, num6), out voxeChunkInfo3) && voxeChunkInfo3.a3 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(_chunkX, num6), out voxeChunkInfo4) && voxeChunkInfo4.a2 > 0 && voxeChunkInfo4.a3 > 0)
		{
			if (voxeChunkInfo.a0 != 2)
			{
				voxeChunkInfo.a0 = 2;
				voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
			}
		}
		else if (voxeChunkInfo.a0 == 2)
		{
			voxeChunkInfo.a0 = 1;
			voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
		}
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo5;
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo6;
		flag2 = voxeChunkInfo.a1 >= 1 && voxeChunkInfo.a0 > 0 && voxeChunkInfo.a2 > 0 && voxeChunkInfo.a3 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num5, _chunkZ), out voxeChunkInfo5) && voxeChunkInfo5.a0 > 0 && voxeChunkInfo5.a2 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(_chunkX, num6), out voxeChunkInfo4) && voxeChunkInfo4.a2 > 0 && voxeChunkInfo4.a3 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num5, num6), out voxeChunkInfo6) && voxeChunkInfo6.a2 > 0;
		if (flag2)
		{
			if (voxeChunkInfo.a1 != 2)
			{
				voxeChunkInfo.a1 = 2;
				voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
			}
		}
		else if (voxeChunkInfo.a1 == 2)
		{
			voxeChunkInfo.a1 = 1;
			voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
		}
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo7;
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo8;
		flag = voxeChunkInfo.a2 >= 1 && voxeChunkInfo.a0 > 0 && voxeChunkInfo.a1 > 0 && voxeChunkInfo.a3 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num4, _chunkZ), out voxeChunkInfo2) && voxeChunkInfo2.a3 > 0 && voxeChunkInfo2.a1 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num4, num7), out voxeChunkInfo7) && voxeChunkInfo7.a1 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(_chunkX, num7), out voxeChunkInfo8) && voxeChunkInfo8.a0 > 0 && voxeChunkInfo8.a1 > 0;
		if (flag)
		{
			if (voxeChunkInfo.a2 != 2)
			{
				voxeChunkInfo.a2 = 2;
				voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
			}
		}
		else if (voxeChunkInfo.a2 == 2)
		{
			voxeChunkInfo.a2 = 1;
			voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
		}
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo9;
		if (voxeChunkInfo.a3 >= 1 && voxeChunkInfo.a0 > 0 && voxeChunkInfo.a1 > 0 && voxeChunkInfo.a2 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num5, _chunkZ), out voxeChunkInfo5) && voxeChunkInfo5.a2 > 0 && voxeChunkInfo5.a0 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(num5, num7), out voxeChunkInfo9) && voxeChunkInfo9.a0 > 0 && this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(_chunkX, num7), out voxeChunkInfo8) && voxeChunkInfo8.a0 > 0 && voxeChunkInfo8.a1 > 0)
		{
			if (voxeChunkInfo.a3 != 2)
			{
				voxeChunkInfo.a3 = 2;
				voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
				return;
			}
		}
		else if (voxeChunkInfo.a3 == 2)
		{
			voxeChunkInfo.a3 = 1;
			voxeChunkInfo.isDirty = (this.isAnyTerrainDirty = true);
		}
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x0022FA74 File Offset: 0x0022DC74
	[PublicizedFrom(EAccessModifier.Private)]
	public bool UpdateChunkHeights(int _chunkX, int _chunkZ, UnityDistantTerrain.VoxeChunkInfo _vciMiddle)
	{
		int num = _chunkX * 16;
		int num2 = _chunkZ * 16;
		int chunkWorldSize = this.terrainConfig.ChunkWorldSize;
		int num3 = Utils.Fastfloor((float)num / (float)chunkWorldSize);
		int num4 = Utils.Fastfloor((float)num2 / (float)chunkWorldSize);
		uint num5 = TileAreaUtils.MakeKey(num3, num4);
		UnityDistantTerrain.TerrainAndWater terrainAndWater = this.terrainTiles[num5];
		if (terrainAndWater == null)
		{
			return false;
		}
		float[,] array = this.terrainHeights[num3, num4];
		if (array == null)
		{
			return false;
		}
		int num6 = num - num3 * chunkWorldSize;
		int num7 = num2 - num4 * chunkWorldSize;
		int num8 = num6 / this.terrainConfig.MetersPerHeightPix;
		num7 /= this.terrainConfig.MetersPerHeightPix;
		int num9 = 16 / this.terrainConfig.MetersPerHeightPix;
		int num10 = num8 / num9 * num9;
		int num11 = num7 / num9 * num9;
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo = null;
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo2 = null;
		UnityDistantTerrain.VoxeChunkInfo voxeChunkInfo3 = null;
		float num12 = array[num11, num10];
		if (_vciMiddle.a0 == 1)
		{
			num12 -= 0.0025f;
		}
		else if (_vciMiddle.a0 == 2)
		{
			num12 = 0f;
		}
		this.heights8x8[0, 0] = num12;
		for (int i = 1; i < 8; i++)
		{
			num12 = array[num11, num10 + i];
			if (_vciMiddle.a1 == 1)
			{
				num12 -= 0.0025f;
			}
			else if (_vciMiddle.a1 == 2)
			{
				num12 = 0f;
			}
			this.heights8x8[0, i] = num12;
		}
		num12 = array[num11, num10 + 8];
		if (voxeChunkInfo != null || this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(_chunkX + 1, _chunkZ), out voxeChunkInfo))
		{
			if (voxeChunkInfo.a0 == 1)
			{
				num12 -= 0.0025f;
			}
			else if (voxeChunkInfo.a0 == 2)
			{
				num12 = 0f;
			}
		}
		this.heights8x8[0, 8] = num12;
		for (int j = 1; j < 8; j++)
		{
			num12 = array[num11 + j, num10];
			if (_vciMiddle.a2 == 1)
			{
				num12 -= 0.0025f;
			}
			else if (_vciMiddle.a2 == 2)
			{
				num12 = 0f;
			}
			this.heights8x8[j, 0] = num12;
		}
		for (int k = 1; k < 8; k++)
		{
			for (int l = 1; l < 8; l++)
			{
				num12 = array[num11 + k, num10 + l];
				if (_vciMiddle.a3 == 1)
				{
					num12 -= 0.0025f;
				}
				else if (_vciMiddle.a3 == 2)
				{
					num12 = 0f;
				}
				this.heights8x8[k, l] = num12;
			}
		}
		for (int m = 1; m < 8; m++)
		{
			num12 = array[num11 + m, num10 + 8];
			if (voxeChunkInfo != null)
			{
				if (voxeChunkInfo.a2 == 1)
				{
					num12 -= 0.0025f;
				}
				else if (voxeChunkInfo.a2 == 2)
				{
					num12 = 0f;
				}
			}
			this.heights8x8[m, 8] = num12;
		}
		num12 = array[num11 + 8, num10];
		if (voxeChunkInfo3 != null || this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(_chunkX, _chunkZ + 1), out voxeChunkInfo3))
		{
			if (voxeChunkInfo3.a0 == 1)
			{
				num12 -= 0.0025f;
			}
			else if (voxeChunkInfo3.a0 == 2)
			{
				num12 = 0f;
			}
		}
		this.heights8x8[8, 0] = num12;
		for (int n = 1; n < 8; n++)
		{
			num12 = array[num11 + 8, num10 + n];
			if (voxeChunkInfo3 != null)
			{
				if (voxeChunkInfo3.a1 == 1)
				{
					num12 -= 0.0025f;
				}
				else if (voxeChunkInfo3.a1 == 2)
				{
					num12 = 0f;
				}
			}
			this.heights8x8[8, n] = num12;
		}
		num12 = array[num11 + 8, num10 + 8];
		if (voxeChunkInfo2 != null || this.dictChunkHeightsArr.TryGetValue(TileAreaUtils.MakeKey(_chunkX + 1, _chunkZ + 1), out voxeChunkInfo2))
		{
			if (voxeChunkInfo2.a0 == 1)
			{
				num12 -= 0.0025f;
			}
			else if (voxeChunkInfo2.a0 == 2)
			{
				num12 = 0f;
			}
		}
		this.heights8x8[8, 8] = num12;
		terrainAndWater.terrain.terrainData.SetHeightsDelayLOD(num10, num11, this.heights8x8);
		this.toUpdateTerrain.Add(terrainAndWater.terrain);
		return true;
	}

	// Token: 0x04004634 RID: 17972
	public const int cSplatBorderSize = 1;

	// Token: 0x04004635 RID: 17973
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileArea<float[,]> terrainHeights;

	// Token: 0x04004636 RID: 17974
	[PublicizedFrom(EAccessModifier.Private)]
	public TileArea<Color32[]> splat0Arr;

	// Token: 0x04004637 RID: 17975
	[PublicizedFrom(EAccessModifier.Private)]
	public TileArea<Color32[]> splat1Arr;

	// Token: 0x04004638 RID: 17976
	[PublicizedFrom(EAccessModifier.Private)]
	public TileArea<Color32[]> splat2Arr;

	// Token: 0x04004639 RID: 17977
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject cacheParentObj;

	// Token: 0x0400463A RID: 17978
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform cacheParentT;

	// Token: 0x0400463B RID: 17979
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject visibleParentObj;

	// Token: 0x0400463C RID: 17980
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform visibleParentT;

	// Token: 0x0400463D RID: 17981
	[PublicizedFrom(EAccessModifier.Private)]
	public Material terrainMaterial;

	// Token: 0x0400463E RID: 17982
	[PublicizedFrom(EAccessModifier.Private)]
	public Material waterMaterial;

	// Token: 0x0400463F RID: 17983
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrain.Config terrainConfig;

	// Token: 0x04004640 RID: 17984
	[PublicizedFrom(EAccessModifier.Private)]
	public int visibleChunks;

	// Token: 0x04004641 RID: 17985
	[PublicizedFrom(EAccessModifier.Private)]
	public TileArea<UnityDistantTerrain.TerrainAndWater> terrainTiles;

	// Token: 0x04004642 RID: 17986
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UnityDistantTerrain.TerrainAndWater> terrainCache = new List<UnityDistantTerrain.TerrainAndWater>();

	// Token: 0x04004643 RID: 17987
	[PublicizedFrom(EAccessModifier.Private)]
	public TileArea<Texture2D>[] splatMapCache = new TileArea<Texture2D>[3];

	// Token: 0x04004644 RID: 17988
	[PublicizedFrom(EAccessModifier.Private)]
	public List<uint> tempKeysToRemove = new List<uint>();

	// Token: 0x04004645 RID: 17989
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3> tempPositions = new List<Vector3>();

	// Token: 0x04004646 RID: 17990
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAnyTerrainDirty;

	// Token: 0x04004647 RID: 17991
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isNeighborsDirty;

	// Token: 0x04004648 RID: 17992
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClipTextureDirty;

	// Token: 0x04004649 RID: 17993
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<uint> visibleTerrainTilesOfObservers = new HashSet<uint>();

	// Token: 0x0400464A RID: 17994
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D terrainClipTexture;

	// Token: 0x0400464B RID: 17995
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32[] terrainClipCols;

	// Token: 0x0400464C RID: 17996
	[PublicizedFrom(EAccessModifier.Private)]
	public UnityDistantTerrainWaterPlane waterPlane;

	// Token: 0x0400464D RID: 17997
	[PublicizedFrom(EAccessModifier.Private)]
	public int waterChunks16x16Width;

	// Token: 0x0400464E RID: 17998
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] waterChunks16x16;

	// Token: 0x0400464F RID: 17999
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<uint> visibleVoxelChunks = new HashSet<uint>();

	// Token: 0x04004650 RID: 18000
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] pixelErrorDistanceDiv = new float[] { 60f, 75f, 90f, 160f, 300f };

	// Token: 0x04004651 RID: 18001
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] basemapDistances = new float[] { 300f, 300f, 400f, 550f, 800f };

	// Token: 0x04004652 RID: 18002
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 colClipTerrainAndWater = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04004653 RID: 18003
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 colClipWater = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04004654 RID: 18004
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 colNoClip = new Color32(0, 0, 0, 0);

	// Token: 0x04004655 RID: 18005
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<uint, UnityDistantTerrain.VoxeChunkInfo> dictChunkHeightsArr = new Dictionary<uint, UnityDistantTerrain.VoxeChunkInfo>();

	// Token: 0x04004656 RID: 18006
	[PublicizedFrom(EAccessModifier.Private)]
	public List<uint> toRemove = new List<uint>();

	// Token: 0x04004657 RID: 18007
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Terrain> toUpdateTerrain = new HashSet<Terrain>();

	// Token: 0x04004658 RID: 18008
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i[] posAround = new Vector3i[]
	{
		new Vector3i(0, 0, 0),
		new Vector3i(-1, 0, -1),
		new Vector3i(0, 0, -1),
		new Vector3i(1, 0, -1),
		new Vector3i(-1, 0, 0),
		new Vector3i(1, 0, 0),
		new Vector3i(-1, 0, 1),
		new Vector3i(0, 0, 1),
		new Vector3i(1, 0, 1)
	};

	// Token: 0x04004659 RID: 18009
	[PublicizedFrom(EAccessModifier.Private)]
	public float[,] heights8x8 = new float[9, 9];

	// Token: 0x02000BC2 RID: 3010
	public struct Config
	{
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06005ABB RID: 23227 RVA: 0x0022FEC9 File Offset: 0x0022E0C9
		public int ChunkWorldSize
		{
			get
			{
				return this.DataTileSize;
			}
		}

		// Token: 0x0400465A RID: 18010
		public int DataWidth;

		// Token: 0x0400465B RID: 18011
		public int DataHeight;

		// Token: 0x0400465C RID: 18012
		public int DataTileSize;

		// Token: 0x0400465D RID: 18013
		public int DataSteps;

		// Token: 0x0400465E RID: 18014
		public int SplatSteps;

		// Token: 0x0400465F RID: 18015
		public int MetersPerHeightPix;

		// Token: 0x04004660 RID: 18016
		public int MaxHeight;

		// Token: 0x04004661 RID: 18017
		public int PixelError;
	}

	// Token: 0x02000BC3 RID: 3011
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TerrainAndWater
	{
		// Token: 0x04004662 RID: 18018
		public Terrain terrain;

		// Token: 0x04004663 RID: 18019
		public GameObject waterPlane;
	}

	// Token: 0x02000BC4 RID: 3012
	[PublicizedFrom(EAccessModifier.Private)]
	public class VoxeChunkInfo
	{
		// Token: 0x06005ABD RID: 23229 RVA: 0x0022FED1 File Offset: 0x0022E0D1
		public bool IsEmpty()
		{
			return this.a0 == 0 && this.a1 == 0 && this.a2 == 0 && this.a3 == 0;
		}

		// Token: 0x04004664 RID: 18020
		public bool isDirty;

		// Token: 0x04004665 RID: 18021
		public byte a0;

		// Token: 0x04004666 RID: 18022
		public byte a1;

		// Token: 0x04004667 RID: 18023
		public byte a2;

		// Token: 0x04004668 RID: 18024
		public byte a3;
	}
}
