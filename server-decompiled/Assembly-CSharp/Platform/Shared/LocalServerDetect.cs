using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Shared
{
	// Token: 0x02001CA9 RID: 7337
	public class LocalServerDetect : IServerListInterface
	{
		// Token: 0x0600D9AE RID: 55726 RVA: 0x004E1788 File Offset: 0x004DF988
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

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x0600D9AF RID: 55727 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D9B0 RID: 55728 RVA: 0x004E17A9 File Offset: 0x004DF9A9
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer || this.initDone)
			{
				return;
			}
			this.owner = _owner;
		}

		// Token: 0x0600D9B1 RID: 55729 RVA: 0x004E17C2 File Offset: 0x004DF9C2
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _errorCallback)
		{
			this.gameServerFoundCallback = _serverFound;
		}

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x0600D9B2 RID: 55730 RVA: 0x004E17CB File Offset: 0x004DF9CB
		public bool IsRefreshing
		{
			get
			{
				return this.isRefreshing;
			}
		}

		// Token: 0x0600D9B3 RID: 55731 RVA: 0x004E17D3 File Offset: 0x004DF9D3
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			this.isRefreshing = true;
			if (this.detectCoroutine == null)
			{
				this.detectCoroutine = ThreadManager.StartCoroutine(this.detectLocalServers());
			}
		}

		// Token: 0x0600D9B4 RID: 55732 RVA: 0x004E17F5 File Offset: 0x004DF9F5
		public void StopSearch()
		{
			this.isRefreshing = false;
			this.detectCoroutine = null;
		}

		// Token: 0x0600D9B5 RID: 55733 RVA: 0x004E1805 File Offset: 0x004DFA05
		public void Disconnect()
		{
			this.isRefreshing = false;
		}

		// Token: 0x0600D9B6 RID: 55734 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D9B7 RID: 55735 RVA: 0x004E180E File Offset: 0x004DFA0E
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

		// Token: 0x0600D9B8 RID: 55736 RVA: 0x004E181D File Offset: 0x004DFA1D
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

		// Token: 0x0400A50D RID: 42253
		[PublicizedFrom(EAccessModifier.Private)]
		public bool initDone;

		// Token: 0x0400A50E RID: 42254
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A50F RID: 42255
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback gameServerFoundCallback;

		// Token: 0x0400A510 RID: 42256
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly WaitForSeconds refreshInterval = new WaitForSeconds(3f);

		// Token: 0x0400A511 RID: 42257
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRefreshing;

		// Token: 0x0400A512 RID: 42258
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine detectCoroutine;
	}
}
