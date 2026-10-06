using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001D05 RID: 7429
	public class LANServerList : IServerListInterface
	{
		// Token: 0x17001C05 RID: 7173
		// (get) Token: 0x0600DC85 RID: 56453 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C06 RID: 7174
		// (get) Token: 0x0600DC86 RID: 56454 RVA: 0x004E8EAB File Offset: 0x004E70AB
		public bool IsRefreshing
		{
			get
			{
				return this.shouldRefresh;
			}
		}

		// Token: 0x0600DC87 RID: 56455 RVA: 0x004E8EB3 File Offset: 0x004E70B3
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DC88 RID: 56456 RVA: 0x004E8EBC File Offset: 0x004E70BC
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			this.serverFoundCallback = _serverFound;
		}

		// Token: 0x0600DC89 RID: 56457 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DC8A RID: 56458 RVA: 0x004E8EC8 File Offset: 0x004E70C8
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

		// Token: 0x0600DC8B RID: 56459 RVA: 0x004E8F58 File Offset: 0x004E7158
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

		// Token: 0x0600DC8C RID: 56460 RVA: 0x004E8F67 File Offset: 0x004E7167
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

		// Token: 0x0600DC8D RID: 56461 RVA: 0x004E8F78 File Offset: 0x004E7178
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

		// Token: 0x0600DC8E RID: 56462 RVA: 0x004E8FDC File Offset: 0x004E71DC
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRulesRequestDone(bool _success, string _message, GameServerInfo _gsi)
		{
			this.cacheControl.SetUpdated(_gsi.GetValue(GameInfoString.IP), _gsi.GetValue(GameInfoInt.Port));
			_gsi.IsLAN = true;
			this.serverFoundCallback(this.owner, _gsi, EServerRelationType.LAN);
		}

		// Token: 0x0600DC8F RID: 56463 RVA: 0x004E9011 File Offset: 0x004E7211
		public void StopSearch()
		{
			this.isPaused = true;
			this.cacheControl.Clear();
		}

		// Token: 0x0600DC90 RID: 56464 RVA: 0x004E9028 File Offset: 0x004E7228
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

		// Token: 0x0400A665 RID: 42597
		[PublicizedFrom(EAccessModifier.Private)]
		public const int serverBroadcastIntervalSeconds = 5;

		// Token: 0x0400A666 RID: 42598
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan rulesRefreshInterval = new TimeSpan(0, 2, 0);

		// Token: 0x0400A667 RID: 42599
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan knownServerTimeout = new TimeSpan(0, 1, 0);

		// Token: 0x0400A668 RID: 42600
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A669 RID: 42601
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback serverFoundCallback;

		// Token: 0x0400A66A RID: 42602
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPAddress multicastGroupIp;

		// Token: 0x0400A66B RID: 42603
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A66C RID: 42604
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldRefresh;

		// Token: 0x0400A66D RID: 42605
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPaused;

		// Token: 0x0400A66E RID: 42606
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine requestCoroutine;

		// Token: 0x0400A66F RID: 42607
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine receiveCoroutine;

		// Token: 0x0400A670 RID: 42608
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientSendHandler sendHandler;

		// Token: 0x0400A671 RID: 42609
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A672 RID: 42610
		[PublicizedFrom(EAccessModifier.Private)]
		public LANServerCacheControl cacheControl = new LANServerCacheControl(LANServerList.rulesRefreshInterval, LANServerList.knownServerTimeout);

		// Token: 0x0400A673 RID: 42611
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];
	}
}
