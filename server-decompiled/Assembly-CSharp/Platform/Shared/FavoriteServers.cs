using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001D0A RID: 7434
	public class FavoriteServers : IServerListInterface
	{
		// Token: 0x0600DC29 RID: 56361 RVA: 0x004E9C0C File Offset: 0x004E7E0C
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

		// Token: 0x17001BF1 RID: 7153
		// (get) Token: 0x0600DC2A RID: 56362 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DC2B RID: 56363 RVA: 0x004E9C2D File Offset: 0x004E7E2D
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer || this.initDone)
			{
				return;
			}
			this.owner = _owner;
		}

		// Token: 0x0600DC2C RID: 56364 RVA: 0x004E9C46 File Offset: 0x004E7E46
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001BF2 RID: 7154
		// (get) Token: 0x0600DC2D RID: 56365 RVA: 0x004E9C4F File Offset: 0x004E7E4F
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DC2E RID: 56366 RVA: 0x004E9C57 File Offset: 0x004E7E57
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			this.isRefreshing = true;
			if (this.detectCoroutine == null)
			{
				this.detectCoroutine = ThreadManager.StartCoroutine(this.detectFavoriteServers());
			}
		}

		// Token: 0x0600DC2F RID: 56367 RVA: 0x004E9C79 File Offset: 0x004E7E79
		public void StopSearch()
		{
			this.isRefreshing = false;
			this.detectCoroutine = null;
		}

		// Token: 0x0600DC30 RID: 56368 RVA: 0x004E9C89 File Offset: 0x004E7E89
		public void Disconnect()
		{
			this.isRefreshing = false;
		}

		// Token: 0x0600DC31 RID: 56369 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DC32 RID: 56370 RVA: 0x004E9C92 File Offset: 0x004E7E92
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

		// Token: 0x0600DC33 RID: 56371 RVA: 0x004E9CA1 File Offset: 0x004E7EA1
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

		// Token: 0x0400A6C0 RID: 42688
		[PublicizedFrom(EAccessModifier.Private)]
		public bool initDone;

		// Token: 0x0400A6C1 RID: 42689
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A6C2 RID: 42690
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A6C3 RID: 42691
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds refreshInterval = new WaitForSeconds(3f);

		// Token: 0x0400A6C4 RID: 42692
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds serverCheckInterval = new WaitForSeconds(0.1f);

		// Token: 0x0400A6C5 RID: 42693
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A6C6 RID: 42694
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine detectCoroutine;
	}
}
