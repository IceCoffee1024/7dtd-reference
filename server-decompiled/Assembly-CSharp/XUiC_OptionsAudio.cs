using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DB8 RID: 3512
[Preserve]
public class XUiC_OptionsAudio : XUiC_OptionsDialogBase
{
	// Token: 0x06006A4B RID: 27211 RVA: 0x0029EAC4 File Offset: 0x0029CCC4
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

	// Token: 0x06006A4C RID: 27212 RVA: 0x0029EB00 File Offset: 0x0029CD00
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

	// Token: 0x06006A4D RID: 27213 RVA: 0x0029EBB0 File Offset: 0x0029CDB0
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
	// (add) Token: 0x06006A4E RID: 27214 RVA: 0x0029ECF4 File Offset: 0x0029CEF4
	// (remove) Token: 0x06006A4F RID: 27215 RVA: 0x0029ED28 File Offset: 0x0029CF28
	public static event Action OnSettingsChanged;

	// Token: 0x17000B90 RID: 2960
	// (get) Token: 0x06006A50 RID: 27216 RVA: 0x0029ED5B File Offset: 0x0029CF5B
	[XuiXmlBinding("voiceavailable")]
	public bool VoiceAvailable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.PartyVoice != null && PlatformManager.MultiPlatform.PartyVoice.Status == EPartyVoiceStatus.Ok;
		}
	}

	// Token: 0x17000B91 RID: 2961
	// (get) Token: 0x06006A51 RID: 27217 RVA: 0x00276D73 File Offset: 0x00274F73
	[XuiXmlBinding("commsallowed")]
	public bool IsCommunicationAllowed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.IsCommunicationAllowed();
		}
	}

	// Token: 0x17000B92 RID: 2962
	// (get) Token: 0x06006A52 RID: 27218 RVA: 0x0029ED7D File Offset: 0x0029CF7D
	[XuiXmlBinding("multiplayerallowed")]
	public bool MultiplayerAllowed
	{
		get
		{
			return PermissionsManager.IsMultiplayerAllowed();
		}
	}

	// Token: 0x06006A53 RID: 27219 RVA: 0x0029ED84 File Offset: 0x0029CF84
	public override void Init()
	{
		base.Init();
		XUiC_OptionsAudio.ID = base.WindowGroup.Id;
		this.initEosVoiceDeviceOptions();
		this.initDiscordVoiceDeviceOptions();
	}

	// Token: 0x06006A54 RID: 27220 RVA: 0x0029EDA8 File Offset: 0x0029CFA8
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

	// Token: 0x06006A55 RID: 27221 RVA: 0x0029EDC5 File Offset: 0x0029CFC5
	public override void OnOpen()
	{
		this.onOpenDiscord();
		base.OnOpen();
	}

	// Token: 0x06006A56 RID: 27222 RVA: 0x0029EDD3 File Offset: 0x0029CFD3
	public override void OnClose()
	{
		base.OnClose();
		this.onCloseDiscord();
	}

	// Token: 0x06006A57 RID: 27223 RVA: 0x0029EDE1 File Offset: 0x0029CFE1
	public void SetActiveTab(string _tabKey)
	{
		XUiC_TabSelector tabSelector = this.TabSelector;
		if (tabSelector == null)
		{
			return;
		}
		tabSelector.SelectTabByName(_tabKey, false);
	}

	// Token: 0x06006A58 RID: 27224 RVA: 0x0029EDF8 File Offset: 0x0029CFF8
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

	// Token: 0x06006A59 RID: 27225 RVA: 0x0029EF3A File Offset: 0x0029D13A
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceDeviceListChanged(DiscordManager.AudioDeviceConfig _inOutConfig)
	{
		XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> xuiC_ComboBoxList = (_inOutConfig.IsOutput ? this.comboDiscordOutputDevice : this.comboDiscordInputDevice);
		XUiC_OptionsAudio.updateVoiceDeviceList(xuiC_ComboBoxList, _inOutConfig.CurrentAudioDevices.Values);
		XUiC_OptionsAudio.updateVoiceDeviceSelection(xuiC_ComboBoxList, _inOutConfig.ActiveAudioDevice);
	}

	// Token: 0x17000B93 RID: 2963
	// (get) Token: 0x06006A5A RID: 27226 RVA: 0x0029EF6E File Offset: 0x0029D16E
	[XuiXmlBinding("discord_ptt")]
	public bool DiscordVoiceButtonMode
	{
		get
		{
			XUiC_OptionEntryGamePrefBool xuiC_OptionEntryGamePrefBool = this.optionDiscordVoiceButtonMode;
			return xuiC_OptionEntryGamePrefBool != null && xuiC_OptionEntryGamePrefBool.CurrentValue;
		}
	}

	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x06006A5B RID: 27227 RVA: 0x0029EF81 File Offset: 0x0029D181
	[XuiXmlBinding("discord_enabled")]
	public bool DiscordEnabled
	{
		get
		{
			return !DiscordManager.Instance.Settings.DiscordDisabled;
		}
	}

	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06006A5C RID: 27228 RVA: 0x0029EF95 File Offset: 0x0029D195
	[XuiXmlBinding("discordinitialized")]
	public bool DiscordInitialized
	{
		get
		{
			return DiscordManager.Instance.IsInitialized;
		}
	}

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06006A5D RID: 27229 RVA: 0x0029EFA1 File Offset: 0x0029D1A1
	[XuiXmlBinding("discord_is_ready")]
	public bool DiscordReady
	{
		get
		{
			return DiscordManager.Instance.IsReady;
		}
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06006A5E RID: 27230 RVA: 0x0008AAB6 File Offset: 0x00088CB6
	[XuiXmlBinding("discord_supports_full_accounts")]
	public bool DiscordSupportsFullAccounts
	{
		get
		{
			return DiscordManager.SupportsFullAccounts;
		}
	}

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06006A5F RID: 27231 RVA: 0x0008AABD File Offset: 0x00088CBD
	[XuiXmlBinding("discord_supports_provisional_accounts")]
	public bool DiscordSupportsProvisionalAccounts
	{
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06006A60 RID: 27232 RVA: 0x0029EFAD File Offset: 0x0029D1AD
	[XuiXmlBinding("discordaccountlinked")]
	public bool DiscordAccountLinked
	{
		get
		{
			DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
			return localUser == null || !localUser.IsProvisionalAccount;
		}
	}

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x06006A61 RID: 27233 RVA: 0x0029EFC8 File Offset: 0x0029D1C8
	[XuiXmlBinding("discord_vad_threshold_manual")]
	public bool DiscordVoiceVadThresholdManual
	{
		get
		{
			return !DiscordManager.Instance.Settings.VoiceVadModeAuto;
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x06006A62 RID: 27234 RVA: 0x0029EFDC File Offset: 0x0029D1DC
	[XuiXmlBinding("discord_in_call")]
	public bool DiscordInCall
	{
		get
		{
			return DiscordManager.Instance.ActiveVoiceLobby != null;
		}
	}

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x06006A63 RID: 27235 RVA: 0x0029EFEB File Offset: 0x0029D1EB
	[XuiXmlBinding("discord_is_speaking")]
	public bool DiscordIsSpeaking
	{
		get
		{
			DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
			return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.IsSpeaking;
		}
	}

	// Token: 0x06006A64 RID: 27236 RVA: 0x0029F007 File Offset: 0x0029D207
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

	// Token: 0x06006A65 RID: 27237 RVA: 0x0029F02E File Offset: 0x0029D22E
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

	// Token: 0x06006A66 RID: 27238 RVA: 0x0029F05B File Offset: 0x0029D25B
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

	// Token: 0x06006A67 RID: 27239 RVA: 0x0029F088 File Offset: 0x0029D288
	[XuiBindEvent("OnPress", "btnDiscordManageSocialSettings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageSocialSettingsOnPress(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.OpenDiscordSocialSettings();
	}

	// Token: 0x06006A68 RID: 27240 RVA: 0x0029F094 File Offset: 0x0029D294
	[XuiBindEvent("OnPress", "btnDiscordManageBlockedUsers")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageBlockedUsersOnPress(XUiController _sender, int _mouseButton)
	{
		GUIWindow window = this.xui.playerUI.windowManager.GetWindow(XUiC_DiscordBlockedUsers.ID);
		this.xui.playerUI.windowManager.Open(window, false);
	}

	// Token: 0x06006A69 RID: 27241 RVA: 0x0029F0D3 File Offset: 0x0029D2D3
	public void OpenAtTab(string _tabName)
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
		this.SetActiveTab(_tabName);
	}

	// Token: 0x06006A6A RID: 27242 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		base.RefreshBindings();
	}

	// Token: 0x06006A6B RID: 27243 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordLocalUserChanged(bool _loggedIn)
	{
		base.RefreshBindings();
	}

	// Token: 0x06006A6C RID: 27244 RVA: 0x0029F0F8 File Offset: 0x0029D2F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceStateChanged(bool _self, ulong _userId)
	{
		if (!_self)
		{
			return;
		}
		base.RefreshBindings();
	}

	// Token: 0x06006A6D RID: 27245 RVA: 0x0029F104 File Offset: 0x0029D304
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

	// Token: 0x06006A6E RID: 27246 RVA: 0x0029F168 File Offset: 0x0029D368
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged += this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged += this.discordLocalUserChanged;
		instance.StatusChanged += this.discordStatusChanged;
		instance.VoiceStateChanged += this.discordVoiceStateChanged;
	}

	// Token: 0x06006A6F RID: 27247 RVA: 0x0029F1C4 File Offset: 0x0029D3C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void onCloseDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged -= this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged -= this.discordLocalUserChanged;
		instance.StatusChanged -= this.discordStatusChanged;
		instance.VoiceStateChanged -= this.discordVoiceStateChanged;
	}

	// Token: 0x06006A7E RID: 27262 RVA: 0x0029F3B0 File Offset: 0x0029D5B0
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

	// Token: 0x0400518E RID: 20878
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice noDeviceEntry = new IPartyVoice.VoiceAudioDeviceNotFound();

	// Token: 0x0400518F RID: 20879
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice defaultDeviceEntry = new IPartyVoice.VoiceAudioDeviceDefault();

	// Token: 0x04005190 RID: 20880
	[XuiBindComponent("OptionVoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceOutputDevice;

	// Token: 0x04005191 RID: 20881
	[XuiBindComponent("VoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceOutputDevice;

	// Token: 0x04005192 RID: 20882
	[XuiBindComponent("OptionVoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceInputDevice;

	// Token: 0x04005193 RID: 20883
	[XuiBindComponent("VoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceInputDevice;

	// Token: 0x04005194 RID: 20884
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDeviceListUpdateFrame;

	// Token: 0x04005195 RID: 20885
	public static string ID = "";

	// Token: 0x04005197 RID: 20887
	public const string DiscordTabName = "xuiOptionsAudioDiscord";

	// Token: 0x04005198 RID: 20888
	[XuiBindComponent("OptionDiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordOutputDevice;

	// Token: 0x04005199 RID: 20889
	[XuiBindComponent("DiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordOutputDevice;

	// Token: 0x0400519A RID: 20890
	[XuiBindComponent("OptionDiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordInputDevice;

	// Token: 0x0400519B RID: 20891
	[XuiBindComponent("DiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordInputDevice;

	// Token: 0x0400519C RID: 20892
	[XuiBindComponent("OptionDiscordVoiceButtonMode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionDiscordVoiceButtonMode;

	// Token: 0x0400519D RID: 20893
	[XuiBindComponent("btnDiscordInitialize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordInitialize;

	// Token: 0x0400519E RID: 20894
	[XuiBindComponent("btnDiscordLinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordLinkAccount;

	// Token: 0x0400519F RID: 20895
	[XuiBindComponent("btnDiscordUnlinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordUnlinkAccount;

	// Token: 0x040051A0 RID: 20896
	[XuiBindComponent("btnDiscordManageSocialSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageSocialSettings;

	// Token: 0x040051A1 RID: 20897
	[XuiBindComponent("btnDiscordManageBlockedUsers", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageBlockedUsers;
}
