using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020011FF RID: 4607
public static class GameOptionsManager
{
	// Token: 0x140000F3 RID: 243
	// (add) Token: 0x0600930E RID: 37646 RVA: 0x00377754 File Offset: 0x00375954
	// (remove) Token: 0x0600930F RID: 37647 RVA: 0x00377788 File Offset: 0x00375988
	public static event Action<int, int> ResolutionChanged;

	// Token: 0x140000F4 RID: 244
	// (add) Token: 0x06009310 RID: 37648 RVA: 0x003777BC File Offset: 0x003759BC
	// (remove) Token: 0x06009311 RID: 37649 RVA: 0x003777F0 File Offset: 0x003759F0
	public static event Action<int> TextureQualityChanged;

	// Token: 0x140000F5 RID: 245
	// (add) Token: 0x06009312 RID: 37650 RVA: 0x00377824 File Offset: 0x00375A24
	// (remove) Token: 0x06009313 RID: 37651 RVA: 0x00377858 File Offset: 0x00375A58
	public static event Action<int> TextureFilterChanged;

	// Token: 0x140000F6 RID: 246
	// (add) Token: 0x06009314 RID: 37652 RVA: 0x0037788C File Offset: 0x00375A8C
	// (remove) Token: 0x06009315 RID: 37653 RVA: 0x003778C0 File Offset: 0x00375AC0
	public static event Action<int> ShadowDistanceChanged;

	// Token: 0x140000F7 RID: 247
	// (add) Token: 0x06009316 RID: 37654 RVA: 0x003778F4 File Offset: 0x00375AF4
	// (remove) Token: 0x06009317 RID: 37655 RVA: 0x00377928 File Offset: 0x00375B28
	public static event Action OnGameOptionsApplied;

	// Token: 0x06009318 RID: 37656 RVA: 0x0037795C File Offset: 0x00375B5C
	[PublicizedFrom(EAccessModifier.Private)]
	static GameOptionsManager()
	{
		GameOptionsManager.initQualityPresets();
	}

	// Token: 0x06009319 RID: 37657 RVA: 0x00377A37 File Offset: 0x00375C37
	public static void ValidateGamePrefs()
	{
		GamePrefs.OnGamePrefChanged += GameOptionsManager.OnGamePrefChanged;
		GameOptionsManager.ValidateFoV();
		GameOptionsManager.ValidateFoV3P();
		GameOptionsManager.ValidateTreeDistance();
		GameOptionsManager.ValidateHudSize();
		GameOptionsManager.ValidateShadowDistance();
	}

