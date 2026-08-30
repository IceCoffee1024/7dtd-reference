using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000DAB RID: 3499
[Preserve]
public class XUiC_OptionsVideoSimplified : XUiC_OptionsVideoBase
{
	// Token: 0x06006B1D RID: 27421 RVA: 0x0029E368 File Offset: 0x0029C568
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

	// Token: 0x06006B1E RID: 27422 RVA: 0x0029E3E4 File Offset: 0x0029C5E4
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
	// (add) Token: 0x06006B1F RID: 27423 RVA: 0x0029E448 File Offset: 0x0029C648
	// (remove) Token: 0x06006B20 RID: 27424 RVA: 0x0029E47C File Offset: 0x0029C67C
	public static event Action OnSettingsChanged;

	// Token: 0x06006B21 RID: 27425 RVA: 0x0029E4AF File Offset: 0x0029C6AF
	public override void Init()
	{
		base.Init();
		XUiC_OptionsVideoSimplified.ID = base.WindowGroup.Id;
		this.initUpscalerModeOptions();
		this.initQualityPresetBasedOptions();
	}

	// Token: 0x06006B22 RID: 27426 RVA: 0x0029E4D3 File Offset: 0x0029C6D3
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateCustomModeVisibility();
	}

	// Token: 0x06006B23 RID: 27427 RVA: 0x0029E4E1 File Offset: 0x0029C6E1
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCustomModeVisibility()
	{
		this.comboQualityPreset.MaxIndex = Math.Max(this.comboQualityPreset.SelectedIndex, 1);
	}

	// Token: 0x06006B24 RID: 27428 RVA: 0x0029E4FF File Offset: 0x0029C6FF
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

	// Token: 0x06006B2B RID: 27435 RVA: 0x0029E5D0 File Offset: 0x0029C7D0
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

	// Token: 0x06006B2C RID: 27436 RVA: 0x0029E600 File Offset: 0x0029C800
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

	// Token: 0x040051A3 RID: 20899
	[XuiBindComponent("OptionConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x040051A4 RID: 20900
	[XuiBindComponent("ConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x040051A5 RID: 20901
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x040051A6 RID: 20902
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x040051A7 RID: 20903
	public static string ID = "";

	// Token: 0x02000DAC RID: 3500
	public enum UpscalerMode
	{
		// Token: 0x040051AA RID: 20906
		FSR3,
		// Token: 0x040051AB RID: 20907
		Scale
	}

	// Token: 0x02000DAD RID: 3501
	public enum GraphicsMode
	{
		// Token: 0x040051AD RID: 20909
		ConsolePerformance,
		// Token: 0x040051AE RID: 20910
		ConsoleQuality,
		// Token: 0x040051AF RID: 20911
		ConsoleCustom
	}
}
