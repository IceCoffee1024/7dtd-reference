using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001C67 RID: 7271
	public class MasterServerList : IServerListInterface
	{
		// Token: 0x0600D726 RID: 55078 RVA: 0x004D7004 File Offset: 0x004D5204
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

		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x0600D727 RID: 55079 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D728 RID: 55080 RVA: 0x004D7061 File Offset: 0x004D5261
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

		// Token: 0x0600D729 RID: 55081 RVA: 0x004D7081 File Offset: 0x004D5281
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x0600D72A RID: 55082 RVA: 0x004D708A File Offset: 0x004D528A
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600D72B RID: 55083 RVA: 0x004D7094 File Offset: 0x004D5294
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

		// Token: 0x0600D72C RID: 55084 RVA: 0x004D71BF File Offset: 0x004D53BF
		public void StopSearch()
		{
			if (this.requestHandle != HServerListRequest.Invalid)
			{
				SteamMatchmakingServers.ReleaseRequest(this.requestHandle);
				this.requestHandle = HServerListRequest.Invalid;
			}
			this.isRefreshing = false;
		}

		// Token: 0x0600D72D RID: 55085 RVA: 0x004D71F0 File Offset: 0x004D53F0
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600D72E RID: 55086 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D72F RID: 55087 RVA: 0x004D71FF File Offset: 0x004D53FF
		[PublicizedFrom(EAccessModifier.Private)]
		public bool OnApplicationQuit()
		{
			this.StopSearch();
			return true;
		}

		// Token: 0x0600D730 RID: 55088 RVA: 0x004D7208 File Offset: 0x004D5408
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

		// Token: 0x0600D731 RID: 55089 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ServerFailedToRespond(HServerListRequest _hRequest, int _iServer)
		{
		}

		// Token: 0x0600D732 RID: 55090 RVA: 0x004D736D File Offset: 0x004D556D
		[PublicizedFrom(EAccessModifier.Private)]
		public void RefreshComplete(HServerListRequest _hRequest, EMatchMakingServerResponse _response)
		{
			ThreadManager.StartCoroutine(this.restartRefreshCo());
		}

		// Token: 0x0600D733 RID: 55091 RVA: 0x004D737B File Offset: 0x004D557B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator restartRefreshCo()
		{
			yield return new WaitForSeconds(4f);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0400A34A RID: 41802
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A34B RID: 41803
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int compatVersionInt;

		// Token: 0x0400A34C RID: 41804
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EServerRelationType source;

		// Token: 0x0400A34D RID: 41805
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A34E RID: 41806
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A34F RID: 41807
		[PublicizedFrom(EAccessModifier.Private)]
		public ISteamMatchmakingServerListResponse matchmakingServerListResponse;

		// Token: 0x0400A350 RID: 41808
		[PublicizedFrom(EAccessModifier.Private)]
		public HServerListRequest requestHandle = HServerListRequest.Invalid;
	}
}
