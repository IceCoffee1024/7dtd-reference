using System;
using System.Collections.Generic;
using Platform;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E2A RID: 3626
[Preserve]
public class XUiC_NewContinueGameSettings : XUiController
{
	// Token: 0x1400009B RID: 155
	// (add) Token: 0x06006EBF RID: 28351 RVA: 0x002B1394 File Offset: 0x002AF594
	// (remove) Token: 0x06006EC0 RID: 28352 RVA: 0x002B13CC File Offset: 0x002AF5CC
	public event Action SettingsChanged;

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06006EC1 RID: 28353 RVA: 0x002B1404 File Offset: 0x002AF604
	// (remove) Token: 0x06006EC2 RID: 28354 RVA: 0x002B143C File Offset: 0x002AF63C
	public event Action OpenSandboxSettingsRequested;

	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x06006EC3 RID: 28355 RVA: 0x002B1474 File Offset: 0x002AF674
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

	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x06006EC4 RID: 28356 RVA: 0x002B14E0 File Offset: 0x002AF6E0
	// (set) Token: 0x06006EC5 RID: 28357 RVA: 0x002B14E8 File Offset: 0x002AF6E8
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

	// Token: 0x06006EC6 RID: 28358 RVA: 0x002B1504 File Offset: 0x002AF704
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

	// Token: 0x06006EC7 RID: 28359 RVA: 0x002B1644 File Offset: 0x002AF844
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

