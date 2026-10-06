using System;
using System.Collections.Generic;
using Platform;

// Token: 0x020008D6 RID: 2262
public class PartyVoice
{
	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06004107 RID: 16647 RVA: 0x00198682 File Offset: 0x00196882
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

	// Token: 0x06004108 RID: 16648 RVA: 0x00198698 File Offset: 0x00196898
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyVoice()
	{
		this.platformPartyVoice = PlatformManager.MultiPlatform.PartyVoice;
		if (this.platformPartyVoice != null)
		{
			this.platformPartyVoice.Initialized += this.OnPlatformPartyVoiceInitialized;
		}
	}

	// Token: 0x06004109 RID: 16649 RVA: 0x001986E8 File Offset: 0x001968E8
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

	// Token: 0x0600410A RID: 16650 RVA: 0x001987A7 File Offset: 0x001969A7
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

	// Token: 0x0600410B RID: 16651 RVA: 0x001987C0 File Offset: 0x001969C0
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

	// Token: 0x0600410C RID: 16652 RVA: 0x0019882C File Offset: 0x00196A2C
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

	// Token: 0x0600410D RID: 16653 RVA: 0x001988AC File Offset: 0x00196AAC
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

	// Token: 0x0600410E RID: 16654 RVA: 0x00198944 File Offset: 0x00196B44
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

	// Token: 0x0600410F RID: 16655 RVA: 0x001989A8 File Offset: 0x00196BA8
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

	// Token: 0x06004110 RID: 16656 RVA: 0x001989FC File Offset: 0x00196BFC
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

	// Token: 0x06004111 RID: 16657 RVA: 0x00198A55 File Offset: 0x00196C55
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlatformPartyVoice_OnRemotePlayerVoiceStateChanged(PlatformUserIdentifierAbs _userIdentifier, IPartyVoice.EVoiceMemberState _voiceState)
	{
		this.playerVoiceStates[_userIdentifier] = _voiceState;
	}

	// Token: 0x06004112 RID: 16658 RVA: 0x00198A64 File Offset: 0x00196C64
	public IPartyVoice.EVoiceMemberState GetVoiceMemberState(PlatformUserIdentifierAbs _userIdentifier)
	{
		IPartyVoice.EVoiceMemberState evoiceMemberState;
		if (!this.playerVoiceStates.TryGetValue(_userIdentifier, out evoiceMemberState))
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		return evoiceMemberState;
	}

	// Token: 0x06004113 RID: 16659 RVA: 0x00198A84 File Offset: 0x00196C84
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerBlockStateChanged(IPlatformUserData _ppd, EBlockType _blockType, EUserBlockState _blockState)
	{
		if (_blockType != EBlockType.VoiceChat || !this.playerVoiceStates.ContainsKey(_ppd.PrimaryId))
		{
			return;
		}
		this.platformPartyVoice.BlockUser(_ppd.PrimaryId, _blockState.IsBlocked());
	}

	// Token: 0x06004114 RID: 16660 RVA: 0x00198AB8 File Offset: 0x00196CB8
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

	// Token: 0x06004115 RID: 16661 RVA: 0x00198B1C File Offset: 0x00196D1C
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

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06004116 RID: 16662 RVA: 0x00198B80 File Offset: 0x00196D80
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

	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06004117 RID: 16663 RVA: 0x00198BD1 File Offset: 0x00196DD1
	public bool SendingVoice
	{
		get
		{
			return this.InVoice && !this.platformPartyVoice.MuteSelf;
		}
	}

	// Token: 0x040034F4 RID: 13556
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyVoice instance;

	// Token: 0x040034F5 RID: 13557
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPartyVoice platformPartyVoice;

	// Token: 0x040034F6 RID: 13558
	[PublicizedFrom(EAccessModifier.Private)]
	public bool platformPartyVoiceInitialized;

	// Token: 0x040034F7 RID: 13559
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x040034F8 RID: 13560
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> playerVoiceStates = new Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState>();
}
