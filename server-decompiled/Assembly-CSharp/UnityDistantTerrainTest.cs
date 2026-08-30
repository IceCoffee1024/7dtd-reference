using System;
using UnityEngine;

// Token: 0x02000BC5 RID: 3013
public class UnityDistantTerrainTest
{
	// Token: 0x06005ABF RID: 23231 RVA: 0x0022FEF6 File Offset: 0x0022E0F6
	public static void Create()
	{
		UnityDistantTerrainTest.Instance = new UnityDistantTerrainTest();
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x0022FF04 File Offset: 0x0022E104
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

	// Token: 0x06005AC1 RID: 23233 RVA: 0x002300A4 File Offset: 0x0022E2A4
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

	// Token: 0x06005AC2 RID: 23234 RVA: 0x002300E1 File Offset: 0x0022E2E1
	public void FrameUpdate(EntityPlayerLocal _player)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.FrameUpdate(_player);
		}
	}

	// Token: 0x06005AC3 RID: 23235 RVA: 0x002300F7 File Offset: 0x0022E2F7
	public void OnChunkVisible(int _chunkX, int _chunkZ, bool _visible)
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.OnChunkUpdate(_chunkX, _chunkZ, _visible);
		}
	}

	// Token: 0x06005AC4 RID: 23236 RVA: 0x0023010F File Offset: 0x0022E30F
	public void Cleanup()
	{
		if (this.unityTerrain != null)
		{
			this.unityTerrain.Cleanup();
			this.unityTerrain = null;
		}
		global::UnityEngine.Object.Destroy(this.parentObj);
	}

	// Token: 0x04004669 RID: 18025
	public const string cGameObjectName = "DistantUnityTerrain";

	// Token: 0x0400466A RID: 18026
	public static UnityDistantTerrainTest Instance;

	// Token: 0x0400466B RID: 18027
	public UnityDistantTerrain unityTerrain;

	// Token: 0x0400466C RID: 18028
	public IBackedArray<ushort> HeightMap;

	// Token: 0x0400466D RID: 18029
	public int hmWidth = 6144;

	// Token: 0x0400466E RID: 18030
	public int hmHeight = 6144;

	// Token: 0x0400466F RID: 18031
	public Material TerrainMaterial;

	// Token: 0x04004670 RID: 18032
	public Material WaterMaterial;

	// Token: 0x04004671 RID: 18033
	public int WaterChunks16x16Width;

	// Token: 0x04004672 RID: 18034
	public byte[] WaterChunks16x16;

	// Token: 0x04004673 RID: 18035
	public GameObject parentObj;

	// Token: 0x04004674 RID: 18036
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cTileSize = 512;
}
