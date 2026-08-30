using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D8A RID: 3466
[Preserve]
public class XUiC_OptionsAudio : XUiC_OptionsDialogBase
{
	// Token: 0x06006992 RID: 27026 RVA: 0x002970BC File Offset: 0x002952BC
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

	// Token: 0x06006993 RID: 27027 RVA: 0x002970F8 File Offset: 0x002952F8
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

	// Token: 0x06006994 RID: 27028 RVA: 0x002971A8 File Offset: 0x002953A8
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
	// (add) Token: 0x06006995 RID: 27029 RVA: 0x002972EC File Offset: 0x002954EC
	// (remove) Token: 0x06006996 RID: 27030 RVA: 0x00297320 File Offset: 0x00295520
	public static event Action OnSettingsChanged;

	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x06006997 RID: 27031 RVA: 0x00297353 File Offset: 0x00295553
	[XuiXmlBinding("voiceavailable")]
	public bool VoiceAvailable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PlatformManager.MultiPlatform.PartyVoice != null && PlatformManager.MultiPlatform.PartyVoice.Status == EPartyVoiceStatus.Ok;
		}
	}

	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x06006998 RID: 27032 RVA: 0x0026F54B File Offset: 0x0026D74B
	[XuiXmlBinding("commsallowed")]
	public bool IsCommunicationAllowed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.IsCommunicationAllowed();
		}
	}

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x06006999 RID: 27033 RVA: 0x00297375 File Offset: 0x00295575
	[XuiXmlBinding("multiplayerallowed")]
	public bool MultiplayerAllowed
	{
		get
		{
			return PermissionsManager.IsMultiplayerAllowed();
		}
	}

	// Token: 0x0600699A RID: 27034 RVA: 0x0029737C File Offset: 0x0029557C
	public override void Init()
	{
		base.Init();
		XUiC_OptionsAudio.ID = base.WindowGroup.Id;
		this.initEosVoiceDeviceOptions();
		this.initDiscordVoiceDeviceOptions();
	}

	// Token: 0x0600699B RID: 27035 RVA: 0x002973A0 File Offset: 0x002955A0
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

	// Token: 0x0600699C RID: 27036 RVA: 0x002973BD File Offset: 0x002955BD
	public override void OnOpen()
	{
		this.onOpenDiscord();
		base.OnOpen();
	}

	// Token: 0x0600699D RID: 27037 RVA: 0x002973CB File Offset: 0x002955CB
	public override void OnClose()
	{
		base.OnClose();
		this.onCloseDiscord();
	}

	// Token: 0x0600699E RID: 27038 RVA: 0x002973D9 File Offset: 0x002955D9
	public void SetActiveTab(string _tabKey)
	{
		XUiC_TabSelector tabSelector = this.TabSelector;
		if (tabSelector == null)
		{
			return;
		}
		tabSelector.SelectTabByName(_tabKey);
	}

	// Token: 0x0600699F RID: 27039 RVA: 0x002973EC File Offset: 0x002955EC
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

	// Token: 0x060069A0 RID: 27040 RVA: 0x0029752E File Offset: 0x0029572E
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceDeviceListChanged(DiscordManager.AudioDeviceConfig _inOutConfig)
	{
		XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> xuiC_ComboBoxList = (_inOutConfig.IsOutput ? this.comboDiscordOutputDevice : this.comboDiscordInputDevice);
		XUiC_OptionsAudio.updateVoiceDeviceList(xuiC_ComboBoxList, _inOutConfig.CurrentAudioDevices.Values);
		XUiC_OptionsAudio.updateVoiceDeviceSelection(xuiC_ComboBoxList, _inOutConfig.ActiveAudioDevice);
	}

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x060069A1 RID: 27041 RVA: 0x00297562 File Offset: 0x00295762
	[XuiXmlBinding("discord_ptt")]
	public bool DiscordVoiceButtonMode
	{
		get
		{
			XUiC_OptionEntryGamePrefBool xuiC_OptionEntryGamePrefBool = this.optionDiscordVoiceButtonMode;
			return xuiC_OptionEntryGamePrefBool != null && xuiC_OptionEntryGamePrefBool.CurrentValue;
		}
	}

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x060069A2 RID: 27042 RVA: 0x00297575 File Offset: 0x00295775
	[XuiXmlBinding("discord_enabled")]
	public bool DiscordEnabled
	{
		get
		{
			return !DiscordManager.Instance.Settings.DiscordDisabled;
		}
	}

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x060069A3 RID: 27043 RVA: 0x00297589 File Offset: 0x00295789
	[XuiXmlBinding("discordinitialized")]
	public bool DiscordInitialized
	{
		get
		{
			return DiscordManager.Instance.IsInitialized;
		}
	}

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x060069A4 RID: 27044 RVA: 0x00297595 File Offset: 0x00295795
	[XuiXmlBinding("discord_is_ready")]
	public bool DiscordReady
	{
		get
		{
			return DiscordManager.Instance.IsReady;
		}
	}

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x060069A5 RID: 27045 RVA: 0x00089F54 File Offset: 0x00088154
	[XuiXmlBinding("discord_supports_full_accounts")]
	public bool DiscordSupportsFullAccounts
	{
		get
		{
			return DiscordManager.SupportsFullAccounts;
		}
	}

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x060069A6 RID: 27046 RVA: 0x00089F5B File Offset: 0x0008815B
	[XuiXmlBinding("discord_supports_provisional_accounts")]
	public bool DiscordSupportsProvisionalAccounts
	{
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x060069A7 RID: 27047 RVA: 0x002975A1 File Offset: 0x002957A1
	[XuiXmlBinding("discordaccountlinked")]
	public bool DiscordAccountLinked
	{
		get
		{
			DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
			return localUser == null || !localUser.IsProvisionalAccount;
		}
	}

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x060069A8 RID: 27048 RVA: 0x002975BC File Offset: 0x002957BC
	[XuiXmlBinding("discord_vad_threshold_manual")]
	public bool DiscordVoiceVadThresholdManual
	{
		get
		{
			return !DiscordManager.Instance.Settings.VoiceVadModeAuto;
		}
	}

	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x060069A9 RID: 27049 RVA: 0x002975D0 File Offset: 0x002957D0
	[XuiXmlBinding("discord_in_call")]
	public bool DiscordInCall
	{
		get
		{
			return DiscordManager.Instance.ActiveVoiceLobby != null;
		}
	}

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x060069AA RID: 27050 RVA: 0x002975DF File Offset: 0x002957DF
	[XuiXmlBinding("discord_is_speaking")]
	public bool DiscordIsSpeaking
	{
		get
		{
			DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
			return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.IsSpeaking;
		}
	}

	// Token: 0x060069AB RID: 27051 RVA: 0x002975FB File Offset: 0x002957FB
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

	// Token: 0x060069AC RID: 27052 RVA: 0x00297622 File Offset: 0x00295822
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

	// Token: 0x060069AD RID: 27053 RVA: 0x0029764F File Offset: 0x0029584F
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

	// Token: 0x060069AE RID: 27054 RVA: 0x0029767C File Offset: 0x0029587C
	[XuiBindEvent("OnPress", "btnDiscordManageSocialSettings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageSocialSettingsOnPress(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.OpenDiscordSocialSettings();
	}

	// Token: 0x060069AF RID: 27055 RVA: 0x00297688 File Offset: 0x00295888
	[XuiBindEvent("OnPress", "btnDiscordManageBlockedUsers")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordManageBlockedUsersOnPress(XUiController _sender, int _mouseButton)
	{
		GUIWindow window = this.xui.playerUI.windowManager.GetWindow(XUiC_DiscordBlockedUsers.ID);
		this.xui.playerUI.windowManager.Open(window, false);
	}

	// Token: 0x060069B0 RID: 27056 RVA: 0x002976C7 File Offset: 0x002958C7
	public void OpenAtTab(string _tabName)
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
		this.SetActiveTab(_tabName);
	}

	// Token: 0x060069B1 RID: 27057 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		base.RefreshBindings();
	}

	// Token: 0x060069B2 RID: 27058 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordLocalUserChanged(bool _loggedIn)
	{
		base.RefreshBindings();
	}

	// Token: 0x060069B3 RID: 27059 RVA: 0x002976EC File Offset: 0x002958EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordVoiceStateChanged(bool _self, ulong _userId)
	{
		if (!_self)
		{
			return;
		}
		base.RefreshBindings();
	}

	// Token: 0x060069B4 RID: 27060 RVA: 0x002976F8 File Offset: 0x002958F8
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

	// Token: 0x060069B5 RID: 27061 RVA: 0x0029775C File Offset: 0x0029595C
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged += this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged += this.discordLocalUserChanged;
		instance.StatusChanged += this.discordStatusChanged;
		instance.VoiceStateChanged += this.discordVoiceStateChanged;
	}

	// Token: 0x060069B6 RID: 27062 RVA: 0x002977B8 File Offset: 0x002959B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void onCloseDiscord()
	{
		DiscordManager instance = DiscordManager.Instance;
		instance.AudioDevicesChanged -= this.discordVoiceDeviceListChanged;
		instance.LocalUserChanged -= this.discordLocalUserChanged;
		instance.StatusChanged -= this.discordStatusChanged;
		instance.VoiceStateChanged -= this.discordVoiceStateChanged;
	}

	// Token: 0x060069C5 RID: 27077 RVA: 0x002979A4 File Offset: 0x00295BA4
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

	// Token: 0x04005090 RID: 20624
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice noDeviceEntry = new IPartyVoice.VoiceAudioDeviceNotFound();

	// Token: 0x04005091 RID: 20625
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly IPartyVoice.VoiceAudioDevice defaultDeviceEntry = new IPartyVoice.VoiceAudioDeviceDefault();

	// Token: 0x04005092 RID: 20626
	[XuiBindComponent("OptionVoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceOutputDevice;

	// Token: 0x04005093 RID: 20627
	[XuiBindComponent("VoiceOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceOutputDevice;

	// Token: 0x04005094 RID: 20628
	[XuiBindComponent("OptionVoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionVoiceInputDevice;

	// Token: 0x04005095 RID: 20629
	[XuiBindComponent("VoiceInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboVoiceInputDevice;

	// Token: 0x04005096 RID: 20630
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastDeviceListUpdateFrame;

	// Token: 0x04005097 RID: 20631
	public static string ID = "";

	// Token: 0x04005099 RID: 20633
	public const string DiscordTabName = "xuiOptionsAudioDiscord";

	// Token: 0x0400509A RID: 20634
	[XuiBindComponent("OptionDiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordOutputDevice;

	// Token: 0x0400509B RID: 20635
	[XuiBindComponent("DiscordOutputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordOutputDevice;

	// Token: 0x0400509C RID: 20636
	[XuiBindComponent("OptionDiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryCustom optionDiscordInputDevice;

	// Token: 0x0400509D RID: 20637
	[XuiBindComponent("DiscordInputDevice", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPartyVoice.VoiceAudioDevice> comboDiscordInputDevice;

	// Token: 0x0400509E RID: 20638
	[XuiBindComponent("OptionDiscordVoiceButtonMode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_OptionEntryGamePrefBool optionDiscordVoiceButtonMode;

	// Token: 0x0400509F RID: 20639
	[XuiBindComponent("btnDiscordInitialize", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordInitialize;

	// Token: 0x040050A0 RID: 20640
	[XuiBindComponent("btnDiscordLinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordLinkAccount;

	// Token: 0x040050A1 RID: 20641
	[XuiBindComponent("btnDiscordUnlinkAccount", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordUnlinkAccount;

	// Token: 0x040050A2 RID: 20642
	[XuiBindComponent("btnDiscordManageSocialSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageSocialSettings;

	// Token: 0x040050A3 RID: 20643
	[XuiBindComponent("btnDiscordManageBlockedUsers", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordManageBlockedUsers;
}
