using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CC5 RID: 7365
	public class LobbyHost : ILobbyHost
	{
		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x0600DA58 RID: 55896 RVA: 0x004E2897 File Offset: 0x004E0A97
		// (set) Token: 0x0600DA59 RID: 55897 RVA: 0x004E289F File Offset: 0x004E0A9F
		public string LobbyId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = string.Empty;

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x0600DA5A RID: 55898 RVA: 0x004E28A8 File Offset: 0x004E0AA8
		public bool IsInLobby
		{
			get
			{
				return this.CurrentLobby.m_SteamID > 0UL;
			}
		}

		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x0600DA5B RID: 55899 RVA: 0x00020119 File Offset: 0x0001E319
		public bool AllowClientLobby
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x0600DA5C RID: 55900 RVA: 0x004E28B9 File Offset: 0x004E0AB9
		// (set) Token: 0x0600DA5D RID: 55901 RVA: 0x004E28C1 File Offset: 0x004E0AC1
		public CSteamID CurrentLobby
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.currentLobby;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this.currentLobby = value;
				this.LobbyId = this.currentLobby.m_SteamID.ToString();
			}
		}

		// Token: 0x0600DA5E RID: 55902 RVA: 0x004E28E0 File Offset: 0x004E0AE0
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			_owner.Api.ClientApiInitialized += delegate
			{
				if (this.m_LobbyCreated == null)
				{
					this.m_gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.Lobby_JoinRequested));
					this.m_lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(new Callback<LobbyDataUpdate_t>.DispatchDelegate(this.Lobby_DataUpdate));
					this.m_LobbyCreated = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(this.LobbyCreated_Callback));
					this.m_LobbyEnter = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(this.LobbyEnter_Callback));
				}
			};
		}

		// Token: 0x0600DA5F RID: 55903 RVA: 0x004E2900 File Offset: 0x004E0B00
		public void JoinLobby(string _lobbyId, Action<LobbyHostJoinResult> _onComplete)
		{
			if (this.CurrentLobby != CSteamID.Nil)
			{
				this.ExitLobby();
			}
			this.gameServerInfo = null;
			ulong num;
			if (StringParsers.TryParseUInt64(_lobbyId, out num))
			{
				this.JoinLobby(num);
			}
			if (_onComplete != null)
			{
				LobbyHostJoinResult lobbyHostJoinResult = new LobbyHostJoinResult
				{
					success = true
				};
				_onComplete(lobbyHostJoinResult);
			}
		}

		// Token: 0x0600DA60 RID: 55904 RVA: 0x004E2959 File Offset: 0x004E0B59
		[PublicizedFrom(EAccessModifier.Private)]
		public void JoinLobby(ulong _steamLobbyId)
		{
			if (_steamLobbyId != CSteamID.Nil.m_SteamID)
			{
				Log.Out("[Steamworks.NET] Joining Lobby");
				this.CurrentLobby = new CSteamID(_steamLobbyId);
				SteamMatchmaking.JoinLobby(this.CurrentLobby);
			}
		}

		// Token: 0x0600DA61 RID: 55905 RVA: 0x004E298A File Offset: 0x004E0B8A
		public void UpdateLobby(GameServerInfo _gameServerInfo)
		{
			if (this.CurrentLobby != CSteamID.Nil)
			{
				this.ExitLobby();
			}
			this.gameServerInfo = null;
			if (!GameManager.IsDedicatedServer && _gameServerInfo != null)
			{
				this.gameServerInfo = _gameServerInfo;
				this.lobbyCreationAttempts = 0;
				this.createLobby();
			}
		}

		// Token: 0x0600DA62 RID: 55906 RVA: 0x004E29C9 File Offset: 0x004E0BC9
		public void ExitLobby()
		{
			Log.Out("[Steamworks.NET] Exiting Lobby");
			if (this.CurrentLobby != CSteamID.Nil)
			{
				SteamMatchmaking.LeaveLobby(this.CurrentLobby);
			}
			this.CurrentLobby = CSteamID.Nil;
			this.gameServerInfo = null;
		}

		// Token: 0x0600DA63 RID: 55907 RVA: 0x004E2A04 File Offset: 0x004E0C04
		public void UpdateGameTimePlayers(ulong _time, int _players)
		{
			if (this.owner.User.UserStatus != EUserStatus.LoggedIn || this.gameServerInfo == null || this.CurrentLobby == CSteamID.Nil)
			{
				return;
			}
			if (Time.unscaledTime - this.timeLastWorldTimeUpdate < 30f)
			{
				return;
			}
			this.timeLastWorldTimeUpdate = Time.unscaledTime;
			SteamMatchmaking.SetLobbyData(this.CurrentLobby, GameInfoString.LevelName.ToStringCached<GameInfoString>(), this.gameServerInfo.GetValue(GameInfoString.LevelName));
			SteamMatchmaking.SetLobbyData(this.CurrentLobby, GameInfoInt.CurrentServerTime.ToStringCached<GameInfoInt>(), _time.ToString());
			SteamMatchmaking.SetLobbyData(this.CurrentLobby, GameInfoInt.CurrentPlayers.ToStringCached<GameInfoInt>(), _players.ToString());
		}

		// Token: 0x0600DA64 RID: 55908 RVA: 0x004E2AB0 File Offset: 0x004E0CB0
		[PublicizedFrom(EAccessModifier.Private)]
		public void PassLobbyToInviteListener(CSteamID _lobbyId)
		{
			foreach (IJoinSessionGameInviteListener joinSessionGameInviteListener in PlatformManager.MultiPlatform.InviteListeners)
			{
				JoinSessionGameInviteListener joinSessionGameInviteListener2 = joinSessionGameInviteListener as JoinSessionGameInviteListener;
				if (joinSessionGameInviteListener2 != null)
				{
					joinSessionGameInviteListener2.SetLobby(_lobbyId);
					break;
				}
			}
		}

		// Token: 0x0600DA65 RID: 55909 RVA: 0x004E2B0C File Offset: 0x004E0D0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void LobbyCreated_Callback(LobbyCreated_t _val)
		{
			this.lobbyCreationAttempts++;
			if (_val.m_eResult == EResult.k_EResultOK && this.gameServerInfo != null)
			{
				Log.Out("[Steamworks.NET] Lobby creation succeeded, LobbyID={0}, server SteamID={1}, server public IP={2}, server port={3}", new object[]
				{
					_val.m_ulSteamIDLobby,
					this.gameServerInfo.GetValue(GameInfoString.SteamID),
					Utils.MaskIp(this.gameServerInfo.GetValue(GameInfoString.IP)),
					this.gameServerInfo.GetValue(GameInfoInt.Port)
				});
				this.CurrentLobby = new CSteamID(_val.m_ulSteamIDLobby);
				foreach (GameInfoString gameInfoString in EnumUtils.Values<GameInfoString>())
				{
					SteamMatchmaking.SetLobbyData(this.CurrentLobby, gameInfoString.ToStringCached<GameInfoString>(), this.gameServerInfo.GetValue(gameInfoString));
				}
				foreach (GameInfoInt gameInfoInt in EnumUtils.Values<GameInfoInt>())
				{
					SteamMatchmaking.SetLobbyData(this.CurrentLobby, gameInfoInt.ToStringCached<GameInfoInt>(), this.gameServerInfo.GetValue(gameInfoInt).ToString());
				}
				using (IEnumerator<GameInfoBool> enumerator3 = EnumUtils.Values<GameInfoBool>().GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						GameInfoBool gameInfoBool = enumerator3.Current;
						SteamMatchmaking.SetLobbyData(this.CurrentLobby, gameInfoBool.ToStringCached<GameInfoBool>(), this.gameServerInfo.GetValue(gameInfoBool).ToString());
					}
					return;
				}
			}
			if (this.lobbyCreationAttempts < 3 && this.gameServerInfo != null)
			{
				this.createLobby();
			}
			Log.Out("[Steamworks.NET] Lobby creation failed: " + _val.m_eResult.ToString());
		}

		// Token: 0x0600DA66 RID: 55910 RVA: 0x004E2CF0 File Offset: 0x004E0EF0
		[PublicizedFrom(EAccessModifier.Private)]
		public void createLobby()
		{
			int value = this.gameServerInfo.GetValue(GameInfoInt.ServerVisibility);
			ELobbyType elobbyType;
			if (value != 1)
			{
				if (value == 2)
				{
					elobbyType = ELobbyType.k_ELobbyTypePublic;
				}
				else
				{
					elobbyType = ELobbyType.k_ELobbyTypePrivate;
				}
			}
			else
			{
				elobbyType = ELobbyType.k_ELobbyTypeFriendsOnly;
			}
			ELobbyType elobbyType2 = elobbyType;
			Log.Out("[Steamworks.NET] Trying to create Lobby (visibility: " + elobbyType2.ToStringCached<ELobbyType>() + ")");
			SteamMatchmaking.CreateLobby(elobbyType2, this.gameServerInfo.GetValue(GameInfoInt.MaxPlayers) + 4);
		}

		// Token: 0x0600DA67 RID: 55911 RVA: 0x004E2D50 File Offset: 0x004E0F50
		[PublicizedFrom(EAccessModifier.Private)]
		public void LobbyEnter_Callback(LobbyEnter_t _val)
		{
			Log.Out("[Steamworks.NET] Lobby entered: " + _val.m_ulSteamIDLobby.ToString());
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected && this.CurrentLobby != CSteamID.Nil)
			{
				this.StartGameWithLobby(new CSteamID(_val.m_ulSteamIDLobby));
			}
		}

		// Token: 0x0600DA68 RID: 55912 RVA: 0x004E2DA7 File Offset: 0x004E0FA7
		[PublicizedFrom(EAccessModifier.Private)]
		public void Lobby_JoinRequested(GameLobbyJoinRequested_t _val)
		{
			Log.Out("[Steamworks.NET] LobbyJoinRequested");
			this.PassLobbyToInviteListener(_val.m_steamIDLobby);
		}

		// Token: 0x0600DA69 RID: 55913 RVA: 0x004E2DC0 File Offset: 0x004E0FC0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Lobby_DataUpdate(LobbyDataUpdate_t _val)
		{
			if (_val.m_ulSteamIDLobby != this.lobbyJoinRequestForId)
			{
				return;
			}
			this.lobbyJoinRequestForId = 0UL;
			Log.Out("[Steamworks.NET] JoinLobby LobbyDataUpdate: " + _val.m_bSuccess.ToString());
			CSteamID csteamID = new CSteamID(_val.m_ulSteamIDLobby);
			if (_val.m_bSuccess != 0)
			{
				this.StartGameWithLobby(csteamID);
			}
		}

		// Token: 0x0600DA6A RID: 55914 RVA: 0x004E2E1C File Offset: 0x004E101C
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartGameWithLobby(CSteamID _lobbyId)
		{
			if (_lobbyId != CSteamID.Nil)
			{
				Log.Out("[Steamworks.NET] Connecting to server from lobby");
				GameServerInfo gameServerInfo = new GameServerInfo();
				int lobbyDataCount = SteamMatchmaking.GetLobbyDataCount(_lobbyId);
				for (int i = 0; i < lobbyDataCount; i++)
				{
					string text;
					string text2;
					if (SteamMatchmaking.GetLobbyDataByIndex(_lobbyId, i, out text, 100, out text2, 200))
					{
						gameServerInfo.ParseAny(text, text2);
					}
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.Connect(gameServerInfo);
				return;
			}
			Log.Warning("[Steamworks.NET] Tried starting a game with an invalid lobby");
		}

		// Token: 0x0400A565 RID: 42341
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A567 RID: 42343
		[PublicizedFrom(EAccessModifier.Private)]
		public CSteamID currentLobby = CSteamID.Nil;

		// Token: 0x0400A568 RID: 42344
		[PublicizedFrom(EAccessModifier.Private)]
		public int lobbyCreationAttempts;

		// Token: 0x0400A569 RID: 42345
		[PublicizedFrom(EAccessModifier.Private)]
		public float timeLastWorldTimeUpdate;

		// Token: 0x0400A56A RID: 42346
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo gameServerInfo;

		// Token: 0x0400A56B RID: 42347
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyCreated_t> m_LobbyCreated;

		// Token: 0x0400A56C RID: 42348
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyEnter_t> m_LobbyEnter;

		// Token: 0x0400A56D RID: 42349
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameLobbyJoinRequested_t> m_gameLobbyJoinRequested;

		// Token: 0x0400A56E RID: 42350
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A56F RID: 42351
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong lobbyJoinRequestForId;
	}
}
