using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CF1 RID: 7409
	public class MasterServerList : IServerListInterface
	{
		// Token: 0x0600DB46 RID: 56134 RVA: 0x004E5C08 File Offset: 0x004E3E08
		public MasterServerList(EServerRelationType _source)
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Application.wantsToQuit += this.OnApplicationQuit;
			this.source = _source;
			this.compatVersionInt = int.Parse(Platform.Steam.Constants.SteamVersionNr.Replace(".", ""));
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600DB47 RID: 56135 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DB48 RID: 56136 RVA: 0x004E5C65 File Offset: 0x004E3E65
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			_owner.Api.ClientApiInitialized += delegate
			{
				if (this.matchmakingServerListResponse == null && !GameManager.IsDedicatedServer)
				{
					this.matchmakingServerListResponse = new ISteamMatchmakingServerListResponse(new ISteamMatchmakingServerListResponse.ServerResponded(this.ServerResponded), new ISteamMatchmakingServerListResponse.ServerFailedToRespond(this.ServerFailedToRespond), new ISteamMatchmakingServerListResponse.RefreshComplete(this.RefreshComplete));
				}
			};
		}

		// Token: 0x0600DB49 RID: 56137 RVA: 0x004E5C85 File Offset: 0x004E3E85
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x0600DB4A RID: 56138 RVA: 0x004E5C8E File Offset: 0x004E3E8E
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DB4B RID: 56139 RVA: 0x004E5C98 File Offset: 0x004E3E98
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			if (this.gameServerFoundCallback == null)
			{
				return;
			}
			if (this.requestHandle != HServerListRequest.Invalid)
			{
				SteamMatchmakingServers.ReleaseRequest(this.requestHandle);
				this.requestHandle = HServerListRequest.Invalid;
			}
			MatchMakingKeyValuePair_t[] array = new MatchMakingKeyValuePair_t[0];
			HServerListRequest hserverListRequest;
			switch (this.source)
			{
			case EServerRelationType.Internet:
				hserverListRequest = SteamMatchmakingServers.RequestInternetServerList((AppId_t)251570U, array, (uint)array.Length, this.matchmakingServerListResponse);
				break;
			case EServerRelationType.LAN:
				hserverListRequest = SteamMatchmakingServers.RequestLANServerList((AppId_t)251570U, this.matchmakingServerListResponse);
				break;
			case EServerRelationType.Friends:
				hserverListRequest = SteamMatchmakingServers.RequestFriendsServerList((AppId_t)251570U, array, (uint)array.Length, this.matchmakingServerListResponse);
				break;
			case EServerRelationType.Favorites:
				hserverListRequest = SteamMatchmakingServers.RequestFavoritesServerList((AppId_t)251570U, array, (uint)array.Length, this.matchmakingServerListResponse);
				break;
			case EServerRelationType.History:
				hserverListRequest = SteamMatchmakingServers.RequestHistoryServerList((AppId_t)251570U, array, (uint)array.Length, this.matchmakingServerListResponse);
				break;
			case EServerRelationType.Spectator:
				hserverListRequest = SteamMatchmakingServers.RequestSpectatorServerList((AppId_t)251570U, array, (uint)array.Length, this.matchmakingServerListResponse);
				break;
			default:
				hserverListRequest = this.requestHandle;
				break;
			}
			this.requestHandle = hserverListRequest;
			this.isRefreshing = true;
		}

		// Token: 0x0600DB4C RID: 56140 RVA: 0x004E5DC3 File Offset: 0x004E3FC3
		public void StopSearch()
		{
			if (this.requestHandle != HServerListRequest.Invalid)
			{
				SteamMatchmakingServers.ReleaseRequest(this.requestHandle);
				this.requestHandle = HServerListRequest.Invalid;
			}
			this.isRefreshing = false;
		}

		// Token: 0x0600DB4D RID: 56141 RVA: 0x004E5DF4 File Offset: 0x004E3FF4
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600DB4E RID: 56142 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DB4F RID: 56143 RVA: 0x004E5E03 File Offset: 0x004E4003
		[PublicizedFrom(EAccessModifier.Private)]
		public bool OnApplicationQuit()
		{
			this.StopSearch();
			return true;
		}

		// Token: 0x0600DB50 RID: 56144 RVA: 0x004E5E0C File Offset: 0x004E400C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ServerResponded(HServerListRequest _hRequest, int _iServer)
		{
			gameserveritem_t serverDetails = SteamMatchmakingServers.GetServerDetails(_hRequest, _iServer);
			if (serverDetails.m_nServerVersion != this.compatVersionInt && this.source != EServerRelationType.Favorites && this.source != EServerRelationType.History && this.source != EServerRelationType.LAN)
			{
				return;
			}
			GameServerInfo gameServerInfo = new GameServerInfo();
			gameServerInfo.SetValue(GameInfoInt.Ping, serverDetails.m_nPing);
			gameServerInfo.SetValue(GameInfoString.IP, NetworkUtils.ToAddr(serverDetails.m_NetAdr.GetIP()));
			gameServerInfo.SetValue(GameInfoInt.Port, (int)serverDetails.m_NetAdr.GetQueryPort());
			gameServerInfo.SetValue(GameInfoString.SteamID, serverDetails.m_steamID.ToString());
			gameServerInfo.SetValue(GameInfoString.UniqueId, serverDetails.m_steamID.ToString());
			gameServerInfo.SetValue(GameInfoString.LevelName, serverDetails.GetMap());
			gameServerInfo.SetValue(GameInfoInt.CurrentPlayers, serverDetails.m_nPlayers);
			gameServerInfo.SetValue(GameInfoInt.MaxPlayers, serverDetails.m_nMaxPlayers);
			gameServerInfo.SetValue(GameInfoBool.IsPasswordProtected, serverDetails.m_bPassword);
			gameServerInfo.SetValue(GameInfoString.GameHost, serverDetails.GetServerName());
			gameServerInfo.LastPlayedLinux = (int)serverDetails.m_ulTimeLastPlayed;
			switch (this.source)
			{
			case EServerRelationType.LAN:
				gameServerInfo.IsLAN = true;
				break;
			case EServerRelationType.Friends:
				gameServerInfo.IsFriends = true;
				break;
			case EServerRelationType.Favorites:
				gameServerInfo.IsFavorite = true;
				break;
			}
			if (NetworkUtils.ParseGameTags(serverDetails.GetGameTags(), gameServerInfo))
			{
				GameServerFoundCallback gameServerFoundCallback = this.gameServerFoundCallback;
				if (gameServerFoundCallback == null)
				{
					return;
				}
				gameServerFoundCallback(this.owner, gameServerInfo, this.source);
			}
		}

		// Token: 0x0600DB51 RID: 56145 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ServerFailedToRespond(HServerListRequest _hRequest, int _iServer)
		{
		}

		// Token: 0x0600DB52 RID: 56146 RVA: 0x004E5F71 File Offset: 0x004E4171
		[PublicizedFrom(EAccessModifier.Private)]
		public void RefreshComplete(HServerListRequest _hRequest, EMatchMakingServerResponse _response)
		{
			ThreadManager.StartCoroutine(this.restartRefreshCo());
		}

		// Token: 0x0600DB53 RID: 56147 RVA: 0x004E5F7F File Offset: 0x004E417F
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator restartRefreshCo()
		{
			yield return new WaitForSeconds(4f);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0400A63A RID: 42554
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A63B RID: 42555
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int compatVersionInt;

		// Token: 0x0400A63C RID: 42556
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EServerRelationType source;

		// Token: 0x0400A63D RID: 42557
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A63E RID: 42558
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A63F RID: 42559
		[PublicizedFrom(EAccessModifier.Private)]
		public ISteamMatchmakingServerListResponse matchmakingServerListResponse;

		// Token: 0x0400A640 RID: 42560
		[PublicizedFrom(EAccessModifier.Private)]
		public HServerListRequest requestHandle = HServerListRequest.Invalid;
	}
}
