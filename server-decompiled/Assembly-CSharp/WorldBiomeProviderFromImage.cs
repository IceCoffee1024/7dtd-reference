using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000C69 RID: 3177
public class WorldBiomeProviderFromImage : IBiomeProvider
{
	// Token: 0x06006017 RID: 24599 RVA: 0x00261C91 File Offset: 0x0025FE91
	public WorldBiomeProviderFromImage(string _levelName, string _worldPath, WorldBiomes _biomes, int _worldSize = 4096)
	{
		this.worldName = _levelName;
		this.worldPath = _worldPath;
		this.m_Biomes = _biomes;
		this.worldSize = _worldSize;
		this.worldSizeHalf = _worldSize / 2;
	}

	// Token: 0x06006018 RID: 24600 RVA: 0x00261CCB File Offset: 0x0025FECB
	public IEnumerator InitData()
	{
		string text = this.worldPath + "/biomes";
		Texture2D biomesTex;
		if (SdFile.Exists(text + ".tga"))
		{
			biomesTex = TextureUtils.LoadTexture(text + ".tga", FilterMode.Point, false, false, null);
		}
		else
		{
			biomesTex = TextureUtils.LoadTexture(text + ".png", FilterMode.Point, false, false, null);
		}
		yield return null;
		this.biomeMapWidth = biomesTex.width;
		this.biomeMapHeight = biomesTex.height;
		this.biomesMapWidthHalf = biomesTex.width / 2;
		this.biomesMapHeightHalf = biomesTex.height / 2;
		Log.Out("Biomes image size w= " + biomesTex.width.ToString() + ", h = " + biomesTex.height.ToString());
		yield return null;
		BiomeImageLoader biomesLoader = new BiomeImageLoader(biomesTex, this.m_Biomes.GetBiomeMap());
		yield return biomesLoader.Load();
		this.m_BiomeMap = biomesLoader.biomeMap;
		biomesLoader = default(BiomeImageLoader);
		yield return null;
		this.biomesScaleDiv = this.worldSize / biomesTex.width;
		global::UnityEngine.Object.Destroy(biomesTex);
		this.noiseGen = new PerlinNoise(this.worldName.GetStableHashCode());
		yield return null;
		Texture2D texture2D = null;
		text = this.worldPath + "/radiation";
		if (SdFile.Exists(text + ".png"))
		{
			texture2D = TextureUtils.LoadTexture(text + ".png", FilterMode.Point, false, false, null);
		}
		else if (SdFile.Exists(text + ".tga"))
		{
			texture2D = TextureUtils.LoadTexture(text + ".tga", FilterMode.Point, false, false, null);
		}
		if (texture2D != null)
		{
			this.radiationMapSize = texture2D.width;
			this.radiationMapScale = this.worldSize / this.radiationMapSize;
			if (texture2D.width <= 512 && texture2D.height <= 512)
			{
				this.radiationMapSmall = new byte[texture2D.width * texture2D.height];
				if (texture2D.format == TextureFormat.RGBA32)
				{
					using (NativeArray<Color32> pixelData = texture2D.GetPixelData<Color32>(0))
					{
						for (int i = 0; i < pixelData.Length; i++)
						{
							this.radiationMapSmall[i] = pixelData[i].r;
						}
						goto IL_037A;
					}
				}
				using (NativeArray<TextureUtils.ColorARGB32> pixelData2 = texture2D.GetPixelData<TextureUtils.ColorARGB32>(0))
				{
					for (int j = 0; j < pixelData2.Length; j++)
					{
						this.radiationMapSmall[j] = pixelData2[j].r;
					}
					goto IL_037A;
				}
			}
			Log.Out("Radiation ignored {0}", new object[] { this.radiationMapSize });
			IL_037A:
			global::UnityEngine.Object.Destroy(texture2D);
		}
		yield break;
	}

