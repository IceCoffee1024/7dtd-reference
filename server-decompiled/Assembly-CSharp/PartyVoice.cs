using System;
using System.Collections.Generic;
using Platform;

// Token: 0x020008E0 RID: 2272
public class PartyVoice
{
	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x0600412A RID: 16682 RVA: 0x0019972A File Offset: 0x0019792A
	public static PartyVoice Instance
	{
		get
		{
			PartyVoice partyVoice;
			if ((partyVoice = PartyVoice.instance) == null)
			{
				partyVoice = (PartyVoice.instance = new PartyVoice());
			}
			return partyVoice;
		}
	}

	// Token: 0x0600412B RID: 16683 RVA: 0x00199740 File Offset: 0x00197940
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyVoice()
	{
		this.platformPartyVoice = PlatformManager.MultiPlatform.PartyVoice;
		if (this.platformPartyVoice != null)
		{
			this.platformPartyVoice.Initialized += this.OnPlatformPartyVoiceInitialized;
		}
	}

	// Token: 0x0600412C RID: 16684 RVA: 0x00199790 File Offset: 0x00197990
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPlatformPartyVoiceInitialized()
	{
		this.platformPartyVoiceInitialized = true;
		this.platformPartyVoice.OnRemotePlayerStateChanged += this.PlatformPartyVoice_OnRemotePlayerStateChanged;
		this.platformPartyVoice.OnRemotePlayerVoiceStateChanged += this.PlatformPartyVoice_OnRemotePlayerVoiceStateChanged;
		GameManager.Instance.OnLocalPlayerChanged += this.localPlayerChangedEvent;
		World world = GameManager.Instance.World;
		EntityPlayerLocal entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		if (entityPlayerLocal != null)
		{
			this.gameStarted(entityPlayerLocal);
		}
		PlatformUserManager.BlockedStateChanged += this.playerBlockStateChanged;
		this.gamePrefChanged(EnumGamePrefs.OptionsVoiceVolumeLevel);
		this.gamePrefChanged(EnumGamePrefs.OptionsVoiceInputDevice);
		this.gamePrefChanged(EnumGamePrefs.OptionsVoiceOutputDevice);
		GamePrefs.OnGamePrefChanged += this.gamePrefChanged;
	}

	// Token: 0x0600412D RID: 16685 RVA: 0x0019984F File Offset: 0x00197A4F
	[PublicizedFrom(EAccessModifier.Private)]
	public void localPlayerChangedEvent(EntityPlayerLocal _newLocalPlayer)
	{
		if (_newLocalPlayer == null)
		{
			this.gameEnded();
			return;
		}
		this.gameStarted(_newLocalPlayer);
	}

	// Token: 0x0600412E RID: 16686 RVA: 0x00199868 File Offset: 0x00197A68
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameStarted(EntityPlayerLocal _newLocalPlayer)
	{
		if (PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.OfflineMode)
		{
			return;
		}
		this.localPlayer = _newLocalPlayer;
		this.localPlayer.PartyJoined += this.playerJoinedParty;
		this.localPlayer.PartyChanged += this.playerJoinedParty;
		this.localPlayer.PartyLeave += this.playerLeftParty;
	}

	// Token: 0x0600412F RID: 16687 RVA: 0x001998D4 File Offset: 0x00197AD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void gameEnded()
	{
		if (this.localPlayer != null)
		{
			this.localPlayer.PartyJoined -= this.playerJoinedParty;
			this.localPlayer.PartyChanged -= this.playerJoinedParty;
			this.localPlayer.PartyLeave -= this.playerLeftParty;
			this.localPlayer = null;
		}
		this.platformPartyVoice.LeaveLobby();
		this.playerVoiceStates.Clear();
	}

