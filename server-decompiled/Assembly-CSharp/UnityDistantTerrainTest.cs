using System;
using UnityEngine;

// Token: 0x02000BE9 RID: 3049
public class UnityDistantTerrainTest
{
	// Token: 0x06005B61 RID: 23393 RVA: 0x00235F0E File Offset: 0x0023410E
	public static void Create()
	{
		UnityDistantTerrainTest.Instance = new UnityDistantTerrainTest();
	}

	// Token: 0x06005B62 RID: 23394 RVA: 0x00235F1C File Offset: 0x0023411C
	public void LoadTerrain()
	{
		if (!this.parentObj)
		{
			this.parentObj = new GameObject("DistantUnityTerrain");
		}
		UnityDistantTerrain.Config config = new UnityDistantTerrain.Config
		{
			DataWidth = this.hmWidth,
			DataHeight = this.hmHeight,
			DataTileSize = 512,
			DataSteps = 2,
			SplatSteps = 1,
			MetersPerHeightPix = 2,
			MaxHeight = 256,
			PixelError = 5
		};
		int num = Mathf.CeilToInt((float)(Mathf.Clamp(GamePrefs.GetInt(EnumGamePrefs.OptionsGfxTerrainQuality), 2, 4) * 512) / (float)config.ChunkWorldSize * 2f) + 1;
		int num2 = config.DataWidth / config.DataTileSize;
		int num3 = config.DataHeight / config.DataTileSize;
		TileAreaConfig tileAreaConfig = new TileAreaConfig
		{
			tileStart = new Vector2i(-num2 / 2, -num3 / 2),
			tileEnd = new Vector2i(Utils.FastMax(0, num2 / 2 - 1), Utils.FastMax(0, num3 / 2 - 1)),
			tileSizeInWorldUnits = config.ChunkWorldSize,
			bWrapAroundX = false,
			bWrapAroundZ = false
		};
		TileAreaConfig tileAreaConfig2 = tileAreaConfig;
		tileAreaConfig2.tileSizeInWorldUnits = config.DataTileSize;
		ITileArea<float[,]> tileArea = this.LoadTerrainHeightTiles(tileAreaConfig, this.HeightMap, config.DataWidth, config.DataHeight, config.DataTileSize);
		this.unityTerrain = new UnityDistantTerrain();
		this.unityTerrain.Init(this.parentObj, config, num, this.TerrainMaterial, this.WaterMaterial, this.WaterChunks16x16Width, this.WaterChunks16x16, tileArea, null, null, null);
	}

	// Token: 0x06005B63 RID: 23395 RVA: 0x002360BC File Offset: 0x002342BC
	public ITileArea<float[,]> LoadTerrainHeightTiles(TileAreaConfig _config, IBackedArray<ushort> _rawHeightMap, int _heightMapWidth, int _heightMapHeight, int _sliceAtPix)
	{
		if (PlatformOptimizations.FileBackedTerrainTiles)
		{
			TileFile<float> tileFile = HeightMapUtils.ConvertAndSliceUnityHeightmapQuarteredToFile(_rawHeightMap, _heightMapWidth, _heightMapHeight, _sliceAtPix);
			return new TileAreaCache<float>(_config, tileFile, 9);
		}
		float[,][,] array = HeightMapUtils.ConvertAndSliceUnityHeightmapQuartered(_rawHeightMap, _heightMapWidth, _heightMapHeight, _sliceAtPix);
		return new TileArea<float[,]>(_config, array);
	}

	// Token: 0x06005B64 RID: 23396 RVA: 0x002360F9 File Offset: 0x002342F9
	public void FrameUpdate(EntityPlayerLocal _player)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.FrameUpdate(_player);
		}
	}

	// Token: 0x06005B65 RID: 23397 RVA: 0x0023610F File Offset: 0x0023430F
	public void OnChunkVisible(int _chunkX, int _chunkZ, bool _visible)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.OnChunkUpdate(_chunkX, _chunkZ, _visible);
		}
	}

	// Token: 0x06005B66 RID: 23398 RVA: 0x00236127 File Offset: 0x00234327
	public void Cleanup()
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.Cleanup();
			this.unityTerrain = null;
		}
		global::UnityEngine.Object.Destroy(this.parentObj);
	}

	// Token: 0x04004744 RID: 18244
	public const string cGameObjectName = "DistantUnityTerrain";

	// Token: 0x04004745 RID: 18245
	public static UnityDistantTerrainTest Instance;

	// Token: 0x04004746 RID: 18246
	public UnityDistantTerrain unityTerrain;

	// Token: 0x04004747 RID: 18247
	public IBackedArray<ushort> HeightMap;

	// Token: 0x04004748 RID: 18248
	public int hmWidth = 6144;

	// Token: 0x04004749 RID: 18249
	public int hmHeight = 6144;

	// Token: 0x0400474A RID: 18250
	public Material TerrainMaterial;

	// Token: 0x0400474B RID: 18251
	public Material WaterMaterial;

	// Token: 0x0400474C RID: 18252
	public int WaterChunks16x16Width;

	// Token: 0x0400474D RID: 18253
	public byte[] WaterChunks16x16;

	// Token: 0x0400474E RID: 18254
	public GameObject parentObj;

	// Token: 0x0400474F RID: 18255
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTileSize = 512;
}
