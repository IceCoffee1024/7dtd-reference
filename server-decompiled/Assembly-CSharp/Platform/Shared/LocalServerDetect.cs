using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CDB RID: 7387
	public class LocalServerDetect : IServerListInterface
	{
		// Token: 0x0600DB28 RID: 56104 RVA: 0x004E62D0 File Offset: 0x004E44D0
		public LocalServerDetect()
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

		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x0600DB29 RID: 56105 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DB2A RID: 56106 RVA: 0x004E62F1 File Offset: 0x004E44F1
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer || this.initDone)
			{
				return;
			}
			this.owner = _owner;
		}

		// Token: 0x0600DB2B RID: 56107 RVA: 0x004E630A File Offset: 0x004E450A
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x0600DB2C RID: 56108 RVA: 0x004E6313 File Offset: 0x004E4513
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DB2D RID: 56109 RVA: 0x004E631B File Offset: 0x004E451B
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			this.isRefreshing = true;
			if (this.detectCoroutine == null)
			{
				this.detectCoroutine = ThreadManager.StartCoroutine(this.detectLocalServers());
			}
		}

		// Token: 0x0600DB2E RID: 56110 RVA: 0x004E633D File Offset: 0x004E453D
		public void StopSearch()
		{
			this.isRefreshing = false;
			this.detectCoroutine = null;
		}

		// Token: 0x0600DB2F RID: 56111 RVA: 0x004E634D File Offset: 0x004E454D
		public void Disconnect()
		{
			this.isRefreshing = false;
		}

		// Token: 0x0600DB30 RID: 56112 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DB31 RID: 56113 RVA: 0x004E6356 File Offset: 0x004E4556
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator detectLocalServers()
		{
			while (this.isRefreshing)
			{
				GameServerInfo gameServerInfo = new GameServerInfo();
				gameServerInfo.SetValue(GameInfoString.IP, "127.0.0.1");
				gameServerInfo.SetValue(GameInfoInt.Port, 26900);
				ServerInformationTcpClient.RequestRules(gameServerInfo, true, new ServerInformationTcpClient.RulesRequestDone(this.callback));
				yield return LocalServerDetect.refreshInterval;
				GameServerInfo gameServerInfo2 = new GameServerInfo();
				gameServerInfo2.SetValue(GameInfoString.IP, "127.0.0.1");
				gameServerInfo2.SetValue(GameInfoInt.Port, 27020);
				ServerInformationTcpClient.RequestRules(gameServerInfo2, true, new ServerInformationTcpClient.RulesRequestDone(this.callback));
				yield return LocalServerDetect.refreshInterval;
			}
			this.detectCoroutine = null;
			yield break;
		}

		// Token: 0x0600DB32 RID: 56114 RVA: 0x004E6365 File Offset: 0x004E4565
		[PublicizedFrom(EAccessModifier.Private)]
		public void callback(bool _success, string _message, GameServerInfo _gsi)
		{
			if (!this.isRefreshing || !_success)
			{
				return;
			}
			_gsi.IsLAN = true;
			GameServerFoundCallback gameServerFoundCallback = this.gameServerFoundCallback;
			if (gameServerFoundCallback == null)
			{
				return;
			}
			gameServerFoundCallback(this.owner, _gsi, EServerRelationType.LAN);
		}

		// Token: 0x0400A5E2 RID: 42466
		[PublicizedFrom(EAccessModifier.Private)]
		public bool initDone;

		// Token: 0x0400A5E3 RID: 42467
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5E4 RID: 42468
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A5E5 RID: 42469
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds refreshInterval = new WaitForSeconds(3f);

		// Token: 0x0400A5E6 RID: 42470
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A5E7 RID: 42471
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine detectCoroutine;
	}
}
