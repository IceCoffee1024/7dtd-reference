using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C1D RID: 3101
[Preserve]
public class WorldEnvironment : MonoBehaviour
{
	// Token: 0x06005E24 RID: 24100 RVA: 0x0024B9E5 File Offset: 0x00249BE5
	public void Init(WorldCreationData _wcd, World _world)
	{
		this.world = _world;
		this.createPrefab(_wcd);
		this.createTransforms(_wcd);
		this.createWeatherEffects(_wcd);
		this.createAtmosphere(_wcd);
		this.createDistantTerrain();
	}

	// Token: 0x06005E25 RID: 24101 RVA: 0x0024BA10 File Offset: 0x00249C10
	public static void OnXMLChanged()
	{
		DynamicProperties properties = WorldEnvironment.Properties;
		if (properties == null)
		{
			return;
		}
		properties.ParseVec("ambientEquatorScale", ref WorldEnvironment.dataAmbientEquatorScale);
		properties.ParseVec("ambientGroundScale", ref WorldEnvironment.dataAmbientGroundScale);
		properties.ParseVec("ambientSkyScale", ref WorldEnvironment.dataAmbientSkyScale);
		properties.ParseVec("ambientSkyDesat", ref WorldEnvironment.dataAmbientSkyDesat);
		properties.ParseVec("ambientMoon", ref WorldEnvironment.dataAmbientMoon);
		properties.ParseFloat("ambientInsideSpeed", ref WorldEnvironment.dataAmbientInsideSpeed);
		properties.ParseFloat("ambientInsideThreshold", ref WorldEnvironment.dataAmbientInsideThreshold);
		properties.ParseVec("ambientInsideEquatorScale", ref WorldEnvironment.dataAmbientInsideEquatorScale);
		properties.ParseVec("ambientInsideGroundScale", ref WorldEnvironment.dataAmbientInsideGroundScale);
		properties.ParseVec("ambientInsideSkyScale", ref WorldEnvironment.dataAmbientInsideSkyScale);
		properties.ParseVec("fogPower", ref WorldEnvironment.dataFogPow);
		properties.ParseVec("fogWater", ref WorldEnvironment.dataFogWater);
		properties.ParseVec("fogWaterColor", ref WorldEnvironment.dataFogWaterColor);
		properties.ParseFloat("test", ref WorldEnvironment.dataTest);
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x000027FC File Offset: 0x000009FC
	public void CreateUnityTerrain()
	{
	}

	// Token: 0x06005E27 RID: 24103 RVA: 0x0024BB08 File Offset: 0x00249D08
	public static GameObject CreateUnityTerrainOld(string _worldPath, int _sliceAtWidth, int _heightMapDataWidth, int _heightMapDataHeight, List<float[,]> _heightMapDataList, int _worldScale = 1, float _yOffsetTerrain = 1f, int _sliceAt = 2048, bool _bEditMode = true, Action<float> _progressCallback = null)
	{
		MicroStopwatch microStopwatch = new MicroStopwatch();
		MicroStopwatch microStopwatch2 = new MicroStopwatch();
		TextureAtlasTerrain textureAtlasTerrain = (TextureAtlasTerrain)MeshDescription.meshes[5].textureAtlas;
		Texture2D[] array = new Texture2D[]
		{
			textureAtlasTerrain.diffuse[6],
			textureAtlasTerrain.diffuse[1],
			textureAtlasTerrain.diffuse[195],
			textureAtlasTerrain.diffuse[195],
			textureAtlasTerrain.diffuse[185],
			textureAtlasTerrain.diffuse[184],
			textureAtlasTerrain.diffuse[438],
			textureAtlasTerrain.diffuse[288],
			textureAtlasTerrain.diffuse[10],
			textureAtlasTerrain.diffuse[11],
			textureAtlasTerrain.diffuse[403],
			textureAtlasTerrain.diffuse[34]
		};
		Texture2D[] array2 = new Texture2D[]
		{
			textureAtlasTerrain.normal[6],
			textureAtlasTerrain.normal[1],
			textureAtlasTerrain.normal[195],
			textureAtlasTerrain.normal[195],
			textureAtlasTerrain.normal[185],
			textureAtlasTerrain.normal[184],
			textureAtlasTerrain.normal[438],
			textureAtlasTerrain.normal[288],
			textureAtlasTerrain.normal[10],
			textureAtlasTerrain.normal[11],
			textureAtlasTerrain.normal[403],
			textureAtlasTerrain.normal[34]
		};
		TerrainLayer[] array3 = new TerrainLayer[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array3[i] = new TerrainLayer
			{
				diffuseTexture = array[i],
				normalMapTexture = array2[i],
				tileSize = new Vector2(10f, 10f)
			};
		}
		int num = 4;
		int num2;
		int num3;
		Color32[] array4 = TextureUtils.LoadTexturePixels(_worldPath + "/splat1", out num2, out num3, null);
		float num4 = (float)num2 / (float)_heightMapDataWidth;
		if (_progressCallback != null)
		{
			_progressCallback(0.05f);
		}
		string text = _worldPath + "/splat2";
		float num5 = 0f;
		int num6;
		int num7;
		Color32[] array5 = TextureUtils.LoadTexturePixels(text, out num6, out num7, null);
		if (array5 != null)
		{
			num5 = (float)num6 / (float)_heightMapDataWidth;
			num += 4;
		}
		if (_progressCallback != null)
		{
			_progressCallback(0.1f);
		}
		string text2 = _worldPath + "/splat3";
		float num8 = 0f;
		int num9;
		int num10;
		Color32[] array6 = TextureUtils.LoadTexturePixels(text2, out num9, out num10, null);
		if (array6 != null)
		{
			num8 = (float)num9 / (float)_heightMapDataWidth;
			num += 4;
		}
		if (_progressCallback != null)
		{
			_progressCallback(0.15f);
		}
		microStopwatch2.ResetAndRestart();
		for (int j = 0; j < array4.Length; j++)
		{
			Color32 color = array4[j];
			int num11 = (int)color.r;
			int num12 = (int)color.g;
			int num13 = (int)color.b;
			int num14 = (int)color.a;
			if (num > 4)
			{
				color = array5[j];
				num11 += (int)color.r;
				num12 += (int)color.g;
				num13 += (int)color.b;
				num14 += (int)color.a;
			}
			if (num > 8)
			{
				color = array6[j];
				num11 += (int)color.r;
				num12 += (int)color.g;
				num13 += (int)color.b;
				num14 += (int)color.a;
			}
			if (num11 + num12 + num13 + num14 < 255)
			{
				array4[j] = new Color32((byte)((int)array4[j].r + (255 - (num11 + num12 + num13 + num14))), array4[j].g, array4[j].b, array4[j].a);
			}
		}
		if (_progressCallback != null)
		{
			_progressCallback(0.2f);
		}
		Log.Out("Splat1 color fix {0}ms", new object[] { microStopwatch2.ElapsedMilliseconds });
		microStopwatch2.ResetAndRestart();
		GameObject gameObject = new GameObject("Terrain");
		float num15 = (float)(_heightMapDataWidth / 2) + 0.5f;
		gameObject.transform.position = new Vector3(-1f * num15 * (float)_worldScale, 1f, -1f * num15 * (float)_worldScale);
		Origin.Add(gameObject.transform, 0);
		int num16 = 0;
		int num17 = _heightMapDataWidth / _sliceAtWidth;
		int num18 = _heightMapDataHeight / _sliceAtWidth;
		Terrain[,] array7 = new Terrain[num17, num18];
		float[,,] array8 = new float[_sliceAtWidth, _sliceAtWidth, num];
		for (int k = 0; k < num18; k++)
		{
			for (int l = 0; l < num17; l++)
			{
				microStopwatch2.ResetAndRestart();
				TerrainData terrainData = new TerrainData();
				terrainData.heightmapResolution = _sliceAtWidth;
				terrainData.size = new Vector3((float)(_sliceAtWidth * _worldScale), 256f, (float)(_sliceAtWidth * _worldScale));
				terrainData.SetHeights(0, 0, _heightMapDataList[num16++]);
				Log.Out("Setting heights to unity terrain took " + microStopwatch2.ElapsedMilliseconds.ToString() + "ms");
				microStopwatch2.ResetAndRestart();
				terrainData.alphamapResolution = _sliceAtWidth;
				terrainData.terrainLayers = array3;
				for (int m = 0; m < _sliceAtWidth; m++)
				{
					int num19 = k * _sliceAtWidth + m;
					int num20 = (int)((float)num19 * num4);
					for (int n = 0; n < _sliceAtWidth; n++)
					{
						int num21 = l * _sliceAtWidth + n;
						int num22 = (int)((float)num21 * num4 + (float)(num20 * num2)) % array4.Length;
						Color32 color2 = array4[num22];
						array8[m, n, 0] = (float)color2.r * 0.003921569f;
						array8[m, n, 1] = (float)color2.g * 0.003921569f;
						array8[m, n, 2] = (float)color2.b * 0.003921569f;
						array8[m, n, 3] = (float)color2.a * 0.003921569f;
						if (num > 4)
						{
							num22 = (int)((float)num21 * num5 + (float)((int)((float)num19 * num5) * num6)) % array5.Length;
							color2 = array5[num22];
							array8[m, n, 4] = (float)color2.r * 0.003921569f;
							array8[m, n, 5] = (float)color2.g * 0.003921569f;
							array8[m, n, 6] = (float)color2.b * 0.003921569f;
							array8[m, n, 7] = (float)color2.a * 0.003921569f;
						}
						if (num > 8)
						{
							num22 = (int)((float)num21 * num8 + (float)((int)((float)num19 * num8) * num9)) % array6.Length;
							color2 = array6[num22];
							array8[m, n, 8] = (float)color2.r * 0.003921569f;
							array8[m, n, 9] = (float)color2.g * 0.003921569f;
							array8[m, n, 10] = (float)color2.b * 0.003921569f;
							array8[m, n, 11] = (float)color2.a * 0.003921569f;
						}
					}
				}
				terrainData.SetAlphamaps(0, 0, array8);
				Log.Out("Splats took " + microStopwatch2.ElapsedMilliseconds.ToString() + "ms");
				microStopwatch2.ResetAndRestart();
				GameObject gameObject2 = Terrain.CreateTerrainGameObject(terrainData);
				Terrain component = gameObject2.GetComponent<Terrain>();
				if (_bEditMode)
				{
					component.heightmapPixelError = 5f;
					component.basemapDistance = 2000f;
				}
				else
				{
					component.heightmapPixelError = 20f;
				}
				array7[l, k] = component;
				gameObject2.layer = 16;
				gameObject2.tag = "T_Mesh";
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = new Vector3((float)(l * _sliceAtWidth * _worldScale), _yOffsetTerrain, (float)(k * _sliceAtWidth * _worldScale));
				float num23 = (float)(k * num17 + l + 1) / (float)(num17 * num18);
				if (_progressCallback != null)
				{
					_progressCallback(0.2f + num23 * 0.8f);
				}
			}
		}
		for (int num24 = 0; num24 < num18; num24++)
		{
			for (int num25 = 0; num25 < num17; num25++)
			{
				array7[num25, num24].SetNeighbors((num25 > 0) ? array7[num25 - 1, num24] : null, (num24 > 0) ? array7[num25, num24 - 1] : null, (num25 < num17 - 1) ? array7[num25 + 1, num24] : null, (num24 < num18 - 1) ? array7[num25, num24 + 1] : null);
			}
		}
		if (_progressCallback != null)
		{
			_progressCallback(1f);
		}
		Log.Out("Creating unity terrain took " + microStopwatch.ElapsedMilliseconds.ToString() + "ms");
		return gameObject;
	}

	// Token: 0x06005E28 RID: 24104 RVA: 0x0024C3DC File Offset: 0x0024A5DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void createDistantTerrain()
	{
		if (!GameManager.IsDedicatedServer && !this.world.ChunkCache.IsFixedSize)
		{
			this.chunkClusterVisibleDelegate = new ChunkCluster.OnChunkVisibleDelegate(this.OnChunkDisplayed);
			this.world.ChunkCache.OnChunkVisibleDelegates += this.chunkClusterVisibleDelegate;
		}
	}

	// Token: 0x06005E29 RID: 24105 RVA: 0x0024C42A File Offset: 0x0024A62A
	public void OnChunkDisplayed(long _key, bool _bDisplayed)
	{
		if (UnityDistantTerrainTest.Instance != null)
		{
			UnityDistantTerrainTest.Instance.OnChunkVisible(WorldChunkCache.extractX(_key), WorldChunkCache.extractZ(_key), _bDisplayed);
		}
	}

	// Token: 0x06005E2A RID: 24106 RVA: 0x0024C44A File Offset: 0x0024A64A
	public void Cleanup()
	{
		this.localPlayer = null;
	}

	// Token: 0x06005E2B RID: 24107 RVA: 0x0024C454 File Offset: 0x0024A654
	[PublicizedFrom(EAccessModifier.Private)]
	public void createPrefab(WorldCreationData _wcd)
	{
		if (_wcd.Properties.Contains("WorldEnvironment", "Prefab"))
		{
			global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(_wcd.Properties.GetString("WorldEnvironment", "Prefab"))).transform.parent = base.transform;
		}
	}

