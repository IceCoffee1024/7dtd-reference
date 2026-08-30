using System;
using System.Collections;
using Platform;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DE2 RID: 3554
[Preserve]
public class XUiC_ContinueGame : XUiC_NewContinueBase
{
	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x06006CC9 RID: 27849 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000C0A RID: 3082
	// (get) Token: 0x06006CCA RID: 27850 RVA: 0x002A4A75 File Offset: 0x002A2C75
	public override bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent();
		}
	}

	// Token: 0x17000C0B RID: 3083
	// (get) Token: 0x06006CCB RID: 27851 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[XuiXmlBinding("iscontinuegame")]
	public bool IsContinueGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x06006CCC RID: 27852 RVA: 0x002A4A81 File Offset: 0x002A2C81
	[XuiXmlBinding("saveselected")]
	public bool SaveSelected
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_SavegamesList xuiC_SavegamesList = this.savesList;
			return ((xuiC_SavegamesList != null) ? xuiC_SavegamesList.SelectedEntryData : null) != null;
		}
	}

	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x06006CCD RID: 27853 RVA: 0x002A4A98 File Offset: 0x002A2C98
	[XuiXmlBinding("saveplayable")]
	public bool SavePlayable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_SavegamesList xuiC_SavegamesList = this.savesList;
			bool? flag;
			if (xuiC_SavegamesList == null)
			{
				flag = null;
			}
			else
			{
				XUiC_SavegamesList.ListEntry selectedEntryData = xuiC_SavegamesList.SelectedEntryData;
				flag = ((selectedEntryData != null) ? new bool?(selectedEntryData.Playable) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}

	// Token: 0x06006CCE RID: 27854 RVA: 0x002A4AE0 File Offset: 0x002A2CE0
	public override void Init()
	{
		base.Init();
		XUiC_ContinueGame.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006CCF RID: 27855 RVA: 0x002A4AF8 File Offset: 0x002A2CF8
	public override void OnOpen()
	{
		this.IsDirty = true;
		this.Settings.WatchForServerEnabledChanges(false);
		base.OnOpen();
		if (this.savesList.EntryCount > 0)
		{
			string @string = GamePrefs.GetString(EnumGamePrefs.GameWorld);
			string string2 = GamePrefs.GetString(EnumGamePrefs.GameName);
			UserDataStorageType @int = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.GameSaveStorageType);
			this.SelectSaveEntry(@string, string2, @int);
			this.validateStartable();
		}
		else
		{
			string text = GamePrefs.GetString(EnumGamePrefs.GameMode);
			if (string.IsNullOrEmpty(text))
			{
				text = (string)GamePrefs.GetDefault(EnumGamePrefs.GameMode);
			}
			GameMode gameMode = GameMode.GetGameModeForName(text);
			if (gameMode == null)
			{
				text = (string)GamePrefs.GetDefault(EnumGamePrefs.GameMode);
				gameMode = GameMode.GetGameModeForName(text);
			}
			this.gameModeChanged(gameMode);
			this.validateStartable();
			base.GetChildById("btnBack").SelectCursorElement(true, false);
			this.updateBarSelectionValues();
		}
		this.Settings.ApplyInitialServerEnabledState();
		if (base.DataManagementBarEnabled)
		{
			SaveInfoProvider.Instance.SetDirty();
			this.DataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
			this.DataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Primary);
			base.updateBarUsageAndAllowanceValues();
		}
		GamePrefs.SetObject(EnumGamePrefs.ResetUnprotectedChunks, GamePrefs.GetDefault(EnumGamePrefs.ResetUnprotectedChunks));
	}

	// Token: 0x06006CD0 RID: 27856 RVA: 0x002A4C0C File Offset: 0x002A2E0C
	[XuiBindEvent("ListEntryDoubleClicked", "savesList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SavesList_OnEntryDoubleClicked(XUiC_List<XUiC_SavegamesList.ListEntry> _list, XUiC_SavegamesList.ListEntry _entry)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && this.savesList.SelectedEntryIndex >= 0)
		{
			base.BtnStart_OnPressed(this, -1);
		}
	}

	// Token: 0x06006CD1 RID: 27857 RVA: 0x002A4C38 File Offset: 0x002A2E38
	[XuiBindEvent("SelectionChanged", "savesList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SavesList_OnSelectionChanged(XUiC_List<XUiC_SavegamesList.ListEntry> _list, XUiC_SavegamesList.ListEntry _previousEntry, XUiC_SavegamesList.ListEntry _newEntry)
	{
		this.IsDirty = true;
		if (_newEntry != null)
		{
			this.Settings.SetGamePrefsToDefaults();
			GamePrefs.Instance.Load(_newEntry.GetSaveDir() + "/gameOptions.sdf");
			GamePrefs.SetObject(EnumGamePrefs.ResetUnprotectedChunks, GamePrefs.GetDefault(EnumGamePrefs.ResetUnprotectedChunks));
			this.gameModeChanged(_newEntry.GameMode);
			GamePrefs.Set(EnumGamePrefs.GameName, _newEntry.SaveName);
			GamePrefs.Set(EnumGamePrefs.GameWorld, _newEntry.WorldName);
			GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, (int)_newEntry.StorageType);
			this.Settings.RefreshMultiplayerOptionStates(GamePrefs.GetBool(EnumGamePrefs.ServerEnabled));
		}
		this.updateBarSelectionValues();
		this.validateStartable();
	}

	// Token: 0x06006CD2 RID: 27858 RVA: 0x002A4CE7 File Offset: 0x002A2EE7
	[PublicizedFrom(EAccessModifier.Private)]
	public void SelectSaveEntry(string _gameWorld, string _gameName, UserDataStorageType _storageType)
	{
		if (!this.savesList.TrySelectEntry(_gameWorld, _gameName, _storageType))
		{
			this.savesList.SelectedEntryIndex = 0;
		}
		this.savesList.SelectCursorElementForSelectedEntry();
	}

	// Token: 0x06006CD3 RID: 27859 RVA: 0x002A4D10 File Offset: 0x002A2F10
	[XuiBindEvent("OnPress", "btnDeleteSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.deleteSaveWithConfirmation();
	}

	// Token: 0x06006CD4 RID: 27860 RVA: 0x002A4D18 File Offset: 0x002A2F18
	[XuiBindEvent("OnHover", "btnDeleteSave")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteSave_OnHover(XUiController _sender, bool _isOver)
	{
		if (base.DataManagementBarEnabled)
		{
			this.DataManagementBar.SetDeleteHovered(_isOver);
		}
	}

	// Token: 0x06006CD5 RID: 27861 RVA: 0x002A4D30 File Offset: 0x002A2F30
	[PublicizedFrom(EAccessModifier.Private)]
	public void deleteSaveWithConfirmation()
	{
		XUiC_ContinueGame.<>c__DisplayClass20_0 CS$<>8__locals1 = new XUiC_ContinueGame.<>c__DisplayClass20_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.saveEntry = this.savesList.SelectedEntryData;
		XUiC_MessageBoxWindowGroup.ShowConfirmCancel(this.xui, Localization.Get("xuiDeleteSaveGame", false, null), string.Format(Localization.Get("xuiSavegameDeleteConfirmation", false, null), CS$<>8__locals1.saveEntry.SaveName), "", new Action(CS$<>8__locals1.<deleteSaveWithConfirmation>g__Confirm|0), new Action(CS$<>8__locals1.<deleteSaveWithConfirmation>g__Cancel|1), 1.5f, false, false, false);
	}

	// Token: 0x06006CD6 RID: 27862 RVA: 0x002A4DB4 File Offset: 0x002A2FB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnOpenSandboxSettingsRequested()
	{
		XUiC_SavegamesList.ListEntry selectedEntryData = this.savesList.SelectedEntryData;
		SandboxOptionManager.Current.SetWorldAndGame(selectedEntryData.WorldName, selectedEntryData.SaveName);
		base.OnOpenSandboxSettingsRequested();
	}

	// Token: 0x06006CD7 RID: 27863 RVA: 0x002A4DEC File Offset: 0x002A2FEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDataManagementWindowClosed()
	{
		base.updateBarUsageAndAllowanceValues();
		if (base.DataManagementBarEnabled)
		{
			this.DataManagementBar.SetDisplayMode(XUiC_DataManagementBar.DisplayMode.Selection);
			this.DataManagementBar.SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion.None);
		}
		XUiC_SavegamesList.ListEntry selectedEntryData = this.savesList.SelectedEntryData;
		string text = null;
		string text2 = null;
		UserDataStorageType userDataStorageType = UserDataStorageType.DeviceLocal;
		if (selectedEntryData != null)
		{
			text = selectedEntryData.WorldName;
			text2 = selectedEntryData.SaveName;
			userDataStorageType = selectedEntryData.StorageType;
		}
		this.savesList.RebuildList(false);
		this.SelectSaveEntry(text, text2, userDataStorageType);
		base.RefreshBindings();
	}

	// Token: 0x06006CD8 RID: 27864 RVA: 0x002A4E68 File Offset: 0x002A3068
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBarSelectionValues()
	{
		if (!base.DataManagementBarEnabled)
		{
			return;
		}
		XUiC_SavegamesList.ListEntry selectedEntryData = this.savesList.SelectedEntryData;
		SaveInfoProvider.SaveEntryInfo saveEntryInfo;
		if (selectedEntryData != null && SaveInfoProvider.Instance.TryGetLocalSaveEntry(selectedEntryData.WorldName, selectedEntryData.SaveName, selectedEntryData.StorageType, out saveEntryInfo) && selectedEntryData.StorageType.UsesDataLimit())
		{
			XUiC_DataManagementBar.BarRegion barRegion = new XUiC_DataManagementBar.BarRegion(saveEntryInfo.BarStartOffset, saveEntryInfo.SizeInfo.ReportedSize);
			this.DataManagementBar.SetSelectedByteRegion(barRegion);
			this.DataManagementBar.SetSelectionDepth(XUiC_DataManagementBar.SelectionDepth.Primary);
			return;
		}
		this.DataManagementBar.SetSelectedByteRegion(XUiC_DataManagementBar.BarRegion.None);
	}

	// Token: 0x06006CD9 RID: 27865 RVA: 0x002A4EFB File Offset: 0x002A30FB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override IEnumerator startGameCo()
	{
		XUiC_SavegamesList.ListEntry selectedSave = this.savesList.SelectedEntryData;
		if (selectedSave.SaveVersion.ReleaseType != Constants.cVersionInformation.ReleaseType || selectedSave.SaveVersion.Major != Constants.cVersionInformation.Major)
		{
			XUiC_ContinueGame.<>c__DisplayClass24_0 CS$<>8__locals1 = new XUiC_ContinueGame.<>c__DisplayClass24_0();
			CS$<>8__locals1.selection = null;
			XUiC_MessageBoxWindowGroup.ShowOkCancel(this.xui, Localization.Get("xuiSaveDifferentGameVersion", false, null), string.Format(Localization.Get("xuiSaveDifferentGameVersionText", false, null), selectedSave.SaveName, selectedSave.SaveVersion.ShortString, Constants.cVersionInformation.ShortString), "", delegate
			{
				CS$<>8__locals1.selection = new bool?(true);
			}, delegate
			{
				CS$<>8__locals1.selection = new bool?(false);
			}, false, false, false);
			while (CS$<>8__locals1.selection == null)
			{
				yield return null;
			}
			if (!CS$<>8__locals1.selection.Value)
			{
				yield break;
			}
			CS$<>8__locals1 = null;
		}
		SaveInfoProvider.SaveEntryInfo saveEntryInfo;
		if (selectedSave.WorldName.Equals("Playtesting") || selectedSave.WorldName.Equals("Empty"))
		{
			GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, 4);
		}
		else if (SaveInfoProvider.Instance.TryGetLocalSaveEntry(selectedSave.WorldName, selectedSave.SaveName, selectedSave.StorageType, out saveEntryInfo) && saveEntryInfo.WorldEntry.Location.Type != PathAbstractions.EAbstractedLocationType.None)
		{
			GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, (int)saveEntryInfo.WorldEntry.Location.Type);
			GamePrefs.Set(EnumGamePrefs.UserWorldStorageType, (int)saveEntryInfo.WorldEntry.Location.StorageType);
		}
		else
		{
			XUiC_ContinueGame.<>c__DisplayClass24_1 CS$<>8__locals2 = new XUiC_ContinueGame.<>c__DisplayClass24_1();
			CS$<>8__locals2.worldLocation = null;
			XUiC_WorldSelectionPopup.Open(this.xui, "xuiWorldConflict", selectedSave.WorldName, delegate
			{
				CS$<>8__locals2.worldLocation = new PathAbstractions.AbstractedLocation?(PathAbstractions.AbstractedLocation.None);
			}, delegate(PathAbstractions.AbstractedLocation _location)
			{
				CS$<>8__locals2.worldLocation = new PathAbstractions.AbstractedLocation?(_location);
			});
			while (CS$<>8__locals2.worldLocation == null)
			{
				yield return null;
			}
			if (CS$<>8__locals2.worldLocation.Value.Type == PathAbstractions.EAbstractedLocationType.None)
			{
				yield break;
			}
			GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, (int)CS$<>8__locals2.worldLocation.Value.Type);
			GamePrefs.Set(EnumGamePrefs.UserWorldStorageType, (int)CS$<>8__locals2.worldLocation.Value.StorageType);
			CS$<>8__locals2 = null;
		}
		if (GamePrefs.GetInt(EnumGamePrefs.ResetUnprotectedChunks) != 0)
		{
			XUiC_ContinueGame.<>c__DisplayClass24_2 CS$<>8__locals3 = new XUiC_ContinueGame.<>c__DisplayClass24_2();
			CS$<>8__locals3.resetConfirm = null;
			XUiC_MessageBoxWindowGroup.ShowOkCancel(this.xui, Localization.Get("goResetUnprotectedChunksMode", false, null), Localization.Get("goResetUnprotectedChunksModeConfirmation", false, null), "", delegate
			{
				CS$<>8__locals3.resetConfirm = new bool?(true);
			}, delegate
			{
				CS$<>8__locals3.resetConfirm = new bool?(false);
			}, false, false, false);
			while (CS$<>8__locals3.resetConfirm == null)
			{
				yield return null;
			}
			if (!CS$<>8__locals3.resetConfirm.Value)
			{
				yield break;
			}
			CS$<>8__locals3 = null;
		}
		GamePrefs.SetPersistent(EnumGamePrefs.GameMode, true);
		this.Settings.SaveGameOptions(false);
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
		yield break;
	}

	// Token: 0x06006CDA RID: 27866 RVA: 0x002A4F0A File Offset: 0x002A310A
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameModeChanged(GameMode _newMode)
	{
		GamePrefs.Set(EnumGamePrefs.GameMode, _newMode.GetTypeName());
		this.Settings.UpdateOptionVisibilityForGameMode(_newMode);
		this.Settings.UpdateOptionValuesFromGamePrefs();
		this.validateStartable();
	}

	// Token: 0x06006CDB RID: 27867 RVA: 0x002A4F38 File Offset: 0x002A3138
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void validateStartable()
	{
		bool flag = this.savesList.SelectedEntryIndex >= 0;
		if (flag)
		{
			XUiC_SavegamesList.ListEntry selectedEntryData = this.savesList.SelectedEntryData;
			flag = selectedEntryData.Playable;
			if (flag && !selectedEntryData.WorldExists)
			{
				flag = false;
			}
		}
		flag &= this.Settings.PortValid;
		base.ValidStartableState = flag;
	}

	// Token: 0x06006CDC RID: 27868 RVA: 0x002A4F90 File Offset: 0x002A3190
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateInput()
	{
		if (!XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false))
		{
			return;
		}
		if (Input.GetKeyUp(KeyCode.Delete) && this.savesList.SelectedEntryData != null)
		{
			this.deleteSaveWithConfirmation();
		}
		if (this.xui.playerUI.playerInput.GUIActions.Apply.WasPressed)
		{
			base.BtnStart_OnPressed(null, 0);
		}
	}

	// Token: 0x06006CDD RID: 27869 RVA: 0x002A5006 File Offset: 0x002A3206
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.doLoadSaveGameAutomation();
		this.updateInput();
	}

	// Token: 0x06006CDE RID: 27870 RVA: 0x002A501C File Offset: 0x002A321C
	[PublicizedFrom(EAccessModifier.Private)]
	public void doLoadSaveGameAutomation()
	{
		EPlatformLoadSaveGameState state = PlatformApplicationManager.LoadSaveGame.GetState();
		if (state != EPlatformLoadSaveGameState.ContinueGameSelect)
		{
			if (state != EPlatformLoadSaveGameState.ContinueGamePlay)
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
			string gameName = PlatformApplicationManager.LoadSaveGame.GameName;
			UserDataStorageType saveStorageType = PlatformApplicationManager.LoadSaveGame.SaveStorageType;
			XUiC_SavegamesList.ListEntry listEntry = this.savesList.SelectedEntryData;
			if (listEntry == null || !listEntry.WorldName.EqualsCaseInsensitive(gameWorld) || !listEntry.SaveName.EqualsCaseInsensitive(gameName))
			{
				if (!this.savesList.TrySelectEntry(gameWorld, gameName, saveStorageType))
				{
					PlatformApplicationManager.LoadSaveGame.SetFailed();
					return;
				}
				listEntry = this.savesList.SelectedEntryData;
			}
			if (listEntry == null || !listEntry.WorldName.EqualsCaseInsensitive(gameWorld) || !listEntry.SaveName.EqualsCaseInsensitive(gameName))
			{
				PlatformApplicationManager.LoadSaveGame.SetFailed();
				return;
			}
			PlatformApplicationManager.LoadSaveGame.AdvanceStateFrom(state);
			return;
		}
	}

	// Token: 0x040052BF RID: 21183
	public static string ID = "";

	// Token: 0x040052C0 RID: 21184
	[XuiBindComponent("saves", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SavegamesList savesList;

	// Token: 0x040052C1 RID: 21185
	[XuiBindComponent("btnDeleteSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDeleteSave;
}
