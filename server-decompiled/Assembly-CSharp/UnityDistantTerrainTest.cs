using System;
using UnityEngine;

// Token: 0x02000BD7 RID: 3031
public class UnityDistantTerrainTest
{
	// Token: 0x06005B48 RID: 23368 RVA: 0x00232C12 File Offset: 0x00230E12
	public static void Create()
	{
		UnityDistantTerrainTest.Instance = new UnityDistantTerrainTest();
	}

	// Token: 0x06005B49 RID: 23369 RVA: 0x00232C20 File Offset: 0x00230E20
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

	// Token: 0x06005B4A RID: 23370 RVA: 0x00232DC0 File Offset: 0x00230FC0
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

	// Token: 0x06005B4B RID: 23371 RVA: 0x00232DFD File Offset: 0x00230FFD
	public void FrameUpdate(EntityPlayerLocal _player)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.FrameUpdate(_player);
		}
	}

	// Token: 0x06005B4C RID: 23372 RVA: 0x00232E13 File Offset: 0x00231013
	public void OnChunkVisible(int _chunkX, int _chunkZ, bool _visible)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.OnChunkUpdate(_chunkX, _chunkZ, _visible);
		}
	}

	// Token: 0x06005B4D RID: 23373 RVA: 0x00232E2B File Offset: 0x0023102B
	public void Cleanup()
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.Cleanup();
			this.unityTerrain = null;
		}
		global::UnityEngine.Object.Destroy(this.parentObj);
	}

	// Token: 0x04004705 RID: 18181
	public const string cGameObjectName = "DistantUnityTerrain";

	// Token: 0x04004706 RID: 18182
	public static UnityDistantTerrainTest Instance;

	// Token: 0x04004707 RID: 18183
	public UnityDistantTerrain unityTerrain;

	// Token: 0x04004708 RID: 18184
	public IBackedArray<ushort> HeightMap;

	// Token: 0x04004709 RID: 18185
	public int hmWidth = 6144;

	// Token: 0x0400470A RID: 18186
	public int hmHeight = 6144;

	// Token: 0x0400470B RID: 18187
	public Material TerrainMaterial;

	// Token: 0x0400470C RID: 18188
	public Material WaterMaterial;

	// Token: 0x0400470D RID: 18189
	public int WaterChunks16x16Width;

	// Token: 0x0400470E RID: 18190
	public byte[] WaterChunks16x16;

	// Token: 0x0400470F RID: 18191
	public GameObject parentObj;

	// Token: 0x04004710 RID: 18192
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTileSize = 512;
}