	// Token: 0x0600931A RID: 37658 RVA: 0x00377A64 File Offset: 0x00375C64
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref <= EnumGamePrefs.OptionsHudSize)
		{
			if (_pref <= EnumGamePrefs.OptionsGfxShadowDistance)
			{
				if (_pref == EnumGamePrefs.OptionsOverallAudioVolumeLevel)
				{
					AudioListener.volume = GamePrefs.GetFloat(EnumGamePrefs.OptionsOverallAudioVolumeLevel);
					return;
				}
				if (_pref != EnumGamePrefs.OptionsGfxShadowDistance)
				{
					return;
				}
				GameOptionsManager.ValidateShadowDistance();
				return;
			}
			else
			{
				if (_pref == EnumGamePrefs.OptionsGfxFOV)
				{
					GameOptionsManager.ValidateFoV();
					return;
				}
				if (_pref != EnumGamePrefs.OptionsHudSize)
				{
					return;
				}
				GameOptionsManager.ValidateHudSize();
				return;
			}
		}
		else if (_pref <= EnumGamePrefs.OptionsGfxTreeDistance)
		{
			if (_pref == EnumGamePrefs.OptionsGfxWaterPtlLimiter)
			{
				WaterSplashCubes.particleLimiter = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxWaterPtlLimiter);
				return;
			}
			if (_pref != EnumGamePrefs.OptionsGfxTreeDistance)
			{
				return;
			}
			GameOptionsManager.ValidateTreeDistance();
			return;
		}
		else
		{
			if (_pref == EnumGamePrefs.OptionsMumblePositionalAudioSupport)
			{
				GameOptionsManager.UpdateMumblePositionalAudioState();
				return;
			}
			if (_pref != EnumGamePrefs.OptionsGfxFOV3P)
			{
				return;
			}
			GameOptionsManager.ValidateFoV3P();
			return;
		}
	}

	// Token: 0x0600931B RID: 37659 RVA: 0x00377B00 File Offset: 0x00375D00
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateFoV()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxFOV);
		if (@int < Constants.cMinCameraFieldOfView)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxFOV, Constants.cMinCameraFieldOfView);
			return;
		}
		if (@int > Constants.cMaxCameraFieldOfView)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxFOV, Constants.cMaxCameraFieldOfView);
		}
	}

	// Token: 0x0600931C RID: 37660 RVA: 0x00377B40 File Offset: 0x00375D40
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateFoV3P()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxFOV3P);
		if (@int < Constants.cMinCameraFieldOfView)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxFOV3P, Constants.cMinCameraFieldOfView);
			return;
		}
		if (@int > Constants.cMaxCameraFieldOfView)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxFOV3P, Constants.cMaxCameraFieldOfView);
		}
	}

	// Token: 0x0600931D RID: 37661 RVA: 0x00377B87 File Offset: 0x00375D87
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateTreeDistance()
	{
		if (GamePrefs.GetInt(EnumGamePrefs.OptionsGfxTreeDistance) < 2)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxTreeDistance, 2);
		}
	}

	// Token: 0x0600931E RID: 37662 RVA: 0x00377BA4 File Offset: 0x00375DA4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateHudSize()
	{
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsHudSize);
		if ((double)@float < 0.01)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsHudSize, 1f);
			return;
		}
		if ((double)@float < 0.5)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsHudSize, 0.5f);
		}
	}

	// Token: 0x0600931F RID: 37663 RVA: 0x00377BF5 File Offset: 0x00375DF5
	[PublicizedFrom(EAccessModifier.Private)]
	public static void UpdateMumblePositionalAudioState()
	{
		if (GamePrefs.GetBool(EnumGamePrefs.OptionsMumblePositionalAudioSupport))
		{
			MumblePositionalAudio.Init();
			return;
		}
		MumblePositionalAudio.Destroy();
	}

	// Token: 0x06009320 RID: 37664 RVA: 0x00377C10 File Offset: 0x00375E10
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ValidateShadowDistance()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowDistance);
		if (@int >= 5 && @int < 20)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxShadowDistance, 20);
		}
	}

	// Token: 0x06009321 RID: 37665 RVA: 0x00377C38 File Offset: 0x00375E38
	public static void ApplyAllOptions(LocalPlayerUI _playerUi)
	{
		QualitySettings.antiAliasing = 0;
		float streamingMipmapBudget = GameOptionsPlatforms.GetStreamingMipmapBudget();
		QualitySettings.streamingMipmapsMemoryBudget = streamingMipmapBudget;
		Log.Out("ApplyAllOptions streaming budget {0} MB", new object[] { streamingMipmapBudget });
		QualitySettings.softParticles = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxWaterPtlLimiter) >= 0.51f;
		GameOptionsManager.ApplyScreenResolution();
		AudioListener.volume = Math.Min(GamePrefs.GetFloat(EnumGamePrefs.OptionsOverallAudioVolumeLevel), 1f);
		GameOptionsManager.ApplyShadowQuality();
		GameOptionsManager.ApplyTextureQuality(-1);
		GameOptionsManager.ApplyTextureFilter();
		GameOptionsManager.ApplyCameraOptions(null);
		Shader.globalMaximumLOD = 400 + GamePrefs.GetInt(EnumGamePrefs.OptionsGfxObjQuality) * 100;
		QualitySettings.lodBias = GameOptionsManager.GetLODBias();
		GameOptionsManager.ApplyTerrainOptions();
		MeshDescription.SetGrassQuality();
		MeshDescription.SetWaterQuality();
		Action onGameOptionsApplied = GameOptionsManager.OnGameOptionsApplied;
		if (onGameOptionsApplied == null)
		{
			return;
		}
		onGameOptionsApplied();
	}

	// Token: 0x06009322 RID: 37666 RVA: 0x00377CF8 File Offset: 0x00375EF8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplyScreenResolution()
	{
		ValueTuple<int, int, FullScreenMode> screenOptions = PlatformApplicationManager.Application.ScreenOptions;
		int item = screenOptions.Item1;
		int item2 = screenOptions.Item2;
		FullScreenMode item3 = screenOptions.Item3;
		Resolution currentResolution = PlatformApplicationManager.Application.GetCurrentResolution();
		Log.Out("ApplyAllOptions current screen {0} x {1}, {2}hz, window {3} x {4}, mode {5}", new object[]
		{
			currentResolution.width,
			currentResolution.height,
			currentResolution.refreshRateRatio.value,
			Screen.width,
			Screen.height,
			Screen.fullScreenMode
		});
		GameOptionsManager.SetResolution(item, item2, item3);
	}

	// Token: 0x06009323 RID: 37667 RVA: 0x00377DA4 File Offset: 0x00375FA4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ApplyShadowQuality()
	{
		Vector3 vector = new Vector3(0.06f, 0.15f, 0.35f);
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowDistance);
		switch (@int)
		{
		case 0:
			QualitySettings.shadowDistance = 35f;
			QualitySettings.shadowCascades = 2;
			QualitySettings.shadowCascade2Split = 0.33f;
			goto IL_00C8;
		case 1:
			QualitySettings.shadowDistance = 80f;
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowCascade4Split = vector;
			goto IL_00C8;
		case 2:
			QualitySettings.shadowDistance = 120f;
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowCascade4Split = vector;
			goto IL_00C8;
		case 3:
			QualitySettings.shadowDistance = 200f;
			QualitySettings.shadowCascades = 4;
			QualitySettings.shadowCascade4Split = vector * 0.8f;
			goto IL_00C8;
		}
		QualitySettings.shadowDistance = 300f;
		QualitySettings.shadowCascades = 4;
		QualitySettings.shadowCascade4Split = vector * 0.6f;
		IL_00C8:
		if (GameOptionsManager.ShadowDistanceChanged != null)
		{
			GameOptionsManager.ShadowDistanceChanged(@int);
		}
		switch (GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowQuality))
		{
		case 0:
			QualitySettings.shadows = ShadowQuality.Disable;
			return;
		case 1:
			QualitySettings.shadows = ShadowQuality.HardOnly;
			QualitySettings.shadowResolution = ShadowResolution.Medium;
			return;
		case 2:
			QualitySettings.shadows = ShadowQuality.All;
			QualitySettings.shadowResolution = ShadowResolution.Medium;
			return;
		case 3:
			QualitySettings.shadows = ShadowQuality.All;
			QualitySettings.shadowResolution = ShadowResolution.High;
			return;
		case 4:
			QualitySettings.shadows = ShadowQuality.All;
			QualitySettings.shadowResolution = ShadowResolution.High;
			return;
		case 5:
			QualitySettings.shadows = ShadowQuality.All;
			QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
			return;
		default:
			return;
		}
	}

	// Token: 0x06009324 RID: 37668 RVA: 0x00377EFC File Offset: 0x003760FC
	public static float GetLODBias()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxObjQuality);
		switch (@int)
		{
		case 0:
			return 0.5f;
		case 1:
			return 0.65f;
		case 2:
			return 0.8f;
		case 3:
			return 1.2f;
		case 4:
			return 1.7f;
		default:
			return (float)@int / 100f;
		}
	}

	// Token: 0x06009325 RID: 37669 RVA: 0x00377F54 File Offset: 0x00376154
	public static void CheckResolution()
	{
		if (GameOptionsManager.screenExclusiveCheckDelay > 0 && --GameOptionsManager.screenExclusiveCheckDelay == 0)
		{
			GameOptionsManager.screenExclusiveCheckDelay = 10;
			if (Screen.width != GameOptionsManager.screenWidth || Screen.height != GameOptionsManager.screenHeight)
			{
				Log.Warning("Fullscreen Exclusive failed! Reverting to {0} x {1}", new object[]
				{
					GameOptionsManager.screenWidth,
					GameOptionsManager.screenHeight
				});
				GameOptionsManager.SetResolution(GameOptionsManager.screenWidth, GameOptionsManager.screenHeight, FullScreenMode.Windowed);
			}
		}
	}

	// Token: 0x06009326 RID: 37670 RVA: 0x00377FD4 File Offset: 0x003761D4
	public static void SetResolution(int _width, int _height, FullScreenMode _fullscreen = FullScreenMode.Windowed)
	{
		if (Screen.width == _width && Screen.height == _height && Screen.fullScreenMode == _fullscreen)
		{
			return;
		}
		Resolution currentResolution = PlatformApplicationManager.Application.GetCurrentResolution();
		Log.Out("SetResolution was screen {0} x {1}, {2}hz, window {3} x {4}, mode {5}", new object[]
		{
			currentResolution.width,
			currentResolution.height,
			currentResolution.refreshRateRatio.value,
			Screen.width,
			Screen.height,
			Screen.fullScreenMode
		});
		Log.Out("SetResolution to {0} x {1}, mode {2}", new object[] { _width, _height, _fullscreen });
		GameOptionsManager.screenWidth = _width;
		GameOptionsManager.screenHeight = _height;
		GameOptionsManager.screenExclusiveCheckDelay = ((_fullscreen == FullScreenMode.ExclusiveFullScreen) ? 10 : 0);
		PlatformApplicationManager.Application.SetResolution(_width, _height, _fullscreen);
		if (GameOptionsManager.ResolutionChanged != null)
		{
			GameOptionsManager.ResolutionChanged(_width, _height);
		}
	}

	// Token: 0x06009327 RID: 37671 RVA: 0x003780D8 File Offset: 0x003762D8
	public static int GetTextureQuality(int _overrideValue = -1)
	{
		if (_overrideValue != -1)
		{
			return _overrideValue;
		}
		int num = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxTexQuality);
		if (Constants.Is32BitOs && num < 2)
		{
			num = 2;
		}
		return Utils.FastMax(num, GameOptionsManager.CalcTextureQualityMin());
	}

	// Token: 0x06009328 RID: 37672 RVA: 0x0037810D File Offset: 0x0037630D
	public static int CalcTextureQualityMin()
	{
		return GameOptionsPlatforms.CalcTextureQualityMin();
	}

	// Token: 0x06009329 RID: 37673 RVA: 0x00378114 File Offset: 0x00376314
	public static void ApplyTextureQuality(int _overrideValue = -1)
	{
		int textureQuality = GameOptionsManager.GetTextureQuality(_overrideValue);
		QualitySettings.streamingMipmapsActive = true;
		QualitySettings.streamingMipmapsMaxLevelReduction = Math.Max(3, GameRenderManager.TextureMipmapLimit);
		GameRenderManager.TextureMipmapLimit = textureQuality;
		float num = 0.6776996f;
		if (textureQuality > 0)
		{
			if (textureQuality == 1)
			{
				num = 0.6f;
			}
			else if (textureQuality == 2)
			{
				num = 0.5f;
			}
			else if (textureQuality == 3)
			{
				num = 0.4f;
			}
		}
		Shader.SetGlobalFloat("_MipSlope", num);
		if (GameOptionsManager.TextureQualityChanged != null)
		{
			GameOptionsManager.TextureQualityChanged(textureQuality);
		}
		Log.Out("Texture quality is set to " + GameRenderManager.TextureMipmapLimit.ToString());
	}

	// Token: 0x0600932A RID: 37674 RVA: 0x003781A9 File Offset: 0x003763A9
	public static int GetTextureFilter()
	{
		return GameOptionsPlatforms.ApplyTextureFilterLimit(GamePrefs.GetInt(EnumGamePrefs.OptionsGfxTexFilter));
	}

	// Token: 0x0600932B RID: 37675 RVA: 0x003781BC File Offset: 0x003763BC
	public static void ApplyTextureFilter()
	{
		int textureFilter = GameOptionsManager.GetTextureFilter();
		QualitySettings.anisotropicFiltering = ((textureFilter == 0) ? AnisotropicFiltering.Disable : ((textureFilter <= 3) ? AnisotropicFiltering.Enable : AnisotropicFiltering.ForceEnable));
		if (GameOptionsManager.TextureFilterChanged != null)
		{
			GameOptionsManager.TextureFilterChanged(textureFilter);
		}
		Log.Out("ApplyTextureFilter {0}, AF {1}", new object[]
		{
			textureFilter,
			QualitySettings.anisotropicFiltering
		});
	}

	// Token: 0x0600932C RID: 37676 RVA: 0x0037821C File Offset: 0x0037641C
	public static void ApplyCameraOptions(EntityPlayerLocal _playerLocal = null)
	{
		bool flag = false;
		GameRenderManager.ApplyCameraOptions(_playerLocal);
		Camera[] array;
		if (_playerLocal)
		{
			array = _playerLocal.GetComponentsInChildren<Camera>();
		}
		else
		{
			array = Camera.allCameras;
		}
		foreach (Camera camera in array)
		{
			if ((camera.cullingMask & 4096) == 0)
			{
				DepthOfField depthOfField;
				if (camera.TryGetComponent<DepthOfField>(out depthOfField))
				{
					depthOfField.enabled = flag;
				}
				camera.allowHDR = true;
				int num = (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) ? GamePrefs.GetInt(EnumGamePrefs.OptionsGfxFOV) : Constants.cDefaultCameraFieldOfView);
				camera.fieldOfView = (float)num;
				camera.renderingPath = RenderingPath.DeferredShading;
				if (QualitySettings.antiAliasing != 0 && camera.actualRenderingPath == RenderingPath.DeferredShading)
				{
					Log.Warning("QualitySettings antialiasing has been enabled but the rendering path is set to deferred. This is incompatible and wastes memory and so will be disabled");
					QualitySettings.antiAliasing = 0;
				}
				camera.farClipPlane = 2000f;
			}
		}
	}

	// Token: 0x0600932D RID: 37677 RVA: 0x003782DC File Offset: 0x003764DC
	public static void ApplyTerrainOptions()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxTerrainQuality);
		if (@int <= 1)
		{
			Shader.EnableKeyword("GAME_TERRAINLOWQ");
		}
		else
		{
			Shader.DisableKeyword("GAME_TERRAINLOWQ");
		}
		if (@int == 0)
		{
			Shader.DisableKeyword("_MAX3LAYER");
			Shader.EnableKeyword("_MAX2LAYER");
		}
		else if (@int <= 1)
		{
			Shader.DisableKeyword("_MAX2LAYER");
			Shader.EnableKeyword("_MAX3LAYER");
		}
		else
		{
			Shader.DisableKeyword("_MAX2LAYER");
			Shader.DisableKeyword("_MAX3LAYER");
		}
		Log.Out("ApplyTerrainOptions {0}", new object[] { @int });
	}

	// Token: 0x0600932E RID: 37678 RVA: 0x00378370 File Offset: 0x00376570
	[PublicizedFrom(EAccessModifier.Private)]
	public static void initQualityPresets()
	{
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxAA] = new List<object> { 0, 1, 2, 4, 4, null, 3, null, 3 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxAASharpness] = new List<object> { 0f, 0f, 0f, 0.5f, 0.6f, null, 0.85f, null, 0.85f };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxMotionBlur] = new List<object> { 0, 0, 1, 1, 2, null, 2, null, 2 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxTexQuality] = new List<object>
		{
			3,
			2,
			1,
			0,
			0,
			null,
			DeviceFlag.XBoxSeriesS.IsCurrent() ? 1 : 0,
			null,
			DeviceFlag.XBoxSeriesS.IsCurrent() ? 1 : 0
		};
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxTexFilter] = new List<object> { 0, 0, 1, 2, 3, null, 3, null, 3 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxReflectQuality] = new List<object> { 0, 0, 1, 2, 3, null, 3, null, 3 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxReflectShadows] = new List<object> { false, false, false, false, true, null, true, null, true };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxShadowDistance] = new List<object> { 0, 0, 1, 2, 3, null, 1, null, 3 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxShadowQuality] = new List<object>
		{
			0,
			1,
			2,
			3,
			4,
			null,
			2,
			null,
			DeviceFlag.XBoxSeriesS.IsCurrent() ? 2 : 3
		};
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxLODDistance] = new List<object> { 0f, 0.25f, 0.5f, 0.75f, 1f, null, 0.75f, null, 0.75f };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxTerrainQuality] = new List<object> { 0, 1, 2, 3, 4, null, 2, null, 4 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxObjQuality] = new List<object>
		{
			0,
			1,
			2,
			3,
			4,
			null,
			2,
			null,
			DeviceFlag.XBoxSeriesS.IsCurrent() ? 3 : 4
		};
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxGrassDistance] = new List<object> { 0, 1, 2, 3, 3, null, 2, null, 3 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxBloom] = new List<object> { false, false, true, true, true, null, true, null, true };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxDOF] = new List<object> { false, false, false, false, false, null, false, null, false };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxSSAO] = new List<object> { false, false, true, true, true, null, true, null, true };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxSSReflections] = new List<object> { 0, 1, 1, 2, 3, null, 1, null, 1 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxSunShafts] = new List<object> { false, false, true, true, true, null, true, null, true };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxWaterQuality] = new List<object> { 0, 0, 1, 1, 1, null, 1, null, 1 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxWaterPtlLimiter] = new List<object> { 0f, 0.2f, 0.5f, 0.75f, 1f, null, 0.75f, null, 0.75f };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxSignQuality] = new List<object> { 0, 1, 2, 3, 4, null, 2, null, 2 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxOcclusion] = new List<object> { false, true, true, true, true, null, true, null, true };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxViewDistance] = new List<object> { 5, 5, 6, 6, 7, null, 6, null, 6 };
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxDynamicScale] = new List<object>
		{
			null,
			null,
			null,
			null,
			null,
			null,
			DeviceFlag.XBoxSeriesS.IsCurrent() ? 0.6f : 0.5f,
			null,
			0.75f
		};
		GameOptionsManager.QualityPresets[EnumGamePrefs.OptionsGfxFSRPreset] = new List<object> { null, null, null, null, null, null, 0, null, 2 };
	}

	// Token: 0x0600932F RID: 37679 RVA: 0x00378F44 File Offset: 0x00377144
	public static void SetGraphicsQuality()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxQualityPreset);
		if (@int < 0)
		{
			Log.Warning(string.Format("SetGraphicsQuality: Selected preset is negative ({0})", @int));
			return;
		}
		foreach (KeyValuePair<EnumGamePrefs, List<object>> keyValuePair in GameOptionsManager.QualityPresets)
		{
			EnumGamePrefs enumGamePrefs;
			List<object> list;
			keyValuePair.Deconstruct(out enumGamePrefs, out list);
			EnumGamePrefs enumGamePrefs2 = enumGamePrefs;
			List<object> list2 = list;
			if (@int >= list2.Count)
			{
				Log.Warning(string.Format("SetGraphicsQuality: Skipping GamePref {0}, selected preset ({1}) outside of defined values ({2})", enumGamePrefs2.ToStringCached<EnumGamePrefs>(), @int, list2.Count));
			}
			else
			{
				object obj = list2[@int];
				if (obj == null)
				{
					Log.Warning(string.Format("SetGraphicsQuality: Skipping GamePref {0}, selected preset ({1}) does not have a value defined for this setting", enumGamePrefs2.ToStringCached<EnumGamePrefs>(), @int));
				}
				else
				{
					GamePrefs.SetObject(enumGamePrefs2, obj);
				}
			}
		}
	}

	// Token: 0x06009330 RID: 37680 RVA: 0x0037902C File Offset: 0x0037722C
	public static double GetUiSizeLimit(double _aspectRation)
	{
		int num = 0;
		while (_aspectRation > GameOptionsManager.uiScaleLimits[num].Item1)
		{
			num++;
		}
		return GameOptionsManager.uiScaleLimits[num].Item2;
	}

	// Token: 0x06009331 RID: 37681 RVA: 0x00379064 File Offset: 0x00377264
	public static double GetUiSizeLimit()
	{
		Vector2i currentScreenSize = GameOptionsManager.CurrentScreenSize;
		return GameOptionsManager.GetUiSizeLimit((double)currentScreenSize.x / (double)currentScreenSize.y);
	}

	// Token: 0x06009332 RID: 37682 RVA: 0x0037908C File Offset: 0x0037728C
	public static float GetActiveUiScale()
	{
		float num = (float)GameOptionsManager.GetUiSizeLimit();
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsHudSize);
		return Utils.FastMin(num, @float);
	}

	// Token: 0x170011C1 RID: 4545
	// (get) Token: 0x06009333 RID: 37683 RVA: 0x003790B0 File Offset: 0x003772B0
	public static Vector2i CurrentScreenSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return new Vector2i(Screen.width, Screen.height);
		}
	}

	// Token: 0x04006C85 RID: 27781
	[PublicizedFrom(EAccessModifier.Private)]
	public static int screenWidth;

	// Token: 0x04006C86 RID: 27782
	[PublicizedFrom(EAccessModifier.Private)]
	public static int screenHeight;

	// Token: 0x04006C87 RID: 27783
	[PublicizedFrom(EAccessModifier.Private)]
	public static int screenExclusiveCheckDelay;

	// Token: 0x04006C88 RID: 27784
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cScreenExclusiveFrameWait = 10;

	// Token: 0x04006C8A RID: 27786
	public static readonly EnumDictionary<EnumGamePrefs, List<object>> QualityPresets = new EnumDictionary<EnumGamePrefs, List<object>>();

	// Token: 0x04006C8B RID: 27787
	[TupleElementNames(new string[] { "aspectLimit", "scaleLimit" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ValueTuple<double, double>[] uiScaleLimits = new ValueTuple<double, double>[]
	{
		new ValueTuple<double, double>(1.2, 0.65),
		new ValueTuple<double, double>(1.26, 0.7),
		new ValueTuple<double, double>(1.34, 0.75),
		new ValueTuple<double, double>(1.51, 0.85),
		new ValueTuple<double, double>(1.61, 0.9),
		new ValueTuple<double, double>(1000.0, 1.0)
	};
}
