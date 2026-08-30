using System;
using System.Collections.Generic;
using Platform;

// Token: 0x020008D1 RID: 2257
public class PartyVoice
{
	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06004171 RID: 16753 RVA: 0x0019856E File Offset: 0x0019676E
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

	// Token: 0x06004172 RID: 16754 RVA: 0x00198584 File Offset: 0x00196784
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyVoice()
	{
		this.platformPartyVoice = PlatformManager.MultiPlatform.PartyVoice;
		if (this.platformPartyVoice != null)
		{
			this.platformPartyVoice.Initialized += this.OnPlatformPartyVoiceInitialized;
		}
	}

	// Token: 0x06004173 RID: 16755 RVA: 0x001985D4 File Offset: 0x001967D4
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

	// Token: 0x06004174 RID: 16756 RVA: 0x00198693 File Offset: 0x00196893
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

	// Token: 0x06004175 RID: 16757 RVA: 0x001986AC File Offset: 0x001968AC
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

	// Token: 0x06004176 RID: 16758 RVA: 0x00198718 File Offset: 0x00196918
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

	// Token: 0x06004177 RID: 16759 RVA: 0x00198798 File Offset: 0x00196998
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

	// Token: 0x06004178 RID: 16760 RVA: 0x00198830 File Offset: 0x00196A30
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

	// Token: 0x06004179 RID: 16761 RVA: 0x00198894 File Offset: 0x00196A94
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

	// Token: 0x0600417A RID: 16762 RVA: 0x001988E8 File Offset: 0x00196AE8
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

	// Token: 0x0600417B RID: 16763 RVA: 0x00198941 File Offset: 0x00196B41
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlatformPartyVoice_OnRemotePlayerVoiceStateChanged(PlatformUserIdentifierAbs _userIdentifier, IPartyVoice.EVoiceMemberState _voiceState)
	{
		this.playerVoiceStates[_userIdentifier] = _voiceState;
	}

	// Token: 0x0600417C RID: 16764 RVA: 0x00198950 File Offset: 0x00196B50
	public IPartyVoice.EVoiceMemberState GetVoiceMemberState(PlatformUserIdentifierAbs _userIdentifier)
	{
		IPartyVoice.EVoiceMemberState evoiceMemberState;
		if (!this.playerVoiceStates.TryGetValue(_userIdentifier, out evoiceMemberState))
		{
			return IPartyVoice.EVoiceMemberState.Disabled;
		}
		return evoiceMemberState;
	}

	// Token: 0x0600417D RID: 16765 RVA: 0x00198970 File Offset: 0x00196B70
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerBlockStateChanged(IPlatformUserData _ppd, EBlockType _blockType, EUserBlockState _blockState)
	{
		if (_blockType != EBlockType.VoiceChat || !this.playerVoiceStates.ContainsKey(_ppd.PrimaryId))
		{
			return;
		}
		this.platformPartyVoice.BlockUser(_ppd.PrimaryId, _blockState.IsBlocked());
	}

	// Token: 0x0600417E RID: 16766 RVA: 0x001989A4 File Offset: 0x00196BA4
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

	// Token: 0x0600417F RID: 16767 RVA: 0x00198A08 File Offset: 0x00196C08
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

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06004180 RID: 16768 RVA: 0x00198A6C File Offset: 0x00196C6C
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

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06004181 RID: 16769 RVA: 0x00198ABD File Offset: 0x00196CBD
	public bool SendingVoice
	{
		get
		{
			return this.InVoice && !this.platformPartyVoice.MuteSelf;
		}
	}

	// Token: 0x04003500 RID: 13568
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyVoice instance;

	// Token: 0x04003501 RID: 13569
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IPartyVoice platformPartyVoice;

	// Token: 0x04003502 RID: 13570
	[PublicizedFrom(EAccessModifier.Private)]
	public bool platformPartyVoiceInitialized;

	// Token: 0x04003503 RID: 13571
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x04003504 RID: 13572
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState> playerVoiceStates = new Dictionary<PlatformUserIdentifierAbs, IPartyVoice.EVoiceMemberState>();
}