	// Token: 0x06004130 RID: 16688 RVA: 0x00199954 File Offset: 0x00197B54
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerJoinedParty(Party _affectedParty, EntityPlayer _player)
	{
		bool flag = _affectedParty.Leader == this.localPlayer;
		if (!this.platformPartyVoice.InLobbyOrProgress)
		{
			if (flag)
			{
				this.platformPartyVoice.CreateLobby(delegate(string _lobbyId)
				{
					if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.SetVoiceLobby, _player.entityId, _player.entityId, null, _lobbyId), false);
						return;
					}
					Party.ServerHandleSetVoiceLoby(_player, _lobbyId);
				});
				return;
			}
			if (!string.IsNullOrEmpty(_affectedParty.VoiceLobbyId))
			{
				this.platformPartyVoice.JoinLobby(_affectedParty.VoiceLobbyId);
				return;
			}
		}
		else if (this.platformPartyVoice.InLobby && this.platformPartyVoice.IsLobbyOwner() && !flag)
		{
			this.promoteLeader(_affectedParty);
		}
	}

	// Token: 0x06004131 RID: 16689 RVA: 0x001999EC File Offset: 0x00197BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerLeftParty(Party _affectedParty, EntityPlayer _player)
	{
		int num = ((_affectedParty == null || _affectedParty.LeaderIndex < 0 || _affectedParty.LeaderIndex > 8 || _affectedParty.MemberList.Count == 0) ? 1 : 0);
		bool flag = this.platformPartyVoice.IsLobbyOwner();
		if (num == 0 && flag)
		{
			this.promoteLeader(_affectedParty);
		}
		this.platformPartyVoice.LeaveLobby();
		this.playerVoiceStates.Clear();
	}

	// Token: 0x06004132 RID: 16690 RVA: 0x00199A50 File Offset: 0x00197C50
	[PublicizedFrom(EAccessModifier.Private)]
	public void promoteLeader(Party _affectedParty)
	{
		int entityId = _affectedParty.Leader.entityId;
		PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(entityId);
		if (playerDataFromEntityID == null)
		{
			Log.Error(string.Format("[Voice] Can not promote lobby owner, no persistent data for party leader {0}", entityId));
			return;
		}
		this.platformPartyVoice.PromoteLeader(playerDataFromEntityID.PrimaryId);
	}

	// Token: 0x06004133 RID: 16691 RVA: 0x00199AA4 File Offset: 0x00197CA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlatformPartyVoice_OnRemotePlayerStateChanged(PlatformUserIdentifierAbs _userIdentifier, IPartyVoice.EVoiceChannelAction _memberChannelAction)
	{
		IPlatformUserData orCreate = PlatformUserManager.GetOrCreate(_userIdentifier);
		if (_memberChannelAction == IPartyVoice.EVoiceChannelAction.Joined)
		{
			this.playerVoiceStates[_userIdentifier] = IPartyVoice.EVoiceMemberState.Normal;
			this.platformPartyVoice.BlockUser(orCreate.PrimaryId, orCreate.Blocked[EBlockType.VoiceChat].IsBlocked());
			return;
		}
		if (_memberChannelAction != IPartyVoice.EVoiceChannelAction.Left)
		{
			return;
		}
		this.playerVoiceStates.Remove(_userIdentifier);
	}

	// Token: 0x06004134 RID: 16692 RVA: 0x00199AFD File Offset: 0x00197CFD
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlatformPartyVoice_OnRemotePlayerVoiceStateChanged(PlatformUserIdentifierAbs _userIdentifier, IPartyVoice.EVoiceMemberState _voiceState)
	{
		this.playerVoiceStates[_userIdentifier] = _voiceState;
	}

	// Token: 0x06004135 RID: 16693 RVA: 0x00199B0C File Offset: 0x00197D0C
	public IPartyVoice.EVoiceMemberState GetVoiceMemberState(PlatformUserIdentifierAbs _userIdentifier)
	{
		IPartyVoice.EVoiceMemberState evoiceMemberState;
		if (!this.playerVoiceStates.TryGetValue(_userIdentifier, out evoiceMemberState))
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		return evoiceMemberState;
	}

	// Token: 0x06004136 RID: 16694 RVA: 0x00199B2C File Offset: 0x00197D2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerBlockStateChanged(IPlatformUserData _ppd, EBlockType _blockType, EUserBlockState _blockState)
	{
		if (_blockType != EBlockType.VoiceChat || !this.playerVoiceStates.ContainsKey(_ppd.PrimaryId))
		{
			return;
		}
		this.platformPartyVoice.BlockUser(_ppd.PrimaryId, _blockState.IsBlocked());
	}

	// Token: 0x06004137 RID: 16695 RVA: 0x00199B60 File Offset: 0x00197D60
	[PublicizedFrom(EAccessModifier.Private)]
	public void gamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.OptionsVoiceVolumeLevel)
		{
			this.platformPartyVoice.OutputVolume = GamePrefs.GetFloat(EnumGamePrefs.OptionsVoiceVolumeLevel);
			return;
		}
		if (_pref == EnumGamePrefs.OptionsVoiceInputDevice)
		{
			this.platformPartyVoice.SetInputDevice(GamePrefs.GetString(EnumGamePrefs.OptionsVoiceInputDevice));
			return;
		}
		if (_pref != EnumGamePrefs.OptionsVoiceOutputDevice)
		{
			return;
		}
		this.platformPartyVoice.SetOutputDevice(GamePrefs.GetString(EnumGamePrefs.OptionsVoiceOutputDevice));
	}

	// Token: 0x06004138 RID: 16696 RVA: 0x00199BC4 File Offset: 0x00197DC4
	public void Update()
	{
		if (!this.platformPartyVoiceInitialized)
		{
			return;
		}
		if (this.localPlayer == null)
		{
			return;
		}
		if (!this.platformPartyVoice.InLobby)
		{
			return;
		}
		this.platformPartyVoice.MuteSelf = !VoiceHelpers.PlatformVoiceEnabled || !VoiceHelpers.PushToTalkPressed();
		this.platformPartyVoice.MuteOthers = !VoiceHelpers.PlatformVoiceEnabled;
	}

	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x06004139 RID: 16697 RVA: 0x00199C28 File Offset: 0x00197E28
	public bool InVoice
	{
		get
		{
			if (!this.platformPartyVoiceInitialized)
			{
				return false;
			}
			if (this.localPlayer == null)
			{
				return false;
			}
			LocalPlayerUI playerUI = this.localPlayer.PlayerUI;
			return !(playerUI == null) && playerUI.playerInput != null && this.platformPartyVoice.InLobby;
		}
	}

	// Token: 0x170006FE RID: 1790
	// (get) Token: 0x0600413A RID: 16698 RVA: 0x00199C79 File Offset: 0x00197E79
	public bool SendingVoice
	{
		get
		{
			return this.InVoice && !this.platformPartyVoice.MuteSelf;
		}
	}

	// Token: 0x04003514 RID: 13588
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyVoice instance;

	// Token: 0x04003515 RID: 13589
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPartyVoice platformPartyVoice;

	// Token: 0x04003516 RID: 13590
	[PublicizedFrom(EAccessModifier.Private)]
	public bool platformPartyVoiceInitialized;

	// Token: 0x04003517 RID: 13591
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04003518 RID: 13592
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> playerVoiceStates = new Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState>();
}
