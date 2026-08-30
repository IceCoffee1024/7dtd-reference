using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D9E RID: 3486
[Preserve]
public class XUiC_OptionsAudio : XUiC_OptionsDialogBase
{
	// Token: 0x06006A2E RID: 27182 RVA: 0x0029A210 File Offset: 0x00298410
	[PublicizedFrom(EAccessModifier.Private)]
	public static void updateVoiceDeviceList(XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> _combo, IEnumerable<IPartyVoice.VoiceAudioDevice> _devices)
	{
		if (_combo == null)
		{
			return;
		}
		_combo.Elements.Clear();
		_combo.Elements.AddRange(_devices);
		if (_combo.Elements.Count == 0)
		{
			_combo.Elements.Add(XUiC_OptionsAudio.noDeviceEntry);
		}
	}

	// Token: 0x06006A2F RID: 27183 RVA: 0x0029A24C File Offset: 0x0029844C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void updateVoiceDeviceSelection(XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> _combo, string _activeDeviceIdentifier)
	{
		if (_combo == null)
		{
			return;
		}
		if (_combo.Elements.Count == 1)
		{
			_combo.SelectedIndex = 0;
			return;
		}
		int num;
		if (string.IsNullOrEmpty(_activeDeviceIdentifier) || (num = _combo.Elements.FindIndex((IPartyVoice.VoiceAudioDevice _device) => _device.Identifier == _activeDeviceIdentifier)) < 0)
		{
			int num2 = _combo.Elements.FindIndex((IPartyVoice.VoiceAudioDevice _device) => _device.IsDefault);
			if (num2 < 0)
			{
				_combo.Elements.Insert(0, XUiC_OptionsAudio.defaultDeviceEntry);
				num2 = 0;
			}
			_combo.SelectedIndex = num2;
			return;
		}
		_combo.SelectedIndex = num;
	}

