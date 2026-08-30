using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Scripting;

// Token: 0x02000DA4 RID: 3492
[Preserve]
public class XUiC_OptionsVideo : XUiC_OptionsVideoBase
{
	// Token: 0x06006AE1 RID: 27361 RVA: 0x0029D4A4 File Offset: 0x0029B6A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void initResolutionOptions()
	{
		if (this.optionResolution == null || this.comboResolution == null || this.optionFullscreen == null || this.comboFullscreen == null)
		{
			return;
		}
		this.optionFullscreen.GetSettingValue = delegate
		{
			FullScreenMode item = PlatformApplicationManager.Application.ScreenOptions.Item3;
			this.comboFullscreen.SelectedIndex = XUiC_OptionsVideo.<initResolutionOptions>g__ConvertFullScreenModeToIndex|6_9(item);
		};
		this.optionFullscreen.DiscardChanges = delegate
		{
			FullScreenMode item2 = PlatformApplicationManager.Application.ScreenOptions.Item3;
			this.comboFullscreen.SelectedIndex = XUiC_OptionsVideo.<initResolutionOptions>g__ConvertFullScreenModeToIndex|6_9(item2);
		};
		this.optionFullscreen.ApplyChanges = delegate(bool _)
		{
		};
		this.optionFullscreen.IsChangedDelegate = delegate
		{
			FullScreenMode item3 = PlatformApplicationManager.Application.ScreenOptions.Item3;
			return this.comboFullscreen.SelectedIndex != XUiC_OptionsVideo.<initResolutionOptions>g__ConvertFullScreenModeToIndex|6_9(item3);
		};
		this.optionResolution.GetSettingValue = delegate
		{
			this.comboResolution.Elements.Clear();
			foreach (Resolution resolution in PlatformApplicationManager.Application.SupportedResolutions)
			{
				XUiC_OptionsVideo.ResolutionInfo resolutionInfo = new XUiC_OptionsVideo.ResolutionInfo(resolution.width, resolution.height);
				if (!this.comboResolution.Elements.Contains(resolutionInfo))
				{
					this.comboResolution.Elements.Add(resolutionInfo);
				}
			}
			this.<initResolutionOptions>g__GetCurrentResolution|6_8();
		};
		this.optionResolution.DiscardChanges = delegate
		{
			this.comboResolution.SelectedIndex = this.comboResolution.Elements.IndexOf(this.lastResolution);
		};
		this.optionResolution.ApplyChanges = delegate(bool _)
		{
			XUiC_OptionsVideo.ResolutionInfo value = this.comboResolution.Value;
			GameOptionsManager.SetResolution(value.Width, value.Height, XUiC_OptionsVideo.<initResolutionOptions>g__ConvertIndexToFullScreenMode|6_10(this.comboFullscreen.SelectedIndex));
			this.<initResolutionOptions>g__GetCurrentResolution|6_8();
		};
		this.optionResolution.IsChangedDelegate = () => !this.comboResolution.Value.Equals(this.lastResolution);
	}

