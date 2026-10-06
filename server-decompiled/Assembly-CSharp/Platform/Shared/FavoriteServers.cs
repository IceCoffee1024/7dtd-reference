using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CD9 RID: 7385
	public class FavoriteServers : IServerListInterface
	{
		// Token: 0x0600DB15 RID: 56085 RVA: 0x004E606C File Offset: 0x004E426C
		public FavoriteServers()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Application.wantsToQuit += delegate
			{
				this.Disconnect();
				return true;
			};
		}

		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x0600DB16 RID: 56086 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DB17 RID: 56087 RVA: 0x004E608D File Offset: 0x004E428D
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer || this.initDone)
			{
				return;
			}
			this.owner = _owner;
		}

		// Token: 0x0600DB18 RID: 56088 RVA: 0x004E60A6 File Offset: 0x004E42A6
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x0600DB19 RID: 56089 RVA: 0x004E60AF File Offset: 0x004E42AF
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DB1A RID: 56090 RVA: 0x004E60B7 File Offset: 0x004E42B7
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			this.isRefreshing = true;
			if (this.detectCoroutine == null)
			{
				this.detectCoroutine = ThreadManager.StartCoroutine(this.detectFavoriteServers());
			}
		}

		// Token: 0x0600DB1B RID: 56091 RVA: 0x004E60D9 File Offset: 0x004E42D9
		public void StopSearch()
		{
			this.isRefreshing = false;
			this.detectCoroutine = null;
		}

		// Token: 0x0600DB1C RID: 56092 RVA: 0x004E60E9 File Offset: 0x004E42E9
		public void Disconnect()
		{
			this.isRefreshing = false;
		}

		// Token: 0x0600DB1D RID: 56093 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DB1E RID: 56094 RVA: 0x004E60F2 File Offset: 0x004E42F2
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator detectFavoriteServers()
		{
			while (this.isRefreshing)
			{
				Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>.Enumerator dictEnumerator = ServerInfoCache.Instance.GetFavoriteServersEnumerator();
				bool flag = dictEnumerator.MoveNext();
				while (flag && this.isRefreshing)
				{
					KeyValuePair<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue> keyValuePair = dictEnumerator.Current;
					GameServerInfo gameServerInfo = new GameServerInfo();
					gameServerInfo.SetValue(GameInfoString.IP, keyValuePair.Key.Address);
					gameServerInfo.SetValue(GameInfoInt.Port, keyValuePair.Key.Port);
					gameServerInfo.IsFavorite = keyValuePair.Value.IsFavorite;
					gameServerInfo.LastPlayedLinux = (int)keyValuePair.Value.LastPlayedTime;
					ServerInformationTcpClient.RequestRules(gameServerInfo, true, new ServerInformationTcpClient.RulesRequestDone(this.callback));
					yield return FavoriteServers.serverCheckInterval;
					try
					{
						flag = dictEnumerator.MoveNext();
					}
					catch (InvalidOperationException)
					{
						flag = false;
					}
				}
				dictEnumerator.Dispose();
				yield return FavoriteServers.refreshInterval;
				dictEnumerator = default(Dictionary<ServerInfoCache.FavoritesHistoryKey, ServerInfoCache.FavoritesHistoryValue>.Enumerator);
			}
			this.detectCoroutine = null;
			yield break;
		}

		// Token: 0x0600DB1F RID: 56095 RVA: 0x004E6101 File Offset: 0x004E4301
		[PublicizedFrom(EAccessModifier.Private)]
		public void callback(bool _success, string _message, GameServerInfo _gsi)
		{
			if (!this.isRefreshing || !_success)
			{
				return;
			}
			GameServerFoundCallback gameServerFoundCallback = this.gameServerFoundCallback;
			if (gameServerFoundCallback == null)
			{
				return;
			}
			gameServerFoundCallback(this.owner, _gsi, _gsi.IsFavorite ? EServerRelationType.Favorites : EServerRelationType.History);
		}

		// Token: 0x0400A5D7 RID: 42455
		[PublicizedFrom(EAccessModifier.Private)]
		public bool initDone;

		// Token: 0x0400A5D8 RID: 42456
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5D9 RID: 42457
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A5DA RID: 42458
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds refreshInterval = new WaitForSeconds(3f);

		// Token: 0x0400A5DB RID: 42459
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds serverCheckInterval = new WaitForSeconds(0.1f);

		// Token: 0x0400A5DC RID: 42460
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A5DD RID: 42461
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine detectCoroutine;
	}
}
