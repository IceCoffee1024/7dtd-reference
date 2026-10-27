using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000B4F RID: 2895
public class ChunkProviderGenerateWorldFromImage : ChunkProviderGenerateWorld
{
	// Token: 0x06005725 RID: 22309 RVA: 0x00219448 File Offset: 0x00217648
	public ChunkProviderGenerateWorldFromImage(string _levelName, PathAbstractions.AbstractedLocation _worldLocation, bool _bLoadDTM = true)
		: base(_levelName, _worldLocation, false)
	{
		this.bLoadDTM = _bLoadDTM;
	}

	// Token: 0x06005726 RID: 22310 RVA: 0x0021945A File Offset: 0x0021765A
	public override void Cleanup()
	{
		HeightMap heightMap = this.heightMap;
		if (heightMap != null)
		{
			heightMap.Dispose();
		}
		this.heightMap = null;
		IBackedArray<ushort> backedArray = this.rawdata;
		if (backedArray != null)
		{
			backedArray.Dispose();
		}
		this.rawdata = null;
		base.Cleanup();
	}

	// Token: 0x06005727 RID: 22311 RVA: 0x00219492 File Offset: 0x00217692
	public override IEnumerator Init(World _world)
	{
		if (this.bLoadDTM)
		{
			yield return this.<>n__0(_world);
		}
		else
		{
			this.world = _world;
		}
		Stopwatch sw = new Stopwatch();
		sw.Start();
		if (this.bLoadDTM)
		{
			this.loadDTM();
		}
		Log.Out("Loading and parsing of dtm.png took " + sw.ElapsedMilliseconds.ToString() + "ms");
		sw.Reset();
		sw.Start();
		HeightMap heightMap = this.heightMap;
		if (heightMap != null)
		{
			heightMap.Dispose();
		}
		this.heightMap = new HeightMap(this.m_Dtm.DimX, this.m_Dtm.DimY, 255f, this.rawdata, 0);
		this.m_BiomeProvider = new WorldBiomeProviderFromImage(this.levelName, this.worldLocation.FullPath, this.world.Biomes, 4096);
		WorldDecoratorPOIFromImage worldDecoratorPOIFromImage = new WorldDecoratorPOIFromImage(this.levelName, this.worldLocation.FullPath, this.GetDynamicPrefabDecorator(), this.m_Dtm.DimX, this.m_Dtm.DimY, null, false, 1, this.heightMap, null);
		this.m_Decorators.Add(worldDecoratorPOIFromImage);
		yield return worldDecoratorPOIFromImage.InitData();
		this.m_Decorators.Add(new WorldDecoratorBlocksFromBiome(this.m_BiomeProvider, this.GetDynamicPrefabDecorator()));
		Log.Out("Loading and parsing of generator took " + sw.ElapsedMilliseconds.ToString() + "ms");
		sw.Reset();
		sw.Start();
		if (this.bLoadDTM)
		{
			this.m_TerrainGenerator = new TerrainFromDTM();
			((TerrainFromDTM)this.m_TerrainGenerator).Init(this.m_Dtm, this.m_BiomeProvider, this.levelName, this.worldLocation.FullPath, this.world.Seed);
			string text = (_world.IsEditor() ? null : GameIO.GetSaveGameRegionDir());
			this.m_RegionFileManager = new RegionFileManager(text, text, 0, !_world.IsEditor());
			MultiBlockManager.Instance.Initialize(this.m_RegionFileManager);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06005728 RID: 22312 RVA: 0x002194A8 File Offset: 0x002176A8
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void loadDTM()
	{
		Texture2D texture2D = null;
		if (SdFile.Exists(this.worldLocation.FullPath + "/dtm.png"))
		{
			texture2D = TextureUtils.LoadTexture(this.worldLocation.FullPath + "/dtm.png", FilterMode.Point, false, false, null);
			Log.Out("Loading local DTM");
		}
		else if (SdFile.Exists(this.worldLocation.FullPath + "/dtm.tga"))
		{
			texture2D = TextureUtils.LoadTexture(this.worldLocation.FullPath + "/dtm.tga", FilterMode.Point, false, false, null);
			Log.Out("Loading local DTM");
		}
		else
		{
			texture2D = Resources.Load("Data/Worlds/" + this.levelName + "/dtm", typeof(Texture2D)) as Texture2D;
		}
		Log.Out("DTM image size w= " + texture2D.width.ToString() + ", h = " + texture2D.height.ToString());
		Color[] pixels = texture2D.GetPixels();
		this.m_Dtm = new ArrayWithOffset<byte>(texture2D.width, texture2D.height);
		IBackedArray<ushort> backedArray = this.rawdata;
		if (backedArray != null)
		{
			backedArray.Dispose();
		}
		this.rawdata = BackedArrays.Create<ushort>(pixels.Length);
		int width = texture2D.width;
		int height = texture2D.height;
		for (int i = 0; i < height; i++)
		{
			Span<ushort> span2;
			using (this.rawdata.GetSpan(i * width, width, out span2))
			{
				for (int j = 0; j < width; j++)
				{
					this.m_Dtm[j + this.m_Dtm.MinPos.x, i + this.m_Dtm.MinPos.y] = (byte)(pixels[width * i + j].grayscale * 255f);
					*span2[j] = (ushort)((int)((byte)(pixels[width * i + j].grayscale * 255f)) | ((int)((byte)((pixels[width * i + j].grayscale * 255f - 255f) * 255f)) << 8));
				}
			}
		}
		Resources.UnloadAsset(texture2D);
	}

	// Token: 0x06005729 RID: 22313 RVA: 0x002196E8 File Offset: 0x002178E8
	public override void ReloadAllChunks()
	{
		this.loadDTM();
		((TerrainFromDTM)this.m_TerrainGenerator).Init(this.m_Dtm, this.m_BiomeProvider, this.levelName, this.worldLocation.FullPath, this.world.Seed);
		base.ReloadAllChunks();
	}

	// Token: 0x0600572A RID: 22314 RVA: 0x0002F260 File Offset: 0x0002D460
	public override EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.GenerateFromDtm;
	}

	// Token: 0x0600572B RID: 22315 RVA: 0x0021973C File Offset: 0x0021793C
	public override bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
	{
		_minSize = new Vector3i(this.m_Dtm.MinPos.x, 0, this.m_Dtm.MinPos.y);
		_maxSize = new Vector3i(this.m_Dtm.MaxPos.x, 255, this.m_Dtm.MaxPos.y);
		return true;
	}

	// Token: 0x0600572C RID: 22316 RVA: 0x002197A8 File Offset: 0x002179A8
	public override int GetPOIBlockIdOverride(int _x, int _z)
	{
		WorldDecoratorPOIFromImage worldDecoratorPOIFromImage = (WorldDecoratorPOIFromImage)this.m_Decorators[0];
		WorldGridCompressedData<byte> poi = worldDecoratorPOIFromImage.m_Poi;
		int num = _x / worldDecoratorPOIFromImage.worldScale;
		int num2 = _z / worldDecoratorPOIFromImage.worldScale;
		if (!poi.Contains(num, num2))
		{
			return 0;
		}
		byte data = poi.GetData(num, num2);
		if (data == 0 || data == 255)
		{
			return 0;
		}
		PoiMapElement poiForColor = this.world.Biomes.getPoiForColor((uint)data);
		if (poiForColor == null)
		{
			return 0;
		}
		return poiForColor.m_BlockValue.type;
	}

	// Token: 0x0600572D RID: 22317 RVA: 0x0021982C File Offset: 0x00217A2C
	public override float GetPOIHeightOverride(int x, int z)
	{
		WorldDecoratorPOIFromImage worldDecoratorPOIFromImage = (WorldDecoratorPOIFromImage)this.m_Decorators[0];
		WorldGridCompressedData<byte> poi = worldDecoratorPOIFromImage.m_Poi;
		byte data;
		if (!poi.Contains(x / worldDecoratorPOIFromImage.worldScale, z / worldDecoratorPOIFromImage.worldScale) || (data = poi.GetData(x / worldDecoratorPOIFromImage.worldScale, z / worldDecoratorPOIFromImage.worldScale)) == 255 || data == 0)
		{
			return 0f;
		}
		PoiMapElement poiForColor = this.world.Biomes.getPoiForColor((uint)data);
		if (poiForColor == null)
		{
			return 0f;
		}
		if (!poiForColor.m_BlockValue.Block.blockMaterial.IsLiquid)
		{
			return 0f;
		}
		return (float)poiForColor.m_YPosFill;
	}

	// Token: 0x040043A7 RID: 17319
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayWithOffset<byte> m_Dtm;

	// Token: 0x040043A8 RID: 17320
	public IBackedArray<ushort> rawdata;

	// Token: 0x040043A9 RID: 17321
	[PublicizedFrom(EAccessModifier.Private)]
	public HeightMap heightMap;

	// Token: 0x040043AA RID: 17322
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bLoadDTM;
}