	// Token: 0x17000BA9 RID: 2985
	// (get) Token: 0x06006AE2 RID: 27362 RVA: 0x0029D59D File Offset: 0x0029B79D
	public static bool Fsr3Supported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return FSR3.FSR3Supported();
		}
	}

	// Token: 0x17000BAA RID: 2986
	// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x0029D5A4 File Offset: 0x0029B7A4
	public static bool DlssSupported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsVideo.Fsr3Supported && DLSS.DLSSSupported();
		}
	}

	// Token: 0x17000BAB RID: 2987
	// (get) Token: 0x06006AE4 RID: 27364 RVA: 0x0029D5B4 File Offset: 0x0029B7B4
	[XuiXmlBinding("upscaler_mode")]
	public XUiC_OptionsVideo.UpscalerMode SelectedUpscalerMode
	{
		get
		{
			XUiC_ComboBoxList<string> xuiC_ComboBoxList = this.comboUpscaler;
			if (xuiC_ComboBoxList == null)
			{
				return XUiC_OptionsVideo.UpscalerMode.Off;
			}
			return (XUiC_OptionsVideo.UpscalerMode)xuiC_ComboBoxList.SelectedIndex;
		}
	}

	// Token: 0x06006AE5 RID: 27365 RVA: 0x0029D5C8 File Offset: 0x0029B7C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void initUpscalerModeOptions()
	{
		if (this.optionUpscaler == null || this.comboUpscaler == null)
		{
			return;
		}
		this.optionUpscaler.GetSettingValue = delegate
		{
			if (!XUiC_OptionsVideo.DlssSupported)
			{
				this.comboUpscaler.MaxIndex = 3;
			}
			if (!XUiC_OptionsVideo.Fsr3Supported)
			{
				this.comboUpscaler.MaxIndex = 2;
			}
			int num = XUiC_OptionsVideo.<initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|16_4();
			if (num == -1)
			{
				if (XUiC_OptionsVideo.Fsr3Supported)
				{
					Log.Out(string.Format("Upscaler mode \"{0}\" is unsupported on this platform; defaulting to \"{1}\".", GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode), 2));
					num = 3;
					GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, 2);
				}
				else
				{
					Log.Out(string.Format("Upscaler mode \"{0}\" is unsupported on this platform; defaulting to \"{1}\".", GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode), 4));
					num = 2;
					GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, 4);
				}
			}
			this.comboUpscaler.SelectedIndex = num;
		};
		this.optionUpscaler.DiscardChanges = delegate
		{
			this.comboUpscaler.SelectedIndex = XUiC_OptionsVideo.<initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|16_4();
		};
		this.optionUpscaler.ApplyChanges = delegate(bool _)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, this.<initUpscalerModeOptions>g__SelectionToUpscalerMode|16_5());
		};
		this.optionUpscaler.IsChangedDelegate = () => this.comboUpscaler.SelectedIndex != XUiC_OptionsVideo.<initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|16_4();
	}

	// Token: 0x06006AE6 RID: 27366 RVA: 0x0029D644 File Offset: 0x0029B844
	[PublicizedFrom(EAccessModifier.Private)]
	public void initQualityPresetBasedOptions()
	{
		this.optionQualityPreset.MapGamePrefToListIndex = delegate(int _prefValue)
		{
			switch (_prefValue)
			{
			case 0:
				return 0;
			case 1:
				return 1;
			case 2:
				return 2;
			case 3:
				return 3;
			case 4:
				return 4;
			case 6:
				return 6;
			case 8:
				return 7;
			}
			return 5;
		};
		this.optionQualityPreset.MapListIndexToGamePref = delegate(int _index)
		{
			switch (_index)
			{
			case 0:
				return 0;
			case 1:
				return 1;
			case 2:
				return 2;
			case 3:
				return 3;
			case 4:
				return 4;
			case 6:
				return 6;
			case 7:
				return 8;
			}
			return 5;
		};
		this.optionViewDistance.MapGamePrefToListIndex = delegate(int _prefValue)
		{
			int num;
			if (_prefValue != 5)
			{
				if (_prefValue != 6)
				{
					num = 2;
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
			return num;
		};
		this.optionViewDistance.MapListIndexToGamePref = delegate(int _index)
		{
			int num2;
			if (_index != 0)
			{
				if (_index != 1)
				{
					num2 = 7;
				}
				else
				{
					num2 = 6;
				}
			}
			else
			{
				num2 = 5;
			}
			return num2;
		};
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			this.comboQualityPreset.Elements.Add("ConsolePerformance");
			this.comboQualityPreset.Elements.Add("ConsoleQuality");
		}
		XUiC_OptionEntryAbs[] allOptions = this.AllOptions;
		for (int i = 0; i < allOptions.Length; i++)
		{
			XUiC_OptionEntryGamePrefAbs xuiC_OptionEntryGamePrefAbs = allOptions[i] as XUiC_OptionEntryGamePrefAbs;
			List<object> list;
			if (xuiC_OptionEntryGamePrefAbs != null && xuiC_OptionEntryGamePrefAbs.GamePref != null && GameOptionsManager.QualityPresets.TryGetValue(xuiC_OptionEntryGamePrefAbs.GamePref.Value, out list))
			{
				xuiC_OptionEntryGamePrefAbs.ValueChanged += this.OnQualityBasedOptionChanged;
			}
		}
	}

	// Token: 0x06006AE7 RID: 27367 RVA: 0x0029D78B File Offset: 0x0029B98B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnQualityBasedOptionChanged(XUiC_OptionEntryAbs _option)
	{
		if (this.changingFromCode)
		{
			return;
		}
		this.comboQualityPreset.SelectedIndex = 5;
	}

	// Token: 0x06006AE8 RID: 27368 RVA: 0x0029D7A4 File Offset: 0x0029B9A4
	[XuiBindEvent("OnValueChanged", "comboQualityPreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnQualityPresetChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		GameOptionsPlatforms.GfxPreset gfxPreset = (GameOptionsPlatforms.GfxPreset)this.optionQualityPreset.SelectedValue;
		if (gfxPreset == GameOptionsPlatforms.GfxPreset.Custom)
		{
			return;
		}
		this.changingFromCode = true;
		int num = (int)gfxPreset;
		XUiC_OptionEntryAbs[] allOptions = this.AllOptions;
		for (int i = 0; i < allOptions.Length; i++)
		{
			XUiC_OptionEntryGamePrefAbs xuiC_OptionEntryGamePrefAbs = allOptions[i] as XUiC_OptionEntryGamePrefAbs;
			List<object> list;
			if (xuiC_OptionEntryGamePrefAbs != null && xuiC_OptionEntryGamePrefAbs.GamePref != null && GameOptionsManager.QualityPresets.TryGetValue(xuiC_OptionEntryGamePrefAbs.GamePref.Value, out list) && num < list.Count && list[num] != null)
			{
				xuiC_OptionEntryGamePrefAbs.SelectedValue = list[num];
			}
		}
		this.changingFromCode = false;
	}

	// Token: 0x14000096 RID: 150
	// (add) Token: 0x06006AE9 RID: 27369 RVA: 0x0029D850 File Offset: 0x0029BA50
	// (remove) Token: 0x06006AEA RID: 27370 RVA: 0x0029D884 File Offset: 0x0029BA84
	public static event Action OnSettingsChanged;

	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x06006AEB RID: 27371 RVA: 0x0029D8B7 File Offset: 0x0029BAB7
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			XUiC_TabSelector tabSelector = this.TabSelector;
			return ((tabSelector != null) ? tabSelector.SelectedTabKey : null) != "xuiOptionsVideoQuality";
		}
	}

	// Token: 0x06006AEC RID: 27372 RVA: 0x0029D8D8 File Offset: 0x0029BAD8
	public override void Init()
	{
		base.Init();
		XUiC_OptionsVideo.ID = base.WindowGroup.Id;
		this.initResolutionOptions();
		this.initUpscalerModeOptions();
		this.initQualityPresetBasedOptions();
		this.optionDynamicMeshDistance.MapGamePrefToListIndex = (int _prefValue) => this.comboDynamicMeshDistance.Elements.IndexOf(_prefValue);
		this.optionDynamicMeshDistance.MapListIndexToGamePref = (int _index) => this.comboDynamicMeshDistance.Elements[_index];
		this.optionFpsLimitInGame.MapGamePrefToListIndex = (int _prefValue) => this.comboFpsLimitInGame.Elements.IndexOf(_prefValue);
		this.optionFpsLimitInGame.MapListIndexToGamePref = (int _index) => this.comboFpsLimitInGame.Elements[_index];
		this.comboFpsLimitInGame.CustomValueFormatter = delegate(int _value)
		{
			if (_value > 0)
			{
				return string.Format(Localization.Get("xuiOptionsVideoFpsLimitValue", false, null), _value);
			}
			return Localization.Get("xuiComboOnOffOff", false, null);
		};
	}

	// Token: 0x06006AED RID: 27373 RVA: 0x0029D994 File Offset: 0x0029BB94
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		GamePrefs.Set(EnumGamePrefs.OptionsGfxMotionBlurEnabled, GamePrefs.GetInt(EnumGamePrefs.OptionsGfxMotionBlur) > 0);
		GameOptionsManager.ApplyAllOptions(this.xui.playerUI);
		QualitySettings.vSyncCount = GamePrefs.GetInt(base.VSyncCountPref);
		ReflectionManager.ApplyOptions(false);
		Action onSettingsChanged = XUiC_OptionsVideo.OnSettingsChanged;
		if (onSettingsChanged == null)
		{
			return;
		}
		onSettingsChanged();
	}

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x06006AEE RID: 27374 RVA: 0x0029D9F3 File Offset: 0x0029BBF3
	[XuiXmlBinding("texture_quality_min")]
	public int TextureQualityMin
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameOptionsManager.CalcTextureQualityMin();
		}
	}

	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x06006AEF RID: 27375 RVA: 0x0029D9FA File Offset: 0x0029BBFA
	[XuiXmlBinding("texture_quality_limited")]
	public bool TextureQualityLimited
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<string> xuiC_ComboBoxList = this.comboTextureQuality;
			return xuiC_ComboBoxList != null && xuiC_ComboBoxList.MinIndex > 0;
		}
	}

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x06006AF0 RID: 27376 RVA: 0x0029DA10 File Offset: 0x0029BC10
	[XuiXmlBinding("antialiasing_mode")]
	public int AntiAliasingMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<string> xuiC_ComboBoxList = this.comboAntiAliasing;
			if (xuiC_ComboBoxList == null)
			{
				return 0;
			}
			return xuiC_ComboBoxList.SelectedIndex;
		}
	}

	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06006AF1 RID: 27377 RVA: 0x0029DA23 File Offset: 0x0029BC23
	[XuiXmlBinding("vsync_mode")]
	public int VSyncMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<string> xuiC_ComboBoxList = this.comboVSync;
			if (xuiC_ComboBoxList == null)
			{
				return 0;
			}
			return xuiC_ComboBoxList.SelectedIndex;
		}
	}

	// Token: 0x06006AFB RID: 27387 RVA: 0x0029DBEC File Offset: 0x0029BDEC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <initResolutionOptions>g__GetCurrentResolution|6_8()
	{
		ValueTuple<int, int, FullScreenMode> screenOptions = PlatformApplicationManager.Application.ScreenOptions;
		int item = screenOptions.Item1;
		int item2 = screenOptions.Item2;
		this.lastResolution = new XUiC_OptionsVideo.ResolutionInfo(item, item2);
		if (!this.comboResolution.Elements.Contains(this.lastResolution))
		{
			this.comboResolution.Elements.Add(this.lastResolution);
		}
		this.comboResolution.Elements.Sort();
		this.comboResolution.SelectedIndex = this.comboResolution.Elements.IndexOf(this.lastResolution);
	}

	// Token: 0x06006AFC RID: 27388 RVA: 0x0029DC7C File Offset: 0x0029BE7C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static int <initResolutionOptions>g__ConvertFullScreenModeToIndex|6_9(FullScreenMode _mode)
	{
		int num;
		if (_mode != FullScreenMode.ExclusiveFullScreen)
		{
			if (_mode == FullScreenMode.FullScreenWindow)
			{
				num = 1;
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 2;
		}
		return num;
	}

	// Token: 0x06006AFD RID: 27389 RVA: 0x0029DC9C File Offset: 0x0029BE9C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static FullScreenMode <initResolutionOptions>g__ConvertIndexToFullScreenMode|6_10(int _index)
	{
		FullScreenMode fullScreenMode;
		if (_index != 1)
		{
			if (_index != 2)
			{
				fullScreenMode = FullScreenMode.Windowed;
			}
			else
			{
				fullScreenMode = FullScreenMode.ExclusiveFullScreen;
			}
		}
		else
		{
			fullScreenMode = FullScreenMode.FullScreenWindow;
		}
		return fullScreenMode;
	}

	// Token: 0x06006B02 RID: 27394 RVA: 0x0029DDB0 File Offset: 0x0029BFB0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static int <initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|16_4()
	{
		switch (GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode))
		{
		case 0:
			return 0;
		case 2:
			return XUiC_OptionsVideo.Fsr3Supported ? 3 : (-1);
		case 3:
			return 1;
		case 4:
			return 2;
		case 5:
			return XUiC_OptionsVideo.DlssSupported ? 4 : (-1);
		}
		return -1;
	}

	// Token: 0x06006B03 RID: 27395 RVA: 0x0029DE14 File Offset: 0x0029C014
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public int <initUpscalerModeOptions>g__SelectionToUpscalerMode|16_5()
	{
		int num;
		switch (this.comboUpscaler.SelectedIndex)
		{
		case 0:
			num = 0;
			break;
		case 1:
			num = 3;
			break;
		case 2:
			num = 4;
			break;
		case 3:
			num = (XUiC_OptionsVideo.Fsr3Supported ? 2 : GameOptionsPlatforms.DefaultUpscalerMode);
			break;
		case 4:
			num = ((XUiC_OptionsVideo.Fsr3Supported && XUiC_OptionsVideo.DlssSupported) ? 5 : GameOptionsPlatforms.DefaultUpscalerMode);
			break;
		default:
			num = GameOptionsPlatforms.DefaultUpscalerMode;
			break;
		}
		return num;
	}

	// Token: 0x04005166 RID: 20838
	[XuiBindComponent("OptionResolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionResolution;

	// Token: 0x04005167 RID: 20839
	[XuiBindComponent("Resolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsVideo.ResolutionInfo> comboResolution;

	// Token: 0x04005168 RID: 20840
	[XuiBindComponent("OptionFullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionFullscreen;

	// Token: 0x04005169 RID: 20841
	[XuiBindComponent("Fullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboFullscreen;

	// Token: 0x0400516A RID: 20842
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsVideo.ResolutionInfo lastResolution;

	// Token: 0x0400516B RID: 20843
	[XuiBindComponent("OptionUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x0400516C RID: 20844
	[XuiBindComponent("Upscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x0400516D RID: 20845
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x0400516E RID: 20846
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x0400516F RID: 20847
	[XuiBindComponent("OptionViewDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionViewDistance;

	// Token: 0x04005170 RID: 20848
	[XuiBindComponent("TextureQuality", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboTextureQuality;

	// Token: 0x04005171 RID: 20849
	[XuiBindComponent("AntiAliasing", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboAntiAliasing;

	// Token: 0x04005172 RID: 20850
	[PublicizedFrom(EAccessModifier.Private)]
	public bool changingFromCode;

	// Token: 0x04005173 RID: 20851
	public static string ID = "";

	// Token: 0x04005175 RID: 20853
	[XuiBindComponent("OptionDynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionDynamicMeshDistance;

	// Token: 0x04005176 RID: 20854
	[XuiBindComponent("DynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboDynamicMeshDistance;

	// Token: 0x04005177 RID: 20855
	[XuiBindComponent("OptionFpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionFpsLimitInGame;

	// Token: 0x04005178 RID: 20856
	[XuiBindComponent("FpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboFpsLimitInGame;

	// Token: 0x04005179 RID: 20857
	[XuiBindComponent("VSync", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboVSync;

	// Token: 0x02000DA5 RID: 3493
	public readonly struct ResolutionInfo : IComparable<XUiC_OptionsVideo.ResolutionInfo>, IEquatable<XUiC_OptionsVideo.ResolutionInfo>
	{
		// Token: 0x06006B08 RID: 27400 RVA: 0x0029DED4 File Offset: 0x0029C0D4
		[return: TupleElementNames(new string[] { "_aspectRatio", "_aspectRatioFactor", "_aspectRatioString" })]
		public static ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string> DimensionsToAspectRatio(int _width, int _height)
		{
			if (_height == 0)
			{
				return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Unknown, 0f, "n/a");
			}
			int num = 1000 * _width / _height;
			if (num >= 1770)
			{
				if (num >= 3550)
				{
					if (num <= 3600)
					{
						return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_32_9, 3.555f, "32:9");
					}
				}
				else if (num >= 1890)
				{
					if (num <= 1900)
					{
						return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_17_9, 2.37f, "17:9");
					}
					if (num <= 2370)
					{
						if (num == 2333 || num == 2370)
						{
							return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_21_9, 2.37f, "21:9");
						}
					}
					else
					{
						if (num == 2400)
						{
							return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_24_10, 2.4f, "24:10");
						}
						if (num == 3200)
						{
							return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_32_10, 3.2f, "32:10");
						}
					}
				}
				else if (num <= 1780)
				{
					return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_16_9, 1.777f, "16:9");
				}
			}
			else if (num >= 1560)
			{
				if (num >= 1660)
				{
					if (num <= 1670)
					{
						return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_5_3, 1.666f, "5:3");
					}
				}
				else
				{
					if (num <= 1570)
					{
						return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_25_16, 1.562f, "25:16");
					}
					if (num == 1600)
					{
						return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_16_10, 1.6f, "16:10");
					}
				}
			}
			else if (num >= 1330)
			{
				if (num <= 1340)
				{
					return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_4_3, 1.333f, "4:3");
				}
				if (num == 1500)
				{
					return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_3_2, 1.5f, "3:2");
				}
			}
			else
			{
				if (num == 1000)
				{
					return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_1_1, 1f, "1:1");
				}
				if (num == 1250)
				{
					return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Aspect_5_4, 1.25f, "5:4");
				}
			}
			float num2 = (float)_width / (float)_height;
			return new ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string>(XUiC_OptionsVideo.ResolutionInfo.EAspectRatio.Unknown, num2, num2.ToCultureInvariantString("0.##") + ":1");
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x0029E100 File Offset: 0x0029C300
		public ResolutionInfo(int _width, int _height)
		{
			this.Width = _width;
			if (_height > _width)
			{
				_height = _width;
			}
			this.Height = _height;
			ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string> valueTuple = XUiC_OptionsVideo.ResolutionInfo.DimensionsToAspectRatio(_width, _height);
			this.AspectRatio = valueTuple.Item1;
			string item = valueTuple.Item3;
			this.label = string.Concat(new string[]
			{
				_width.ToString(),
				"x",
				_height.ToString(),
				" (",
				item,
				")"
			});
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x0029E180 File Offset: 0x0029C380
		public int CompareTo(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			int num = this.Width.CompareTo(_other.Width);
			if (num == 0)
			{
				return this.Height.CompareTo(_other.Height);
			}
			return num;
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x0029E1BB File Offset: 0x0029C3BB
		public bool Equals(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			return this.Width == _other.Width && this.Height == _other.Height;
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x0029E1DB File Offset: 0x0029C3DB
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0400517A RID: 20858
		public readonly int Width;

		// Token: 0x0400517B RID: 20859
		public readonly int Height;

		// Token: 0x0400517C RID: 20860
		public readonly XUiC_OptionsVideo.ResolutionInfo.EAspectRatio AspectRatio;

		// Token: 0x0400517D RID: 20861
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string label;

		// Token: 0x02000DA6 RID: 3494
		public enum EAspectRatio
		{
			// Token: 0x0400517F RID: 20863
			Aspect_32_9,
			// Token: 0x04005180 RID: 20864
			Aspect_32_10,
			// Token: 0x04005181 RID: 20865
			Aspect_24_10,
			// Token: 0x04005182 RID: 20866
			Aspect_21_9,
			// Token: 0x04005183 RID: 20867
			Aspect_17_9,
			// Token: 0x04005184 RID: 20868
			Aspect_16_9,
			// Token: 0x04005185 RID: 20869
			Aspect_5_3,
			// Token: 0x04005186 RID: 20870
			Aspect_16_10,
			// Token: 0x04005187 RID: 20871
			Aspect_25_16,
			// Token: 0x04005188 RID: 20872
			Aspect_3_2,
			// Token: 0x04005189 RID: 20873
			Aspect_4_3,
			// Token: 0x0400518A RID: 20874
			Aspect_5_4,
			// Token: 0x0400518B RID: 20875
			Aspect_1_1,
			// Token: 0x0400518C RID: 20876
			Unknown
		}
	}

	// Token: 0x02000DA7 RID: 3495
	public enum UpscalerMode
	{
		// Token: 0x0400518E RID: 20878
		Off,
		// Token: 0x0400518F RID: 20879
		Dynamic,
		// Token: 0x04005190 RID: 20880
		Scale,
		// Token: 0x04005191 RID: 20881
		FSR3,
		// Token: 0x04005192 RID: 20882
		DLSS
	}

	// Token: 0x02000DA8 RID: 3496
	public enum GraphicsMode
	{
		// Token: 0x04005194 RID: 20884
		Lowest,
		// Token: 0x04005195 RID: 20885
		Low,
		// Token: 0x04005196 RID: 20886
		Medium,
		// Token: 0x04005197 RID: 20887
		High,
		// Token: 0x04005198 RID: 20888
		Ultra,
		// Token: 0x04005199 RID: 20889
		Custom,
		// Token: 0x0400519A RID: 20890
		ConsolePerformance,
		// Token: 0x0400519B RID: 20891
		ConsoleQuality
	}
}