	// Token: 0x06005E2C RID: 24108 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createWeatherEffects(WorldCreationData _wcd)
	{
	}

	// Token: 0x06005E2D RID: 24109 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createTransforms(WorldCreationData _wcd)
	{
	}

	// Token: 0x06005E2E RID: 24110 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createAtmosphere(WorldCreationData _wcd)
	{
	}

	// Token: 0x06005E2F RID: 24111 RVA: 0x0024C4A7 File Offset: 0x0024A6A7
	public Color GetAmbientColor()
	{
		if (this.world == null || this.world.BiomeAtmosphereEffects == null)
		{
			return Color.black;
		}
		return this.world.BiomeAtmosphereEffects.GetSkyColorSpectrum(this.dayTimeScalar);
	}

	// Token: 0x06005E30 RID: 24112 RVA: 0x0024C4DC File Offset: 0x0024A6DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void AmbientSpectrumFrameUpdate()
	{
		if (this.world == null || this.world.BiomeAtmosphereEffects == null)
		{
			return;
		}
		float num = 0f;
		if (this.localPlayer && this.localPlayer.PlayerStats.LightInsidePer >= WorldEnvironment.dataAmbientInsideThreshold)
		{
			num = 1f;
		}
		this.insideCurrent = Mathf.MoveTowards(this.insideCurrent, num, WorldEnvironment.dataAmbientInsideSpeed * Time.deltaTime);
		float dayPercent = SkyManager.dayPercent;
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxBrightness);
		float num2 = ((@float < 0.5f) ? Utils.FastLerpUnclamped(0.2f, 1f, @float * 2f) : (@float + 0.5f));
		float num3 = Mathf.LerpUnclamped(Utils.FastMax(1f, num2), num2, this.insideCurrent);
		num2 = Utils.FastLerp(num2, num3, dayPercent * 2f);
		this.brightnessInOutDayNight = num2;
		float num4 = SkyManager.GetMoonAmbientScale(WorldEnvironment.dataAmbientMoon.x, WorldEnvironment.dataAmbientMoon.y);
		num4 = Mathf.LerpUnclamped(num4, 1f, this.insideCurrent);
		num2 *= num4;
		num2 += this.nightVisionBrightness;
		Color skyColor = SkyManager.GetSkyColor();
		Color color = new Color(0.7f, 0.7f, 0.7f, 1f);
		float num5 = Mathf.LerpUnclamped(WorldEnvironment.dataAmbientSkyDesat.y, WorldEnvironment.dataAmbientSkyDesat.x, dayPercent);
		Color color2 = Color.LerpUnclamped(skyColor, color, num5);
		float num6 = Mathf.LerpUnclamped(WorldEnvironment.dataAmbientSkyScale.y, WorldEnvironment.dataAmbientSkyScale.x, dayPercent);
		float num7 = Mathf.LerpUnclamped(WorldEnvironment.dataAmbientInsideSkyScale.y, WorldEnvironment.dataAmbientInsideSkyScale.x, dayPercent);
		float num8 = Mathf.LerpUnclamped(num6, num7, this.insideCurrent);
		float num9 = SkyManager.GetLuma(color2) * num8;
		num8 *= num2;
		RenderSettings.ambientSkyColor = color2 * num8;
		float num10 = Mathf.LerpUnclamped(WorldEnvironment.dataAmbientEquatorScale.y, WorldEnvironment.dataAmbientEquatorScale.x, dayPercent);
		float num11 = Mathf.LerpUnclamped(WorldEnvironment.dataAmbientInsideEquatorScale.y, WorldEnvironment.dataAmbientInsideEquatorScale.x, dayPercent);
		num10 = Mathf.LerpUnclamped(num10, num11, this.insideCurrent);
		num10 *= num2;
		RenderSettings.ambientEquatorColor = SkyManager.GetFogColor() * num10;
		Color sunLightColor = SkyManager.GetSunLightColor();
		float num12 = Mathf.LerpUnclamped(WorldEnvironment.dataAmbientGroundScale.y, WorldEnvironment.dataAmbientGroundScale.x, dayPercent);
		float num13 = Mathf.LerpUnclamped(WorldEnvironment.dataAmbientInsideGroundScale.y, WorldEnvironment.dataAmbientInsideGroundScale.x, dayPercent);
		float num14 = Mathf.LerpUnclamped(num12, num13, this.insideCurrent);
		num9 += SkyManager.GetLuma(sunLightColor) * num14;
		num14 *= num2;
		RenderSettings.ambientGroundColor = sunLightColor * num14;
		WorldEnvironment.AmbientTotal = num9 * num4;
	}

	// Token: 0x06005E31 RID: 24113 RVA: 0x0024C774 File Offset: 0x0024A974
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpectrumsFrameUpdate()
	{
		if (this.world == null || !this.localPlayer)
		{
			return;
		}
		BiomeAtmosphereEffects biomeAtmosphereEffects = this.world.BiomeAtmosphereEffects;
		if (biomeAtmosphereEffects == null)
		{
			return;
		}
		Color color = biomeAtmosphereEffects.GetSkyColorSpectrum(this.dayTimeScalar);
		SkyManager.SetSkyColor(color);
		color = biomeAtmosphereEffects.GetSunColorSpectrum(this.dayTimeScalar);
		SkyManager.SetSunColor(color);
		SkyManager.SetSunIntensity(color.a * 2f);
		color = biomeAtmosphereEffects.GetMoonColorSpectrum(this.dayTimeScalar);
		SkyManager.SetMoonLightColor(color);
		color = biomeAtmosphereEffects.GetFogColorSpectrum(this.dayTimeScalar);
		Color fogFadeColorSpectrum = biomeAtmosphereEffects.GetFogFadeColorSpectrum(this.dayTimeScalar);
		SkyManager.SetFogFade(1f - fogFadeColorSpectrum.r - 0.5f, 1f - fogFadeColorSpectrum.a);
		Color color2 = new Color(color.r, color.g, color.b, 1f);
		color2 *= Utils.FastMin(this.brightnessInOutDayNight, 1f);
		float dayPercent = SkyManager.dayPercent;
		float num = Mathf.Pow(color.a, Utils.FastLerpUnclamped(WorldEnvironment.dataFogPow.y, WorldEnvironment.dataFogPow.x, dayPercent));
		num += WeatherManager.currentWeather.FogPercent();
		if (num > 1f)
		{
			num = 1f;
		}
		if (this.fogDensityOverride >= 0f)
		{
			num = this.fogDensityOverride;
			color2 = this.fogColorOverride;
		}
		float num2 = 0.01f;
		if (this.localPlayer.IsUnderwaterCamera)
		{
			this.isUnderWater = true;
			num = WorldEnvironment.dataFogWater.x;
			num2 = WorldEnvironment.dataFogWater.y;
			float num3 = 0.35f + SkyManager.dayPercent * 0.65f;
			color2 = new Color(WorldEnvironment.dataFogWaterColor.x * num3, WorldEnvironment.dataFogWaterColor.y * num3, WorldEnvironment.dataFogWaterColor.z * num3, 1f);
		}
		else if (this.isUnderWater)
		{
			this.isUnderWater = false;
			num2 = 1f;
		}
		if (this.localPlayer.bPlayingSpawnIn)
		{
			num2 = 1f;
		}
		if (this.localPlayer.InWorldLookPercent < 1f)
		{
			color2 = Color.LerpUnclamped(new Color(0.5f, 0.5f, 0.2f), color2, this.localPlayer.InWorldLookPercent);
			num = Utils.FastLerpUnclamped(0.6f, num, this.localPlayer.InWorldLookPercent);
		}
		SkyManager.SetFogColor(Color.Lerp(SkyManager.GetFogColor(), color2, num2));
		SkyManager.SetFogDensity(Mathf.Lerp(SkyManager.GetFogDensity(), num, num2));
	}

	// Token: 0x06005E32 RID: 24114 RVA: 0x0024C9E8 File Offset: 0x0024ABE8
	public void WorldTimeChanged()
	{
		WeatherManager.SetWorldTime(this.world.worldTime);
		SkyManager.SetGameTime(this.world.worldTime);
		if (this.world.BiomeAtmosphereEffects != null)
		{
			this.world.BiomeAtmosphereEffects.Update();
		}
	}

	// Token: 0x06005E33 RID: 24115 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void enableAllDisplayedDistantChunks(bool _bEnable)
	{
	}

	// Token: 0x06005E34 RID: 24116 RVA: 0x0024CA28 File Offset: 0x0024AC28
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.dayTimeScalar = SkyManager.GetWorldRotation();
		if (!WeatherManager.Instance)
		{
			return;
		}
		WeatherManager.Instance.FrameUpdate();
		this.AmbientSpectrumFrameUpdate();
		if (this.localPlayer == null)
		{
			this.localPlayer = this.world.GetPrimaryPlayer();
			if (this.localPlayer == null)
			{
				return;
			}
		}
		if (!GameManager.IsDedicatedServer && UnityDistantTerrainTest.Instance != null)
		{
			UnityDistantTerrainTest.Instance.FrameUpdate(this.localPlayer);
		}
		this.SpectrumsFrameUpdate();
		IList<ChunkGameObject> displayedChunkGameObjects = this.world.m_ChunkManager.GetDisplayedChunkGameObjects();
		int count = displayedChunkGameObjects.Count;
		int num = count;
		if (num > 8)
		{
			num = 8;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = this.chunkLODIndex + 1;
			this.chunkLODIndex = num2;
			if (num2 >= count)
			{
				this.chunkLODIndex = 0;
			}
			displayedChunkGameObjects[this.chunkLODIndex].CheckLODs(-1);
		}
	}

	// Token: 0x06005E35 RID: 24117 RVA: 0x000027FC File Offset: 0x000009FC
	public void CreateLevelBorderBox(World _world)
	{
	}

	// Token: 0x06005E36 RID: 24118 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetColliders(float _worldX, float _worldZ, float _worldXDim, float _worldZDim, float _waterSize, float _bDistance)
	{
	}

	// Token: 0x06005E37 RID: 24119 RVA: 0x0024CB0B File Offset: 0x0024AD0B
	public Color GetSunLightColor()
	{
		return this.world.BiomeAtmosphereEffects.GetSunColorSpectrum(this.dayTimeScalar);
	}

	// Token: 0x06005E38 RID: 24120 RVA: 0x0024CB23 File Offset: 0x0024AD23
	public Color GetMoonLightColor()
	{
		return this.world.BiomeAtmosphereEffects.GetMoonColorSpectrum(this.dayTimeScalar);
	}

	// Token: 0x06005E39 RID: 24121 RVA: 0x0024CB3C File Offset: 0x0024AD3C
	public static float CalculateCelestialAngle(ulong _worldTime, float _t)
	{
		float num = ((float)((int)(_worldTime % 24000UL)) + _t) / 24000f - 0.25f;
		if (num < 0f)
		{
			num += 1f;
		}
		if (num > 1f)
		{
			num -= 1f;
		}
		float num2 = num;
		num = 1f - (float)((Math.Cos((double)num * 3.141592653589793) + 1.0) / 2.0);
		return num2 + (num - num2) / 3f;
	}

	// Token: 0x06005E3A RID: 24122 RVA: 0x0024CBBE File Offset: 0x0024ADBE
	public BiomeAtmosphereEffects GetBiomeAtmosphereEffects()
	{
		return this.world.BiomeAtmosphereEffects;
	}

	// Token: 0x06005E3B RID: 24123 RVA: 0x0024CBCC File Offset: 0x0024ADCC
	public int DistantTerrain_GetBlockIdAt(int x, int y, int z)
	{
		int poiblockIdOverride = this.world.ChunkCache.ChunkProvider.GetPOIBlockIdOverride(x, z);
		if (poiblockIdOverride != 0)
		{
			return poiblockIdOverride;
		}
		BlockValue blockValue = this.world.ChunkCache.ChunkProvider.GetBiomeProvider().GetTopmostBlockValue(x, z);
		if (!blockValue.isair)
		{
			return blockValue.type;
		}
		float num;
		BiomeDefinition biomeAt = this.world.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt(x, z, out num);
		if (biomeAt == null)
		{
			return 1;
		}
		blockValue = biomeAt.m_Layers[0].m_Block.blockValues[0];
		return blockValue.type;
	}

	// Token: 0x06005E3C RID: 24124 RVA: 0x0024CC6C File Offset: 0x0024AE6C
	public void SetFogOverride(Color _color = default(Color), float _density = -1f)
	{
		this.fogColorOverride = _color;
		this.fogDensityOverride = _density;
	}

	// Token: 0x06005E3D RID: 24125 RVA: 0x0024CC7C File Offset: 0x0024AE7C
	public void SetNightVision(float _brightness)
	{
		this.nightVisionBrightness = _brightness * 0.4f;
	}

	// Token: 0x0400491D RID: 18717
	public static DynamicProperties Properties;

	// Token: 0x0400491E RID: 18718
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientEquatorScale;

	// Token: 0x0400491F RID: 18719
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientGroundScale;

	// Token: 0x04004920 RID: 18720
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientSkyScale;

	// Token: 0x04004921 RID: 18721
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientSkyDesat;

	// Token: 0x04004922 RID: 18722
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientMoon;

	// Token: 0x04004923 RID: 18723
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float dataAmbientInsideSpeed;

	// Token: 0x04004924 RID: 18724
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float dataAmbientInsideThreshold;

	// Token: 0x04004925 RID: 18725
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientInsideEquatorScale;

	// Token: 0x04004926 RID: 18726
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientInsideGroundScale;

	// Token: 0x04004927 RID: 18727
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataAmbientInsideSkyScale;

	// Token: 0x04004928 RID: 18728
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataFogPow;

	// Token: 0x04004929 RID: 18729
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 dataFogWater;

	// Token: 0x0400492A RID: 18730
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector3 dataFogWaterColor;

	// Token: 0x0400492B RID: 18731
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float dataTest;

	// Token: 0x0400492C RID: 18732
	public const float cFogTransitionSpeed = 0.01f;

	// Token: 0x0400492D RID: 18733
	public const float cBrightnessMin = 0.2f;

	// Token: 0x0400492E RID: 18734
	public static float AmbientTotal;

	// Token: 0x0400492F RID: 18735
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public World world;

	// Token: 0x04004930 RID: 18736
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04004931 RID: 18737
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float dayTimeScalar;

	// Token: 0x04004932 RID: 18738
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float insideCurrent;

	// Token: 0x04004933 RID: 18739
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Color fogColorOverride;

	// Token: 0x04004934 RID: 18740
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fogDensityOverride = -1f;

	// Token: 0x04004935 RID: 18741
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float brightnessInOutDayNight;

	// Token: 0x04004936 RID: 18742
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nightVisionBrightness;

	// Token: 0x04004937 RID: 18743
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isUnderWater;

	// Token: 0x04004938 RID: 18744
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ChunkCluster.OnChunkVisibleDelegate chunkClusterVisibleDelegate;

	// Token: 0x04004939 RID: 18745
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bTerrainActived;

	// Token: 0x0400493A RID: 18746
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int chunkLODIndex;
}
