using System;
using System.Collections;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using WorldGenerationEngineFinal;

// Token: 0x02000E70 RID: 3696
[Preserve]
public class XUiC_WorldGenerationWindow : XUiController
{
	// Token: 0x1400009F RID: 159
	// (add) Token: 0x06007117 RID: 28951 RVA: 0x002BBDF0 File Offset: 0x002B9FF0
	// (remove) Token: 0x06007118 RID: 28952 RVA: 0x002BBE28 File Offset: 0x002BA028
	public event Action OnCountyNameChanged;

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x06007119 RID: 28953 RVA: 0x002BBE60 File Offset: 0x002BA060
	// (remove) Token: 0x0600711A RID: 28954 RVA: 0x002BBE98 File Offset: 0x002BA098
	public event Action OnWorldSizeChanged;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x0600711B RID: 28955 RVA: 0x002BBED0 File Offset: 0x002BA0D0
	// (remove) Token: 0x0600711C RID: 28956 RVA: 0x002BBF08 File Offset: 0x002BA108
	public event Action OnWorldStorageChanged;

	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x0600711D RID: 28957 RVA: 0x002BBF40 File Offset: 0x002BA140
	// (remove) Token: 0x0600711E RID: 28958 RVA: 0x002BBF78 File Offset: 0x002BA178
	public event Action<UserDataStorageType> OnGameSaveStorageChanged;

	// Token: 0x17000CEA RID: 3306
	// (get) Token: 0x0600711F RID: 28959 RVA: 0x002BBFAD File Offset: 0x002BA1AD
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

	// Token: 0x17000CEB RID: 3307
	// (get) Token: 0x06007120 RID: 28960 RVA: 0x002BBFC0 File Offset: 0x002BA1C0
	// (set) Token: 0x06007121 RID: 28961 RVA: 0x002BBFD3 File Offset: 0x002BA1D3
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

	// Token: 0x17000CEC RID: 3308
	// (get) Token: 0x06007122 RID: 28962 RVA: 0x002BBFE9 File Offset: 0x002BA1E9
	// (set) Token: 0x06007123 RID: 28963 RVA: 0x002BBFFF File Offset: 0x002BA1FF
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

	// Token: 0x17000CED RID: 3309
	// (get) Token: 0x06007124 RID: 28964 RVA: 0x002BC016 File Offset: 0x002BA216
	// (set) Token: 0x06007125 RID: 28965 RVA: 0x002BC02C File Offset: 0x002BA22C
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

	// Token: 0x17000CEE RID: 3310
	// (get) Token: 0x06007126 RID: 28966 RVA: 0x002BC043 File Offset: 0x002BA243
	// (set) Token: 0x06007127 RID: 28967 RVA: 0x002BC059 File Offset: 0x002BA259
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

	// Token: 0x17000CEF RID: 3311
	// (get) Token: 0x06007128 RID: 28968 RVA: 0x002BC070 File Offset: 0x002BA270
	// (set) Token: 0x06007129 RID: 28969 RVA: 0x002BC083 File Offset: 0x002BA283
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

	// Token: 0x17000CF0 RID: 3312
	// (get) Token: 0x0600712A RID: 28970 RVA: 0x002BC099 File Offset: 0x002BA299
	// (set) Token: 0x0600712B RID: 28971 RVA: 0x002BC0AC File Offset: 0x002BA2AC
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

	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x0600712C RID: 28972 RVA: 0x002BC0C2 File Offset: 0x002BA2C2
	// (set) Token: 0x0600712D RID: 28973 RVA: 0x002BC0D5 File Offset: 0x002BA2D5
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

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x0600712E RID: 28974 RVA: 0x002BC0EB File Offset: 0x002BA2EB
	// (set) Token: 0x0600712F RID: 28975 RVA: 0x002BC0FE File Offset: 0x002BA2FE
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

	// Token: 0x17000CF3 RID: 3315
	// (get) Token: 0x06007130 RID: 28976 RVA: 0x002BC114 File Offset: 0x002BA314
	// (set) Token: 0x06007131 RID: 28977 RVA: 0x002BC127 File Offset: 0x002BA327
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

	// Token: 0x17000CF4 RID: 3316
	// (get) Token: 0x06007132 RID: 28978 RVA: 0x002BC13D File Offset: 0x002BA33D
	// (set) Token: 0x06007133 RID: 28979 RVA: 0x002BC150 File Offset: 0x002BA350
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

	// Token: 0x17000CF5 RID: 3317
	// (get) Token: 0x06007134 RID: 28980 RVA: 0x002BC166 File Offset: 0x002BA366
	// (set) Token: 0x06007135 RID: 28981 RVA: 0x002BC179 File Offset: 0x002BA379
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