	// Token: 0x06006019 RID: 24601 RVA: 0x00261CDC File Offset: 0x0025FEDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void FillRadiationResult<T>(NativeArray<T> radPixs, byte[,][,] result, Func<T, byte> processColor) where T : struct
	{
		for (int i = 0; i < result.GetLength(0); i++)
		{
			for (int j = 0; j < result.GetLength(1); j++)
			{
				result[j, i] = new byte[512, 512];
				for (int k = 0; k < 512; k++)
				{
					for (int l = 0; l < 512; l++)
					{
						int num = (i * 512 + k) * this.radiationMapSize + (j * 512 + l);
						T t = radPixs[num];
						byte b = processColor(t);
						result[j, i][l, k] = b;
					}
				}
			}
		}
	}

	// Token: 0x0600601A RID: 24602 RVA: 0x00261D90 File Offset: 0x0025FF90
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void FillRadiationFileBackedArray<T>(NativeArray<T> radPixs, FileBackedArray<byte> fba, Func<T, byte> processColor) where T : struct
	{
		for (int i = 0; i < this.radiationTilesZ; i++)
		{
			for (int j = 0; j < this.radiationTilesX; j++)
			{
				int num = i * 512 * 512 * this.radiationTilesX + j * 512 * 512;
				int num2 = 0;
				Span<byte> span2;
				using (fba.GetSpan(num, 262144, out span2))
				{
					for (int k = 0; k < 512; k++)
					{
						for (int l = 0; l < 512; l++)
						{
							int num3 = (i * 512 + l) * this.radiationMapSize + (j * 512 + k);
							T t = radPixs[num3];
							byte b = processColor(t);
							*span2[num2] = b;
							num2++;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600601B RID: 24603 RVA: 0x00261E88 File Offset: 0x00260088
	[PublicizedFrom(EAccessModifier.Private)]
	public byte ProcessColor(Color32 pixel)
	{
		byte b = 0;
		if (pixel.g > 0)
		{
			b = 1;
		}
		if (pixel.b > 0)
		{
			b = 2;
		}
		if (pixel.r > 0)
		{
			b = 3;
		}
		return b;
	}

	// Token: 0x0600601C RID: 24604 RVA: 0x00261EBC File Offset: 0x002600BC
	[PublicizedFrom(EAccessModifier.Private)]
	public byte ProcessColor(TextureUtils.ColorARGB32 pixel)
	{
		byte b = 0;
		if (pixel.g > 0)
		{
			b = 1;
		}
		if (pixel.b > 0)
		{
			b = 2;
		}
		if (pixel.r > 0)
		{
			b = 3;
		}
		return b;
	}

	// Token: 0x0600601D RID: 24605 RVA: 0x00261EF0 File Offset: 0x002600F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void RadiationTileArrayFileFromTexture(FileBackedArray<byte> fba, Texture2D radiationTexture)
	{
		if (radiationTexture.format == TextureFormat.RGBA32)
		{
			using (NativeArray<Color32> pixelData = radiationTexture.GetPixelData<Color32>(0))
			{
				this.FillRadiationFileBackedArray<Color32>(pixelData, fba, new Func<Color32, byte>(this.ProcessColor));
				return;
			}
		}
		using (NativeArray<TextureUtils.ColorARGB32> pixelData2 = radiationTexture.GetPixelData<TextureUtils.ColorARGB32>(0))
		{
			this.FillRadiationFileBackedArray<TextureUtils.ColorARGB32>(pixelData2, fba, new Func<TextureUtils.ColorARGB32, byte>(this.ProcessColor));
		}
	}

	// Token: 0x0600601E RID: 24606 RVA: 0x00261F7C File Offset: 0x0026017C
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[,][,] RadiationTileArrayFromTexture(Texture2D radiationTexture)
	{
		int num = radiationTexture.width / 512;
		int num2 = radiationTexture.height / 512;
		byte[,][,] array = new byte[num, num2][,];
		if (radiationTexture.format == TextureFormat.RGBA32)
		{
			using (NativeArray<Color32> pixelData = radiationTexture.GetPixelData<Color32>(0))
			{
				this.FillRadiationResult<Color32>(pixelData, array, new Func<Color32, byte>(this.ProcessColor));
				return array;
			}
		}
		using (NativeArray<TextureUtils.ColorARGB32> pixelData2 = radiationTexture.GetPixelData<TextureUtils.ColorARGB32>(0))
		{
			this.FillRadiationResult<TextureUtils.ColorARGB32>(pixelData2, array, new Func<TextureUtils.ColorARGB32, byte>(this.ProcessColor));
		}
		return array;
	}

	// Token: 0x0600601F RID: 24607 RVA: 0x00262028 File Offset: 0x00260228
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldCoordsToTileCoords(int worldX, int worldZ, TileAreaConfig tileAreaConfig, out int tileX, out int tileZ, out int posX, out int posZ)
	{
		tileX = (worldX + this.worldSizeHalf) / 512 + tileAreaConfig.tileStart.x;
		tileZ = (worldZ + this.worldSizeHalf) / 512 + tileAreaConfig.tileStart.y;
		posX = worldX - tileX * 512;
		posZ = worldZ - tileZ * 512;
	}

	// Token: 0x06006020 RID: 24608 RVA: 0x0026208C File Offset: 0x0026028C
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileArea<byte[,]> LoadRadiationMap(Texture2D radiationTex, TileAreaConfig tileAreaConfig)
	{
		byte[,][,] array = this.RadiationTileArrayFromTexture(radiationTex);
		return new TileArea<byte[,]>(tileAreaConfig, array);
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x002620A8 File Offset: 0x002602A8
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileArea<byte[,]> LoadRadiationMapToFile(Texture2D radiationTex, TileAreaConfig tileAreaConfig)
	{
		FileBackedArray<byte> fileBackedArray = new FileBackedArray<byte>(radiationTex.width * radiationTex.height);
		this.RadiationTileArrayFileFromTexture(fileBackedArray, radiationTex);
		TileFile<byte> tileFile = new TileFile<byte>(fileBackedArray, 512, this.radiationTilesX, this.radiationTilesZ);
		return new TileAreaCache<byte>(tileAreaConfig, tileFile, 9);
	}

	// Token: 0x06006022 RID: 24610 RVA: 0x002620F4 File Offset: 0x002602F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadSplatMaps(string _levelName, int _worldWidth)
	{
		string fullPath = PathAbstractions.WorldsSearchPaths.GetLocation(_levelName, null, null).FullPath;
		string text = fullPath + "/splat1.png";
		if (!SdFile.Exists(text))
		{
			return;
		}
		Texture2D texture2D = TextureUtils.LoadTexture(text, FilterMode.Point, false, false, null);
		Color32[] pixels = texture2D.GetPixels32();
		Color32[] array = null;
		Color32[] array2 = null;
		this.splatW = texture2D.width;
		int height = texture2D.height;
		this.splatScaleDiv = _worldWidth / this.splatW;
		if (Application.isEditor)
		{
			global::UnityEngine.Object.DestroyImmediate(texture2D);
		}
		else
		{
			global::UnityEngine.Object.Destroy(texture2D);
		}
		this.cntSplatChannels += 4;
		string text2 = fullPath + "/splat2.png";
		if (SdFile.Exists(text2))
		{
			Texture2D texture2D2 = TextureUtils.LoadTexture(text2, FilterMode.Point, false, false, null);
			array = texture2D2.GetPixels32();
			if (Application.isEditor)
			{
				global::UnityEngine.Object.DestroyImmediate(texture2D2);
			}
			else
			{
				global::UnityEngine.Object.Destroy(texture2D2);
			}
			this.cntSplatChannels += 4;
		}
		string text3 = fullPath + "/splat3.png";
		if (SdFile.Exists(text3))
		{
			Texture2D texture2D3 = TextureUtils.LoadTexture(text3, FilterMode.Point, false, false, null);
			array2 = texture2D3.GetPixels32();
			if (Application.isEditor)
			{
				global::UnityEngine.Object.DestroyImmediate(texture2D3);
			}
			else
			{
				global::UnityEngine.Object.Destroy(texture2D3);
			}
			this.cntSplatChannels += 4;
		}
		this.splatMapMaxValue = new byte[pixels.Length];
		Color32 color = new Color32(0, 0, 0, 0);
		Color32 color2 = new Color32(0, 0, 0, 0);
		Color32 color3 = new Color32(0, 0, 0, 0);
		for (int i = pixels.Length - 1; i >= 0; i--)
		{
			color = pixels[i];
			if (this.cntSplatChannels > 4)
			{
				color2 = array[i];
			}
			if (this.cntSplatChannels > 8)
			{
				color3 = array2[i];
			}
			int num = 0;
			if (color.r >= color.g && color.r >= color.b && color.r >= color.a && color.r >= color2.r && color.r >= color2.g && color.r >= color2.b && color.r >= color2.a && color.r >= color3.r && color.r >= color3.g && color.r >= color3.b && color.r >= color3.a)
			{
				num = 0;
			}
			else if (color.g >= color.r && color.g >= color.b && color.g >= color.a && color.g >= color2.r && color.g >= color2.g && color.g >= color2.b && color.g >= color2.a && color.g >= color3.r && color.g >= color3.g && color.g >= color3.b && color.g >= color3.a)
			{
				num = 1;
			}
			else if (color.b >= color.r && color.b >= color.g && color.b >= color.a && color.b >= color2.r && color.b >= color2.g && color.b >= color2.b && color.b >= color2.a && color.b >= color3.r && color.b >= color3.g && color.b >= color3.b && color.b >= color3.a)
			{
				num = 2;
			}
			else if (color.a >= color.r && color.a >= color.g && color.a >= color.b && color.a >= color2.r && color.a >= color2.g && color.a >= color2.b && color.a >= color2.a && color.a >= color3.r && color.a >= color3.g && color.a >= color3.b && color.a >= color3.a)
			{
				num = 3;
			}
			else if (color2.r >= color2.g && color2.r >= color2.b && color2.r >= color2.a && color2.r >= color.r && color2.r >= color.g && color2.r >= color.b && color2.r >= color.a && color2.r >= color3.r && color2.r >= color3.g && color2.r >= color3.b && color2.r >= color3.a)
			{
				num = 4;
			}
			else if (color2.g >= color2.r && color2.g >= color2.b && color2.g >= color2.a && color2.g >= color.r && color2.g >= color.g && color2.g >= color.b && color2.g >= color.a && color2.g >= color3.r && color2.g >= color3.g && color2.g >= color3.b && color2.g >= color3.a)
			{
				num = 5;
			}
			else if (color2.b >= color2.r && color2.b >= color2.g && color2.b >= color2.a && color2.b >= color.r && color2.b >= color.g && color2.b >= color.b && color2.b >= color.a && color2.b >= color3.r && color2.b >= color3.g && color2.b >= color3.b && color2.b >= color3.a)
			{
				num = 6;
			}
			else if (color2.a >= color2.r && color2.a >= color2.g && color2.a >= color2.b && color2.a >= color.r && color2.a >= color.g && color2.a >= color.b && color2.a >= color.a && color2.a >= color3.r && color2.a >= color3.g && color2.a >= color3.b && color2.a >= color3.a)
			{
				num = 7;
			}
			else if (color3.r >= color3.g && color3.r >= color3.b && color3.r >= color3.a && color3.r >= color.r && color3.r >= color.g && color3.r >= color.b && color3.r >= color.a && color3.r >= color2.r && color3.r >= color2.g && color3.r >= color2.b && color3.r >= color2.a)
			{
				num = 8;
			}
			else if (color3.g >= color3.r && color3.g >= color3.b && color3.g >= color3.a && color3.g >= color.r && color3.g >= color.g && color3.g >= color.b && color3.g >= color.a && color3.g >= color2.r && color3.g >= color2.g && color3.g >= color2.b && color3.g >= color2.a)
			{
				num = 9;
			}
			else if (color3.b >= color3.r && color3.b >= color3.g && color3.b >= color3.a && color3.b >= color.r && color3.b >= color.g && color3.b >= color.b && color3.b >= color.a && color3.b >= color2.r && color3.b >= color2.g && color3.b >= color2.b && color3.b >= color3.a)
			{
				num = 10;
			}
			else if (color3.a >= color3.r && color3.a >= color3.g && color3.a >= color3.b && color3.a >= color.r && color3.a >= color.g && color3.a >= color.b && color3.a >= color.a && color3.a >= color2.r && color3.a >= color2.g && color3.a >= color2.b && color3.a >= color2.a)
			{
				num = 11;
			}
			this.splatMapMaxValue[i] = (byte)num;
		}
	}

	// Token: 0x06006023 RID: 24611 RVA: 0x00262BD1 File Offset: 0x00260DD1
	public string GetWorldName()
	{
		return this.worldName;
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2)
	{
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x00262BD9 File Offset: 0x00260DD9
	public BiomeDefinition GetBiomeAt(int x, int z, out float _intensity)
	{
		_intensity = 1f;
		return this.GetBiomeAt(x, z);
	}

	// Token: 0x06006026 RID: 24614 RVA: 0x00262BEC File Offset: 0x00260DEC
	public BiomeDefinition GetBiomeAt(int x, int z)
	{
		if (this.biomesScaleDiv == 0)
		{
			return null;
		}
		int num = x / this.biomesScaleDiv + this.biomesMapWidthHalf;
		if (num < 0 || num >= this.m_BiomeMap.width)
		{
			return null;
		}
		int num2 = z / this.biomesScaleDiv + this.biomesMapHeightHalf;
		if (num2 < 0 || num2 >= this.m_BiomeMap.height)
		{
			return null;
		}
		byte value = this.m_BiomeMap.GetValue(num, num2);
		if (value == 255)
		{
			return null;
		}
		return this.m_Biomes.GetBiome(value);
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x00262C70 File Offset: 0x00260E70
	public BiomeDefinition GetBiomeOrSubAt(int x, int z)
	{
		BiomeDefinition biomeDefinition = this.GetBiomeAt(x, z);
		if (biomeDefinition != null)
		{
			int subBiomeIdxAt = this.GetSubBiomeIdxAt(biomeDefinition, x, 0, z);
			if (subBiomeIdxAt >= 0)
			{
				biomeDefinition = biomeDefinition.subbiomes[subBiomeIdxAt];
			}
		}
		return biomeDefinition;
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x00262CA8 File Offset: 0x00260EA8
	public int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z)
	{
		double num = -3.4028234663852886E+38;
		double num2 = 0.0;
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < bd.subbiomes.Count; i++)
		{
			BiomeDefinition biomeDefinition = bd.subbiomes[i];
			if (num != (double)biomeDefinition.noiseFreq || vector != biomeDefinition.noiseOffset)
			{
				num = (double)biomeDefinition.noiseFreq;
				vector = biomeDefinition.noiseOffset;
				num2 = (double)((float)this.noiseGen.FBM((double)((float)_x + vector.x), (double)((float)_z + vector.y), num));
				num2 = num2 * 0.5 + 0.5;
			}
			if (num2 >= (double)biomeDefinition.noiseMin && num2 < (double)biomeDefinition.noiseMax)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06006029 RID: 24617 RVA: 0x00262D77 File Offset: 0x00260F77
	public Vector2i GetSize()
	{
		return new Vector2i(this.biomeMapWidth, this.biomeMapHeight);
	}

	// Token: 0x0600602A RID: 24618 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetHumidityAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x0600602B RID: 24619 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetTemperatureAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x0600602C RID: 24620 RVA: 0x00262D8C File Offset: 0x00260F8C
	public float GetRadiationAt(int x, int z)
	{
		if (this.radiationMapSmall == null)
		{
			return 0f;
		}
		int num = (x + this.worldSizeHalf) / this.radiationMapScale + (z + this.worldSizeHalf) / this.radiationMapScale * this.radiationMapSize;
		if (num >= 0 && num < this.radiationMapSmall.Length)
		{
			return (float)this.radiationMapSmall[num];
		}
		return 0f;
	}

	// Token: 0x0600602D RID: 24621 RVA: 0x00262DEC File Offset: 0x00260FEC
	public BlockValue GetTopmostBlockValue(int xWorld, int zWorld)
	{
		this.bvReturn.type = 0;
		if (xWorld < -this.worldSizeHalf || xWorld >= this.worldSizeHalf || zWorld < -this.worldSizeHalf || zWorld >= this.worldSizeHalf)
		{
			return this.bvReturn;
		}
		if (this.cntSplatChannels > 0)
		{
			switch (this.splatMapMaxValue[(xWorld + this.worldSizeHalf) / this.splatScaleDiv + (zWorld + this.worldSizeHalf) / this.splatScaleDiv * this.splatW])
			{
			case 0:
				this.bvReturn.type = 14;
				break;
			case 1:
				this.bvReturn.type = 1;
				break;
			case 2:
				this.bvReturn.type = 9;
				break;
			case 3:
				this.bvReturn.type = 8;
				break;
			case 4:
				this.bvReturn.type = 12;
				break;
			case 5:
				this.bvReturn.type = 13;
				break;
			case 6:
				this.bvReturn.type = 16;
				break;
			case 7:
				this.bvReturn.type = 11;
				break;
			case 8:
				this.bvReturn.type = 3;
				break;
			case 9:
				this.bvReturn.type = 29;
				break;
			case 10:
				this.bvReturn.type = 28;
				break;
			case 11:
				this.bvReturn.type = 2;
				break;
			}
		}
		return this.bvReturn;
	}

	// Token: 0x0600602E RID: 24622 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04004B6B RID: 19307
	[PublicizedFrom(EAccessModifier.Private)]
	public GridCompressedData<byte> m_BiomeMap;

	// Token: 0x04004B6C RID: 19308
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBiomes m_Biomes;

	// Token: 0x04004B6D RID: 19309
	[PublicizedFrom(EAccessModifier.Private)]
	public PerlinNoise noiseGen;

	// Token: 0x04004B6E RID: 19310
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string worldName;

	// Token: 0x04004B6F RID: 19311
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string worldPath;

	// Token: 0x04004B70 RID: 19312
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomeMapWidth;

	// Token: 0x04004B71 RID: 19313
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomesMapWidthHalf;

	// Token: 0x04004B72 RID: 19314
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomeMapHeight;

	// Token: 0x04004B73 RID: 19315
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomesMapHeightHalf;

	// Token: 0x04004B74 RID: 19316
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomesScaleDiv;

	// Token: 0x04004B75 RID: 19317
	[PublicizedFrom(EAccessModifier.Private)]
	public int radiationMapSize;

	// Token: 0x04004B76 RID: 19318
	[PublicizedFrom(EAccessModifier.Private)]
	public int radiationMapScale;

	// Token: 0x04004B77 RID: 19319
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] radiationMapSmall;

	// Token: 0x04004B78 RID: 19320
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRadiationTileSize = 512;

	// Token: 0x04004B79 RID: 19321
	[PublicizedFrom(EAccessModifier.Private)]
	public int radiationTilesX;

	// Token: 0x04004B7A RID: 19322
	[PublicizedFrom(EAccessModifier.Private)]
	public int radiationTilesZ;

	// Token: 0x04004B7B RID: 19323
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] splatMapMaxValue;

	// Token: 0x04004B7C RID: 19324
	[PublicizedFrom(EAccessModifier.Private)]
	public int cntSplatChannels;

	// Token: 0x04004B7D RID: 19325
	[PublicizedFrom(EAccessModifier.Private)]
	public int splatScaleDiv;

	// Token: 0x04004B7E RID: 19326
	[PublicizedFrom(EAccessModifier.Private)]
	public int splatW;

	// Token: 0x04004B7F RID: 19327
	[PublicizedFrom(EAccessModifier.Private)]
	public int worldSize;

	// Token: 0x04004B80 RID: 19328
	[PublicizedFrom(EAccessModifier.Private)]
	public int worldSizeHalf;

	// Token: 0x04004B81 RID: 19329
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue bvReturn = BlockValue.Air;
}
