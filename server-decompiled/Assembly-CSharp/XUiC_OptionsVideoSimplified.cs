using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000DD9 RID: 3545
[Preserve]
public class XUiC_OptionsVideoSimplified : XUiC_OptionsVideoBase
{
	// Token: 0x06006BD5 RID: 27605 RVA: 0x002A5D80 File Offset: 0x002A3F80
	[PublicizedFrom(EAccessModifier.Private)]
	public void initUpscalerModeOptions()
	{
		if (this.optionUpscaler == null || this.comboUpscaler == null)
		{
			return;
		}
		this.optionUpscaler.GetSettingValue = delegate
		{
			int num = XUiC_OptionsVideoSimplified.<initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|3_4();
			if (num == -1)
			{
				Log.Out(string.Format("Upscaler mode \"{0}\" is unsupported on this platform; defaulting to \"{1}\".", GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode), 2));
				num = 0;
				GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, 2);
			}
			this.comboUpscaler.SelectedIndex = num;
		};
		this.optionUpscaler.DiscardChanges = delegate
		{
			this.comboUpscaler.SelectedIndex = XUiC_OptionsVideoSimplified.<initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|3_4();
		};
		this.optionUpscaler.ApplyChanges = delegate(bool _)
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxUpscalerMode, this.<initUpscalerModeOptions>g__SelectionToUpscalerMode|3_5());
		};
		this.optionUpscaler.IsChangedDelegate = () => this.comboUpscaler.SelectedIndex != XUiC_OptionsVideoSimplified.<initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|3_4();
	}

	// Token: 0x06006BD6 RID: 27606 RVA: 0x002A5DFC File Offset: 0x002A3FFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void initQualityPresetBasedOptions()
	{
		this.optionQualityPreset.MapGamePrefToListIndex = delegate(int _prefValue)
		{
			XUiC_OptionsVideoSimplified.GraphicsMode graphicsMode;
			if (_prefValue != 6)
			{
				if (_prefValue != 8)
				{
					graphicsMode = XUiC_OptionsVideoSimplified.GraphicsMode.ConsoleCustom;
				}
				else
				{
					graphicsMode = XUiC_OptionsVideoSimplified.GraphicsMode.ConsoleQuality;
				}
			}
			else
			{
				graphicsMode = XUiC_OptionsVideoSimplified.GraphicsMode.ConsolePerformance;
			}
			return (int)graphicsMode;
		};
		this.optionQualityPreset.MapListIndexToGamePref = delegate(int _index)
		{
			GameOptionsPlatforms.GfxPreset gfxPreset;
			if (_index != 0)
			{
				if (_index != 1)
				{
					gfxPreset = GameOptionsPlatforms.GfxPreset.Custom;
				}
				else
				{
					gfxPreset = GameOptionsPlatforms.GfxPreset.ConsoleQuality;
				}
			}
			else
			{
				gfxPreset = GameOptionsPlatforms.GfxPreset.ConsolePerformance;
			}
			return (int)gfxPreset;
		};
	}

	// Token: 0x14000097 RID: 151
	// (add) Token: 0x06006BD7 RID: 27607 RVA: 0x002A5E60 File Offset: 0x002A4060
	// (remove) Token: 0x06006BD8 RID: 27608 RVA: 0x002A5E94 File Offset: 0x002A4094
	public static event Action OnSettingsChanged;

	// Token: 0x06006BD9 RID: 27609 RVA: 0x002A5EC7 File Offset: 0x002A40C7
	public override void Init()
	{
		base.Init();
		XUiC_OptionsVideoSimplified.ID = base.WindowGroup.Id;
		this.initUpscalerModeOptions();
		this.initQualityPresetBasedOptions();
	}

	// Token: 0x06006BDA RID: 27610 RVA: 0x002A5EEB File Offset: 0x002A40EB
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateCustomModeVisibility();
	}

	// Token: 0x06006BDB RID: 27611 RVA: 0x002A5EF9 File Offset: 0x002A40F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCustomModeVisibility()
	{
		this.comboQualityPreset.MaxIndex = Math.Max(this.comboQualityPreset.SelectedIndex, 1);
	}

	// Token: 0x06006BDC RID: 27612 RVA: 0x002A5F17 File Offset: 0x002A4117
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.updateCustomModeVisibility();
		GameOptionsManager.SetGraphicsQuality();
		GameOptionsManager.ApplyAllOptions(this.xui.playerUI);
		Action onSettingsChanged = XUiC_OptionsVideoSimplified.OnSettingsChanged;
		if (onSettingsChanged == null)
		{
			return;
		}
		onSettingsChanged();
	}

	// Token: 0x06006BE3 RID: 27619 RVA: 0x002A5FE8 File Offset: 0x002A41E8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static int <initUpscalerModeOptions>g__GamePrefUpscalerModeIndex|3_4()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxUpscalerMode);
		int num;
		if (@int != 2)
		{
			if (@int != 4)
			{
				num = -1;
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
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x002A6018 File Offset: 0x002A4218
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public int <initUpscalerModeOptions>g__SelectionToUpscalerMode|3_5()
	{
		int selectedIndex = this.comboUpscaler.SelectedIndex;
		int num;
		if (selectedIndex != 0)
		{
			if (selectedIndex != 1)
			{
				num = GameOptionsPlatforms.DefaultUpscalerMode;
			}
			else
			{
				num = 4;
			}
		}
		else
		{
			num = 2;
		}
		return num;
	}

	// Token: 0x040052A1 RID: 21153
	[XuiBindComponent("OptionConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x040052A2 RID: 21154
	[XuiBindComponent("ConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x040052A3 RID: 21155
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x040052A4 RID: 21156
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x040052A5 RID: 21157
	public static string ID = "";

	// Token: 0x02000DDA RID: 3546
	public enum UpscalerMode
	{
		// Token: 0x040052A8 RID: 21160
		FSR3,
		// Token: 0x040052A9 RID: 21161
		Scale
	}

	// Token: 0x02000DDB RID: 3547
	public enum GraphicsMode
	{
		// Token: 0x040052AB RID: 21163
		ConsolePerformance,
		// Token: 0x040052AC RID: 21164
		ConsoleQuality,
		// Token: 0x040052AD RID: 21165
		ConsoleCustom
	}
}
