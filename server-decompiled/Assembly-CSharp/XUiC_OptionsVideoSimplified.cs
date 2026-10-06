using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000DCE RID: 3534
[Preserve]
public class XUiC_OptionsVideoSimplified : XUiC_OptionsVideoBase
{
	// Token: 0x06006BB0 RID: 27568 RVA: 0x002A4858 File Offset: 0x002A2A58
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

	// Token: 0x06006BB1 RID: 27569 RVA: 0x002A48D4 File Offset: 0x002A2AD4
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
	// (add) Token: 0x06006BB2 RID: 27570 RVA: 0x002A4938 File Offset: 0x002A2B38
	// (remove) Token: 0x06006BB3 RID: 27571 RVA: 0x002A496C File Offset: 0x002A2B6C
	public static event Action OnSettingsChanged;

	// Token: 0x06006BB4 RID: 27572 RVA: 0x002A499F File Offset: 0x002A2B9F
	public override void Init()
	{
		base.Init();
		XUiC_OptionsVideoSimplified.ID = base.WindowGroup.Id;
		this.initUpscalerModeOptions();
		this.initQualityPresetBasedOptions();
	}

	// Token: 0x06006BB5 RID: 27573 RVA: 0x002A49C3 File Offset: 0x002A2BC3
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateCustomModeVisibility();
	}

	// Token: 0x06006BB6 RID: 27574 RVA: 0x002A49D1 File Offset: 0x002A2BD1
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCustomModeVisibility()
	{
		this.comboQualityPreset.MaxIndex = Math.Max(this.comboQualityPreset.SelectedIndex, 1);
	}

	// Token: 0x06006BB7 RID: 27575 RVA: 0x002A49EF File Offset: 0x002A2BEF
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

	// Token: 0x06006BBE RID: 27582 RVA: 0x002A4AC0 File Offset: 0x002A2CC0
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

	// Token: 0x06006BBF RID: 27583 RVA: 0x002A4AF0 File Offset: 0x002A2CF0
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

	// Token: 0x0400527C RID: 21116
	[XuiBindComponent("OptionConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x0400527D RID: 21117
	[XuiBindComponent("ConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x0400527E RID: 21118
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x0400527F RID: 21119
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x04005280 RID: 21120
	public static string ID = "";

	// Token: 0x02000DCF RID: 3535
	public enum UpscalerMode
	{
		// Token: 0x04005283 RID: 21123
		FSR3,
		// Token: 0x04005284 RID: 21124
		Scale
	}

	// Token: 0x02000DD0 RID: 3536
	public enum GraphicsMode
	{
		// Token: 0x04005286 RID: 21126
		ConsolePerformance,
		// Token: 0x04005287 RID: 21127
		ConsoleQuality,
		// Token: 0x04005288 RID: 21128
		ConsoleCustom
	}
}
