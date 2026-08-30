using System;
using System.Collections.Generic;
using Platform;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E06 RID: 3590
[Preserve]
public class XUiC_NewContinueGameSettings : XUiController
{
	// Token: 0x1400009B RID: 155
	// (add) Token: 0x06006E27 RID: 28199 RVA: 0x002AAAD8 File Offset: 0x002A8CD8
	// (remove) Token: 0x06006E28 RID: 28200 RVA: 0x002AAB10 File Offset: 0x002A8D10
	public event Action SettingsChanged;

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06006E29 RID: 28201 RVA: 0x002AAB48 File Offset: 0x002A8D48
	// (remove) Token: 0x06006E2A RID: 28202 RVA: 0x002AAB80 File Offset: 0x002A8D80
	public event Action OpenSandboxSettingsRequested;

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x06006E2B RID: 28203 RVA: 0x002AABB8 File Offset: 0x002A8DB8
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

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x06006E2C RID: 28204 RVA: 0x002AAC24 File Offset: 0x002A8E24
	// (set) Token: 0x06006E2D RID: 28205 RVA: 0x002AAC2C File Offset: 0x002A8E2C
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

	// Token: 0x06006E2E RID: 28206 RVA: 0x002AAC48 File Offset: 0x002A8E48
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

	// Token: 0x06006E2F RID: 28207 RVA: 0x002AAD88 File Offset: 0x002A8F88
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

	// Token: 0x06006E30 RID: 28208 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void Option_OnValueChanged(XUiC_GamePrefSelector _uiElement, EnumGamePrefs _gamePref)
	{
		this.IsDirty = true;
	}

