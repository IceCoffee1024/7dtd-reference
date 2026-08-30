using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001CAC RID: 7340
	public class LANServerList : IServerListInterface
	{
		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x0600D96F RID: 55663 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x0600D970 RID: 55664 RVA: 0x004DDD4F File Offset: 0x004DBF4F
		public bool IsRefreshing
		{
			get
			{
				return this.shouldRefresh;
			}
		}

		// Token: 0x0600D971 RID: 55665 RVA: 0x004DDD57 File Offset: 0x004DBF57
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600D972 RID: 55666 RVA: 0x004DDD60 File Offset: 0x004DBF60
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			this.serverFoundCallback = _serverFound;
		}

		// Token: 0x0600D973 RID: 55667 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D974 RID: 55668 RVA: 0x004DDD6C File Offset: 0x004DBF6C
		public void StartSearch(IList<IServerListInterface.ServerFilter> _activeFilters)
		{
			try
			{
				this.shouldRefresh = true;
				this.isPaused = false;
				this.udpClient = new UdpClient(AddressFamily.InterNetwork);
				this.sendHandler = new UdpClientSendHandler(this.udpClient);
				this.receiveHandler = new UdpClientReceiveHandler(this.udpClient);
				this.requestCoroutine = ThreadManager.StartCoroutine(this.LANServerInfoRequestCoroutine());
				this.receiveCoroutine = ThreadManager.StartCoroutine(this.LANServerInfoReceiveCoroutine());
			}
			catch (Exception ex)
			{
				Log.Error("[LANServerList] Could not start LAN server search");
				Log.Exception(ex);
			}
		}

		// Token: 0x0600D975 RID: 55669 RVA: 0x004DDDFC File Offset: 0x004DBFFC
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator LANServerInfoRequestCoroutine()
		{
			while (this.shouldRefresh)
			{
				IPEndPoint ipendPoint = new IPEndPoint(LANServerSearchConfig.MulticastGroupIp, 11000);
				if (!this.sendHandler.BeginSend(LANServerList.emptyMessage, 0, ipendPoint))
				{
					yield break;
				}
				while (!this.sendHandler.isComplete)
				{
					yield return null;
				}
				yield return new WaitForSeconds(5f);
				while (this.isPaused)
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x0600D976 RID: 55670 RVA: 0x004DDE0B File Offset: 0x004DC00B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator LANServerInfoReceiveCoroutine()
		{
			while (this.shouldRefresh)
			{
				if (!this.receiveHandler.BeginReceive())
				{
					yield break;
				}
				while (!this.receiveHandler.isComplete)
				{
					yield return null;
				}
				while (this.isPaused)
				{
					yield return null;
				}
				IPEndPoint remoteEP = this.receiveHandler.remoteEP;
				byte[] message = this.receiveHandler.message;
				int length = this.receiveHandler.length;
				if (remoteEP != null && message != null && length == 4)
				{
					int num = 0;
					int num2 = StreamUtils.ReadInt32(message, ref num);
					IPEndPoint ipendPoint = remoteEP;
					ipendPoint.Port = num2;
					this.OnServerFound(ipendPoint);
				}
			}
			yield break;
		}

		// Token: 0x0600D977 RID: 55671 RVA: 0x004DDE1C File Offset: 0x004DC01C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnServerFound(IPEndPoint endpoint)
		{
			string text = endpoint.Address.ToString();
			if (!this.cacheControl.IsUpdateRequired(text, endpoint.Port))
			{
				return;
			}
			GameServerInfo gameServerInfo = new GameServerInfo();
			gameServerInfo.SetValue(GameInfoString.IP, endpoint.Address.ToString());
			gameServerInfo.SetValue(GameInfoInt.Port, endpoint.Port);
			ServerInformationTcpClient.RequestRules(gameServerInfo, false, new ServerInformationTcpClient.RulesRequestDone(this.OnRulesRequestDone));
		}

		// Token: 0x0600D978 RID: 55672 RVA: 0x004DDE80 File Offset: 0x004DC080
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRulesRequestDone(bool _success, string _message, GameServerInfo _gsi)
		{
			this.cacheControl.SetUpdated(_gsi.GetValue(GameInfoString.IP), _gsi.GetValue(GameInfoInt.Port));
			_gsi.IsLAN = true;
			this.serverFoundCallback(this.owner, _gsi, EServerRelationType.LAN);
		}

		// Token: 0x0600D979 RID: 55673 RVA: 0x004DDEB5 File Offset: 0x004DC0B5
		public void StopSearch()
		{
			this.isPaused = true;
			this.cacheControl.Clear();
		}

		// Token: 0x0600D97A RID: 55674 RVA: 0x004DDECC File Offset: 0x004DC0CC
		public void Disconnect()
		{
			this.StopSearch();
			this.isPaused = false;
			this.shouldRefresh = false;
			if (this.requestCoroutine != null)
			{
				ThreadManager.StopCoroutine(this.requestCoroutine);
			}
			if (this.receiveCoroutine != null)
			{
				ThreadManager.StopCoroutine(this.receiveCoroutine);
			}
			UdpClient udpClient = this.udpClient;
			if (udpClient != null)
			{
				udpClient.Dispose();
			}
			this.udpClient = null;
		}

		// Token: 0x0400A45D RID: 42077
		[PublicizedFrom(EAccessModifier.Private)]
		public const int serverBroadcastIntervalSeconds = 5;

		// Token: 0x0400A45E RID: 42078
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan rulesRefreshInterval = new TimeSpan(0, 2, 0);

		// Token: 0x0400A45F RID: 42079
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan knownServerTimeout = new TimeSpan(0, 1, 0);

		// Token: 0x0400A460 RID: 42080
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A461 RID: 42081
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback serverFoundCallback;

		// Token: 0x0400A462 RID: 42082
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPAddress multicastGroupIp;

		// Token: 0x0400A463 RID: 42083
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A464 RID: 42084
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldRefresh;

		// Token: 0x0400A465 RID: 42085
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPaused;

		// Token: 0x0400A466 RID: 42086
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine requestCoroutine;

		// Token: 0x0400A467 RID: 42087
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine receiveCoroutine;

		// Token: 0x0400A468 RID: 42088
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientSendHandler sendHandler;

		// Token: 0x0400A469 RID: 42089
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A46A RID: 42090
		[PublicizedFrom(EAccessModifier.Private)]
		public LANServerCacheControl cacheControl = new LANServerCacheControl(LANServerList.rulesRefreshInterval, LANServerList.knownServerTimeout);

		// Token: 0x0400A46B RID: 42091
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];
	}
}
