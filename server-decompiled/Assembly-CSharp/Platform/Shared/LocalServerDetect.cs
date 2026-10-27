using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001D0C RID: 7436
	public class LocalServerDetect : IServerListInterface
	{
		// Token: 0x0600DC3C RID: 56380 RVA: 0x004E9E70 File Offset: 0x004E8070
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

		// Token: 0x17001BF5 RID: 7157
		// (get) Token: 0x0600DC3D RID: 56381 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DC3E RID: 56382 RVA: 0x004E9E91 File Offset: 0x004E8091
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer || this.initDone)
			{
				return;
			}
			this.owner = _owner;
		}

		// Token: 0x0600DC3F RID: 56383 RVA: 0x004E9EAA File Offset: 0x004E80AA
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001BF6 RID: 7158
		// (get) Token: 0x0600DC40 RID: 56384 RVA: 0x004E9EB3 File Offset: 0x004E80B3
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600DC41 RID: 56385 RVA: 0x004E9EBB File Offset: 0x004E80BB
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			this.isRefreshing = true;
			if (this.detectCoroutine == null)
			{
				this.detectCoroutine = ThreadManager.StartCoroutine(this.detectLocalServers());
			}
		}

		// Token: 0x0600DC42 RID: 56386 RVA: 0x004E9EDD File Offset: 0x004E80DD
		public void StopSearch()
		{
			this.isRefreshing = false;
			this.detectCoroutine = null;
		}

		// Token: 0x0600DC43 RID: 56387 RVA: 0x004E9EED File Offset: 0x004E80ED
		public void Disconnect()
		{
			this.isRefreshing = false;
		}

		// Token: 0x0600DC44 RID: 56388 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DC45 RID: 56389 RVA: 0x004E9EF6 File Offset: 0x004E80F6
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

		// Token: 0x0600DC46 RID: 56390 RVA: 0x004E9F05 File Offset: 0x004E8105
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

		// Token: 0x0400A6CB RID: 42699
		[PublicizedFrom(EAccessModifier.Private)]
		public bool initDone;

		// Token: 0x0400A6CC RID: 42700
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A6CD RID: 42701
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A6CE RID: 42702
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds refreshInterval = new WaitForSeconds(3f);

		// Token: 0x0400A6CF RID: 42703
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A6D0 RID: 42704
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine detectCoroutine;
	}
}
