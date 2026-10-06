using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using WorldGenerationEngineFinal;

// Token: 0x02000E65 RID: 3685
[Preserve]
public class XUiC_WorldGenerationWindow : XUiController
{
	// Token: 0x1400009F RID: 159
	// (add) Token: 0x060070F2 RID: 28914 RVA: 0x002BA884 File Offset: 0x002B8A84
	// (remove) Token: 0x060070F3 RID: 28915 RVA: 0x002BA8BC File Offset: 0x002B8ABC
	public event Action OnCountyNameChanged;

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x060070F4 RID: 28916 RVA: 0x002BA8F4 File Offset: 0x002B8AF4
	// (remove) Token: 0x060070F5 RID: 28917 RVA: 0x002BA92C File Offset: 0x002B8B2C
	public event Action OnWorldSizeChanged;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x060070F6 RID: 28918 RVA: 0x002BA964 File Offset: 0x002B8B64
	// (remove) Token: 0x060070F7 RID: 28919 RVA: 0x002BA99C File Offset: 0x002B8B9C
	public event Action OnWorldStorageChanged;

	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x060070F8 RID: 28920 RVA: 0x002BA9D4 File Offset: 0x002B8BD4
	// (remove) Token: 0x060070F9 RID: 28921 RVA: 0x002BAA0C File Offset: 0x002B8C0C
	public event Action<UserDataStorageType> OnGameSaveStorageChanged;

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x060070FA RID: 28922 RVA: 0x002BAA41 File Offset: 0x002B8C41
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

	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x060070FB RID: 28923 RVA: 0x002BAA54 File Offset: 0x002B8C54
	// (set) Token: 0x060070FC RID: 28924 RVA: 0x002BAA67 File Offset: 0x002B8C67
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

	// Token: 0x17000CE6 RID: 3302
	// (get) Token: 0x060070FD RID: 28925 RVA: 0x002BAA7D File Offset: 0x002B8C7D
	// (set) Token: 0x060070FE RID: 28926 RVA: 0x002BAA93 File Offset: 0x002B8C93
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

	// Token: 0x17000CE7 RID: 3303
	// (get) Token: 0x060070FF RID: 28927 RVA: 0x002BAAAA File Offset: 0x002B8CAA
	// (set) Token: 0x06007100 RID: 28928 RVA: 0x002BAAC0 File Offset: 0x002B8CC0
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

	// Token: 0x17000CE8 RID: 3304
	// (get) Token: 0x06007101 RID: 28929 RVA: 0x002BAAD7 File Offset: 0x002B8CD7
	// (set) Token: 0x06007102 RID: 28930 RVA: 0x002BAAED File Offset: 0x002B8CED
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

	// Token: 0x17000CE9 RID: 3305
	// (get) Token: 0x06007103 RID: 28931 RVA: 0x002BAB04 File Offset: 0x002B8D04
	// (set) Token: 0x06007104 RID: 28932 RVA: 0x002BAB17 File Offset: 0x002B8D17
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

	// Token: 0x17000CEA RID: 3306
	// (get) Token: 0x06007105 RID: 28933 RVA: 0x002BAB2D File Offset: 0x002B8D2D
	// (set) Token: 0x06007106 RID: 28934 RVA: 0x002BAB40 File Offset: 0x002B8D40
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

	// Token: 0x17000CEB RID: 3307
	// (get) Token: 0x06007107 RID: 28935 RVA: 0x002BAB56 File Offset: 0x002B8D56
	// (set) Token: 0x06007108 RID: 28936 RVA: 0x002BAB69 File Offset: 0x002B8D69
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

	// Token: 0x17000CEC RID: 3308
	// (get) Token: 0x06007109 RID: 28937 RVA: 0x002BAB7F File Offset: 0x002B8D7F
	// (set) Token: 0x0600710A RID: 28938 RVA: 0x002BAB92 File Offset: 0x002B8D92
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

	// Token: 0x17000CED RID: 3309
	// (get) Token: 0x0600710B RID: 28939 RVA: 0x002BABA8 File Offset: 0x002B8DA8
	// (set) Token: 0x0600710C RID: 28940 RVA: 0x002BABBB File Offset: 0x002B8DBB
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

	// Token: 0x17000CEE RID: 3310
	// (get) Token: 0x0600710D RID: 28941 RVA: 0x002BABD1 File Offset: 0x002B8DD1
	// (set) Token: 0x0600710E RID: 28942 RVA: 0x002BABE4 File Offset: 0x002B8DE4
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