	// Token: 0x17000C41 RID: 3137
	// (get) Token: 0x06006E31 RID: 28209 RVA: 0x002AAE84 File Offset: 0x002A9084
	// (set) Token: 0x06006E32 RID: 28210 RVA: 0x002AAE8C File Offset: 0x002A908C
	[XuiXmlAttribute("sandbox_values_columns", false)]
	public int SandboxValuesColumns
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C42 RID: 3138
	// (get) Token: 0x06006E33 RID: 28211 RVA: 0x002AAE95 File Offset: 0x002A9095
	// (set) Token: 0x06006E34 RID: 28212 RVA: 0x002AAE9D File Offset: 0x002A909D
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

	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x06006E35 RID: 28213 RVA: 0x002AAED3 File Offset: 0x002A90D3
	// (set) Token: 0x06006E36 RID: 28214 RVA: 0x002AAEDB File Offset: 0x002A90DB
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

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06006E37 RID: 28215 RVA: 0x002AAF11 File Offset: 0x002A9111
	[XuiXmlBinding("sandboxvalues")]
	public Dictionary<string, string> SandboxValues
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	} = new Dictionary<string, string>();

	// Token: 0x06006E38 RID: 28216 RVA: 0x002AAF19 File Offset: 0x002A9119
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

	// Token: 0x06006E39 RID: 28217 RVA: 0x002AAF2B File Offset: 0x002A912B
	public void ResetSandboxPresetToDefault()
	{
		this.sandboxPresetSelector.SelectPresetByName("", null, false);
	}

	// Token: 0x06006E3A RID: 28218 RVA: 0x002AAF40 File Offset: 0x002A9140
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

	// Token: 0x06006E3B RID: 28219 RVA: 0x002AAFB6 File Offset: 0x002A91B6
	[XuiBindEvent("SandboxPresetSelectionChanged", "sandboxPresetSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void SandboxPresetSelector_OnValueChanged(SandboxPresetInfo _oldPreset, SandboxPresetInfo _newPreset)
	{
		this.updateSandboxData();
	}

	// Token: 0x06006E3C RID: 28220 RVA: 0x002AAFC0 File Offset: 0x002A91C0
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

	// Token: 0x06006E3D RID: 28221 RVA: 0x002AB04A File Offset: 0x002A924A
	[PublicizedFrom(EAccessModifier.Private)]
	public void CmbBloodMoonFrequency_OnChangeHandler(XUiC_GamePrefSelector _prefSelector, EnumGamePrefs _gamePref)
	{
		this.gameOptions[EnumGamePrefs.BloodMoonRange].Enabled = GamePrefs.GetInt(_gamePref) != 0;
	}

	// Token: 0x06006E3E RID: 28222 RVA: 0x002AB068 File Offset: 0x002A9268
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

	// Token: 0x06006E3F RID: 28223 RVA: 0x002AB0CC File Offset: 0x002A92CC
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

	// Token: 0x06006E40 RID: 28224 RVA: 0x002AB118 File Offset: 0x002A9318
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

	// Token: 0x06006E41 RID: 28225 RVA: 0x002AB178 File Offset: 0x002A9378
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

	// Token: 0x06006E42 RID: 28226 RVA: 0x002AB1C4 File Offset: 0x002A93C4
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

	// Token: 0x06006E43 RID: 28227 RVA: 0x002AB3DC File Offset: 0x002A95DC
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

	// Token: 0x06006E44 RID: 28228 RVA: 0x002AB448 File Offset: 0x002A9648
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

	// Token: 0x06006E45 RID: 28229 RVA: 0x002AB4B5 File Offset: 0x002A96B5
	public void UpdateServerEnabledState()
	{
		this.serverEnabledSelector.SetCurrentValue();
	}

	// Token: 0x06006E46 RID: 28230 RVA: 0x002AB4C2 File Offset: 0x002A96C2
	public void UpdateCrossplayEnabledState()
	{
		this.gameOptions[EnumGamePrefs.ServerAllowCrossplay].SetCurrentValue();
	}

	// Token: 0x06006E47 RID: 28231 RVA: 0x002AB4D6 File Offset: 0x002A96D6
	public void ApplyCreativeModeChangeAllowed(bool _allowed)
	{
		this.gameOptions[EnumGamePrefs.BuildCreate].Enabled = _allowed;
	}

	// Token: 0x06006E48 RID: 28232 RVA: 0x002AB4EC File Offset: 0x002A96EC
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

	// Token: 0x06006E49 RID: 28233 RVA: 0x002AB564 File Offset: 0x002A9764
	public void UpdateOptionVisibilityForGameMode(GameMode _gameMode)
	{
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			keyValuePair.Value.SetCurrentGameMode(_gameMode);
		}
	}

	// Token: 0x06006E4A RID: 28234 RVA: 0x002AB5C0 File Offset: 0x002A97C0
	public void UpdateOptionValuesFromGamePrefs()
	{
		this.UpdateSandboxPresetGroups();
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			keyValuePair.Value.SetCurrentValue();
		}
	}

	// Token: 0x06006E4B RID: 28235 RVA: 0x002AB620 File Offset: 0x002A9820
	public void SetGamePrefsToDefaults()
	{
		foreach (KeyValuePair<EnumGamePrefs, XUiC_GamePrefSelector> keyValuePair in this.gameOptions)
		{
			GamePrefs.SetObject(keyValuePair.Key, GamePrefs.GetDefault(keyValuePair.Key));
		}
	}

	// Token: 0x04005379 RID: 21369
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector tabSelector;

	// Token: 0x0400537A RID: 21370
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EnumGamePrefs, XUiC_GamePrefSelector> gameOptions = new EnumDictionary<EnumGamePrefs, XUiC_GamePrefSelector>();

	// Token: 0x0400537B RID: 21371
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_GamePrefSelector serverEnabledSelector;

	// Token: 0x0400537E RID: 21374
	[PublicizedFrom(EAccessModifier.Private)]
	public bool portValid;

	// Token: 0x0400537F RID: 21375
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_GamePrefSelector> tabPrefsTmp = new List<XUiC_GamePrefSelector>();

	// Token: 0x04005380 RID: 21376
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxPresetSelector sandboxPresetSelector;

	// Token: 0x04005381 RID: 21377
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxSettingsDisplay sandboxDisplay;

	// Token: 0x04005382 RID: 21378
	[XuiBindComponent("btnSandboxOptions", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSandboxOptions;

	// Token: 0x04005383 RID: 21379
	[XuiBindComponent("sandboxPresetValues.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_ScrollView sandboxPresetValues;

	// Token: 0x04005384 RID: 21380
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005385 RID: 21381
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplate;

	// Token: 0x04005386 RID: 21382
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandBoxValuesTemplateDefaults;
}
