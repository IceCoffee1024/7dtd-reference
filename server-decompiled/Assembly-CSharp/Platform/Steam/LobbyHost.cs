using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CF6 RID: 7414
	public class LobbyHost : ILobbyHost
	{
		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x0600DB6C RID: 56172 RVA: 0x004E6437 File Offset: 0x004E4637
		// (set) Token: 0x0600DB6D RID: 56173 RVA: 0x004E643F File Offset: 0x004E463F
		public string LobbyId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = string.Empty;

		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x0600DB6E RID: 56174 RVA: 0x004E6448 File Offset: 0x004E4648
		public bool IsInLobby
		{
			get
			{
				return this.CurrentLobby.m_SteamID > 0UL;
			}
		}

		// Token: 0x17001BDA RID: 7130
		// (get) Token: 0x0600DB6F RID: 56175 RVA: 0x00020119 File Offset: 0x0001E319
		public bool AllowClientLobby
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x0600DB70 RID: 56176 RVA: 0x004E6459 File Offset: 0x004E4659
		// (set) Token: 0x0600DB71 RID: 56177 RVA: 0x004E6461 File Offset: 0x004E4661
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

		// Token: 0x0600DB72 RID: 56178 RVA: 0x004E6480 File Offset: 0x004E4680
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

		// Token: 0x0600DB73 RID: 56179 RVA: 0x004E64A0 File Offset: 0x004E46A0
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

		// Token: 0x0600DB74 RID: 56180 RVA: 0x004E64F9 File Offset: 0x004E46F9
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

		// Token: 0x0600DB75 RID: 56181 RVA: 0x004E652A File Offset: 0x004E472A
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

		// Token: 0x0600DB76 RID: 56182 RVA: 0x004E6569 File Offset: 0x004E4769
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

		// Token: 0x0600DB77 RID: 56183 RVA: 0x004E65A4 File Offset: 0x004E47A4
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

		// Token: 0x0600DB78 RID: 56184 RVA: 0x004E6650 File Offset: 0x004E4850
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

		// Token: 0x0600DB79 RID: 56185 RVA: 0x004E66AC File Offset: 0x004E48AC
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

		// Token: 0x0600DB7A RID: 56186 RVA: 0x004E6890 File Offset: 0x004E4A90
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

		// Token: 0x0600DB7B RID: 56187 RVA: 0x004E68F0 File Offset: 0x004E4AF0
		[PublicizedFrom(EAccessModifier.Private)]
		public void LobbyEnter_Callback(LobbyEnter_t _val)
		{
			Log.Out("[Steamworks.NET] Lobby entered: " + _val.m_ulSteamIDLobby.ToString());
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected && this.CurrentLobby != CSteamID.Nil)
			{
				this.StartGameWithLobby(new CSteamID(_val.m_ulSteamIDLobby));
			}
		}

		// Token: 0x0600DB7C RID: 56188 RVA: 0x004E6947 File Offset: 0x004E4B47
		[PublicizedFrom(EAccessModifier.Private)]
		public void Lobby_JoinRequested(GameLobbyJoinRequested_t _val)
		{
			Log.Out("[Steamworks.NET] LobbyJoinRequested");
			this.PassLobbyToInviteListener(_val.m_steamIDLobby);
		}

		// Token: 0x0600DB7D RID: 56189 RVA: 0x004E6960 File Offset: 0x004E4B60
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

		// Token: 0x0600DB7E RID: 56190 RVA: 0x004E69BC File Offset: 0x004E4BBC
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

		// Token: 0x0400A64E RID: 42574
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A650 RID: 42576
		[PublicizedFrom(EAccessModifier.Private)]
		public CSteamID currentLobby = CSteamID.Nil;

		// Token: 0x0400A651 RID: 42577
		[PublicizedFrom(EAccessModifier.Private)]
		public int lobbyCreationAttempts;

		// Token: 0x0400A652 RID: 42578
		[PublicizedFrom(EAccessModifier.Private)]
		public float timeLastWorldTimeUpdate;

		// Token: 0x0400A653 RID: 42579
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo gameServerInfo;

		// Token: 0x0400A654 RID: 42580
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyCreated_t> m_LobbyCreated;

		// Token: 0x0400A655 RID: 42581
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyEnter_t> m_LobbyEnter;

		// Token: 0x0400A656 RID: 42582
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameLobbyJoinRequested_t> m_gameLobbyJoinRequested;

		// Token: 0x0400A657 RID: 42583
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A658 RID: 42584
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong lobbyJoinRequestForId;
	}
}
