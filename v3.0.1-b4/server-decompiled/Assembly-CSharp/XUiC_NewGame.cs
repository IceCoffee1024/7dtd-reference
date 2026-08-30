using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using SandboxOptions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E08 RID: 3592
[Preserve]
public class XUiC_NewGame : XUiC_NewContinueBase
{
	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x06006E52 RID: 28242 RVA: 0x002AB6F4 File Offset: 0x002A98F4
	// (set) Token: 0x06006E53 RID: 28243 RVA: 0x002AB6FC File Offset: 0x002A98FC
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

	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x06006E54 RID: 28244 RVA: 0x002AB71B File Offset: 0x002A991B
	// (set) Token: 0x06006E55 RID: 28245 RVA: 0x002AB723 File Offset: 0x002A9923
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

	// Token: 0x17000C47 RID: 3143
	// (get) Token: 0x06006E56 RID: 28246 RVA: 0x002AB73D File Offset: 0x002A993D
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

	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x06006E57 RID: 28247 RVA: 0x002AB750 File Offset: 0x002A9950
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

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x06006E58 RID: 28248 RVA: 0x00010E62 File Offset: 0x0000F062
	[XuiXmlBinding("iscontinuegame")]
	public bool IsContinueGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x06006E59 RID: 28249 RVA: 0x002AB763 File Offset: 0x002A9963
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

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x06006E5A RID: 28250 RVA: 0x002AB776 File Offset: 0x002A9976
	[XuiXmlBinding("saveStorageTooltip")]
	public string SaveStorageTooltipKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveStorageTooltipKey;
		}
	}

	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x06006E5B RID: 28251 RVA: 0x002AB77E File Offset: 0x002A997E
	[XuiXmlBinding("saveDataLimitTooltip")]
	public string SaveDataLimitTooltipKey
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.saveDataLimitTooltipKey;
		}
	}

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x06006E5C RID: 28252 RVA: 0x002AB788 File Offset: 0x002A9988
	public XUiC_NewGame.GameWorldInfo? SelectedWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<XUiC_NewGame.GameWorldInfo> xuiC_ComboBoxList = this.cbxWorldName;
			return new XUiC_NewGame.GameWorldInfo?((xuiC_ComboBoxList != null) ? xuiC_ComboBoxList.Value : default(XUiC_NewGame.GameWorldInfo));
		}
	}

	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x06006E5D RID: 28253 RVA: 0x002AB7B4 File Offset: 0x002A99B4
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

	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06006E5E RID: 28254 RVA: 0x002AB7E0 File Offset: 0x002A99E0
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

	// Token: 0x17000C50 RID: 3152
	// (get) Token: 0x06006E5F RID: 28255 RVA: 0x002AB840 File Offset: 0x002A9A40
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

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x06006E60 RID: 28256 RVA: 0x002AB87C File Offset: 0x002A9A7C
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

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x06006E61 RID: 28257 RVA: 0x002AB8C3 File Offset: 0x002A9AC3
	[XuiXmlBinding("isgenerateworld")]
	public bool IsGenerateWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.WorldType == XUiC_NewGame.EWorldType.CreateRandom;
		}
	}

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x06006E62 RID: 28258 RVA: 0x002AB8D0 File Offset: 0x002A9AD0
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

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x06006E63 RID: 28259 RVA: 0x002AB905 File Offset: 0x002A9B05
	public override bool PlayIntroMovie
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return GamePrefs.GetBool(EnumGamePrefs.OptionsIntroMovieEnabled);
		}
	}

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x06006E64 RID: 28260 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowChangingCreativeMode
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06006E65 RID: 28261 RVA: 0x002AB911 File Offset: 0x002A9B11
	public override void Init()
	{
		base.Init();
		XUiC_NewGame.ID = base.WindowGroup.Id;
		this.txtGameName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
		SaveDataLimitUIHelper.AddComboBox(this.cbxSaveDataLimit);
	}

	// Token: 0x06006E66 RID: 28262 RVA: 0x002AB954 File Offset: 0x002A9B54
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

	// Token: 0x06006E67 RID: 28263 RVA: 0x002ABBCC File Offset: 0x002A9DCC
	public override void OnClose()
	{
		base.OnClose();
		SaveDataLimitUIHelper.OnValueChanged = (Action)Delegate.Remove(SaveDataLimitUIHelper.OnValueChanged, new Action(this.CbxSaveDataLimit_OnValueChanged));
		this.worldGenerationControls.OnWorldSizeChanged -= this.updateBarPendingValue;
	}

	// Token: 0x06006E68 RID: 28264 RVA: 0x002ABC0B File Offset: 0x002A9E0B
	[XuiBindEvent("OnPress", "btnGenerateWorld")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRwgPreviewerOnOnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_WorldGenerationWindow.Open(this.xui, XUiC_NewGame.ID);
	}

	// Token: 0x06006E69 RID: 28265 RVA: 0x002ABC1D File Offset: 0x002A9E1D
	[XuiBindEvent("OnValueChanged", "cbxGameMode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxGameMode_OnValueChanged(XUiController _sender, GameMode _oldValue, GameMode _newValue)
	{
		this.gameModeChanged(_newValue);
	}

	// Token: 0x06006E6A RID: 28266 RVA: 0x002ABC26 File Offset: 0x002A9E26
	[XuiBindEvent("OnValueChanged", "cbxWorldType")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxWorldType_OnValueChanged(XUiController _sender, XUiC_NewGame.EWorldType _oldValue, XUiC_NewGame.EWorldType _newValue)
	{
		this.IsDirty = true;
		this.updateWorlds();
		this.validateStartable();
	}

	// Token: 0x06006E6B RID: 28267 RVA: 0x002ABC3C File Offset: 0x002A9E3C
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

	// Token: 0x06006E6C RID: 28268 RVA: 0x002ABCA7 File Offset: 0x002A9EA7
	[XuiBindEvent("OnChangeHandler", "txtGameName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtGameName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		GamePrefs.Set(EnumGamePrefs.GameName, _text);
		this.validateStartable();
	}

	// Token: 0x06006E6D RID: 28269 RVA: 0x002ABCB7 File Offset: 0x002A9EB7
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxSaveDataLimit_OnValueChanged()
	{
		this.IsDirty = true;
		this.updateBarPendingValue();
	}

	// Token: 0x06006E6E RID: 28270 RVA: 0x002ABCC6 File Offset: 0x002A9EC6
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

	// Token: 0x06006E6F RID: 28271 RVA: 0x002ABCF2 File Offset: 0x002A9EF2
	[XuiBindEvent("OnGameSaveStorageChanged", "worldGenerationControls")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldGenerationGameSaveStorageChanged(UserDataStorageType _newValue)
	{
		this.IsDirty = true;
		this.cbxGameSaveStorage.Value = _newValue;
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006E70 RID: 28272 RVA: 0x002ABD13 File Offset: 0x002A9F13
	[XuiBindEvent("OnWorldStorageChanged", "worldGenerationControls")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void worldGenerationWorldSaveStorageChanged()
	{
		this.updateBarPendingValue();
		this.validateStartable();
	}

	// Token: 0x06006E71 RID: 28273 RVA: 0x002ABD24 File Offset: 0x002A9F24
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

	// Token: 0x06006E72 RID: 28274 RVA: 0x002ABF0C File Offset: 0x002AA10C
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

	// Token: 0x06006E73 RID: 28275 RVA: 0x002ABFD4 File Offset: 0x002AA1D4
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

	// Token: 0x06006E74 RID: 28276 RVA: 0x002AC0CC File Offset: 0x002AA2CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnOpenSandboxSettingsRequested()
	{
		SandboxOptionManager.Current.SetWorldAndGame("", "");
		base.OnOpenSandboxSettingsRequested();
	}

	// Token: 0x06006E75 RID: 28277 RVA: 0x002AC0E8 File Offset: 0x002AA2E8
	[XuiBindEvent("OnPress", "btnDefaults")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDefaults_OnPressed(XUiController _sender, int _mouseButton)
	{
		GamePrefs.Set(EnumGamePrefs.ServerEnabled, (bool)GamePrefs.GetDefault(EnumGamePrefs.ServerEnabled));
		this.SelectedGameMode.ResetGamePrefs();
		this.Settings.UpdateOptionValuesFromGamePrefs();
		this.Settings.UpdateServerEnabledState();
	}

	// Token: 0x06006E76 RID: 28278 RVA: 0x002AC124 File Offset: 0x002AA324
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

	// Token: 0x06006E77 RID: 28279 RVA: 0x002AC190 File Offset: 0x002AA390
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

	// Token: 0x06006E78 RID: 28280 RVA: 0x002AC1E0 File Offset: 0x002AA3E0
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
		yield break;
	}

	// Token: 0x06006E79 RID: 28281 RVA: 0x002AC1F0 File Offset: 0x002AA3F0
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

	// Token: 0x06006E7A RID: 28282 RVA: 0x002AC248 File Offset: 0x002AA448
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

	// Token: 0x06006E7B RID: 28283 RVA: 0x002AC2A8 File Offset: 0x002AA4A8
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

	// Token: 0x06006E7C RID: 28284 RVA: 0x002AC38C File Offset: 0x002AA58C
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

	// Token: 0x06006E7D RID: 28285 RVA: 0x002AC42C File Offset: 0x002AA62C
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

	// Token: 0x06006E7E RID: 28286 RVA: 0x002AC510 File Offset: 0x002AA710
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

	// Token: 0x06006E7F RID: 28287 RVA: 0x002AC56A File Offset: 0x002AA76A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.doLoadSaveGameAutomation();
		this.updateInput();
	}

	// Token: 0x06006E80 RID: 28288 RVA: 0x002AC580 File Offset: 0x002AA780
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

	// Token: 0x06006E83 RID: 28291 RVA: 0x002AC670 File Offset: 0x002AA870
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

	// Token: 0x06006E84 RID: 28292 RVA: 0x002AC6DC File Offset: 0x002AA8DC
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

	// Token: 0x0400538B RID: 21387
	public static string ID = "";

	// Token: 0x0400538C RID: 21388
	[XuiBindComponent("cbxGameMode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<GameMode> cbxGameMode;

	// Token: 0x0400538D RID: 21389
	[XuiBindComponent("cbxWorldType", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<XUiC_NewGame.EWorldType> cbxWorldType;

	// Token: 0x0400538E RID: 21390
	[XuiBindComponent("cbxWorldName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<XUiC_NewGame.GameWorldInfo> cbxWorldName;

	// Token: 0x0400538F RID: 21391
	[XuiBindComponent("txtGameName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtGameName;

	// Token: 0x04005390 RID: 21392
	[XuiBindComponent("btnDefaults", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x04005391 RID: 21393
	[XuiBindComponent("btnGenerateWorld", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnGenerateWorld;

	// Token: 0x04005392 RID: 21394
	[XuiBindComponent("cbxSaveDataLimit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<SaveDataLimitType> cbxSaveDataLimit;

	// Token: 0x04005393 RID: 21395
	[PublicizedFrom(EAccessModifier.Private)]
	public string saveDataLimitTooltipKey = string.Empty;

	// Token: 0x04005394 RID: 21396
	[XuiBindComponent("cbxGameSaveStorage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxEnum<UserDataStorageType> cbxGameSaveStorage;

	// Token: 0x04005395 RID: 21397
	[PublicizedFrom(EAccessModifier.Private)]
	public string saveStorageTooltipKey = string.Empty;

	// Token: 0x04005396 RID: 21398
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationWindow worldGenerationControls;

	// Token: 0x04005397 RID: 21399
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EnumGameMode, List<XUiC_NewGame.GameWorldInfo>> worldsPerMode = new EnumDictionary<EnumGameMode, List<XUiC_NewGame.GameWorldInfo>>();

	// Token: 0x04005398 RID: 21400
	[PublicizedFrom(EAccessModifier.Private)]
	public long pendingPreviewSize;

	// Token: 0x04005399 RID: 21401
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameNameTooltip = "";

	// Token: 0x0400539A RID: 21402
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validGameName;

	// Token: 0x0400539B RID: 21403
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ProfilerMarker pmOnOpen = new ProfilerMarker("OnOpen");

	// Token: 0x02000E09 RID: 3593
	public enum EWorldType
	{
		// Token: 0x0400539D RID: 21405
		Handmade,
		// Token: 0x0400539E RID: 21406
		ExistingRandom,
		// Token: 0x0400539F RID: 21407
		CreateRandom
	}

	// Token: 0x02000E0A RID: 3594
	public struct GameWorldInfo
	{
		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06006E85 RID: 28293 RVA: 0x002AC74A File Offset: 0x002AA94A
		public bool IsRandomWorld
		{
			get
			{
				GameUtils.WorldInfo worldInfo = this.WorldInfo;
				return worldInfo != null && worldInfo.RandomGeneratedWorld;
			}
		}

		// Token: 0x06006E86 RID: 28294 RVA: 0x002AC760 File Offset: 0x002AA960
		public override string ToString()
		{
			if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional && this.Location.Type == PathAbstractions.EAbstractedLocationType.UserDataPath)
			{
				return this.RealName + " [808080][i](" + this.Location.StorageType.LocalizedName() + ")[/i][-]";
			}
			return this.RealName;
		}

		// Token: 0x040053A0 RID: 21408
		public string RealName;

		// Token: 0x040053A1 RID: 21409
		public string CustName;

		// Token: 0x040053A2 RID: 21410
		public string Description;

		// Token: 0x040053A3 RID: 21411
		public GameUtils.WorldInfo WorldInfo;

		// Token: 0x040053A4 RID: 21412
		public PathAbstractions.AbstractedLocation Location;
	}
}
