using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001D36 RID: 7478
	public class LANServerList : IServerListInterface
	{
		// Token: 0x17001C4D RID: 7245
		// (get) Token: 0x0600DD99 RID: 56729 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsPrefiltered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C4E RID: 7246
		// (get) Token: 0x0600DD9A RID: 56730 RVA: 0x004ECA4B File Offset: 0x004EAC4B
		public bool IsRefreshing
		{
			get
			{
				return this.shouldRefresh;
			}
		}

		// Token: 0x0600DD9B RID: 56731 RVA: 0x004ECA53 File Offset: 0x004EAC53
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DD9C RID: 56732 RVA: 0x004ECA5C File Offset: 0x004EAC5C
		public void RegisterGameServerFoundCallback(GameServerFoundCallback _serverFound, MaxResultsReachedCallback _maxResultsCallback, ServerSearchErrorCallback _sessionSearchErrorCallback)
		{
			this.serverFoundCallback = _serverFound;
		}

		// Token: 0x0600DD9D RID: 56733 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetSingleServerDetails(GameServerInfo _serverInfo, EServerRelationType _relation, GameServerFoundCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD9E RID: 56734 RVA: 0x004ECA68 File Offset: 0x004EAC68
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

		// Token: 0x0600DD9F RID: 56735 RVA: 0x004ECAF8 File Offset: 0x004EACF8
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

		// Token: 0x0600DDA0 RID: 56736 RVA: 0x004ECB07 File Offset: 0x004EAD07
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

		// Token: 0x0600DDA1 RID: 56737 RVA: 0x004ECB18 File Offset: 0x004EAD18
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

		// Token: 0x0600DDA2 RID: 56738 RVA: 0x004ECB7C File Offset: 0x004EAD7C
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRulesRequestDone(bool _success, string _message, GameServerInfo _gsi)
		{
			this.cacheControl.SetUpdated(_gsi.GetValue(GameInfoString.IP), _gsi.GetValue(GameInfoInt.Port));
			_gsi.IsLAN = true;
			this.serverFoundCallback(this.owner, _gsi, EServerRelationType.LAN);
		}

		// Token: 0x0600DDA3 RID: 56739 RVA: 0x004ECBB1 File Offset: 0x004EADB1
		public void StopSearch()
		{
			this.isPaused = true;
			this.cacheControl.Clear();
		}

		// Token: 0x0600DDA4 RID: 56740 RVA: 0x004ECBC8 File Offset: 0x004EADC8
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

		// Token: 0x0400A74E RID: 42830
		[PublicizedFrom(EAccessModifier.Private)]
		public const int serverBroadcastIntervalSeconds = 5;

		// Token: 0x0400A74F RID: 42831
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan rulesRefreshInterval = new TimeSpan(0, 2, 0);

		// Token: 0x0400A750 RID: 42832
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan knownServerTimeout = new TimeSpan(0, 1, 0);

		// Token: 0x0400A751 RID: 42833
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A752 RID: 42834
		[PublicizedFrom(EAccessModifier.Private)]
		public GameServerFoundCallback serverFoundCallback;

		// Token: 0x0400A753 RID: 42835
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IPAddress multicastGroupIp;

		// Token: 0x0400A754 RID: 42836
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A755 RID: 42837
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldRefresh;

		// Token: 0x0400A756 RID: 42838
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPaused;

		// Token: 0x0400A757 RID: 42839
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine requestCoroutine;

		// Token: 0x0400A758 RID: 42840
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine receiveCoroutine;

		// Token: 0x0400A759 RID: 42841
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientSendHandler sendHandler;

		// Token: 0x0400A75A RID: 42842
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A75B RID: 42843
		[PublicizedFrom(EAccessModifier.Private)]
		public LANServerCacheControl cacheControl = new LANServerCacheControl(LANServerList.rulesRefreshInterval, LANServerList.knownServerTimeout);

		// Token: 0x0400A75C RID: 42844
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];
	}
}
