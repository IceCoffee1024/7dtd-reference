using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001CD3 RID: 7379
	public class LANServerList : IServerListInterface
	{
		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x0600DB0A RID: 56074 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x0600DB0B RID: 56075 RVA: 0x004E4363 File Offset: 0x004E2563
		public bool IsRefreshing
		{
			get
			{
				return this.shouldRefresh;
			}
		}

		// Token: 0x0600DB0C RID: 56076 RVA: 0x004E436B File Offset: 0x004E256B
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DB0D RID: 56077 RVA: 0x004E4374 File Offset: 0x004E2574
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			this.serverFoundCallback = _serverFound;
		}

		// Token: 0x0600DB0E RID: 56078 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DB0F RID: 56079 RVA: 0x004E4380 File Offset: 0x004E2580
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

		// Token: 0x0600DB10 RID: 56080 RVA: 0x004E4410 File Offset: 0x004E2610
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

		// Token: 0x0600DB11 RID: 56081 RVA: 0x004E441F File Offset: 0x004E261F
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

		// Token: 0x0600DB12 RID: 56082 RVA: 0x004E4430 File Offset: 0x004E2630
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

		// Token: 0x0600DB13 RID: 56083 RVA: 0x004E4494 File Offset: 0x004E2694
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRulesRequestDone(bool _success, string _message, GameServerInfo _gsi)
		{
			this.cacheControl.SetUpdated(_gsi.GetValue(GameInfoString.IP), _gsi.GetValue(GameInfoInt.Port));
			_gsi.IsLAN = true;
			this.serverFoundCallback(this.owner, _gsi, EServerRelationType.LAN);
		}

		// Token: 0x0600DB14 RID: 56084 RVA: 0x004E44C9 File Offset: 0x004E26C9
		public void StopSearch()
		{
			this.isPaused = true;
			this.cacheControl.Clear();
		}

		// Token: 0x0600DB15 RID: 56085 RVA: 0x004E44E0 File Offset: 0x004E26E0
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

		// Token: 0x0400A590 RID: 42384
		[PublicizedFrom(EAccessModifier.Private)]
		public const int serverBroadcastIntervalSeconds = 5;

		// Token: 0x0400A591 RID: 42385
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan rulesRefreshInterval = new TimeSpan(0, 2, 0);

		// Token: 0x0400A592 RID: 42386
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan knownServerTimeout = new TimeSpan(0, 1, 0);

		// Token: 0x0400A593 RID: 42387
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A594 RID: 42388
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback serverFoundCallback;

		// Token: 0x0400A595 RID: 42389
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPAddress multicastGroupIp;

		// Token: 0x0400A596 RID: 42390
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A597 RID: 42391
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldRefresh;

		// Token: 0x0400A598 RID: 42392
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPaused;

		// Token: 0x0400A599 RID: 42393
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine requestCoroutine;

		// Token: 0x0400A59A RID: 42394
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine receiveCoroutine;

		// Token: 0x0400A59B RID: 42395
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientSendHandler sendHandler;

		// Token: 0x0400A59C RID: 42396
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A59D RID: 42397
		[PublicizedFrom(EAccessModifier.Private)]
		public LANServerCacheControl cacheControl = new LANServerCacheControl(LANServerList.rulesRefreshInterval, LANServerList.knownServerTimeout);

		// Token: 0x0400A59E RID: 42398
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];
	}
}