	// Token: 0x17000CEF RID: 3311
	// (get) Token: 0x0600710F RID: 28943 RVA: 0x002BABFA File Offset: 0x002B8DFA
	// (set) Token: 0x06007110 RID: 28944 RVA: 0x002BAC0D File Offset: 0x002B8E0D
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

	// Token: 0x17000CF0 RID: 3312
	// (get) Token: 0x06007111 RID: 28945 RVA: 0x002BAC23 File Offset: 0x002B8E23
	// (set) Token: 0x06007112 RID: 28946 RVA: 0x002BAC36 File Offset: 0x002B8E36
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

	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x06007113 RID: 28947 RVA: 0x002BAC4C File Offset: 0x002B8E4C
	// (set) Token: 0x06007114 RID: 28948 RVA: 0x002BAC5F File Offset: 0x002B8E5F
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

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x06007115 RID: 28949 RVA: 0x002BAC75 File Offset: 0x002B8E75
	public bool HasSufficientSpace
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !SaveInfoProvider.DataLimitEnabled || this.pendingBytes <= this.totalAvailableBytes;
		}
	}

	// Token: 0x17000CF3 RID: 3315
	// (get) Token: 0x06007116 RID: 28950 RVA: 0x002BAC91 File Offset: 0x002B8E91
	public bool IsAdvancedUi
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.windowGroup.Id == XUiC_WorldGenerationWindowGroup.ID;
		}
	}

	// Token: 0x17000CF4 RID: 3316
	// (get) Token: 0x06007117 RID: 28951 RVA: 0x002BACA8 File Offset: 0x002B8EA8
	// (set) Token: 0x06007118 RID: 28952 RVA: 0x002BACB0 File Offset: 0x002B8EB0
	public int WorldSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CF5 RID: 3317
	// (get) Token: 0x06007119 RID: 28953 RVA: 0x002BACB9 File Offset: 0x002B8EB9
	// (set) Token: 0x0600711A RID: 28954 RVA: 0x002BACC1 File Offset: 0x002B8EC1
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

	// Token: 0x17000CF6 RID: 3318
	// (get) Token: 0x0600711B RID: 28955 RVA: 0x002BACD1 File Offset: 0x002B8ED1
	// (set) Token: 0x0600711C RID: 28956 RVA: 0x002BACE2 File Offset: 0x002B8EE2
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

	// Token: 0x17000CF7 RID: 3319
	// (get) Token: 0x0600711D RID: 28957 RVA: 0x002BACF2 File Offset: 0x002B8EF2
	// (set) Token: 0x0600711E RID: 28958 RVA: 0x002BAD03 File Offset: 0x002B8F03
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

	// Token: 0x17000CF8 RID: 3320
	// (get) Token: 0x0600711F RID: 28959 RVA: 0x002BAD13 File Offset: 0x002B8F13
	[XuiXmlBinding("showbar")]
	public bool HasDataManagementBar
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.dataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		}
	}

	// Token: 0x17000CF9 RID: 3321
	// (get) Token: 0x06007120 RID: 28960 RVA: 0x002BAD24 File Offset: 0x002B8F24
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

	// Token: 0x06007121 RID: 28961 RVA: 0x002992B2 File Offset: 0x002974B2
	[XuiXmlBinding("isroamingoptional")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingIsRoamingOptional()
	{
		return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
	}

	// Token: 0x17000CFA RID: 3322
	// (get) Token: 0x06007122 RID: 28962 RVA: 0x002BAD68 File Offset: 0x002B8F68
	// (set) Token: 0x06007123 RID: 28963 RVA: 0x002BAD70 File Offset: 0x002B8F70
	public WorldBuilder WorldBuilder
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CFB RID: 3323
	// (get) Token: 0x06007124 RID: 28964 RVA: 0x002BAD79 File Offset: 0x002B8F79
	// (set) Token: 0x06007125 RID: 28965 RVA: 0x002BAD81 File Offset: 0x002B8F81
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

	// Token: 0x17000CFC RID: 3324
	// (get) Token: 0x06007126 RID: 28966 RVA: 0x002BAD91 File Offset: 0x002B8F91
	// (set) Token: 0x06007127 RID: 28967 RVA: 0x002BAD99 File Offset: 0x002B8F99
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

	// Token: 0x17000CFD RID: 3325
	// (get) Token: 0x06007128 RID: 28968 RVA: 0x002BADAC File Offset: 0x002B8FAC
	public static bool WorldBuilderIsGenerating
	{
		get
		{
			XUiC_WorldGenerationWindow instance = XUiC_WorldGenerationWindow.Instance;
			WorldBuilder worldBuilder = ((instance != null) ? instance.WorldBuilder : null);
			return worldBuilder != null && !worldBuilder.IsFinished;
		}
	}

	// Token: 0x06007129 RID: 28969 RVA: 0x002BADDC File Offset: 0x002B8FDC
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

	// Token: 0x0600712A RID: 28970 RVA: 0x002BAE10 File Offset: 0x002B9010
	[XuiBindEvent("OnPress", "btnNewGame")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void NewGameButton_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.StartCoroutine(this.saveAndNewGameCo());
	}

	// Token: 0x0600712B RID: 28971 RVA: 0x002BAE24 File Offset: 0x002B9024
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

	// Token: 0x0600712C RID: 28972 RVA: 0x002BAE34 File Offset: 0x002B9034
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

	// Token: 0x0600712D RID: 28973 RVA: 0x002BAEB4 File Offset: 0x002B90B4
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

	// Token: 0x0600712E RID: 28974 RVA: 0x002BB114 File Offset: 0x002B9314
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

	// Token: 0x0600712F RID: 28975 RVA: 0x002BB18F File Offset: 0x002B938F
	[XuiBindEvent("ValueChanged", "biomeBoxes")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BiomeWeight_OnValueChanged()
	{
		this.updateBiomePercentages();
	}

	// Token: 0x06007130 RID: 28976 RVA: 0x002BB197 File Offset: 0x002B9397
	[XuiBindEvent("OnValueChanged", "cbxPlainsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlainsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(false);
	}

	// Token: 0x06007131 RID: 28977 RVA: 0x002BB197 File Offset: 0x002B9397
	[XuiBindEvent("OnValueChanged", "cbxHillsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void HillsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(false);
	}

	// Token: 0x06007132 RID: 28978 RVA: 0x002BB1A0 File Offset: 0x002B93A0
	[XuiBindEvent("OnValueChanged", "cbxMountainsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void MountainsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(true);
	}

	// Token: 0x06007133 RID: 28979 RVA: 0x002BB1AC File Offset: 0x002B93AC
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

	// Token: 0x06007134 RID: 28980 RVA: 0x002BB2F4 File Offset: 0x002B94F4
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

	// Token: 0x06007135 RID: 28981 RVA: 0x002BB425 File Offset: 0x002B9625
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

	// Token: 0x06007136 RID: 28982 RVA: 0x002BB444 File Offset: 0x002B9644
	[XuiBindEvent("OnChangeHandler", "txtSeedInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SeedInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.RefreshCountyName();
	}

	// Token: 0x06007137 RID: 28983 RVA: 0x002BB44C File Offset: 0x002B964C
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

	// Token: 0x06007138 RID: 28984 RVA: 0x002BB47B File Offset: 0x002B967B
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

	// Token: 0x06007139 RID: 28985 RVA: 0x002BB4B6 File Offset: 0x002B96B6
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveDataLimit_OnChanged()
	{
		this.validateDataLimit();
	}

	// Token: 0x0600713A RID: 28986 RVA: 0x002BB4BE File Offset: 0x002B96BE
	public void SetGameSaveStorageValue(UserDataStorageType _newValue)
	{
		this.cbxGameSaveStorage.Value = _newValue;
	}

	// Token: 0x0600713B RID: 28987 RVA: 0x002BB4CC File Offset: 0x002B96CC
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

	// Token: 0x0600713C RID: 28988 RVA: 0x002BB500 File Offset: 0x002B9700
	public void ValidateNewRwg()
	{
		string text = this.CountyName;
		bool flag = PathAbstractions.WorldsSearchPaths.GetLocation(text, null, null).Type != PathAbstractions.EAbstractedLocationType.None;
		this.ValidCountyName = !flag;
		this.validateDataLimit();
	}

	// Token: 0x0600713D RID: 28989 RVA: 0x002BB550 File Offset: 0x002B9750
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

	// Token: 0x0600713E RID: 28990 RVA: 0x002BB5A3 File Offset: 0x002B97A3
	[XuiBindEvent("OnPress", "btnManage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnManage_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x0600713F RID: 28991 RVA: 0x002BB5B7 File Offset: 0x002B97B7
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDataManagementWindowClosed()
	{
		this.updateBarValues();
	}

	// Token: 0x06007140 RID: 28992 RVA: 0x002BB5C0 File Offset: 0x002B97C0
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

	// Token: 0x06007141 RID: 28993 RVA: 0x002BB64C File Offset: 0x002B984C
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

	// Token: 0x06007142 RID: 28994 RVA: 0x002BB6A6 File Offset: 0x002B98A6
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

	// Token: 0x06007143 RID: 28995 RVA: 0x002BB6C3 File Offset: 0x002B98C3
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

	// Token: 0x06007144 RID: 28996 RVA: 0x002BB6E0 File Offset: 0x002B98E0
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

	// Token: 0x06007145 RID: 28997 RVA: 0x002BB731 File Offset: 0x002B9931
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

	// Token: 0x06007146 RID: 28998 RVA: 0x002BB740 File Offset: 0x002B9940
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(this.lastWindowID, true);
	}

	// Token: 0x06007147 RID: 28999 RVA: 0x002BB77C File Offset: 0x002B997C
	public override void OnClose()
	{
		base.OnClose();
		this.destroyBuilder();
		PathAbstractions.CacheEnabled = false;
		this.CleanupPreviewManager();
		RenderSettings.fogDensity = this.oldFogDensity;
		SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Remove(SaveDataLimitUIHelper.OnValueChanged, new Action(this.SaveDataLimit_OnChanged));
	}

	// Token: 0x06007148 RID: 29000 RVA: 0x002BB7CC File Offset: 0x002B99CC
	public void CleanupPreviewManager()
	{
		if (this.prefabPreviewManagerInstance == null)
		{
			return;
		}
		this.prefabPreviewManagerInstance.Cleanup();
		this.prefabPreviewManagerInstance = null;
	}

	// Token: 0x06007149 RID: 29001 RVA: 0x002BB7EC File Offset: 0x002B99EC
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

	// Token: 0x0600714A RID: 29002 RVA: 0x002BB86C File Offset: 0x002B9A6C
	public static void Open(XUi _xui, string _previousWindowId)
	{
		XUiC_WorldGenerationWindow childByType = _xui.GetChildByType<XUiC_WorldGenerationWindowGroup>().GetChildByType<XUiC_WorldGenerationWindow>();
		childByType.lastWindowID = _previousWindowId;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, true);
	}

	// Token: 0x0600714B RID: 29003 RVA: 0x002BB8A4 File Offset: 0x002B9AA4
	public static bool IsWindowOpen(XUi _xui)
	{
		XUiC_WorldGenerationWindow childByType = _xui.GetChildByType<XUiC_WorldGenerationWindowGroup>().GetChildByType<XUiC_WorldGenerationWindow>();
		return _xui.playerUI.windowManager.IsWindowOpen(childByType.windowGroup);
	}

	// Token: 0x0400557E RID: 21886
	public static XUiC_WorldGenerationWindow Instance;

	// Token: 0x0400557F RID: 21887
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastWindowID = string.Empty;

	// Token: 0x04005584 RID: 21892
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationPreview previewWindow;

	// Token: 0x04005585 RID: 21893
	[XuiBindComponent("progress", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView progressView;

	// Token: 0x04005586 RID: 21894
	[XuiBindComponent("seedInput", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSeedInput;

	// Token: 0x04005587 RID: 21895
	[XuiBindComponent("generate", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnGenerate;

	// Token: 0x04005588 RID: 21896
	[XuiBindComponent("btnNewGame", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnNewGame;

	// Token: 0x04005589 RID: 21897
	[XuiBindComponent("WorldSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> cbxWorldSize;

	// Token: 0x0400558A RID: 21898
	[XuiBindComponent("SaveDataLimitComboBox", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x0400558B RID: 21899
	[XuiBindComponent("PreviewQuality", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<XUiC_WorldGenerationWindow.PreviewQuality> cbxPreviewQuality;

	// Token: 0x0400558C RID: 21900
	[XuiBindComponent("BiomeLayout", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.BiomeLayout> cbxBiomeLayout;

	// Token: 0x0400558D RID: 21901
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow.XUiC_RwgBiome[] biomeBoxes;

	// Token: 0x0400558E RID: 21902
	[XuiBindComponent("PlainsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxPlainsWeight;

	// Token: 0x0400558F RID: 21903
	[XuiBindComponent("HillsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxHillsWeight;

	// Token: 0x04005590 RID: 21904
	[XuiBindComponent("MountainsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxMountainsWeight;

	// Token: 0x04005591 RID: 21905
	[XuiBindComponent("Rivers", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxRivers;

	// Token: 0x04005592 RID: 21906
	[XuiBindComponent("Craters", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCraters;

	// Token: 0x04005593 RID: 21907
	[XuiBindComponent("Cracks", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCanyons;

	// Token: 0x04005594 RID: 21908
	[XuiBindComponent("Lakes", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxLakes;

	// Token: 0x04005595 RID: 21909
	[XuiBindComponent("Rural", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxRural;

	// Token: 0x04005596 RID: 21910
	[XuiBindComponent("Town", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxTown;

	// Token: 0x04005597 RID: 21911
	[XuiBindComponent("City", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCity;

	// Token: 0x04005598 RID: 21912
	[XuiBindComponent("Towns", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxTowns;

	// Token: 0x04005599 RID: 21913
	[XuiBindComponent("Wilderness", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxWilderness;

	// Token: 0x0400559A RID: 21914
	[XuiBindComponent("btnDataManagement", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManage;

	// Token: 0x0400559B RID: 21915
	[XuiBindComponent("data_bar_controller", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x0400559C RID: 21916
	[XuiBindComponent("GameSaveStorageComboBox", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxGameSaveStorage;

	// Token: 0x0400559D RID: 21917
	[XuiBindComponent("WorldSaveStorage", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxWorldSaveStorage;

	// Token: 0x0400559E RID: 21918
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabPreviewManager prefabPreviewManagerInstance;

	// Token: 0x0400559F RID: 21919
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingBytes;

	// Token: 0x040055A0 RID: 21920
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalAvailableBytes;

	// Token: 0x040055A2 RID: 21922
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validCountyName;

	// Token: 0x040055A3 RID: 21923
	[PublicizedFrom(EAccessModifier.Private)]
	public string countyName;

	// Token: 0x040055A4 RID: 21924
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText;

	// Token: 0x040055A5 RID: 21925
	[PublicizedFrom(EAccessModifier.Private)]
	public float oldFogDensity;

	// Token: 0x040055A7 RID: 21927
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x040055A8 RID: 21928
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] biomeToUiName = new string[] { "xuiPineForest", "xuiBurntForest", "xuiDesert", "xuiSnow", "xuiWasteland" };

	// Token: 0x040055A9 RID: 21929
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isGenerating;

	// Token: 0x040055AA RID: 21930
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCancelling;

	// Token: 0x02000E66 RID: 3686
	public enum PreviewQuality
	{
		// Token: 0x040055AC RID: 21932
		NoPreview,
		// Token: 0x040055AD RID: 21933
		Lowest,
		// Token: 0x040055AE RID: 21934
		Low,
		// Token: 0x040055AF RID: 21935
		Default,
		// Token: 0x040055B0 RID: 21936
		High,
		// Token: 0x040055B1 RID: 21937
		Highest
	}

	// Token: 0x02000E67 RID: 3687
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class XUiC_RwgBiome : XUiController
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x0600714E RID: 29006 RVA: 0x002BB91C File Offset: 0x002B9B1C
		// (remove) Token: 0x0600714F RID: 29007 RVA: 0x002BB954 File Offset: 0x002B9B54
		public event Action ValueChanged;

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06007150 RID: 29008 RVA: 0x002BB989 File Offset: 0x002B9B89
		// (set) Token: 0x06007151 RID: 29009 RVA: 0x002BB991 File Offset: 0x002B9B91
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

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06007152 RID: 29010 RVA: 0x002BB9CB File Offset: 0x002B9BCB
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

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06007153 RID: 29011 RVA: 0x002BB9F0 File Offset: 0x002B9BF0
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

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06007154 RID: 29012 RVA: 0x002BBA3E File Offset: 0x002B9C3E
		public int Value
		{
			get
			{
				return (int)this.combo.Value;
			}
		}

		// Token: 0x06007155 RID: 29013 RVA: 0x002BBA4C File Offset: 0x002B9C4C
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

		// Token: 0x06007156 RID: 29014 RVA: 0x00291183 File Offset: 0x0028F383
		public override void Update(float _dt)
		{
			base.Update(_dt);
			base.handleDirtyUpdateDefault();
		}

		// Token: 0x06007157 RID: 29015 RVA: 0x002BBA5E File Offset: 0x002B9C5E
		public void SetLabel(string _text)
		{
			this.combo.ValueTextOverride = _text;
		}

		// Token: 0x040055B2 RID: 21938
		[XuiBindComponent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_ComboBoxInt combo;

		// Token: 0x040055B4 RID: 21940
		[PublicizedFrom(EAccessModifier.Private)]
		public int biomeIdx;
	}
}
