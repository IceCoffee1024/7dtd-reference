using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DAD RID: 3501
[Preserve]
public class XUiC_OptionsAudio : XUiC_OptionsDialogBase
{
	// Token: 0x06006A26 RID: 27174 RVA: 0x0029D59C File Offset: 0x0029B79C
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

	// Token: 0x06006A27 RID: 27175 RVA: 0x0029D5D8 File Offset: 0x0029B7D8
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

	// Token: 0x06006A28 RID: 27176 RVA: 0x0029D688 File Offset: 0x0029B888
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

	// Token: 0x14000093 RID: 147
	// (add) Token: 0x06006A29 RID: 27177 RVA: 0x0029D7CC File Offset: 0x0029B9CC
	// (remove) Token: 0x06006A2A RID: 27178 RVA: 0x0029D800 File Offset: 0x0029BA00
	public static event Action OnSettingsChanged;

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x06006A2B RID: 27179 RVA: 0x0029D833 File Offset: 0x0029BA33
	[XuiXmlBinding("voiceavailable")]
	public bool VoiceAvailable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.PartyVoice != null && PlatformManager.MultiPlatform.PartyVoice.Status == EPartyVoiceStatus.Ok;
		}
	}

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x06006A2C RID: 27180 RVA: 0x0027584B File Offset: 0x00273A4B
	[XuiXmlBinding("commsallowed")]
	public bool IsCommunicationAllowed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.IsCommunicationAllowed();
		}
	}

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x06006A2D RID: 27181 RVA: 0x0029D855 File Offset: 0x0029BA55
	[XuiXmlBinding("multiplayerallowed")]
	public bool MultiplayerAllowed
	{
		get
		{
			return PermissionsManager.IsMultiplayerAllowed();
		}
	}

	// Token: 0x06006A2E RID: 27182 RVA: 0x0029D85C File Offset: 0x0029BA5C
	public override void Init()
	{
		base.Init();
		XUiC_OptionsAudio.ID = base.WindowGroup.Id;
		this.initEosVoiceDeviceOptions();
		this.initDiscordVoiceDeviceOptions();
	}

	// Token: 0x06006A2F RID: 27183 RVA: 0x0029D880 File Offset: 0x0029BA80
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

	// Token: 0x06006A30 RID: 27184 RVA: 0x0029D89D File Offset: 0x0029BA9D
	public override void OnOpen()
	{
		this.onOpenDiscord();
		base.OnOpen();
	}

	// Token: 0x06006A31 RID: 27185 RVA: 0x0029D8AB File Offset: 0x0029BAAB
	public override void OnClose()
	{
		base.OnClose();
		this.onCloseDiscord();
	}

	// Token: 0x06006A32 RID: 27186 RVA: 0x0029D8B9 File Offset: 0x0029BAB9
	public void SetActiveTab(string _tabKey)
	{
		XUiC_TabSelector tabSelector = this.TabSelector;
		if (tabSelector == null)
		{
			return;
		}
		tabSelector.SelectTabByName(_tabKey, false);
	}

	// Token: 0x06006A33 RID: 27187 RVA: 0x0029D8D0 File Offset: 0x0029BAD0
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

	// Token: 0x06006A34 RID: 27188 RVA: 0x0029DA12 File Offset: 0x0029BC12
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceDeviceListChanged(DiscordManager.AudioDeviceConfig _inOutConfig)
	{
		XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> xuiC_ComboBoxList = (_inOutConfig.IsOutput ? this.comboDiscordOutputDevice : this.comboDiscordInputDevice);
		XUiC_OptionsAudio.updateVoiceDeviceList(xuiC_ComboBoxList, _inOutConfig.CurrentAudioDevices.Values);
		XUiC_OptionsAudio.updateVoiceDeviceSelection(xuiC_ComboBoxList, _inOutConfig.ActiveAudioDevice);
	}

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x06006A35 RID: 27189 RVA: 0x0029DA46 File Offset: 0x0029BC46
	[XuiXmlBinding("discord_ptt")]
	public bool DiscordVoiceButtonMode
	{
		get
		{
			XUiC_OptionEntryGamePrefBool xuiC_OptionEntryGamePrefBool = this.optionDiscordVoiceButtonMode;
			return xuiC_OptionEntryGamePrefBool != null && xuiC_OptionEntryGamePrefBool.CurrentValue;
		}
	}

	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x06006A36 RID: 27190 RVA: 0x0029DA59 File Offset: 0x0029BC59
	[XuiXmlBinding("discord_enabled")]
	public bool DiscordEnabled
	{
		get
		{
			return !DiscordManager.Instance.Settings.DiscordDisabled;
		}
	}

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x06006A37 RID: 27191 RVA: 0x0029DA6D File Offset: 0x0029BC6D
	[XuiXmlBinding("discordinitialized")]
	public bool DiscordInitialized
	{
		get
		{
			return DiscordManager.Instance.IsInitialized;
		}
	}

	// Token: 0x17000B90 RID: 2960
	// (get) Token: 0x06006A38 RID: 27192 RVA: 0x0029DA79 File Offset: 0x0029BC79
	[XuiXmlBinding("discord_is_ready")]
	public bool DiscordReady
	{
		get
		{
			return DiscordManager.Instance.IsReady;
		}
	}

	// Token: 0x17000B91 RID: 2961
	// (get) Token: 0x06006A39 RID: 27193 RVA: 0x0008A882 File Offset: 0x00088A82
	[XuiXmlBinding("discord_supports_full_accounts")]
	public bool DiscordSupportsFullAccounts
	{
		get
		{
			return DiscordManager.SupportsFullAccounts;
		}
	}

	// Token: 0x17000B92 RID: 2962
	// (get) Token: 0x06006A3A RID: 27194 RVA: 0x0008A889 File Offset: 0x00088A89
	[XuiXmlBinding("discord_supports_provisional_accounts")]
	public bool DiscordSupportsProvisionalAccounts
	{
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x17000B93 RID: 2963
	// (get) Token: 0x06006A3B RID: 27195 RVA: 0x0029DA85 File Offset: 0x0029BC85
	[XuiXmlBinding("discordaccountlinked")]
	public bool DiscordAccountLinked
	{
		get
		{
			DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
			return localUser == null || !localUser.IsProvisionalAccount;
		}
	}

	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x06006A3C RID: 27196 RVA: 0x0029DAA0 File Offset: 0x0029BCA0
	[XuiXmlBinding("discord_vad_threshold_manual")]
	public bool DiscordVoiceVadThresholdManual
	{
		get
		{
			return !DiscordManager.Instance.Settings.VoiceVadModeAuto;
		}
	}

	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06006A3D RID: 27197 RVA: 0x0029DAB4 File Offset: 0x0029BCB4
	[XuiXmlBinding("discord_in_call")]
	public bool DiscordInCall
	{
		get
		{
			return DiscordManager.Instance.ActiveVoiceLobby != null;
		}
	}

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06006A3E RID: 27198 RVA: 0x0029DAC3 File Offset: 0x0029BCC3
	[XuiXmlBinding("discord_is_speaking")]
	public bool DiscordIsSpeaking
	{
		get
		{
			DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
			return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.IsSpeaking;
		}
	}

	// Token: 0x06006A3F RID: 27199 RVA: 0x0029DADF File Offset: 0x0029BCDF
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

	// Token: 0x06006A40 RID: 27200 RVA: 0x0029DB06 File Offset: 0x0029BD06
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

	// Token: 0x06006A41 RID: 27201 RVA: 0x0029DB33 File Offset: 0x0029BD33
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

	// Token: 0x06006A42 RID: 27202 RVA: 0x0029DB60 File Offset: 0x0029BD60
	[XuiBindEvent("OnPress", "btnDiscordManageSocialSettings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageSocialSettingsOnPress(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.OpenDiscordSocialSettings();
	}

	// Token: 0x06006A43 RID: 27203 RVA: 0x0029DB6C File Offset: 0x0029BD6C
	[XuiBindEvent("OnPress", "btnDiscordManageBlockedUsers")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageBlockedUsersOnPress(XUiController _sender, int _mouseButton)
	{
		GUIWindow window = this.xui.playerUI.windowManager.GetWindow(XUiC_DiscordBlockedUsers.ID);
		this.xui.playerUI.windowManager.Open(window, false);
	}

	// Token: 0x06006A44 RID: 27204 RVA: 0x0029DBAB File Offset: 0x0029BDAB
	public void OpenAtTab(string _tabName)
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
		this.SetActiveTab(_tabName);
	}

	// Token: 0x06006A45 RID: 27205 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		base.RefreshBindings();
	}

	// Token: 0x06006A46 RID: 27206 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordLocalUserChanged(bool _loggedIn)
	{
		base.RefreshBindings();
	}

	// Token: 0x06006A47 RID: 27207 RVA: 0x0029DBD0 File Offset: 0x0029BDD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceStateChanged(bool _self, ulong _userId)
	{
		if (!_self)
		{
			return;
		}
		base.RefreshBindings();
	}

	// Token: 0x06006A48 RID: 27208 RVA: 0x0029DBDC File Offset: 0x0029BDDC
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

	// Token: 0x06006A49 RID: 27209 RVA: 0x0029DC40 File Offset: 0x0029BE40
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged += this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged += this.discordLocalUserChanged;
		instance.StatusChanged += this.discordStatusChanged;
		instance.VoiceStateChanged += this.discordVoiceStateChanged;
	}

	// Token: 0x06006A4A RID: 27210 RVA: 0x0029DC9C File Offset: 0x0029BE9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void onCloseDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged -= this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged -= this.discordLocalUserChanged;
		instance.StatusChanged -= this.discordStatusChanged;
		instance.VoiceStateChanged -= this.discordVoiceStateChanged;
	}

	// Token: 0x06006A59 RID: 27225 RVA: 0x0029DE88 File Offset: 0x0029C088
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

	// Token: 0x04005169 RID: 20841
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice noDeviceEntry = new IPartyVoice.VoiceAudioDeviceNotFound();

	// Token: 0x0400516A RID: 20842
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice defaultDeviceEntry = new IPartyVoice.VoiceAudioDeviceDefault();

	// Token: 0x0400516B RID: 20843
	[XuiBindComponent("OptionVoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceOutputDevice;

	// Token: 0x0400516C RID: 20844
	[XuiBindComponent("VoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceOutputDevice;

	// Token: 0x0400516D RID: 20845
	[XuiBindComponent("OptionVoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceInputDevice;

	// Token: 0x0400516E RID: 20846
	[XuiBindComponent("VoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceInputDevice;

	// Token: 0x0400516F RID: 20847
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDeviceListUpdateFrame;

	// Token: 0x04005170 RID: 20848
	public static string ID = "";

	// Token: 0x04005172 RID: 20850
	public const string DiscordTabName = "xuiOptionsAudioDiscord";

	// Token: 0x04005173 RID: 20851
	[XuiBindComponent("OptionDiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordOutputDevice;

	// Token: 0x04005174 RID: 20852
	[XuiBindComponent("DiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordOutputDevice;

	// Token: 0x04005175 RID: 20853
	[XuiBindComponent("OptionDiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordInputDevice;

	// Token: 0x04005176 RID: 20854
	[XuiBindComponent("DiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordInputDevice;

	// Token: 0x04005177 RID: 20855
	[XuiBindComponent("OptionDiscordVoiceButtonMode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionDiscordVoiceButtonMode;

	// Token: 0x04005178 RID: 20856
	[XuiBindComponent("btnDiscordInitialize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordInitialize;

	// Token: 0x04005179 RID: 20857
	[XuiBindComponent("btnDiscordLinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordLinkAccount;

	// Token: 0x0400517A RID: 20858
	[XuiBindComponent("btnDiscordUnlinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordUnlinkAccount;

	// Token: 0x0400517B RID: 20859
	[XuiBindComponent("btnDiscordManageSocialSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageSocialSettings;

	// Token: 0x0400517C RID: 20860
	[XuiBindComponent("btnDiscordManageBlockedUsers", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageBlockedUsers;
}
