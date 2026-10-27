using System;
using System.Collections.Generic;
using Platform;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E35 RID: 3637
[Preserve]
public class XUiC_NewContinueGameSettings : XUiController
{
	// Token: 0x1400009B RID: 155
	// (add) Token: 0x06006EE4 RID: 28388 RVA: 0x002B2900 File Offset: 0x002B0B00
	// (remove) Token: 0x06006EE5 RID: 28389 RVA: 0x002B2938 File Offset: 0x002B0B38
	public event Action SettingsChanged;

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06006EE6 RID: 28390 RVA: 0x002B2970 File Offset: 0x002B0B70
	// (remove) Token: 0x06006EE7 RID: 28391 RVA: 0x002B29A8 File Offset: 0x002B0BA8
	public event Action OpenSandboxSettingsRequested;

	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x06006EE8 RID: 28392 RVA: 0x002B29E0 File Offset: 0x002B0BE0
	[XuiXmlBinding("crossplayTooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			string permissionDenyReason = PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.All);
			if (!string.IsNullOrEmpty(permissionDenyReason))
			{
				return permissionDenyReason;
			}
			if ((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent() && this.gameOptions.Count > 0)
			{
				this.gameOptions[EnumGamePrefs.ServerEACPeerToPeer].CheckDefaultValue();
				return string.Format(Localization.Get("xuiOptionsGeneralCrossplayTooltipPC", false, null), 8);
			}
			return Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x06006EE9 RID: 28393 RVA: 0x002B2A4C File Offset: 0x002B0C4C
	// (set) Token: 0x06006EEA RID: 28394 RVA: 0x002B2A54 File Offset: 0x002B0C54
	[XuiXmlBinding("port_valid")]
	public bool PortValid
	{
		get
		{
			return this.portValid;
		}
		set
		{
			if (this.portValid == value)
			{
				return;
			}
			this.portValid = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006EEB RID: 28395 RVA: 0x002B2A70 File Offset: 0x002B0C70
	public override void Init()
	{
		base.Init();
		XUiC_GamePrefSelector[] childControllers = base.GetChildControllers<XUiC_GamePrefSelector>("", null);
		int i = 0;
		while (i < childControllers.Length)
		{
			XUiC_GamePrefSelector xuiC_GamePrefSelector = childControllers[i];
			XUiC_GamePrefSelector xuiC_GamePrefSelector2 = xuiC_GamePrefSelector;
			xuiC_GamePrefSelector2.OnValueChanged = (Action<XUiC_GamePrefSelector, EnumGamePrefs>)Delegate.Combine(xuiC_GamePrefSelector2.OnValueChanged, new Action<XUiC_GamePrefSelector, EnumGamePrefs>(this.Option_OnValueChanged));
			EnumGamePrefs gamePref = xuiC_GamePrefSelector.GamePref;
			if (gamePref <= EnumGamePrefs.ServerEACPeerToPeer)
			{
				if (gamePref == EnumGamePrefs.ServerPort)
				{
					xuiC_GamePrefSelector.ControlText.UIInput.validation = UIInput.Validation.Integer;
					xuiC_GamePrefSelector.ControlText.OnChangeHandler += this.TxtPort_OnChangeHandler;
					goto IL_0116;
				}
				if (gamePref != EnumGamePrefs.ServerEACPeerToPeer)
				{
					goto IL_0116;
				}
				XUiC_GamePrefSelector xuiC_GamePrefSelector3 = xuiC_GamePrefSelector;
				xuiC_GamePrefSelector3.OnValueChanged = (Action<XUiC_GamePrefSelector, EnumGamePrefs>)Delegate.Combine(xuiC_GamePrefSelector3.OnValueChanged, new Action<XUiC_GamePrefSelector, EnumGamePrefs>(delegate(XUiC_GamePrefSelector _, EnumGamePrefs _)
				{
					this.RefreshMultiplayerOptionStates(GamePrefs.GetBool(EnumGamePrefs.ServerEnabled));
				}));
				goto IL_0116;
			}
			else
			{
				if (gamePref == EnumGamePrefs.BloodMoonFrequency)
				{
					XUiC_GamePrefSelector xuiC_GamePrefSelector4 = xuiC_GamePrefSelector;
					xuiC_GamePrefSelector4.OnValueChanged = (Action<XUiC_GamePrefSelector, EnumGamePrefs>)Delegate.Combine(xuiC_GamePrefSelector4.OnValueChanged, new Action<XUiC_GamePrefSelector, EnumGamePrefs>(this.CmbBloodMoonFrequency_OnChangeHandler));
					goto IL_0116;
				}
				if (gamePref == EnumGamePrefs.AirDropFrequency)
				{
					xuiC_GamePrefSelector.ValuePreDisplayModifierFunc = (int _n) => _n / 24;
					goto IL_0116;
				}
				if (gamePref != EnumGamePrefs.ServerEnabled)
				{
					goto IL_0116;
				}
				this.serverEnabledSelector = xuiC_GamePrefSelector;
			}
			IL_0123:
			i++;
			continue;
			IL_0116:
			this.gameOptions.Add(gamePref, xuiC_GamePrefSelector);
			goto IL_0123;
		}
	}

	// Token: 0x06006EEC RID: 28396 RVA: 0x002B2BB0 File Offset: 0x002B0DB0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && this.tabSelector != null)
		{
			foreach (XUiC_TabSelectorTab xuiC_TabSelectorTab in this.tabSelector.Tabs)
			{
				xuiC_TabSelectorTab.GetChildControllers<XUiC_GamePrefSelector>("", this.tabPrefsTmp);
				bool flag = false;
				foreach (XUiC_GamePrefSelector xuiC_GamePrefSelector in this.tabPrefsTmp)
				{
					flag |= xuiC_GamePrefSelector.ViewComponent.IsVisible && xuiC_GamePrefSelector.Enabled && !xuiC_GamePrefSelector.IsDefaultValueForGameMode();
				}
				xuiC_TabSelectorTab.TabHighlight = flag;
				this.tabPrefsTmp.Clear();
			}
		}
		if (base.handleDirtyUpdateDefault())
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.sandboxPresetValues.ResetPosition), 3);
		}
	}

	// Token: 0x06006EED RID: 28397 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Option_OnValueChanged(XUiC_GamePrefSelector _uiElement, EnumGamePrefs _gamePref)
	{
		this.IsDirty = true;
	}

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x06006EEE RID: 28398 RVA: 0x002B2CAC File Offset: 0x002B0EAC
	// (set) Token: 0x06006EEF RID: 28399 RVA: 0x002B2CB4 File Offset: 0x002B0EB4
	[XuiXmlAttribute("sandbox_values_columns", false)]
	public int SandboxValuesColumns
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x06006EF0 RID: 28400 RVA: 0x002B2CBD File Offset: 0x002B0EBD
	// (set) Token: 0x06006EF1 RID: 28401 RVA: 0x002B2CC5 File Offset: 0x002B0EC5
	[XuiXmlAttribute("sandbox_values_template", false)]
	public string SandBoxValuesTemplate
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.sandBoxValuesTemplate;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == null)
			{
				this.sandBoxValuesTemplate = "";
				return;
			}
			this.sandBoxValuesTemplate = value.Replace("%0", "{0}").Replace("%1", "{1}");
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x06006EF2 RID: 28402 RVA: 0x002B2CFB File Offset: 0x002B0EFB
	// (set) Token: 0x06006EF3 RID: 28403 RVA: 0x002B2D03 File Offset: 0x002B0F03
	[XuiXmlAttribute("sandbox_values_template_defaults", false)]
	public string SandBoxValuesTemplateDefaults
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.sandBoxValuesTemplateDefaults;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == null)
			{
				this.sandBoxValuesTemplateDefaults = "";
				return;
			}
			this.sandBoxValuesTemplateDefaults = value.Replace("%0", "{0}").Replace("%1", "{1}");
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x06006EF4 RID: 28404 RVA: 0x002B2D39 File Offset: 0x002B0F39
	[XuiXmlBinding("sandboxvalues")]
	public Dictionary<string, string> SandboxValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	} = new Dictionary<string, string>();

	// Token: 0x06006EF5 RID: 28405 RVA: 0x002B2D41 File Offset: 0x002B0F41
	[XuiBindEvent("OnPress", "btnSandboxOptions")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSandboxOptions_OnPressed(XUiController _sender, int _mouseButton)
	{
		Action openSandboxSettingsRequested = this.OpenSandboxSettingsRequested;
		if (openSandboxSettingsRequested == null)
		{
			return;
		}
		openSandboxSettingsRequested();
	}

	// Token: 0x06006EF6 RID: 28406 RVA: 0x002B2D53 File Offset: 0x002B0F53
	public void ResetSandboxPresetToDefault()
	{
		this.sandboxPresetSelector.SelectPresetByName("", null, false);
	}

	// Token: 0x06006EF7 RID: 28407 RVA: 0x002B2D68 File Offset: 0x002B0F68
	public void UpdateSandboxPresetGroups()
	{
		string @string = GamePrefs.GetString(EnumGamePrefs.SandboxPreset);
		if (@string == "Custom" || this.sandboxManager.GetPreset(@string) == null)
		{
			this.sandboxManager.LoadOptionsFromCode(GamePrefs.GetString(EnumGamePrefs.SandboxCode), SandboxOptionManager.CustomPreset);
			this.sandboxPresetSelector.SelectPresetByName("Custom", null, false);
		}
		else
		{
			this.sandboxPresetSelector.SelectPresetByName(@string, null, false);
		}
		this.updateSandboxData();
	}

	// Token: 0x06006EF8 RID: 28408 RVA: 0x002B2DDE File Offset: 0x002B0FDE
	[XuiBindEvent("SandboxPresetSelectionChanged", "sandboxPresetSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SandboxPresetSelector_OnValueChanged(SandboxPresetInfo _oldPreset, SandboxPresetInfo _newPreset)
	{
		this.updateSandboxData();
	}

	// Token: 0x06006EF9 RID: 28409 RVA: 0x002B2DE8 File Offset: 0x002B0FE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateSandboxData()
	{
		SandboxPresetInfo selectedPreset = this.sandboxPresetSelector.SelectedPreset;
		GamePrefs.Set(EnumGamePrefs.SandboxPreset, selectedPreset.InternalName);
		GamePrefs.Set(EnumGamePrefs.SandboxCode, selectedPreset.SandboxCode);
		if (selectedPreset.IsCustomPreset)
		{
			this.sandboxManager.LoadOptionsFromCode(selectedPreset.SandboxCode);
			this.sandboxDisplay.SandboxCode = selectedPreset.SandboxCode;
		}
		else
		{
			this.sandboxDisplay.SandboxName = selectedPreset.InternalName;
		}
		this.sandboxDisplay.IsDirty = true;
		this.IsDirty = true;
	}

	// Token: 0x06006EFA RID: 28410 RVA: 0x002B2E72 File Offset: 0x002B1072
	[PublicizedFrom(EAccessModifier.Private)]
	public void CmbBloodMoonFrequency_OnChangeHandler(XUiC_GamePrefSelector _prefSelector, EnumGamePrefs _gamePref)
	{
		this.gameOptions[EnumGamePrefs.BloodMoonRange].Enabled = GamePrefs.GetInt(_gamePref) != 0;
	}

	// Token: 0x06006EFB RID: 28411 RVA: 0x002B2E90 File Offset: 0x002B1090
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPort_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		XUiC_TextInput xuiC_TextInput = (XUiC_TextInput)_sender;
		if (_text.Length < 1)
		{
			xuiC_TextInput.Text = "0";
		}
		else if (_text.Length > 1 && _text[0] == '0')
		{
			xuiC_TextInput.Text = _text.Substring(1);
		}
		this.validatePort(xuiC_TextInput);
		Action settingsChanged = this.SettingsChanged;
		if (settingsChanged == null)
		{
			return;
		}
		settingsChanged();
	}

	// Token: 0x06006EFC RID: 28412 RVA: 0x002B2EF4 File Offset: 0x002B10F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void validatePort(XUiC_TextInput _txtPort)
	{
		int num;
		this.PortValid = StringParsers.TryParseSInt32(_txtPort.Text, out num) && num >= 1024 && num < 65533;
		if (!this.PortValid)
		{
			_txtPort.ActiveTextColor = Color.red;
		}
	}

	// Token: 0x06006EFD RID: 28413 RVA: 0x002B2F40 File Offset: 0x002B1140
	public void WatchForServerEnabledChanges(bool _doWatch)
	{
		if (_doWatch)
		{
			XUiC_GamePrefSelector xuiC_GamePrefSelector = this.serverEnabledSelector;
			xuiC_GamePrefSelector.OnValueChanged = (Action<XUiC_GamePrefSelector, EnumGamePrefs>)Delegate.Combine(xuiC_GamePrefSelector.OnValueChanged, new Action<XUiC_GamePrefSelector, EnumGamePrefs>(this.CmbServerEnabled_OnChangeHandler));
			return;
		}
		XUiC_GamePrefSelector xuiC_GamePrefSelector2 = this.serverEnabledSelector;
		xuiC_GamePrefSelector2.OnValueChanged = (Action<XUiC_GamePrefSelector, EnumGamePrefs>)Delegate.Remove(xuiC_GamePrefSelector2.OnValueChanged, new Action<XUiC_GamePrefSelector, EnumGamePrefs>(this.CmbServerEnabled_OnChangeHandler));
	}

	// Token: 0x06006EFE RID: 28414 RVA: 0x002B2FA0 File Offset: 0x002B11A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CmbServerEnabled_OnChangeHandler(XUiC_GamePrefSelector _prefSelector, EnumGamePrefs _gamePref)
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.ServerEnabled);
		this.RefreshMultiplayerOptionStates(@bool);
		if (@bool && (!PermissionsManager.IsMultiplayerAllowed() || !PermissionsManager.CanHostMultiplayer()))
		{
			XUiC_MultiplayerPrivilegeNotification.ResolvePrivilegesWithDialog(EUserPerms.HostMultiplayer, delegate(bool _result)
			{
				GamePrefs.Set(EnumGamePrefs.ServerEnabled, _result);
				this.serverEnabledSelector.SetCurrentValue();
			}, -1f, false, null);
		}
	}

	// Token: 0x06006EFF RID: 28415 RVA: 0x002B2FEC File Offset: 0x002B11EC
	public void RefreshMultiplayerOptionStates(bool _enabled)
	{
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		bool? flag;
		if (crossplatformPlatform == null)
		{
			flag = null;
		}
		else
		{
			IAntiCheatServer antiCheatServer = crossplatformPlatform.AntiCheatServer;
			flag = ((antiCheatServer != null) ? new bool?(antiCheatServer.ServerEacAvailable()) : null);
		}
		bool? flag2 = flag;
		bool flag3 = flag2.GetValueOrDefault() && _enabled;
		IPlatform crossplatformPlatform2 = PlatformManager.CrossplatformPlatform;
		bool? flag4;
		if (crossplatformPlatform2 == null)
		{
			flag4 = null;
		}
		else
		{
			IAntiCheatServer antiCheatServer2 = crossplatformPlatform2.AntiCheatServer;
			flag4 = ((antiCheatServer2 != null) ? new bool?(antiCheatServer2.ServerEacEnabled()) : null);
		}
		flag2 = flag4;
		bool flag5 = flag2.GetValueOrDefault() && _enabled;
		bool flag6 = PermissionsManager.IsCrossplayAllowed() && (flag5 || !Submission.Enabled) && _enabled;
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.ServerEACPeerToPeer);
		bool bool2 = GamePrefs.GetBool(EnumGamePrefs.ServerAllowCrossplay);
		bool flag7 = (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent() || !Submission.Enabled;
		GamePrefs.Set(EnumGamePrefs.ServerEACPeerToPeer, !flag7 || flag5);
		this.gameOptions[EnumGamePrefs.ServerEACPeerToPeer].ViewComponent.IsVisible = flag7;
		this.gameOptions[EnumGamePrefs.ServerEACPeerToPeer].Enabled = flag3;
		if (@bool != GamePrefs.GetBool(EnumGamePrefs.ServerEACPeerToPeer))
		{
			this.gameOptions[EnumGamePrefs.ServerEACPeerToPeer].SetCurrentValue();
		}
		this.gameOptions[EnumGamePrefs.ServerEACPeerToPeer].CheckDefaultValue();
		if (_enabled)
		{
			GamePrefs.Set(EnumGamePrefs.ServerAllowCrossplay, bool2 && flag6);
		}
		this.gameOptions[EnumGamePrefs.ServerAllowCrossplay].Enabled = flag6;
		this.gameOptions[EnumGamePrefs.ServerAllowCrossplay].SetCurrentValue();
		this.gameOptions[EnumGamePrefs.ServerAllowCrossplay].CheckDefaultValue();
		this.gameOptions[EnumGamePrefs.Region].Enabled = _enabled;
		this.gameOptions[EnumGamePrefs.ServerVisibility].Enabled = _enabled;
		this.gameOptions[EnumGamePrefs.ServerPassword].Enabled = _enabled;
		this.gameOptions[EnumGamePrefs.ServerMaxPlayerCount].Enabled = _enabled;
		this.gameOptions[EnumGamePrefs.Region].CheckDefaultValue();
		this.gameOptions[EnumGamePrefs.ServerVisibility].CheckDefaultValue();
		this.gameOptions[EnumGamePrefs.ServerPassword].CheckDefaultValue();
		this.gameOptions[EnumGamePrefs.ServerMaxPlayerCount].CheckDefaultValue();
	}

	// Token: 0x06006F00 RID: 28416 RVA: 0x002B3204 File Offset: 0x002B1404
	public void ApplyMaxPlayerCountOptions()
	{
		if (GamePrefs.GetString(EnumGamePrefs.GameMode) != "GameMode" + EnumGameMode.Survival.ToStringCached<EnumGameMode>() || GameModeSurvival.OverrideMaxPlayerCount < 2)
		{
			return;
		}
		List<string> list = new List<string>();
		for (int i = 2; i <= GameModeSurvival.OverrideMaxPlayerCount; i++)
		{
			list.Add(i.ToString());
		}
		this.gameOptions[EnumGamePrefs.ServerMaxPlayerCount].OverrideValues(list);
	}

	// Token: 0x06006F01 RID: 28417 RVA: 0x002B3270 File Offset: 0x002B1470
	public void ApplyInitialServerEnabledState()
	{
		bool flag = GamePrefs.GetBool(EnumGamePrefs.ServerEnabled) && PermissionsManager.IsMultiplayerAllowed() && PermissionsManager.CanHostMultiplayer();
		GamePrefs.Set(EnumGamePrefs.ServerEnabled, flag);
		this.serverEnabledSelector.SetCurrentValue();
		if (PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.OfflineMode)
		{
			this.serverEnabledSelector.Enabled = false;
		}
		else
		{
			this.WatchForServerEnabledChanges(true);
		}
		this.RefreshMultiplayerOptionStates(flag);
	}

	// Token: 0x06006F02 RID: 28418 RVA: 0x002B32DD File Offset: 0x002B14DD
	public void UpdateServerEnabledState()
	{
		this.serverEnabledSelector.SetCurrentValue();
	}

	// Token: 0x06006F03 RID: 28419 RVA: 0x002B32EA File Offset: 0x002B14EA
	public void UpdateCrossplayEnabledState()
	{
		this.gameOptions[EnumGamePrefs.ServerAllowCrossplay].SetCurrentValue();
	}

	// Token: 0x06006F04 RID: 28420 RVA: 0x002B32FE File Offset: 0x002B14FE
	public void ApplyCreativeModeChangeAllowed(bool _allowed)
	{
		this.gameOptions[EnumGamePrefs.BuildCreate].Enabled = _allowed;
	}

	// Token: 0x06006F05 RID: 28421 RVA: 0x002B3314 File Offset: 0x002B1514
	public void SaveGameOptions(bool _saveAsLastUsed)
	{
		List<EnumGamePrefs> list = new List<EnumGamePrefs>(this.gameOptions.Count);
		this.gameOptions.CopyKeysTo<EnumGamePrefs, XUiC_GamePrefSelector>(list);
		list.Add(EnumGamePrefs.SandboxCode);
		list.Add(EnumGamePrefs.SandboxPreset);
		GamePrefs.Instance.Save(GameIO.GetSaveGameDir() + "/gameOptions.sdf", list);
		if (_saveAsLastUsed)
		{
			GamePrefs.Instance.Save(GameIO.GetSaveGameRootDir() + "/newGameOptions.sdf", list);
		}
	}

	// Token: 0x06006F06 RID: 28422 RVA: 0x002B338C File Offset: 0x002B158C
	public void UpdateOptionVisibilityForGameMode(GameMode _gameMode)
	{
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			keyValuePair.Value.SetCurrentGameMode(_gameMode);
		}
	}

	// Token: 0x06006F07 RID: 28423 RVA: 0x002B33E8 File Offset: 0x002B15E8
	public void UpdateOptionValuesFromGamePrefs()
	{
		this.UpdateSandboxPresetGroups();
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			keyValuePair.Value.SetCurrentValue();
		}
	}

	// Token: 0x06006F08 RID: 28424 RVA: 0x002B3448 File Offset: 0x002B1648
	public void SetGamePrefsToDefaults()
	{
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			GamePrefs.SetObject(keyValuePair.Key, GamePrefs.GetDefault(keyValuePair.Key));
		}
	}

	// Token: 0x04005479 RID: 21625
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector tabSelector;

	// Token: 0x0400547A RID: 21626
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EnumGamePrefs, XUiC_GamePrefSelector> gameOptions = new EnumDictionary<EnumGamePrefs, XUiC_GamePrefSelector>();

	// Token: 0x0400547B RID: 21627
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamePrefSelector serverEnabledSelector;

	// Token: 0x0400547E RID: 21630
	[PublicizedFrom(EAccessModifier.Private)]
	public bool portValid;

	// Token: 0x0400547F RID: 21631
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_GamePrefSelector> tabPrefsTmp = new List<XUiC_GamePrefSelector>();

	// Token: 0x04005480 RID: 21632
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxPresetSelector sandboxPresetSelector;

	// Token: 0x04005481 RID: 21633
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxSettingsDisplay sandboxDisplay;

	// Token: 0x04005482 RID: 21634
	[XuiBindComponent("btnSandboxOptions", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSandboxOptions;

	// Token: 0x04005483 RID: 21635
	[XuiBindComponent("sandboxPresetValues.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView sandboxPresetValues;

	// Token: 0x04005484 RID: 21636
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005485 RID: 21637
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;

	// Token: 0x04005486 RID: 21638
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplateDefaults;
}
