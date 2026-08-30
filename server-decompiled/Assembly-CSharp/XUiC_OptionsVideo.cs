using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Scripting;

// Token: 0x02000DB8 RID: 3512
[Preserve]
public class XUiC_OptionsVideo : XUiC_OptionsVideoBase
{
	// Token: 0x06006B7C RID: 27516 RVA: 0x002A0604 File Offset: 0x0029E804
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

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x06006B7D RID: 27517 RVA: 0x002A06FD File Offset: 0x0029E8FD
	public static bool Fsr3Supported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return FSR3.FSR3Supported();
		}
	}

	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x06006B7E RID: 27518 RVA: 0x002A0704 File Offset: 0x0029E904
	public static bool DlssSupported
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_OptionsVideo.Fsr3Supported && DLSS.DLSSSupported();
		}
	}

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x06006B7F RID: 27519 RVA: 0x002A0714 File Offset: 0x0029E914
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

	// Token: 0x06006B80 RID: 27520 RVA: 0x002A0728 File Offset: 0x0029E928
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

	// Token: 0x06006B81 RID: 27521 RVA: 0x002A07A4 File Offset: 0x0029E9A4
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

	// Token: 0x06006B82 RID: 27522 RVA: 0x002A08EB File Offset: 0x0029EAEB
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnQualityBasedOptionChanged(XUiC_OptionEntryAbs _option)
	{
		if (this.changingFromCode)
		{
			return;
		}
		this.comboQualityPreset.SelectedIndex = 5;
	}

	// Token: 0x06006B83 RID: 27523 RVA: 0x002A0904 File Offset: 0x0029EB04
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

	// Token: 0x14000097 RID: 151
	// (add) Token: 0x06006B84 RID: 27524 RVA: 0x002A09B0 File Offset: 0x0029EBB0
	// (remove) Token: 0x06006B85 RID: 27525 RVA: 0x002A09E4 File Offset: 0x0029EBE4
	public static event Action OnSettingsChanged;

	// Token: 0x17000BC4 RID: 3012
	// (get) Token: 0x06006B86 RID: 27526 RVA: 0x002A0A17 File Offset: 0x0029EC17
	public override bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			XUiC_TabSelector tabSelector = this.TabSelector;
			return ((tabSelector != null) ? tabSelector.SelectedTabKey : null) != "xuiOptionsVideoQuality";
		}
	}

	// Token: 0x06006B87 RID: 27527 RVA: 0x002A0A38 File Offset: 0x0029EC38
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

	// Token: 0x06006B88 RID: 27528 RVA: 0x002A0AF4 File Offset: 0x0029ECF4
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

	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x06006B89 RID: 27529 RVA: 0x002A0B53 File Offset: 0x0029ED53
	[XuiXmlBinding("texture_quality_min")]
	public int TextureQualityMin
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameOptionsManager.CalcTextureQualityMin();
		}
	}

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x06006B8A RID: 27530 RVA: 0x002A0B5A File Offset: 0x0029ED5A
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

	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06006B8B RID: 27531 RVA: 0x002A0B70 File Offset: 0x0029ED70
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

	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x06006B8C RID: 27532 RVA: 0x002A0B83 File Offset: 0x0029ED83
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

	// Token: 0x06006B96 RID: 27542 RVA: 0x002A0D4C File Offset: 0x0029EF4C
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

	// Token: 0x06006B97 RID: 27543 RVA: 0x002A0DDC File Offset: 0x0029EFDC
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

	// Token: 0x06006B98 RID: 27544 RVA: 0x002A0DFC File Offset: 0x0029EFFC
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

	// Token: 0x06006B9D RID: 27549 RVA: 0x002A0F10 File Offset: 0x0029F110
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

	// Token: 0x06006B9E RID: 27550 RVA: 0x002A0F74 File Offset: 0x0029F174
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

	// Token: 0x04005208 RID: 21000
	[XuiBindComponent("OptionResolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionResolution;

	// Token: 0x04005209 RID: 21001
	[XuiBindComponent("Resolution", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_OptionsVideo.ResolutionInfo> comboResolution;

	// Token: 0x0400520A RID: 21002
	[XuiBindComponent("OptionFullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionFullscreen;

	// Token: 0x0400520B RID: 21003
	[XuiBindComponent("Fullscreen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboFullscreen;

	// Token: 0x0400520C RID: 21004
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsVideo.ResolutionInfo lastResolution;

	// Token: 0x0400520D RID: 21005
	[XuiBindComponent("OptionUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x0400520E RID: 21006
	[XuiBindComponent("Upscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x0400520F RID: 21007
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x04005210 RID: 21008
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x04005211 RID: 21009
	[XuiBindComponent("OptionViewDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionViewDistance;

	// Token: 0x04005212 RID: 21010
	[XuiBindComponent("TextureQuality", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboTextureQuality;

	// Token: 0x04005213 RID: 21011
	[XuiBindComponent("AntiAliasing", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboAntiAliasing;

	// Token: 0x04005214 RID: 21012
	[PublicizedFrom(EAccessModifier.Private)]
	public bool changingFromCode;

	// Token: 0x04005215 RID: 21013
	public static string ID = "";

	// Token: 0x04005217 RID: 21015
	[XuiBindComponent("OptionDynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionDynamicMeshDistance;

	// Token: 0x04005218 RID: 21016
	[XuiBindComponent("DynamicMeshDistance", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboDynamicMeshDistance;

	// Token: 0x04005219 RID: 21017
	[XuiBindComponent("OptionFpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionFpsLimitInGame;

	// Token: 0x0400521A RID: 21018
	[XuiBindComponent("FpsLimitInGame", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> comboFpsLimitInGame;

	// Token: 0x0400521B RID: 21019
	[XuiBindComponent("VSync", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboVSync;

	// Token: 0x02000DB9 RID: 3513
	public readonly struct ResolutionInfo : IComparable<XUiC_OptionsVideo.ResolutionInfo>, IEquatable<XUiC_OptionsVideo.ResolutionInfo>
	{
		// Token: 0x06006BA3 RID: 27555 RVA: 0x002A1034 File Offset: 0x0029F234
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

		// Token: 0x06006BA4 RID: 27556 RVA: 0x002A1260 File Offset: 0x0029F460
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

		// Token: 0x06006BA5 RID: 27557 RVA: 0x002A12E0 File Offset: 0x0029F4E0
		public int CompareTo(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			int num = this.Width.CompareTo(_other.Width);
			if (num == 0)
			{
				return this.Height.CompareTo(_other.Height);
			}
			return num;
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x002A131B File Offset: 0x0029F51B
		public bool Equals(XUiC_OptionsVideo.ResolutionInfo _other)
		{
			return this.Width == _other.Width && this.Height == _other.Height;
		}

		// Token: 0x06006BA7 RID: 27559 RVA: 0x002A133B File Offset: 0x0029F53B
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0400521C RID: 21020
		public readonly int Width;

		// Token: 0x0400521D RID: 21021
		public readonly int Height;

		// Token: 0x0400521E RID: 21022
		public readonly XUiC_OptionsVideo.ResolutionInfo.EAspectRatio AspectRatio;

		// Token: 0x0400521F RID: 21023
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string label;

		// Token: 0x02000DBA RID: 3514
		public enum EAspectRatio
		{
			// Token: 0x04005221 RID: 21025
			Aspect_32_9,
			// Token: 0x04005222 RID: 21026
			Aspect_32_10,
			// Token: 0x04005223 RID: 21027
			Aspect_24_10,
			// Token: 0x04005224 RID: 21028
			Aspect_21_9,
			// Token: 0x04005225 RID: 21029
			Aspect_17_9,
			// Token: 0x04005226 RID: 21030
			Aspect_16_9,
			// Token: 0x04005227 RID: 21031
			Aspect_5_3,
			// Token: 0x04005228 RID: 21032
			Aspect_16_10,
			// Token: 0x04005229 RID: 21033
			Aspect_25_16,
			// Token: 0x0400522A RID: 21034
			Aspect_3_2,
			// Token: 0x0400522B RID: 21035
			Aspect_4_3,
			// Token: 0x0400522C RID: 21036
			Aspect_5_4,
			// Token: 0x0400522D RID: 21037
			Aspect_1_1,
			// Token: 0x0400522E RID: 21038
			Unknown
		}
	}

	// Token: 0x02000DBB RID: 3515
	public enum UpscalerMode
	{
		// Token: 0x04005230 RID: 21040
		Off,
		// Token: 0x04005231 RID: 21041
		Dynamic,
		// Token: 0x04005232 RID: 21042
		Scale,
		// Token: 0x04005233 RID: 21043
		FSR3,
		// Token: 0x04005234 RID: 21044
		DLSS
	}

	// Token: 0x02000DBC RID: 3516
	public enum GraphicsMode
	{
		// Token: 0x04005236 RID: 21046
		Lowest,
		// Token: 0x04005237 RID: 21047
		Low,
		// Token: 0x04005238 RID: 21048
		Medium,
		// Token: 0x04005239 RID: 21049
		High,
		// Token: 0x0400523A RID: 21050
		Ultra,
		// Token: 0x0400523B RID: 21051
		Custom,
		// Token: 0x0400523C RID: 21052
		ConsolePerformance,
		// Token: 0x0400523D RID: 21053
		ConsoleQuality
	}
}