	// Token: 0x17000CF6 RID: 3318
	// (get) Token: 0x06007136 RID: 28982 RVA: 0x002BC18F File Offset: 0x002BA38F
	// (set) Token: 0x06007137 RID: 28983 RVA: 0x002BC1A2 File Offset: 0x002BA3A2
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

	// Token: 0x17000CF7 RID: 3319
	// (get) Token: 0x06007138 RID: 28984 RVA: 0x002BC1B8 File Offset: 0x002BA3B8
	// (set) Token: 0x06007139 RID: 28985 RVA: 0x002BC1CB File Offset: 0x002BA3CB
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

	// Token: 0x17000CF8 RID: 3320
	// (get) Token: 0x0600713A RID: 28986 RVA: 0x002BC1E1 File Offset: 0x002BA3E1
	public bool HasSufficientSpace
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !SaveInfoProvider.DataLimitEnabled || this.pendingBytes <= this.totalAvailableBytes;
		}
	}

	// Token: 0x17000CF9 RID: 3321
	// (get) Token: 0x0600713B RID: 28987 RVA: 0x002BC1FD File Offset: 0x002BA3FD
	public bool IsAdvancedUi
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.windowGroup.Id == XUiC_WorldGenerationWindowGroup.ID;
		}
	}

	// Token: 0x17000CFA RID: 3322
	// (get) Token: 0x0600713C RID: 28988 RVA: 0x002BC214 File Offset: 0x002BA414
	// (set) Token: 0x0600713D RID: 28989 RVA: 0x002BC21C File Offset: 0x002BA41C
	public int WorldSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CFB RID: 3323
	// (get) Token: 0x0600713E RID: 28990 RVA: 0x002BC225 File Offset: 0x002BA425
	// (set) Token: 0x0600713F RID: 28991 RVA: 0x002BC22D File Offset: 0x002BA42D
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

	// Token: 0x17000CFC RID: 3324
	// (get) Token: 0x06007140 RID: 28992 RVA: 0x002BC23D File Offset: 0x002BA43D
	// (set) Token: 0x06007141 RID: 28993 RVA: 0x002BC24E File Offset: 0x002BA44E
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

	// Token: 0x17000CFD RID: 3325
	// (get) Token: 0x06007142 RID: 28994 RVA: 0x002BC25E File Offset: 0x002BA45E
	// (set) Token: 0x06007143 RID: 28995 RVA: 0x002BC26F File Offset: 0x002BA46F
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

	// Token: 0x17000CFE RID: 3326
	// (get) Token: 0x06007144 RID: 28996 RVA: 0x002BC27F File Offset: 0x002BA47F
	[XuiXmlBinding("showbar")]
	public bool HasDataManagementBar
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.dataManagementBar != null && SaveInfoProvider.DataLimitEnabled;
		}
	}

	// Token: 0x17000CFF RID: 3327
	// (get) Token: 0x06007145 RID: 28997 RVA: 0x002BC290 File Offset: 0x002BA490
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

	// Token: 0x06007146 RID: 28998 RVA: 0x0029A7DA File Offset: 0x002989DA
	[XuiXmlBinding("isroamingoptional")]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingIsRoamingOptional()
	{
		return PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional;
	}

	// Token: 0x17000D00 RID: 3328
	// (get) Token: 0x06007147 RID: 28999 RVA: 0x002BC2D4 File Offset: 0x002BA4D4
	// (set) Token: 0x06007148 RID: 29000 RVA: 0x002BC2DC File Offset: 0x002BA4DC
	public WorldBuilder WorldBuilder
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D01 RID: 3329
	// (get) Token: 0x06007149 RID: 29001 RVA: 0x002BC2E5 File Offset: 0x002BA4E5
	// (set) Token: 0x0600714A RID: 29002 RVA: 0x002BC2ED File Offset: 0x002BA4ED
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

	// Token: 0x17000D02 RID: 3330
	// (get) Token: 0x0600714B RID: 29003 RVA: 0x002BC2FD File Offset: 0x002BA4FD
	// (set) Token: 0x0600714C RID: 29004 RVA: 0x002BC305 File Offset: 0x002BA505
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

	// Token: 0x17000D03 RID: 3331
	// (get) Token: 0x0600714D RID: 29005 RVA: 0x002BC318 File Offset: 0x002BA518
	public static bool WorldBuilderIsGenerating
	{
		get
		{
			XUiC_WorldGenerationWindow instance = XUiC_WorldGenerationWindow.Instance;
			WorldBuilder worldBuilder = ((instance != null) ? instance.WorldBuilder : null);
			return worldBuilder != null && !worldBuilder.IsFinished;
		}
	}

	// Token: 0x0600714E RID: 29006 RVA: 0x002BC348 File Offset: 0x002BA548
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

	// Token: 0x0600714F RID: 29007 RVA: 0x002BC37C File Offset: 0x002BA57C
	[XuiBindEvent("OnPress", "btnNewGame")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void NewGameButton_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.StartCoroutine(this.saveAndNewGameCo());
	}

	// Token: 0x06007150 RID: 29008 RVA: 0x002BC390 File Offset: 0x002BA590
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

	// Token: 0x06007151 RID: 29009 RVA: 0x002BC3A0 File Offset: 0x002BA5A0
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

	// Token: 0x06007152 RID: 29010 RVA: 0x002BC420 File Offset: 0x002BA620
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

	// Token: 0x06007153 RID: 29011 RVA: 0x002BC680 File Offset: 0x002BA880
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

	// Token: 0x06007154 RID: 29012 RVA: 0x002BC6FB File Offset: 0x002BA8FB
	[XuiBindEvent("ValueChanged", "biomeBoxes")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BiomeWeight_OnValueChanged()
	{
		this.updateBiomePercentages();
	}

	// Token: 0x06007155 RID: 29013 RVA: 0x002BC703 File Offset: 0x002BA903
	[XuiBindEvent("OnValueChanged", "cbxPlainsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlainsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(false);
	}

	// Token: 0x06007156 RID: 29014 RVA: 0x002BC703 File Offset: 0x002BA903
	[XuiBindEvent("OnValueChanged", "cbxHillsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void HillsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(false);
	}

	// Token: 0x06007157 RID: 29015 RVA: 0x002BC70C File Offset: 0x002BA90C
	[XuiBindEvent("OnValueChanged", "cbxMountainsWeight")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void MountainsWeight_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.updateTerrainPercentages(true);
	}

	// Token: 0x06007158 RID: 29016 RVA: 0x002BC718 File Offset: 0x002BA918
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

	// Token: 0x06007159 RID: 29017 RVA: 0x002BC860 File Offset: 0x002BAA60
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

	// Token: 0x0600715A RID: 29018 RVA: 0x002BC991 File Offset: 0x002BAB91
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

	// Token: 0x0600715B RID: 29019 RVA: 0x002BC9B0 File Offset: 0x002BABB0
	[XuiBindEvent("OnChangeHandler", "txtSeedInput")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SeedInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.RefreshCountyName();
	}

	// Token: 0x0600715C RID: 29020 RVA: 0x002BC9B8 File Offset: 0x002BABB8
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

	// Token: 0x0600715D RID: 29021 RVA: 0x002BC9E7 File Offset: 0x002BABE7
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

	// Token: 0x0600715E RID: 29022 RVA: 0x002BCA22 File Offset: 0x002BAC22
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveDataLimit_OnChanged()
	{
		this.validateDataLimit();
	}

	// Token: 0x0600715F RID: 29023 RVA: 0x002BCA2A File Offset: 0x002BAC2A
	public void SetGameSaveStorageValue(UserDataStorageType _newValue)
	{
		this.cbxGameSaveStorage.Value = _newValue;
	}

	// Token: 0x06007160 RID: 29024 RVA: 0x002BCA38 File Offset: 0x002BAC38
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

	// Token: 0x06007161 RID: 29025 RVA: 0x002BCA6C File Offset: 0x002BAC6C
	public void ValidateNewRwg()
	{
		string text = this.CountyName;
		bool flag = PathAbstractions.WorldsSearchPaths.GetLocation(text, null, null).Type != PathAbstractions.EAbstractedLocationType.None;
		this.ValidCountyName = !flag;
		this.validateDataLimit();
	}

	// Token: 0x06007162 RID: 29026 RVA: 0x002BCABC File Offset: 0x002BACBC
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

	// Token: 0x06007163 RID: 29027 RVA: 0x002BCB0F File Offset: 0x002BAD0F
	[XuiBindEvent("OnPress", "btnManage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnManage_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x06007164 RID: 29028 RVA: 0x002BCB23 File Offset: 0x002BAD23
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDataManagementWindowClosed()
	{
		this.updateBarValues();
	}

	// Token: 0x06007165 RID: 29029 RVA: 0x002BCB2C File Offset: 0x002BAD2C
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

	// Token: 0x06007166 RID: 29030 RVA: 0x002BCBB8 File Offset: 0x002BADB8
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

	// Token: 0x06007167 RID: 29031 RVA: 0x002BCC12 File Offset: 0x002BAE12
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

	// Token: 0x06007168 RID: 29032 RVA: 0x002BCC2F File Offset: 0x002BAE2F
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

	// Token: 0x06007169 RID: 29033 RVA: 0x002BCC4C File Offset: 0x002BAE4C
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

	// Token: 0x0600716A RID: 29034 RVA: 0x002BCC9D File Offset: 0x002BAE9D
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

	// Token: 0x0600716B RID: 29035 RVA: 0x002BCCAC File Offset: 0x002BAEAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void close()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(this.lastWindowID, true);
	}

	// Token: 0x0600716C RID: 29036 RVA: 0x002BCCE8 File Offset: 0x002BAEE8
	public override void OnClose()
	{
		base.OnClose();
		this.destroyBuilder();
		PathAbstractions.CacheEnabled = false;
		this.CleanupPreviewManager();
		RenderSettings.fogDensity = this.oldFogDensity;
		SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Remove(SaveDataLimitUIHelper.OnValueChanged, new Action(this.SaveDataLimit_OnChanged));
	}

	// Token: 0x0600716D RID: 29037 RVA: 0x002BCD38 File Offset: 0x002BAF38
	public void CleanupPreviewManager()
	{
		if (this.prefabPreviewManagerInstance == null)
		{
			return;
		}
		this.prefabPreviewManagerInstance.Cleanup();
		this.prefabPreviewManagerInstance = null;
	}

	// Token: 0x0600716E RID: 29038 RVA: 0x002BCD58 File Offset: 0x002BAF58
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

	// Token: 0x0600716F RID: 29039 RVA: 0x002BCDD8 File Offset: 0x002BAFD8
	public static void Open(XUi _xui, string _previousWindowId)
	{
		XUiC_WorldGenerationWindow childByType = _xui.GetChildByType<XUiC_WorldGenerationWindowGroup>().GetChildByType<XUiC_WorldGenerationWindow>();
		childByType.lastWindowID = _previousWindowId;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, true);
	}

	// Token: 0x06007170 RID: 29040 RVA: 0x002BCE10 File Offset: 0x002BB010
	public static bool IsWindowOpen(XUi _xui)
	{
		XUiC_WorldGenerationWindow childByType = _xui.GetChildByType<XUiC_WorldGenerationWindowGroup>().GetChildByType<XUiC_WorldGenerationWindow>();
		return _xui.playerUI.windowManager.IsWindowOpen(childByType.windowGroup);
	}

	// Token: 0x040055A3 RID: 21923
	public static XUiC_WorldGenerationWindow Instance;

	// Token: 0x040055A4 RID: 21924
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastWindowID = string.Empty;

	// Token: 0x040055A9 RID: 21929
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationPreview previewWindow;

	// Token: 0x040055AA RID: 21930
	[XuiBindComponent("progress", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView progressView;

	// Token: 0x040055AB RID: 21931
	[XuiBindComponent("seedInput", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtSeedInput;

	// Token: 0x040055AC RID: 21932
	[XuiBindComponent("generate", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnGenerate;

	// Token: 0x040055AD RID: 21933
	[XuiBindComponent("btnNewGame", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnNewGame;

	// Token: 0x040055AE RID: 21934
	[XuiBindComponent("WorldSize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<int> cbxWorldSize;

	// Token: 0x040055AF RID: 21935
	[XuiBindComponent("SaveDataLimitComboBox", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x040055B0 RID: 21936
	[XuiBindComponent("PreviewQuality", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<XUiC_WorldGenerationWindow.PreviewQuality> cbxPreviewQuality;

	// Token: 0x040055B1 RID: 21937
	[XuiBindComponent("BiomeLayout", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.BiomeLayout> cbxBiomeLayout;

	// Token: 0x040055B2 RID: 21938
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow.XUiC_RwgBiome[] biomeBoxes;

	// Token: 0x040055B3 RID: 21939
	[XuiBindComponent("PlainsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxPlainsWeight;

	// Token: 0x040055B4 RID: 21940
	[XuiBindComponent("HillsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxHillsWeight;

	// Token: 0x040055B5 RID: 21941
	[XuiBindComponent("MountainsWeight", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxInt cbxMountainsWeight;

	// Token: 0x040055B6 RID: 21942
	[XuiBindComponent("Rivers", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxRivers;

	// Token: 0x040055B7 RID: 21943
	[XuiBindComponent("Craters", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCraters;

	// Token: 0x040055B8 RID: 21944
	[XuiBindComponent("Cracks", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCanyons;

	// Token: 0x040055B9 RID: 21945
	[XuiBindComponent("Lakes", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxLakes;

	// Token: 0x040055BA RID: 21946
	[XuiBindComponent("Rural", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxRural;

	// Token: 0x040055BB RID: 21947
	[XuiBindComponent("Town", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxTown;

	// Token: 0x040055BC RID: 21948
	[XuiBindComponent("City", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxCity;

	// Token: 0x040055BD RID: 21949
	[XuiBindComponent("Towns", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxTowns;

	// Token: 0x040055BE RID: 21950
	[XuiBindComponent("Wilderness", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<WorldBuilder.GenerationSelections> cbxWilderness;

	// Token: 0x040055BF RID: 21951
	[XuiBindComponent("btnDataManagement", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnManage;

	// Token: 0x040055C0 RID: 21952
	[XuiBindComponent("data_bar_controller", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DataManagementBar dataManagementBar;

	// Token: 0x040055C1 RID: 21953
	[XuiBindComponent("GameSaveStorageComboBox", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxGameSaveStorage;

	// Token: 0x040055C2 RID: 21954
	[XuiBindComponent("WorldSaveStorage", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxWorldSaveStorage;

	// Token: 0x040055C3 RID: 21955
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabPreviewManager prefabPreviewManagerInstance;

	// Token: 0x040055C4 RID: 21956
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingBytes;

	// Token: 0x040055C5 RID: 21957
	[PublicizedFrom(EAccessModifier.Private)]
	public long totalAvailableBytes;

	// Token: 0x040055C7 RID: 21959
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validCountyName;

	// Token: 0x040055C8 RID: 21960
	[PublicizedFrom(EAccessModifier.Private)]
	public string countyName;

	// Token: 0x040055C9 RID: 21961
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText;

	// Token: 0x040055CA RID: 21962
	[PublicizedFrom(EAccessModifier.Private)]
	public float oldFogDensity;

	// Token: 0x040055CC RID: 21964
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x040055CD RID: 21965
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string[] biomeToUiName = new string[] { "xuiPineForest", "xuiBurntForest", "xuiDesert", "xuiSnow", "xuiWasteland" };

	// Token: 0x040055CE RID: 21966
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isGenerating;

	// Token: 0x040055CF RID: 21967
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCancelling;

	// Token: 0x02000E71 RID: 3697
	public enum PreviewQuality
	{
		// Token: 0x040055D1 RID: 21969
		NoPreview,
		// Token: 0x040055D2 RID: 21970
		Lowest,
		// Token: 0x040055D3 RID: 21971
		Low,
		// Token: 0x040055D4 RID: 21972
		Default,
		// Token: 0x040055D5 RID: 21973
		High,
		// Token: 0x040055D6 RID: 21974
		Highest
	}

	// Token: 0x02000E72 RID: 3698
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class XUiC_RwgBiome : XUiController
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06007173 RID: 29043 RVA: 0x002BCE88 File Offset: 0x002BB088
		// (remove) Token: 0x06007174 RID: 29044 RVA: 0x002BCEC0 File Offset: 0x002BB0C0
		public event Action ValueChanged;

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06007175 RID: 29045 RVA: 0x002BCEF5 File Offset: 0x002BB0F5
		// (set) Token: 0x06007176 RID: 29046 RVA: 0x002BCEFD File Offset: 0x002BB0FD
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

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06007177 RID: 29047 RVA: 0x002BCF37 File Offset: 0x002BB137
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

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06007178 RID: 29048 RVA: 0x002BCF5C File Offset: 0x002BB15C
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

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06007179 RID: 29049 RVA: 0x002BCFAA File Offset: 0x002BB1AA
		public int Value
		{
			get
			{
				return (int)this.combo.Value;
			}
		}

		// Token: 0x0600717A RID: 29050 RVA: 0x002BCFB8 File Offset: 0x002BB1B8
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

		// Token: 0x0600717B RID: 29051 RVA: 0x002926AB File Offset: 0x002908AB
		public override void Update(float _dt)
		{
			base.Update(_dt);
			base.handleDirtyUpdateDefault();
		}

		// Token: 0x0600717C RID: 29052 RVA: 0x002BCFCA File Offset: 0x002BB1CA
		public void SetLabel(string _text)
		{
			this.combo.ValueTextOverride = _text;
		}

		// Token: 0x040055D7 RID: 21975
		[XuiBindComponent(true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_ComboBoxInt combo;

		// Token: 0x040055D9 RID: 21977
		[PublicizedFrom(EAccessModifier.Private)]
		public int biomeIdx;
	}
}