	// Token: 0x06006EC8 RID: 28360 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void Option_OnValueChanged(XUiC_GamePrefSelector _uiElement, EnumGamePrefs _gamePref)
	{
		this.IsDirty = true;
	}

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x06006EC9 RID: 28361 RVA: 0x002B1740 File Offset: 0x002AF940
	// (set) Token: 0x06006ECA RID: 28362 RVA: 0x002B1748 File Offset: 0x002AF948
	[XuiXmlAttribute("sandbox_values_columns", false)]
	public int SandboxValuesColumns
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x06006ECB RID: 28363 RVA: 0x002B1751 File Offset: 0x002AF951
	// (set) Token: 0x06006ECC RID: 28364 RVA: 0x002B1759 File Offset: 0x002AF959
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

	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x06006ECD RID: 28365 RVA: 0x002B178F File Offset: 0x002AF98F
	// (set) Token: 0x06006ECE RID: 28366 RVA: 0x002B1797 File Offset: 0x002AF997
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

	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x06006ECF RID: 28367 RVA: 0x002B17CD File Offset: 0x002AF9CD
	[XuiXmlBinding("sandboxvalues")]
	public Dictionary<string, string> SandboxValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	} = new Dictionary<string, string>();

	// Token: 0x06006ED0 RID: 28368 RVA: 0x002B17D5 File Offset: 0x002AF9D5
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

	// Token: 0x06006ED1 RID: 28369 RVA: 0x002B17E7 File Offset: 0x002AF9E7
	public void ResetSandboxPresetToDefault()
	{
		this.sandboxPresetSelector.SelectPresetByName("", null, false);
	}

	// Token: 0x06006ED2 RID: 28370 RVA: 0x002B17FC File Offset: 0x002AF9FC
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

	// Token: 0x06006ED3 RID: 28371 RVA: 0x002B1872 File Offset: 0x002AFA72
	[XuiBindEvent("SandboxPresetSelectionChanged", "sandboxPresetSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SandboxPresetSelector_OnValueChanged(SandboxPresetInfo _oldPreset, SandboxPresetInfo _newPreset)
	{
		this.updateSandboxData();
	}

	// Token: 0x06006ED4 RID: 28372 RVA: 0x002B187C File Offset: 0x002AFA7C
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

	// Token: 0x06006ED5 RID: 28373 RVA: 0x002B1906 File Offset: 0x002AFB06
	[PublicizedFrom(EAccessModifier.Private)]
	public void CmbBloodMoonFrequency_OnChangeHandler(XUiC_GamePrefSelector _prefSelector, EnumGamePrefs _gamePref)
	{
		this.gameOptions[EnumGamePrefs.BloodMoonRange].Enabled = GamePrefs.GetInt(_gamePref) != 0;
	}

	// Token: 0x06006ED6 RID: 28374 RVA: 0x002B1924 File Offset: 0x002AFB24
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

	// Token: 0x06006ED7 RID: 28375 RVA: 0x002B1988 File Offset: 0x002AFB88
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

	// Token: 0x06006ED8 RID: 28376 RVA: 0x002B19D4 File Offset: 0x002AFBD4
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

	// Token: 0x06006ED9 RID: 28377 RVA: 0x002B1A34 File Offset: 0x002AFC34
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

	// Token: 0x06006EDA RID: 28378 RVA: 0x002B1A80 File Offset: 0x002AFC80
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

	// Token: 0x06006EDB RID: 28379 RVA: 0x002B1C98 File Offset: 0x002AFE98
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

	// Token: 0x06006EDC RID: 28380 RVA: 0x002B1D04 File Offset: 0x002AFF04
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

	// Token: 0x06006EDD RID: 28381 RVA: 0x002B1D71 File Offset: 0x002AFF71
	public void UpdateServerEnabledState()
	{
		this.serverEnabledSelector.SetCurrentValue();
	}

	// Token: 0x06006EDE RID: 28382 RVA: 0x002B1D7E File Offset: 0x002AFF7E
	public void UpdateCrossplayEnabledState()
	{
		this.gameOptions[EnumGamePrefs.ServerAllowCrossplay].SetCurrentValue();
	}

	// Token: 0x06006EDF RID: 28383 RVA: 0x002B1D92 File Offset: 0x002AFF92
	public void ApplyCreativeModeChangeAllowed(bool _allowed)
	{
		this.gameOptions[EnumGamePrefs.BuildCreate].Enabled = _allowed;
	}

	// Token: 0x06006EE0 RID: 28384 RVA: 0x002B1DA8 File Offset: 0x002AFFA8
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

	// Token: 0x06006EE1 RID: 28385 RVA: 0x002B1E20 File Offset: 0x002B0020
	public void UpdateOptionVisibilityForGameMode(GameMode _gameMode)
	{
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			keyValuePair.Value.SetCurrentGameMode(_gameMode);
		}
	}

	// Token: 0x06006EE2 RID: 28386 RVA: 0x002B1E7C File Offset: 0x002B007C
	public void UpdateOptionValuesFromGamePrefs()
	{
		this.UpdateSandboxPresetGroups();
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			keyValuePair.Value.SetCurrentValue();
		}
	}

	// Token: 0x06006EE3 RID: 28387 RVA: 0x002B1EDC File Offset: 0x002B00DC
	public void SetGamePrefsToDefaults()
	{
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			GamePrefs.SetObject(keyValuePair.Key, GamePrefs.GetDefault(keyValuePair.Key));
		}
	}

	// Token: 0x04005454 RID: 21588
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector tabSelector;

	// Token: 0x04005455 RID: 21589
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EnumGamePrefs, XUiC_GamePrefSelector> gameOptions = new EnumDictionary<EnumGamePrefs, XUiC_GamePrefSelector>();

	// Token: 0x04005456 RID: 21590
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamePrefSelector serverEnabledSelector;

	// Token: 0x04005459 RID: 21593
	[PublicizedFrom(EAccessModifier.Private)]
	public bool portValid;

	// Token: 0x0400545A RID: 21594
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_GamePrefSelector> tabPrefsTmp = new List<XUiC_GamePrefSelector>();

	// Token: 0x0400545B RID: 21595
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxPresetSelector sandboxPresetSelector;

	// Token: 0x0400545C RID: 21596
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxSettingsDisplay sandboxDisplay;

	// Token: 0x0400545D RID: 21597
	[XuiBindComponent("btnSandboxOptions", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSandboxOptions;

	// Token: 0x0400545E RID: 21598
	[XuiBindComponent("sandboxPresetValues.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView sandboxPresetValues;

	// Token: 0x0400545F RID: 21599
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005460 RID: 21600
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;

	// Token: 0x04005461 RID: 21601
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplateDefaults;
}
