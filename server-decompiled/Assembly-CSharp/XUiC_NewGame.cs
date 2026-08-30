using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using SandboxOptions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E1D RID: 3613
[Preserve]
public class XUiC_NewGame : XUiC_NewContinueBase
{
	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x06006EF2 RID: 28402 RVA: 0x002AEC08 File Offset: 0x002ACE08
	// (set) Token: 0x06006EF3 RID: 28403 RVA: 0x002AEC10 File Offset: 0x002ACE10
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

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x06006EF4 RID: 28404 RVA: 0x002AEC2F File Offset: 0x002ACE2F
	// (set) Token: 0x06006EF5 RID: 28405 RVA: 0x002AEC37 File Offset: 0x002ACE37
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

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x06006EF6 RID: 28406 RVA: 0x002AEC51 File Offset: 0x002ACE51
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

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x06006EF7 RID: 28407 RVA: 0x002AEC64 File Offset: 0x002ACE64
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

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x06006EF8 RID: 28408 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("iscontinuegame")]
	public bool IsContinueGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x06006EF9 RID: 28409 RVA: 0x002AEC77 File Offset: 0x002ACE77
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

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x06006EFA RID: 28410 RVA: 0x002AEC8A File Offset: 0x002ACE8A
	[XuiXmlBinding("saveStorageTooltip")]
	public string SaveStorageTooltipKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveStorageTooltipKey;
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x06006EFB RID: 28411 RVA: 0x002AEC92 File Offset: 0x002ACE92
	[XuiXmlBinding("saveDataLimitTooltip")]
	public string SaveDataLimitTooltipKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveDataLimitTooltipKey;
		}
	}

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x06006EFC RID: 28412 RVA: 0x002AEC9C File Offset: 0x002ACE9C
	public XUiC_NewGame.GameWorldInfo? SelectedWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<XUiC_NewGame.GameWorldInfo> xuiC_ComboBoxList = this.cbxWorldName;
			return new XUiC_NewGame.GameWorldInfo?((xuiC_ComboBoxList != null) ? xuiC_ComboBoxList.Value : default(XUiC_NewGame.GameWorldInfo));
		}
	}

	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x06006EFD RID: 28413 RVA: 0x002AECC8 File Offset: 0x002ACEC8
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

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x06006EFE RID: 28414 RVA: 0x002AECF4 File Offset: 0x002ACEF4
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

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x06006EFF RID: 28415 RVA: 0x002AED54 File Offset: 0x002ACF54
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

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x06006F00 RID: 28416 RVA: 0x002AED90 File Offset: 0x002ACF90
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

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x06006F01 RID: 28417 RVA: 0x002AEDD7 File Offset: 0x002ACFD7
	[XuiXmlBinding("isgenerateworld")]
	public bool IsGenerateWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.WorldType == XUiC_NewGame.EWorldType.CreateRandom;
		}
	}

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x06006F02 RID: 28418 RVA: 0x002AEDE4 File Offset: 0x002ACFE4
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

	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x06006F03 RID: 28419 RVA: 0x002AEE19 File Offset: 0x002AD019
	public override bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsIntroMovieEnabled);
		}
	}

	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x06006F04 RID: 28420 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06006F05 RID: 28421 RVA: 0x002AEE25 File Offset: 0x002AD025
	public override void Init()
	{
		base.Init();
		XUiC_NewGame.ID = base.WindowGroup.Id;
		this.txtGameName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
		SaveDataLimitUIHelper.AddComboBox(this.cbxSaveDataLimit);
	}

	// Token: 0x06006F06 RID: 28422 RVA: 0x002AEE68 File Offset: 0x002AD068
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

	// Token: 0x06006F07 RID: 28423 RVA: 0x002AF0E0 File Offset: 0x002AD2E0
	public override void OnClose()
	{
		base.OnClose();
		SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Remove(SaveDataLimitUIHelper.OnValueChanged, new Action(this.CbxSaveDataLimit_OnValueChanged));
		this.worldGenerationControls.OnWorldSizeChanged -= this.updateBarPendingValue;
	}

	// Token: 0x06006F08 RID: 28424 RVA: 0x002AF11F File Offset: 0x002AD31F
	[XuiBindEvent("OnPress", "btnGenerateWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRwgPreviewerOnOnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_WorldGenerationWindow.Open(this.xui, XUiC_NewGame.ID);
	}

	// Token: 0x06006F09 RID: 28425 RVA: 0x002AF131 File Offset: 0x002AD331
	[XuiBindEvent("OnValueChanged", "cbxGameMode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxGameMode_OnValueChanged(XUiController _sender, GameMode _oldValue, GameMode _newValue)
	{
		this.gameModeChanged(_newValue);
	}

	// Token: 0x06006F0A RID: 28426 RVA: 0x002AF13A File Offset: 0x002AD33A
	[XuiBindEvent("OnValueChanged", "cbxWorldType")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxWorldType_OnValueChanged(XUiController _sender, XUiC_NewGame.EWorldType _oldValue, XUiC_NewGame.EWorldType _newValue)
	{
		this.IsDirty = true;
		this.updateWorlds();
		this.validateStartable();
	}

	// Token: 0x06006F0B RID: 28427 RVA: 0x002AF150 File Offset: 0x002AD350
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

	// Token: 0x06006F0C RID: 28428 RVA: 0x002AF1BB File Offset: 0x002AD3BB
	[XuiBindEvent("OnChangeHandler", "txtGameName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtGameName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		GamePrefs.Set(EnumGamePrefs.GameName, _text);
		this.validateStartable();
	}

	// Token: 0x06006F0D RID: 28429 RVA: 0x002AF1CB File Offset: 0x002AD3CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxSaveDataLimit_OnValueChanged()
	{
		this.IsDirty = true;
		this.updateBarPendingValue();
	}

	// Token: 0x06006F0E RID: 28430 RVA: 0x002AF1DA File Offset: 0x002AD3DA
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

	// Token: 0x06006F0F RID: 28431 RVA: 0x002AF206 File Offset: 0x002AD406
	[XuiBindEvent("OnGameSaveStorageChanged", "worldGenerationControls")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldGenerationGameSaveStorageChanged(UserDataStorageType _newValue)
	{
		this.IsDirty = true;
		this.cbxGameSaveStorage.Value = _newValue;
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006F10 RID: 28432 RVA: 0x002AF227 File Offset: 0x002AD427
	[XuiBindEvent("OnWorldStorageChanged", "worldGenerationControls")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldGenerationWorldSaveStorageChanged()
	{
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006F11 RID: 28433 RVA: 0x002AF238 File Offset: 0x002AD438
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

	// Token: 0x06006F12 RID: 28434 RVA: 0x002AF420 File Offset: 0x002AD620
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

	// Token: 0x06006F13 RID: 28435 RVA: 0x002AF4E8 File Offset: 0x002AD6E8
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

	// Token: 0x06006F14 RID: 28436 RVA: 0x002AF5E0 File Offset: 0x002AD7E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnOpenSandboxSettingsRequested()
	{
		SandboxOptionManager.Current.SetWorldAndGame("", "");
		base.OnOpenSandboxSettingsRequested();
	}

	// Token: 0x06006F15 RID: 28437 RVA: 0x002AF5FC File Offset: 0x002AD7FC
	[XuiBindEvent("OnPress", "btnDefaults")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDefaults_OnPressed(XUiController _sender, int _mouseButton)
	{
		GamePrefs.Set(EnumGamePrefs.ServerEnabled, (bool)GamePrefs.GetDefault(EnumGamePrefs.ServerEnabled));
		this.SelectedGameMode.ResetGamePrefs();
		this.Settings.UpdateOptionValuesFromGamePrefs();
		this.Settings.UpdateServerEnabledState();
	}

	// Token: 0x06006F16 RID: 28438 RVA: 0x002AF638 File Offset: 0x002AD838
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

	// Token: 0x06006F17 RID: 28439 RVA: 0x002AF6A4 File Offset: 0x002AD8A4
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

	// Token: 0x06006F18 RID: 28440 RVA: 0x002AF6F4 File Offset: 0x002AD8F4
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

	// Token: 0x06006F19 RID: 28441 RVA: 0x002AF704 File Offset: 0x002AD904
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

	// Token: 0x06006F1A RID: 28442 RVA: 0x002AF75C File Offset: 0x002AD95C
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

	// Token: 0x06006F1B RID: 28443 RVA: 0x002AF7BC File Offset: 0x002AD9BC
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

	// Token: 0x06006F1C RID: 28444 RVA: 0x002AF8A0 File Offset: 0x002ADAA0
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

	// Token: 0x06006F1D RID: 28445 RVA: 0x002AF940 File Offset: 0x002ADB40
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

	// Token: 0x06006F1E RID: 28446 RVA: 0x002AFA24 File Offset: 0x002ADC24
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

	// Token: 0x06006F1F RID: 28447 RVA: 0x002AFA7E File Offset: 0x002ADC7E
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.doLoadSaveGameAutomation();
		this.updateInput();
	}

	// Token: 0x06006F20 RID: 28448 RVA: 0x002AFA94 File Offset: 0x002ADC94
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

	// Token: 0x06006F23 RID: 28451 RVA: 0x002AFB84 File Offset: 0x002ADD84
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

	// Token: 0x06006F24 RID: 28452 RVA: 0x002AFBF0 File Offset: 0x002ADDF0
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

	// Token: 0x0400542F RID: 21551
	public static string ID = "";

	// Token: 0x04005430 RID: 21552
	[XuiBindComponent("cbxGameMode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<GameMode> cbxGameMode;

	// Token: 0x04005431 RID: 21553
	[XuiBindComponent("cbxWorldType", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<XUiC_NewGame.EWorldType> cbxWorldType;

	// Token: 0x04005432 RID: 21554
	[XuiBindComponent("cbxWorldName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_NewGame.GameWorldInfo> cbxWorldName;

	// Token: 0x04005433 RID: 21555
	[XuiBindComponent("txtGameName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtGameName;

	// Token: 0x04005434 RID: 21556
	[XuiBindComponent("btnDefaults", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x04005435 RID: 21557
	[XuiBindComponent("btnGenerateWorld", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnGenerateWorld;

	// Token: 0x04005436 RID: 21558
	[XuiBindComponent("cbxSaveDataLimit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x04005437 RID: 21559
	[PublicizedFrom(EAccessModifier.Private)]
	public string saveDataLimitTooltipKey = string.Empty;

	// Token: 0x04005438 RID: 21560
	[XuiBindComponent("cbxGameSaveStorage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxGameSaveStorage;

	// Token: 0x04005439 RID: 21561
	[PublicizedFrom(EAccessModifier.Private)]
	public string saveStorageTooltipKey = string.Empty;

	// Token: 0x0400543A RID: 21562
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow worldGenerationControls;

	// Token: 0x0400543B RID: 21563
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EnumGameMode, List<XUiC_NewGame.GameWorldInfo>> worldsPerMode = new EnumDictionary<EnumGameMode, List<XUiC_NewGame.GameWorldInfo>>();

	// Token: 0x0400543C RID: 21564
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingPreviewSize;

	// Token: 0x0400543D RID: 21565
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameNameTooltip = "";

	// Token: 0x0400543E RID: 21566
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validGameName;

	// Token: 0x0400543F RID: 21567
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmOnOpen = new ProfilerMarker("OnOpen");

	// Token: 0x02000E1E RID: 3614
	public enum EWorldType
	{
		// Token: 0x04005441 RID: 21569
		Handmade,
		// Token: 0x04005442 RID: 21570
		ExistingRandom,
		// Token: 0x04005443 RID: 21571
		CreateRandom
	}

	// Token: 0x02000E1F RID: 3615
	public struct GameWorldInfo
	{
		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06006F25 RID: 28453 RVA: 0x002AFC5E File Offset: 0x002ADE5E
		public bool IsRandomWorld
		{
			get
			{
				GameUtils.WorldInfo worldInfo = this.WorldInfo;
				return worldInfo != null && worldInfo.RandomGeneratedWorld;
			}
		}

		// Token: 0x06006F26 RID: 28454 RVA: 0x002AFC74 File Offset: 0x002ADE74
		public override string ToString()
		{
			if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.Location.Type == PathAbstractions.EAbstractedLocationType.UserDataPath)
			{
				return this.RealName + " [808080][i](" + this.Location.StorageType.LocalizedName() + ")[/i][-]";
			}
			return this.RealName;
		}

		// Token: 0x04005444 RID: 21572
		public string RealName;

		// Token: 0x04005445 RID: 21573
		public string CustName;

		// Token: 0x04005446 RID: 21574
		public string Description;

		// Token: 0x04005447 RID: 21575
		public GameUtils.WorldInfo WorldInfo;

		// Token: 0x04005448 RID: 21576
		public PathAbstractions.AbstractedLocation Location;
	}
}
