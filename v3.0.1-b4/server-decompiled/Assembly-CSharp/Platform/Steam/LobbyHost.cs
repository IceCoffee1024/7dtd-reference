using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001C6C RID: 7276
	public class LobbyHost : ILobbyHost
	{
		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x0600D74C RID: 55116 RVA: 0x004D7833 File Offset: 0x004D5A33
		// (set) Token: 0x0600D74D RID: 55117 RVA: 0x004D783B File Offset: 0x004D5A3B
		public string LobbyId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = string.Empty;

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x0600D74E RID: 55118 RVA: 0x004D7844 File Offset: 0x004D5A44
		public bool IsInLobby
		{
			get
			{
				return this.CurrentLobby.m_SteamID > 0UL;
			}
		}

		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x0600D74F RID: 55119 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool AllowClientLobby
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x0600D750 RID: 55120 RVA: 0x004D7855 File Offset: 0x004D5A55
		// (set) Token: 0x0600D751 RID: 55121 RVA: 0x004D785D File Offset: 0x004D5A5D
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

		// Token: 0x0600D752 RID: 55122 RVA: 0x004D787C File Offset: 0x004D5A7C
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

		// Token: 0x0600D753 RID: 55123 RVA: 0x004D789C File Offset: 0x004D5A9C
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

		// Token: 0x0600D754 RID: 55124 RVA: 0x004D78F5 File Offset: 0x004D5AF5
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

		// Token: 0x0600D755 RID: 55125 RVA: 0x004D7926 File Offset: 0x004D5B26
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

		// Token: 0x0600D756 RID: 55126 RVA: 0x004D7965 File Offset: 0x004D5B65
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

		// Token: 0x0600D757 RID: 55127 RVA: 0x004D79A0 File Offset: 0x004D5BA0
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

		// Token: 0x0600D758 RID: 55128 RVA: 0x004D7A4C File Offset: 0x004D5C4C
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

		// Token: 0x0600D759 RID: 55129 RVA: 0x004D7AA8 File Offset: 0x004D5CA8
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

		// Token: 0x0600D75A RID: 55130 RVA: 0x004D7C8C File Offset: 0x004D5E8C
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

		// Token: 0x0600D75B RID: 55131 RVA: 0x004D7CEC File Offset: 0x004D5EEC
		[PublicizedFrom(EAccessModifier.Private)]
		public void LobbyEnter_Callback(LobbyEnter_t _val)
		{
			Log.Out("[Steamworks.NET] Lobby entered: " + _val.m_ulSteamIDLobby.ToString());
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected && this.CurrentLobby != CSteamID.Nil)
			{
				this.StartGameWithLobby(new CSteamID(_val.m_ulSteamIDLobby));
			}
		}

		// Token: 0x0600D75C RID: 55132 RVA: 0x004D7D43 File Offset: 0x004D5F43
		[PublicizedFrom(EAccessModifier.Private)]
		public void Lobby_JoinRequested(GameLobbyJoinRequested_t _val)
		{
			Log.Out("[Steamworks.NET] LobbyJoinRequested");
			this.PassLobbyToInviteListener(_val.m_steamIDLobby);
		}

		// Token: 0x0600D75D RID: 55133 RVA: 0x004D7D5C File Offset: 0x004D5F5C
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

		// Token: 0x0600D75E RID: 55134 RVA: 0x004D7DB8 File Offset: 0x004D5FB8
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

		// Token: 0x0400A35E RID: 41822
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A360 RID: 41824
		[PublicizedFrom(EAccessModifier.Private)]
		public CSteamID currentLobby = CSteamID.Nil;

		// Token: 0x0400A361 RID: 41825
		[PublicizedFrom(EAccessModifier.Private)]
		public int lobbyCreationAttempts;

		// Token: 0x0400A362 RID: 41826
		[PublicizedFrom(EAccessModifier.Private)]
		public float timeLastWorldTimeUpdate;

		// Token: 0x0400A363 RID: 41827
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerInfo gameServerInfo;

		// Token: 0x0400A364 RID: 41828
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyCreated_t> m_LobbyCreated;

		// Token: 0x0400A365 RID: 41829
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyEnter_t> m_LobbyEnter;

		// Token: 0x0400A366 RID: 41830
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameLobbyJoinRequested_t> m_gameLobbyJoinRequested;

		// Token: 0x0400A367 RID: 41831
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<LobbyDataUpdate_t> m_lobbyDataUpdate;

		// Token: 0x0400A368 RID: 41832
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong lobbyJoinRequestForId;
	}
}
