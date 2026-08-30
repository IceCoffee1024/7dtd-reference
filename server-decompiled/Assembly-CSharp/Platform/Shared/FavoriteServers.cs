using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CA7 RID: 7335
	public class FavoriteServers : IServerListInterface
	{
		// Token: 0x0600D99B RID: 55707 RVA: 0x004E1524 File Offset: 0x004DF724
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

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x0600D99C RID: 55708 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D99D RID: 55709 RVA: 0x004E1545 File Offset: 0x004DF745
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer || this.initDone)
			{
				return;
			}
			this.owner = _owner;
		}

		// Token: 0x0600D99E RID: 55710 RVA: 0x004E155E File Offset: 0x004DF75E
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x0600D99F RID: 55711 RVA: 0x004E1567 File Offset: 0x004DF767
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600D9A0 RID: 55712 RVA: 0x004E156F File Offset: 0x004DF76F
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			this.isRefreshing = true;
			if (this.detectCoroutine == null)
			{
				this.detectCoroutine = ThreadManager.StartCoroutine(this.detectFavoriteServers());
			}
		}

		// Token: 0x0600D9A1 RID: 55713 RVA: 0x004E1591 File Offset: 0x004DF791
		public void StopSearch()
		{
			this.isRefreshing = false;
			this.detectCoroutine = null;
		}

		// Token: 0x0600D9A2 RID: 55714 RVA: 0x004E15A1 File Offset: 0x004DF7A1
		public void Disconnect()
		{
			this.isRefreshing = false;
		}

		// Token: 0x0600D9A3 RID: 55715 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D9A4 RID: 55716 RVA: 0x004E15AA File Offset: 0x004DF7AA
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

		// Token: 0x0600D9A5 RID: 55717 RVA: 0x004E15B9 File Offset: 0x004DF7B9
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

		// Token: 0x0400A502 RID: 42242
		[PublicizedFrom(EAccessModifier.Private)]
		public bool initDone;

		// Token: 0x0400A503 RID: 42243
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A504 RID: 42244
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A505 RID: 42245
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds refreshInterval = new WaitForSeconds(3f);

		// Token: 0x0400A506 RID: 42246
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds serverCheckInterval = new WaitForSeconds(0.1f);

		// Token: 0x0400A507 RID: 42247
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A508 RID: 42248
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine detectCoroutine;
	}
}
