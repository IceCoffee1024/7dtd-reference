using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001C93 RID: 7315
	public class LobbyHost : ILobbyHost
	{
		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x0600D8DE RID: 55518 RVA: 0x004DDD4F File Offset: 0x004DBF4F
		// (set) Token: 0x0600D8DF RID: 55519 RVA: 0x004DDD57 File Offset: 0x004DBF57
		public string LobbyId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = string.Empty;

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x0600D8E0 RID: 55520 RVA: 0x004DDD60 File Offset: 0x004DBF60
		public bool IsInLobby
		{
			get
			{
				return this.CurrentLobby.m_SteamID > 0UL;
			}
		}

		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x0600D8E1 RID: 55521 RVA: 0x00020021 File Offset: 0x0001E221
		public bool AllowClientLobby
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x0600D8E2 RID: 55522 RVA: 0x004DDD71 File Offset: 0x004DBF71
		// (set) Token: 0x0600D8E3 RID: 55523 RVA: 0x004DDD79 File Offset: 0x004DBF79
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

		// Token: 0x0600D8E4 RID: 55524 RVA: 0x004DDD98 File Offset: 0x004DBF98
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

		// Token: 0x0600D8E5 RID: 55525 RVA: 0x004DDDB8 File Offset: 0x004DBFB8
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

		// Token: 0x0600D8E6 RID: 55526 RVA: 0x004DDE11 File Offset: 0x004DC011
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

		// Token: 0x0600D8E7 RID: 55527 RVA: 0x004DDE42 File Offset: 0x004DC042
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

		// Token: 0x0600D8E8 RID: 55528 RVA: 0x004DDE81 File Offset: 0x004DC081
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

		// Token: 0x0600D8E9 RID: 55529 RVA: 0x004DDEBC File Offset: 0x004DC0BC
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

		// Token: 0x0600D8EA RID: 55530 RVA: 0x004DDF68 File Offset: 0x004DC168
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

		// Token: 0x0600D8EB RID: 55531 RVA: 0x004DDFC4 File Offset: 0x004DC1C4
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

		// Token: 0x0600D8EC RID: 55532 RVA: 0x004DE1A8 File Offset: 0x004DC3A8
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

		// Token: 0x0600D8ED RID: 55533 RVA: 0x004DE208 File Offset: 0x004DC408
		[PublicizedFrom(EAccessModifier.Private)]
		public void LobbyEnter_Callback(LobbyEnter_t _val)
		{
			Log.Out("[Steamworks.NET] Lobby entered: " + _val.m_ulSteamIDLobby.ToString());
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected && this.CurrentLobby != CSteamID.Nil)
			{
				this.StartGameWithLobby(new CSteamID(_val.m_ulSteamIDLobby));
			}
		}

		// Token: 0x0600D8EE RID: 55534 RVA: 0x004DE25F File Offset: 0x004DC45F
		[PublicizedFrom(EAccessModifier.Private)]
		public void Lobby_JoinRequested(GameLobbyJoinRequested_t _val)
		{
			Log.Out("[Steamworks.NET] LobbyJoinRequested");
			this.PassLobbyToInviteListener(_val.m_steamIDLobby);
		}

		// Token: 0x0600D8EF RID: 55535 RVA: 0x004DE278 File Offset: 0x004DC478
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

		// Token: 0x0600D8F0 RID: 55536 RVA: 0x004DE2D4 File Offset: 0x004DC4D4
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

		// Token: 0x0400A490 RID: 42128
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A492 RID: 42130
		[PublicizedFrom(EAccessModifier.Private)]
		public CSteamID currentLobby = CSteamID.Nil;

		// Token: 0x0400A493 RID: 42131
		[PublicizedFrom(EAccessModifier.Private)]
		public int lobbyCreationAttempts;

		// Token: 0x0400A494 RID: 42132
		[PublicizedFrom(EAccessModifier.Private)]
		public float timeLastWorldTimeUpdate;

		// Token: 0x0400A495 RID: 42133
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo gameServerInfo;

		// Token: 0x0400A496 RID: 42134
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyCreated_t> m_LobbyCreated;

		// Token: 0x0400A497 RID: 42135
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyEnter_t> m_LobbyEnter;

		// Token: 0x0400A498 RID: 42136
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameLobbyJoinRequested_t> m_gameLobbyJoinRequested;

		// Token: 0x0400A499 RID: 42137
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A49A RID: 42138
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong lobbyJoinRequestForId;
	}
}
