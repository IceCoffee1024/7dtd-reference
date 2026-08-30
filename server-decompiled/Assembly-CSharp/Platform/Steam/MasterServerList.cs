using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001C8E RID: 7310
	public class MasterServerList : IServerListInterface
	{
		// Token: 0x0600D8B8 RID: 55480 RVA: 0x004DD520 File Offset: 0x004DB720
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

		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x0600D8B9 RID: 55481 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D8BA RID: 55482 RVA: 0x004DD57D File Offset: 0x004DB77D
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

		// Token: 0x0600D8BB RID: 55483 RVA: 0x004DD59D File Offset: 0x004DB79D
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x0600D8BC RID: 55484 RVA: 0x004DD5A6 File Offset: 0x004DB7A6
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600D8BD RID: 55485 RVA: 0x004DD5B0 File Offset: 0x004DB7B0
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

		// Token: 0x0600D8BE RID: 55486 RVA: 0x004DD6DB File Offset: 0x004DB8DB
		public void StopSearch()
		{
			if (this.requestHandle != HServerListRequest.Invalid)
			{
				SteamMatchmakingServers.ReleaseRequest(this.requestHandle);
				this.requestHandle = HServerListRequest.Invalid;
			}
			this.isRefreshing = false;
		}

		// Token: 0x0600D8BF RID: 55487 RVA: 0x004DD70C File Offset: 0x004DB90C
		public void Disconnect()
		{
			this.StopSearch();
			this.gameServerFoundCallback = null;
		}

		// Token: 0x0600D8C0 RID: 55488 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D8C1 RID: 55489 RVA: 0x004DD71B File Offset: 0x004DB91B
		[PublicizedFrom(EAccessModifier.Private)]
		public bool OnApplicationQuit()
		{
			this.StopSearch();
			return true;
		}

		// Token: 0x0600D8C2 RID: 55490 RVA: 0x004DD724 File Offset: 0x004DB924
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

		// Token: 0x0600D8C3 RID: 55491 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ServerFailedToRespond(HServerListRequest _hRequest, int _iServer)
		{
		}

		// Token: 0x0600D8C4 RID: 55492 RVA: 0x004DD889 File Offset: 0x004DBA89
		[PublicizedFrom(EAccessModifier.Private)]
		public void RefreshComplete(HServerListRequest _hRequest, EMatchMakingServerResponse _response)
		{
			ThreadManager.StartCoroutine(this.restartRefreshCo());
		}

		// Token: 0x0600D8C5 RID: 55493 RVA: 0x004DD897 File Offset: 0x004DBA97
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator restartRefreshCo()
		{
			yield return new WaitForSeconds(4f);
			this.StartSearch(null);
			yield break;
		}

		// Token: 0x0400A47C RID: 42108
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A47D RID: 42109
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int compatVersionInt;

		// Token: 0x0400A47E RID: 42110
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EServerRelationType source;

		// Token: 0x0400A47F RID: 42111
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A480 RID: 42112
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A481 RID: 42113
		[PublicizedFrom(EAccessModifier.Private)]
		public ISteamMatchmakingServerListResponse matchmakingServerListResponse;

		// Token: 0x0400A482 RID: 42114
		[PublicizedFrom(EAccessModifier.Private)]
		public HServerListRequest requestHandle = HServerListRequest.Invalid;
	}
}
