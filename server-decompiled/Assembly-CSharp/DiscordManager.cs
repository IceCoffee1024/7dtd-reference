using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Audio;
using Discord.Sdk;
using Newtonsoft.Json;
using Platform;
using Twitch;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000305 RID: 773
public class DiscordManager
{
	// Token: 0x17000272 RID: 626
	// (get) Token: 0x060015CA RID: 5578 RVA: 0x00081BD3 File Offset: 0x0007FDD3
	public static DiscordManager Instance
	{
		get
		{
			DiscordManager discordManager;
			if ((discordManager = DiscordManager.instance) == null)
			{
				discordManager = (DiscordManager.instance = new DiscordManager());
			}
			return discordManager;
		}
	}

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060015CB RID: 5579 RVA: 0x00081BEC File Offset: 0x0007FDEC
	// (remove) Token: 0x060015CC RID: 5580 RVA: 0x00081C24 File Offset: 0x0007FE24
	public event DiscordManager.UserAuthorizationResultCallback UserAuthorizationResult;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060015CD RID: 5581 RVA: 0x00081C5C File Offset: 0x0007FE5C
	// (remove) Token: 0x060015CE RID: 5582 RVA: 0x00081C94 File Offset: 0x0007FE94
	public event Action<DiscordManager.EDiscordStatus> StatusChanged;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060015CF RID: 5583 RVA: 0x00081CCC File Offset: 0x0007FECC
	// (remove) Token: 0x060015D0 RID: 5584 RVA: 0x00081D04 File Offset: 0x0007FF04
	public event DiscordManager.LocalUserChangedCallback LocalUserChanged;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060015D1 RID: 5585 RVA: 0x00081D3C File Offset: 0x0007FF3C
	// (remove) Token: 0x060015D2 RID: 5586 RVA: 0x00081D74 File Offset: 0x0007FF74
	public event DiscordManager.LobbyStateChangedCallback LobbyStateChanged;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060015D3 RID: 5587 RVA: 0x00081DAC File Offset: 0x0007FFAC
	// (remove) Token: 0x060015D4 RID: 5588 RVA: 0x00081DE4 File Offset: 0x0007FFE4
	public event DiscordManager.LobbyMembersChangedCallback LobbyMembersChanged;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060015D5 RID: 5589 RVA: 0x00081E1C File Offset: 0x0008001C
	// (remove) Token: 0x060015D6 RID: 5590 RVA: 0x00081E54 File Offset: 0x00080054
	public event DiscordManager.CallChangedCallback CallChanged;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060015D7 RID: 5591 RVA: 0x00081E8C File Offset: 0x0008008C
	// (remove) Token: 0x060015D8 RID: 5592 RVA: 0x00081EC4 File Offset: 0x000800C4
	public event DiscordManager.CallStatusChangedCallback CallStatusChanged;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060015D9 RID: 5593 RVA: 0x00081EFC File Offset: 0x000800FC
	// (remove) Token: 0x060015DA RID: 5594 RVA: 0x00081F34 File Offset: 0x00080134
	public event DiscordManager.CallMembersChangedCallback CallMembersChanged;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060015DB RID: 5595 RVA: 0x00081F6C File Offset: 0x0008016C
	// (remove) Token: 0x060015DC RID: 5596 RVA: 0x00081FA4 File Offset: 0x000801A4
	public event DiscordManager.VoiceStateChangedCallback VoiceStateChanged;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060015DD RID: 5597 RVA: 0x00081FDC File Offset: 0x000801DC
	// (remove) Token: 0x060015DE RID: 5598 RVA: 0x00082014 File Offset: 0x00080214
	public event DiscordManager.SelfMuteStateChangedCallback SelfMuteStateChanged;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060015DF RID: 5599 RVA: 0x0008204C File Offset: 0x0008024C
	// (remove) Token: 0x060015E0 RID: 5600 RVA: 0x00082084 File Offset: 0x00080284
	public event DiscordManager.FriendsListChangedCallback FriendsListChanged;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060015E1 RID: 5601 RVA: 0x000820BC File Offset: 0x000802BC
	// (remove) Token: 0x060015E2 RID: 5602 RVA: 0x000820F4 File Offset: 0x000802F4
	public event DiscordManager.RelationshipChangedCallback RelationshipChanged;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x060015E3 RID: 5603 RVA: 0x0008212C File Offset: 0x0008032C
	// (remove) Token: 0x060015E4 RID: 5604 RVA: 0x00082164 File Offset: 0x00080364
	public event DiscordManager.ActivityInviteReceivedCallback ActivityInviteReceived;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x060015E5 RID: 5605 RVA: 0x0008219C File Offset: 0x0008039C
	// (remove) Token: 0x060015E6 RID: 5606 RVA: 0x000821D4 File Offset: 0x000803D4
	public event DiscordManager.ActivityJoiningCallback ActivityJoining;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x060015E7 RID: 5607 RVA: 0x0008220C File Offset: 0x0008040C
	// (remove) Token: 0x060015E8 RID: 5608 RVA: 0x00082244 File Offset: 0x00080444
	public event DiscordManager.PendingActionsUpdateCallback PendingActionsUpdate;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x060015E9 RID: 5609 RVA: 0x0008227C File Offset: 0x0008047C
	// (remove) Token: 0x060015EA RID: 5610 RVA: 0x000822B4 File Offset: 0x000804B4
	public event DiscordManager.AudioDevicesChangedCallback AudioDevicesChanged;

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x060015EB RID: 5611 RVA: 0x000822EC File Offset: 0x000804EC
	public DiscordManager.EDiscordStatus Status
	{
		get
		{
			if (this.client == null)
			{
				return DiscordManager.EDiscordStatus.NotInitialized;
			}
			DiscordManager.EDiscordStatus ediscordStatus;
			switch (this.client.GetStatus())
			{
			case Discord.Sdk.Client.Status.Disconnected:
				ediscordStatus = DiscordManager.EDiscordStatus.Disconnected;
				break;
			case Discord.Sdk.Client.Status.Connecting:
				ediscordStatus = DiscordManager.EDiscordStatus.Connecting;
				break;
			case Discord.Sdk.Client.Status.Connected:
				ediscordStatus = DiscordManager.EDiscordStatus.Connecting;
				break;
			case Discord.Sdk.Client.Status.Ready:
				ediscordStatus = DiscordManager.EDiscordStatus.Ready;
				break;
			case Discord.Sdk.Client.Status.Reconnecting:
				ediscordStatus = DiscordManager.EDiscordStatus.Connecting;
				break;
			case Discord.Sdk.Client.Status.Disconnecting:
				ediscordStatus = DiscordManager.EDiscordStatus.Disconnecting;
				break;
			default:
				ediscordStatus = DiscordManager.EDiscordStatus.Disconnected;
				break;
			}
			return ediscordStatus;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x060015EC RID: 5612 RVA: 0x0008234A File Offset: 0x0008054A
	public bool IsReady
	{
		get
		{
			return this.Status == DiscordManager.EDiscordStatus.Ready;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x060015ED RID: 5613 RVA: 0x00082355 File Offset: 0x00080555
	public bool IsInitialized
	{
		get
		{
			return this.Status > DiscordManager.EDiscordStatus.NotInitialized;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x060015EE RID: 5614 RVA: 0x00082360 File Offset: 0x00080560
	// (set) Token: 0x060015EF RID: 5615 RVA: 0x00082368 File Offset: 0x00080568
	public DiscordManager.DiscordUser LocalUser
	{
		get
		{
			return this.localUser;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.localUser == value)
			{
				return;
			}
			this.localUser = value;
			this.localDiscordOrEntityIdChanged();
			DiscordManager.LocalUserChangedCallback localUserChanged = this.LocalUserChanged;
			if (localUserChanged != null)
			{
				localUserChanged(this.localUser != null);
			}
			this.refreshCachedUserHandlesAndRelationships();
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x060015F0 RID: 5616 RVA: 0x000823A1 File Offset: 0x000805A1
	public static LoggingSeverity LogLevel
	{
		get
		{
			return DiscordManager.logLevel;
		}
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x000823A8 File Offset: 0x000805A8
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager()
	{
		this.setLogLevelsFromCmdLine();
		if (!DiscordManager.SupportsProvisionalAccounts)
		{
			Log.Warning("[Discord] Full Discord integration only available when running with EOS cross platform provider!");
		}
		this.Settings = DiscordManager.DiscordSettings.Load();
		this.userMappings = new DiscordManager.DiscordUserMappingManager(this);
		this.AuthManager = new DiscordManager.AuthAndLoginManager(this);
		this.Presence = new DiscordManager.PresenceManager(this);
		this.globalLobby = new DiscordManager.LobbyInfo(this, DiscordManager.ELobbyType.Global);
		this.partyLobby = new DiscordManager.LobbyInfo(this, DiscordManager.ELobbyType.Party);
		this.AudioOutput = new DiscordManager.AudioDeviceConfig(this, true);
		this.AudioInput = new DiscordManager.AudioDeviceConfig(this, false);
		this.UserSettings = DiscordManager.DiscordUserSettingsManager.Load();
		this.registerGameEventHandlers();
		this.ActivityInviteReceived += delegate(DiscordManager.DiscordUser _, bool _, ActivityActionTypes _)
		{
			this.updatePendingActionsEvent();
		};
		this.RelationshipChanged += delegate(DiscordManager.DiscordUser _)
		{
			this.updatePendingActionsEvent();
		};
		this.FriendsListChanged += this.updatePendingActionsEvent;
		if (GameManager.IsDedicatedServer || !this.Settings.DiscordFirstTimeInfoShown || this.Settings.DiscordDisabled)
		{
			return;
		}
		this.Init(false);
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x000824BC File Offset: 0x000806BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setLogLevelsFromCmdLine()
	{
		string text = GameUtils.GetLaunchArgument("discordloglevel");
		LoggingSeverity loggingSeverity;
		if (text != null && EnumUtils.TryParse<LoggingSeverity>(text, out loggingSeverity, true))
		{
			DiscordManager.logLevel = loggingSeverity;
		}
		text = GameUtils.GetLaunchArgument("discordloglevelrtc");
		LoggingSeverity loggingSeverity2;
		if (text != null && EnumUtils.TryParse<LoggingSeverity>(text, out loggingSeverity2, true))
		{
			DiscordManager.logLevelRtc = loggingSeverity2;
		}
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x00082508 File Offset: 0x00080708
	public void Init(bool _forceReinit = false)
	{
		if (!VoiceHelpers.VoiceAllowed || !PermissionsManager.IsMultiplayerAllowed())
		{
			return;
		}
		if (this.client != null)
		{
			if (!_forceReinit)
			{
				return;
			}
			this.client.Disconnect();
		}
		else
		{
			NativeMethods.UnhandledException += DiscordManager.nativeMethodException;
			Log.Out(string.Format("[Discord] Initializing, version {0}.{1}.{2}, # {3}", new object[]
			{
				Discord.Sdk.Client.GetVersionMajor(),
				Discord.Sdk.Client.GetVersionMinor(),
				Discord.Sdk.Client.GetVersionPatch(),
				Discord.Sdk.Client.GetVersionHash()
			}));
			this.client = new Discord.Sdk.Client();
			this.client.SetApplicationId(1296840202995896363UL);
			this.client.SetLogDir("", LoggingSeverity.None);
			this.client.SetVoiceLogDir("", LoggingSeverity.None);
			this.client.AddLogCallback(new Discord.Sdk.Client.LogCallback(this.OnDiscordLogMessageReceived), DiscordManager.logLevel);
			this.client.AddVoiceLogCallback(new Discord.Sdk.Client.LogCallback(this.OnDiscordRtcLogMessageReceived), DiscordManager.logLevelRtc);
			this.registerGameStartupForInvites();
		}
		this.client.UpdateToken(AuthorizationTokenType.Bearer, "", delegate(ClientResult _)
		{
		});
		this.registerGlobalDiscordCallbacks();
		this.client.SetAutomaticGainControl(true);
		this.client.SetEchoCancellation(true);
		this.client.SetNoiseSuppression(true);
		this.client.SetOutputVolume((float)this.Settings.OutputVolume);
		this.client.SetInputVolume((float)this.Settings.InputVolume);
		this.updateAudioDeviceList();
		this.Settings.OutputDeviceChanged += delegate(string _)
		{
			this.AudioOutput.UpdateAudioDeviceList();
		};
		this.Settings.InputDeviceChanged += delegate(string _)
		{
			this.AudioInput.UpdateAudioDeviceList();
		};
		this.Settings.OutputVolumeChanged += delegate(int _v)
		{
			Discord.Sdk.Client client = this.client;
			if (client == null)
			{
				return;
			}
			client.SetOutputVolume((float)_v);
		};
		this.Settings.InputVolumeChanged += delegate(int _v)
		{
			Discord.Sdk.Client client2 = this.client;
			if (client2 == null)
			{
				return;
			}
			client2.SetInputVolume((float)_v);
		};
		this.Settings.VoiceModePttChanged += delegate(bool _)
		{
			DiscordManager.LobbyInfo activeVoiceLobby = this.ActiveVoiceLobby;
			if (activeVoiceLobby == null)
			{
				return;
			}
			activeVoiceLobby.VoiceCall.SetPushToTalkMode();
		};
		this.friendsServerList.Init(null);
		ServerListManager.Instance.RegisterAdditionalServerList(this.friendsServerList);
		Log.Out("[Discord] Initialized");
		Action<DiscordManager.EDiscordStatus> statusChanged = this.StatusChanged;
		if (statusChanged != null)
		{
			statusChanged(this.Status);
		}
		this.registerGameChatHandling();
		DiscordManager.CallInfo.LoadSounds();
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x00082764 File Offset: 0x00080964
	[PublicizedFrom(EAccessModifier.Private)]
	public void registerGlobalDiscordCallbacks()
	{
		this.client.SetDeviceChangeCallback(new Discord.Sdk.Client.DeviceChangeCallback(this.OnDeviceChanged));
		this.client.SetLobbyCreatedCallback(new Discord.Sdk.Client.LobbyCreatedCallback(this.OnLobbyCreated));
		this.client.SetLobbyDeletedCallback(new Discord.Sdk.Client.LobbyDeletedCallback(this.OnLobbyDeleted));
		this.client.SetLobbyUpdatedCallback(new Discord.Sdk.Client.LobbyUpdatedCallback(this.OnLobbyUpdated));
		this.client.SetLobbyMemberAddedCallback(new Discord.Sdk.Client.LobbyMemberAddedCallback(this.OnLobbyMemberAdded));
		this.client.SetLobbyMemberRemovedCallback(new Discord.Sdk.Client.LobbyMemberRemovedCallback(this.OnLobbyMemberRemoved));
		this.client.SetLobbyMemberUpdatedCallback(new Discord.Sdk.Client.LobbyMemberUpdatedCallback(this.OnLobbyMemberUpdated));
		this.client.SetMessageCreatedCallback(new Discord.Sdk.Client.MessageCreatedCallback(this.OnMessageCreated));
		this.client.SetMessageDeletedCallback(new Discord.Sdk.Client.MessageDeletedCallback(this.OnMessageDeleted));
		this.client.SetMessageUpdatedCallback(new Discord.Sdk.Client.MessageUpdatedCallback(this.OnMessageUpdated));
		this.client.SetNoAudioInputCallback(new Discord.Sdk.Client.NoAudioInputCallback(this.OnNoAudioInput));
		this.client.SetRelationshipCreatedCallback(new Discord.Sdk.Client.RelationshipCreatedCallback(this.OnRelationshipCreated));
		this.client.SetRelationshipDeletedCallback(new Discord.Sdk.Client.RelationshipDeletedCallback(this.OnRelationshipDeleted));
		this.client.SetUserUpdatedCallback(new Discord.Sdk.Client.UserUpdatedCallback(this.OnUserUpdated));
		this.client.SetVoiceParticipantChangedCallback(new Discord.Sdk.Client.VoiceParticipantChangedCallback(this.OnVoiceParticipantChanged));
		this.AuthManager.RegisterDiscordCallbacks();
		this.Presence.RegisterDiscordCallbacks();
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000828E0 File Offset: 0x00080AE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void registerGameStartupForInvites()
	{
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			return;
		}
		bool flag;
		switch (PlatformManager.NativePlatform.PlatformIdentifier)
		{
		case EPlatformIdentifier.Local:
			flag = this.<registerGameStartupForInvites>g__RegisterNativePcLauncher|100_1();
			goto IL_0083;
		case EPlatformIdentifier.EOS:
			flag = false;
			goto IL_0083;
		case EPlatformIdentifier.Steam:
			flag = this.<registerGameStartupForInvites>g__RegisterSteamLauncher|100_0();
			goto IL_0083;
		case EPlatformIdentifier.XBL:
			flag = this.<registerGameStartupForInvites>g__RegisterNativePcLauncher|100_1();
			goto IL_0083;
		case EPlatformIdentifier.EGS:
			flag = false;
			goto IL_0083;
		}
		throw new ArgumentOutOfRangeException("PlatformIdentifier", "Invalid native platform " + PlatformManager.NativePlatform.PlatformIdentifier.ToStringCached<EPlatformIdentifier>() + " for registering Discord launch command");
		IL_0083:
		bool flag2 = flag;
		Log.Out("[Discord] Registering game for Discord invites: " + (flag2 ? "Success" : "Failed"));
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x00082990 File Offset: 0x00080B90
	[PublicizedFrom(EAccessModifier.Private)]
	public void cleanup(ref ModEvents.SGameShutdownData _data)
	{
		this.Settings.Save();
		if (this.client == null)
		{
			return;
		}
		Log.Out("[Discord] Cleanup");
		this.leaveLobbies(true);
		this.client.Disconnect();
		this.client.Dispose();
		this.client = null;
		Action<DiscordManager.EDiscordStatus> statusChanged = this.StatusChanged;
		if (statusChanged == null)
		{
			return;
		}
		statusChanged(this.Status);
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060015F7 RID: 5623 RVA: 0x000829F5 File Offset: 0x00080BF5
	public static bool SupportsProvisionalAccounts
	{
		get
		{
			return PlatformManager.CrossplatformPlatform != null && PlatformManager.CrossplatformPlatform.PlatformIdentifier == EPlatformIdentifier.EOS;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x060015F8 RID: 5624 RVA: 0x00082A0D File Offset: 0x00080C0D
	public static bool SupportsFullAccounts
	{
		get
		{
			return !DeviceFlag.PS5.IsCurrent();
		}
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x00082A1C File Offset: 0x00080C1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRelationshipCreated(ulong _userId, bool _isDiscordRelationship)
	{
		DiscordManager.logCallbackInfo(string.Format("OnRelationshipCreated: {0}, {1}", _userId, _isDiscordRelationship), LogType.Log);
		DiscordManager.DiscordUser user = this.GetUser(_userId);
		user.UpdateRelationship();
		user.UpdatePresenceInfo();
		DiscordManager.RelationshipChangedCallback relationshipChanged = this.RelationshipChanged;
		if (relationshipChanged != null)
		{
			relationshipChanged(user);
		}
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged == null)
		{
			return;
		}
		friendsListChanged();
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x00082A7C File Offset: 0x00080C7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRelationshipDeleted(ulong _userId, bool _isDiscordRelationship)
	{
		DiscordManager.logCallbackInfo(string.Format("OnRelationshipDeleted: {0}, {1}", _userId, _isDiscordRelationship), LogType.Log);
		DiscordManager.DiscordUser user = this.GetUser(_userId);
		user.UpdateRelationship();
		user.UpdatePresenceInfo();
		DiscordManager.RelationshipChangedCallback relationshipChanged = this.RelationshipChanged;
		if (relationshipChanged != null)
		{
			relationshipChanged(user);
		}
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged == null)
		{
			return;
		}
		friendsListChanged();
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x00082ADC File Offset: 0x00080CDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearFriends()
	{
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num, out discordUser);
			discordUser.UpdateRelationship();
		}
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged == null)
		{
			return;
		}
		friendsListChanged();
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x00082B4C File Offset: 0x00080D4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void getFriends()
	{
		if (!this.IsReady)
		{
			DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
			if (friendsListChanged == null)
			{
				return;
			}
			friendsListChanged();
			return;
		}
		else
		{
			foreach (RelationshipHandle relationshipHandle in this.client.GetRelationships())
			{
				this.GetUser(relationshipHandle.Id()).UpdatePresenceInfo();
			}
			DiscordManager.FriendsListChangedCallback friendsListChanged2 = this.FriendsListChanged;
			if (friendsListChanged2 == null)
			{
				return;
			}
			friendsListChanged2();
			return;
		}
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x00082BB4 File Offset: 0x00080DB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnUserUpdated(ulong _userId)
	{
		DiscordManager.DiscordUser user = this.GetUser(_userId);
		user.UpdatePresenceInfo();
		user.UpdateRelationship();
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged != null)
		{
			friendsListChanged();
		}
		DiscordManager.logCallbackInfo(string.Format("OnUserUpdated: user={0}", user), LogType.Log);
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x00082BF8 File Offset: 0x00080DF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void onLogMessageReceivedGeneric(string _message, LoggingSeverity _severity, string _prefix)
	{
		Match match = DiscordManager.logMessageMatcher.Match(_message);
		if (match.Success)
		{
			if (match.Groups[4].Value.IndexOf('\n') >= 0)
			{
				_message = match.Groups[4].Value + " (" + match.Groups[3].Value + ")\n";
			}
			else
			{
				_message = match.Groups[4].Value + " (" + match.Groups[3].Value + ")";
			}
		}
		else if (_message.IndexOf('\n') == _message.Length - 1)
		{
			_message = _message.Substring(0, _message.Length - 1);
		}
		switch (_severity)
		{
		case LoggingSeverity.Verbose:
			Log.Out("[Discord]" + _prefix + "[Log](Verb) " + _message);
			return;
		case LoggingSeverity.Info:
			Log.Out("[Discord]" + _prefix + "[Log](Info) " + _message);
			return;
		case LoggingSeverity.Warning:
			Log.Warning("[Discord]" + _prefix + "[Log] " + _message);
			return;
		case LoggingSeverity.Error:
			Log.Error("[Discord]" + _prefix + "[Log] " + _message);
			return;
		case LoggingSeverity.None:
			Log.Out("[Discord]" + _prefix + "[Log](None) " + _message);
			return;
		default:
			Log.Error(string.Format("[Discord]{0}[Log] Unknown log severity ({1}): {2}", _prefix, _severity, _message));
			return;
		}
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x00082D6C File Offset: 0x00080F6C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void nativeMethodException(Exception _e)
	{
		Log.Error("[Discord] Exception: " + _e.Message);
		Log.Exception(_e);
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x00082D89 File Offset: 0x00080F89
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDiscordLogMessageReceived(string _message, LoggingSeverity _severity)
	{
		this.onLogMessageReceivedGeneric(_message, _severity, "");
	}

	// Token: 0x06001601 RID: 5633 RVA: 0x00082D98 File Offset: 0x00080F98
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDiscordRtcLogMessageReceived(string _message, LoggingSeverity _severity)
	{
		this.onLogMessageReceivedGeneric(_message, _severity, "[RTC]");
	}

	// Token: 0x06001602 RID: 5634 RVA: 0x00082DA7 File Offset: 0x00080FA7
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnMessageCreated(ulong _messageId)
	{
		this.handleMessage(true, _messageId);
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x00082DB1 File Offset: 0x00080FB1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnMessageDeleted(ulong _messageId, ulong _channelId)
	{
		DiscordManager.logCallbackInfo(string.Format("OnMessageDeleted: msg={0}, channel={1}", _messageId, _channelId), LogType.Log);
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x00082DCF File Offset: 0x00080FCF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnMessageUpdated(ulong _messageId)
	{
		this.handleMessage(false, _messageId);
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x00082DDC File Offset: 0x00080FDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleMessage(bool _created, ulong _messageId)
	{
		if (this.client == null)
		{
			return;
		}
		using (MessageHandle messageHandle = this.client.GetMessageHandle(_messageId))
		{
			if (messageHandle == null)
			{
				DiscordManager.logCallbackInfo(string.Format("{0}: msg={1}, No message handle", _created ? "OnMessageCreated" : "OnMessageUpdated", _messageId), LogType.Warning);
			}
			else
			{
				ulong num = messageHandle.ChannelId();
				ulong num2 = messageHandle.Id();
				string text = messageHandle.Content();
				messageHandle.RawContent();
				using (AdditionalContent additionalContent = messageHandle.AdditionalContent())
				{
					AdditionalContentType? additionalContentType = ((additionalContent != null) ? new AdditionalContentType?(additionalContent.Type()) : null);
					DiscordManager.DiscordUser user = this.GetUser(messageHandle.AuthorId());
					bool isLocalAccount = user.IsLocalAccount;
					DiscordManager.DiscordUser discordUser = (isLocalAccount ? this.GetUser(messageHandle.RecipientId()) : this.LocalUser);
					DiscordManager.DiscordUser discordUser2 = (isLocalAccount ? discordUser : user);
					DiscordManager.logCallbackInfo(string.Format("{0}: msg={1} channel={2} sender='{3}' recipient='{4}' outbound='{5}' text='<redacted>' rawContent='<redacted>'", new object[]
					{
						_created ? "OnMessageCreated" : "OnMessageUpdated",
						num2,
						num,
						user.DisplayName,
						discordUser.DisplayName,
						isLocalAccount
					}), LogType.Log);
					if (additionalContent != null)
					{
						DiscordManager.logCallbackInfo(string.Format("{0}: Additional content: Count={1}, type={2} title='{3}'", new object[]
						{
							_created ? "OnMessageCreated" : "OnMessageUpdated",
							additionalContent.Count(),
							additionalContentType.Value.ToStringCached<AdditionalContentType>(),
							additionalContent.Title()
						}), LogType.Log);
					}
					if (_created)
					{
						if (this.Settings.DmPrivacyMode && !discordUser2.MessageSentFromGame)
						{
							if (DiscordManager.logLevel == LoggingSeverity.Verbose)
							{
								Log.Out(string.Format("[Discord] Not showing received DM: Privacy mode active and no message sent to the user yet ({0})", discordUser2));
							}
						}
						else
						{
							LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
							if (!(uiforPrimaryPlayer == null) && !(uiforPrimaryPlayer.entityPlayer == null))
							{
								EMessageSender emessageSender = EMessageSender.SenderIdAsPlayer;
								int num3;
								if (!this.userMappings.TryGetEntityId(discordUser2.ID, out num3))
								{
									emessageSender = EMessageSender.None;
									num3 = -1;
								}
								XUiC_Chat.EnforceTargetExists(uiforPrimaryPlayer.xui, EChatType.Discord, discordUser2.ID.ToString());
								if (!string.IsNullOrEmpty(text))
								{
									XUiC_ChatOutput.AddMessage(uiforPrimaryPlayer.xui, EnumGameMessages.Chat, text, EChatType.Discord, isLocalAccount ? EChatDirection.Outbound : EChatDirection.Inbound, num3, discordUser2.DisplayName, discordUser2.ID.ToString(), emessageSender, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
								}
								if (additionalContent != null)
								{
									if (additionalContentType != null)
									{
										string text2;
										switch (additionalContentType.GetValueOrDefault())
										{
										case AdditionalContentType.Other:
											text2 = Localization.Get("discordMessageAdditionalContentTypeOther", false, null);
											break;
										case AdditionalContentType.Attachment:
											text2 = ((additionalContent.Count() <= 1) ? Localization.Get("discordMessageAdditionalContentTypeAttachmentSingle", false, null) : string.Format(Localization.Get("discordMessageAdditionalContentTypeAttachmentMultiple", false, null), additionalContent.Count()));
											break;
										case AdditionalContentType.Poll:
											text2 = Localization.Get("discordMessageAdditionalContentTypePoll", false, null);
											break;
										case AdditionalContentType.VoiceMessage:
											text2 = Localization.Get("discordMessageAdditionalContentTypeVoiceMessage", false, null);
											break;
										case AdditionalContentType.Thread:
											text2 = Localization.Get("discordMessageAdditionalContentTypeThread", false, null);
											break;
										case AdditionalContentType.Embed:
											text2 = Localization.Get("discordMessageAdditionalContentTypeEmbed", false, null);
											break;
										case AdditionalContentType.Sticker:
											text2 = Localization.Get("discordMessageAdditionalContentTypeSticker", false, null);
											break;
										default:
											goto IL_0322;
										}
										string text3 = text2;
										string text4 = ((this.client.CanOpenMessageInDiscord(num2) && !this.LocalUser.IsProvisionalAccount && !(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent()) ? (LabelUrlUtils.BuildUrlFunctionString("DiscordMessageButton", new ValueTuple<string, string>("MessageId", num2.ToString())) + text3 + " [sp=ui_game_symbol_external_link][/url]") : text3);
										XUiC_ChatOutput.AddMessage(uiforPrimaryPlayer.xui, EnumGameMessages.Chat, text4, EChatType.Discord, isLocalAccount ? EChatDirection.Outbound : EChatDirection.Inbound, num3, discordUser2.DisplayName, discordUser2.ID.ToString(), emessageSender, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
										goto IL_03C8;
									}
									IL_0322:
									throw new ArgumentOutOfRangeException("additionalContentType", additionalContentType.Value, "Invalid AdditionalContentType");
								}
							}
						}
					}
					IL_03C8:;
				}
			}
		}
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x00083200 File Offset: 0x00081400
	[PublicizedFrom(EAccessModifier.Private)]
	public void registerGameChatHandling()
	{
		LabelUrlUtils.RegisterLabelUrlHandler("DiscordMessageButton", new LabelUrlUtils.HandleTextClickUrlDelegate(this.<registerGameChatHandling>g__DiscordButtonHandler|123_1), new LabelUrlUtils.HandleTextHoverUrlDelegate(DiscordManager.<registerGameChatHandling>g__DiscordButtonHoverHandler|123_0));
		XUiC_Chat.RegisterCustomMessagingHandler(EChatType.Discord, new XUiC_Chat.IsValidTarget(this.<registerGameChatHandling>g__IsValidTarget|123_4), new XUiC_Chat.GetTargetDisplayName(this.<registerGameChatHandling>g__GetTargetDisplayName|123_3), new XUiC_Chat.SendMessage(this.<registerGameChatHandling>g__SendMessage|123_2));
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x00083259 File Offset: 0x00081459
	[PublicizedFrom(EAccessModifier.Private)]
	public void suppressDmNotifications(bool _suppress)
	{
		if (GameManager.IsDedicatedServer || !this.IsInitialized)
		{
			return;
		}
		if (GameManager.Instance.World == null)
		{
			_suppress = false;
		}
		if (!GamePrefs.GetBool(EnumGamePrefs.DiscordMuteDmNotifications))
		{
			_suppress = false;
		}
		this.client.SetShowingChat(_suppress);
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x00083295 File Offset: 0x00081495
	public DiscordManager.LobbyInfo GetLobby(DiscordManager.ELobbyType _type)
	{
		if (_type != DiscordManager.ELobbyType.Global)
		{
			return this.partyLobby;
		}
		return this.globalLobby;
	}

	// Token: 0x06001609 RID: 5641 RVA: 0x000832A7 File Offset: 0x000814A7
	[PublicizedFrom(EAccessModifier.Private)]
	public void leaveLobbies(bool _manual = true)
	{
		this.LeaveLobbyVoice(_manual);
		this.globalLobby.Leave(_manual);
		this.partyLobby.Leave(_manual);
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x000832C8 File Offset: 0x000814C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLobbyCreated(ulong _lobbyId)
	{
		DiscordManager.logCallbackInfo(string.Format("OnLobbyCreated: lobby={0}", _lobbyId), LogType.Log);
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x000832E0 File Offset: 0x000814E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLobbyDeleted(ulong _lobbyId)
	{
		DiscordManager.logCallbackInfo(string.Format("OnLobbyDeleted: lobby={0}", _lobbyId), LogType.Log);
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x000832F8 File Offset: 0x000814F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLobbyUpdated(ulong _lobbyId)
	{
		DiscordManager.logCallbackInfo(string.Format("OnLobbyUpdated: lobby={0}", _lobbyId), LogType.Log);
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x00083310 File Offset: 0x00081510
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLobbyMemberAdded(ulong _lobbyId, ulong _memberId)
	{
		this.OnLobbyMemberChanged(_lobbyId, _memberId, DiscordManager.LobbyMemberActionType.Add);
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x0008331B File Offset: 0x0008151B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLobbyMemberRemoved(ulong _lobbyId, ulong _memberId)
	{
		this.OnLobbyMemberChanged(_lobbyId, _memberId, DiscordManager.LobbyMemberActionType.Remove);
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x00083326 File Offset: 0x00081526
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLobbyMemberUpdated(ulong _lobbyId, ulong _memberId)
	{
		this.OnLobbyMemberChanged(_lobbyId, _memberId, DiscordManager.LobbyMemberActionType.Update);
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x00083334 File Offset: 0x00081534
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLobbyMemberChanged(ulong _lobbyId, ulong _memberId, DiscordManager.LobbyMemberActionType _actionType)
	{
		DiscordManager.DiscordUser user = this.GetUser(_memberId);
		DiscordManager.logCallbackInfo(string.Format("OnLobbyMember{0}: lobby={1} user={2}", _actionType.ToStringCached<DiscordManager.LobbyMemberActionType>(), _lobbyId, user), LogType.Log);
		this.globalLobby.UpdateMembers();
		this.partyLobby.UpdateMembers();
		DiscordManager.LobbyInfo activeVoiceLobby = this.ActiveVoiceLobby;
		if (activeVoiceLobby == null)
		{
			return;
		}
		activeVoiceLobby.VoiceCall.UpdateMembers(false);
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06001611 RID: 5649 RVA: 0x00083392 File Offset: 0x00081592
	// (set) Token: 0x06001612 RID: 5650 RVA: 0x000833A5 File Offset: 0x000815A5
	public bool Mute
	{
		get
		{
			Discord.Sdk.Client client = this.client;
			return client != null && client.GetSelfMuteAll();
		}
		set
		{
			Discord.Sdk.Client client = this.client;
			if (client != null)
			{
				client.SetSelfMuteAll(value);
			}
			DiscordManager.SelfMuteStateChangedCallback selfMuteStateChanged = this.SelfMuteStateChanged;
			if (selfMuteStateChanged == null)
			{
				return;
			}
			selfMuteStateChanged(value, this.Deaf);
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06001613 RID: 5651 RVA: 0x000833D0 File Offset: 0x000815D0
	// (set) Token: 0x06001614 RID: 5652 RVA: 0x000833E3 File Offset: 0x000815E3
	public bool Deaf
	{
		get
		{
			Discord.Sdk.Client client = this.client;
			return client != null && client.GetSelfDeafAll();
		}
		set
		{
			Discord.Sdk.Client client = this.client;
			if (client != null)
			{
				client.SetSelfDeafAll(value);
			}
			DiscordManager.SelfMuteStateChangedCallback selfMuteStateChanged = this.SelfMuteStateChanged;
			if (selfMuteStateChanged == null)
			{
				return;
			}
			selfMuteStateChanged(this.Mute, value);
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06001615 RID: 5653 RVA: 0x0008340E File Offset: 0x0008160E
	public DiscordManager.LobbyInfo ActiveVoiceLobby
	{
		get
		{
			if (this.partyLobby.IsInVoice)
			{
				return this.partyLobby;
			}
			if (this.globalLobby.IsInVoice)
			{
				return this.globalLobby;
			}
			return null;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06001616 RID: 5654 RVA: 0x0008343C File Offset: 0x0008163C
	public bool AnyLobbyInUnstableVoiceState
	{
		get
		{
			Call.Status status = this.globalLobby.VoiceCall.Status;
			if (status != Call.Status.Connected && status != Call.Status.Disconnected)
			{
				return true;
			}
			status = this.partyLobby.VoiceCall.Status;
			return status != Call.Status.Connected && status != Call.Status.Disconnected;
		}
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x00083480 File Offset: 0x00081680
	public void JoinLobbyVoice(DiscordManager.ELobbyType _type)
	{
		DiscordManager.LobbyInfo lobby = this.GetLobby(_type);
		if (this.ActiveVoiceLobby == lobby)
		{
			return;
		}
		this.LeaveLobbyVoice(true);
		lobby.VoiceCall.Join();
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x000834B1 File Offset: 0x000816B1
	public void LeaveLobbyVoice(bool _manual = true)
	{
		DiscordManager.LobbyInfo activeVoiceLobby = this.ActiveVoiceLobby;
		if (activeVoiceLobby == null)
		{
			return;
		}
		activeVoiceLobby.VoiceCall.Leave(_manual);
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x000834C9 File Offset: 0x000816C9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnNoAudioInput(bool _inputDetected)
	{
		DiscordManager.logCallbackInfo(string.Format("OnNoAudioInput: inputDetected={0}", _inputDetected), LogType.Log);
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x000834E4 File Offset: 0x000816E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVoiceParticipantChanged(ulong _lobbyId, ulong _userId, bool _added)
	{
		DiscordManager.DiscordUser user = this.GetUser(_userId);
		DiscordManager.logCallbackInfo(string.Format("OnVoiceParticipantChanged: lobby={0} user={1} added={2}", _lobbyId, user, _added), LogType.Log);
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x00083516 File Offset: 0x00081716
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDeviceChanged(AudioDevice[] _inputDevices, AudioDevice[] _outputDevices)
	{
		DiscordManager.logCallbackInfo(string.Format("OnDeviceChanged: input devices: {0}, output devices: {1}", _inputDevices.Length, _outputDevices.Length), LogType.Log);
		this.AudioOutput.ApplyDevicesFound(_outputDevices);
		this.AudioInput.ApplyDevicesFound(_inputDevices);
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00083550 File Offset: 0x00081750
	[PublicizedFrom(EAccessModifier.Private)]
	public void fireAudioDevicesChanged(DiscordManager.AudioDeviceConfig _config)
	{
		DiscordManager.AudioDevicesChangedCallback audioDevicesChanged = this.AudioDevicesChanged;
		if (audioDevicesChanged == null)
		{
			return;
		}
		audioDevicesChanged(_config);
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x00083563 File Offset: 0x00081763
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAudioDeviceList()
	{
		this.AudioOutput.UpdateAudioDeviceList();
		this.AudioInput.UpdateAudioDeviceList();
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x0008357C File Offset: 0x0008177C
	[PublicizedFrom(EAccessModifier.Private)]
	public void registerGameEventHandlers()
	{
		ModEvents.GameFocus.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameFocusData>(this.gameFocusChanged));
		ModEvents.MainMenuOpening.RegisterHandler(new ModEvents.ModEventInterruptibleHandlerDelegate<ModEvents.SMainMenuOpeningData>(this.mainMenuOpening));
		ModEvents.UnityUpdate.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SUnityUpdateData>(this.update));
		ModEvents.ServerRegistered.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SServerRegisteredData>(this.serverStarted));
		ModEvents.PlayerJoinedGame.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SPlayerJoinedGameData>(this.playerJoined));
		ModEvents.PlayerSpawning.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SPlayerSpawningData>(this.playerSpawning));
		ModEvents.PlayerSpawnedInWorld.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SPlayerSpawnedInWorldData>(this.playerSpawned));
		ModEvents.PlayerDisconnected.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SPlayerDisconnectedData>(this.playerDisconnected));
		ModEvents.GameStarting.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartingData>(this.gameStarting));
		ModEvents.GameUpdate.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameUpdateData>(this.inGameUpdate));
		ModEvents.WorldShuttingDown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SWorldShuttingDownData>(this.gameEnded));
		ModEvents.GameShutdown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameShutdownData>(this.cleanup));
		this.AuthManager.RegisterGameEventHandlers();
		GameManager.Instance.OnLocalPlayerChanged += this.localPlayerChangedEvent;
		PlatformUserManager.BlockedStateChanged += this.playerBlockStateChanged;
		World world = GameManager.Instance.World;
		EntityPlayerLocal entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		if (entityPlayerLocal != null)
		{
			this.playerCreated(entityPlayerLocal);
		}
		GamePrefs.OnGamePrefChanged += this.gamePrefChanged;
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x000836FB File Offset: 0x000818FB
	[PublicizedFrom(EAccessModifier.Private)]
	public void gamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.DiscordMuteDmNotifications)
		{
			this.suppressDmNotifications(GameManager.Instance.World != null);
		}
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x00083718 File Offset: 0x00081918
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameFocusChanged(ref ModEvents.SGameFocusData _data)
	{
		this.suppressDmNotifications(_data.IsFocused);
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x00083728 File Offset: 0x00081928
	[PublicizedFrom(EAccessModifier.Private)]
	public ModEvents.EModEventResult mainMenuOpening(ref ModEvents.SMainMenuOpeningData _data)
	{
		if (_data.OpenedBefore)
		{
			return ModEvents.EModEventResult.Continue;
		}
		if (PlatformManager.MultiPlatform.User.UserStatus != EUserStatus.LoggedIn)
		{
			return ModEvents.EModEventResult.Continue;
		}
		if (GameManager.IsDedicatedServer || !VoiceHelpers.VoiceAllowed || !PermissionsManager.IsMultiplayerAllowed())
		{
			return ModEvents.EModEventResult.Continue;
		}
		if (!this.Settings.DiscordFirstTimeInfoShown && !this.Settings.DiscordDisabled)
		{
			LocalPlayerUI.primaryUI.windowManager.Open(XUiC_DiscordInfo.ID, true);
			return ModEvents.EModEventResult.StopHandlersAndVanilla;
		}
		if (this.Settings.DiscordDisabled)
		{
			return ModEvents.EModEventResult.Continue;
		}
		this.Init(false);
		if (this.AuthManager.IsLoggingIn || this.Status != DiscordManager.EDiscordStatus.Disconnected)
		{
			return ModEvents.EModEventResult.Continue;
		}
		XUiC_DiscordLogin.Open(null, true, true, true, true, false);
		this.AuthManager.AutoLogin();
		return ModEvents.EModEventResult.StopHandlersAndVanilla;
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x000837DF File Offset: 0x000819DF
	[PublicizedFrom(EAccessModifier.Private)]
	public void update(ref ModEvents.SUnityUpdateData _data)
	{
		if (!this.IsReady)
		{
			return;
		}
		this.handlePushToTalkButton();
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x000837F0 File Offset: 0x000819F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void handlePushToTalkButton()
	{
		DiscordManager.LobbyInfo activeVoiceLobby = this.ActiveVoiceLobby;
		if (activeVoiceLobby == null || !activeVoiceLobby.IsInVoice)
		{
			return;
		}
		if (this.Settings.VoiceModePtt)
		{
			this.ActiveVoiceLobby.VoiceCall.SetPushToTalkActive(VoiceHelpers.PushToTalkPressed());
			return;
		}
		if (VoiceHelpers.PushToTalkWasPressed())
		{
			this.Mute = !this.Mute;
		}
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x0008384B File Offset: 0x00081A4B
	[PublicizedFrom(EAccessModifier.Private)]
	public void serverStarted(ref ModEvents.SServerRegisteredData _data)
	{
		if (this.globalLobby.Secret != null)
		{
			return;
		}
		this.ReceivedLobbySecret(DiscordManager.ELobbyType.Global, SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoString.IP) + "_" + Utils.GenerateGuid());
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x00083881 File Offset: 0x00081A81
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerJoined(ref ModEvents.SPlayerJoinedGameData _data)
	{
		this.userMappings.SendMappingsToClient(_data.ClientInfo);
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x00083894 File Offset: 0x00081A94
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerSpawning(ref ModEvents.SPlayerSpawningData _data)
	{
		_data.ClientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageDiscordLobbySecret>().Setup(DiscordManager.ELobbyType.Global, this.globalLobby.Secret));
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x000838B8 File Offset: 0x00081AB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerSpawned(ref ModEvents.SPlayerSpawnedInWorldData _data)
	{
		this.updateUserNames();
		if (GameManager.IsDedicatedServer || !this.IsInitialized)
		{
			return;
		}
		if (!_data.IsLocalPlayer)
		{
			return;
		}
		this.suppressDmNotifications(true);
		if (this.Settings.AutoJoinVoiceMode == DiscordManager.EAutoJoinVoiceMode.Global && this.globalLobby.Secret != null)
		{
			this.JoinLobbyVoice(DiscordManager.ELobbyType.Global);
		}
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x00083910 File Offset: 0x00081B10
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerDisconnected(ref ModEvents.SPlayerDisconnectedData _data)
	{
		int entityId = _data.ClientInfo.entityId;
		if (entityId == -1)
		{
			return;
		}
		this.userMappings.UpdateMapping(entityId, true, 0UL);
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged != null)
		{
			friendsListChanged();
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageDiscordIdMappings>().Setup(entityId, true, 0UL), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00083988 File Offset: 0x00081B88
	public void ReceivedLobbySecret(DiscordManager.ELobbyType _lobbyType, string _secret)
	{
		DiscordManager.LobbyInfo lobby = this.GetLobby(_lobbyType);
		Log.Out("[Discord] " + _lobbyType.ToStringCached<DiscordManager.ELobbyType>() + " lobby: " + _secret);
		lobby.Secret = _secret;
		if (GameManager.IsDedicatedServer || !this.IsReady)
		{
			return;
		}
		lobby.Join(true);
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x000839D6 File Offset: 0x00081BD6
	public void LeftParty()
	{
		this.partyLobby.Secret = null;
		if (GameManager.IsDedicatedServer || !this.IsInitialized)
		{
			return;
		}
		this.partyLobby.Leave(false);
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x00083A00 File Offset: 0x00081C00
	public void UserMappingReceived(int _entityId, bool _remove, ulong _discordId, bool _batch = false)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageDiscordIdMappings>().Setup(_entityId, _remove, _discordId), false, -1, -1, -1, null, 192, false);
		}
		this.userMappings.UpdateMapping(_entityId, _remove, _discordId);
		if (_discordId > 0UL)
		{
			this.GetUser(_discordId).UpdatePlayerName(null);
		}
		if (!_batch)
		{
			DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
			if (friendsListChanged == null)
			{
				return;
			}
			friendsListChanged();
		}
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x00083A78 File Offset: 0x00081C78
	public void UserMappingsReceived(List<int> _entityIds, List<ulong> _discordIds)
	{
		for (int i = 0; i < _entityIds.Count; i++)
		{
			this.UserMappingReceived(_entityIds[i], false, _discordIds[i], true);
		}
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged == null)
		{
			return;
		}
		friendsListChanged();
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x00083ABC File Offset: 0x00081CBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameStarting(ref ModEvents.SGameStartingData _data)
	{
		this.Presence.SetRichPresenceState(new IRichPresence.PresenceStates?(_data.AsServer ? IRichPresence.PresenceStates.Loading : IRichPresence.PresenceStates.Connecting));
		this.resetPendingOutgoingJoinRequests();
		if (PlatformManager.MultiPlatform.User.UserStatus != EUserStatus.LoggedIn)
		{
			return;
		}
		if (GameManager.IsDedicatedServer || !VoiceHelpers.VoiceAllowed || !PermissionsManager.IsMultiplayerAllowed())
		{
			return;
		}
		if (this.Settings.DiscordDisabled)
		{
			return;
		}
		this.Init(false);
		if (this.AuthManager.IsLoggingIn || this.Status != DiscordManager.EDiscordStatus.Disconnected)
		{
			return;
		}
		this.AuthManager.AutoLogin();
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x00083B4C File Offset: 0x00081D4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void inGameUpdate(ref ModEvents.SGameUpdateData _data)
	{
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime < this.nextActivityUpdate)
		{
			return;
		}
		this.nextActivityUpdate = unscaledTime + 10f;
		this.Presence.SetRichPresenceState(new IRichPresence.PresenceStates?(IRichPresence.PresenceStates.InGame));
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x00083B88 File Offset: 0x00081D88
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameEnded(ref ModEvents.SWorldShuttingDownData _data)
	{
		this.globalLobby.Secret = null;
		this.partyLobby.Secret = null;
		this.userMappings.Clear();
		this.Presence.SetRichPresenceState(new IRichPresence.PresenceStates?(IRichPresence.PresenceStates.Menu));
		if (GameManager.IsDedicatedServer || !this.IsInitialized)
		{
			return;
		}
		this.leaveLobbies(true);
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged != null)
		{
			friendsListChanged();
		}
		this.suppressDmNotifications(false);
		this.Settings.Save();
		this.UserSettings.Save();
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x00083C10 File Offset: 0x00081E10
	[PublicizedFrom(EAccessModifier.Private)]
	public void localPlayerChangedEvent(EntityPlayerLocal _newLocalPlayer)
	{
		if (!(_newLocalPlayer != null))
		{
			this.playerDestroyed();
			return;
		}
		this.playerCreated(_newLocalPlayer);
		DiscordManager.DiscordUser discordUser = this.LocalUser;
		if (discordUser != null)
		{
			discordUser.UpdatePlayerName(_newLocalPlayer);
		}
		this.localDiscordOrEntityIdChanged();
		DiscordManager.FriendsListChangedCallback friendsListChanged = this.FriendsListChanged;
		if (friendsListChanged == null)
		{
			return;
		}
		friendsListChanged();
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x00083C5C File Offset: 0x00081E5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerCreated(EntityPlayerLocal _newLocalPlayer)
	{
		if (PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.OfflineMode)
		{
			return;
		}
		this.localPlayer = _newLocalPlayer;
		this.localPlayer.PartyJoined += this.playerJoinedParty;
		this.localPlayer.PartyLeave += this.playerLeftParty;
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x00083CB4 File Offset: 0x00081EB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerDestroyed()
	{
		if (this.localPlayer != null)
		{
			this.localPlayer.PartyJoined -= this.playerJoinedParty;
			this.localPlayer.PartyLeave -= this.playerLeftParty;
			this.localPlayer = null;
		}
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x00083D04 File Offset: 0x00081F04
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerJoinedParty(Party _affectedParty, EntityPlayer _player)
	{
		this.ReceivedLobbySecret(DiscordManager.ELobbyType.Party, string.Format("{0}_{1}", this.globalLobby.Secret, _affectedParty.PartyID));
		if (this.Settings.AutoJoinVoiceMode == DiscordManager.EAutoJoinVoiceMode.Party && this.ActiveVoiceLobby == null)
		{
			this.JoinLobbyVoice(DiscordManager.ELobbyType.Party);
		}
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x00083D55 File Offset: 0x00081F55
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerLeftParty(Party _affectedParty, EntityPlayer _player)
	{
		this.LeftParty();
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x00083D60 File Offset: 0x00081F60
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerBlockStateChanged(IPlatformUserData _ppd, EBlockType _blockType, EUserBlockState _blockState)
	{
		if (_blockType != EBlockType.VoiceChat)
		{
			return;
		}
		PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
		PersistentPlayerData persistentPlayerData = ((persistentPlayers != null) ? persistentPlayers.GetPlayerData(_ppd.PrimaryId) : null);
		if (persistentPlayerData == null || persistentPlayerData.EntityId == -1)
		{
			return;
		}
		DiscordManager.DiscordUser discordUser;
		if (!this.TryGetUserFromEntityId(persistentPlayerData.EntityId, out discordUser))
		{
			return;
		}
		DiscordManager.LobbyInfo activeVoiceLobby = this.ActiveVoiceLobby;
		if (activeVoiceLobby == null)
		{
			return;
		}
		activeVoiceLobby.VoiceCall.UpdateBlockState(discordUser, _blockState.IsBlocked());
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x00083DC9 File Offset: 0x00081FC9
	public void OpenDiscordSocialSettings()
	{
		this.client.OpenConnectedGamesSettingsInDiscord(delegate(ClientResult _result)
		{
			DiscordManager.logCallbackInfoWithClientResult("OpenConnectedGamesSettingsInDiscord", null, _result, true);
		});
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x00083DF8 File Offset: 0x00081FF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void localDiscordOrEntityIdChanged()
	{
		if (this.localPlayer == null)
		{
			return;
		}
		DiscordManager.DiscordUserMappingManager discordUserMappingManager = this.userMappings;
		int entityId = this.localPlayer.entityId;
		bool flag = false;
		DiscordManager.DiscordUser discordUser = this.LocalUser;
		discordUserMappingManager.UpdateMapping(entityId, flag, (discordUser != null) ? discordUser.ID : 0UL);
		DiscordManager.DiscordUser discordUser2 = this.LocalUser;
		if (discordUser2 != null)
		{
			discordUser2.UpdatePlayerName(null);
		}
		ConnectionManager connectionManager = SingletonMonoBehaviour<ConnectionManager>.Instance;
		NetPackageDiscordIdMappings package = NetPackageManager.GetPackage<NetPackageDiscordIdMappings>();
		int entityId2 = this.localPlayer.entityId;
		bool flag2 = false;
		DiscordManager.DiscordUser discordUser3 = this.LocalUser;
		connectionManager.SendToClientsOrServer(package.Setup(entityId2, flag2, (discordUser3 != null) ? discordUser3.ID : 0UL));
		if (!GameManager.IsDedicatedServer && this.IsReady)
		{
			this.globalLobby.Join(false);
			this.partyLobby.Join(false);
		}
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x00083EAC File Offset: 0x000820AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateUserNames()
	{
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num, out discordUser);
			discordUser.UpdatePlayerName(null);
		}
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x00083F0C File Offset: 0x0008210C
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetPendingOutgoingJoinRequests()
	{
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num, out discordUser);
			discordUser.PendingOutgoingJoinRequest = false;
		}
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x00083F6C File Offset: 0x0008216C
	[PublicizedFrom(EAccessModifier.Private)]
	public void refreshCachedUserHandlesAndRelationships()
	{
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num, out discordUser);
			discordUser.TryUpdateDiscordHandle();
		}
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x00083FCC File Offset: 0x000821CC
	public int GetPendingActionsCount()
	{
		int num = 0;
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num2;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num2, out discordUser);
			if (discordUser.PendingAction)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x00084034 File Offset: 0x00082234
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePendingActionsEvent()
	{
		DiscordManager.PendingActionsUpdateCallback pendingActionsUpdate = this.PendingActionsUpdate;
		if (pendingActionsUpdate == null)
		{
			return;
		}
		pendingActionsUpdate(this.GetPendingActionsCount());
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x0008404C File Offset: 0x0008224C
	public DiscordManager.DiscordUser GetUser(ulong _userId)
	{
		DiscordManager.DiscordUser discordUser;
		if (!this.knownUsers.TryGetValue(_userId, out discordUser))
		{
			discordUser = new DiscordManager.DiscordUser(this, _userId, false);
			this.knownUsers[_userId] = discordUser;
		}
		discordUser.TryUpdateDiscordHandle();
		return discordUser;
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x00084088 File Offset: 0x00082288
	public bool TryGetUserFromEntityId(int _entityId, out DiscordManager.DiscordUser _user)
	{
		ulong num;
		if (!this.userMappings.TryGetDiscordId(_entityId, out num))
		{
			_user = null;
			return false;
		}
		return this.knownUsers.TryGetValue(num, out _user);
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x000840B7 File Offset: 0x000822B7
	public bool TryGetUserFromEntity(EntityPlayer _entity, out DiscordManager.DiscordUser _user)
	{
		if (_entity == null)
		{
			_user = null;
			return false;
		}
		return this.TryGetUserFromEntityId(_entity.entityId, out _user);
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x000840D4 File Offset: 0x000822D4
	public void GetAllUsers(IList<DiscordManager.DiscordUser> _target)
	{
		this.knownUsers.CopyValuesTo<ulong, DiscordManager.DiscordUser>(_target);
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x000840E4 File Offset: 0x000822E4
	public void GetFriends(HashSet<DiscordManager.DiscordUser> _target)
	{
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num, out discordUser);
			DiscordManager.DiscordUser discordUser2 = discordUser;
			if (discordUser2.IsFriend)
			{
				_target.Add(discordUser2);
			}
		}
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x00084150 File Offset: 0x00082350
	public void GetBlockedUsers(HashSet<DiscordManager.DiscordUser> _target)
	{
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num, out discordUser);
			DiscordManager.DiscordUser discordUser2 = discordUser;
			if (discordUser2.IsBlocked)
			{
				_target.Add(discordUser2);
			}
		}
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x000841BC File Offset: 0x000823BC
	public void GetUsersWithPendingAction(HashSet<DiscordManager.DiscordUser> _target)
	{
		foreach (KeyValuePair<ulong, DiscordManager.DiscordUser> keyValuePair in this.knownUsers)
		{
			ulong num;
			DiscordManager.DiscordUser discordUser;
			keyValuePair.Deconstruct(out num, out discordUser);
			DiscordManager.DiscordUser discordUser2 = discordUser;
			if (discordUser2.PendingAction)
			{
				_target.Add(discordUser2);
			}
		}
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x00084228 File Offset: 0x00082428
	public void GetInServer(HashSet<DiscordManager.DiscordUser> _target)
	{
		this.userMappings.GetAll(delegate(int _, ulong _discordId)
		{
			if (_discordId <= 0UL)
			{
				return;
			}
			DiscordManager.DiscordUser user = this.GetUser(_discordId);
			if (user.IsLocalAccount)
			{
				return;
			}
			user.RequestAvatar();
			_target.Add(user);
		});
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x00084260 File Offset: 0x00082460
	[PublicizedFrom(EAccessModifier.Private)]
	public static void logCallbackInfo(string _message, LogType _logType = LogType.Log)
	{
		switch (_logType)
		{
		case LogType.Error:
		case LogType.Exception:
			Log.Error("[Discord][CB] " + _message);
			return;
		case LogType.Warning:
			if (DiscordManager.logLevel <= LoggingSeverity.Warning)
			{
				Log.Warning("[Discord][CB] " + _message);
			}
			return;
		}
		if (DiscordManager.logLevel <= LoggingSeverity.Info)
		{
			Log.Out("[Discord][CB] " + _message);
		}
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x000842CC File Offset: 0x000824CC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void logCallbackInfoWithClientResult(string _callbackName, string _message, ClientResult _result, bool _disposeClientResult = false)
	{
		bool flag = _result.Type() != ErrorType.None;
		LogType logType = LogType.Error;
		if (!flag)
		{
			logType = LogType.Log;
			if (DiscordManager.logLevel > LoggingSeverity.Info)
			{
				if (_disposeClientResult)
				{
					_result.Dispose();
				}
				return;
			}
		}
		string text = DiscordManager.clientResultToString(_result, _disposeClientResult);
		DiscordManager.logCallbackInfo(string.IsNullOrEmpty(_message) ? (_callbackName + " (" + text + ")") : string.Concat(new string[] { _callbackName, " (", text, "): ", _message }), logType);
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x00084348 File Offset: 0x00082548
	[PublicizedFrom(EAccessModifier.Private)]
	public static string clientResultToString(ClientResult _result, bool _dispose = false)
	{
		string text = string.Format("{0}/{1}/{2}/'{3}'", new object[]
		{
			_result.Type().ToStringCached<ErrorType>(),
			_result.ErrorCode(),
			_result.Status().ToStringCached<HttpStatusCode>(),
			_result.Error()
		});
		if (_dispose)
		{
			_result.Dispose();
		}
		return text;
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x00084421 File Offset: 0x00082621
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <registerGameStartupForInvites>g__RegisterSteamLauncher|100_0()
	{
		return this.client.RegisterLaunchSteamApplication(1296840202995896363UL, 251570U);
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x0008443C File Offset: 0x0008263C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <registerGameStartupForInvites>g__RegisterNativePcLauncher|100_1()
	{
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer)
		{
			return this.client.RegisterLaunchCommand(1296840202995896363UL, "com.company.7dLauncher");
		}
		return this.client.RegisterLaunchCommand(1296840202995896363UL, GameIO.GetLauncherExecutablePath());
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x0008448C File Offset: 0x0008268C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <registerGameChatHandling>g__DiscordButtonHoverHandler|123_0(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements, out string _tooltipText)
	{
		_tooltipText = null;
		string text;
		if (!_urlElements.TryGetValue("MessageId", out text))
		{
			Log.Warning("DiscordButton URL (" + _sourceUrl + "): No MessageId defined");
			return;
		}
		ulong num;
		if (!StringParsers.TryParseUInt64(text, out num))
		{
			Log.Warning("DiscordButton URL (" + _sourceUrl + "): Invalid MessageId");
			return;
		}
		_tooltipText = Localization.Get("xuiChatOpenMessageInDiscord", false, null);
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x000844F0 File Offset: 0x000826F0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <registerGameChatHandling>g__DiscordButtonHandler|123_1(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements)
	{
		string text;
		if (!_urlElements.TryGetValue("MessageId", out text))
		{
			Log.Warning("DiscordButton URL (" + _sourceUrl + "): No MessageId defined");
			return;
		}
		ulong messageId;
		if (!StringParsers.TryParseUInt64(text, out messageId))
		{
			Log.Warning("DiscordButton URL (" + _sourceUrl + "): Invalid MessageId");
			return;
		}
		this.client.OpenMessageInDiscord(messageId, new Discord.Sdk.Client.ProvisionalUserMergeRequiredCallback(DiscordManager.<registerGameChatHandling>g__ProvisionalUserMergeRequiredCallback|123_6), delegate(ClientResult _result)
		{
			DiscordManager.logCallbackInfoWithClientResult("OpenMessageInDiscord", messageId.ToString(), _result, true);
		});
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x00084575 File Offset: 0x00082775
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <registerGameChatHandling>g__ProvisionalUserMergeRequiredCallback|123_6()
	{
		Log.Warning("[Discord] ProvisionalUserMergeRequiredCallback fired!");
	}

	// Token: 0x06001655 RID: 5717 RVA: 0x00084584 File Offset: 0x00082784
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <registerGameChatHandling>g__SendMessage|123_2(EChatType _chatType, string _targetId, string _message)
	{
		ulong num;
		if (!ulong.TryParse(_targetId, out num))
		{
			throw new ArgumentException("Could not parse chat Discord id '" + _targetId + "'");
		}
		this.GetUser(num).MessageSentFromGame = true;
		this.client.SendUserMessage(num, _message, delegate(ClientResult _result, ulong _messageId)
		{
			DiscordManager.logCallbackInfoWithClientResult("SendUserMessage", _messageId.ToString(), _result, true);
		});
	}

	// Token: 0x06001656 RID: 5718 RVA: 0x000845EC File Offset: 0x000827EC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public string <registerGameChatHandling>g__GetTargetDisplayName|123_3(EChatType _chatType, string _targetId)
	{
		ulong num;
		if (!ulong.TryParse(_targetId, out num))
		{
			throw new ArgumentException("Could not parse chat Discord id '" + _targetId + "'");
		}
		DiscordManager.DiscordUser user = this.GetUser(num);
		return string.Format(Localization.Get("xuiChatTargetWhisper", false, null), user.DisplayName + " [discord] ");
	}

	// Token: 0x06001657 RID: 5719 RVA: 0x00084644 File Offset: 0x00082844
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <registerGameChatHandling>g__IsValidTarget|123_4(EChatType _chatType, string _targetId)
	{
		if (_targetId == null)
		{
			return false;
		}
		ulong num;
		if (!ulong.TryParse(_targetId, out num))
		{
			throw new ArgumentException("Could not parse chat Discord id '" + _targetId + "' for target validation");
		}
		ulong num2 = num;
		DiscordManager.DiscordUser discordUser = this.LocalUser;
		return num2 != ((discordUser != null) ? discordUser.ID : 0UL);
	}

	// Token: 0x04000EA2 RID: 3746
	[PublicizedFrom(EAccessModifier.Private)]
	public const ulong DiscordApplicationId = 1296840202995896363UL;

	// Token: 0x04000EA3 RID: 3747
	[PublicizedFrom(EAccessModifier.Private)]
	public const ulong DiscordClientId = 1296840202995896363UL;

	// Token: 0x04000EA4 RID: 3748
	[PublicizedFrom(EAccessModifier.Private)]
	public static DiscordManager instance;

	// Token: 0x04000EB5 RID: 3765
	public readonly DiscordManager.DiscordSettings Settings;

	// Token: 0x04000EB6 RID: 3766
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DiscordManager.DiscordUserSettingsManager UserSettings;

	// Token: 0x04000EB7 RID: 3767
	[PublicizedFrom(EAccessModifier.Private)]
	public Discord.Sdk.Client client;

	// Token: 0x04000EB8 RID: 3768
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.DiscordUser localUser;

	// Token: 0x04000EB9 RID: 3769
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DiscordManager.DiscordUserMappingManager userMappings;

	// Token: 0x04000EBA RID: 3770
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<ulong, DiscordManager.DiscordUser> knownUsers = new Dictionary<ulong, DiscordManager.DiscordUser>();

	// Token: 0x04000EBB RID: 3771
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DiscordManager.FriendsServerList friendsServerList = new DiscordManager.FriendsServerList();

	// Token: 0x04000EBC RID: 3772
	[PublicizedFrom(EAccessModifier.Private)]
	public static LoggingSeverity logLevel = LoggingSeverity.Warning;

	// Token: 0x04000EBD RID: 3773
	[PublicizedFrom(EAccessModifier.Private)]
	public static LoggingSeverity logLevelRtc = LoggingSeverity.Warning;

	// Token: 0x04000EBE RID: 3774
	public readonly DiscordManager.AuthAndLoginManager AuthManager;

	// Token: 0x04000EBF RID: 3775
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex logMessageMatcher = new Regex("^\\[([^\\]]+)\\] \\[(\\d+)\\] \\(([^)]+)\\): (.*)\\n$", RegexOptions.Compiled | RegexOptions.Singleline);

	// Token: 0x04000EC0 RID: 3776
	[PublicizedFrom(EAccessModifier.Private)]
	public const string UrlTypeDiscordMessageButton = "DiscordMessageButton";

	// Token: 0x04000EC1 RID: 3777
	[PublicizedFrom(EAccessModifier.Private)]
	public const string UrlFieldMessageId = "MessageId";

	// Token: 0x04000EC2 RID: 3778
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DiscordManager.LobbyInfo globalLobby;

	// Token: 0x04000EC3 RID: 3779
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DiscordManager.LobbyInfo partyLobby;

	// Token: 0x04000EC4 RID: 3780
	public readonly DiscordManager.PresenceManager Presence;

	// Token: 0x04000EC5 RID: 3781
	public readonly DiscordManager.AudioDeviceConfig AudioOutput;

	// Token: 0x04000EC6 RID: 3782
	public readonly DiscordManager.AudioDeviceConfig AudioInput;

	// Token: 0x04000EC7 RID: 3783
	[PublicizedFrom(EAccessModifier.Private)]
	public float nextActivityUpdate;

	// Token: 0x04000EC8 RID: 3784
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04000EC9 RID: 3785
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentFileVersion = 1;

	// Token: 0x02000306 RID: 774
	public class AuthAndLoginManager
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x0008468F File Offset: 0x0008288F
		// (set) Token: 0x06001659 RID: 5721 RVA: 0x00084697 File Offset: 0x00082897
		public DiscordManager.EDiscordAccountType IsLoggingInWith
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x000846A0 File Offset: 0x000828A0
		public bool IsLoggingIn
		{
			get
			{
				return this.IsLoggingInWith > DiscordManager.EDiscordAccountType.None;
			}
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x000846AB File Offset: 0x000828AB
		public AuthAndLoginManager(DiscordManager _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x000846BC File Offset: 0x000828BC
		public void RegisterGameEventHandlers()
		{
			ModEvents.MainMenuOpened.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SMainMenuOpenedData>(this.OnMainMenuOpened));
			ModEvents.GameStarting.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SGameStartingData>(this.OnGameStarting));
			ModEvents.PlayerSpawnedInWorld.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SPlayerSpawnedInWorldData>(this.OnPlayerSpawned));
			ModEvents.WorldShuttingDown.RegisterHandler(new ModEvents.ModEventHandlerDelegate<ModEvents.SWorldShuttingDownData>(this.OnWorldShuttingDown));
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00084724 File Offset: 0x00082924
		public void RegisterDiscordCallbacks()
		{
			this.owner.client.SetStatusChangedCallback(new Discord.Sdk.Client.OnStatusChanged(this.OnStatusChanged));
			this.owner.client.SetTokenExpirationCallback(new Discord.Sdk.Client.TokenExpirationCallback(this.OnTokenExpiration));
			this.owner.StatusChanged += this.OnDiscordStatusChanged;
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00084780 File Offset: 0x00082980
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnMainMenuOpened(ref ModEvents.SMainMenuOpenedData _data)
		{
			this.updateExternalAuthRequestHandler();
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00084780 File Offset: 0x00082980
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDiscordStatusChanged(DiscordManager.EDiscordStatus _status)
		{
			this.updateExternalAuthRequestHandler();
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00084788 File Offset: 0x00082988
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGameStarting(ref ModEvents.SGameStartingData _data)
		{
			this.playerSpawned = false;
			this.updateExternalAuthRequestHandler();
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00084797 File Offset: 0x00082997
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnPlayerSpawned(ref ModEvents.SPlayerSpawnedInWorldData _data)
		{
			this.playerSpawned = true;
			this.updateExternalAuthRequestHandler();
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00084788 File Offset: 0x00082988
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnWorldShuttingDown(ref ModEvents.SWorldShuttingDownData _data)
		{
			this.playerSpawned = false;
			this.updateExternalAuthRequestHandler();
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x000847A8 File Offset: 0x000829A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateExternalAuthRequestHandler()
		{
			if (GameManager.IsDedicatedServer || !this.owner.IsInitialized)
			{
				return;
			}
			bool flag = this.owner.Status == DiscordManager.EDiscordStatus.Ready;
			bool isStartingGame = GameManager.Instance.IsStartingGame;
			bool flag2 = GameManager.Instance.GetGameStateManager().IsGameStarted();
			if (DiscordManager.LogLevel < LoggingSeverity.Warning)
			{
				Log.Out(string.Format("[Discord] UpdateExternalAuthRequestHandler: Valid={0}, StartingGame={1}, InGame={2}, Spawned={3}", new object[] { flag, isStartingGame, flag2, this.playerSpawned }));
			}
			if (flag && ((!flag2 && !isStartingGame) || this.playerSpawned))
			{
				DiscordManager discordManager = this.owner;
				if (discordManager == null)
				{
					return;
				}
				Discord.Sdk.Client client = discordManager.client;
				if (client == null)
				{
					return;
				}
				client.RegisterAuthorizeRequestCallback(new Discord.Sdk.Client.AuthorizeRequestCallback(this.OnExternalAuthorizeRequest));
				return;
			}
			else
			{
				DiscordManager discordManager2 = this.owner;
				if (discordManager2 == null)
				{
					return;
				}
				Discord.Sdk.Client client2 = discordManager2.client;
				if (client2 == null)
				{
					return;
				}
				client2.RemoveAuthorizeRequestCallback();
				return;
			}
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0008488C File Offset: 0x00082A8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnExternalAuthorizeRequest()
		{
			Log.Out("[Discord] AuthorizeRequest from Discord client");
			string returnWindow = null;
			if (!GameManager.Instance.GetGameStateManager().IsGameStarted())
			{
				returnWindow = LocalPlayerUI.primaryUI.windowManager.GetModalWindow().Id;
			}
			XUiC_DiscordLogin.Open(delegate
			{
				if (string.IsNullOrEmpty(returnWindow))
				{
					return;
				}
				LocalPlayerUI.primaryUI.windowManager.Open(returnWindow, true);
			}, false, false, false, true, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
			this.LoginDiscordUser();
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x000848FD File Offset: 0x00082AFD
		public void LoginWithPlatformDefaultAccountType()
		{
			if (DiscordManager.SupportsProvisionalAccounts)
			{
				this.LoginProvisionalAccount();
				return;
			}
			this.LoginDiscordUser();
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00084914 File Offset: 0x00082B14
		public void AutoLogin()
		{
			switch (this.owner.Settings.LastAccountType)
			{
			case DiscordManager.EDiscordAccountType.None:
				this.LoginWithPlatformDefaultAccountType();
				return;
			case DiscordManager.EDiscordAccountType.Regular:
				this.LoginDiscordUser();
				return;
			case DiscordManager.EDiscordAccountType.Provisional:
				this.LoginProvisionalAccount();
				return;
			default:
				throw new ArgumentOutOfRangeException("LastAccountType");
			}
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00084965 File Offset: 0x00082B65
		[PublicizedFrom(EAccessModifier.Private)]
		public void prepareLoginStart(DiscordManager.EDiscordAccountType _accountType)
		{
			this.IsLoggingInWith = _accountType;
			this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.None;
			this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.None;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0008497C File Offset: 0x00082B7C
		public void AbortAuth()
		{
			if (!this.inAuthProcess)
			{
				return;
			}
			if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
			{
				this.owner.client.AbortGetTokenFromDevice();
			}
			else
			{
				this.owner.client.AbortAuthorize();
			}
			this.inAuthProcess = false;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000849BC File Offset: 0x00082BBC
		public void LoginDiscordUser()
		{
			this.owner.Init(false);
			if (this.owner.IsReady && !this.owner.LocalUser.IsProvisionalAccount)
			{
				Log.Out("[Discord] Already logged in");
				return;
			}
			this.prepareLoginStart(DiscordManager.EDiscordAccountType.Regular);
			if (!string.IsNullOrEmpty(this.owner.Settings.AccessToken))
			{
				this.loginWithStoredTokens();
				return;
			}
			if (!string.IsNullOrEmpty(this.owner.Settings.RefreshToken))
			{
				this.refreshToken();
				return;
			}
			Log.Out("[Discord] Logging in with Discord user");
			if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
			{
				this.loginDiscordUserConsole();
				return;
			}
			this.loginDiscordUserPC();
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00084A62 File Offset: 0x00082C62
		[PublicizedFrom(EAccessModifier.Private)]
		public void loginWithStoredTokens()
		{
			Log.Out("[Discord] Logging in with existing access token");
			this.owner.client.UpdateToken(AuthorizationTokenType.Bearer, this.owner.Settings.AccessToken, new Discord.Sdk.Client.UpdateTokenCallback(this.updateTokenCallback));
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00084A9C File Offset: 0x00082C9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void loginDiscordUserPC()
		{
			AuthorizationCodeVerifier codeVerifier = this.owner.client.CreateAuthorizationCodeVerifier();
			AuthorizationArgs authorizationArgs = new AuthorizationArgs();
			authorizationArgs.SetClientId(1296840202995896363UL);
			authorizationArgs.SetScopes(Discord.Sdk.Client.GetDefaultCommunicationScopes());
			authorizationArgs.SetCodeChallenge(codeVerifier.Challenge());
			DiscordManager.UserAuthorizationResultCallback userAuthorizationResult = this.owner.UserAuthorizationResult;
			if (userAuthorizationResult != null)
			{
				userAuthorizationResult(false, DiscordManager.EFullAccountLoginResult.RequestingAuth, DiscordManager.EProvisionalAccountLoginResult.None, false);
			}
			this.inAuthProcess = true;
			this.owner.client.Authorize(authorizationArgs, delegate(ClientResult _result, string _code, string _uri)
			{
				this.inAuthProcess = false;
				ErrorType errorType = _result.Type();
				int num = _result.ErrorCode();
				DiscordManager.logCallbackInfoWithClientResult("Authorize (PC)", string.Concat(new string[] { "code='", _code, "' uri='", _uri, "'" }), _result, true);
				if (this.owner.client == null)
				{
					return;
				}
				if (errorType != ErrorType.None)
				{
					if (errorType == ErrorType.Aborted)
					{
						this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.AuthCancelled;
						Log.Out("[Discord] Auth aborted");
					}
					else if (num == 5000)
					{
						this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.AuthCancelled;
						Log.Out("[Discord] Auth cancelled");
					}
					else
					{
						this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.AuthFailed;
						Log.Out("[Discord] Auth failed");
					}
					DiscordManager.UserAuthorizationResultCallback userAuthorizationResult2 = this.owner.UserAuthorizationResult;
					if (userAuthorizationResult2 != null)
					{
						userAuthorizationResult2(false, this.fullAccountLoginResult, DiscordManager.EProvisionalAccountLoginResult.None, false);
					}
					this.loginProvisionalAccountInternal(false);
					return;
				}
				DiscordManager.UserAuthorizationResultCallback userAuthorizationResult3 = this.owner.UserAuthorizationResult;
				if (userAuthorizationResult3 != null)
				{
					userAuthorizationResult3(false, DiscordManager.EFullAccountLoginResult.AuthAccepted, DiscordManager.EProvisionalAccountLoginResult.None, false);
				}
				if (!DiscordManager.SupportsProvisionalAccounts)
				{
					this.owner.client.GetToken(1296840202995896363UL, _code, codeVerifier.Verifier(), _uri, new Discord.Sdk.Client.TokenExchangeCallback(this.tokenExchangeCallbackFullAccount));
					return;
				}
				string authTicket = PlatformManager.CrossplatformPlatform.AuthenticationClient.GetAuthTicket();
				if (string.IsNullOrEmpty(authTicket))
				{
					Log.Error("[Discord] Logging in with merged account failed, could not fetch EOS IdToken");
					this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.PlatformError;
					this.invokeAuthResultCallback(false);
					return;
				}
				this.owner.client.GetTokenFromProvisionalMerge(1296840202995896363UL, _code, codeVerifier.Verifier(), _uri, AuthenticationExternalAuthType.EpicOnlineServicesIdToken, authTicket, new Discord.Sdk.Client.TokenExchangeCallback(this.tokenExchangeCallbackFullAccount));
			});
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00084B3C File Offset: 0x00082D3C
		[PublicizedFrom(EAccessModifier.Private)]
		public void loginDiscordUserConsole()
		{
			DeviceAuthorizationArgs deviceAuthorizationArgs = new DeviceAuthorizationArgs();
			deviceAuthorizationArgs.SetClientId(1296840202995896363UL);
			deviceAuthorizationArgs.SetScopes(Discord.Sdk.Client.GetDefaultCommunicationScopes());
			string authTicket = PlatformManager.CrossplatformPlatform.AuthenticationClient.GetAuthTicket();
			if (string.IsNullOrEmpty(authTicket))
			{
				Log.Error("[Discord] Logging in with merged account failed, could not fetch EOS IdToken");
				this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.PlatformError;
				this.invokeAuthResultCallback(false);
				return;
			}
			this.inAuthProcess = true;
			this.owner.client.GetTokenFromDeviceProvisionalMerge(deviceAuthorizationArgs, AuthenticationExternalAuthType.EpicOnlineServicesIdToken, authTicket, new Discord.Sdk.Client.TokenExchangeCallback(this.tokenExchangeCallbackFullAccount));
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00084BC4 File Offset: 0x00082DC4
		[PublicizedFrom(EAccessModifier.Private)]
		public void tokenExchangeCallbackFullAccount(ClientResult _result, string _accessToken, string _refreshToken, AuthorizationTokenType _tokenType, int _expiresIn, string _scope)
		{
			this.inAuthProcess = false;
			DiscordManager.logCallbackInfoWithClientResult("OnTokenExchange (Full)", string.Format("tokenType={0}, expires={1}, scope='{2}'", _tokenType.ToStringCached<AuthorizationTokenType>(), _expiresIn, _scope), _result, true);
			if (_accessToken != "")
			{
				this.owner.Settings.AccessToken = _accessToken;
				this.owner.Settings.RefreshToken = _refreshToken;
				this.owner.client.UpdateToken(AuthorizationTokenType.Bearer, _accessToken, new Discord.Sdk.Client.UpdateTokenCallback(this.updateTokenCallback));
				return;
			}
			Log.Warning("[Discord] Failed retrieving account token!");
			this.owner.Settings.LastAccountType = DiscordManager.EDiscordAccountType.None;
			this.owner.Settings.AccessToken = null;
			this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.TokenExchangeFailed;
			this.loginProvisionalAccountInternal(false);
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00084C88 File Offset: 0x00082E88
		[PublicizedFrom(EAccessModifier.Private)]
		public void refreshToken()
		{
			string refreshToken = this.owner.Settings.RefreshToken;
			if (string.IsNullOrEmpty(refreshToken))
			{
				Log.Warning("[Discord] No refresh token");
				return;
			}
			Log.Out("[Discord] Trying to refresh access token");
			this.owner.Settings.RefreshToken = null;
			this.owner.client.RefreshToken(1296840202995896363UL, refreshToken, new Discord.Sdk.Client.TokenExchangeCallback(this.<refreshToken>g__RefreshTokenExchangeCallback|29_0));
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00084CFC File Offset: 0x00082EFC
		public void UnmergeAccount()
		{
			this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.None;
			this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.None;
			this.IsLoggingInWith = DiscordManager.EDiscordAccountType.Provisional;
			if (!DiscordManager.SupportsProvisionalAccounts)
			{
				Log.Error("[Discord] Unmerging account only available when running with EOS");
				return;
			}
			string authTicket = PlatformManager.CrossplatformPlatform.AuthenticationClient.GetAuthTicket();
			if (string.IsNullOrEmpty(authTicket))
			{
				Log.Error("[Discord] Unmerging account failed, could not fetch EOS IdToken");
				return;
			}
			this.owner.client.UnmergeIntoProvisionalAccount(1296840202995896363UL, AuthenticationExternalAuthType.EpicOnlineServicesIdToken, authTicket, delegate(ClientResult _result)
			{
				bool flag = _result.Type() != ErrorType.None;
				DiscordManager.logCallbackInfoWithClientResult("UnmergeIntoProvisionalAccount", null, _result, true);
				if (flag)
				{
					this.invokeAuthResultCallback(false);
					return;
				}
				this.LoginProvisionalAccount();
			});
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00084D7A File Offset: 0x00082F7A
		public void LoginProvisionalAccount()
		{
			this.owner.Init(false);
			this.prepareLoginStart(DiscordManager.EDiscordAccountType.Provisional);
			this.loginProvisionalAccountInternal(false);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00084D98 File Offset: 0x00082F98
		[PublicizedFrom(EAccessModifier.Private)]
		public void loginProvisionalAccountInternal(bool _isRefresh)
		{
			if (!DiscordManager.SupportsProvisionalAccounts)
			{
				Log.Error("[Discord] Provisional account login only available when running with EOS");
				this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.NotSupported;
				this.invokeAuthResultCallback(false);
				return;
			}
			if (this.owner.IsReady && !_isRefresh)
			{
				Log.Out("[Discord] Already logged in");
				this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.Success;
				this.IsLoggingInWith = DiscordManager.EDiscordAccountType.Provisional;
				this.invokeAuthResultCallback(true);
				return;
			}
			if (PlatformManager.CrossplatformPlatform.User.UserStatus != EUserStatus.LoggedIn)
			{
				Log.Out("[Discord] Can not log in with provisional account, not logged in to cross platform provider");
				this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.PlatformError;
				this.invokeAuthResultCallback(false);
				return;
			}
			Log.Out(_isRefresh ? "[Discord] Refreshing provisional account token" : "[Discord] Logging in with provisional account");
			string authTicket = PlatformManager.CrossplatformPlatform.AuthenticationClient.GetAuthTicket();
			if (string.IsNullOrEmpty(authTicket))
			{
				Log.Error("[Discord] Logging in with provisional account failed, could not fetch EOS IdToken");
				this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.PlatformError;
				this.invokeAuthResultCallback(false);
				return;
			}
			this.owner.client.GetProvisionalToken(1296840202995896363UL, AuthenticationExternalAuthType.EpicOnlineServicesIdToken, authTicket, new Discord.Sdk.Client.TokenExchangeCallback(this.<loginProvisionalAccountInternal>g__TokenExchangeCallbackProvisional|33_0));
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00084E8B File Offset: 0x0008308B
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateTokenCallback(ClientResult _result)
		{
			bool flag = _result.Type() != ErrorType.None;
			DiscordManager.logCallbackInfoWithClientResult("UpdateToken", null, _result, true);
			if (flag)
			{
				Log.Error("[Discord] UpdateToken failed!");
				return;
			}
			this.owner.client.Connect();
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00084EC0 File Offset: 0x000830C0
		public void Disconnect()
		{
			this.IsLoggingInWith = DiscordManager.EDiscordAccountType.None;
			if (this.owner.client == null)
			{
				return;
			}
			this.owner.leaveLobbies(true);
			if (this.owner.Status != DiscordManager.EDiscordStatus.Disconnected)
			{
				this.owner.client.Disconnect();
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00084F0C File Offset: 0x0008310C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnStatusChanged(Discord.Sdk.Client.Status _status, Discord.Sdk.Client.Error _error, int _errorDetail)
		{
			DiscordManager.logCallbackInfo(string.Format("OnStatusChanged status={0} error={1} errorDetail={2}", _status, _error, _errorDetail), (_error == Discord.Sdk.Client.Error.None) ? LogType.Log : LogType.Error);
			if (_status == Discord.Sdk.Client.Status.Disconnected && _error == Discord.Sdk.Client.Error.ConnectionFailed)
			{
				if (this.IsLoggingInWith == DiscordManager.EDiscordAccountType.Regular)
				{
					this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.ConnectionFailed;
				}
				else
				{
					this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.ConnectionFailed;
				}
				this.invokeAuthResultCallback(false);
			}
			if (_errorDetail == 4004)
			{
				if (!string.IsNullOrEmpty(this.owner.Settings.RefreshToken))
				{
					this.refreshToken();
					return;
				}
				this.loginProvisionalAccountInternal(false);
				return;
			}
			else
			{
				if (_errorDetail == 4003)
				{
					DiscordManager.DiscordUser localUser = this.owner.LocalUser;
					if (localUser == null || !localUser.IsProvisionalAccount)
					{
						this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.TokenRevoked;
						this.owner.Settings.AccessToken = null;
						this.owner.Settings.RefreshToken = null;
						this.loginProvisionalAccountInternal(false);
						return;
					}
				}
				switch (_status)
				{
				case Discord.Sdk.Client.Status.Disconnected:
				case Discord.Sdk.Client.Status.Disconnecting:
					this.IsLoggingInWith = DiscordManager.EDiscordAccountType.None;
					this.owner.leaveLobbies(false);
					if (this.owner.LocalUser != null)
					{
						this.owner.LocalUser.Dispose();
						this.owner.LocalUser = null;
					}
					this.owner.clearFriends();
					break;
				case Discord.Sdk.Client.Status.Connecting:
				case Discord.Sdk.Client.Status.Connected:
				case Discord.Sdk.Client.Status.Reconnecting:
				case Discord.Sdk.Client.Status.HttpWait:
					break;
				case Discord.Sdk.Client.Status.Ready:
				{
					UserHandle currentUserV = this.owner.client.GetCurrentUserV2();
					if (currentUserV == null)
					{
						Log.Error("[Discord] StatusChanged=Ready, but could not fetch CurrentUser");
						if (this.IsLoggingInWith == DiscordManager.EDiscordAccountType.Regular)
						{
							this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.PlatformError;
						}
						else
						{
							this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.PlatformError;
						}
						this.invokeAuthResultCallback(false);
						return;
					}
					bool flag = currentUserV.IsProvisional();
					this.owner.Settings.LastAccountType = (flag ? DiscordManager.EDiscordAccountType.Provisional : DiscordManager.EDiscordAccountType.Regular);
					ulong num = currentUserV.Id();
					bool flag2 = this.owner.LocalUser == null || this.owner.LocalUser.ID != num;
					if (flag2)
					{
						DiscordManager.DiscordUser localUser2 = this.owner.LocalUser;
						if (localUser2 != null)
						{
							localUser2.Dispose();
						}
						this.owner.LocalUser = new DiscordManager.DiscordUser(this.owner, num, true);
						this.owner.knownUsers[num] = this.owner.LocalUser;
					}
					this.owner.knownUsers[this.owner.LocalUser.ID] = this.owner.LocalUser;
					if (flag)
					{
						this.owner.client.UpdateProvisionalAccountDisplayName(GamePrefs.GetString(EnumGamePrefs.PlayerName), delegate(ClientResult _result)
						{
							DiscordManager.logCallbackInfoWithClientResult("UpdateProvisionalAccountDisplayName", null, _result, true);
						});
					}
					this.owner.getFriends();
					this.owner.Presence.SetRichPresenceState(null);
					this.owner.Settings.Save();
					if (!flag)
					{
						this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.Success;
					}
					else
					{
						this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.Success;
					}
					if (flag2)
					{
						Log.Out("[Discord] Logged in");
						this.invokeAuthResultCallback(true);
					}
					break;
				}
				default:
					throw new ArgumentOutOfRangeException("_status", _status, null);
				}
				Action<DiscordManager.EDiscordStatus> statusChanged = this.owner.StatusChanged;
				if (statusChanged == null)
				{
					return;
				}
				statusChanged(this.owner.Status);
				return;
			}
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00085224 File Offset: 0x00083424
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnTokenExpiration()
		{
			DiscordManager.logCallbackInfo("OnTokenExpiration", LogType.Log);
			if (this.owner.LocalUser == null)
			{
				Log.Out("[Discord] Received token expiration without a logged in user.");
				return;
			}
			if (this.owner.LocalUser.IsProvisionalAccount)
			{
				this.loginProvisionalAccountInternal(true);
				return;
			}
			this.refreshToken();
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00085274 File Offset: 0x00083474
		[PublicizedFrom(EAccessModifier.Private)]
		public void invokeAuthResultCallback(bool _success)
		{
			bool flag = this.fullAccountLoginResult == DiscordManager.EFullAccountLoginResult.Success || (this.fullAccountLoginResult == DiscordManager.EFullAccountLoginResult.None && this.provisionalAccountLoginResult == DiscordManager.EProvisionalAccountLoginResult.Success);
			DiscordManager.UserAuthorizationResultCallback userAuthorizationResult = this.owner.UserAuthorizationResult;
			if (userAuthorizationResult != null)
			{
				userAuthorizationResult(true, this.fullAccountLoginResult, this.provisionalAccountLoginResult, flag);
			}
			if (!_success)
			{
				this.IsLoggingInWith = DiscordManager.EDiscordAccountType.None;
			}
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000852D0 File Offset: 0x000834D0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <refreshToken>g__RefreshTokenExchangeCallback|29_0(ClientResult _result, string _accessToken, string _refreshToken, AuthorizationTokenType _tokenType, int _expiresIn, string _scope)
		{
			DiscordManager.logCallbackInfoWithClientResult("OnTokenExchange (Refresh)", string.Format("tokenType={0}, expires={1}, scope='{2}'", _tokenType.ToStringCached<AuthorizationTokenType>(), _expiresIn, _scope), _result, true);
			if (_accessToken != "")
			{
				this.owner.Settings.AccessToken = _accessToken;
				this.owner.Settings.RefreshToken = _refreshToken;
				this.owner.client.UpdateToken(AuthorizationTokenType.Bearer, _accessToken, new Discord.Sdk.Client.UpdateTokenCallback(this.updateTokenCallback));
				return;
			}
			Log.Warning("[Discord] Failed refreshing token!");
			this.owner.Settings.LastAccountType = DiscordManager.EDiscordAccountType.None;
			this.owner.Settings.AccessToken = null;
			this.fullAccountLoginResult = DiscordManager.EFullAccountLoginResult.TokenRefreshFailed;
			this.loginProvisionalAccountInternal(false);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x000853B0 File Offset: 0x000835B0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public void <loginProvisionalAccountInternal>g__TokenExchangeCallbackProvisional|33_0(ClientResult _result, string _accessToken, string _refreshToken, AuthorizationTokenType _tokenType, int _expiresIn, string _scope)
		{
			if (_result.ErrorCode() == 530010)
			{
				this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.PlatformIdLinkedToDiscordAccount;
				Log.Out("[Discord] Can not login with provisional account, platform ID already linked to a Discord account");
				this.invokeAuthResultCallback(false);
				_result.Dispose();
				return;
			}
			DiscordManager.logCallbackInfoWithClientResult("OnTokenExchange (Provisional)", string.Format("tokenType={0}, expires={1}, scope='{2}'", _tokenType.ToStringCached<AuthorizationTokenType>(), _expiresIn, _scope), _result, true);
			if (_accessToken != "")
			{
				this.owner.client.UpdateToken(AuthorizationTokenType.Bearer, _accessToken, new Discord.Sdk.Client.UpdateTokenCallback(this.updateTokenCallback));
				return;
			}
			Log.Warning("[Discord] Failed retrieving provisional token!");
			this.provisionalAccountLoginResult = DiscordManager.EProvisionalAccountLoginResult.TokenExchangeFailed;
			this.invokeAuthResultCallback(false);
		}

		// Token: 0x04000ECA RID: 3786
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager owner;

		// Token: 0x04000ECC RID: 3788
		[PublicizedFrom(EAccessModifier.Private)]
		public bool playerSpawned;

		// Token: 0x04000ECD RID: 3789
		[PublicizedFrom(EAccessModifier.Private)]
		public DiscordManager.EFullAccountLoginResult fullAccountLoginResult;

		// Token: 0x04000ECE RID: 3790
		[PublicizedFrom(EAccessModifier.Private)]
		public bool inAuthProcess;

		// Token: 0x04000ECF RID: 3791
		[PublicizedFrom(EAccessModifier.Private)]
		public DiscordManager.EProvisionalAccountLoginResult provisionalAccountLoginResult;
	}

	// Token: 0x0200030A RID: 778
	public class CallInfo
	{
		// Token: 0x06001681 RID: 5761 RVA: 0x00085670 File Offset: 0x00083870
		public static void LoadSounds()
		{
			LoadManager.LoadAsset<AudioClip>("@:Sounds/UI/ui_discord_join.wav", delegate(AudioClip _o)
			{
				DiscordManager.CallInfo.soundOtherJoin = _o;
			}, null, false, false, false);
			LoadManager.LoadAsset<AudioClip>("@:Sounds/UI/ui_discord_leave.wav", delegate(AudioClip _o)
			{
				DiscordManager.CallInfo.soundOtherLeave = _o;
			}, null, false, false, false);
			LoadManager.LoadAsset<AudioClip>("@:Sounds/UI/ui_discord_kicked.wav", delegate(AudioClip _o)
			{
				DiscordManager.CallInfo.soundSelfNoManualLeave = _o;
			}, null, false, false, false);
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00085707 File Offset: 0x00083907
		// (set) Token: 0x06001683 RID: 5763 RVA: 0x0008570F File Offset: 0x0008390F
		public Call.Status Status
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001684 RID: 5764 RVA: 0x00085718 File Offset: 0x00083918
		public bool IsJoined
		{
			get
			{
				return this.call != null && this.Status == Call.Status.Connected;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x0008572D File Offset: 0x0008392D
		public Call Call
		{
			get
			{
				return this.call;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x00085735 File Offset: 0x00083935
		// (set) Token: 0x06001687 RID: 5767 RVA: 0x0008573D File Offset: 0x0008393D
		public bool IsSpeaking
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00085748 File Offset: 0x00083948
		public CallInfo(DiscordManager.LobbyInfo _ownerLobby, DiscordManager _ownerManager)
		{
			this.ownerLobby = _ownerLobby;
			this.ownerManager = _ownerManager;
			this.ownerManager.Settings.VoiceVadModeChanged += this.OnVadModeChanged;
			this.ownerManager.Settings.VoiceVadThresholdChanged += this.OnVadThresholdChanged;
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x000857B8 File Offset: 0x000839B8
		public void Join()
		{
			if (!this.ownerLobby.IsJoined)
			{
				Log.Error("[Discord] Failed to start call for lobby, lobby not entered yet");
				return;
			}
			this.startedJoining = true;
			this.manualLeave = false;
			this.Status = Call.Status.Joining;
			DiscordManager.CallStatusChangedCallback callStatusChanged = this.ownerManager.CallStatusChanged;
			if (callStatusChanged != null)
			{
				callStatusChanged(this, this.Status);
			}
			this.call = this.ownerManager.client.StartCall(this.ownerLobby.Id);
			if (this.call == null)
			{
				Log.Error(string.Format("[Discord] Failed to start call for lobby {0}", this.ownerLobby.Id));
				this.Leave(false);
				return;
			}
			this.SetPushToTalkMode();
			this.call.SetPTTReleaseDelay(200U);
			this.call.SetVADThreshold(this.ownerManager.Settings.VoiceVadModeAuto, (float)this.ownerManager.Settings.VoiceVadThreshold);
			this.call.SetStatusChangedCallback(new Call.OnStatusChanged(this.OnCallStatusChanged));
			this.call.SetParticipantChangedCallback(new Call.OnParticipantChanged(this.OnParticipantChanged));
			this.call.SetOnVoiceStateChangedCallback(new Call.OnVoiceStateChanged(this.OnVoiceStateChanged));
			this.call.SetSpeakingStatusChangedCallback(new Call.OnSpeakingStatusChanged(this.OnSpeakingStatusChanged));
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00085900 File Offset: 0x00083B00
		public void Leave(bool _manual)
		{
			this.startedJoining = false;
			this.manualLeave = _manual;
			this.Status = Call.Status.Disconnected;
			this.IsSpeaking = false;
			this.ownerManager.client.EndCall(this.ownerLobby.Id, delegate
			{
			});
			Call call = this.call;
			if (call != null)
			{
				call.Dispose();
			}
			this.call = null;
			this.UpdateMembers(false);
			DiscordManager.CallChangedCallback callChanged = this.ownerManager.CallChanged;
			if (callChanged == null)
			{
				return;
			}
			callChanged(null);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00085998 File Offset: 0x00083B98
		public void SetPushToTalkMode()
		{
			this.call.SetAudioMode(this.ownerManager.Settings.VoiceModePtt ? AudioModeType.MODE_PTT : AudioModeType.MODE_VAD);
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x000859BB File Offset: 0x00083BBB
		public void SetPushToTalkActive(bool _pushToTalkPressed)
		{
			this.call.SetPTTActive(_pushToTalkPressed);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000859C9 File Offset: 0x00083BC9
		public float GetParticipantVolume(DiscordManager.DiscordUser _user)
		{
			Call call = this.call;
			if (call == null)
			{
				return 0f;
			}
			return call.GetParticipantVolume(_user.ID);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x000859E6 File Offset: 0x00083BE6
		public void SetParticipantVolume(DiscordManager.DiscordUser _user, float _volume)
		{
			Call call = this.call;
			if (call == null)
			{
				return;
			}
			call.SetParticipantVolume(_user.ID, _volume);
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00085A00 File Offset: 0x00083C00
		public void UpdateMembers(bool _applySavedVolume = false)
		{
			if (!this.IsJoined)
			{
				this.callMembersCurrent.Clear();
				this.callMembersOld.Clear();
			}
			else
			{
				Dictionary<ulong, DiscordManager.CallInfo.MemberState> dictionary = this.callMembersCurrent;
				Dictionary<ulong, DiscordManager.CallInfo.MemberState> dictionary2 = this.callMembersOld;
				this.callMembersOld = dictionary;
				this.callMembersCurrent = dictionary2;
				this.callMembersCurrent.Clear();
				foreach (ulong num in this.call.GetParticipants())
				{
					DiscordManager.DiscordUser user = this.ownerManager.GetUser(num);
					using (VoiceStateHandle voiceStateHandle = this.call.GetVoiceStateHandle(num))
					{
						DiscordManager.CallInfo.MemberState memberState;
						if (!this.callMembersOld.TryGetValue(num, out memberState))
						{
							memberState = default(DiscordManager.CallInfo.MemberState);
						}
						memberState.Speaking = false;
						memberState.Muted = voiceStateHandle.SelfMute();
						memberState.Deafened = voiceStateHandle.SelfDeaf();
						this.callMembersCurrent.Add(num, memberState);
						if (_applySavedVolume)
						{
							user.Volume = this.ownerManager.UserSettings.GetUserVolume(user.ID);
						}
					}
				}
				this.callMembersOld.Clear();
			}
			DiscordManager.CallMembersChangedCallback callMembersChanged = this.ownerManager.CallMembersChanged;
			if (callMembersChanged == null)
			{
				return;
			}
			callMembersChanged(this);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00085B48 File Offset: 0x00083D48
		public bool TryGetMember(ulong _userId, out DiscordManager.CallInfo.MemberState _state)
		{
			return this.callMembersCurrent.TryGetValue(_userId, out _state);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00085B57 File Offset: 0x00083D57
		public void GetMembers(List<ulong> _target)
		{
			this.callMembersCurrent.CopyKeysTo<ulong, DiscordManager.CallInfo.MemberState>(_target);
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00085B68 File Offset: 0x00083D68
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnCallStatusChanged(Call.Status _status, Call.Error _error, int _errorDetail)
		{
			DiscordManager.logCallbackInfo(string.Format("Call: Status changed: status={0} error={1} errorDetail={2}", _status.ToStringCached<Call.Status>(), _error.ToStringCached<Call.Error>(), _errorDetail), (_error == Call.Error.None) ? LogType.Log : LogType.Error);
			if (this.startedJoining)
			{
				this.Status = _status;
			}
			switch (_status)
			{
			case Call.Status.Disconnected:
			case Call.Status.Disconnecting:
				if (!this.manualLeave)
				{
					Manager.PlayXUiSound(DiscordManager.CallInfo.soundSelfNoManualLeave, 1f);
				}
				this.IsSpeaking = false;
				if (this.startedJoining)
				{
					this.Leave(false);
				}
				break;
			case Call.Status.Joining:
			case Call.Status.Connecting:
			case Call.Status.SignalingConnected:
			case Call.Status.Reconnecting:
				break;
			case Call.Status.Connected:
			{
				if (DiscordManager.logLevel == LoggingSeverity.Verbose)
				{
					float inputVolume = this.ownerManager.client.GetInputVolume();
					AudioModeType audioMode = this.call.GetAudioMode();
					VADThresholdSettings vadthreshold = this.call.GetVADThreshold();
					Log.Out(string.Format("[Discord] AUDIO SETTINGS: InputVolume={0}, AudioMode={1}, VAD auto={2}, threshold={3}", new object[]
					{
						inputVolume,
						audioMode.ToStringCached<AudioModeType>(),
						vadthreshold.Automatic(),
						vadthreshold.VadThreshold()
					}));
				}
				this.UpdateMembers(true);
				DiscordManager.CallChangedCallback callChanged = this.ownerManager.CallChanged;
				if (callChanged != null)
				{
					callChanged(this);
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("_status", _status, null);
			}
			DiscordManager.CallStatusChangedCallback callStatusChanged = this.ownerManager.CallStatusChanged;
			if (callStatusChanged == null)
			{
				return;
			}
			callStatusChanged(this, this.Status);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x00085CC4 File Offset: 0x00083EC4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnSpeakingStatusChanged(ulong _userId, bool _isPlayingSound)
		{
			DiscordManager.DiscordUser localUser = this.ownerManager.LocalUser;
			ulong? num = ((localUser != null) ? new ulong?(localUser.ID) : null);
			bool flag = (_userId == num.GetValueOrDefault()) & (num != null);
			DiscordManager.logCallbackInfo(string.Format("Call: Speaking state changed: user={0}({1}) isPlaying={2}", _userId, flag ? "SELF" : "other", _isPlayingSound), LogType.Log);
			if (flag)
			{
				this.IsSpeaking = _isPlayingSound;
			}
			DiscordManager.CallInfo.MemberState memberState;
			if (!this.callMembersCurrent.TryGetValue(_userId, out memberState))
			{
				Log.Warning(string.Format("[Discord] Speaking status changed for user not in call member list ({0})", _userId));
				return;
			}
			memberState.Speaking = _isPlayingSound;
			this.callMembersCurrent[_userId] = memberState;
			DiscordManager.VoiceStateChangedCallback voiceStateChanged = this.ownerManager.VoiceStateChanged;
			if (voiceStateChanged == null)
			{
				return;
			}
			voiceStateChanged(flag, _userId);
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00085D90 File Offset: 0x00083F90
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnParticipantChanged(ulong _userId, bool _added)
		{
			DiscordManager.DiscordUser localUser = this.ownerManager.LocalUser;
			ulong? num = ((localUser != null) ? new ulong?(localUser.ID) : null);
			bool flag = (_userId == num.GetValueOrDefault()) & (num != null);
			DiscordManager.logCallbackInfo(string.Format("Call: Participant changed: user={0}({1}) added={2}", _userId, flag ? "SELF" : "other", _added), LogType.Log);
			this.UpdateMembers(false);
			if (_added)
			{
				DiscordManager.DiscordUser user = this.ownerManager.GetUser(_userId);
				int num2;
				if (!this.ownerManager.userMappings.TryGetEntityId(_userId, out num2))
				{
					return;
				}
				PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
				PersistentPlayerData persistentPlayerData = ((persistentPlayers != null) ? persistentPlayers.GetPlayerDataFromEntityID(num2) : null);
				if (persistentPlayerData == null)
				{
					return;
				}
				if (!flag)
				{
					IPlatformUserData orCreate = PlatformUserManager.GetOrCreate(persistentPlayerData.PrimaryId);
					this.UpdateBlockState(user, orCreate.Blocked[EBlockType.VoiceChat].IsBlocked());
					user.Volume = this.ownerManager.UserSettings.GetUserVolume(user.ID);
				}
			}
			if (!flag)
			{
				Manager.PlayXUiSound(_added ? DiscordManager.CallInfo.soundOtherJoin : DiscordManager.CallInfo.soundOtherLeave, 1f);
			}
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00085EB4 File Offset: 0x000840B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnVoiceStateChanged(ulong _userId)
		{
			DiscordManager.DiscordUser localUser = this.ownerManager.LocalUser;
			ulong? num = ((localUser != null) ? new ulong?(localUser.ID) : null);
			bool flag = (_userId == num.GetValueOrDefault()) & (num != null);
			DiscordManager.logCallbackInfo(string.Format("Call: Voice state changed: user={0}({1})", _userId, flag ? "SELF" : "other"), LogType.Log);
			DiscordManager.CallInfo.MemberState memberState;
			if (!this.callMembersCurrent.TryGetValue(_userId, out memberState))
			{
				Log.Warning(string.Format("[Discord] VoiceState changed for user not in call member list ({0})", _userId));
				return;
			}
			using (VoiceStateHandle voiceStateHandle = this.call.GetVoiceStateHandle(_userId))
			{
				if (voiceStateHandle == null)
				{
					Log.Warning(string.Format("[Discord] VoiceState changed for user {0} but can not get current state", _userId));
				}
				else
				{
					memberState.Muted = voiceStateHandle.SelfMute();
					memberState.Deafened = voiceStateHandle.SelfDeaf();
					this.callMembersCurrent[_userId] = memberState;
					DiscordManager.VoiceStateChangedCallback voiceStateChanged = this.ownerManager.VoiceStateChanged;
					if (voiceStateChanged != null)
					{
						voiceStateChanged(flag, _userId);
					}
				}
			}
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x00085FC8 File Offset: 0x000841C8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnVadModeChanged(bool _auto)
		{
			if (!this.IsJoined)
			{
				return;
			}
			VADThresholdSettings vadthreshold = this.call.GetVADThreshold();
			this.call.SetVADThreshold(_auto, vadthreshold.VadThreshold());
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00085FFC File Offset: 0x000841FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnVadThresholdChanged(int _threshold)
		{
			if (!this.IsJoined)
			{
				return;
			}
			VADThresholdSettings vadthreshold = this.call.GetVADThreshold();
			this.call.SetVADThreshold(vadthreshold.Automatic(), (float)_threshold);
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00086034 File Offset: 0x00084234
		public void UpdateBlockState(DiscordManager.DiscordUser _user, bool _isBlocked)
		{
			DiscordManager.CallInfo.MemberState memberState;
			if (!this.callMembersCurrent.TryGetValue(_user.ID, out memberState))
			{
				return;
			}
			_user.LocalMuted = _isBlocked;
		}

		// Token: 0x04000ED5 RID: 3797
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioClip soundOtherJoin;

		// Token: 0x04000ED6 RID: 3798
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioClip soundOtherLeave;

		// Token: 0x04000ED7 RID: 3799
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioClip soundSelfNoManualLeave;

		// Token: 0x04000ED8 RID: 3800
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager.LobbyInfo ownerLobby;

		// Token: 0x04000ED9 RID: 3801
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager ownerManager;

		// Token: 0x04000EDA RID: 3802
		[PublicizedFrom(EAccessModifier.Private)]
		public Call call;

		// Token: 0x04000EDB RID: 3803
		[PublicizedFrom(EAccessModifier.Private)]
		public bool startedJoining;

		// Token: 0x04000EDC RID: 3804
		[PublicizedFrom(EAccessModifier.Private)]
		public bool manualLeave;

		// Token: 0x04000EDF RID: 3807
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<ulong, DiscordManager.CallInfo.MemberState> callMembersCurrent = new Dictionary<ulong, DiscordManager.CallInfo.MemberState>();

		// Token: 0x04000EE0 RID: 3808
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<ulong, DiscordManager.CallInfo.MemberState> callMembersOld = new Dictionary<ulong, DiscordManager.CallInfo.MemberState>();

		// Token: 0x0200030B RID: 779
		public struct MemberState
		{
			// Token: 0x04000EE1 RID: 3809
			public bool Speaking;

			// Token: 0x04000EE2 RID: 3810
			public bool Muted;

			// Token: 0x04000EE3 RID: 3811
			public bool Deafened;
		}
	}

	// Token: 0x0200030D RID: 781
	public enum EDiscordStatus
	{
		// Token: 0x04000EEA RID: 3818
		NotInitialized,
		// Token: 0x04000EEB RID: 3819
		Disconnected,
		// Token: 0x04000EEC RID: 3820
		Ready,
		// Token: 0x04000EED RID: 3821
		Connecting,
		// Token: 0x04000EEE RID: 3822
		Disconnecting
	}

	// Token: 0x0200030E RID: 782
	public enum ELobbyType : byte
	{
		// Token: 0x04000EF0 RID: 3824
		Global,
		// Token: 0x04000EF1 RID: 3825
		Party
	}

	// Token: 0x0200030F RID: 783
	public enum EDiscordAccountType
	{
		// Token: 0x04000EF3 RID: 3827
		None,
		// Token: 0x04000EF4 RID: 3828
		Regular,
		// Token: 0x04000EF5 RID: 3829
		Provisional
	}

	// Token: 0x02000310 RID: 784
	public enum EFullAccountLoginResult
	{
		// Token: 0x04000EF7 RID: 3831
		None,
		// Token: 0x04000EF8 RID: 3832
		Success,
		// Token: 0x04000EF9 RID: 3833
		RequestingAuth,
		// Token: 0x04000EFA RID: 3834
		AuthAccepted,
		// Token: 0x04000EFB RID: 3835
		AuthCancelled,
		// Token: 0x04000EFC RID: 3836
		AuthFailed,
		// Token: 0x04000EFD RID: 3837
		TokenExchangeFailed,
		// Token: 0x04000EFE RID: 3838
		TokenRefreshFailed,
		// Token: 0x04000EFF RID: 3839
		TokenRevoked,
		// Token: 0x04000F00 RID: 3840
		ConnectionFailed,
		// Token: 0x04000F01 RID: 3841
		PlatformError
	}

	// Token: 0x02000311 RID: 785
	public enum EProvisionalAccountLoginResult
	{
		// Token: 0x04000F03 RID: 3843
		None,
		// Token: 0x04000F04 RID: 3844
		Success,
		// Token: 0x04000F05 RID: 3845
		NotSupported,
		// Token: 0x04000F06 RID: 3846
		PlatformIdLinkedToDiscordAccount,
		// Token: 0x04000F07 RID: 3847
		TokenExchangeFailed,
		// Token: 0x04000F08 RID: 3848
		ConnectionFailed,
		// Token: 0x04000F09 RID: 3849
		PlatformError
	}

	// Token: 0x02000312 RID: 786
	public enum EAutoJoinVoiceMode
	{
		// Token: 0x04000F0B RID: 3851
		None,
		// Token: 0x04000F0C RID: 3852
		Global,
		// Token: 0x04000F0D RID: 3853
		Party
	}

	// Token: 0x02000313 RID: 787
	// (Invoke) Token: 0x060016A0 RID: 5792
	public delegate void UserAuthorizationResultCallback(bool _isDone, DiscordManager.EFullAccountLoginResult _fullAccResult, DiscordManager.EProvisionalAccountLoginResult _provisionalAccResult, bool _isExpectedSuccess);

	// Token: 0x02000314 RID: 788
	// (Invoke) Token: 0x060016A4 RID: 5796
	public delegate void LocalUserChangedCallback(bool _loggedIn);

	// Token: 0x02000315 RID: 789
	// (Invoke) Token: 0x060016A8 RID: 5800
	public delegate void LobbyStateChangedCallback(DiscordManager.LobbyInfo _lobby, bool _isReady, bool _isJoined);

	// Token: 0x02000316 RID: 790
	// (Invoke) Token: 0x060016AC RID: 5804
	public delegate void LobbyMembersChangedCallback(DiscordManager.LobbyInfo _lobby);

	// Token: 0x02000317 RID: 791
	// (Invoke) Token: 0x060016B0 RID: 5808
	public delegate void CallChangedCallback(DiscordManager.CallInfo _newCall);

	// Token: 0x02000318 RID: 792
	// (Invoke) Token: 0x060016B4 RID: 5812
	public delegate void CallStatusChangedCallback(DiscordManager.CallInfo _call, Call.Status _callStatus);

	// Token: 0x02000319 RID: 793
	// (Invoke) Token: 0x060016B8 RID: 5816
	public delegate void CallMembersChangedCallback(DiscordManager.CallInfo _call);

	// Token: 0x0200031A RID: 794
	// (Invoke) Token: 0x060016BC RID: 5820
	public delegate void VoiceStateChangedCallback(bool _self, ulong _userId);

	// Token: 0x0200031B RID: 795
	// (Invoke) Token: 0x060016C0 RID: 5824
	public delegate void SelfMuteStateChangedCallback(bool _selfMute, bool _selfDeaf);

	// Token: 0x0200031C RID: 796
	// (Invoke) Token: 0x060016C4 RID: 5828
	public delegate void FriendsListChangedCallback();

	// Token: 0x0200031D RID: 797
	// (Invoke) Token: 0x060016C8 RID: 5832
	public delegate void RelationshipChangedCallback(DiscordManager.DiscordUser _user);

	// Token: 0x0200031E RID: 798
	// (Invoke) Token: 0x060016CC RID: 5836
	public delegate void ActivityInviteReceivedCallback(DiscordManager.DiscordUser _user, bool _cleared, ActivityActionTypes _type);

	// Token: 0x0200031F RID: 799
	// (Invoke) Token: 0x060016D0 RID: 5840
	public delegate void ActivityJoiningCallback();

	// Token: 0x02000320 RID: 800
	// (Invoke) Token: 0x060016D4 RID: 5844
	public delegate void PendingActionsUpdateCallback(int _pendingActionsCount);

	// Token: 0x02000321 RID: 801
	// (Invoke) Token: 0x060016D8 RID: 5848
	public delegate void AudioDevicesChangedCallback(DiscordManager.AudioDeviceConfig _inOutConfig);

	// Token: 0x02000322 RID: 802
	[PublicizedFrom(EAccessModifier.Private)]
	public enum LobbyMemberActionType
	{
		// Token: 0x04000F0F RID: 3855
		Add,
		// Token: 0x04000F10 RID: 3856
		Remove,
		// Token: 0x04000F11 RID: 3857
		Update
	}

	// Token: 0x02000323 RID: 803
	public class AudioDeviceConfig
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x00086082 File Offset: 0x00084282
		// (set) Token: 0x060016DC RID: 5852 RVA: 0x0008608A File Offset: 0x0008428A
		public Dictionary<string, DiscordManager.DiscordAudioDevice> CurrentAudioDevices
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Dictionary<string, DiscordManager.DiscordAudioDevice>();

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00086093 File Offset: 0x00084293
		// (set) Token: 0x060016DE RID: 5854 RVA: 0x0008609B File Offset: 0x0008429B
		public string ActiveAudioDevice
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x000860A4 File Offset: 0x000842A4
		public string ConfigAudioDevice
		{
			get
			{
				if (!this.IsOutput)
				{
					return this.owner.Settings.SelectedInputDevice;
				}
				return this.owner.Settings.SelectedOutputDevice;
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x000860CF File Offset: 0x000842CF
		public AudioDeviceConfig(DiscordManager _owner, bool _isOutput)
		{
			this.owner = _owner;
			this.IsOutput = _isOutput;
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x000860FB File Offset: 0x000842FB
		public void UpdateAudioDeviceList()
		{
			if (this.owner.client == null)
			{
				this.CurrentAudioDevices.Clear();
				this.owner.fireAudioDevicesChanged(this);
				return;
			}
			this.getDevices();
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00086128 File Offset: 0x00084328
		[PublicizedFrom(EAccessModifier.Private)]
		public void getDevices()
		{
			if (this.IsOutput)
			{
				this.owner.client.GetOutputDevices(new Discord.Sdk.Client.GetOutputDevicesCallback(this.ApplyDevicesFound));
				return;
			}
			this.owner.client.GetInputDevices(new Discord.Sdk.Client.GetInputDevicesCallback(this.ApplyDevicesFound));
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00086178 File Offset: 0x00084378
		[PublicizedFrom(EAccessModifier.Private)]
		public void swapAndClearAudioDeviceList()
		{
			Dictionary<string, DiscordManager.DiscordAudioDevice> currentAudioDevices = this.CurrentAudioDevices;
			Dictionary<string, DiscordManager.DiscordAudioDevice> dictionary = this.oldAudioDevices;
			this.oldAudioDevices = currentAudioDevices;
			this.CurrentAudioDevices = dictionary;
			this.CurrentAudioDevices.Clear();
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x000861B0 File Offset: 0x000843B0
		public void ApplyDevicesFound(AudioDevice[] _devices)
		{
			this.swapAndClearAudioDeviceList();
			for (int i = 0; i < _devices.Length; i++)
			{
				DiscordManager.DiscordAudioDevice discordAudioDevice = new DiscordManager.DiscordAudioDevice(_devices[i], this.IsOutput);
				this.CurrentAudioDevices[discordAudioDevice.Identifier] = discordAudioDevice;
			}
			if (this.IsOutput)
			{
				this.owner.client.GetCurrentOutputDevice(new Discord.Sdk.Client.GetCurrentOutputDeviceCallback(this.getCurrentDeviceCallbackFn));
				return;
			}
			this.owner.client.GetCurrentInputDevice(new Discord.Sdk.Client.GetCurrentInputDeviceCallback(this.getCurrentDeviceCallbackFn));
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00086238 File Offset: 0x00084438
		[PublicizedFrom(EAccessModifier.Private)]
		public void getCurrentDeviceCallbackFn(AudioDevice _device)
		{
			bool flag = this.CurrentAudioDevices.Count == this.oldAudioDevices.Count && this.CurrentAudioDevices.Keys.All<string>(new Func<string, bool>(this.oldAudioDevices.ContainsKey));
			this.oldAudioDevices.Clear();
			string text = _device.Id();
			bool flag2 = text == this.ActiveAudioDevice;
			this.ActiveAudioDevice = text;
			Log.Out(string.Format("[Discord] Current {0} device: {1} // {2} // {3}", new object[]
			{
				this.IsOutput ? "output" : "input",
				_device.Id(),
				_device.Name(),
				_device.IsDefault()
			}));
			DiscordManager.DiscordAudioDevice discordAudioDevice;
			if (this.CurrentAudioDevices.TryGetValue(this.ConfigAudioDevice, out discordAudioDevice) && this.ConfigAudioDevice != text)
			{
				Log.Out(string.Format("[Discord] Setting {0} device from config: {1} // {2} // {3}", new object[]
				{
					this.IsOutput ? "output" : "input",
					discordAudioDevice.Identifier,
					discordAudioDevice,
					discordAudioDevice.IsDefault
				}));
				if (this.IsOutput)
				{
					this.owner.client.SetOutputDevice(this.ConfigAudioDevice, delegate(ClientResult _result)
					{
						DiscordManager.logCallbackInfoWithClientResult("SetOutputDevice", null, _result, true);
					});
				}
				else
				{
					this.owner.client.SetInputDevice(this.ConfigAudioDevice, delegate(ClientResult _result)
					{
						DiscordManager.logCallbackInfoWithClientResult("SetInputDevice", null, _result, true);
					});
				}
				this.ActiveAudioDevice = this.ConfigAudioDevice;
			}
			if (!flag || !flag2)
			{
				this.owner.fireAudioDevicesChanged(this);
			}
		}

		// Token: 0x04000F12 RID: 3858
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager owner;

		// Token: 0x04000F13 RID: 3859
		public readonly bool IsOutput;

		// Token: 0x04000F15 RID: 3861
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, DiscordManager.DiscordAudioDevice> oldAudioDevices = new Dictionary<string, DiscordManager.DiscordAudioDevice>();
	}

	// Token: 0x02000325 RID: 805
	public class DiscordAudioDevice : IPartyVoice.VoiceAudioDevice
	{
		// Token: 0x060016EA RID: 5866 RVA: 0x00086421 File Offset: 0x00084621
		public DiscordAudioDevice(AudioDevice _device, bool _isOutput)
			: base(_isOutput, _device.IsDefault())
		{
			this.id = _device.Id();
			this.name = _device.Name();
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00086448 File Offset: 0x00084648
		public override string ToString()
		{
			if (!this.IsDefault)
			{
				return this.name;
			}
			return "(Default) " + this.name;
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060016EC RID: 5868 RVA: 0x00086469 File Offset: 0x00084669
		public override string Identifier
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x04000F1A RID: 3866
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string id;

		// Token: 0x04000F1B RID: 3867
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;
	}

	// Token: 0x02000326 RID: 806
	public class DiscordSettings
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060016ED RID: 5869 RVA: 0x00086474 File Offset: 0x00084674
		// (remove) Token: 0x060016EE RID: 5870 RVA: 0x000864AC File Offset: 0x000846AC
		public event Action<string> OutputDeviceChanged;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060016EF RID: 5871 RVA: 0x000864E4 File Offset: 0x000846E4
		// (remove) Token: 0x060016F0 RID: 5872 RVA: 0x0008651C File Offset: 0x0008471C
		public event Action<string> InputDeviceChanged;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060016F1 RID: 5873 RVA: 0x00086554 File Offset: 0x00084754
		// (remove) Token: 0x060016F2 RID: 5874 RVA: 0x0008658C File Offset: 0x0008478C
		public event Action<int> OutputVolumeChanged;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060016F3 RID: 5875 RVA: 0x000865C4 File Offset: 0x000847C4
		// (remove) Token: 0x060016F4 RID: 5876 RVA: 0x000865FC File Offset: 0x000847FC
		public event Action<int> InputVolumeChanged;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060016F5 RID: 5877 RVA: 0x00086634 File Offset: 0x00084834
		// (remove) Token: 0x060016F6 RID: 5878 RVA: 0x0008666C File Offset: 0x0008486C
		public event Action<bool> VoiceModePttChanged;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060016F7 RID: 5879 RVA: 0x000866A4 File Offset: 0x000848A4
		// (remove) Token: 0x060016F8 RID: 5880 RVA: 0x000866DC File Offset: 0x000848DC
		public event Action<bool> VoiceVadModeChanged;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060016F9 RID: 5881 RVA: 0x00086714 File Offset: 0x00084914
		// (remove) Token: 0x060016FA RID: 5882 RVA: 0x0008674C File Offset: 0x0008494C
		public event Action<int> VoiceVadThresholdChanged;

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x00086781 File Offset: 0x00084981
		// (set) Token: 0x060016FC RID: 5884 RVA: 0x0008678D File Offset: 0x0008498D
		public bool DiscordFirstTimeInfoShown
		{
			get
			{
				return GamePrefs.GetBool(EnumGamePrefs.DiscordFirstTimeInfoShown);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordFirstTimeInfoShown, value);
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x0008679A File Offset: 0x0008499A
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x000867A6 File Offset: 0x000849A6
		public bool DiscordDisabled
		{
			get
			{
				return GamePrefs.GetBool(EnumGamePrefs.DiscordDisabled);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordDisabled, value);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x000867B3 File Offset: 0x000849B3
		// (set) Token: 0x06001700 RID: 5888 RVA: 0x000867BF File Offset: 0x000849BF
		public DiscordManager.EDiscordAccountType LastAccountType
		{
			get
			{
				return (DiscordManager.EDiscordAccountType)GamePrefs.GetInt(EnumGamePrefs.DiscordLastAccountType);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordLastAccountType, (int)value);
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x000867CC File Offset: 0x000849CC
		// (set) Token: 0x06001702 RID: 5890 RVA: 0x000867D8 File Offset: 0x000849D8
		public string AccessToken
		{
			get
			{
				return GamePrefs.GetString(EnumGamePrefs.DiscordAccessToken);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordAccessToken, value);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x000867E5 File Offset: 0x000849E5
		// (set) Token: 0x06001704 RID: 5892 RVA: 0x000867F1 File Offset: 0x000849F1
		public string RefreshToken
		{
			get
			{
				return GamePrefs.GetString(EnumGamePrefs.DiscordRefreshToken);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordRefreshToken, value);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x000867FE File Offset: 0x000849FE
		// (set) Token: 0x06001706 RID: 5894 RVA: 0x0008680A File Offset: 0x00084A0A
		public string SelectedOutputDevice
		{
			get
			{
				return GamePrefs.GetString(EnumGamePrefs.DiscordSelectedOutputDevice);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordSelectedOutputDevice, value);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x00086817 File Offset: 0x00084A17
		// (set) Token: 0x06001708 RID: 5896 RVA: 0x00086823 File Offset: 0x00084A23
		public string SelectedInputDevice
		{
			get
			{
				return GamePrefs.GetString(EnumGamePrefs.DiscordSelectedInputDevice);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordSelectedInputDevice, value);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x00086830 File Offset: 0x00084A30
		// (set) Token: 0x0600170A RID: 5898 RVA: 0x00086847 File Offset: 0x00084A47
		public int OutputVolume
		{
			get
			{
				return Mathf.RoundToInt(GamePrefs.GetFloat(EnumGamePrefs.DiscordOutputVolume) * 100f);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordOutputVolume, (float)value / 100f);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x0008685B File Offset: 0x00084A5B
		// (set) Token: 0x0600170C RID: 5900 RVA: 0x00086872 File Offset: 0x00084A72
		public int InputVolume
		{
			get
			{
				return Mathf.RoundToInt(GamePrefs.GetFloat(EnumGamePrefs.DiscordInputVolume) * 100f);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordInputVolume, (float)value / 100f);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x00086886 File Offset: 0x00084A86
		// (set) Token: 0x0600170E RID: 5902 RVA: 0x00086892 File Offset: 0x00084A92
		public bool VoiceModePtt
		{
			get
			{
				return GamePrefs.GetBool(EnumGamePrefs.DiscordVoiceModePtt);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordVoiceModePtt, value);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x0008689F File Offset: 0x00084A9F
		// (set) Token: 0x06001710 RID: 5904 RVA: 0x000868AB File Offset: 0x00084AAB
		public bool VoiceVadModeAuto
		{
			get
			{
				return GamePrefs.GetBool(EnumGamePrefs.DiscordVoiceVadModeAuto);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordVoiceVadModeAuto, value);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x000868B8 File Offset: 0x00084AB8
		// (set) Token: 0x06001712 RID: 5906 RVA: 0x000868C4 File Offset: 0x00084AC4
		public int VoiceVadThreshold
		{
			get
			{
				return GamePrefs.GetInt(EnumGamePrefs.DiscordVoiceVadThreshold);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordVoiceVadThreshold, value);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x000868D1 File Offset: 0x00084AD1
		// (set) Token: 0x06001714 RID: 5908 RVA: 0x000868DD File Offset: 0x00084ADD
		public bool DmPrivacyMode
		{
			get
			{
				return GamePrefs.GetBool(EnumGamePrefs.DiscordDmPrivacyMode);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordDmPrivacyMode, value);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x000868EA File Offset: 0x00084AEA
		// (set) Token: 0x06001716 RID: 5910 RVA: 0x000868F6 File Offset: 0x00084AF6
		public DiscordManager.EAutoJoinVoiceMode AutoJoinVoiceMode
		{
			get
			{
				return (DiscordManager.EAutoJoinVoiceMode)GamePrefs.GetInt(EnumGamePrefs.DiscordAutoJoinVoiceMode);
			}
			set
			{
				GamePrefs.Set(EnumGamePrefs.DiscordAutoJoinVoiceMode, (int)value);
			}
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00086903 File Offset: 0x00084B03
		[PublicizedFrom(EAccessModifier.Private)]
		public DiscordSettings()
		{
			GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x0008691C File Offset: 0x00084B1C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnGamePrefChanged(EnumGamePrefs _pref)
		{
			switch (_pref)
			{
			case EnumGamePrefs.DiscordSelectedOutputDevice:
			{
				Action<string> outputDeviceChanged = this.OutputDeviceChanged;
				if (outputDeviceChanged == null)
				{
					return;
				}
				outputDeviceChanged(this.SelectedOutputDevice);
				return;
			}
			case EnumGamePrefs.DiscordSelectedInputDevice:
			{
				Action<string> inputDeviceChanged = this.InputDeviceChanged;
				if (inputDeviceChanged == null)
				{
					return;
				}
				inputDeviceChanged(this.SelectedInputDevice);
				return;
			}
			case EnumGamePrefs.DiscordOutputVolume:
			{
				Action<int> outputVolumeChanged = this.OutputVolumeChanged;
				if (outputVolumeChanged == null)
				{
					return;
				}
				outputVolumeChanged(this.OutputVolume);
				return;
			}
			case EnumGamePrefs.DiscordInputVolume:
			{
				Action<int> inputVolumeChanged = this.InputVolumeChanged;
				if (inputVolumeChanged == null)
				{
					return;
				}
				inputVolumeChanged(this.InputVolume);
				return;
			}
			case EnumGamePrefs.DiscordVoiceModePtt:
			{
				Action<bool> voiceModePttChanged = this.VoiceModePttChanged;
				if (voiceModePttChanged == null)
				{
					return;
				}
				voiceModePttChanged(this.VoiceModePtt);
				return;
			}
			case EnumGamePrefs.DiscordVoiceVadModeAuto:
			{
				Action<bool> voiceVadModeChanged = this.VoiceVadModeChanged;
				if (voiceVadModeChanged == null)
				{
					return;
				}
				voiceVadModeChanged(this.VoiceVadModeAuto);
				return;
			}
			case EnumGamePrefs.DiscordVoiceVadThreshold:
			{
				Action<int> voiceVadThresholdChanged = this.VoiceVadThresholdChanged;
				if (voiceVadThresholdChanged == null)
				{
					return;
				}
				voiceVadThresholdChanged(this.VoiceVadThreshold);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x000869F4 File Offset: 0x00084BF4
		public static DiscordManager.DiscordSettings Load()
		{
			if (!SdPlayerPrefs.HasKey("DiscordSettings"))
			{
				return new DiscordManager.DiscordSettings();
			}
			DiscordManager.DiscordSettings discordSettings;
			try
			{
				JsonConvert.DeserializeObject<DiscordManager.DiscordSettings.LegacySettings>(SdPlayerPrefs.GetString("DiscordSettings"));
				discordSettings = new DiscordManager.DiscordSettings();
				Log.Out(string.Format("[Discord] Loaded legacy settings with DiscordDisabled={0}", discordSettings.DiscordDisabled));
				GamePrefs.Instance.Save();
			}
			catch (JsonException ex)
			{
				Log.Error("[Discord] Failed loading legacy settings:");
				Log.Exception(ex);
				discordSettings = new DiscordManager.DiscordSettings();
			}
			SdPlayerPrefs.DeleteKey("DiscordSettings");
			return discordSettings;
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00086A84 File Offset: 0x00084C84
		public void Save()
		{
			Log.Out(string.Format("[Discord] Saving settings with DiscordDisabled={0}", this.DiscordDisabled));
			GamePrefs.Instance.Save();
		}

		// Token: 0x04000F23 RID: 3875
		public const string DiscordSettingsPlayerPrefName = "DiscordSettings";

		// Token: 0x02000327 RID: 807
		[PublicizedFrom(EAccessModifier.Private)]
		public class LegacySettings
		{
			// Token: 0x17000296 RID: 662
			// (set) Token: 0x0600171B RID: 5915 RVA: 0x0008678D File Offset: 0x0008498D
			public bool DiscordFirstTimeInfoShown
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordFirstTimeInfoShown, value);
				}
			}

			// Token: 0x17000297 RID: 663
			// (set) Token: 0x0600171C RID: 5916 RVA: 0x000867A6 File Offset: 0x000849A6
			public bool DiscordDisabled
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordDisabled, value);
				}
			}

			// Token: 0x17000298 RID: 664
			// (set) Token: 0x0600171D RID: 5917 RVA: 0x000867BF File Offset: 0x000849BF
			public DiscordManager.EDiscordAccountType LastAccountType
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordLastAccountType, (int)value);
				}
			}

			// Token: 0x17000299 RID: 665
			// (set) Token: 0x0600171E RID: 5918 RVA: 0x000867D8 File Offset: 0x000849D8
			public string AccessToken
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordAccessToken, value);
				}
			}

			// Token: 0x1700029A RID: 666
			// (set) Token: 0x0600171F RID: 5919 RVA: 0x000867F1 File Offset: 0x000849F1
			public string RefreshToken
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordRefreshToken, value);
				}
			}

			// Token: 0x1700029B RID: 667
			// (set) Token: 0x06001720 RID: 5920 RVA: 0x0008680A File Offset: 0x00084A0A
			public string SelectedOutputDevice
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordSelectedOutputDevice, value);
				}
			}

			// Token: 0x1700029C RID: 668
			// (set) Token: 0x06001721 RID: 5921 RVA: 0x00086823 File Offset: 0x00084A23
			public string SelectedInputDevice
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordSelectedInputDevice, value);
				}
			}

			// Token: 0x1700029D RID: 669
			// (set) Token: 0x06001722 RID: 5922 RVA: 0x00086847 File Offset: 0x00084A47
			public int OutputVolume
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordOutputVolume, (float)value / 100f);
				}
			}

			// Token: 0x1700029E RID: 670
			// (set) Token: 0x06001723 RID: 5923 RVA: 0x00086872 File Offset: 0x00084A72
			public int InputVolume
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordInputVolume, (float)value / 100f);
				}
			}

			// Token: 0x1700029F RID: 671
			// (set) Token: 0x06001724 RID: 5924 RVA: 0x00086892 File Offset: 0x00084A92
			public bool VoiceModePtt
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordVoiceModePtt, value);
				}
			}

			// Token: 0x170002A0 RID: 672
			// (set) Token: 0x06001725 RID: 5925 RVA: 0x000868AB File Offset: 0x00084AAB
			public bool VoiceVadModeAuto
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordVoiceVadModeAuto, value);
				}
			}

			// Token: 0x170002A1 RID: 673
			// (set) Token: 0x06001726 RID: 5926 RVA: 0x000868C4 File Offset: 0x00084AC4
			public int VoiceVadThreshold
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordVoiceVadThreshold, value);
				}
			}

			// Token: 0x170002A2 RID: 674
			// (set) Token: 0x06001727 RID: 5927 RVA: 0x000868DD File Offset: 0x00084ADD
			public bool DmPrivacyMode
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordDmPrivacyMode, value);
				}
			}

			// Token: 0x170002A3 RID: 675
			// (set) Token: 0x06001728 RID: 5928 RVA: 0x000868F6 File Offset: 0x00084AF6
			public DiscordManager.EAutoJoinVoiceMode AutoJoinVoiceMode
			{
				set
				{
					GamePrefs.Set(EnumGamePrefs.DiscordAutoJoinVoiceMode, (int)value);
				}
			}
		}
	}

	// Token: 0x02000328 RID: 808
	public class DiscordUser : IDisposable, IEquatable<DiscordManager.DiscordUser>
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x00086AAA File Offset: 0x00084CAA
		// (set) Token: 0x0600172B RID: 5931 RVA: 0x00086AB2 File Offset: 0x00084CB2
		public bool IsProvisionalAccount
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x00086ABB File Offset: 0x00084CBB
		public StatusType DiscordState
		{
			get
			{
				UserHandle userHandle = this.userHandle;
				if (userHandle == null)
				{
					return StatusType.Offline;
				}
				return userHandle.Status();
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x00086ACE File Offset: 0x00084CCE
		public string DiscordStateLocalized
		{
			get
			{
				string text = "discordState";
				UserHandle userHandle = this.userHandle;
				return Localization.Get(text + ((userHandle != null) ? userHandle.Status() : StatusType.Offline).ToStringCached<StatusType>(), false, null);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x00086AF8 File Offset: 0x00084CF8
		public string DisplayName
		{
			get
			{
				return this.playerName ?? this.DiscordDisplayName;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x00086B0A File Offset: 0x00084D0A
		public string DiscordDisplayName
		{
			get
			{
				return this.discordDisplayName ?? "<unknown>";
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x00086B1B File Offset: 0x00084D1B
		public string DiscordUserName
		{
			get
			{
				UserHandle userHandle = this.userHandle;
				return ((userHandle != null) ? userHandle.Username() : null) ?? "<unknown>";
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00086B38 File Offset: 0x00084D38
		public string PlayerName
		{
			get
			{
				return this.playerName ?? "<unknown>";
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x00086B49 File Offset: 0x00084D49
		public Texture2D Avatar
		{
			get
			{
				return this.avatar;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x00086B51 File Offset: 0x00084D51
		public bool InGlobalLobby
		{
			get
			{
				return this.ownerManager.globalLobby.HasMember(this.ID);
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x00086B69 File Offset: 0x00084D69
		public bool InPartyLobby
		{
			get
			{
				return this.ownerManager.partyLobby.HasMember(this.ID);
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00086B81 File Offset: 0x00084D81
		public bool PendingAction
		{
			get
			{
				return this.PendingIncomingJoinRequest || this.PendingIncomingInvite || this.PendingFriendRequest;
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00086B9B File Offset: 0x00084D9B
		public DiscordUser(DiscordManager _ownerManager, ulong _id, bool _isLocalAccount = false)
		{
			this.ownerManager = _ownerManager;
			this.ID = _id;
			this.IsLocalAccount = _isLocalAccount;
			this.TryUpdateDiscordHandle();
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00086BC0 File Offset: 0x00084DC0
		public void TryUpdateDiscordHandle()
		{
			this.userHandle = ((!this.ownerManager.IsReady) ? null : this.ownerManager.client.GetUser(this.ID));
			if (this.userHandle != null)
			{
				this.IsProvisionalAccount = this.userHandle.IsProvisional();
				this.updateDisplayName();
			}
			this.UpdateRelationship();
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00086C20 File Offset: 0x00084E20
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateDisplayName()
		{
			string text = this.userHandle.DisplayName();
			foreach (char c in text)
			{
				if (c >= '\ud800' && c < '\ue000')
				{
					this.discordDisplayName = this.userHandle.Username();
					return;
				}
			}
			this.discordDisplayName = text;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00086C7D File Offset: 0x00084E7D
		public void RequestAvatar()
		{
			if (!this.avatarStartedDownload && this.userHandle != null)
			{
				ThreadManager.StartCoroutine(this.downloadDiscordAvatar());
			}
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00086C9B File Offset: 0x00084E9B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator downloadDiscordAvatar()
		{
			this.avatarStartedDownload = true;
			string text = null;
			try
			{
				text = this.userHandle.AvatarUrl(UserHandle.AvatarType.Png, UserHandle.AvatarType.Png);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
			if (string.IsNullOrEmpty(text))
			{
				yield break;
			}
			MicroStopwatch mswDownload = new MicroStopwatch(true);
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(text);
			www.SendWebRequest();
			while (!www.isDone)
			{
				yield return null;
			}
			if (www.result == UnityWebRequest.Result.Success)
			{
				Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				this.avatar = TextureUtils.CloneTexture(texture, false, false, true);
				global::UnityEngine.Object.DestroyImmediate(texture);
				DiscordManager.FriendsListChangedCallback friendsListChanged = this.ownerManager.FriendsListChanged;
				if (friendsListChanged != null)
				{
					friendsListChanged();
				}
				if (DiscordManager.logLevel == LoggingSeverity.Verbose)
				{
					Log.Out(string.Format("[Discord] Downloading avatar for user {0} took {1} ms. Size: {2} B, resolution: {3} x {4}", new object[]
					{
						this.DiscordDisplayName,
						mswDownload.ElapsedMilliseconds,
						www.downloadedBytes,
						this.avatar.width,
						this.avatar.height
					}));
				}
			}
			else if (DiscordManager.logLevel <= LoggingSeverity.Warning)
			{
				Log.Warning("[Discord] Retrieving avatar for user " + this.DiscordDisplayName + " failed: " + www.error);
			}
			yield break;
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00086CAC File Offset: 0x00084EAC
		public void UpdatePlayerName(EntityPlayer _entity = null)
		{
			if (_entity != null)
			{
				this.playerName = _entity.PlayerDisplayName;
				return;
			}
			int num;
			if (!this.ownerManager.userMappings.TryGetEntityId(this.ID, out num))
			{
				return;
			}
			if (GameManager.Instance.World == null || !GameManager.Instance.World.Players.dict.TryGetValue(num, out _entity))
			{
				return;
			}
			this.playerName = _entity.PlayerDisplayName;
			DiscordManager.FriendsListChangedCallback friendsListChanged = this.ownerManager.FriendsListChanged;
			if (friendsListChanged == null)
			{
				return;
			}
			friendsListChanged();
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x00086D38 File Offset: 0x00084F38
		public bool InCurrentVoice
		{
			get
			{
				DiscordManager.LobbyInfo activeVoiceLobby = this.ownerManager.ActiveVoiceLobby;
				DiscordManager.CallInfo.MemberState memberState;
				return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.TryGetMember(this.ID, out memberState);
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x00086D68 File Offset: 0x00084F68
		// (set) Token: 0x0600173E RID: 5950 RVA: 0x00086D98 File Offset: 0x00084F98
		public double Volume
		{
			get
			{
				return (double)(this.InCurrentVoice ? (this.ownerManager.ActiveVoiceLobby.VoiceCall.GetParticipantVolume(this) / 100f) : 0f);
			}
			set
			{
				if (this.IsLocalAccount)
				{
					return;
				}
				if (!this.InCurrentVoice)
				{
					return;
				}
				this.ownerManager.ActiveVoiceLobby.VoiceCall.SetParticipantVolume(this, Mathf.Clamp((float)value * 100f, 0f, 200f));
				this.ownerManager.UserSettings.SetUserVolume(this.ID, value);
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x00086DFC File Offset: 0x00084FFC
		public bool IsSpeaking
		{
			get
			{
				DiscordManager.LobbyInfo activeVoiceLobby = this.ownerManager.ActiveVoiceLobby;
				DiscordManager.CallInfo.MemberState memberState;
				return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.TryGetMember(this.ID, out memberState) && memberState.Speaking;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00086E37 File Offset: 0x00085037
		public bool IsMutedLocalOrRemote
		{
			get
			{
				return this.IsMuted || this.LocalMuted;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x00086E4C File Offset: 0x0008504C
		// (set) Token: 0x06001742 RID: 5954 RVA: 0x00086EA4 File Offset: 0x000850A4
		public bool LocalMuted
		{
			get
			{
				DiscordManager.LobbyInfo activeVoiceLobby = this.ownerManager.ActiveVoiceLobby;
				bool? flag;
				if (activeVoiceLobby == null)
				{
					flag = null;
				}
				else
				{
					Call call = activeVoiceLobby.VoiceCall.Call;
					flag = ((call != null) ? new bool?(call.GetLocalMute(this.ID)) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
			set
			{
				DiscordManager.LobbyInfo activeVoiceLobby = this.ownerManager.ActiveVoiceLobby;
				if (activeVoiceLobby == null)
				{
					return;
				}
				Call call = activeVoiceLobby.VoiceCall.Call;
				if (call == null)
				{
					return;
				}
				call.SetLocalMute(this.ID, value);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00086ED4 File Offset: 0x000850D4
		public bool IsMuted
		{
			get
			{
				DiscordManager.LobbyInfo activeVoiceLobby = this.ownerManager.ActiveVoiceLobby;
				DiscordManager.CallInfo.MemberState memberState;
				return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.TryGetMember(this.ID, out memberState) && memberState.Muted;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00086F10 File Offset: 0x00085110
		public bool IsDeafened
		{
			get
			{
				DiscordManager.LobbyInfo activeVoiceLobby = this.ownerManager.ActiveVoiceLobby;
				DiscordManager.CallInfo.MemberState memberState;
				return activeVoiceLobby != null && activeVoiceLobby.VoiceCall.TryGetMember(this.ID, out memberState) && memberState.Deafened;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x00086F4C File Offset: 0x0008514C
		public IPartyVoice.EVoiceMemberState VoiceState
		{
			get
			{
				DiscordManager.LobbyInfo activeVoiceLobby = this.ownerManager.ActiveVoiceLobby;
				DiscordManager.CallInfo.MemberState memberState;
				if (activeVoiceLobby == null || !activeVoiceLobby.VoiceCall.TryGetMember(this.ID, out memberState))
				{
					return IPartyVoice.EVoiceMemberState.Disabled;
				}
				if (this.LocalMuted || memberState.Muted)
				{
					return IPartyVoice.EVoiceMemberState.Muted;
				}
				if (memberState.Speaking)
				{
					return IPartyVoice.EVoiceMemberState.VoiceActive;
				}
				return IPartyVoice.EVoiceMemberState.Normal;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x00086F9E File Offset: 0x0008519E
		// (set) Token: 0x06001747 RID: 5959 RVA: 0x00086FA6 File Offset: 0x000851A6
		public RelationshipType DiscordRelationship
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x00086FAF File Offset: 0x000851AF
		// (set) Token: 0x06001749 RID: 5961 RVA: 0x00086FB7 File Offset: 0x000851B7
		public RelationshipType GameRelationship
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x00086FC0 File Offset: 0x000851C0
		public bool IsDiscordFriend
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.DiscordRelationship == RelationshipType.Friend;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x00086FCB File Offset: 0x000851CB
		public bool IsGameFriend
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.GameRelationship == RelationshipType.Friend;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00086FD6 File Offset: 0x000851D6
		public bool IsFriend
		{
			get
			{
				return this.IsDiscordFriend || this.IsGameFriend;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x00086FE8 File Offset: 0x000851E8
		public bool IsBlocked
		{
			get
			{
				return this.DiscordRelationship == RelationshipType.Blocked;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x00086FF3 File Offset: 0x000851F3
		public bool PendingFriendRequest
		{
			get
			{
				return !this.isSpamRequest && (this.DiscordRelationship == RelationshipType.PendingIncoming || this.GameRelationship == RelationshipType.PendingIncoming);
			}
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00087014 File Offset: 0x00085214
		public void UpdateRelationship()
		{
			if (this.ownerManager.IsReady)
			{
				RelationshipHandle relationshipHandle = this.ownerManager.client.GetRelationshipHandle(this.ID);
				this.DiscordRelationship = relationshipHandle.DiscordRelationshipType();
				this.GameRelationship = relationshipHandle.GameRelationshipType();
				this.isSpamRequest = relationshipHandle.IsSpamRequest();
			}
			else
			{
				this.DiscordRelationship = RelationshipType.None;
				this.GameRelationship = RelationshipType.None;
				this.isSpamRequest = false;
			}
			if (this.IsFriend || this.IsBlocked || this.PendingFriendRequest)
			{
				this.RequestAvatar();
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000870A0 File Offset: 0x000852A0
		public void SendFriendRequest(bool _gameFriend)
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			string text = (_gameFriend ? "game" : "Discord");
			switch (_gameFriend ? this.GameRelationship : this.DiscordRelationship)
			{
			case RelationshipType.Friend:
				Log.Out(string.Format("[Discord] Not sending {0} friend request (already friends) to {1}", text, this));
				return;
			case RelationshipType.PendingIncoming:
				Log.Out(string.Format("[Discord] Accepting {0} friend request from {1}", text, this));
				goto IL_009C;
			case RelationshipType.PendingOutgoing:
				Log.Out(string.Format("[Discord] Not sending {0} friend request (already sent) to {1}", text, this));
				return;
			}
			Log.Out(string.Format("[Discord] Sending {0} friend request to {1}", text, this));
			IL_009C:
			if (_gameFriend)
			{
				this.ownerManager.client.SendGameFriendRequestById(this.ID, new Discord.Sdk.Client.UpdateRelationshipCallback(DiscordManager.DiscordUser.<SendFriendRequest>g__SendRequestCallback|77_0));
				return;
			}
			this.ownerManager.client.SendDiscordFriendRequestById(this.ID, new Discord.Sdk.Client.UpdateRelationshipCallback(DiscordManager.DiscordUser.<SendFriendRequest>g__SendRequestCallback|77_0));
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00087194 File Offset: 0x00085394
		public void DeclineFriendRequest(bool _gameFriend)
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			string text = (_gameFriend ? "game" : "Discord");
			if ((_gameFriend ? this.GameRelationship : this.DiscordRelationship) != RelationshipType.PendingIncoming)
			{
				Log.Out(string.Format("[Discord] Not rejecting {0} friend request (no pending request) from {1}", text, this));
				return;
			}
			Log.Out(string.Format("[Discord] Rejecting {0} friend request from {1}", text, this));
			if (_gameFriend)
			{
				this.ownerManager.client.RejectGameFriendRequest(this.ID, new Discord.Sdk.Client.UpdateRelationshipCallback(DiscordManager.DiscordUser.<DeclineFriendRequest>g__RejectRequestCallback|78_0));
				return;
			}
			this.ownerManager.client.RejectDiscordFriendRequest(this.ID, new Discord.Sdk.Client.UpdateRelationshipCallback(DiscordManager.DiscordUser.<DeclineFriendRequest>g__RejectRequestCallback|78_0));
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00087240 File Offset: 0x00085440
		public void RemoveFriend()
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			if (!this.IsFriend)
			{
				Log.Out(string.Format("[Discord] Not removing friend (neither a game nor Discord friend): {0}", this));
				return;
			}
			this.ownerManager.client.RemoveDiscordAndGameFriend(this.ID, new Discord.Sdk.Client.UpdateRelationshipCallback(DiscordManager.DiscordUser.<RemoveFriend>g__RemoveFriendCallback|79_0));
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00087298 File Offset: 0x00085498
		public void BlockUser()
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			if (this.DiscordRelationship == RelationshipType.Blocked)
			{
				Log.Out(string.Format("[Discord] Not blocking user (already blocked): {0}", this));
				return;
			}
			this.ownerManager.client.BlockUser(this.ID, new Discord.Sdk.Client.UpdateRelationshipCallback(DiscordManager.DiscordUser.<BlockUser>g__BlockUserCallback|80_0));
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x000872F0 File Offset: 0x000854F0
		public void UnblockUser()
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			if (this.DiscordRelationship != RelationshipType.Blocked)
			{
				Log.Out(string.Format("[Discord] Not unblocking user (not blocked): {0}", this));
				return;
			}
			this.ownerManager.client.UnblockUser(this.ID, new Discord.Sdk.Client.UpdateRelationshipCallback(DiscordManager.DiscordUser.<UnblockUser>g__UnblockUserUserCallback|81_0));
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x00087347 File Offset: 0x00085547
		// (set) Token: 0x06001756 RID: 5974 RVA: 0x0008734F File Offset: 0x0008554F
		public Activity Activity
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x00087358 File Offset: 0x00085558
		// (set) Token: 0x06001758 RID: 5976 RVA: 0x00087360 File Offset: 0x00085560
		public bool PendingOutgoingJoinRequest { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x00087369 File Offset: 0x00085569
		// (set) Token: 0x0600175A RID: 5978 RVA: 0x00087371 File Offset: 0x00085571
		public bool PendingIncomingJoinRequest
		{
			get
			{
				return this.pendingIncomingJoinRequest;
			}
			set
			{
				if (value == this.pendingIncomingJoinRequest)
				{
					return;
				}
				this.pendingIncomingJoinRequest = value;
				DiscordManager.ActivityInviteReceivedCallback activityInviteReceived = this.ownerManager.ActivityInviteReceived;
				if (activityInviteReceived == null)
				{
					return;
				}
				activityInviteReceived(this, !value, ActivityActionTypes.JoinRequest);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x0008739F File Offset: 0x0008559F
		public bool PendingIncomingInvite
		{
			get
			{
				return this.incomingInviteActivity != null;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x000873AA File Offset: 0x000855AA
		public bool JoinableActivity
		{
			get
			{
				Activity activity = this.Activity;
				return ((activity != null) ? activity.Party() : null) != null;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x000873C1 File Offset: 0x000855C1
		public bool InGame
		{
			get
			{
				return this.Activity != null;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x000873CC File Offset: 0x000855CC
		public bool InSameSession
		{
			get
			{
				int num;
				return this.ownerManager.userMappings.TryGetEntityId(this.ID, out num);
			}
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x000873F1 File Offset: 0x000855F1
		public void UpdatePresenceInfo()
		{
			if (!this.ownerManager.IsReady)
			{
				this.Activity = null;
				return;
			}
			this.Activity = this.userHandle.GameActivity();
			this.logPresenceInfo();
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00087420 File Offset: 0x00085620
		[PublicizedFrom(EAccessModifier.Private)]
		public void logPresenceInfo()
		{
			StatusType statusType = this.userHandle.Status();
			string text = "<null>";
			if (this.Activity != null)
			{
				ulong? num = this.Activity.ApplicationId();
				string text2 = this.Activity.Details();
				string text3 = this.Activity.Name();
				string text4 = this.Activity.State();
				ActivityTypes activityTypes = this.Activity.Type();
				using (ActivityAssets activityAssets = this.Activity.Assets())
				{
					using (ActivityParty activityParty = this.Activity.Party())
					{
						string text5 = "null";
						if (activityParty != null)
						{
							text5 = string.Format("id={0}, size={1}, max={2}", activityParty.Id(), activityParty.CurrentSize(), activityParty.MaxSize());
						}
						using (ActivitySecrets activitySecrets = this.Activity.Secrets())
						{
							string text6 = ((activitySecrets != null) ? activitySecrets.Join() : null) ?? "null";
							using (ActivityTimestamps activityTimestamps = this.Activity.Timestamps())
							{
								string text7 = "null";
								if (activityTimestamps != null)
								{
									text7 = string.Format("start={0}, end={1}", activityTimestamps.Start(), activityTimestamps.End());
								}
								text = string.Format(" appId={0}, type={1}, name='{2}', state={3}, details='{4}', assets={5}, party=<{6}>, secrets.join={7}, timestamps=<{8}> ", new object[]
								{
									num,
									activityTypes.ToStringCached<ActivityTypes>(),
									text3,
									text4,
									text2,
									activityAssets != null,
									text5,
									text6,
									text7
								});
							}
						}
					}
				}
			}
			DiscordManager.logCallbackInfo(string.Format("OnPresenceChanged: user={0}, status={1}, activity=<{2}>", this, statusType.ToStringCached<StatusType>(), text), LogType.Log);
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00087638 File Offset: 0x00085838
		public void SetIncomingInviteActivity(ActivityInvite _invite)
		{
			this.incomingInviteActivity = _invite;
			DiscordManager.ActivityInviteReceivedCallback activityInviteReceived = this.ownerManager.ActivityInviteReceived;
			if (activityInviteReceived == null)
			{
				return;
			}
			activityInviteReceived(this, _invite == null, ActivityActionTypes.Join);
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0008765C File Offset: 0x0008585C
		public void SendInvite()
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			Log.Out(string.Format("[Discord] Sending invite to {0}", this));
			this.PendingIncomingJoinRequest = false;
			this.ownerManager.client.SendActivityInvite(this.ID, "", delegate(ClientResult _result)
			{
				DiscordManager.logCallbackInfoWithClientResult("SendActivityInvite", null, _result, true);
			});
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000876C8 File Offset: 0x000858C8
		public void SendJoinRequest()
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			Log.Out(string.Format("[Discord] Sending join request to {0}", this));
			this.ownerManager.client.SendActivityJoinRequest(this.ID, delegate(ClientResult _result)
			{
				DiscordManager.logCallbackInfoWithClientResult("SendActivityJoinRequest", null, _result, true);
				this.PendingOutgoingJoinRequest = true;
			});
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00087715 File Offset: 0x00085915
		public void DeclineJoinRequest()
		{
			if (!this.PendingIncomingJoinRequest)
			{
				Log.Out(string.Format("[Discord] Trying to decline incoming join request without first receiving a request from {0}", this));
				return;
			}
			this.PendingIncomingJoinRequest = false;
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00087737 File Offset: 0x00085937
		public void DeclineInvite()
		{
			if (!this.PendingIncomingInvite)
			{
				Log.Out(string.Format("[Discord] Trying to decline invite without first receiving an invite from {0}", this));
				return;
			}
			this.incomingInviteActivity = null;
			DiscordManager.ActivityInviteReceivedCallback activityInviteReceived = this.ownerManager.ActivityInviteReceived;
			if (activityInviteReceived == null)
			{
				return;
			}
			activityInviteReceived(this, true, ActivityActionTypes.Join);
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00087774 File Offset: 0x00085974
		public void AcceptInvite(ActivityInvite _invite = null)
		{
			if (!this.ownerManager.IsReady)
			{
				return;
			}
			if (_invite == null)
			{
				_invite = this.incomingInviteActivity;
			}
			this.incomingInviteActivity = null;
			if (_invite == null)
			{
				Log.Out(string.Format("[Discord] Trying to accept invite without first receiving an invite from {0}", this));
				return;
			}
			Log.Out(string.Format("[Discord] Accepting invite from {0}", this));
			this.PendingOutgoingJoinRequest = false;
			DiscordManager.ActivityInviteReceivedCallback activityInviteReceived = this.ownerManager.ActivityInviteReceived;
			if (activityInviteReceived != null)
			{
				activityInviteReceived(this, true, ActivityActionTypes.Join);
			}
			this.ownerManager.client.AcceptActivityInvite(_invite, delegate(ClientResult _result, string _secret)
			{
				DiscordManager.logCallbackInfoWithClientResult("AcceptActivityInvite", "secret=" + _secret, _result, true);
				_invite.Dispose();
			});
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00087824 File Offset: 0x00085A24
		public override string ToString()
		{
			return string.Format("<Id={0}, local={1}, Discord='{2}', Player='{3}', DcRel={4}, GameRel={5}>", new object[]
			{
				this.ID,
				this.IsLocalAccount,
				this.DiscordDisplayName,
				this.PlayerName,
				this.DiscordRelationship.ToStringCached<RelationshipType>(),
				this.GameRelationship.ToStringCached<RelationshipType>()
			});
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0008788B File Offset: 0x00085A8B
		public void Dispose()
		{
			UserHandle userHandle = this.userHandle;
			if (userHandle != null)
			{
				userHandle.Dispose();
			}
			Activity activity = this.Activity;
			if (activity != null)
			{
				activity.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x000878B5 File Offset: 0x00085AB5
		public bool Equals(DiscordManager.DiscordUser _other)
		{
			return _other != null && (this == _other || this.ID == _other.ID);
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x000878D0 File Offset: 0x00085AD0
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((DiscordManager.DiscordUser)_obj)));
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00087900 File Offset: 0x00085B00
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0008791B File Offset: 0x00085B1B
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <SendFriendRequest>g__SendRequestCallback|77_0(ClientResult _result)
		{
			DiscordManager.logCallbackInfoWithClientResult("Send*FriendRequestById", null, _result, true);
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0008792A File Offset: 0x00085B2A
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <DeclineFriendRequest>g__RejectRequestCallback|78_0(ClientResult _result)
		{
			DiscordManager.logCallbackInfoWithClientResult("Reject*FriendRequest", null, _result, true);
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00087939 File Offset: 0x00085B39
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <RemoveFriend>g__RemoveFriendCallback|79_0(ClientResult _result)
		{
			DiscordManager.logCallbackInfoWithClientResult("RemoveDiscordAndGameFriend", null, _result, true);
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00087948 File Offset: 0x00085B48
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <BlockUser>g__BlockUserCallback|80_0(ClientResult _result)
		{
			DiscordManager.logCallbackInfoWithClientResult("BlockUser", null, _result, true);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00087957 File Offset: 0x00085B57
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <UnblockUser>g__UnblockUserUserCallback|81_0(ClientResult _result)
		{
			DiscordManager.logCallbackInfoWithClientResult("UnblockUser", null, _result, true);
		}

		// Token: 0x04000F24 RID: 3876
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager ownerManager;

		// Token: 0x04000F25 RID: 3877
		[PublicizedFrom(EAccessModifier.Private)]
		public UserHandle userHandle;

		// Token: 0x04000F26 RID: 3878
		[PublicizedFrom(EAccessModifier.Private)]
		public string discordDisplayName;

		// Token: 0x04000F27 RID: 3879
		public readonly ulong ID;

		// Token: 0x04000F28 RID: 3880
		public readonly bool IsLocalAccount;

		// Token: 0x04000F2A RID: 3882
		[PublicizedFrom(EAccessModifier.Private)]
		public string playerName;

		// Token: 0x04000F2B RID: 3883
		[PublicizedFrom(EAccessModifier.Private)]
		public bool avatarStartedDownload;

		// Token: 0x04000F2C RID: 3884
		[PublicizedFrom(EAccessModifier.Private)]
		public Texture2D avatar;

		// Token: 0x04000F2D RID: 3885
		public bool MessageSentFromGame;

		// Token: 0x04000F30 RID: 3888
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isSpamRequest;

		// Token: 0x04000F33 RID: 3891
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pendingIncomingJoinRequest;

		// Token: 0x04000F34 RID: 3892
		[PublicizedFrom(EAccessModifier.Private)]
		public ActivityInvite incomingInviteActivity;
	}

	// Token: 0x0200032C RID: 812
	public class DiscordUserMappingManager
	{
		// Token: 0x0600177D RID: 6013 RVA: 0x00087B7C File Offset: 0x00085D7C
		public DiscordUserMappingManager(DiscordManager _ownerManager)
		{
			this.ownerManager = _ownerManager;
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00087BA4 File Offset: 0x00085DA4
		public void UpdateMapping(int _entityId, bool _remove, ulong _discordId)
		{
			ulong num;
			if (this.entityIdToDiscordId.TryGetValue(_entityId, out num))
			{
				this.discordIdToEntityId.Remove(num);
			}
			if (_remove)
			{
				this.entityIdToDiscordId.Remove(_entityId);
				return;
			}
			this.entityIdToDiscordId[_entityId] = _discordId;
			this.discordIdToEntityId[_discordId] = _entityId;
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00087BF9 File Offset: 0x00085DF9
		public bool TryGetDiscordId(int _entity, out ulong _discordId)
		{
			return this.entityIdToDiscordId.TryGetValue(_entity, out _discordId);
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00087C08 File Offset: 0x00085E08
		public bool TryGetEntityId(ulong _discordId, out int _entity)
		{
			return this.discordIdToEntityId.TryGetValue(_discordId, out _entity);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00087C18 File Offset: 0x00085E18
		public void SendMappingsToClient(ClientInfo _clientInfo)
		{
			List<int> list = new List<int>();
			List<ulong> list2 = new List<ulong>();
			foreach (KeyValuePair<int, ulong> keyValuePair in this.entityIdToDiscordId)
			{
				int num;
				ulong num2;
				keyValuePair.Deconstruct(out num, out num2);
				int num3 = num;
				ulong num4 = num2;
				list.Add(num3);
				list2.Add(num4);
			}
			_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageDiscordIdMappings>().Setup(list, list2));
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00087CA4 File Offset: 0x00085EA4
		public void Clear()
		{
			this.discordIdToEntityId.Clear();
			this.entityIdToDiscordId.Clear();
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00087CBC File Offset: 0x00085EBC
		public void GetAll(Action<int, ulong> _callback)
		{
			if (GameManager.Instance.World == null)
			{
				return;
			}
			foreach (KeyValuePair<int, ulong> keyValuePair in this.entityIdToDiscordId)
			{
				int num;
				ulong num2;
				keyValuePair.Deconstruct(out num, out num2);
				int num3 = num;
				ulong num4 = num2;
				_callback(num3, num4);
			}
		}

		// Token: 0x04000F3D RID: 3901
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager ownerManager;

		// Token: 0x04000F3E RID: 3902
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ulong, int> discordIdToEntityId = new Dictionary<ulong, int>();

		// Token: 0x04000F3F RID: 3903
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<int, ulong> entityIdToDiscordId = new Dictionary<int, ulong>();
	}

	// Token: 0x0200032D RID: 813
	public class DiscordUserSettingsManager
	{
		// Token: 0x06001784 RID: 6020 RVA: 0x00087D30 File Offset: 0x00085F30
		public double GetUserVolume(ulong _userId)
		{
			int num;
			if (this.userVolumes.TryGetValue(_userId, out num))
			{
				return (double)num / 100.0;
			}
			return 1.0;
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00087D64 File Offset: 0x00085F64
		public void SetUserVolume(ulong _userId, double _volume)
		{
			int num = Mathf.RoundToInt((float)(_volume * 100.0));
			num = Mathf.Clamp(num, 0, 200);
			if (num >= 99 && num <= 101)
			{
				this.userVolumes.Remove(_userId);
				return;
			}
			this.userVolumes[_userId] = num;
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x00087DB5 File Offset: 0x00085FB5
		public static string DataFilePath
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameIO.GetUserGameDataDir() + "/DiscordUserSettings.dat";
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00087DC8 File Offset: 0x00085FC8
		public static DiscordManager.DiscordUserSettingsManager Load()
		{
			DiscordManager.DiscordUserSettingsManager discordUserSettingsManager = new DiscordManager.DiscordUserSettingsManager();
			if (!SdFile.Exists(DiscordManager.DiscordUserSettingsManager.DataFilePath))
			{
				return discordUserSettingsManager;
			}
			DiscordManager.DiscordUserSettingsManager discordUserSettingsManager2;
			try
			{
				using (Stream stream = SdFile.OpenRead(DiscordManager.DiscordUserSettingsManager.DataFilePath))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						pooledBinaryReader.ReadInt32();
						int num = pooledBinaryReader.ReadInt32();
						for (int i = 0; i < num; i++)
						{
							ulong num2 = pooledBinaryReader.ReadUInt64();
							int num3 = pooledBinaryReader.ReadInt32();
							discordUserSettingsManager.userVolumes[num2] = num3;
						}
						discordUserSettingsManager2 = discordUserSettingsManager;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("[Discord] Failed loading UserSettings file: " + ex.Message);
				Log.Exception(ex);
				discordUserSettingsManager2 = new DiscordManager.DiscordUserSettingsManager();
			}
			return discordUserSettingsManager2;
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00087EB0 File Offset: 0x000860B0
		public void Save()
		{
			using (Stream stream = SdFile.Open(DiscordManager.DiscordUserSettingsManager.DataFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(stream);
					pooledBinaryWriter.Write(1);
					pooledBinaryWriter.Write(this.userVolumes.Count);
					foreach (KeyValuePair<ulong, int> keyValuePair in this.userVolumes)
					{
						ulong num;
						int num2;
						keyValuePair.Deconstruct(out num, out num2);
						ulong num3 = num;
						int num4 = num2;
						pooledBinaryWriter.Write(num3);
						pooledBinaryWriter.Write(num4);
					}
				}
			}
		}

		// Token: 0x04000F40 RID: 3904
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ulong, int> userVolumes = new Dictionary<ulong, int>();
	}

	// Token: 0x0200032E RID: 814
	public class FriendsServerList : IServerListInterface
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x00087F97 File Offset: 0x00086197
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing && this.sessionSearchCount > 0;
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00087FAC File Offset: 0x000861AC
		public void Init(IPlatform _owner)
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			this.serverLookup = ((crossplatformPlatform != null) ? crossplatformPlatform.ServerLookupInterface : null);
			if (this.serverLookup == null)
			{
				Log.Warning("[Discord] No crossplatform server lookup interface found, friends session search is not possible");
			}
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00087FD7 File Offset: 0x000861D7
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			if (this.serverLookup == null)
			{
				return;
			}
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00087FE9 File Offset: 0x000861E9
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			if (this.serverLookup == null)
			{
				return;
			}
			Log.Out("[Discord] FriendsServerList starting search");
			this.isRefreshing = true;
			ThreadManager.StartCoroutine(this.refreshFriendsCo());
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00088011 File Offset: 0x00086211
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator refreshFriendsCo()
		{
			while (this.isRefreshing && this.gameServerFoundCallback != null)
			{
				this.users.Clear();
				DiscordManager.Instance.GetFriends(this.users);
				using (HashSet<DiscordManager.DiscordUser>.Enumerator enumerator = this.users.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DiscordManager.DiscordUser discordUser = enumerator.Current;
						Activity activity = discordUser.Activity;
						string text;
						if (activity == null)
						{
							text = null;
						}
						else
						{
							ActivityParty activityParty = activity.Party();
							text = ((activityParty != null) ? activityParty.Id() : null);
						}
						string text2 = text;
						if (!string.IsNullOrEmpty(text2))
						{
							GameServerInfo gameServerInfo = new GameServerInfo();
							gameServerInfo.SetValue(GameInfoString.UniqueId, text2);
							Interlocked.Increment(ref this.sessionSearchCount);
							this.serverLookup.GetSingleServerDetails(gameServerInfo, EServerRelationType.Friends, new GameServerFoundCallback(this.OnServerFound));
						}
					}
					goto IL_00EB;
				}
				goto IL_00D4;
				IL_00EB:
				if (this.sessionSearchCount <= 0)
				{
					yield return new WaitForSeconds(3f);
					continue;
				}
				IL_00D4:
				yield return null;
				goto IL_00EB;
			}
			yield break;
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00088020 File Offset: 0x00086220
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerFound(IPlatform _sourcePlatform, GameServerInfo _info, EServerRelationType _source)
		{
			Interlocked.Decrement(ref this.sessionSearchCount);
			if (_info == null || this.gameServerFoundCallback == null)
			{
				return;
			}
			_info.IsFriends = true;
			GameServerFoundCallback gameServerFoundCallback = this.gameServerFoundCallback;
			if (gameServerFoundCallback == null)
			{
				return;
			}
			gameServerFoundCallback(_sourcePlatform, _info, _source);
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x00088054 File Offset: 0x00086254
		public void StopSearch()
		{
			this.isRefreshing = false;
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0008805D File Offset: 0x0008625D
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000F41 RID: 3905
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x04000F42 RID: 3906
		[PublicizedFrom(EAccessModifier.Private)]
		public IServerListInterface serverLookup;

		// Token: 0x04000F43 RID: 3907
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x04000F44 RID: 3908
		[PublicizedFrom(EAccessModifier.Private)]
		public int sessionSearchCount;

		// Token: 0x04000F45 RID: 3909
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();
	}

	// Token: 0x02000330 RID: 816
	public class LobbyInfo
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x000881E8 File Offset: 0x000863E8
		// (set) Token: 0x0600179C RID: 6044 RVA: 0x000881F0 File Offset: 0x000863F0
		public string Secret
		{
			get
			{
				return this.secret;
			}
			set
			{
				if (value == this.secret)
				{
					return;
				}
				this.secret = value;
				DiscordManager.LobbyStateChangedCallback lobbyStateChanged = this.ownerManager.LobbyStateChanged;
				if (lobbyStateChanged == null)
				{
					return;
				}
				lobbyStateChanged(this, this.IsReady, this.IsJoined);
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x0008822A File Offset: 0x0008642A
		public bool IsReady
		{
			get
			{
				return !string.IsNullOrEmpty(this.Secret);
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x0008823A File Offset: 0x0008643A
		public bool IsJoined
		{
			get
			{
				return this.Id > 0UL;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x00088246 File Offset: 0x00086446
		// (set) Token: 0x060017A0 RID: 6048 RVA: 0x0008824E File Offset: 0x0008644E
		public ulong Id
		{
			get
			{
				return this.id;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				if (value == this.id)
				{
					return;
				}
				this.id = value;
				DiscordManager.LobbyStateChangedCallback lobbyStateChanged = this.ownerManager.LobbyStateChanged;
				if (lobbyStateChanged == null)
				{
					return;
				}
				lobbyStateChanged(this, this.IsReady, this.IsJoined);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x00088283 File Offset: 0x00086483
		public DiscordManager.CallInfo VoiceCall
		{
			get
			{
				return this.voiceCall;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x0008828B File Offset: 0x0008648B
		public bool IsInVoice
		{
			get
			{
				return this.voiceCall.IsJoined;
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00088298 File Offset: 0x00086498
		public LobbyInfo(DiscordManager _ownerManager, DiscordManager.ELobbyType _lobbyType)
		{
			this.ownerManager = _ownerManager;
			this.LobbyType = _lobbyType;
			this.voiceCall = new DiscordManager.CallInfo(this, this.ownerManager);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000882CC File Offset: 0x000864CC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void Finalize()
		{
			try
			{
				LobbyHandle lobbyHandle = this.handle;
				if (lobbyHandle != null)
				{
					lobbyHandle.Dispose();
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00088304 File Offset: 0x00086504
		public void Join(bool _errorWithoutSecret = true)
		{
			if (this.IsJoined)
			{
				Log.Warning("[Discord] Lobby.Join failed, already in lobby");
				return;
			}
			if (string.IsNullOrEmpty(this.Secret))
			{
				if (_errorWithoutSecret)
				{
					Log.Error("[Discord] Lobby.Join failed, no secret set");
				}
				return;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
			dictionary2["GameSession"] = gameServerInfo.GetValue(GameInfoString.UniqueId);
			dictionary2["Platform"] = ((PlatformManager.CrossplatformPlatform != null) ? PlatformManager.CrossplatformPlatform.PlatformIdentifier.ToStringCached<EPlatformIdentifier>() : PlatformManager.NativePlatform.PlatformIdentifier.ToStringCached<EPlatformIdentifier>());
			Log.Out(string.Concat(new string[]
			{
				"[Discord] CreateOrJoinLobby: ",
				this.Secret,
				", session=",
				dictionary2["GameSession"],
				", platform=",
				dictionary2["Platform"]
			}));
			this.ownerManager.client.CreateOrJoinLobbyWithMetadata(this.Secret, dictionary2, dictionary, delegate(ClientResult _result, ulong _lobbyId)
			{
				DiscordManager.logCallbackInfoWithClientResult("CreateOrJoinLobbyResult", string.Format("lobbyId={0}", _lobbyId), _result, false);
				if (_result.Type() != ErrorType.None)
				{
					_result.Dispose();
					return;
				}
				this.Id = _lobbyId;
				this.handle = this.ownerManager.client.GetLobbyHandle(_lobbyId);
				this.UpdateMembers();
				_result.Dispose();
			});
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00088424 File Offset: 0x00086624
		public void Leave(bool _manual = true)
		{
			if (this.IsInVoice)
			{
				this.VoiceCall.Leave(_manual);
			}
			if (this.IsJoined)
			{
				this.ownerManager.client.LeaveLobby(this.Id, delegate(ClientResult _result)
				{
					DiscordManager.logCallbackInfoWithClientResult("LeaveLobby", string.Format("type={0}", this.LobbyType), _result, true);
				});
			}
			this.Id = 0UL;
			LobbyHandle lobbyHandle = this.handle;
			if (lobbyHandle != null)
			{
				lobbyHandle.Dispose();
			}
			this.handle = null;
			this.UpdateMembers();
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00088498 File Offset: 0x00086698
		public void UpdateMembers()
		{
			this.lobbyMembers.Clear();
			if (this.IsJoined)
			{
				foreach (ulong num in this.handle.LobbyMemberIds())
				{
					this.ownerManager.GetUser(num);
					this.lobbyMembers.Add(num);
				}
			}
			DiscordManager.LobbyMembersChangedCallback lobbyMembersChanged = this.ownerManager.LobbyMembersChanged;
			if (lobbyMembersChanged == null)
			{
				return;
			}
			lobbyMembersChanged(this);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00088506 File Offset: 0x00086706
		public bool HasMember(ulong _userId)
		{
			return this.lobbyMembers.Contains(_userId);
		}

		// Token: 0x04000F49 RID: 3913
		[PublicizedFrom(EAccessModifier.Private)]
		public const string LobbyMetadataKeyGameSession = "GameSession";

		// Token: 0x04000F4A RID: 3914
		[PublicizedFrom(EAccessModifier.Private)]
		public const string LobbyMetadataKeyPlatform = "Platform";

		// Token: 0x04000F4B RID: 3915
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager ownerManager;

		// Token: 0x04000F4C RID: 3916
		public readonly DiscordManager.ELobbyType LobbyType;

		// Token: 0x04000F4D RID: 3917
		[PublicizedFrom(EAccessModifier.Private)]
		public string secret;

		// Token: 0x04000F4E RID: 3918
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong id;

		// Token: 0x04000F4F RID: 3919
		[PublicizedFrom(EAccessModifier.Private)]
		public LobbyHandle handle;

		// Token: 0x04000F50 RID: 3920
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager.CallInfo voiceCall;

		// Token: 0x04000F51 RID: 3921
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<ulong> lobbyMembers = new HashSet<ulong>();
	}

	// Token: 0x02000331 RID: 817
	public class PresenceManager
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x00088599 File Offset: 0x00086799
		public bool JoinableActivitySet
		{
			get
			{
				return !string.IsNullOrEmpty(this.joinSecretJson);
			}
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000885AC File Offset: 0x000867AC
		public PresenceManager(DiscordManager _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00088618 File Offset: 0x00086818
		[PublicizedFrom(EAccessModifier.Private)]
		public ActivityAssets initActivity()
		{
			this.activity = new Activity();
			this.activity.SetName("7 Days To Die");
			this.activity.SetType(ActivityTypes.Playing);
			ActivityAssets activityAssets = new ActivityAssets();
			activityAssets.SetLargeImage("7dtd");
			return activityAssets;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00088654 File Offset: 0x00086854
		public void RegisterDiscordCallbacks()
		{
			this.owner.client.SetActivityInviteCreatedCallback(new Discord.Sdk.Client.ActivityInviteCallback(this.OnActivityInviteCreated));
			this.owner.client.SetActivityInviteUpdatedCallback(new Discord.Sdk.Client.ActivityInviteCallback(this.OnActivityInviteUpdated));
			this.owner.client.SetActivityJoinCallback(new Discord.Sdk.Client.ActivityJoinCallback(this.OnActivityJoin));
			this.SetRichPresenceState(new IRichPresence.PresenceStates?(IRichPresence.PresenceStates.Menu));
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x000886C4 File Offset: 0x000868C4
		public void SetRichPresenceState(IRichPresence.PresenceStates? _state = null)
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			IRichPresence.PresenceStates presenceStates = _state.GetValueOrDefault();
			if (_state == null)
			{
				presenceStates = this.currentRichPresenceState;
				_state = new IRichPresence.PresenceStates?(presenceStates);
			}
			if (this.currentRichPresenceState == _state.Value)
			{
				IRichPresence.PresenceStates? presenceStates2 = _state;
				presenceStates = IRichPresence.PresenceStates.InGame;
				if ((presenceStates2.GetValueOrDefault() == presenceStates) & (presenceStates2 != null))
				{
					this.refreshRichPresenceData();
				}
				this.sendCurrentRichPresence();
				return;
			}
			this.timeStartedCurrentActivity = (ulong)((_state.Value == IRichPresence.PresenceStates.InGame) ? Utils.CurrentUnixTime : 0U);
			this.joinSecretJson = null;
			this.currentRichPresenceState = _state.Value;
			this.refreshRichPresenceData();
			this.sendCurrentRichPresence();
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00088768 File Offset: 0x00086968
		[PublicizedFrom(EAccessModifier.Private)]
		public void sendCurrentRichPresence()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			if (!this.owner.IsReady)
			{
				return;
			}
			if (this.activity == null)
			{
				return;
			}
			this.owner.client.UpdateRichPresence(this.activity, delegate(ClientResult _result)
			{
				DiscordManager.logCallbackInfoWithClientResult("UpdateRichPresence", null, _result, true);
				DiscordManager.FriendsListChangedCallback friendsListChanged = this.owner.FriendsListChanged;
				if (friendsListChanged != null)
				{
					friendsListChanged();
				}
				Activity activity = this.activity;
				if (activity != null)
				{
					activity.Dispose();
				}
				this.activity = null;
			});
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x000887B8 File Offset: 0x000869B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void refreshRichPresenceData()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			using (ActivityAssets activityAssets = this.initActivity())
			{
				this.setTimestamps();
				this.setDetailsAndState();
				this.setLargeImageAndTooltip(activityAssets);
				this.setSmallImageAndTooltip(activityAssets);
				this.setParty();
				this.setPlatforms();
				this.activity.SetAssets(activityAssets);
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00088824 File Offset: 0x00086A24
		[PublicizedFrom(EAccessModifier.Private)]
		public void setDetailsAndState()
		{
			switch (this.currentRichPresenceState)
			{
			case IRichPresence.PresenceStates.Menu:
				this.activity.SetDetails(Localization.Get("discordPresenceDetailsInMenu", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
				this.activity.SetState(null);
				return;
			case IRichPresence.PresenceStates.Loading:
				this.activity.SetDetails(Localization.Get("discordPresenceDetailsStartingGame", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
				this.activity.SetState(null);
				return;
			case IRichPresence.PresenceStates.Connecting:
				this.activity.SetDetails(Localization.Get("discordPresenceDetailsConnectingToServer", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
				this.activity.SetState(null);
				return;
			case IRichPresence.PresenceStates.InGame:
			{
				World world = GameManager.Instance.World;
				EntityPlayerLocal primaryPlayer = world.GetPrimaryPlayer();
				if (primaryPlayer == null)
				{
					this.activity.SetDetails(null);
					this.activity.SetState(null);
					return;
				}
				if (GameManager.Instance.IsEditMode())
				{
					this.activity.SetDetails(Localization.Get(PrefabEditModeManager.Instance.IsActive() ? "discordPresenceDetailsPoiEditor" : "discordPresenceDetailsWorldEditor", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
					this.activity.SetState(null);
					return;
				}
				Party party = primaryPlayer.Party;
				int num = ((party != null) ? party.MemberList.Count : 1);
				this.activity.SetState(string.Format(Localization.Get((num > 1) ? "discordPresenceStateInParty" : "discordPresenceStateSolo", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage), num));
				if (TwitchManager.HasInstance && TwitchManager.Current.InitState == TwitchManager.InitStates.Ready)
				{
					this.activity.SetDetails(Localization.Get("discordPresenceDetailsTwitchIntegration", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
					return;
				}
				ulong worldTime = world.worldTime;
				int @int = GameStats.GetInt(EnumUtils.Parse<EnumGameStats>("BloodMoonDay", false));
				ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GamePrefs.GetInt(EnumGamePrefs.DayLightLength));
				if (GameUtils.IsBloodMoonTime(worldTime, valueTuple, @int))
				{
					this.activity.SetDetails(Localization.Get("discordPresenceDetailsBloodMoon", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
					return;
				}
				Quest activeQuest = primaryPlayer.QuestJournal.ActiveQuest;
				if (activeQuest != null)
				{
					this.activity.SetDetails(string.Format(Localization.Get("discordPresenceDetailsQuesting", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage), activeQuest.QuestClass.Name));
					return;
				}
				if (DiscordManager.PresenceManager.<setDetailsAndState>g__PlayerAtHome|16_0(primaryPlayer))
				{
					this.activity.SetDetails(Localization.Get("discordPresenceDetailsAtHome", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
					return;
				}
				this.activity.SetDetails(Localization.Get("discordPresenceDetailsExploring", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage));
				return;
			}
			default:
				throw new ArgumentOutOfRangeException("currentRichPresenceState", this.currentRichPresenceState, null);
			}
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00088AA0 File Offset: 0x00086CA0
		[PublicizedFrom(EAccessModifier.Private)]
		public void setTimestamps()
		{
			if (this.timeStartedCurrentActivity <= 0UL)
			{
				this.activity.SetTimestamps(null);
				return;
			}
			using (ActivityTimestamps activityTimestamps = new ActivityTimestamps())
			{
				activityTimestamps.SetStart(this.timeStartedCurrentActivity);
				this.activity.SetTimestamps(activityTimestamps);
			}
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00088B00 File Offset: 0x00086D00
		[PublicizedFrom(EAccessModifier.Private)]
		public void setLargeImageAndTooltip(ActivityAssets _activityAssets)
		{
			DiscordManager.PresenceManager.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1._activityAssets = _activityAssets;
			IRichPresence.PresenceStates presenceStates = this.currentRichPresenceState;
			if (presenceStates <= IRichPresence.PresenceStates.Connecting)
			{
				DiscordManager.PresenceManager.<setLargeImageAndTooltip>g__SetDefaultImage|20_0(ref CS$<>8__locals1);
				return;
			}
			if (presenceStates != IRichPresence.PresenceStates.InGame)
			{
				throw new ArgumentOutOfRangeException("currentRichPresenceState", this.currentRichPresenceState, null);
			}
			if (GameManager.Instance.IsEditMode())
			{
				DiscordManager.PresenceManager.<setLargeImageAndTooltip>g__SetDefaultImage|20_0(ref CS$<>8__locals1);
				return;
			}
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer == null)
			{
				DiscordManager.PresenceManager.<setLargeImageAndTooltip>g__SetDefaultImage|20_0(ref CS$<>8__locals1);
				return;
			}
			BiomeDefinition biomeStandingOn = primaryPlayer.biomeStandingOn;
			if (biomeStandingOn == null)
			{
				DiscordManager.PresenceManager.<setLargeImageAndTooltip>g__SetDefaultImage|20_0(ref CS$<>8__locals1);
				return;
			}
			string sBiomeName = biomeStandingOn.m_sBiomeName;
			ValueTuple<string, string> valueTuple;
			if (!this.biomeNameToAssetsMap.TryGetValue(sBiomeName, out valueTuple))
			{
				string text = (this.supportedBiomeImages.Contains(biomeStandingOn.m_BiomeType) ? ("biome" + biomeStandingOn.m_BiomeType.ToStringCached<BiomeDefinition.BiomeType>().ToLower()) : "7dtd");
				valueTuple = new ValueTuple<string, string>(Localization.Get("biome_" + sBiomeName, false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage), text);
				this.biomeNameToAssetsMap[sBiomeName] = valueTuple;
			}
			CS$<>8__locals1._activityAssets.SetLargeImage(valueTuple.Item2);
			CS$<>8__locals1._activityAssets.SetLargeText(valueTuple.Item1);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00088C34 File Offset: 0x00086E34
		[PublicizedFrom(EAccessModifier.Private)]
		public void setSmallImageAndTooltip(ActivityAssets _activityAssets)
		{
			DiscordManager.PresenceManager.<>c__DisplayClass22_0 CS$<>8__locals1;
			CS$<>8__locals1._activityAssets = _activityAssets;
			IRichPresence.PresenceStates presenceStates = this.currentRichPresenceState;
			if (presenceStates <= IRichPresence.PresenceStates.Connecting)
			{
				DiscordManager.PresenceManager.<setSmallImageAndTooltip>g__ClearImage|22_0(ref CS$<>8__locals1);
				return;
			}
			if (presenceStates != IRichPresence.PresenceStates.InGame)
			{
				throw new ArgumentOutOfRangeException("currentRichPresenceState", this.currentRichPresenceState, null);
			}
			if (GameManager.Instance.IsEditMode())
			{
				DiscordManager.PresenceManager.<setSmallImageAndTooltip>g__ClearImage|22_0(ref CS$<>8__locals1);
				return;
			}
			World world = GameManager.Instance.World;
			ulong worldTime = world.worldTime;
			int bloodMoonWarningHour = World.BloodMoonWarningHour;
			ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(worldTime);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			int @int = GameStats.GetInt(EnumUtils.Parse<EnumGameStats>("BloodMoonDay", false));
			ValueTuple<int, int> valueTuple2 = GameUtils.CalcDuskDawnHours(GamePrefs.GetInt(EnumGamePrefs.DayLightLength));
			bool flag = world.IsDaytime();
			bool flag2 = GameUtils.IsBloodMoonTime(worldTime, valueTuple2, @int);
			bool flag3 = !flag2 && bloodMoonWarningHour != -1 && GameStats.GetInt(EnumGameStats.BloodMoonDay) == item && bloodMoonWarningHour <= item2;
			string text = ValueDisplayFormatters.WorldTime(worldTime, DiscordManager.PresenceManager.dayTimeFormatString);
			if (flag2)
			{
				CS$<>8__locals1._activityAssets.SetSmallImage("statebloodmoon");
			}
			else if (flag3)
			{
				CS$<>8__locals1._activityAssets.SetSmallImage("statebloodmoonwarning");
			}
			else if (flag)
			{
				CS$<>8__locals1._activityAssets.SetSmallImage("stateday");
			}
			else
			{
				CS$<>8__locals1._activityAssets.SetSmallImage("statenight");
			}
			CS$<>8__locals1._activityAssets.SetSmallText(text);
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00088D78 File Offset: 0x00086F78
		[PublicizedFrom(EAccessModifier.Private)]
		public void setParty()
		{
			if (this.currentRichPresenceState != IRichPresence.PresenceStates.InGame)
			{
				this.activity.SetParty(null);
				this.activity.SetSecrets(null);
				return;
			}
			GameServerInfo currentGameServerInfoServerOrClient = SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient;
			string value = currentGameServerInfoServerOrClient.GetValue(GameInfoString.UniqueId);
			if (string.IsNullOrEmpty(value))
			{
				this.activity.SetParty(null);
				this.activity.SetSecrets(null);
				return;
			}
			using (ActivityParty activityParty = new ActivityParty())
			{
				activityParty.SetId(value);
				activityParty.SetCurrentSize(GameManager.Instance.World.Players.Count);
				activityParty.SetMaxSize(currentGameServerInfoServerOrClient.GetValue(GameInfoInt.MaxPlayers));
				this.activity.SetParty(activityParty);
				if (string.IsNullOrEmpty(this.joinSecretJson))
				{
					this.joinSecretJson = JsonConvert.SerializeObject(new DiscordManager.PresenceManager.ActivitySecret(currentGameServerInfoServerOrClient));
				}
				using (ActivitySecrets activitySecrets = new ActivitySecrets())
				{
					activitySecrets.SetJoin(this.joinSecretJson);
					this.activity.SetSecrets(activitySecrets);
				}
			}
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00088E8C File Offset: 0x0008708C
		[PublicizedFrom(EAccessModifier.Private)]
		public void setPlatforms()
		{
			if (this.currentRichPresenceState != IRichPresence.PresenceStates.InGame)
			{
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.CurrentGameServerInfoServerOrClient.GetValue(GameInfoBool.AllowCrossplay))
			{
				this.activity.SetSupportedPlatforms(ActivityGamePlatforms.Desktop | ActivityGamePlatforms.Xbox | ActivityGamePlatforms.PS5);
				return;
			}
			Activity activity = this.activity;
			ActivityGamePlatforms activityGamePlatforms;
			switch (PlatformManager.NativePlatform.PlatformIdentifier)
			{
			case EPlatformIdentifier.None:
				throw new ArgumentException("None", "SetSupportedPlatforms");
			case EPlatformIdentifier.Local:
				activityGamePlatforms = ActivityGamePlatforms.Desktop;
				break;
			case EPlatformIdentifier.EOS:
				throw new ArgumentException("EOS", "SetSupportedPlatforms");
			case EPlatformIdentifier.Steam:
				activityGamePlatforms = ActivityGamePlatforms.Desktop;
				break;
			case EPlatformIdentifier.XBL:
				activityGamePlatforms = ActivityGamePlatforms.Xbox;
				break;
			case EPlatformIdentifier.PSN:
				activityGamePlatforms = ActivityGamePlatforms.PS5;
				break;
			case EPlatformIdentifier.EGS:
				activityGamePlatforms = ActivityGamePlatforms.Desktop;
				break;
			case EPlatformIdentifier.LAN:
				throw new ArgumentException("LAN", "SetSupportedPlatforms");
			case EPlatformIdentifier.Count:
				throw new ArgumentException("Count", "SetSupportedPlatforms");
			default:
				throw new ArgumentOutOfRangeException();
			}
			activity.SetSupportedPlatforms(activityGamePlatforms);
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00088F6A File Offset: 0x0008716A
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnActivityInviteCreated(ActivityInvite _invite)
		{
			this.handleInvite(true, _invite);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00088F74 File Offset: 0x00087174
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnActivityInviteUpdated(ActivityInvite _invite)
		{
			this.handleInvite(false, _invite);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00088F80 File Offset: 0x00087180
		[PublicizedFrom(EAccessModifier.Private)]
		public void handleInvite(bool _created, ActivityInvite _invite)
		{
			DiscordManager.logCallbackInfo(string.Format("{0}: sender={1}/type={2}/party={3}/message={4}/valid={5}", new object[]
			{
				_created ? "OnActivityInviteCreated" : "OnActivityInviteUpdated",
				_invite.SenderId(),
				_invite.Type().ToStringCached<ActivityActionTypes>(),
				_invite.PartyId(),
				_invite.MessageId(),
				_invite.IsValid()
			}), LogType.Log);
			DiscordManager.DiscordUser user = this.owner.GetUser(_invite.SenderId());
			ActivityActionTypes activityActionTypes = _invite.Type();
			bool flag = _invite.IsValid();
			if (activityActionTypes != ActivityActionTypes.Join)
			{
				if (activityActionTypes == ActivityActionTypes.JoinRequest)
				{
					user.PendingIncomingJoinRequest = flag;
					_invite.Dispose();
					return;
				}
				return;
			}
			else
			{
				if (!user.PendingOutgoingJoinRequest)
				{
					user.SetIncomingInviteActivity(flag ? _invite : null);
					return;
				}
				if (flag)
				{
					user.AcceptInvite(_invite);
					return;
				}
				_invite.Dispose();
				return;
			}
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00089054 File Offset: 0x00087254
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnActivityJoin(string _joinSecret)
		{
			DiscordManager.logCallbackInfo("OnActivityJoin", LogType.Log);
			DiscordManager.PresenceManager.ActivitySecret activitySecret;
			try
			{
				activitySecret = JsonConvert.DeserializeObject<DiscordManager.PresenceManager.ActivitySecret>(_joinSecret);
			}
			catch (JsonException ex)
			{
				Log.Error("[Discord] Failed reading invite secret:");
				Log.Exception(ex);
				return;
			}
			DiscordManager.ActivityJoiningCallback activityJoining = this.owner.ActivityJoining;
			if (activityJoining != null)
			{
				activityJoining();
			}
			DiscordInviteListener.ListenerInstance.SetPendingInvite(activitySecret.SessionID, activitySecret.Password);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0008912C File Offset: 0x0008732C
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static bool <setDetailsAndState>g__PlayerAtHome|16_0(EntityPlayerLocal _player)
		{
			SpawnPosition spawnPoint = _player.GetSpawnPoint();
			return (!spawnPoint.IsUndef() && (spawnPoint.position - _player.position).sqrMagnitude <= 2500f) || GameManager.Instance.World.GetLandClaimOwnerInParty(_player, _player.persistentPlayerData);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00089181 File Offset: 0x00087381
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <setLargeImageAndTooltip>g__SetDefaultImage|20_0(ref DiscordManager.PresenceManager.<>c__DisplayClass20_0 A_0)
		{
			A_0._activityAssets.SetLargeImage("7dtd");
			A_0._activityAssets.SetLargeText(null);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0008919F File Offset: 0x0008739F
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <setSmallImageAndTooltip>g__ClearImage|22_0(ref DiscordManager.PresenceManager.<>c__DisplayClass22_0 A_0)
		{
			A_0._activityAssets.SetSmallImage(null);
			A_0._activityAssets.SetSmallText(null);
		}

		// Token: 0x04000F52 RID: 3922
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly string discordPresenceLocalizationLanguage = "english";

		// Token: 0x04000F53 RID: 3923
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly DiscordManager owner;

		// Token: 0x04000F54 RID: 3924
		[PublicizedFrom(EAccessModifier.Private)]
		public IRichPresence.PresenceStates currentRichPresenceState = IRichPresence.PresenceStates.InGame;

		// Token: 0x04000F55 RID: 3925
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong timeStartedCurrentActivity = (ulong)Utils.CurrentUnixTime;

		// Token: 0x04000F56 RID: 3926
		[PublicizedFrom(EAccessModifier.Private)]
		public string joinSecretJson;

		// Token: 0x04000F57 RID: 3927
		[PublicizedFrom(EAccessModifier.Private)]
		public Activity activity;

		// Token: 0x04000F58 RID: 3928
		[PublicizedFrom(EAccessModifier.Private)]
		public const string DefaultLargeImageName = "7dtd";

		// Token: 0x04000F59 RID: 3929
		[TupleElementNames(new string[] { "label", "image" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, ValueTuple<string, string>> biomeNameToAssetsMap = new Dictionary<string, ValueTuple<string, string>>();

		// Token: 0x04000F5A RID: 3930
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<BiomeDefinition.BiomeType> supportedBiomeImages = new HashSet<BiomeDefinition.BiomeType>
		{
			BiomeDefinition.BiomeType.burnt_forest,
			BiomeDefinition.BiomeType.Desert,
			BiomeDefinition.BiomeType.PineForest,
			BiomeDefinition.BiomeType.Snow,
			BiomeDefinition.BiomeType.Wasteland
		};

		// Token: 0x04000F5B RID: 3931
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly string dayTimeFormatString = Localization.Get("xuiDay", false, DiscordManager.PresenceManager.discordPresenceLocalizationLanguage) + " {0}, {1:00}:{2:00}";

		// Token: 0x02000332 RID: 818
		[JsonObject(MemberSerialization.Fields)]
		[PublicizedFrom(EAccessModifier.Private)]
		public class ActivitySecret
		{
			// Token: 0x060017C1 RID: 6081 RVA: 0x000891BC File Offset: 0x000873BC
			public ActivitySecret(GameServerInfo _gsi)
			{
				this.SessionID = _gsi.GetValue(GameInfoString.UniqueId);
				this.ServerIP = _gsi.GetValue(GameInfoString.IP);
				this.ServerPort = _gsi.GetValue(GameInfoInt.Port);
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					this.Password = GamePrefs.GetString(EnumGamePrefs.ServerPassword);
					return;
				}
				this.Password = ServerInfoCache.Instance.GetPassword(_gsi) ?? "";
			}

			// Token: 0x04000F5C RID: 3932
			[JsonProperty("ID")]
			public readonly string SessionID;

			// Token: 0x04000F5D RID: 3933
			[JsonProperty("IP")]
			public string ServerIP;

			// Token: 0x04000F5E RID: 3934
			[JsonProperty("Port")]
			public int ServerPort;

			// Token: 0x04000F5F RID: 3935
			[JsonProperty("PW")]
			public string Password;
		}
	}
}
