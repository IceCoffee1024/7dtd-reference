using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using SandboxOptions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E2C RID: 3628
[Preserve]
public class XUiC_NewGame : XUiC_NewContinueBase
{
	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x06006EEA RID: 28394 RVA: 0x002B1FB0 File Offset: 0x002B01B0
	// (set) Token: 0x06006EEB RID: 28395 RVA: 0x002B1FB8 File Offset: 0x002B01B8
	[XuiXmlBinding("gamenametooltip")]
	public string GameNameTooltip
	{
		get
		{
			return this.gameNameTooltip;
		}
		set
		{
			if (this.gameNameTooltip == value)
			{
				return;
			}
			this.gameNameTooltip = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x06006EEC RID: 28396 RVA: 0x002B1FD7 File Offset: 0x002B01D7
	// (set) Token: 0x06006EED RID: 28397 RVA: 0x002B1FDF File Offset: 0x002B01DF
	[XuiXmlBinding("gamenamevalid")]
	public bool ValidGameName
	{
		get
		{
			return this.validGameName;
		}
		set
		{
			if (this.validGameName == value)
			{
				return;
			}
			this.validGameName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x06006EEE RID: 28398 RVA: 0x002B1FF9 File Offset: 0x002B01F9
	[XuiXmlBinding("countynamevalid")]
	public bool CountyNameValid
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_WorldGenerationWindow xuiC_WorldGenerationWindow = this.worldGenerationControls;
			return xuiC_WorldGenerationWindow == null || xuiC_WorldGenerationWindow.ValidCountyName;
		}
	}

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x06006EEF RID: 28399 RVA: 0x002B200C File Offset: 0x002B020C
	public GameMode SelectedGameMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<GameMode> xuiC_ComboBoxList = this.cbxGameMode;
			if (xuiC_ComboBoxList == null)
			{
				return null;
			}
			return xuiC_ComboBoxList.Value;
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x06006EF0 RID: 28400 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("iscontinuegame")]
	public bool IsContinueGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x06006EF1 RID: 28401 RVA: 0x002B201F File Offset: 0x002B021F
	[XuiXmlBinding("worldtype")]
	public XUiC_NewGame.EWorldType WorldType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<XUiC_NewGame.EWorldType> xuiC_ComboBoxEnum = this.cbxWorldType;
			if (xuiC_ComboBoxEnum == null)
			{
				return XUiC_NewGame.EWorldType.Handmade;
			}
			return xuiC_ComboBoxEnum.Value;
		}
	}

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x06006EF2 RID: 28402 RVA: 0x002B2032 File Offset: 0x002B0232
	[XuiXmlBinding("saveStorageTooltip")]
	public string SaveStorageTooltipKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveStorageTooltipKey;
		}
	}

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x06006EF3 RID: 28403 RVA: 0x002B203A File Offset: 0x002B023A
	[XuiXmlBinding("saveDataLimitTooltip")]
	public string SaveDataLimitTooltipKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveDataLimitTooltipKey;
		}
	}

	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x06006EF4 RID: 28404 RVA: 0x002B2044 File Offset: 0x002B0244
	public XUiC_NewGame.GameWorldInfo? SelectedWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<XUiC_NewGame.GameWorldInfo> xuiC_ComboBoxList = this.cbxWorldName;
			return new XUiC_NewGame.GameWorldInfo?((xuiC_ComboBoxList != null) ? xuiC_ComboBoxList.Value : default(XUiC_NewGame.GameWorldInfo));
		}
	}

	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x06006EF5 RID: 28405 RVA: 0x002B2070 File Offset: 0x002B0270
	public GameUtils.WorldInfo SelectedWorldInfo
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.SelectedWorld == null)
			{
				return null;
			}
			XUiC_NewGame.GameWorldInfo? gameWorldInfo;
			return gameWorldInfo.GetValueOrDefault().WorldInfo;
		}
	}

	// Token: 0x17000C83 RID: 3203
	// (get) Token: 0x06006EF6 RID: 28406 RVA: 0x002B209C File Offset: 0x002B029C
	[XuiXmlBinding("worldgameversion")]
	public string WorldGameVersion
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			GameUtils.WorldInfo selectedWorldInfo = this.SelectedWorldInfo;
			if (selectedWorldInfo == null)
			{
				return "";
			}
			if (!selectedWorldInfo.GameVersionCreated.IsValid)
			{
				return "";
			}
			return selectedWorldInfo.GameVersionCreated.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>() + " " + selectedWorldInfo.GameVersionCreated.Major.ToString();
		}
	}

	// Token: 0x17000C84 RID: 3204
	// (get) Token: 0x06006EF7 RID: 28407 RVA: 0x002B20FC File Offset: 0x002B02FC
	[XuiXmlBinding("differentworldversion")]
	public bool DifferentWorldVersion
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			GameUtils.WorldInfo selectedWorldInfo = this.SelectedWorldInfo;
			return selectedWorldInfo != null && selectedWorldInfo.GameVersionCreated.IsValid && !selectedWorldInfo.GameVersionCreated.EqualsMajor(Constants.cVersionInformation);
		}
	}

	// Token: 0x17000C85 RID: 3205
	// (get) Token: 0x06006EF8 RID: 28408 RVA: 0x002B2138 File Offset: 0x002B0338
	[XuiXmlBinding("mapsize")]
	public string MapSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			GameUtils.WorldInfo selectedWorldInfo = this.SelectedWorldInfo;
			if (selectedWorldInfo == null)
			{
				return "";
			}
			return string.Format("{0} x {1}", selectedWorldInfo.WorldSize.x, selectedWorldInfo.WorldSize.y);
		}
	}

	// Token: 0x17000C86 RID: 3206
	// (get) Token: 0x06006EF9 RID: 28409 RVA: 0x002B217F File Offset: 0x002B037F
	[XuiXmlBinding("isgenerateworld")]
	public bool IsGenerateWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.WorldType == XUiC_NewGame.EWorldType.CreateRandom;
		}
	}

	// Token: 0x17000C87 RID: 3207
	// (get) Token: 0x06006EFA RID: 28410 RVA: 0x002B218C File Offset: 0x002B038C
	[XuiXmlBinding("worldstorageusesdatalimit")]
	public bool WorldStorageUsesDataLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_NewGame.GameWorldInfo? gameWorldInfo;
			return this.SelectedWorld != null && gameWorldInfo.GetValueOrDefault().Location.StorageType.UsesDataLimit();
		}
	}

	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x06006EFB RID: 28411 RVA: 0x002B21C1 File Offset: 0x002B03C1
	public override bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsIntroMovieEnabled);
		}
	}

	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x06006EFC RID: 28412 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06006EFD RID: 28413 RVA: 0x002B21CD File Offset: 0x002B03CD
	public override void Init()
	{
		base.Init();
		XUiC_NewGame.ID = base.WindowGroup.Id;
		this.txtGameName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
		SaveDataLimitUIHelper.AddComboBox(this.cbxSaveDataLimit);
	}

	// Token: 0x06006EFE RID: 28414 RVA: 0x002B2210 File Offset: 0x002B0410
	public override void OnOpen()
	{
		using (this.pmOnOpen.Auto())
		{
			this.IsDirty = true;
			this.Settings.WatchForServerEnabledChanges(false);
			this.cbxGameMode.Elements.Clear();
			this.createWorldList();
			foreach (GameMode gameMode in GameMode.AvailGameModes)
			{
				if (this.worldsPerMode.ContainsKey((EnumGameMode)gameMode.GetID()))
				{
					this.cbxGameMode.Elements.Add(gameMode);
				}
			}
			if (this.cbxGameMode.Elements.Count == 0)
			{
				Log.Error("No supported GameMode found in any world!");
			}
			string modeName = GamePrefs.GetString(EnumGamePrefs.GameMode);
			int num = this.cbxGameMode.Elements.FindIndex((GameMode _mode) => _mode.GetTypeName().EqualsCaseInsensitive(modeName));
			if (num < 0)
			{
				num = 0;
			}
			this.cbxGameMode.SelectedIndex = num;
			this.gameModeChanged(this.SelectedGameMode);
			GamePrefs.Instance.Load(GameIO.GetSaveGameRootDir() + "/newGameOptions.sdf");
			this.Settings.UpdateOptionValuesFromGamePrefs();
			this.Settings.ResetSandboxPresetToDefault();
			if (GamePrefs.GetInt(EnumGamePrefs.MaxChunkAge) == 0)
			{
				GamePrefs.Set(EnumGamePrefs.MaxChunkAge, (int)GamePrefs.GetDefault(EnumGamePrefs.MaxChunkAge));
			}
			if (GamePrefs.GetString(EnumGamePrefs.GameName).Trim().Length < 1)
			{
				GamePrefs.Set(EnumGamePrefs.GameName, "MyGame");
			}
			this.txtGameName.Text = GamePrefs.GetString(EnumGamePrefs.GameName).Trim();
			this.validateStartable();
			base.OnOpen();
			this.Settings.ApplyMaxPlayerCountOptions();
			this.Settings.UpdateOptionVisibilityForGameMode(this.SelectedGameMode);
			base.GetChildById("txtGameName").SelectCursorElement(true, false);
			this.Settings.ApplyInitialServerEnabledState();
			if (base.DataManagementBarEnabled)
			{
				SaveInfoProvider.Instance.SetDirty();
				this.DataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Preview);
				this.DataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Primary);
				base.updateBarUsageAndAllowanceValues();
				this.updateBarPendingValue();
				SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Combine(SaveDataLimitUIHelper.OnValueChanged, new Action(this.CbxSaveDataLimit_OnValueChanged));
				this.worldGenerationControls.OnWorldSizeChanged += this.updateBarPendingValue;
			}
			this.cbxGameSaveStorage.Value = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
		}
	}

	// Token: 0x06006EFF RID: 28415 RVA: 0x002B2488 File Offset: 0x002B0688
	public override void OnClose()
	{
		base.OnClose();
		SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Remove(SaveDataLimitUIHelper.OnValueChanged, new Action(this.CbxSaveDataLimit_OnValueChanged));
		this.worldGenerationControls.OnWorldSizeChanged -= this.updateBarPendingValue;
	}

	// Token: 0x06006F00 RID: 28416 RVA: 0x002B24C7 File Offset: 0x002B06C7
	[XuiBindEvent("OnPress", "btnGenerateWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRwgPreviewerOnOnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_WorldGenerationWindow.Open(this.xui, XUiC_NewGame.ID);
	}

	// Token: 0x06006F01 RID: 28417 RVA: 0x002B24D9 File Offset: 0x002B06D9
	[XuiBindEvent("OnValueChanged", "cbxGameMode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxGameMode_OnValueChanged(XUiController _sender, GameMode _oldValue, GameMode _newValue)
	{
		this.gameModeChanged(_newValue);
	}

	// Token: 0x06006F02 RID: 28418 RVA: 0x002B24E2 File Offset: 0x002B06E2
	[XuiBindEvent("OnValueChanged", "cbxWorldType")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxWorldType_OnValueChanged(XUiController _sender, XUiC_NewGame.EWorldType _oldValue, XUiC_NewGame.EWorldType _newValue)
	{
		this.IsDirty = true;
		this.updateWorlds();
		this.validateStartable();
	}

	// Token: 0x06006F03 RID: 28419 RVA: 0x002B24F8 File Offset: 0x002B06F8
	[XuiBindEvent("OnValueChanged", "cbxWorldName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxWorldName_OnValueChanged(XUiController _sender, XUiC_NewGame.GameWorldInfo _oldValue, XUiC_NewGame.GameWorldInfo _newValue)
	{
		this.IsDirty = true;
		GamePrefs.Set(EnumGamePrefs.GameWorld, _newValue.RealName);
		if (this.IsGenerateWorld)
		{
			GamePrefs.Set(EnumGamePrefs.UserWorldStorageType, GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType));
			this.worldGenerationControls.ValidateNewRwg();
		}
		else
		{
			GamePrefs.Set(EnumGamePrefs.UserWorldStorageType, (int)_newValue.Location.StorageType);
		}
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006F04 RID: 28420 RVA: 0x002B2563 File Offset: 0x002B0763
	[XuiBindEvent("OnChangeHandler", "txtGameName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtGameName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		GamePrefs.Set(EnumGamePrefs.GameName, _text);
		this.validateStartable();
	}

	// Token: 0x06006F05 RID: 28421 RVA: 0x002B2573 File Offset: 0x002B0773
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxSaveDataLimit_OnValueChanged()
	{
		this.IsDirty = true;
		this.updateBarPendingValue();
	}

	// Token: 0x06006F06 RID: 28422 RVA: 0x002B2582 File Offset: 0x002B0782
	[XuiBindEvent("OnValueChanged", "cbxGameSaveStorage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxGameSaveStorage_OnValueChanged(XUiController _sender, UserDataStorageType _oldValue, UserDataStorageType _newValue)
	{
		this.IsDirty = true;
		GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, (int)_newValue);
		this.worldGenerationControls.SetGameSaveStorageValue(_newValue);
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006F07 RID: 28423 RVA: 0x002B25AE File Offset: 0x002B07AE
	[XuiBindEvent("OnGameSaveStorageChanged", "worldGenerationControls")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldGenerationGameSaveStorageChanged(UserDataStorageType _newValue)
	{
		this.IsDirty = true;
		this.cbxGameSaveStorage.Value = _newValue;
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006F08 RID: 28424 RVA: 0x002B25CF File Offset: 0x002B07CF
	[XuiBindEvent("OnWorldStorageChanged", "worldGenerationControls")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldGenerationWorldSaveStorageChanged()
	{
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006F09 RID: 28425 RVA: 0x002B25E0 File Offset: 0x002B07E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void createWorldList()
	{
		this.worldsPerMode.Clear();
		List<PathAbstractions.AbstractedLocation> availablePathsList = PathAbstractions.WorldsSearchPaths.GetAvailablePathsList(null, false, null);
		if (availablePathsList.Count == 0)
		{
			Log.Error("No worlds found!");
			return;
		}
		foreach (PathAbstractions.AbstractedLocation abstractedLocation in availablePathsList)
		{
			XUiC_NewGame.GameWorldInfo gameWorldInfo = new XUiC_NewGame.GameWorldInfo
			{
				RealName = abstractedLocation.Name,
				Location = abstractedLocation
			};
			GameUtils.WorldInfo worldInfo = GameUtils.WorldInfo.LoadWorldInfo(abstractedLocation);
			if (worldInfo == null)
			{
				gameWorldInfo.Description = "No Description Found";
				if (!this.worldsPerMode.ContainsKey(EnumGameMode.Creative))
				{
					this.worldsPerMode.Add(EnumGameMode.Creative, new List<XUiC_NewGame.GameWorldInfo>());
				}
				this.worldsPerMode[EnumGameMode.Creative].Add(gameWorldInfo);
			}
			else
			{
				gameWorldInfo.CustName = worldInfo.Name;
				gameWorldInfo.Description = worldInfo.Description;
				gameWorldInfo.WorldInfo = worldInfo;
				string[] modes = worldInfo.Modes;
				if (modes != null)
				{
					foreach (string text in modes)
					{
						EnumGameMode enumGameMode;
						if (EnumUtils.TryParse<EnumGameMode>(text, out enumGameMode, true))
						{
							if (!this.worldsPerMode.ContainsKey(enumGameMode))
							{
								this.worldsPerMode.Add(enumGameMode, new List<XUiC_NewGame.GameWorldInfo>());
							}
							this.worldsPerMode[enumGameMode].Add(gameWorldInfo);
						}
						else
						{
							Log.Warning(string.Concat(new string[] { "World \"", gameWorldInfo.RealName, "\" has unknown game mode \"", text, "\"." }));
						}
					}
				}
			}
		}
		if (this.worldsPerMode.Count == 0)
		{
			Log.Error("No world with any valid GameMode found!");
		}
	}

	// Token: 0x06006F0A RID: 28426 RVA: 0x002B27C8 File Offset: 0x002B09C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateWorlds()
	{
		EnumGameMode id = (EnumGameMode)this.SelectedGameMode.GetID();
		this.cbxWorldName.Elements.Clear();
		List<XUiC_NewGame.GameWorldInfo> list;
		if (this.WorldType != XUiC_NewGame.EWorldType.CreateRandom && this.worldsPerMode.TryGetValue(id, out list))
		{
			foreach (XUiC_NewGame.GameWorldInfo gameWorldInfo in list)
			{
				if (this.<updateWorlds>g__CanAddWorld|66_0(gameWorldInfo))
				{
					this.cbxWorldName.Elements.Add(gameWorldInfo);
				}
			}
		}
		this.cbxWorldName.SelectedIndex = 0;
		this.CbxWorldName_OnValueChanged(this.cbxWorldName, default(XUiC_NewGame.GameWorldInfo), this.SelectedWorld.Value);
	}

	// Token: 0x06006F0B RID: 28427 RVA: 0x002B2890 File Offset: 0x002B0A90
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectWorld(string _worldName)
	{
		int num = this.cbxWorldName.Elements.FindIndex((XUiC_NewGame.GameWorldInfo _info) => _info.RealName.EqualsCaseInsensitive(_worldName));
		if (num < 0)
		{
			EnumGameMode id = (EnumGameMode)this.SelectedGameMode.GetID();
			List<XUiC_NewGame.GameWorldInfo> list;
			if (this.worldsPerMode.TryGetValue(id, out list))
			{
				num = list.FindIndex((XUiC_NewGame.GameWorldInfo _info) => _info.RealName.EqualsCaseInsensitive(_worldName));
				if (num >= 0)
				{
					XUiC_NewGame.GameWorldInfo gameWorldInfo = list[num];
					this.cbxWorldType.Value = (gameWorldInfo.IsRandomWorld ? XUiC_NewGame.EWorldType.ExistingRandom : XUiC_NewGame.EWorldType.Handmade);
					this.CbxWorldType_OnValueChanged(this.cbxWorldType, XUiC_NewGame.EWorldType.Handmade, this.cbxWorldType.Value);
				}
			}
			num = this.cbxWorldName.Elements.FindIndex((XUiC_NewGame.GameWorldInfo _info) => _info.RealName.EqualsCaseInsensitive(_worldName));
		}
		this.cbxWorldName.SelectedIndex = num;
		this.CbxWorldName_OnValueChanged(this.cbxWorldName, default(XUiC_NewGame.GameWorldInfo), this.SelectedWorld.Value);
	}

	// Token: 0x06006F0C RID: 28428 RVA: 0x002B2988 File Offset: 0x002B0B88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnOpenSandboxSettingsRequested()
	{
		SandboxOptionManager.Current.SetWorldAndGame("", "");
		base.OnOpenSandboxSettingsRequested();
	}

	// Token: 0x06006F0D RID: 28429 RVA: 0x002B29A4 File Offset: 0x002B0BA4
	[XuiBindEvent("OnPress", "btnDefaults")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDefaults_OnPressed(XUiController _sender, int _mouseButton)
	{
		GamePrefs.Set(EnumGamePrefs.ServerEnabled, (bool)GamePrefs.GetDefault(EnumGamePrefs.ServerEnabled));
		this.SelectedGameMode.ResetGamePrefs();
		this.Settings.UpdateOptionValuesFromGamePrefs();
		this.Settings.UpdateServerEnabledState();
	}

	// Token: 0x06006F0E RID: 28430 RVA: 0x002B29E0 File Offset: 0x002B0BE0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDataManagementWindowClosed()
	{
		base.updateBarUsageAndAllowanceValues();
		string @string = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		this.createWorldList();
		this.updateWorlds();
		this.selectWorld(@string);
		if (base.DataManagementBarEnabled)
		{
			this.DataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Preview);
			this.updateBarPendingValue();
		}
		this.worldGenerationControls.RefreshCountyName();
		this.validateStartable();
		this.worldGenerationControls.RefreshBindings();
		base.RefreshBindings();
	}

	// Token: 0x06006F0F RID: 28431 RVA: 0x002B2A4C File Offset: 0x002B0C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBarPendingValue()
	{
		if (!base.DataManagementBarEnabled)
		{
			return;
		}
		UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
		this.pendingPreviewSize = (@int.UsesDataLimit() ? this.<updateBarPendingValue>g__CalculatePendingSaveSize|71_0() : 0L);
		this.DataManagementBar.SetPendingBytes(this.pendingPreviewSize);
		this.validateStartable();
	}

	// Token: 0x06006F10 RID: 28432 RVA: 0x002B2A9C File Offset: 0x002B0C9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override IEnumerator startGameCo()
	{
		string worldName;
		Vector2i worldSize;
		if (this.IsGenerateWorld)
		{
			worldName = null;
			yield return this.worldGenerationControls.GenerateCo(delegate(string _name)
			{
				worldName = _name;
			}, null);
			if (worldName == null)
			{
				this.xui.playerUI.windowManager.Open(this.windowGroup, true);
				yield break;
			}
			GamePrefs.Set(EnumGamePrefs.GameWorld, worldName);
			GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, 2);
			worldSize.y = (worldSize.x = this.worldGenerationControls.WorldSize);
		}
		else
		{
			worldSize = this.SelectedWorldInfo.WorldSize;
			worldName = this.SelectedWorldInfo.Name;
			XUiC_NewGame.GameWorldInfo? gameWorldInfo;
			GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, (int)((this.SelectedWorld != null) ? new PathAbstractions.EAbstractedLocationType?(gameWorldInfo.GetValueOrDefault().Location.Type) : null).Value);
		}
		PathAbstractions.AbstractedLocation abstractedLocation = PathAbstractions.Contextual.FindActiveWorldLocation();
		UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
		long saveDataSize = SaveDataLimitUIHelper.CurrentValue.CalculateTotalSize(worldSize);
		XUiC_SaveSpaceNeeded confirmationWindow = XUiC_SaveSpaceNeeded.Open(saveDataSize, new string[]
		{
			abstractedLocation.FullPath,
			GameIO.GetSaveGameDir(worldName, GamePrefs.GetString(EnumGamePrefs.GameName), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType))
		}, @int, true, true, false, null, "xuiDmCreateSave", null, null, "xuiCreate", null);
		if (confirmationWindow != null)
		{
			while (confirmationWindow.IsOpen || confirmationWindow.Result == XUiC_SaveSpaceNeeded.ConfirmationResult.Pending)
			{
				yield return null;
			}
			if (confirmationWindow.Result != XUiC_SaveSpaceNeeded.ConfirmationResult.Confirmed)
			{
				this.xui.playerUI.windowManager.Open(this.windowGroup, true);
				yield break;
			}
		}
		SaveDataLimit.SetLimitToPref(saveDataSize);
		GamePrefs.SetPersistent(EnumGamePrefs.GameMode, true);
		this.Settings.SaveGameOptions(true);
		if (PlatformOptimizations.RestartAfterRwg)
		{
			yield return PlatformApplicationManager.CheckRestartCoroutine(true);
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		bool flag = !GamePrefs.GetBool(EnumGamePrefs.ServerEnabled);
		NetworkConnectionError networkConnectionError = SingletonMonoBehaviour<ConnectionManager>.Instance.StartServers(GamePrefs.GetString(EnumGamePrefs.ServerPassword), flag);
		if (networkConnectionError != NetworkConnectionError.NoError)
		{
			XUiC_MessageBoxWindowGroup.ShowNetworkError(this.xui, networkConnectionError, false);
		}
		else
		{
			XUiC_NewContinueBase.LastStartedNewGame = true;
		}
		yield break;
	}

	// Token: 0x06006F11 RID: 28433 RVA: 0x002B2AAC File Offset: 0x002B0CAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameModeChanged(GameMode _newMode)
	{
		GamePrefs.Set(EnumGamePrefs.GameMode, _newMode.GetTypeName());
		string @string = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		this.updateWorlds();
		this.selectWorld(@string);
		this.Settings.UpdateOptionVisibilityForGameMode(_newMode);
		this.Settings.UpdateOptionValuesFromGamePrefs();
		this.Settings.ResetSandboxPresetToDefault();
		this.validateStartable();
	}

	// Token: 0x06006F12 RID: 28434 RVA: 0x002B2B04 File Offset: 0x002B0D04
	[XuiBindEvent("OnCountyNameChanged", "worldGenerationControls")]
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void validateStartable()
	{
		string text;
		bool flag = this.validateWorld(out text);
		flag &= this.validateGameName(text);
		flag &= this.validateStorage();
		if (base.DataManagementBarEnabled)
		{
			flag &= SaveInfoProvider.Instance.TotalAvailableBytes >= this.pendingPreviewSize;
		}
		flag &= this.Settings.PortValid;
		base.ValidStartableState = flag;
	}

	// Token: 0x06006F13 RID: 28435 RVA: 0x002B2B64 File Offset: 0x002B0D64
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateGameName(string _worldName)
	{
		string text = this.txtGameName.Text;
		UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
		bool flag = GameUtils.ValidateGameName(text);
		bool flag2 = text.Trim().Length > 0;
		bool flag3 = !text.EqualsCaseInsensitive("Region");
		bool flag4 = !SdFile.Exists(GameIO.GetSaveGameDir(_worldName, text, @int) + "/main.ttw");
		this.ValidGameName = flag && flag2 && flag3 && flag4;
		if (!flag2)
		{
			this.GameNameTooltip = Localization.Get("mmLblNameErrorEmpty", false, null);
		}
		else if (!flag)
		{
			this.GameNameTooltip = Localization.Get("mmLblNameErrorInvalid", false, null);
		}
		else if (!flag3)
		{
			this.GameNameTooltip = Localization.Get("mmLblNameErrorDefault", false, null);
		}
		else if (!flag4)
		{
			this.GameNameTooltip = Localization.Get("mmLblNameErrorExists", false, null);
		}
		else
		{
			this.GameNameTooltip = "";
		}
		return this.ValidGameName;
	}

	// Token: 0x06006F14 RID: 28436 RVA: 0x002B2C48 File Offset: 0x002B0E48
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateWorld(out string _worldName)
	{
		XUiC_NewGame.GameWorldInfo value = this.SelectedWorld.Value;
		bool flag = true;
		bool flag2;
		if (!this.IsGenerateWorld)
		{
			flag2 = value.Location.Type != PathAbstractions.EAbstractedLocationType.None;
			if (this.SelectedWorldInfo != null)
			{
				bool flag3 = this.SelectedWorldInfo.GameVersionCreated.IsValid && !this.SelectedWorldInfo.GameVersionCreated.EqualsMajor(Constants.cVersionInformation);
				flag &= !flag3;
			}
			_worldName = value.RealName;
		}
		else
		{
			flag2 = this.worldGenerationControls.ValidCountyName;
			_worldName = this.worldGenerationControls.CountyName;
		}
		return flag2 && flag;
	}

	// Token: 0x06006F15 RID: 28437 RVA: 0x002B2CE8 File Offset: 0x002B0EE8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validateStorage()
	{
		UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
		XUiC_NewGame.GameWorldInfo value = this.SelectedWorld.Value;
		bool flag = this.IsGenerateWorld || value.Location.Type == PathAbstractions.EAbstractedLocationType.GameData || value.Location.StorageType != UserDataStorageType.DeviceLocal || @int == UserDataStorageType.DeviceLocal;
		if (flag)
		{
			this.cbxGameSaveStorage.TextColor = Color.white;
			this.saveStorageTooltipKey = string.Empty;
		}
		else
		{
			this.cbxGameSaveStorage.TextColor = Color.red;
			this.saveStorageTooltipKey = "mmLblNameErrorSaveStorageInvalid";
		}
		bool flag2 = !@int.UsesDataLimit() || this.cbxSaveDataLimit.Value > SaveDataLimitType.Unlimited;
		if (flag2)
		{
			this.cbxSaveDataLimit.TextColor = Color.white;
			this.saveDataLimitTooltipKey = string.Empty;
		}
		else
		{
			this.cbxSaveDataLimit.TextColor = Color.red;
			this.saveDataLimitTooltipKey = "mmLblNameErrorSaveDataLimitNoUnlimited";
		}
		return flag && flag2;
	}

	// Token: 0x06006F16 RID: 28438 RVA: 0x002B2DCC File Offset: 0x002B0FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateInput()
	{
		if (!XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false))
		{
			return;
		}
		if (this.xui.playerUI.playerInput.GUIActions.Apply.WasPressed)
		{
			base.BtnStart_OnPressed(null, 0);
		}
	}

	// Token: 0x06006F17 RID: 28439 RVA: 0x002B2E26 File Offset: 0x002B1026
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.doLoadSaveGameAutomation();
		this.updateInput();
	}

	// Token: 0x06006F18 RID: 28440 RVA: 0x002B2E3C File Offset: 0x002B103C
	[PublicizedFrom(EAccessModifier.Private)]
	public void doLoadSaveGameAutomation()
	{
		EPlatformLoadSaveGameState state = PlatformApplicationManager.LoadSaveGame.GetState();
		if (state != EPlatformLoadSaveGameState.NewGameSelect)
		{
			if (state != EPlatformLoadSaveGameState.NewGamePlay)
			{
				return;
			}
			if (!this.BtnStart.ViewComponent.Enabled)
			{
				PlatformApplicationManager.LoadSaveGame.SetFailed();
				return;
			}
			base.BtnStart_OnPressed(this, -1);
			PlatformApplicationManager.LoadSaveGame.AdvanceStateFrom(state);
			return;
		}
		else
		{
			string gameWorld = PlatformApplicationManager.LoadSaveGame.GameWorld;
			if (!this.SelectedWorld.Value.RealName.EqualsCaseInsensitive(gameWorld))
			{
				this.selectWorld(gameWorld);
			}
			if (!this.SelectedWorld.Value.RealName.EqualsCaseInsensitive(gameWorld))
			{
				PlatformApplicationManager.LoadSaveGame.SetFailed();
				return;
			}
			PlatformApplicationManager.LoadSaveGame.AdvanceStateFrom(state);
			return;
		}
	}

	// Token: 0x06006F1B RID: 28443 RVA: 0x002B2F2C File Offset: 0x002B112C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <updateWorlds>g__CanAddWorld|66_0(XUiC_NewGame.GameWorldInfo _worldInfo)
	{
		if (_worldInfo.WorldInfo != null && PlatformOptimizations.EnforceMaxWorldSizeHost)
		{
			Vector2i worldSize = _worldInfo.WorldInfo.WorldSize;
			if (worldSize.x > PlatformOptimizations.MaxWorldSizeHost || worldSize.y > PlatformOptimizations.MaxWorldSizeHost)
			{
				return false;
			}
		}
		XUiC_NewGame.EWorldType worldType = this.WorldType;
		if (worldType != XUiC_NewGame.EWorldType.Handmade)
		{
			if (worldType == XUiC_NewGame.EWorldType.ExistingRandom)
			{
				if (!_worldInfo.IsRandomWorld)
				{
					return false;
				}
			}
		}
		else if (_worldInfo.IsRandomWorld)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06006F1C RID: 28444 RVA: 0x002B2F98 File Offset: 0x002B1198
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public long <updateBarPendingValue>g__CalculatePendingSaveSize|71_0()
	{
		Vector2i worldSize;
		if (this.IsGenerateWorld)
		{
			worldSize.y = (worldSize.x = Math.Max(1, this.worldGenerationControls.WorldSize));
		}
		else
		{
			worldSize = this.SelectedWorldInfo.WorldSize;
		}
		if (SaveDataLimitUIHelper.CurrentValue == SaveDataLimitType.Unlimited)
		{
			return Math.Max(SaveDataLimitType.Short.CalculateTotalSize(worldSize), SaveInfoProvider.Instance.TotalAvailableBytes);
		}
		return SaveDataLimitUIHelper.CurrentValue.CalculateTotalSize(worldSize);
	}

	// Token: 0x04005466 RID: 21606
	public static string ID = "";

	// Token: 0x04005467 RID: 21607
	[XuiBindComponent("cbxGameMode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<GameMode> cbxGameMode;

	// Token: 0x04005468 RID: 21608
	[XuiBindComponent("cbxWorldType", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<XUiC_NewGame.EWorldType> cbxWorldType;

	// Token: 0x04005469 RID: 21609
	[XuiBindComponent("cbxWorldName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_NewGame.GameWorldInfo> cbxWorldName;

	// Token: 0x0400546A RID: 21610
	[XuiBindComponent("txtGameName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtGameName;

	// Token: 0x0400546B RID: 21611
	[XuiBindComponent("btnDefaults", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x0400546C RID: 21612
	[XuiBindComponent("btnGenerateWorld", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnGenerateWorld;

	// Token: 0x0400546D RID: 21613
	[XuiBindComponent("cbxSaveDataLimit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x0400546E RID: 21614
	[PublicizedFrom(EAccessModifier.Private)]
	public string saveDataLimitTooltipKey = string.Empty;

	// Token: 0x0400546F RID: 21615
	[XuiBindComponent("cbxGameSaveStorage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxGameSaveStorage;

	// Token: 0x04005470 RID: 21616
	[PublicizedFrom(EAccessModifier.Private)]
	public string saveStorageTooltipKey = string.Empty;

	// Token: 0x04005471 RID: 21617
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow worldGenerationControls;

	// Token: 0x04005472 RID: 21618
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EnumGameMode, List<XUiC_NewGame.GameWorldInfo>> worldsPerMode = new EnumDictionary<EnumGameMode, List<XUiC_NewGame.GameWorldInfo>>();

	// Token: 0x04005473 RID: 21619
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingPreviewSize;

	// Token: 0x04005474 RID: 21620
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameNameTooltip = "";

	// Token: 0x04005475 RID: 21621
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validGameName;

	// Token: 0x04005476 RID: 21622
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmOnOpen = new ProfilerMarker("OnOpen");

	// Token: 0x02000E2D RID: 3629
	public enum EWorldType
	{
		// Token: 0x04005478 RID: 21624
		Handmade,
		// Token: 0x04005479 RID: 21625
		ExistingRandom,
		// Token: 0x0400547A RID: 21626
		CreateRandom
	}

	// Token: 0x02000E2E RID: 3630
	public struct GameWorldInfo
	{
		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06006F1D RID: 28445 RVA: 0x002B3006 File Offset: 0x002B1206
		public bool IsRandomWorld
		{
			get
			{
				GameUtils.WorldInfo worldInfo = this.WorldInfo;
				return worldInfo != null && worldInfo.RandomGeneratedWorld;
			}
		}

		// Token: 0x06006F1E RID: 28446 RVA: 0x002B301C File Offset: 0x002B121C
		public override string ToString()
		{
			if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.Location.Type == PathAbstractions.EAbstractedLocationType.UserDataPath)
			{
				return this.RealName + " [808080][i](" + this.Location.StorageType.LocalizedName() + ")[/i][-]";
			}
			return this.RealName;
		}

		// Token: 0x0400547B RID: 21627
		public string RealName;

		// Token: 0x0400547C RID: 21628
		public string CustName;

		// Token: 0x0400547D RID: 21629
		public string Description;

		// Token: 0x0400547E RID: 21630
		public GameUtils.WorldInfo WorldInfo;

		// Token: 0x0400547F RID: 21631
		public PathAbstractions.AbstractedLocation Location;
	}
}
