using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Scripting;

// Token: 0x02000DD2 RID: 3538
[Preserve]
public class XUiC_OptionsVideo : XUiC_OptionsVideoBase
{
	// Token: 0x06006B99 RID: 27545 RVA: 0x002A4EBC File Offset: 0x002A30BC
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

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x06006B9A RID: 27546 RVA: 0x002A4FB5 File Offset: 0x002A31B5
	public static bool Fsr3Supported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return FSR3.FSR3Supported();
		}
	}

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x06006B9B RID: 27547 RVA: 0x002A4FBC File Offset: 0x002A31BC
	public static bool DlssSupported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsVideo.Fsr3Supported && DLSS.DLSSSupported();
		}
	}

	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x06006B9C RID: 27548 RVA: 0x002A4FCC File Offset: 0x002A31CC
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

	// Token: 0x06006B9D RID: 27549 RVA: 0x002A4FE0 File Offset: 0x002A31E0
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

	// Token: 0x06006B9E RID: 27550 RVA: 0x002A505C File Offset: 0x002A325C
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

	// Token: 0x06006B9F RID: 27551 RVA: 0x002A51A3 File Offset: 0x002A33A3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnQualityBasedOptionChanged(XUiC_OptionEntryAbs _option)
	{
		if (this.changingFromCode)
		{
			return;
		}
		this.comboQualityPreset.SelectedIndex = 5;
	}

	// Token: 0x06006BA0 RID: 27552 RVA: 0x002A51BC File Offset: 0x002A33BC
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
	// (add) Token: 0x06006BA1 RID: 27553 RVA: 0x002A5268 File Offset: 0x002A3468
	// (remove) Token: 0x06006BA2 RID: 27554 RVA: 0x002A529C File Offset: 0x002A349C
	public static event Action OnSettingsChanged;

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x06006BA3 RID: 27555 RVA: 0x002A52CF File Offset: 0x002A34CF
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			XUiC_TabSelector tabSelector = this.TabSelector;
			return ((tabSelector != null) ? tabSelector.SelectedTabKey : null) != "xuiOptionsVideoQuality";
		}
	}

	// Token: 0x06006BA4 RID: 27556 RVA: 0x002A52F0 File Offset: 0x002A34F0
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

	// Token: 0x06006BA5 RID: 27557 RVA: 0x002A53AC File Offset: 0x002A35AC
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

	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x06006BA6 RID: 27558 RVA: 0x002A540B File Offset: 0x002A360B
	[XuiXmlBinding("texture_quality_min")]
	public int TextureQualityMin
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameOptionsManager.CalcTextureQualityMin();
		}
	}

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x06006BA7 RID: 27559 RVA: 0x002A5412 File Offset: 0x002A3612
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

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x06006BA8 RID: 27560 RVA: 0x002A5428 File Offset: 0x002A3628
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

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x06006BA9 RID: 27561 RVA: 0x002A543B File Offset: 0x002A363B
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

	// Token: 0x06006BB3 RID: 27571 RVA: 0x002A5604 File Offset: 0x002A3804
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

	// Token: 0x06006BB4 RID: 27572 RVA: 0x002A5694 File Offset: 0x002A3894
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

	// Token: 0x06006BB5 RID: 27573 RVA: 0x002A56B4 File Offset: 0x002A38B4
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

	// Token: 0x06006BBA RID: 27578 RVA: 0x002A57C8 File Offset: 0x002A39C8
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

	// Token: 0x06006BBB RID: 27579 RVA: 0x002A582C File Offset: 0x002A3A2C
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

	// Token: 0x04005264 RID: 21092
	[XuiBindComponent("OptionResolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionResolution;

	// Token: 0x04005265 RID: 21093
	[XuiBindComponent("Resolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsVideo.ResolutionInfo> comboResolution;

	// Token: 0x04005266 RID: 21094
	[XuiBindComponent("OptionFullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionFullscreen;

	// Token: 0x04005267 RID: 21095
	[XuiBindComponent("Fullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboFullscreen;

	// Token: 0x04005268 RID: 21096
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsVideo.ResolutionInfo lastResolution;

	// Token: 0x04005269 RID: 21097
	[XuiBindComponent("OptionUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x0400526A RID: 21098
	[XuiBindComponent("Upscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x0400526B RID: 21099
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x0400526C RID: 21100
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x0400526D RID: 21101
	[XuiBindComponent("OptionViewDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionViewDistance;

	// Token: 0x0400526E RID: 21102
	[XuiBindComponent("TextureQuality", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboTextureQuality;

	// Token: 0x0400526F RID: 21103
	[XuiBindComponent("AntiAliasing", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboAntiAliasing;

	// Token: 0x04005270 RID: 21104
	[PublicizedFrom(EAccessModifier.Private)]
	public bool changingFromCode;

	// Token: 0x04005271 RID: 21105
	public static string ID = "";

	// Token: 0x04005273 RID: 21107
	[XuiBindComponent("OptionDynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionDynamicMeshDistance;

	// Token: 0x04005274 RID: 21108
	[XuiBindComponent("DynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboDynamicMeshDistance;

	// Token: 0x04005275 RID: 21109
	[XuiBindComponent("OptionFpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionFpsLimitInGame;

	// Token: 0x04005276 RID: 21110
	[XuiBindComponent("FpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboFpsLimitInGame;

	// Token: 0x04005277 RID: 21111
	[XuiBindComponent("VSync", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboVSync;

	// Token: 0x02000DD3 RID: 3539
	public readonly struct ResolutionInfo : IComparable<XUiC_OptionsVideo.ResolutionInfo>, IEquatable<XUiC_OptionsVideo.ResolutionInfo>
	{
		// Token: 0x06006BC0 RID: 27584 RVA: 0x002A58EC File Offset: 0x002A3AEC
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

		// Token: 0x06006BC1 RID: 27585 RVA: 0x002A5B18 File Offset: 0x002A3D18
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

		// Token: 0x06006BC2 RID: 27586 RVA: 0x002A5B98 File Offset: 0x002A3D98
		public int CompareTo(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			int num = this.Width.CompareTo(_other.Width);
			if (num == 0)
			{
				return this.Height.CompareTo(_other.Height);
			}
			return num;
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x002A5BD3 File Offset: 0x002A3DD3
		public bool Equals(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			return this.Width == _other.Width && this.Height == _other.Height;
		}

		// Token: 0x06006BC4 RID: 27588 RVA: 0x002A5BF3 File Offset: 0x002A3DF3
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x04005278 RID: 21112
		public readonly int Width;

		// Token: 0x04005279 RID: 21113
		public readonly int Height;

		// Token: 0x0400527A RID: 21114
		public readonly XUiC_OptionsVideo.ResolutionInfo.EAspectRatio AspectRatio;

		// Token: 0x0400527B RID: 21115
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string label;

		// Token: 0x02000DD4 RID: 3540
		public enum EAspectRatio
		{
			// Token: 0x0400527D RID: 21117
			Aspect_32_9,
			// Token: 0x0400527E RID: 21118
			Aspect_32_10,
			// Token: 0x0400527F RID: 21119
			Aspect_24_10,
			// Token: 0x04005280 RID: 21120
			Aspect_21_9,
			// Token: 0x04005281 RID: 21121
			Aspect_17_9,
			// Token: 0x04005282 RID: 21122
			Aspect_16_9,
			// Token: 0x04005283 RID: 21123
			Aspect_5_3,
			// Token: 0x04005284 RID: 21124
			Aspect_16_10,
			// Token: 0x04005285 RID: 21125
			Aspect_25_16,
			// Token: 0x04005286 RID: 21126
			Aspect_3_2,
			// Token: 0x04005287 RID: 21127
			Aspect_4_3,
			// Token: 0x04005288 RID: 21128
			Aspect_5_4,
			// Token: 0x04005289 RID: 21129
			Aspect_1_1,
			// Token: 0x0400528A RID: 21130
			Unknown
		}
	}

	// Token: 0x02000DD5 RID: 3541
	public enum UpscalerMode
	{
		// Token: 0x0400528C RID: 21132
		Off,
		// Token: 0x0400528D RID: 21133
		Dynamic,
		// Token: 0x0400528E RID: 21134
		Scale,
		// Token: 0x0400528F RID: 21135
		FSR3,
		// Token: 0x04005290 RID: 21136
		DLSS
	}

	// Token: 0x02000DD6 RID: 3542
	public enum GraphicsMode
	{
		// Token: 0x04005292 RID: 21138
		Lowest,
		// Token: 0x04005293 RID: 21139
		Low,
		// Token: 0x04005294 RID: 21140
		Medium,
		// Token: 0x04005295 RID: 21141
		High,
		// Token: 0x04005296 RID: 21142
		Ultra,
		// Token: 0x04005297 RID: 21143
		Custom,
		// Token: 0x04005298 RID: 21144
		ConsolePerformance,
		// Token: 0x04005299 RID: 21145
		ConsoleQuality
	}
}
