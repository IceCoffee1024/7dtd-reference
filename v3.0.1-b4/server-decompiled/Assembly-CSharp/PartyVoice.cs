using System;
using System.Collections.Generic;
using Platform;

// Token: 0x020008C5 RID: 2245
public class PartyVoice
{
	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x0600410C RID: 16652 RVA: 0x00196496 File Offset: 0x00194696
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

	// Token: 0x0600410D RID: 16653 RVA: 0x001964AC File Offset: 0x001946AC
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyVoice()
	{
		this.platformPartyVoice = PlatformManager.MultiPlatform.PartyVoice;
		if (this.platformPartyVoice != null)
		{
			this.platformPartyVoice.Initialized += this.OnPlatformPartyVoiceInitialized;
		}
	}

	// Token: 0x0600410E RID: 16654 RVA: 0x001964FC File Offset: 0x001946FC
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

	// Token: 0x0600410F RID: 16655 RVA: 0x001965BB File Offset: 0x001947BB
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

	// Token: 0x06004110 RID: 16656 RVA: 0x001965D4 File Offset: 0x001947D4
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

	// Token: 0x06004111 RID: 16657 RVA: 0x00196640 File Offset: 0x00194840
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

	// Token: 0x06004112 RID: 16658 RVA: 0x001966C0 File Offset: 0x001948C0
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

	// Token: 0x06004113 RID: 16659 RVA: 0x00196758 File Offset: 0x00194958
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

	// Token: 0x06004114 RID: 16660 RVA: 0x001967BC File Offset: 0x001949BC
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

	// Token: 0x06004115 RID: 16661 RVA: 0x00196810 File Offset: 0x00194A10
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

	// Token: 0x06004116 RID: 16662 RVA: 0x00196869 File Offset: 0x00194A69
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlatformPartyVoice_OnRemotePlayerVoiceStateChanged(PlatformUserIdentifierAbs _userIdentifier, IPartyVoice.EVoiceMemberState _voiceState)
	{
		this.playerVoiceStates[_userIdentifier] = _voiceState;
	}

	// Token: 0x06004117 RID: 16663 RVA: 0x00196878 File Offset: 0x00194A78
	public IPartyVoice.EVoiceMemberState GetVoiceMemberState(PlatformUserIdentifierAbs _userIdentifier)
	{
		IPartyVoice.EVoiceMemberState evoiceMemberState;
		if (!this.playerVoiceStates.TryGetValue(_userIdentifier, out evoiceMemberState))
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		return evoiceMemberState;
	}

	// Token: 0x06004118 RID: 16664 RVA: 0x00196898 File Offset: 0x00194A98
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerBlockStateChanged(IPlatformUserData _ppd, EBlockType _blockType, EUserBlockState _blockState)
	{
		if (_blockType != EBlockType.VoiceChat || !this.playerVoiceStates.ContainsKey(_ppd.PrimaryId))
		{
			return;
		}
		this.platformPartyVoice.BlockUser(_ppd.PrimaryId, _blockState.IsBlocked());
	}

	// Token: 0x06004119 RID: 16665 RVA: 0x001968CC File Offset: 0x00194ACC
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

	// Token: 0x0600411A RID: 16666 RVA: 0x00196930 File Offset: 0x00194B30
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

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x0600411B RID: 16667 RVA: 0x00196994 File Offset: 0x00194B94
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

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x0600411C RID: 16668 RVA: 0x001969E5 File Offset: 0x00194BE5
	public bool SendingVoice
	{
		get
		{
			return this.InVoice && !this.platformPartyVoice.MuteSelf;
		}
	}

	// Token: 0x0400347F RID: 13439
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyVoice instance;

	// Token: 0x04003480 RID: 13440
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPartyVoice platformPartyVoice;

	// Token: 0x04003481 RID: 13441
	[PublicizedFrom(EAccessModifier.Private)]
	public bool platformPartyVoiceInitialized;

	// Token: 0x04003482 RID: 13442
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04003483 RID: 13443
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> playerVoiceStates = new Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState>();
}
