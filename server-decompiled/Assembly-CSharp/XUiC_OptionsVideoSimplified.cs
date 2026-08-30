using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000DBF RID: 3519
[Preserve]
public class XUiC_OptionsVideoSimplified : XUiC_OptionsVideoBase
{
	// Token: 0x06006BB8 RID: 27576 RVA: 0x002A14C8 File Offset: 0x0029F6C8
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

	// Token: 0x06006BB9 RID: 27577 RVA: 0x002A1544 File Offset: 0x0029F744
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

	// Token: 0x14000098 RID: 152
	// (add) Token: 0x06006BBA RID: 27578 RVA: 0x002A15A8 File Offset: 0x0029F7A8
	// (remove) Token: 0x06006BBB RID: 27579 RVA: 0x002A15DC File Offset: 0x0029F7DC
	public static event Action OnSettingsChanged;

	// Token: 0x06006BBC RID: 27580 RVA: 0x002A160F File Offset: 0x0029F80F
	public override void Init()
	{
		base.Init();
		XUiC_OptionsVideoSimplified.ID = base.WindowGroup.Id;
		this.initUpscalerModeOptions();
		this.initQualityPresetBasedOptions();
	}

	// Token: 0x06006BBD RID: 27581 RVA: 0x002A1633 File Offset: 0x0029F833
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateCustomModeVisibility();
	}

	// Token: 0x06006BBE RID: 27582 RVA: 0x002A1641 File Offset: 0x0029F841
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCustomModeVisibility()
	{
		this.comboQualityPreset.MaxIndex = Math.Max(this.comboQualityPreset.SelectedIndex, 1);
	}

	// Token: 0x06006BBF RID: 27583 RVA: 0x002A165F File Offset: 0x0029F85F
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

	// Token: 0x06006BC6 RID: 27590 RVA: 0x002A1730 File Offset: 0x0029F930
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

	// Token: 0x06006BC7 RID: 27591 RVA: 0x002A1760 File Offset: 0x0029F960
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

	// Token: 0x04005245 RID: 21061
	[XuiBindComponent("OptionConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionUpscaler;

	// Token: 0x04005246 RID: 21062
	[XuiBindComponent("ConsoleUpscaler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboUpscaler;

	// Token: 0x04005247 RID: 21063
	[XuiBindComponent("OptionQualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefIntIndex optionQualityPreset;

	// Token: 0x04005248 RID: 21064
	[XuiBindComponent("QualityPreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<string> comboQualityPreset;

	// Token: 0x04005249 RID: 21065
	public static string ID = "";

	// Token: 0x02000DC0 RID: 3520
	public enum UpscalerMode
	{
		// Token: 0x0400524C RID: 21068
		FSR3,
		// Token: 0x0400524D RID: 21069
		Scale
	}

	// Token: 0x02000DC1 RID: 3521
	public enum GraphicsMode
	{
		// Token: 0x0400524F RID: 21071
		ConsolePerformance,
		// Token: 0x04005250 RID: 21072
		ConsoleQuality,
		// Token: 0x04005251 RID: 21073
		ConsoleCustom
	}
}