	// Token: 0x06006A30 RID: 27184 RVA: 0x0029A2FC File Offset: 0x002984FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void initEosVoiceDeviceOptions()
	{
		if (this.optionVoiceOutputDevice == null || this.comboVoiceOutputDevice == null || this.optionVoiceInputDevice == null || this.comboVoiceInputDevice == null)
		{
			return;
		}
		this.optionVoiceOutputDevice.GetSettingValue = delegate
		{
			this.<initEosVoiceDeviceOptions>g__UpdateVoiceDeviceLists|9_12();
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboVoiceOutputDevice, GamePrefs.GetString(EnumGamePrefs.OptionsVoiceOutputDevice));
		};
		this.optionVoiceOutputDevice.DiscardChanges = delegate
		{
			this.<initEosVoiceDeviceOptions>g__UpdateVoiceDeviceLists|9_12();
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboVoiceOutputDevice, GamePrefs.GetString(EnumGamePrefs.OptionsVoiceOutputDevice));
		};
		this.optionVoiceOutputDevice.ApplyChanges = delegate(bool _)
		{
			if (this.VoiceAvailable)
			{
				GamePrefs.Set(EnumGamePrefs.OptionsVoiceOutputDevice, this.comboVoiceOutputDevice.Value.Identifier);
			}
		};
		this.optionVoiceOutputDevice.ResetDefaults = delegate
		{
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboVoiceOutputDevice, (string)GamePrefs.GetDefault(EnumGamePrefs.OptionsVoiceOutputDevice));
		};
		this.optionVoiceOutputDevice.IsChangedDelegate = delegate
		{
			string @string = GamePrefs.GetString(EnumGamePrefs.OptionsVoiceOutputDevice);
			IPartyVoice.VoiceAudioDevice value = this.comboVoiceOutputDevice.Value;
			return !@string.Equals((value != null) ? value.Identifier : null);
		};
		this.optionVoiceOutputDevice.IsDefaultDelegate = delegate
		{
			string text = (string)GamePrefs.GetDefault(EnumGamePrefs.OptionsVoiceOutputDevice);
			IPartyVoice.VoiceAudioDevice value2 = this.comboVoiceOutputDevice.Value;
			return text.Equals((value2 != null) ? value2.Identifier : null);
		};
		this.optionVoiceInputDevice.GetSettingValue = delegate
		{
			this.<initEosVoiceDeviceOptions>g__UpdateVoiceDeviceLists|9_12();
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboVoiceInputDevice, GamePrefs.GetString(EnumGamePrefs.OptionsVoiceInputDevice));
		};
		this.optionVoiceInputDevice.DiscardChanges = delegate
		{
			this.<initEosVoiceDeviceOptions>g__UpdateVoiceDeviceLists|9_12();
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboVoiceInputDevice, GamePrefs.GetString(EnumGamePrefs.OptionsVoiceInputDevice));
		};
		this.optionVoiceInputDevice.ApplyChanges = delegate(bool _)
		{
			if (this.VoiceAvailable)
			{
				GamePrefs.Set(EnumGamePrefs.OptionsVoiceInputDevice, this.comboVoiceInputDevice.Value.Identifier);
			}
		};
		this.optionVoiceInputDevice.ResetDefaults = delegate
		{
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboVoiceInputDevice, (string)GamePrefs.GetDefault(EnumGamePrefs.OptionsVoiceInputDevice));
		};
		this.optionVoiceInputDevice.IsChangedDelegate = delegate
		{
			string string2 = GamePrefs.GetString(EnumGamePrefs.OptionsVoiceInputDevice);
			IPartyVoice.VoiceAudioDevice value3 = this.comboVoiceInputDevice.Value;
			return !string2.Equals((value3 != null) ? value3.Identifier : null);
		};
		this.optionVoiceInputDevice.IsDefaultDelegate = delegate
		{
			string text2 = (string)GamePrefs.GetDefault(EnumGamePrefs.OptionsVoiceInputDevice);
			IPartyVoice.VoiceAudioDevice value4 = this.comboVoiceInputDevice.Value;
			return text2.Equals((value4 != null) ? value4.Identifier : null);
		};
	}

	// Token: 0x14000094 RID: 148
	// (add) Token: 0x06006A31 RID: 27185 RVA: 0x0029A440 File Offset: 0x00298640
	// (remove) Token: 0x06006A32 RID: 27186 RVA: 0x0029A474 File Offset: 0x00298674
	public static event Action OnSettingsChanged;

	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x06006A33 RID: 27187 RVA: 0x0029A4A7 File Offset: 0x002986A7
	[XuiXmlBinding("voiceavailable")]
	public bool VoiceAvailable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.PartyVoice != null && PlatformManager.MultiPlatform.PartyVoice.Status == EPartyVoiceStatus.Ok;
		}
	}

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x06006A34 RID: 27188 RVA: 0x00272447 File Offset: 0x00270647
	[XuiXmlBinding("commsallowed")]
	public bool IsCommunicationAllowed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.IsCommunicationAllowed();
		}
	}

	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x06006A35 RID: 27189 RVA: 0x0029A4C9 File Offset: 0x002986C9
	[XuiXmlBinding("multiplayerallowed")]
	public bool MultiplayerAllowed
	{
		get
		{
			return PermissionsManager.IsMultiplayerAllowed();
		}
	}

	// Token: 0x06006A36 RID: 27190 RVA: 0x0029A4D0 File Offset: 0x002986D0
	public override void Init()
	{
		base.Init();
		XUiC_OptionsAudio.ID = base.WindowGroup.Id;
		this.initEosVoiceDeviceOptions();
		this.initDiscordVoiceDeviceOptions();
	}

	// Token: 0x06006A37 RID: 27191 RVA: 0x0029A4F4 File Offset: 0x002986F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void afterChangesSaved()
	{
		base.afterChangesSaved();
		this.applyDiscordDisabledChanged();
		Action onSettingsChanged = XUiC_OptionsAudio.OnSettingsChanged;
		if (onSettingsChanged == null)
		{
			return;
		}
		onSettingsChanged();
	}

	// Token: 0x06006A38 RID: 27192 RVA: 0x0029A511 File Offset: 0x00298711
	public override void OnOpen()
	{
		this.onOpenDiscord();
		base.OnOpen();
	}

	// Token: 0x06006A39 RID: 27193 RVA: 0x0029A51F File Offset: 0x0029871F
	public override void OnClose()
	{
		base.OnClose();
		this.onCloseDiscord();
	}

	// Token: 0x06006A3A RID: 27194 RVA: 0x0029A52D File Offset: 0x0029872D
	public void SetActiveTab(string _tabKey)
	{
		XUiC_TabSelector tabSelector = this.TabSelector;
		if (tabSelector == null)
		{
			return;
		}
		tabSelector.SelectTabByName(_tabKey);
	}

	// Token: 0x06006A3B RID: 27195 RVA: 0x0029A540 File Offset: 0x00298740
	[PublicizedFrom(EAccessModifier.Private)]
	public void initDiscordVoiceDeviceOptions()
	{
		if (this.optionDiscordOutputDevice == null || this.comboDiscordOutputDevice == null || this.optionDiscordInputDevice == null || this.comboDiscordInputDevice == null)
		{
			return;
		}
		this.optionDiscordOutputDevice.GetSettingValue = delegate
		{
			if (!this.IsCommunicationAllowed)
			{
				return;
			}
			XUiC_OptionsAudio.updateVoiceDeviceList(this.comboDiscordOutputDevice, DiscordManager.Instance.AudioOutput.CurrentAudioDevices.Values);
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboDiscordOutputDevice, DiscordManager.Instance.AudioOutput.ActiveAudioDevice);
		};
		this.optionDiscordOutputDevice.DiscardChanges = delegate
		{
			XUiC_OptionsAudio.updateVoiceDeviceList(this.comboDiscordOutputDevice, DiscordManager.Instance.AudioOutput.CurrentAudioDevices.Values);
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboDiscordOutputDevice, DiscordManager.Instance.AudioOutput.ActiveAudioDevice);
		};
		this.optionDiscordOutputDevice.ApplyChanges = delegate(bool _)
		{
			if (this.VoiceAvailable)
			{
				DiscordManager.Instance.Settings.SelectedOutputDevice = this.comboDiscordOutputDevice.Value.Identifier;
			}
		};
		this.optionDiscordOutputDevice.ResetDefaults = delegate
		{
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboDiscordOutputDevice, (string)GamePrefs.GetDefault(EnumGamePrefs.DiscordSelectedOutputDevice));
		};
		this.optionDiscordOutputDevice.IsChangedDelegate = delegate
		{
			string @string = GamePrefs.GetString(EnumGamePrefs.DiscordSelectedOutputDevice);
			IPartyVoice.VoiceAudioDevice value = this.comboDiscordOutputDevice.Value;
			return !@string.Equals((value != null) ? value.Identifier : null);
		};
		this.optionDiscordOutputDevice.IsDefaultDelegate = delegate
		{
			string text = (string)GamePrefs.GetDefault(EnumGamePrefs.DiscordSelectedOutputDevice);
			IPartyVoice.VoiceAudioDevice value2 = this.comboDiscordOutputDevice.Value;
			return text.Equals((value2 != null) ? value2.Identifier : null);
		};
		this.optionDiscordInputDevice.GetSettingValue = delegate
		{
			if (!this.IsCommunicationAllowed)
			{
				return;
			}
			XUiC_OptionsAudio.updateVoiceDeviceList(this.comboDiscordInputDevice, DiscordManager.Instance.AudioInput.CurrentAudioDevices.Values);
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboDiscordInputDevice, DiscordManager.Instance.AudioInput.ActiveAudioDevice);
		};
		this.optionDiscordInputDevice.DiscardChanges = delegate
		{
			XUiC_OptionsAudio.updateVoiceDeviceList(this.comboDiscordInputDevice, DiscordManager.Instance.AudioInput.CurrentAudioDevices.Values);
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboDiscordInputDevice, DiscordManager.Instance.AudioInput.ActiveAudioDevice);
		};
		this.optionDiscordInputDevice.ApplyChanges = delegate(bool _)
		{
			if (this.VoiceAvailable)
			{
				DiscordManager.Instance.Settings.SelectedInputDevice = this.comboDiscordInputDevice.Value.Identifier;
			}
		};
		this.optionDiscordInputDevice.ResetDefaults = delegate
		{
			XUiC_OptionsAudio.updateVoiceDeviceSelection(this.comboDiscordInputDevice, (string)GamePrefs.GetDefault(EnumGamePrefs.DiscordSelectedInputDevice));
		};
		this.optionDiscordInputDevice.IsChangedDelegate = delegate
		{
			string string2 = GamePrefs.GetString(EnumGamePrefs.DiscordSelectedInputDevice);
			IPartyVoice.VoiceAudioDevice value3 = this.comboDiscordInputDevice.Value;
			return !string2.Equals((value3 != null) ? value3.Identifier : null);
		};
		this.optionDiscordInputDevice.IsDefaultDelegate = delegate
		{
			string text2 = (string)GamePrefs.GetDefault(EnumGamePrefs.DiscordSelectedInputDevice);
			IPartyVoice.VoiceAudioDevice value4 = this.comboDiscordInputDevice.Value;
			return text2.Equals((value4 != null) ? value4.Identifier : null);
		};
	}

	// Token: 0x06006A3C RID: 27196 RVA: 0x0029A682 File Offset: 0x00298882
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceDeviceListChanged(DiscordManager.AudioDeviceConfig _inOutConfig)
	{
		XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> xuiC_ComboBoxList = (_inOutConfig.IsOutput ? this.comboDiscordOutputDevice : this.comboDiscordInputDevice);
		XUiC_OptionsAudio.updateVoiceDeviceList(xuiC_ComboBoxList, _inOutConfig.CurrentAudioDevices.Values);
		XUiC_OptionsAudio.updateVoiceDeviceSelection(xuiC_ComboBoxList, _inOutConfig.ActiveAudioDevice);
	}

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x06006A3D RID: 27197 RVA: 0x0029A6B6 File Offset: 0x002988B6
	[XuiXmlBinding("discord_ptt")]
	public bool DiscordVoiceButtonMode
	{
		get
		{
			XUiC_OptionEntryGamePrefBool xuiC_OptionEntryGamePrefBool = this.optionDiscordVoiceButtonMode;
			return xuiC_OptionEntryGamePrefBool != null && xuiC_OptionEntryGamePrefBool.CurrentValue;
		}
	}

	// Token: 0x17000B73 RID: 2931
	// (get) Token: 0x06006A3E RID: 27198 RVA: 0x0029A6C9 File Offset: 0x002988C9
	[XuiXmlBinding("discord_enabled")]
	public bool DiscordEnabled
	{
		get
		{
			return !DiscordManager.Instance.Settings.DiscordDisabled;
		}
	}

	// Token: 0x17000B74 RID: 2932
	// (get) Token: 0x06006A3F RID: 27199 RVA: 0x0029A6DD File Offset: 0x002988DD
	[XuiXmlBinding("discordinitialized")]
	public bool DiscordInitialized
	{
		get
		{
			return DiscordManager.Instance.IsInitialized;
		}
	}

	// Token: 0x17000B75 RID: 2933
	// (get) Token: 0x06006A40 RID: 27200 RVA: 0x0029A6E9 File Offset: 0x002988E9
	[XuiXmlBinding("discord_is_ready")]
	public bool DiscordReady
	{
		get
		{
			return DiscordManager.Instance.IsReady;
		}
	}

	// Token: 0x17000B76 RID: 2934
	// (get) Token: 0x06006A41 RID: 27201 RVA: 0x0008A602 File Offset: 0x00088802
	[XuiXmlBinding("discord_supports_full_accounts")]
	public bool DiscordSupportsFullAccounts
	{
		get
		{
			return DiscordManager.SupportsFullAccounts;
		}
	}

	// Token: 0x17000B77 RID: 2935
	// (get) Token: 0x06006A42 RID: 27202 RVA: 0x0008A609 File Offset: 0x00088809
	[XuiXmlBinding("discord_supports_provisional_accounts")]
	public bool DiscordSupportsProvisionalAccounts
	{
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x17000B78 RID: 2936
	// (get) Token: 0x06006A43 RID: 27203 RVA: 0x0029A6F5 File Offset: 0x002988F5
	[XuiXmlBinding("discordaccountlinked")]
	public bool DiscordAccountLinked
	{
		get
		{
			DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
			return localUser == null || !localUser.IsProvisionalAccount;
		}
	}

	// Token: 0x17000B79 RID: 2937
	// (get) Token: 0x06006A44 RID: 27204 RVA: 0x0029A710 File Offset: 0x00298910
	[XuiXmlBinding("discord_vad_threshold_manual")]
	public bool DiscordVoiceVadThresholdManual
	{
		get
		{
			return !DiscordManager.Instance.Settings.VoiceVadModeAuto;
		}
	}

	// Token: 0x17000B7A RID: 2938
	// (get) Token: 0x06006A45 RID: 27205 RVA: 0x0029A724 File Offset: 0x00298924
	[XuiXmlBinding("discord_in_call")]
	public bool DiscordInCall
	{
		get
		{
			return DiscordManager.Instance.ActiveVoiceLobby != null;
		}
	}

	// Token: 0x17000B7B RID: 2939
	// (get) Token: 0x06006A46 RID: 27206 RVA: 0x0029A733 File Offset: 0x00298933
	[XuiXmlBinding("discord_is_speaking")]
	public bool DiscordIsSpeaking
	{
		get
		{
			DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
			return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.IsSpeaking;
		}
	}

	// Token: 0x06006A47 RID: 27207 RVA: 0x0029A74F File Offset: 0x0029894F
	[XuiBindEvent("OnPress", "btnDiscordInitialize")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordInitializeOnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_DiscordLogin.Open(delegate
		{
			this.OpenAtTab("xuiOptionsAudioDiscord");
		}, false, false, false, true, false);
		DiscordManager.Instance.AuthManager.AutoLogin();
	}

	// Token: 0x06006A48 RID: 27208 RVA: 0x0029A776 File Offset: 0x00298976
	[XuiBindEvent("OnPress", "btnDiscordLinkAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordLinkAccountOnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_DiscordLogin.Open(delegate
		{
			this.OpenAtTab("xuiOptionsAudioDiscord");
		}, false, false, false, true, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06006A49 RID: 27209 RVA: 0x0029A7A3 File Offset: 0x002989A3
	[XuiBindEvent("OnPress", "btnDiscordUnlinkAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordUnlinkAccountOnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_DiscordLogin.Open(delegate
		{
			this.OpenAtTab("xuiOptionsAudioDiscord");
		}, false, false, false, true, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
		DiscordManager.Instance.AuthManager.UnmergeAccount();
	}

	// Token: 0x06006A4A RID: 27210 RVA: 0x0029A7D0 File Offset: 0x002989D0
	[XuiBindEvent("OnPress", "btnDiscordManageSocialSettings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageSocialSettingsOnPress(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.OpenDiscordSocialSettings();
	}

	// Token: 0x06006A4B RID: 27211 RVA: 0x0029A7DC File Offset: 0x002989DC
	[XuiBindEvent("OnPress", "btnDiscordManageBlockedUsers")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageBlockedUsersOnPress(XUiController _sender, int _mouseButton)
	{
		GUIWindow window = this.xui.playerUI.windowManager.GetWindow(XUiC_DiscordBlockedUsers.ID);
		this.xui.playerUI.windowManager.Open(window, false);
	}

	// Token: 0x06006A4C RID: 27212 RVA: 0x0029A81B File Offset: 0x00298A1B
	public void OpenAtTab(string _tabName)
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
		this.SetActiveTab(_tabName);
	}

	// Token: 0x06006A4D RID: 27213 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		base.RefreshBindings();
	}

	// Token: 0x06006A4E RID: 27214 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordLocalUserChanged(bool _loggedIn)
	{
		base.RefreshBindings();
	}

	// Token: 0x06006A4F RID: 27215 RVA: 0x0029A840 File Offset: 0x00298A40
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceStateChanged(bool _self, ulong _userId)
	{
		if (!_self)
		{
			return;
		}
		base.RefreshBindings();
	}

	// Token: 0x06006A50 RID: 27216 RVA: 0x0029A84C File Offset: 0x00298A4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyDiscordDisabledChanged()
	{
		bool isInitialized = DiscordManager.Instance.IsInitialized;
		bool flag = !DiscordManager.Instance.Settings.DiscordDisabled;
		if (isInitialized == flag)
		{
			return;
		}
		if (!flag)
		{
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiOptionsAudioDiscordDisableRestartTitle", false, null), Localization.Get("xuiOptionsAudioDiscordDisableRestartText", false, null), "", null, false, false, false);
		}
		base.RefreshBindings();
	}

	// Token: 0x06006A51 RID: 27217 RVA: 0x0029A8B0 File Offset: 0x00298AB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged += this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged += this.discordLocalUserChanged;
		instance.StatusChanged += this.discordStatusChanged;
		instance.VoiceStateChanged += this.discordVoiceStateChanged;
	}

	// Token: 0x06006A52 RID: 27218 RVA: 0x0029A90C File Offset: 0x00298B0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void onCloseDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged -= this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged -= this.discordLocalUserChanged;
		instance.StatusChanged -= this.discordStatusChanged;
		instance.VoiceStateChanged -= this.discordVoiceStateChanged;
	}

	// Token: 0x06006A61 RID: 27233 RVA: 0x0029AAF8 File Offset: 0x00298CF8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <initEosVoiceDeviceOptions>g__UpdateVoiceDeviceLists|9_12()
	{
		int frameCount = Time.frameCount;
		if (this.lastDeviceListUpdateFrame == frameCount)
		{
			return;
		}
		this.lastDeviceListUpdateFrame = frameCount;
		if (!this.VoiceAvailable || !this.IsCommunicationAllowed)
		{
			return;
		}
		ValueTuple<IList<IPartyVoice.VoiceAudioDevice>, IList<IPartyVoice.VoiceAudioDevice>> devicesList = PlatformManager.MultiPlatform.PartyVoice.GetDevicesList();
		IList<IPartyVoice.VoiceAudioDevice> item = devicesList.Item1;
		IList<IPartyVoice.VoiceAudioDevice> item2 = devicesList.Item2;
		XUiC_OptionsAudio.updateVoiceDeviceList(this.comboVoiceOutputDevice, item2);
		XUiC_OptionsAudio.updateVoiceDeviceList(this.comboVoiceInputDevice, item);
	}

	// Token: 0x04005132 RID: 20786
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice noDeviceEntry = new IPartyVoice.VoiceAudioDeviceNotFound();

	// Token: 0x04005133 RID: 20787
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice defaultDeviceEntry = new IPartyVoice.VoiceAudioDeviceDefault();

	// Token: 0x04005134 RID: 20788
	[XuiBindComponent("OptionVoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceOutputDevice;

	// Token: 0x04005135 RID: 20789
	[XuiBindComponent("VoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceOutputDevice;

	// Token: 0x04005136 RID: 20790
	[XuiBindComponent("OptionVoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceInputDevice;

	// Token: 0x04005137 RID: 20791
	[XuiBindComponent("VoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceInputDevice;

	// Token: 0x04005138 RID: 20792
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDeviceListUpdateFrame;

	// Token: 0x04005139 RID: 20793
	public static string ID = "";

	// Token: 0x0400513B RID: 20795
	public const string DiscordTabName = "xuiOptionsAudioDiscord";

	// Token: 0x0400513C RID: 20796
	[XuiBindComponent("OptionDiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordOutputDevice;

	// Token: 0x0400513D RID: 20797
	[XuiBindComponent("DiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordOutputDevice;

	// Token: 0x0400513E RID: 20798
	[XuiBindComponent("OptionDiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordInputDevice;

	// Token: 0x0400513F RID: 20799
	[XuiBindComponent("DiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordInputDevice;

	// Token: 0x04005140 RID: 20800
	[XuiBindComponent("OptionDiscordVoiceButtonMode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionDiscordVoiceButtonMode;

	// Token: 0x04005141 RID: 20801
	[XuiBindComponent("btnDiscordInitialize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordInitialize;

	// Token: 0x04005142 RID: 20802
	[XuiBindComponent("btnDiscordLinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordLinkAccount;

	// Token: 0x04005143 RID: 20803
	[XuiBindComponent("btnDiscordUnlinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordUnlinkAccount;

	// Token: 0x04005144 RID: 20804
	[XuiBindComponent("btnDiscordManageSocialSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageSocialSettings;

	// Token: 0x04005145 RID: 20805
	[XuiBindComponent("btnDiscordManageBlockedUsers", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageBlockedUsers;
}
