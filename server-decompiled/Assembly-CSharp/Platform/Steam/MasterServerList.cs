using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CC0 RID: 7360
	public class MasterServerList : IServerListInterface
	{
		// Token: 0x0600DA32 RID: 55858 RVA: 0x004E2068 File Offset: 0x004E0268
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

		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x0600DA33 RID: 55859 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DA34 RID: 55860 RVA: 0x004E20C5 File Offset: 0x004E02C5
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

		// Token: 0x0600DA35 RID: 55861 RVA: 0x004E20E5 File Offset: 0x004E02E5
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x0600DA36 RID: 55862 RVA: 0x004E20EE File Offset: 0x004E02EE
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DA37 RID: 55863 RVA: 0x004E20F8 File Offset: 0x004E02F8
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

		// Token: 0x0600DA38 RID: 55864 RVA: 0x004E2223 File Offset: 0x004E0423
		public void StopSearch()
		{
			if (this.requestHandle != HServerListRequest.Invalid)
			{
				SteamMatchmakingServers.ReleaseRequest(this.requestHandle);
				this.requestHandle = HServerListRequest.Invalid;
			}
			this.isRefreshing = false;
		}

		// Token: 0x0600DA39 RID: 55865 RVA: 0x004E2254 File Offset: 0x004E0454
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600DA3A RID: 55866 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DA3B RID: 55867 RVA: 0x004E2263 File Offset: 0x004E0463
		[PublicizedFrom(EAccessModifier.Private)]
		public bool OnApplicationQuit()
		{
			this.StopSearch();
			return true;
		}

		// Token: 0x0600DA3C RID: 55868 RVA: 0x004E226C File Offset: 0x004E046C
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

		// Token: 0x0600DA3D RID: 55869 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ServerFailedToRespond(HServerListRequest _hRequest, int _iServer)
		{
		}

		// Token: 0x0600DA3E RID: 55870 RVA: 0x004E23D1 File Offset: 0x004E05D1
		[PublicizedFrom(EAccessModifier.Private)]
		public void RefreshComplete(HServerListRequest _hRequest, EMatchMakingServerResponse _response)
		{
			ThreadManager.StartCoroutine(this.restartRefreshCo());
		}

		// Token: 0x0600DA3F RID: 55871 RVA: 0x004E23DF File Offset: 0x004E05DF
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator restartRefreshCo()
		{
			yield return new WaitForSeconds(4f);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0400A551 RID: 42321
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A552 RID: 42322
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int compatVersionInt;

		// Token: 0x0400A553 RID: 42323
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EServerRelationType source;

		// Token: 0x0400A554 RID: 42324
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A555 RID: 42325
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A556 RID: 42326
		[PublicizedFrom(EAccessModifier.Private)]
		public ISteamMatchmakingServerListResponse matchmakingServerListResponse;

		// Token: 0x0400A557 RID: 42327
		[PublicizedFrom(EAccessModifier.Private)]
		public HServerListRequest requestHandle = HServerListRequest.Invalid;
	}
}
