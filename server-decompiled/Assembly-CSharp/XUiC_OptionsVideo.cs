using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Scripting;

// Token: 0x02000DC7 RID: 3527
[Preserve]
public class XUiC_OptionsVideo : XUiC_OptionsVideoBase
{
	// Token: 0x06006B74 RID: 27508 RVA: 0x002A3994 File Offset: 0x002A1B94
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

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x06006B75 RID: 27509 RVA: 0x002A3A8D File Offset: 0x002A1C8D
	public static bool Fsr3Supported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return FSR3.FSR3Supported();
		}
	}

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x06006B76 RID: 27510 RVA: 0x002A3A94 File Offset: 0x002A1C94
	public static bool DlssSupported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsVideo.Fsr3Supported && DLSS.DLSSSupported();
		}
	}

	// Token: 0x17000BDE RID: 3038
	// (get) Token: 0x06006B77 RID: 27511 RVA: 0x002A3AA4 File Offset: 0x002A1CA4
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

	// Token: 0x06006B78 RID: 27512 RVA: 0x002A3AB8 File Offset: 0x002A1CB8
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

	// Token: 0x06006B79 RID: 27513 RVA: 0x002A3B34 File Offset: 0x002A1D34
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

	// Token: 0x06006B7A RID: 27514 RVA: 0x002A3C7B File Offset: 0x002A1E7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnQualityBasedOptionChanged(XUiC_OptionEntryAbs _option)
	{
		if (this.changingFromCode)
		{
			return;
		}
		this.comboQualityPreset.SelectedIndex = 5;
	}

	// Token: 0x06006B7B RID: 27515 RVA: 0x002A3C94 File Offset: 0x002A1E94
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
	// (add) Token: 0x06006B7C RID: 27516 RVA: 0x002A3D40 File Offset: 0x002A1F40
	// (remove) Token: 0x06006B7D RID: 27517 RVA: 0x002A3D74 File Offset: 0x002A1F74
	public static event Action OnSettingsChanged;

	// Token: 0x17000BDF RID: 3039
	// (get) Token: 0x06006B7E RID: 27518 RVA: 0x002A3DA7 File Offset: 0x002A1FA7
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			XUiC_TabSelector tabSelector = this.TabSelector;
			return ((tabSelector != null) ? tabSelector.SelectedTabKey : null) != "xuiOptionsVideoQuality";
		}
	}

	// Token: 0x06006B7F RID: 27519 RVA: 0x002A3DC8 File Offset: 0x002A1FC8
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

	// Token: 0x06006B80 RID: 27520 RVA: 0x002A3E84 File Offset: 0x002A2084
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

	// Token: 0x17000BE0 RID: 3040
	// (get) Token: 0x06006B81 RID: 27521 RVA: 0x002A3EE3 File Offset: 0x002A20E3
	[XuiXmlBinding("texture_quality_min")]
	public int TextureQualityMin
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameOptionsManager.CalcTextureQualityMin();
		}
	}

	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x06006B82 RID: 27522 RVA: 0x002A3EEA File Offset: 0x002A20EA
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

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x06006B83 RID: 27523 RVA: 0x002A3F00 File Offset: 0x002A2100
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

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x06006B84 RID: 27524 RVA: 0x002A3F13 File Offset: 0x002A2113
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

	// Token: 0x06006B8E RID: 27534 RVA: 0x002A40DC File Offset: 0x002A22DC
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

	// Token: 0x06006B8F RID: 27535 RVA: 0x002A416C File Offset: 0x002A236C
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

	// Token: 0x06006B90 RID: 27536 RVA: 0x002A418C File Offset: 0x002A238C
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

	// Token: 0x06006B95 RID: 27541 RVA: 0x002A42A0 File Offset: 0x002A24A0
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

	// Token: 0x06006B96 RID: 27542 RVA: 0x002A4304 File Offset: 0x002A2504
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

	// Token: 0x0400523F RID: 21055
	[XuiBindComponent("OptionResolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionResolution;

	// Token: 0x04005240 RID: 21056
	[XuiBindComponent("Resolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsVideo.ResolutionInfo> comboResolution;

	// Token: 0x04005241 RID: 21057
	[XuiBindComponent("OptionFullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionFullscreen;

	// Token: 0x04005242 RID: 21058
	[XuiBindComponent("Fullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboFullscreen;

	// Token: 0x04005243 RID: 21059
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsVideo.ResolutionInfo lastResolution;

	// Token: 0x04005244 RID: 21060
	[XuiBindComponent("OptionUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x04005245 RID: 21061
	[XuiBindComponent("Upscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x04005246 RID: 21062
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x04005247 RID: 21063
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x04005248 RID: 21064
	[XuiBindComponent("OptionViewDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionViewDistance;

	// Token: 0x04005249 RID: 21065
	[XuiBindComponent("TextureQuality", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboTextureQuality;

	// Token: 0x0400524A RID: 21066
	[XuiBindComponent("AntiAliasing", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboAntiAliasing;

	// Token: 0x0400524B RID: 21067
	[PublicizedFrom(EAccessModifier.Private)]
	public bool changingFromCode;

	// Token: 0x0400524C RID: 21068
	public static string ID = "";

	// Token: 0x0400524E RID: 21070
	[XuiBindComponent("OptionDynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionDynamicMeshDistance;

	// Token: 0x0400524F RID: 21071
	[XuiBindComponent("DynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboDynamicMeshDistance;

	// Token: 0x04005250 RID: 21072
	[XuiBindComponent("OptionFpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionFpsLimitInGame;

	// Token: 0x04005251 RID: 21073
	[XuiBindComponent("FpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboFpsLimitInGame;

	// Token: 0x04005252 RID: 21074
	[XuiBindComponent("VSync", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboVSync;

	// Token: 0x02000DC8 RID: 3528
	public readonly struct ResolutionInfo : IComparable<XUiC_OptionsVideo.ResolutionInfo>, IEquatable<XUiC_OptionsVideo.ResolutionInfo>
	{
		// Token: 0x06006B9B RID: 27547 RVA: 0x002A43C4 File Offset: 0x002A25C4
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

		// Token: 0x06006B9C RID: 27548 RVA: 0x002A45F0 File Offset: 0x002A27F0
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

		// Token: 0x06006B9D RID: 27549 RVA: 0x002A4670 File Offset: 0x002A2870
		public int CompareTo(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			int num = this.Width.CompareTo(_other.Width);
			if (num == 0)
			{
				return this.Height.CompareTo(_other.Height);
			}
			return num;
		}

		// Token: 0x06006B9E RID: 27550 RVA: 0x002A46AB File Offset: 0x002A28AB
		public bool Equals(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			return this.Width == _other.Width && this.Height == _other.Height;
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x002A46CB File Offset: 0x002A28CB
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x04005253 RID: 21075
		public readonly int Width;

		// Token: 0x04005254 RID: 21076
		public readonly int Height;

		// Token: 0x04005255 RID: 21077
		public readonly XUiC_OptionsVideo.ResolutionInfo.EAspectRatio AspectRatio;

		// Token: 0x04005256 RID: 21078
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string label;

		// Token: 0x02000DC9 RID: 3529
		public enum EAspectRatio
		{
			// Token: 0x04005258 RID: 21080
			Aspect_32_9,
			// Token: 0x04005259 RID: 21081
			Aspect_32_10,
			// Token: 0x0400525A RID: 21082
			Aspect_24_10,
			// Token: 0x0400525B RID: 21083
			Aspect_21_9,
			// Token: 0x0400525C RID: 21084
			Aspect_17_9,
			// Token: 0x0400525D RID: 21085
			Aspect_16_9,
			// Token: 0x0400525E RID: 21086
			Aspect_5_3,
			// Token: 0x0400525F RID: 21087
			Aspect_16_10,
			// Token: 0x04005260 RID: 21088
			Aspect_25_16,
			// Token: 0x04005261 RID: 21089
			Aspect_3_2,
			// Token: 0x04005262 RID: 21090
			Aspect_4_3,
			// Token: 0x04005263 RID: 21091
			Aspect_5_4,
			// Token: 0x04005264 RID: 21092
			Aspect_1_1,
			// Token: 0x04005265 RID: 21093
			Unknown
		}
	}

	// Token: 0x02000DCA RID: 3530
	public enum UpscalerMode
	{
		// Token: 0x04005267 RID: 21095
		Off,
		// Token: 0x04005268 RID: 21096
		Dynamic,
		// Token: 0x04005269 RID: 21097
		Scale,
		// Token: 0x0400526A RID: 21098
		FSR3,
		// Token: 0x0400526B RID: 21099
		DLSS
	}

	// Token: 0x02000DCB RID: 3531
	public enum GraphicsMode
	{
		// Token: 0x0400526D RID: 21101
		Lowest,
		// Token: 0x0400526E RID: 21102
		Low,
		// Token: 0x0400526F RID: 21103
		Medium,
		// Token: 0x04005270 RID: 21104
		High,
		// Token: 0x04005271 RID: 21105
		Ultra,
		// Token: 0x04005272 RID: 21106
		Custom,
		// Token: 0x04005273 RID: 21107
		ConsolePerformance,
		// Token: 0x04005274 RID: 21108
		ConsoleQuality
	}
}
