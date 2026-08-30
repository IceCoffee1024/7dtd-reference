using System;
using System.Collections.Generic;
using HorizonBasedAmbientOcclusion;
using PI.NGSS;
using TND.DLSS;
using TND.FSR;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001326 RID: 4902
public class GameRenderManager
{
	// Token: 0x06009AAF RID: 39599 RVA: 0x003A68DD File Offset: 0x003A4ADD
	public static GameRenderManager Create(EntityPlayerLocal player)
	{
		GameRenderManager gameRenderManager = new GameRenderManager();
		gameRenderManager.player = player;
		gameRenderManager.Init();
		return gameRenderManager;
	}

	// Token: 0x06009AB0 RID: 39600 RVA: 0x003A68F4 File Offset: 0x003A4AF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		this.graphManager = GameGraphManager.Create(this.player);
		this.lightManager = GameLightManager.Create(this.player);
		this.reflectionManager = ReflectionManager.Create(this.player);
		this.PostProcessInit();
		this.DynamicResolutionInit();
	}

	// Token: 0x06009AB1 RID: 39601 RVA: 0x003A6940 File Offset: 0x003A4B40
	public void Destroy()
	{
		this.lightManager.Destroy();
		this.lightManager = null;
		this.reflectionManager.Destroy();
		this.reflectionManager = null;
		this.DynamicResolutionDestroyRT();
	}

	// Token: 0x06009AB2 RID: 39602 RVA: 0x003A696C File Offset: 0x003A4B6C
	public void FrameUpdate()
	{
		this.lightManager.FrameUpdate();
		this.reflectionManager.FrameUpdate();
		this.DynamicResolutionUpdate();
	}

	// Token: 0x06009AB3 RID: 39603 RVA: 0x003A698A File Offset: 0x003A4B8A
	[PublicizedFrom(EAccessModifier.Private)]
	public void PostProcessInit()
	{
		Camera playerCamera = this.player.playerCamera;
	}

	// Token: 0x17001244 RID: 4676
	// (get) Token: 0x06009AB4 RID: 39604 RVA: 0x003A6998 File Offset: 0x003A4B98
	// (set) Token: 0x06009AB5 RID: 39605 RVA: 0x003A699F File Offset: 0x003A4B9F
	public static int TextureMipmapLimit
	{
		get
		{
			return QualitySettings.globalTextureMipmapLimit;
		}
		set
		{
			QualitySettings.globalTextureMipmapLimit = value;
		}
	}

	// Token: 0x06009AB6 RID: 39606 RVA: 0x003A69A8 File Offset: 0x003A4BA8
	public static void ApplyCameraOptions(EntityPlayerLocal player)
	{
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if (player)
		{
			player.renderManager.ApplyCameraOptions();
			return;
		}
		List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
		for (int i = 0; i < localPlayers.Count; i++)
		{
			localPlayers[i].renderManager.ApplyCameraOptions();
		}
	}

	// Token: 0x06009AB7 RID: 39607 RVA: 0x003A6A08 File Offset: 0x003A4C08
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyCameraOptions()
	{
		Camera playerCamera = this.player.playerCamera;
		this.layer = playerCamera.GetComponent<PostProcessLayer>();
		playerCamera.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals | DepthTextureMode.MotionVectors;
		NGSS_FrustumShadows_7DTD component = playerCamera.GetComponent<NGSS_FrustumShadows_7DTD>();
		switch (GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowQuality))
		{
		case 0:
		case 1:
		case 2:
			component.enabled = false;
			break;
		case 3:
			component.enabled = true;
			component.m_shadowsBlurIterations = 1;
			component.m_raySamples = 32;
			break;
		case 4:
			component.enabled = true;
			component.m_shadowsBlurIterations = 2;
			component.m_raySamples = 48;
			break;
		case 5:
			component.enabled = true;
			component.m_shadowsBlurIterations = 4;
			component.m_raySamples = 64;
			break;
		}
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxAA);
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxAASharpness);
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxBloom);
		int int2 = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxSSReflections);
		bool bool2 = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxSSAO);
		bool bool3 = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxSunShafts);
		int num = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxMotionBlur);
		if (!GamePrefs.GetBool(EnumGamePrefs.OptionsGfxMotionBlurEnabled))
		{
			num = 0;
		}
		PostProcessVolume component2 = playerCamera.GetComponent<PostProcessVolume>();
		if (component2)
		{
			PostProcessProfile profile = component2.profile;
			if (profile)
			{
				component2.enabled = false;
				ScreenSpaceReflections setting = profile.GetSetting<ScreenSpaceReflections>();
				if (setting)
				{
					switch (int2)
					{
					case 1:
						setting.maximumIterationCount.Override(200);
						setting.resolution.Override(ScreenSpaceReflectionResolution.Downsampled);
						break;
					case 2:
						setting.maximumIterationCount.Override(120);
						setting.resolution.Override(ScreenSpaceReflectionResolution.FullSize);
						break;
					case 3:
						setting.maximumIterationCount.Override(250);
						setting.resolution.Override(ScreenSpaceReflectionResolution.FullSize);
						break;
					}
					setting.enabled.Override(int2 > 0);
				}
				MotionBlur setting2 = profile.GetSetting<MotionBlur>();
				setting2.enabled.Override(num != 0);
				if (num != 1)
				{
					if (num == 2)
					{
						setting2.shutterAngle.Override(270f);
						setting2.sampleCount.Override(10);
					}
				}
				else
				{
					setting2.shutterAngle.Override(135f);
					setting2.sampleCount.Override(5);
				}
				profile.GetSetting<Bloom>().enabled.Override(@bool);
				ColorGrading setting3 = profile.GetSetting<ColorGrading>();
				float num2 = 0.5f - GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxBrightness);
				if (num2 < 0f)
				{
					num2 *= 0.4f;
				}
				else
				{
					num2 = 0f;
				}
				setting3.toneCurveGamma.Override(1f + num2);
				SunShaftsEffect sunShaftsEffect;
				if (profile.TryGetSettings<SunShaftsEffect>(out sunShaftsEffect))
				{
					sunShaftsEffect.enabled.Override(bool3);
				}
				component2.enabled = true;
			}
		}
		HBAO component3 = playerCamera.GetComponent<HBAO>();
		if (component3)
		{
			GameOptionsPlatforms.GfxPreset int3 = (GameOptionsPlatforms.GfxPreset)GamePrefs.GetInt(EnumGamePrefs.OptionsGfxQualityPreset);
			switch (int3)
			{
			case GameOptionsPlatforms.GfxPreset.Lowest:
			case GameOptionsPlatforms.GfxPreset.Low:
			case GameOptionsPlatforms.GfxPreset.Medium:
				component3.SetQuality(HBAO.Quality.Low);
				goto IL_031B;
			case GameOptionsPlatforms.GfxPreset.High:
				component3.SetQuality(HBAO.Quality.Medium);
				goto IL_031B;
			case GameOptionsPlatforms.GfxPreset.Ultra:
				component3.SetQuality(HBAO.Quality.High);
				goto IL_031B;
			case GameOptionsPlatforms.GfxPreset.Custom:
			case GameOptionsPlatforms.GfxPreset.ConsolePerformance:
			case GameOptionsPlatforms.GfxPreset.LEGACY_ConsolePerformanceFSR:
			case GameOptionsPlatforms.GfxPreset.ConsoleQuality:
				break;
			default:
				if (int3 != GameOptionsPlatforms.GfxPreset.Simplified)
				{
				}
				break;
			}
			component3.SetQuality(HBAO.Quality.Medium);
			IL_031B:
			component3.enabled = bool2;
		}
		int num3 = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode);
		if (num3 == 2 && !FSR3.FSR3Supported())
		{
			num3 = 4;
			GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, num3);
		}
		if (num3 == 5 && (!FSR3.FSR3Supported() || !DLSS.DLSSSupported()))
		{
			num3 = 4;
			GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, num3);
		}
		int int4 = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxDynamicMinFPS);
		float num4;
		if (num3 != 3)
		{
			if (num3 != 4)
			{
				num4 = -1f;
			}
			else
			{
				num4 = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxDynamicScale);
			}
		}
		else
		{
			num4 = 0f;
		}
		float num5 = num4;
		this.SetDynamicResolution(num5, (float)int4, -1f);
		if (this.layer)
		{
			if (num3 == 5 || num3 == 2)
			{
				PostProcessLayer postProcessLayer = this.layer;
				PostProcessLayer.Antialiasing antialiasing;
				if (num3 == 5)
				{
					antialiasing = PostProcessLayer.Antialiasing.DLSS;
				}
				else
				{
					antialiasing = PostProcessLayer.Antialiasing.FSR3;
				}
				postProcessLayer.antialiasingMode = antialiasing;
				this.layer.fsr3.sharpness = @float;
				this.layer.dlss.sharpness = @float;
				this.UpscalingSetQuality(GamePrefs.GetInt(EnumGamePrefs.OptionsGfxFSRPreset));
			}
			else
			{
				this.SetAntialiasing(@int, @float, this.layer);
			}
			Rect rect = playerCamera.rect;
			rect.x = ((this.layer.antialiasingMode == PostProcessLayer.Antialiasing.DLSS || this.layer.antialiasingMode == PostProcessLayer.Antialiasing.FSR3) ? 1E-07f : 0f);
			playerCamera.rect = rect;
		}
		this.reflectionManager.ApplyCameraOptions(playerCamera);
	}

	// Token: 0x06009AB8 RID: 39608 RVA: 0x003A6E90 File Offset: 0x003A5090
	public void SetAntialiasing(int aaQuality, float sharpness, PostProcessLayer mainLayer)
	{
		if (aaQuality == 0)
		{
			mainLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
			this.UpscalingSetQuality(-1);
			return;
		}
		if (aaQuality <= 3)
		{
			if (aaQuality == 1)
			{
				mainLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
				mainLayer.fastApproximateAntialiasing.fastMode = false;
			}
			else if (aaQuality == 2)
			{
				mainLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
				mainLayer.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Medium;
			}
			else
			{
				mainLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
				mainLayer.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.High;
			}
		}
		else if (aaQuality == 4)
		{
			mainLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
			mainLayer.temporalAntialiasing.jitterSpread = 0.35f;
			mainLayer.temporalAntialiasing.stationaryBlending = 0.8f;
			mainLayer.temporalAntialiasing.motionBlending = 0.75f;
			mainLayer.temporalAntialiasing.sharpness = sharpness * 0.2f;
		}
		else
		{
			Log.Error(string.Format("Unsupported aaQuality value \"{0}\".", aaQuality));
		}
		this.UpscalingSetQuality(-1);
	}

	// Token: 0x06009AB9 RID: 39609 RVA: 0x003A6F68 File Offset: 0x003A5168
	public void DynamicResolutionInit()
	{
		this.DynamicResolutionDestroyRT();
		Camera playerCamera = this.player.playerCamera;
		Camera finalCamera = this.player.finalCamera;
		bool flag = finalCamera != playerCamera;
		if (!GameRenderManager.dynamicIsEnabled)
		{
			if (flag)
			{
				global::UnityEngine.Object.Destroy(finalCamera.gameObject);
			}
			this.player.finalCamera = playerCamera;
			return;
		}
		if (!flag)
		{
			this.AddFinalCameraToPlayer();
		}
		this.DynamicResolutionAllocRTs();
	}

	// Token: 0x06009ABA RID: 39610 RVA: 0x003A6FCC File Offset: 0x003A51CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddFinalCameraToPlayer()
	{
		GameObject gameObject = new GameObject("FinalCamera");
		gameObject.transform.SetParent(this.player.cameraTransform, false);
		Camera camera = gameObject.AddComponent<Camera>();
		this.player.finalCamera = camera;
		camera.clearFlags = CameraClearFlags.Nothing;
		camera.cullingMask = 0;
		camera.depth = -0.1f;
		gameObject.AddComponent<LocalPlayerFinalCamera>().entityPlayerLocal = this.player;
	}

	// Token: 0x06009ABB RID: 39611 RVA: 0x003A7038 File Offset: 0x003A5238
	public void DynamicResolutionUpdate()
	{
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if (!GameRenderManager.dynamicIsEnabled)
		{
			return;
		}
		if (Screen.width != this.dynamicScreenW)
		{
			this.DynamicResolutionAllocRTs();
			return;
		}
		if (this.dynamicScaleOverride > 0f)
		{
			return;
		}
		if (this.dynamicUpdateDelay > 0f)
		{
			this.dynamicUpdateDelay -= Time.deltaTime;
			return;
		}
		float num = Time.deltaTime + 0.001f;
		this.dynamicFPS = this.dynamicFPS * 0.5f + 0.5f / num;
		float num2 = 0.1f * num;
		if (this.dynamicFPS < this.dynamicFPSTargetMin)
		{
			this.dynamicScaleTarget -= num2;
			if (this.dynamicScaleTarget < 0.4f)
			{
				this.dynamicScaleTarget = 0.4f;
			}
		}
		else
		{
			this.dynamicScaleTarget += num2 * 0.2f;
			if (this.dynamicFPS > this.dynamicFPSTargetMax)
			{
				this.dynamicScaleTarget += num2;
			}
			if (this.dynamicScaleTarget > 1f)
			{
				this.dynamicScaleTarget = 1f;
			}
		}
		if (this.dynamicScaleTarget < 1f || this.dynamicScale >= 1f)
		{
			float num3 = this.dynamicScaleTarget - this.dynamicScale;
			if (num3 > -0.049f && num3 < 0.049f)
			{
				return;
			}
		}
		this.dynamicScale = this.dynamicScaleTarget;
		RenderTexture renderTexture = null;
		for (int i = 0; i < this.dynamicRTs.Length; i++)
		{
			renderTexture = this.dynamicRTs[i];
			float num4 = (this.dynamicScales[i] + this.dynamicScales[i + 1]) * 0.5f;
			if (this.dynamicScale >= num4)
			{
				break;
			}
		}
		if (this.dynamicRT == renderTexture)
		{
			return;
		}
		this.dynamicRT = renderTexture;
	}

	// Token: 0x06009ABC RID: 39612 RVA: 0x003A71F0 File Offset: 0x003A53F0
	public bool DynamicResolutionUpdateGraph(ref float value)
	{
		if (this.dynamicRT != null)
		{
			float num = (float)this.dynamicRT.width / (float)Screen.width;
			if (num != value)
			{
				value = num;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009ABD RID: 39613 RVA: 0x003A722C File Offset: 0x003A542C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DynamicResolutionAllocRTs()
	{
		this.DynamicResolutionDestroyRT();
		this.dynamicScreenW = Screen.width;
		int num = ((this.dynamicScaleOverride > 0f) ? 1 : 4);
		this.dynamicRTs = new RenderTexture[num];
		for (int i = 0; i < num; i++)
		{
			float num2 = this.dynamicScales[i];
			if (this.dynamicScaleOverride > 0f)
			{
				num2 = this.dynamicScaleOverride;
			}
			RenderTexture renderTexture = this.DynamicResolutionAllocRT(num2);
			this.dynamicRTs[i] = renderTexture;
		}
		this.dynamicRT = this.dynamicRTs[0];
		this.dynamicScale = 1f;
		this.dynamicScaleTarget = 1f;
		this.dynamicUpdateDelay = 18f;
	}

	// Token: 0x06009ABE RID: 39614 RVA: 0x003A72D4 File Offset: 0x003A54D4
	public RenderTexture DynamicResolutionAllocRT(float scale)
	{
		int num = (int)((float)Screen.width * scale);
		int num2 = (int)((float)Screen.height * scale);
		RenderTexture renderTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
		renderTexture.name = string.Format("DynRT{0}x{1}", num, num2);
		Log.Out("DynamicResolutionAllocRT scale {0}, Tex {1}x{2}", new object[] { scale, num, num2 });
		return renderTexture;
	}

	// Token: 0x06009ABF RID: 39615 RVA: 0x003A734C File Offset: 0x003A554C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DynamicResolutionDestroyRT()
	{
		List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
		for (int i = 0; i < localPlayers.Count; i++)
		{
			localPlayers[i].playerCamera.targetTexture = null;
		}
		if (this.dynamicRTs != null)
		{
			for (int j = 0; j < this.dynamicRTs.Length; j++)
			{
				this.dynamicRTs[j].Release();
				global::UnityEngine.Object.Destroy(this.dynamicRTs[j]);
			}
			this.dynamicRTs = null;
		}
		this.dynamicRT = null;
	}

	// Token: 0x06009AC0 RID: 39616 RVA: 0x003A73D0 File Offset: 0x003A55D0
	public void SetDynamicResolution(float scale, float fpsMin, float fpsMax)
	{
		GameRenderManager.dynamicIsEnabled = scale >= 0f;
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxVsync);
		this.dynamicFPSTargetMin = fpsMin;
		if (fpsMin < 0f)
		{
			this.dynamicFPSTargetMin = 30f;
		}
		if (@int > 0)
		{
			this.dynamicFPSTargetMin = Utils.FastMin(30f, this.dynamicFPSTargetMin);
		}
		if (@int > 1)
		{
			this.dynamicFPSTargetMin = Utils.FastMin(18f, this.dynamicFPSTargetMin);
		}
		this.dynamicFPSTargetMax = fpsMax;
		if (fpsMax < 0f)
		{
			this.dynamicFPSTargetMax = 64f;
			if (@int > 0)
			{
				this.dynamicFPSTargetMax = 55f;
			}
			if (@int > 1)
			{
				this.dynamicFPSTargetMax = 25f;
			}
		}
		this.dynamicScaleOverride = scale;
		if (this.dynamicScaleOverride > 0f)
		{
			this.dynamicScaleOverride = Mathf.Clamp(this.dynamicScaleOverride, 0.1f, 1f);
			this.dynamicScale = this.dynamicScaleOverride;
		}
		this.DynamicResolutionInit();
	}

	// Token: 0x06009AC1 RID: 39617 RVA: 0x003A74BC File Offset: 0x003A56BC
	public RenderTexture GetDynamicRenderTexture()
	{
		return this.dynamicRT;
	}

	// Token: 0x06009AC2 RID: 39618 RVA: 0x003A74C4 File Offset: 0x003A56C4
	public void DynamicResolutionRender()
	{
		Graphics.Blit(this.GetDynamicRenderTexture(), null);
	}

	// Token: 0x06009AC3 RID: 39619 RVA: 0x003A74D4 File Offset: 0x003A56D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpscalingSetQuality(int _quality)
	{
		if (_quality < 0)
		{
			if (this.upscalingEnabled)
			{
				this.upscalingEnabled = false;
				this.SetMipmapBias(0f);
			}
			return;
		}
		this.upscalingEnabled = true;
		this.mipmapTextureMem = 0UL;
		FSR3 fsr = this.layer.fsr3;
		FSR3_Quality fsr3_Quality;
		switch (_quality)
		{
		case 0:
			fsr3_Quality = FSR3_Quality.UltraPerformance;
			break;
		case 1:
			fsr3_Quality = FSR3_Quality.Performance;
			break;
		case 2:
			fsr3_Quality = FSR3_Quality.Balanced;
			break;
		case 3:
			fsr3_Quality = FSR3_Quality.Quality;
			break;
		case 4:
			fsr3_Quality = FSR3_Quality.UltraQuality;
			break;
		default:
			fsr3_Quality = FSR3_Quality.NativeAA;
			break;
		}
		fsr.qualityMode = fsr3_Quality;
		if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceVendor.ToLower().Contains("nvidia"))
		{
			this.layer.fsr3.exposureSource = FSR3.ExposureSource.Default;
		}
		DLSS dlss = this.layer.dlss;
		DLSS_Quality dlss_Quality;
		switch (_quality)
		{
		case 0:
			dlss_Quality = DLSS_Quality.UltraPerformance;
			break;
		case 1:
			dlss_Quality = DLSS_Quality.Performance;
			break;
		case 2:
			dlss_Quality = DLSS_Quality.Balanced;
			break;
		case 3:
			dlss_Quality = DLSS_Quality.Quality;
			break;
		case 4:
			dlss_Quality = DLSS_Quality.UltraQuality;
			break;
		default:
			dlss_Quality = DLSS_Quality.NativeAA;
			break;
		}
		dlss.qualityMode = dlss_Quality;
	}

	// Token: 0x06009AC4 RID: 39620 RVA: 0x003A75CC File Offset: 0x003A57CC
	public void UpscalingPreCull()
	{
		if (!this.upscalingEnabled)
		{
			return;
		}
		switch (this.layer.antialiasingMode)
		{
		case PostProcessLayer.Antialiasing.FSR1:
			this.UpdateMipmaps((float)this.layer.fsr1.renderSize.x / (float)Screen.width, 1f);
			return;
		case PostProcessLayer.Antialiasing.FSR3:
			this.UpdateMipmaps((float)this.layer.fsr3.renderSize.x / (float)Screen.width, 0.3f);
			return;
		case PostProcessLayer.Antialiasing.DLSS:
			this.UpdateMipmaps((float)this.layer.dlss.renderSize.x / (float)Screen.width, 1f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06009AC5 RID: 39621 RVA: 0x003A7680 File Offset: 0x003A5880
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateMipmaps(float _renderToScreenRatio, float biasStrength = 1f)
	{
		this.mipmapDelay -= Time.deltaTime;
		if (this.mipmapDelay <= 0f)
		{
			this.mipmapDelay = 2f;
			ulong currentTextureMemory = Texture.currentTextureMemory;
			if (this.mipmapTextureMem != currentTextureMemory)
			{
				this.mipmapTextureMem = currentTextureMemory;
				float num = biasStrength * (Mathf.Log(_renderToScreenRatio, 2f) - 1f);
				this.SetMipmapBias(num);
			}
		}
	}

	// Token: 0x06009AC6 RID: 39622 RVA: 0x003A76EC File Offset: 0x003A58EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMipmapBias(float _bias)
	{
		Texture2D[] array = Resources.FindObjectsOfTypeAll(typeof(Texture2D)) as Texture2D[];
		for (int i = 0; i < array.Length; i++)
		{
			array[i].mipMapBias = _bias;
		}
		Texture2DArray[] array2 = Resources.FindObjectsOfTypeAll(typeof(Texture2DArray)) as Texture2DArray[];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].mipMapBias = _bias;
		}
	}

	// Token: 0x06009AC7 RID: 39623 RVA: 0x003A7751 File Offset: 0x003A5951
	public void OnGUI()
	{
		this.graphManager.Draw();
	}

	// Token: 0x06009AC8 RID: 39624 RVA: 0x003A775E File Offset: 0x003A595E
	public bool FPSUpdateGraph(ref float value)
	{
		value = 1f / (Time.deltaTime + 0.001f);
		return true;
	}

	// Token: 0x06009AC9 RID: 39625 RVA: 0x003A7774 File Offset: 0x003A5974
	public bool SPFUpdateGraph(ref float value)
	{
		value = (Time.deltaTime + 0.0001f) * 1000f;
		return true;
	}

	// Token: 0x04007489 RID: 29833
	public GameGraphManager graphManager;

	// Token: 0x0400748A RID: 29834
	public GameLightManager lightManager;

	// Token: 0x0400748B RID: 29835
	public ReflectionManager reflectionManager;

	// Token: 0x0400748C RID: 29836
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400748D RID: 29837
	[PublicizedFrom(EAccessModifier.Private)]
	public PostProcessLayer layer;

	// Token: 0x0400748E RID: 29838
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicUpdateDelay = 18f;

	// Token: 0x0400748F RID: 29839
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicChangeSeconds = 5f;

	// Token: 0x04007490 RID: 29840
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicFPSMin = 30f;

	// Token: 0x04007491 RID: 29841
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicFPSMax = 64f;

	// Token: 0x04007492 RID: 29842
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicFPSVSyncMin = 30f;

	// Token: 0x04007493 RID: 29843
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicFPSVSyncMax = 55f;

	// Token: 0x04007494 RID: 29844
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicScaleMin = 0.4f;

	// Token: 0x04007495 RID: 29845
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDynamicScaleThreshold = 0.049f;

	// Token: 0x04007496 RID: 29846
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDynamicRTCount = 4;

	// Token: 0x04007497 RID: 29847
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly float[] dynamicScales = new float[] { 1f, 0.75f, 0.62f, 0.5f, 0f };

	// Token: 0x04007498 RID: 29848
	public static bool dynamicIsEnabled;

	// Token: 0x04007499 RID: 29849
	[PublicizedFrom(EAccessModifier.Private)]
	public float dynamicUpdateDelay;

	// Token: 0x0400749A RID: 29850
	[PublicizedFrom(EAccessModifier.Private)]
	public float dynamicFPSTargetMin;

	// Token: 0x0400749B RID: 29851
	[PublicizedFrom(EAccessModifier.Private)]
	public float dynamicFPSTargetMax = 64f;

	// Token: 0x0400749C RID: 29852
	[PublicizedFrom(EAccessModifier.Private)]
	public float dynamicFPS = 60f;

	// Token: 0x0400749D RID: 29853
	[PublicizedFrom(EAccessModifier.Private)]
	public float dynamicScale;

	// Token: 0x0400749E RID: 29854
	[PublicizedFrom(EAccessModifier.Private)]
	public float dynamicScaleTarget;

	// Token: 0x0400749F RID: 29855
	[PublicizedFrom(EAccessModifier.Private)]
	public float dynamicScaleOverride;

	// Token: 0x040074A0 RID: 29856
	[PublicizedFrom(EAccessModifier.Private)]
	public int dynamicScreenW;

	// Token: 0x040074A1 RID: 29857
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture dynamicRT;

	// Token: 0x040074A2 RID: 29858
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture[] dynamicRTs;

	// Token: 0x040074A3 RID: 29859
	[PublicizedFrom(EAccessModifier.Private)]
	public bool upscalingEnabled;

	// Token: 0x040074A4 RID: 29860
	[PublicizedFrom(EAccessModifier.Private)]
	public float mipmapDelay;

	// Token: 0x040074A5 RID: 29861
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong mipmapTextureMem;
}
