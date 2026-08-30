using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using WorldGenerationEngineFinal;

// Token: 0x02000E41 RID: 3649
[Preserve]
public class XUiC_WorldGenerationWindow : XUiController
{
	// Token: 0x1400009F RID: 159
	// (add) Token: 0x0600705A RID: 28762 RVA: 0x002B3FB0 File Offset: 0x002B21B0
	// (remove) Token: 0x0600705B RID: 28763 RVA: 0x002B3FE8 File Offset: 0x002B21E8
	public event Action OnCountyNameChanged;

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x0600705C RID: 28764 RVA: 0x002B4020 File Offset: 0x002B2220
	// (remove) Token: 0x0600705D RID: 28765 RVA: 0x002B4058 File Offset: 0x002B2258
	public event Action OnWorldSizeChanged;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x0600705E RID: 28766 RVA: 0x002B4090 File Offset: 0x002B2290
	// (remove) Token: 0x0600705F RID: 28767 RVA: 0x002B40C8 File Offset: 0x002B22C8
	public event Action OnWorldStorageChanged;

	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x06007060 RID: 28768 RVA: 0x002B4100 File Offset: 0x002B2300
	// (remove) Token: 0x06007061 RID: 28769 RVA: 0x002B4138 File Offset: 0x002B2338
	public event Action<UserDataStorageType> OnGameSaveStorageChanged;

