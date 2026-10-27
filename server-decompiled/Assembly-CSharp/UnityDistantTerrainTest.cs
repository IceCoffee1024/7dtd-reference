using System;
using UnityEngine;

// Token: 0x02000BF4 RID: 3060
public class UnityDistantTerrainTest
{
	// Token: 0x06005B86 RID: 23430 RVA: 0x0023745A File Offset: 0x0023565A
	public static void Create()
	{
		UnityDistantTerrainTest.Instance = new UnityDistantTerrainTest();
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x00237468 File Offset: 0x00235668
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

	// Token: 0x06005B88 RID: 23432 RVA: 0x00237608 File Offset: 0x00235808
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

	// Token: 0x06005B89 RID: 23433 RVA: 0x00237645 File Offset: 0x00235845
	public void FrameUpdate(EntityPlayerLocal _player)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.FrameUpdate(_player);
		}
	}

	// Token: 0x06005B8A RID: 23434 RVA: 0x0023765B File Offset: 0x0023585B
	public void OnChunkVisible(int _chunkX, int _chunkZ, bool _visible)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.OnChunkUpdate(_chunkX, _chunkZ, _visible);
		}
	}

	// Token: 0x06005B8B RID: 23435 RVA: 0x00237673 File Offset: 0x00235873
	public void Cleanup()
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.Cleanup();
			this.unityTerrain = null;
		}
		global::UnityEngine.Object.Destroy(this.parentObj);
	}

	// Token: 0x04004769 RID: 18281
	public const string cGameObjectName = "DistantUnityTerrain";

	// Token: 0x0400476A RID: 18282
	public static UnityDistantTerrainTest Instance;

	// Token: 0x0400476B RID: 18283
	public UnityDistantTerrain unityTerrain;

	// Token: 0x0400476C RID: 18284
	public IBackedArray<ushort> HeightMap;

	// Token: 0x0400476D RID: 18285
	public int hmWidth = 6144;

	// Token: 0x0400476E RID: 18286
	public int hmHeight = 6144;

	// Token: 0x0400476F RID: 18287
	public Material TerrainMaterial;

	// Token: 0x04004770 RID: 18288
	public Material WaterMaterial;

	// Token: 0x04004771 RID: 18289
	public int WaterChunks16x16Width;

	// Token: 0x04004772 RID: 18290
	public byte[] WaterChunks16x16;

	// Token: 0x04004773 RID: 18291
	public GameObject parentObj;

	// Token: 0x04004774 RID: 18292
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTileSize = 512;
}