	// Token: 0x17000CB0 RID: 3248
	// (get) Token: 0x06007062 RID: 28770 RVA: 0x002B416D File Offset: 0x002B236D
	public XUiC_WorldGenerationWindow.PreviewQuality PreviewQualityLevel
	{
		get
		{
			XUiC_ComboBoxEnum<XUiC_WorldGenerationWindow.PreviewQuality> xuiC_ComboBoxEnum = this.cbxPreviewQuality;
			if (xuiC_ComboBoxEnum == null)
			{
				return XUiC_WorldGenerationWindow.PreviewQuality.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
	}

	// Token: 0x17000CB1 RID: 3249
	// (get) Token: 0x06007063 RID: 28771 RVA: 0x002B4180 File Offset: 0x002B2380
	// (set) Token: 0x06007064 RID: 28772 RVA: 0x002B4193 File Offset: 0x002B2393
	public WorldBuilder.BiomeLayout BiomeLayout
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.BiomeLayout> xuiC_ComboBoxEnum = this.cbxBiomeLayout;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.BiomeLayout.CenterForest;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxBiomeLayout != null)
			{
				this.cbxBiomeLayout.Value = value;
			}
		}
	}

	// Token: 0x17000CB2 RID: 3250
	// (get) Token: 0x06007065 RID: 28773 RVA: 0x002B41A9 File Offset: 0x002B23A9
	// (set) Token: 0x06007066 RID: 28774 RVA: 0x002B41BF File Offset: 0x002B23BF
	public int PlainsWeight
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxInt xuiC_ComboBoxInt = this.cbxPlainsWeight;
			return (int)((xuiC_ComboBoxInt != null) ? xuiC_ComboBoxInt.Value : 4L);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxPlainsWeight != null)
			{
				this.cbxPlainsWeight.Value = (long)value;
			}
		}
	}

	// Token: 0x17000CB3 RID: 3251
	// (get) Token: 0x06007067 RID: 28775 RVA: 0x002B41D6 File Offset: 0x002B23D6
	// (set) Token: 0x06007068 RID: 28776 RVA: 0x002B41EC File Offset: 0x002B23EC
	public int HillsWeight
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxInt xuiC_ComboBoxInt = this.cbxHillsWeight;
			return (int)((xuiC_ComboBoxInt != null) ? xuiC_ComboBoxInt.Value : 4L);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxHillsWeight != null)
			{
				this.cbxHillsWeight.Value = (long)value;
			}
		}
	}

	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x06007069 RID: 28777 RVA: 0x002B4203 File Offset: 0x002B2403
	// (set) Token: 0x0600706A RID: 28778 RVA: 0x002B4219 File Offset: 0x002B2419
	public int MountainsWeight
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxInt xuiC_ComboBoxInt = this.cbxMountainsWeight;
			return (int)((xuiC_ComboBoxInt != null) ? xuiC_ComboBoxInt.Value : 2L);
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxMountainsWeight != null)
			{
				this.cbxMountainsWeight.Value = (long)value;
			}
		}
	}

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x0600706B RID: 28779 RVA: 0x002B4230 File Offset: 0x002B2430
	// (set) Token: 0x0600706C RID: 28780 RVA: 0x002B4243 File Offset: 0x002B2443
	public WorldBuilder.GenerationSelections Rivers
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxRivers;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxRivers != null)
			{
				this.cbxRivers.Value = value;
			}
		}
	}

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x0600706D RID: 28781 RVA: 0x002B4259 File Offset: 0x002B2459
	// (set) Token: 0x0600706E RID: 28782 RVA: 0x002B426C File Offset: 0x002B246C
	public WorldBuilder.GenerationSelections Craters
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxCraters;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxCraters != null)
			{
				this.cbxCraters.Value = value;
			}
		}
	}

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x0600706F RID: 28783 RVA: 0x002B4282 File Offset: 0x002B2482
	// (set) Token: 0x06007070 RID: 28784 RVA: 0x002B4295 File Offset: 0x002B2495
	public WorldBuilder.GenerationSelections Canyons
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxCanyons;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxCanyons != null)
			{
				this.cbxCanyons.Value = value;
			}
		}
	}

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x06007071 RID: 28785 RVA: 0x002B42AB File Offset: 0x002B24AB
	// (set) Token: 0x06007072 RID: 28786 RVA: 0x002B42BE File Offset: 0x002B24BE
	public WorldBuilder.GenerationSelections Lakes
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxLakes;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxLakes != null)
			{
				this.cbxLakes.Value = value;
			}
		}
	}

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x06007073 RID: 28787 RVA: 0x002B42D4 File Offset: 0x002B24D4
	// (set) Token: 0x06007074 RID: 28788 RVA: 0x002B42E7 File Offset: 0x002B24E7
	public WorldBuilder.GenerationSelections Rural
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxRural;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxRural != null)
			{
				this.cbxRural.Value = value;
			}
		}
	}

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x06007075 RID: 28789 RVA: 0x002B42FD File Offset: 0x002B24FD
	// (set) Token: 0x06007076 RID: 28790 RVA: 0x002B4310 File Offset: 0x002B2510
	public WorldBuilder.GenerationSelections Town
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxTown;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxTown != null)
			{
				this.cbxTown.Value = value;
			}
		}
	}

	// Token: 0x17000CBB RID: 3259
	// (get) Token: 0x06007077 RID: 28791 RVA: 0x002B4326 File Offset: 0x002B2526
	// (set) Token: 0x06007078 RID: 28792 RVA: 0x002B4339 File Offset: 0x002B2539
	public WorldBuilder.GenerationSelections City
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxCity;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxCity != null)
			{
				this.cbxCity.Value = value;
			}
		}
	}

	// Token: 0x17000CBC RID: 3260
	// (get) Token: 0x06007079 RID: 28793 RVA: 0x002B434F File Offset: 0x002B254F
	// (set) Token: 0x0600707A RID: 28794 RVA: 0x002B4362 File Offset: 0x002B2562
	public WorldBuilder.GenerationSelections Towns
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxTowns;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxTowns != null)
			{
				this.cbxTowns.Value = value;
			}
		}
	}

	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x0600707B RID: 28795 RVA: 0x002B4378 File Offset: 0x002B2578
	// (set) Token: 0x0600707C RID: 28796 RVA: 0x002B438B File Offset: 0x002B258B
	public WorldBuilder.GenerationSelections Wilderness
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> xuiC_ComboBoxEnum = this.cbxWilderness;
			if (xuiC_ComboBoxEnum == null)
			{
				return WorldBuilder.GenerationSelections.Default;
			}
			return xuiC_ComboBoxEnum.Value;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.cbxWilderness != null)
			{
				this.cbxWilderness.Value = value;
			}
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x0600707D RID: 28797 RVA: 0x002B43A1 File Offset: 0x002B25A1
	public bool HasSufficientSpace
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !SaveInfoProvider.DataLimitEnabled || this.pendingBytes <= this.totalAvailableBytes;
		}
	}

	// Token: 0x17000CBF RID: 3263
	// (get) Token: 0x0600707E RID: 28798 RVA: 0x002B43BD File Offset: 0x002B25BD
	public bool IsAdvancedUi
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.windowGroup.Id == XUiC_WorldGenerationWindowGroup.ID;
		}
	}

	// Token: 0x17000CC0 RID: 3264
	// (get) Token: 0x0600707F RID: 28799 RVA: 0x002B43D4 File Offset: 0x002B25D4
	// (set) Token: 0x06007080 RID: 28800 RVA: 0x002B43DC File Offset: 0x002B25DC
	public int WorldSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CC1 RID: 3265
	// (get) Token: 0x06007081 RID: 28801 RVA: 0x002B43E5 File Offset: 0x002B25E5
	// (set) Token: 0x06007082 RID: 28802 RVA: 0x002B43ED File Offset: 0x002B25ED
	[XuiXmlBinding("countynamevalid")]
	public bool ValidCountyName
	{
		get
		{
			return this.validCountyName;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.validCountyName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CC2 RID: 3266
	// (get) Token: 0x06007083 RID: 28803 RVA: 0x002B43FD File Offset: 0x002B25FD
	// (set) Token: 0x06007084 RID: 28804 RVA: 0x002B440E File Offset: 0x002B260E
	[XuiXmlBinding("countyname")]
	public string CountyName
	{
		get
		{
			return this.countyName ?? "";
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.countyName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x06007085 RID: 28805 RVA: 0x002B441E File Offset: 0x002B261E
	// (set) Token: 0x06007086 RID: 28806 RVA: 0x002B442F File Offset: 0x002B262F
	[XuiXmlBinding("statustext")]
	public string StatusText
	{
		get
		{
			return this.statusText ?? "";
		}
		set
		{
			this.statusText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x06007087 RID: 28807 RVA: 0x002B443F File Offset: 0x002B263F
	[XuiXmlBinding("showbar")]
	public bool HasDataManagementBar
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.dataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		}
	}

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x06007088 RID: 28808 RVA: 0x002B4450 File Offset: 0x002B2650
	[XuiXmlBinding("canstartnewgame")]
	public bool CanStartNewGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.IsGenerating)
			{
				WorldBuilder worldBuilder = this.WorldBuilder;
				if (worldBuilder != null && worldBuilder.IsFinished && !worldBuilder.IsCanceled && this.WorldBuilder.CanSaveData())
				{
					return this.HasSufficientSpace;
				}
			}
			return false;
		}
	}

	// Token: 0x06007089 RID: 28809 RVA: 0x00292DD2 File Offset: 0x00290FD2
	[XuiXmlBinding("isroamingoptional")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingIsRoamingOptional()
	{
		return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
	}

	// Token: 0x17000CC6 RID: 3270
	// (get) Token: 0x0600708A RID: 28810 RVA: 0x002B4494 File Offset: 0x002B2694
	// (set) Token: 0x0600708B RID: 28811 RVA: 0x002B449C File Offset: 0x002B269C
	public WorldBuilder WorldBuilder
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CC7 RID: 3271
	// (get) Token: 0x0600708C RID: 28812 RVA: 0x002B44A5 File Offset: 0x002B26A5
	// (set) Token: 0x0600708D RID: 28813 RVA: 0x002B44AD File Offset: 0x002B26AD
	[XuiXmlBinding("isgenerating")]
	public bool IsGenerating
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isGenerating;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.isGenerating = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CC8 RID: 3272
	// (get) Token: 0x0600708E RID: 28814 RVA: 0x002B44BD File Offset: 0x002B26BD
	// (set) Token: 0x0600708F RID: 28815 RVA: 0x002B44C5 File Offset: 0x002B26C5
	[XuiXmlBinding("iscancelling")]
	public bool IsCancelling
	{
		get
		{
			return this.isCancelling;
		}
		set
		{
			this.isCancelling = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000CC9 RID: 3273
	// (get) Token: 0x06007090 RID: 28816 RVA: 0x002B44D8 File Offset: 0x002B26D8
	public static bool WorldBuilderIsGenerating
	{
		get
		{
			XUiC_WorldGenerationWindow instance = XUiC_WorldGenerationWindow.Instance;
			WorldBuilder worldBuilder = ((instance != null) ? instance.WorldBuilder : null);
			return worldBuilder != null && !worldBuilder.IsFinished;
		}
	}

	// Token: 0x06007091 RID: 28817 RVA: 0x002B4508 File Offset: 0x002B2708
	public static void CancelGeneration()
	{
		XUiC_WorldGenerationWindow instance = XUiC_WorldGenerationWindow.Instance;
		WorldBuilder worldBuilder = ((instance != null) ? instance.WorldBuilder : null);
		if (worldBuilder != null)
		{
			worldBuilder.IsCanceled = true;
			XUiC_WorldGenerationWindow.Instance.IsCancelling = true;
		}
	}

	// Token: 0x06007092 RID: 28818 RVA: 0x002B453C File Offset: 0x002B273C
	[XuiBindEvent("OnPress", "btnNewGame")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void NewGameButton_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.StartCoroutine(this.saveAndNewGameCo());
	}

	// Token: 0x06007093 RID: 28819 RVA: 0x002B4550 File Offset: 0x002B2750
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator saveAndNewGameCo()
	{
		if (this.isClosing)
		{
			yield break;
		}
		this.isClosing = true;
		bool shouldClose = false;
		WorldBuilder worldBuilder = this.WorldBuilder;
		WorldBuilder.SaveDataPromptMode saveDataPromptMode = WorldBuilder.SaveDataPromptMode.OnInsufficientSpace;
		XUiV_Window parentWindow = base.GetParentWindow();
		yield return worldBuilder.SaveData(saveDataPromptMode, ((parentWindow != null) ? parentWindow.Controller : null) ?? this, delegate
		{
			shouldClose = false;
		}, null, delegate
		{
			shouldClose = true;
		});
		if (!shouldClose)
		{
			this.isClosing = false;
			yield break;
		}
		GamePrefs.Set(EnumGamePrefs.GameWorld, this.CountyName);
		this.checkProfile(XUiC_NewGame.ID);
		yield break;
	}

	// Token: 0x06007094 RID: 28820 RVA: 0x002B4560 File Offset: 0x002B2760
	[PublicizedFrom(EAccessModifier.Private)]
	public void checkProfile(string _windowToOpen)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (ProfileSDF.CurrentProfileName().Length == 0)
		{
			XUiC_PlayerProfile.Open(this.xui, delegate
			{
				this.xui.playerUI.windowManager.Open(_windowToOpen, true);
			});
			return;
		}
		this.xui.playerUI.windowManager.Open(_windowToOpen, true);
	}

	// Token: 0x06007095 RID: 28821 RVA: 0x002B45E0 File Offset: 0x002B27E0
	public override void OnOpen()
	{
		XUiC_WorldGenerationWindow.Instance = this;
		base.OnOpen();
		this.IsGenerating = false;
		this.IsCancelling = false;
		this.isClosing = false;
		PathAbstractions.CacheEnabled = true;
		if (this.previewWindow != null)
		{
			this.prefabPreviewManagerInstance = new PrefabPreviewManager(this.previewWindow);
		}
		if (this.cbxWorldSize != null)
		{
			if (PlatformOptimizations.EnforceMaxWorldSizeHost)
			{
				int num = this.cbxWorldSize.Elements.FindLastIndex((int _element) => _element <= PlatformOptimizations.MaxWorldSizeHost);
				if (num >= 0)
				{
					this.cbxWorldSize.MinIndex = 0;
					this.cbxWorldSize.MaxIndex = num;
					this.cbxWorldSize.SelectedIndex = num;
				}
			}
			if (this.cbxWorldSize.Elements.Contains(8192))
			{
				this.cbxWorldSize.Value = 8192;
			}
		}
		if (this.cbxWorldSaveStorage != null)
		{
			this.cbxWorldSaveStorage.Value = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);
		}
		if (this.cbxGameSaveStorage != null)
		{
			this.cbxGameSaveStorage.Value = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
		}
		SaveDataLimitUIHelper.AddComboBox(this.cbxSaveDataLimit);
		SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Combine(SaveDataLimitUIHelper.OnValueChanged, new Action(this.SaveDataLimit_OnChanged));
		this.Rivers = WorldBuilder.GenerationSelections.Default;
		this.Craters = WorldBuilder.GenerationSelections.Default;
		this.Canyons = WorldBuilder.GenerationSelections.Default;
		this.Lakes = WorldBuilder.GenerationSelections.Default;
		this.Rural = WorldBuilder.GenerationSelections.Default;
		this.Town = WorldBuilder.GenerationSelections.Default;
		this.City = WorldBuilder.GenerationSelections.Default;
		this.Towns = WorldBuilder.GenerationSelections.Default;
		this.Wilderness = WorldBuilder.GenerationSelections.Default;
		this.PlainsWeight = 4;
		this.HillsWeight = 4;
		this.MountainsWeight = 2;
		this.BiomeLayout = WorldBuilder.BiomeLayout.CenterForest;
		for (int i = 0; i < this.biomeBoxes.Length; i++)
		{
			this.biomeBoxes[i].BiomeIdx = i;
		}
		this.updateTerrainPercentages(false);
		this.updateBiomePercentages();
		if (this.cbxPreviewQuality != null)
		{
			this.cbxPreviewQuality.Value = XUiC_WorldGenerationWindow.PreviewQuality.Default;
			this.cbxPreviewQuality.Max = new XUiC_WorldGenerationWindow.PreviewQuality?(PlatformOptimizations.MaxRWGPreviewQuality);
		}
		if (this.txtSeedInput != null)
		{
			this.SeedInput_OnChangeHandler(this.txtSeedInput, this.txtSeedInput.Text, true);
		}
		if (this.cbxWorldSize != null)
		{
			this.WorldSizeComboBox_OnValueChanged(this.cbxWorldSize, this.cbxWorldSize.Value - 1, this.cbxWorldSize.Value);
		}
		this.oldFogDensity = RenderSettings.fogDensity;
		RenderSettings.fogDensity = 0f;
		this.updateBarValues();
	}

	// Token: 0x06007096 RID: 28822 RVA: 0x002B4840 File Offset: 0x002B2A40
	[XuiBindEvent("OnValueChanged", "cbxPreviewQuality")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Quality_OnValueChanged(XUiController _sender, XUiC_WorldGenerationWindow.PreviewQuality _oldValue, XUiC_WorldGenerationWindow.PreviewQuality _newValue)
	{
		if (this.previewWindow != null)
		{
			this.previewWindow.PreviewInit();
			this.previewWindow.PreviewTextureDraw();
		}
		if (this.prefabPreviewManagerInstance == null || !this.prefabPreviewManagerInstance.Initialized)
		{
			return;
		}
		if (_oldValue >= XUiC_WorldGenerationWindow.PreviewQuality.Low && _oldValue <= XUiC_WorldGenerationWindow.PreviewQuality.High && _newValue >= XUiC_WorldGenerationWindow.PreviewQuality.Low && _newValue <= XUiC_WorldGenerationWindow.PreviewQuality.High)
		{
			return;
		}
		this.prefabPreviewManagerInstance.RemovePrefabs();
		this.prefabPreviewManagerInstance.InitPrefabs();
		this.prefabPreviewManagerInstance.ForceUpdate();
	}

	// Token: 0x06007097 RID: 28823 RVA: 0x002B48BB File Offset: 0x002B2ABB
	[XuiBindEvent("ValueChanged", "biomeBoxes")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BiomeWeight_OnValueChanged()
	{
		this.updateBiomePercentages();
	}

	// Token: 0x06007098 RID: 28824 RVA: 0x002B48C3 File Offset: 0x002B2AC3
	[XuiBindEvent("OnValueChanged", "cbxPlainsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlainsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(false);
	}

	// Token: 0x06007099 RID: 28825 RVA: 0x002B48C3 File Offset: 0x002B2AC3
	[XuiBindEvent("OnValueChanged", "cbxHillsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void HillsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(false);
	}

	// Token: 0x0600709A RID: 28826 RVA: 0x002B48CC File Offset: 0x002B2ACC
	[XuiBindEvent("OnValueChanged", "cbxMountainsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void MountainsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(true);
	}

	// Token: 0x0600709B RID: 28827 RVA: 0x002B48D8 File Offset: 0x002B2AD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTerrainPercentages(bool _isMountainsChanged = false)
	{
		if (this.cbxPlainsWeight == null || this.cbxHillsWeight == null || this.cbxMountainsWeight == null)
		{
			return;
		}
		int num2;
		int num3;
		int num4;
		for (;;)
		{
			float num = (float)(this.PlainsWeight + this.HillsWeight + this.MountainsWeight);
			if (num <= 0f)
			{
				num = 1f;
			}
			num2 = Mathf.RoundToInt((float)this.PlainsWeight / num * 100f);
			num3 = Mathf.RoundToInt((float)this.HillsWeight / num * 100f);
			num4 = Mathf.RoundToInt((float)this.MountainsWeight / num * 100f);
			if (num4 <= 50)
			{
				break;
			}
			if (_isMountainsChanged)
			{
				this.PlainsWeight++;
			}
			else
			{
				this.MountainsWeight--;
			}
		}
		if (num2 + num3 + num4 == 0)
		{
			num2 = 100;
		}
		this.cbxPlainsWeight.ValueTextOverride = string.Format("{0}%", Mathf.Max(0, num2));
		this.cbxHillsWeight.ValueTextOverride = string.Format("{0}%", Mathf.Max(0, num3));
		this.cbxMountainsWeight.ValueTextOverride = string.Format("{0}%", Mathf.Max(0, num4));
		this.cbxPlainsWeight.IsDirty = true;
		this.cbxHillsWeight.IsDirty = true;
		this.cbxMountainsWeight.IsDirty = true;
	}

	// Token: 0x0600709C RID: 28828 RVA: 0x002B4A20 File Offset: 0x002B2C20
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBiomePercentages()
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		for (int i = 0; i < 5; i++)
		{
			XUiC_WorldGenerationWindow.XUiC_RwgBiome xuiC_RwgBiome = ((i < this.biomeBoxes.Length) ? this.biomeBoxes[i] : null);
			if (xuiC_RwgBiome != null)
			{
				float num4 = (float)xuiC_RwgBiome.Value;
				num += num4;
				if (num4 > num2)
				{
					num2 = num4;
					num3 = i;
				}
			}
		}
		int num5 = 0;
		for (int j = 0; j < 5; j++)
		{
			XUiC_WorldGenerationWindow.XUiC_RwgBiome xuiC_RwgBiome2 = ((j < this.biomeBoxes.Length) ? this.biomeBoxes[j] : null);
			if (xuiC_RwgBiome2 != null)
			{
				int num6 = Mathf.RoundToInt((float)xuiC_RwgBiome2.Value / num * 100f);
				num6 = Utils.FastMax(5, num6);
				num5 += num6;
				if (j == 4)
				{
					num6 += 100 - num5;
					if (num6 < 5)
					{
						XUiC_WorldGenerationWindow.XUiC_RwgBiome xuiC_RwgBiome3 = this.biomeBoxes[num3];
						int num7 = Mathf.RoundToInt((float)xuiC_RwgBiome3.Value / num * 100f);
						xuiC_RwgBiome3.SetLabel(string.Format("{0}%", num7 + 5 - num6));
						num6 = 5;
					}
				}
				xuiC_RwgBiome2.SetLabel(string.Format("{0}%", num6));
				xuiC_RwgBiome2.IsDirty = true;
			}
		}
	}

	// Token: 0x0600709D RID: 28829 RVA: 0x002B4B51 File Offset: 0x002B2D51
	[XuiBindEvent("OnValueChanged", "cbxWorldSize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldSizeComboBox_OnValueChanged(XUiController _sender, int _oldValue, int _newValue)
	{
		this.WorldSize = _newValue;
		this.RefreshCountyName();
		Action onWorldSizeChanged = this.OnWorldSizeChanged;
		if (onWorldSizeChanged == null)
		{
			return;
		}
		onWorldSizeChanged();
	}

	// Token: 0x0600709E RID: 28830 RVA: 0x002B4B70 File Offset: 0x002B2D70
	[XuiBindEvent("OnChangeHandler", "txtSeedInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SeedInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.RefreshCountyName();
	}

	// Token: 0x0600709F RID: 28831 RVA: 0x002B4B78 File Offset: 0x002B2D78
	[XuiBindEvent("OnValueChanged", "cbxGameSaveStorage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void GameSaveStorage_OnValueChanged(XUiController _sender, UserDataStorageType _oldValue, UserDataStorageType _newValue)
	{
		GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, (int)_newValue);
		GamePrefs.Set(EnumGamePrefs.UserWorldStorageType, (int)_newValue);
		this.ValidateNewRwg();
		Action<UserDataStorageType> onGameSaveStorageChanged = this.OnGameSaveStorageChanged;
		if (onGameSaveStorageChanged == null)
		{
			return;
		}
		onGameSaveStorageChanged(_newValue);
	}

	// Token: 0x060070A0 RID: 28832 RVA: 0x002B4BA7 File Offset: 0x002B2DA7
	[XuiBindEvent("OnValueChanged", "cbxWorldSaveStorage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldSaveStorage_OnValueChanged(XUiController _sender, UserDataStorageType _oldValue, UserDataStorageType _newValue)
	{
		GamePrefs.Set(EnumGamePrefs.UserWorldStorageType, (int)_newValue);
		WorldBuilder worldBuilder = this.WorldBuilder;
		if (worldBuilder != null)
		{
			worldBuilder.SetStorageType(_newValue);
		}
		this.updateBarValues();
		this.ValidateNewRwg();
		Action onWorldStorageChanged = this.OnWorldStorageChanged;
		if (onWorldStorageChanged == null)
		{
			return;
		}
		onWorldStorageChanged();
	}

	// Token: 0x060070A1 RID: 28833 RVA: 0x002B4BE2 File Offset: 0x002B2DE2
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveDataLimit_OnChanged()
	{
		this.validateDataLimit();
	}

	// Token: 0x060070A2 RID: 28834 RVA: 0x002B4BEA File Offset: 0x002B2DEA
	public void SetGameSaveStorageValue(UserDataStorageType _newValue)
	{
		this.cbxGameSaveStorage.Value = _newValue;
	}

	// Token: 0x060070A3 RID: 28835 RVA: 0x002B4BF8 File Offset: 0x002B2DF8
	public void RefreshCountyName()
	{
		this.CountyName = WorldBuilder.GetGeneratedWorldName(this.txtSeedInput.Text, this.WorldSize);
		this.ValidateNewRwg();
		Action onCountyNameChanged = this.OnCountyNameChanged;
		if (onCountyNameChanged == null)
		{
			return;
		}
		onCountyNameChanged();
	}

	// Token: 0x060070A4 RID: 28836 RVA: 0x002B4C2C File Offset: 0x002B2E2C
	public void ValidateNewRwg()
	{
		string text = this.CountyName;
		bool flag = PathAbstractions.WorldsSearchPaths.GetLocation(text, null, null).Type != PathAbstractions.EAbstractedLocationType.None;
		this.ValidCountyName = !flag;
		this.validateDataLimit();
	}

	// Token: 0x060070A5 RID: 28837 RVA: 0x002B4C7C File Offset: 0x002B2E7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void validateDataLimit()
	{
		if (this.cbxSaveDataLimit == null)
		{
			return;
		}
		bool flag = !((UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType)).UsesDataLimit() || this.cbxSaveDataLimit.Value > SaveDataLimitType.Unlimited;
		this.cbxSaveDataLimit.TextColor = (flag ? Color.white : Color.red);
	}

	// Token: 0x060070A6 RID: 28838 RVA: 0x002B4CCF File Offset: 0x002B2ECF
	[XuiBindEvent("OnPress", "btnManage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnManage_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x060070A7 RID: 28839 RVA: 0x002B4CE3 File Offset: 0x002B2EE3
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDataManagementWindowClosed()
	{
		this.updateBarValues();
	}

	// Token: 0x060070A8 RID: 28840 RVA: 0x002B4CEC File Offset: 0x002B2EEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBarValues()
	{
		if (!this.HasDataManagementBar)
		{
			base.RefreshBindings();
			return;
		}
		SaveInfoProvider instance = SaveInfoProvider.Instance;
		this.dataManagementBar.SetUsedBytes(instance.TotalUsedBytes);
		this.dataManagementBar.SetAllowanceBytes(instance.TotalAllowanceBytes);
		this.dataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Preview);
		WorldBuilder worldBuilder = this.WorldBuilder;
		this.pendingBytes = ((worldBuilder != null) ? worldBuilder.SerializedSize : 0L);
		this.totalAvailableBytes = instance.TotalAvailableBytes;
		this.dataManagementBar.SetPendingBytes(this.pendingBytes);
		base.RefreshBindings();
	}

	// Token: 0x060070A9 RID: 28841 RVA: 0x002B4D78 File Offset: 0x002B2F78
	[XuiBindEvent("OnPress", "btnGenerate")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void GenerateButton_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.IsGenerating)
		{
			if (this.WorldBuilder != null)
			{
				this.WorldBuilder.IsCanceled = true;
			}
			return;
		}
		XUiC_WorldGenerationPreview xuiC_WorldGenerationPreview = this.previewWindow;
		if (xuiC_WorldGenerationPreview != null)
		{
			xuiC_WorldGenerationPreview.TerrainCleanup();
		}
		PrefabPreviewManager prefabPreviewManager = this.prefabPreviewManagerInstance;
		if (prefabPreviewManager != null)
		{
			prefabPreviewManager.ClearOldPreview();
		}
		ThreadManager.StartCoroutine(this.GenerateCo(null, null));
	}

	// Token: 0x060070AA RID: 28842 RVA: 0x002B4DD2 File Offset: 0x002B2FD2
	public IEnumerator GenerateCo(Action<string> _onSuccess = null, Action _onFailure = null)
	{
		this.IsGenerating = true;
		this.IsCancelling = false;
		this.updateBarValues();
		this.WorldSize = this.cbxWorldSize.Value;
		this.destroyBuilder();
		UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);
		this.WorldBuilder = new WorldBuilder(this.txtSeedInput.Text, this.WorldSize, @int)
		{
			PreviewWindow = this.previewWindow
		};
		XUiC_WorldGenerationPreview.WorldBuilder = this.WorldBuilder;
		this.WorldBuilder.Towns = this.Towns;
		this.WorldBuilder.Wilderness = this.Wilderness;
		this.WorldBuilder.Rivers = this.Rivers;
		this.WorldBuilder.Craters = this.Craters;
		this.WorldBuilder.Canyons = this.Canyons;
		this.WorldBuilder.Lakes = this.Lakes;
		this.WorldBuilder.Plains = this.PlainsWeight;
		this.WorldBuilder.Hills = this.HillsWeight;
		this.WorldBuilder.Mountains = this.MountainsWeight;
		this.WorldBuilder.biomeLayout = this.BiomeLayout;
		for (int i = 0; i < 5; i++)
		{
			XUiC_WorldGenerationWindow.XUiC_RwgBiome xuiC_RwgBiome = ((i < this.biomeBoxes.Length) ? this.biomeBoxes[i] : null);
			if (xuiC_RwgBiome != null)
			{
				this.WorldBuilder.SetBiomeWeight((BiomeType)i, xuiC_RwgBiome.Value);
			}
		}
		if (this.prefabPreviewManagerInstance != null)
		{
			this.prefabPreviewManagerInstance.ReadyToDisplay = false;
		}
		this.updateBarValues();
		yield return GCUtils.WaitForIdle();
		yield return this.WorldBuilder.GenerateFromUI();
		if (this.previewWindow != null)
		{
			yield return this.WorldBuilder.FinishForPreview();
			if (!this.WorldBuilder.IsCanceled)
			{
				yield return this.previewWindow.ShowPreview(XUiC_WorldGenerationPreview.PreviewStep.Done);
				yield return new WaitForSeconds(2f);
			}
		}
		else
		{
			XUiC_WorldGenerationWindow.<>c__DisplayClass152_0 CS$<>8__locals1 = new XUiC_WorldGenerationWindow.<>c__DisplayClass152_0();
			XUiC_ProgressWindow.Close(LocalPlayerUI.primaryUI);
			CS$<>8__locals1.success = false;
			WorldBuilder worldBuilder = this.WorldBuilder;
			WorldBuilder.SaveDataPromptMode saveDataPromptMode = WorldBuilder.SaveDataPromptMode.OnInsufficientSpace;
			XUiV_Window parentWindow = base.GetParentWindow();
			yield return worldBuilder.SaveData(saveDataPromptMode, ((parentWindow != null) ? parentWindow.Controller : null) ?? this, null, delegate
			{
				CS$<>8__locals1.success = false;
			}, delegate
			{
				CS$<>8__locals1.success = true;
			});
			if (CS$<>8__locals1.success)
			{
				if (_onSuccess != null)
				{
					_onSuccess(this.WorldBuilder.WorldName);
				}
			}
			else if (_onFailure != null)
			{
				_onFailure();
			}
			this.destroyBuilder();
			CS$<>8__locals1 = null;
		}
		XUiC_ProgressWindow.Close(LocalPlayerUI.primaryUI);
		if (this.prefabPreviewManagerInstance != null)
		{
			this.prefabPreviewManagerInstance.ReadyToDisplay = true;
		}
		this.IsCancelling = false;
		this.IsGenerating = false;
		this.StatusText = "";
		this.updateBarValues();
		yield break;
	}

	// Token: 0x060070AB RID: 28843 RVA: 0x002B4DEF File Offset: 0x002B2FEF
	[PublicizedFrom(EAccessModifier.Private)]
	public void destroyBuilder()
	{
		if (this.WorldBuilder == null)
		{
			return;
		}
		this.WorldBuilder.Cleanup();
		this.WorldBuilder = null;
	}

	// Token: 0x060070AC RID: 28844 RVA: 0x002B4E0C File Offset: 0x002B300C
	public void StartClose()
	{
		if (this.IsGenerating)
		{
			return;
		}
		if (this.WorldBuilder == null || !this.WorldBuilder.IsFinished || !this.WorldBuilder.CanSaveData())
		{
			this.close();
			return;
		}
		this.xui.StartCoroutine(this.startCloseCo());
	}

	// Token: 0x060070AD RID: 28845 RVA: 0x002B4E5D File Offset: 0x002B305D
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startCloseCo()
	{
		if (this.isClosing)
		{
			yield break;
		}
		this.isClosing = true;
		bool shouldClose = false;
		WorldBuilder worldBuilder = this.WorldBuilder;
		WorldBuilder.SaveDataPromptMode saveDataPromptMode = WorldBuilder.SaveDataPromptMode.On;
		XUiV_Window parentWindow = base.GetParentWindow();
		yield return worldBuilder.SaveData(saveDataPromptMode, ((parentWindow != null) ? parentWindow.Controller : null) ?? this, delegate
		{
			shouldClose = false;
		}, delegate
		{
			shouldClose = true;
		}, delegate
		{
			shouldClose = true;
		});
		if (!shouldClose)
		{
			this.updateBarValues();
			this.isClosing = false;
			yield break;
		}
		this.close();
		yield break;
	}

	// Token: 0x060070AE RID: 28846 RVA: 0x002B4E6C File Offset: 0x002B306C
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(this.lastWindowID, true);
	}

	// Token: 0x060070AF RID: 28847 RVA: 0x002B4EA8 File Offset: 0x002B30A8
	public override void OnClose()
	{
		base.OnClose();
		this.destroyBuilder();
		PathAbstractions.CacheEnabled = false;
		this.CleanupPreviewManager();
		RenderSettings.fogDensity = this.oldFogDensity;
		SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Remove(SaveDataLimitUIHelper.OnValueChanged, new Action(this.SaveDataLimit_OnChanged));
	}

	// Token: 0x060070B0 RID: 28848 RVA: 0x002B4EF8 File Offset: 0x002B30F8
	public void CleanupPreviewManager()
	{
		if (this.prefabPreviewManagerInstance == null)
		{
			return;
		}
		this.prefabPreviewManagerInstance.Cleanup();
		this.prefabPreviewManagerInstance = null;
	}

	// Token: 0x060070B1 RID: 28849 RVA: 0x002B4F18 File Offset: 0x002B3118
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (!this.IsAdvancedUi)
		{
			return;
		}
		if (this.progressView != null && XUiUtils.HotkeysAllowedFor(this.progressView, false) && PlatformManager.NativePlatform.Input.PrimaryPlayer.GUIActions.Cancel.WasReleased)
		{
			XUiC_WorldGenerationWindow.CancelGeneration();
		}
		if (this.prefabPreviewManagerInstance != null && this.prefabPreviewManagerInstance.ReadyToDisplay)
		{
			this.prefabPreviewManagerInstance.Update();
		}
	}

	// Token: 0x060070B2 RID: 28850 RVA: 0x002B4F98 File Offset: 0x002B3198
	public static void Open(XUi _xui, string _previousWindowId)
	{
		XUiC_WorldGenerationWindow childByType = _xui.GetChildByType<XUiC_WorldGenerationWindowGroup>().GetChildByType<XUiC_WorldGenerationWindow>();
		childByType.lastWindowID = _previousWindowId;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, true);
	}

	// Token: 0x060070B3 RID: 28851 RVA: 0x002B4FD0 File Offset: 0x002B31D0
	public static bool IsWindowOpen(XUi _xui)
	{
		XUiC_WorldGenerationWindow childByType = _xui.GetChildByType<XUiC_WorldGenerationWindowGroup>().GetChildByType<XUiC_WorldGenerationWindow>();
		return _xui.playerUI.windowManager.IsWindowOpen(childByType.windowGroup);
	}

	// Token: 0x040054A3 RID: 21667
	public static XUiC_WorldGenerationWindow Instance;

	// Token: 0x040054A4 RID: 21668
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastWindowID = string.Empty;

	// Token: 0x040054A9 RID: 21673
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationPreview previewWindow;

	// Token: 0x040054AA RID: 21674
	[XuiBindComponent("progress", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView progressView;

	// Token: 0x040054AB RID: 21675
	[XuiBindComponent("seedInput", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSeedInput;

	// Token: 0x040054AC RID: 21676
	[XuiBindComponent("generate", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnGenerate;

	// Token: 0x040054AD RID: 21677
	[XuiBindComponent("btnNewGame", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnNewGame;

	// Token: 0x040054AE RID: 21678
	[XuiBindComponent("WorldSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> cbxWorldSize;

	// Token: 0x040054AF RID: 21679
	[XuiBindComponent("SaveDataLimitComboBox", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x040054B0 RID: 21680
	[XuiBindComponent("PreviewQuality", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<XUiC_WorldGenerationWindow.PreviewQuality> cbxPreviewQuality;

	// Token: 0x040054B1 RID: 21681
	[XuiBindComponent("BiomeLayout", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.BiomeLayout> cbxBiomeLayout;

	// Token: 0x040054B2 RID: 21682
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow.XUiC_RwgBiome[] biomeBoxes;

	// Token: 0x040054B3 RID: 21683
	[XuiBindComponent("PlainsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxPlainsWeight;

	// Token: 0x040054B4 RID: 21684
	[XuiBindComponent("HillsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxHillsWeight;

	// Token: 0x040054B5 RID: 21685
	[XuiBindComponent("MountainsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxMountainsWeight;

	// Token: 0x040054B6 RID: 21686
	[XuiBindComponent("Rivers", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxRivers;

	// Token: 0x040054B7 RID: 21687
	[XuiBindComponent("Craters", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCraters;

	// Token: 0x040054B8 RID: 21688
	[XuiBindComponent("Cracks", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCanyons;

	// Token: 0x040054B9 RID: 21689
	[XuiBindComponent("Lakes", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxLakes;

	// Token: 0x040054BA RID: 21690
	[XuiBindComponent("Rural", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxRural;

	// Token: 0x040054BB RID: 21691
	[XuiBindComponent("Town", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxTown;

	// Token: 0x040054BC RID: 21692
	[XuiBindComponent("City", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCity;

	// Token: 0x040054BD RID: 21693
	[XuiBindComponent("Towns", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxTowns;

	// Token: 0x040054BE RID: 21694
	[XuiBindComponent("Wilderness", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxWilderness;

	// Token: 0x040054BF RID: 21695
	[XuiBindComponent("btnDataManagement", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManage;

	// Token: 0x040054C0 RID: 21696
	[XuiBindComponent("data_bar_controller", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x040054C1 RID: 21697
	[XuiBindComponent("GameSaveStorageComboBox", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxGameSaveStorage;

	// Token: 0x040054C2 RID: 21698
	[XuiBindComponent("WorldSaveStorage", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxWorldSaveStorage;

	// Token: 0x040054C3 RID: 21699
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabPreviewManager prefabPreviewManagerInstance;

	// Token: 0x040054C4 RID: 21700
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingBytes;

	// Token: 0x040054C5 RID: 21701
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalAvailableBytes;

	// Token: 0x040054C7 RID: 21703
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validCountyName;

	// Token: 0x040054C8 RID: 21704
	[PublicizedFrom(EAccessModifier.Private)]
	public string countyName;

	// Token: 0x040054C9 RID: 21705
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText;

	// Token: 0x040054CA RID: 21706
	[PublicizedFrom(EAccessModifier.Private)]
	public float oldFogDensity;

	// Token: 0x040054CC RID: 21708
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x040054CD RID: 21709
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] biomeToUiName = new string[] { "xuiPineForest", "xuiBurntForest", "xuiDesert", "xuiSnow", "xuiWasteland" };

	// Token: 0x040054CE RID: 21710
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isGenerating;

	// Token: 0x040054CF RID: 21711
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCancelling;

	// Token: 0x02000E42 RID: 3650
	public enum PreviewQuality
	{
		// Token: 0x040054D1 RID: 21713
		NoPreview,
		// Token: 0x040054D2 RID: 21714
		Lowest,
		// Token: 0x040054D3 RID: 21715
		Low,
		// Token: 0x040054D4 RID: 21716
		Default,
		// Token: 0x040054D5 RID: 21717
		High,
		// Token: 0x040054D6 RID: 21718
		Highest
	}

	// Token: 0x02000E43 RID: 3651
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class XUiC_RwgBiome : XUiController
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x060070B6 RID: 28854 RVA: 0x002B5048 File Offset: 0x002B3248
		// (remove) Token: 0x060070B7 RID: 28855 RVA: 0x002B5080 File Offset: 0x002B3280
		public event Action ValueChanged;

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060070B8 RID: 28856 RVA: 0x002B50B5 File Offset: 0x002B32B5
		// (set) Token: 0x060070B9 RID: 28857 RVA: 0x002B50BD File Offset: 0x002B32BD
		public int BiomeIdx
		{
			get
			{
				return this.biomeIdx;
			}
			set
			{
				this.biomeIdx = value;
				this.combo.Value = (long)((this.BiomeIdx < WorldBuilderConstants.BiomeWeightDefaults.Length) ? WorldBuilderConstants.BiomeWeightDefaults[this.BiomeIdx] : 0);
				this.IsDirty = true;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060070BA RID: 28858 RVA: 0x002B50F7 File Offset: 0x002B32F7
		[XuiXmlBinding("biomename")]
		public string BiomeName
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this.BiomeIdx >= XUiC_WorldGenerationWindow.biomeToUiName.Length)
				{
					return "";
				}
				return XUiC_WorldGenerationWindow.biomeToUiName[this.BiomeIdx];
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060070BB RID: 28859 RVA: 0x002B511C File Offset: 0x002B331C
		[XuiXmlBinding("biomecolor")]
		public Color BiomeColor
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (this.BiomeIdx >= WorldBuilderConstants.biomeColorList.Count)
				{
					return Color.clear;
				}
				return (WorldBuilderConstants.biomeColorList[this.BiomeIdx] * 0.7f).WithAlpha(1f);
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x060070BC RID: 28860 RVA: 0x002B516A File Offset: 0x002B336A
		public int Value
		{
			get
			{
				return (int)this.combo.Value;
			}
		}

		// Token: 0x060070BD RID: 28861 RVA: 0x002B5178 File Offset: 0x002B3378
		[XuiBindEvent("OnValueChanged", "combo")]
		[PublicizedFrom(EAccessModifier.Private)]
		public void comboValueChanged(XUiController _sender, long _oldValue, long _newValue)
		{
			Action valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged();
		}

		// Token: 0x060070BE RID: 28862 RVA: 0x0028AD37 File Offset: 0x00288F37
		public override void Update(float _dt)
		{
			base.Update(_dt);
			base.handleDirtyUpdateDefault();
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x002B518A File Offset: 0x002B338A
		public void SetLabel(string _text)
		{
			this.combo.ValueTextOverride = _text;
		}

		// Token: 0x040054D7 RID: 21719
		[XuiBindComponent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_ComboBoxInt combo;

		// Token: 0x040054D9 RID: 21721
		[PublicizedFrom(EAccessModifier.Private)]
		public int biomeIdx;
	}
}
