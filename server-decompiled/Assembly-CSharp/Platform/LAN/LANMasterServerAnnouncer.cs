using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001CCE RID: 7374
	public class LANMasterServerAnnouncer : IMasterServerAnnouncer
	{
		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x0600DAF3 RID: 56051 RVA: 0x00020021 File Offset: 0x0001E221
		public bool GameServerInitialized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600DAF4 RID: 56052 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DAF5 RID: 56053 RVA: 0x0004E53C File Offset: 0x0004C73C
		public string GetServerPorts()
		{
			return string.Empty;
		}

		// Token: 0x0600DAF6 RID: 56054 RVA: 0x004E3F00 File Offset: 0x004E2100
		public void AdvertiseServer(Action _onServerRegistered)
		{
			int num = 11000;
			IPAddress multicastGroupIp = LANServerSearchConfig.MulticastGroupIp;
			try
			{
				this.udpClient = new UdpClient(num);
				this.udpClient.JoinMulticastGroup(multicastGroupIp);
				this.receiveHandler = new UdpClientReceiveHandler(this.udpClient);
				this.shouldAnnounce = true;
				this.replyCoroutine = ThreadManager.StartCoroutine(this.LANServerListReplyTask());
				Log.Out(string.Format("[{0}] listening on {1} and multicast group {2}", "LANMasterServerAnnouncer", num, multicastGroupIp));
			}
			catch (SocketException ex)
			{
				Log.Warning(string.Format("[{0}] could not start LAN server search listening on port {1} and multicast group {2}. ErrorCode: {3}, Message: {4}", new object[] { "LANMasterServerAnnouncer", num, multicastGroupIp, ex.ErrorCode, ex.Message }));
			}
			catch (Exception ex2)
			{
				Log.Exception(ex2);
			}
			_onServerRegistered();
		}

		// Token: 0x0600DAF7 RID: 56055 RVA: 0x004E3FE4 File Offset: 0x004E21E4
		public IEnumerator LANServerListReplyTask()
		{
			while (this.shouldAnnounce)
			{
				if (!this.receiveHandler.BeginReceive())
				{
					Log.Error("[LANMasterServerAnnouncer] could not start receive");
					yield break;
				}
				while (!this.receiveHandler.isComplete)
				{
					yield return null;
				}
				IPEndPoint remoteEP = this.receiveHandler.remoteEP;
				byte[] message = this.receiveHandler.message;
				int length = this.receiveHandler.length;
				if (remoteEP != null && message != null && length == 0)
				{
					this.SendReply(remoteEP);
				}
			}
			yield break;
		}

		// Token: 0x0600DAF8 RID: 56056 RVA: 0x004E3FF4 File Offset: 0x004E21F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void SendReply(IPEndPoint _remoteEP)
		{
			try
			{
				int value = SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoInt.Port);
				int num = 0;
				StreamUtils.Write(LANMasterServerAnnouncer.replyBuffer, value, ref num);
				this.udpClient.Client.SendTo(LANMasterServerAnnouncer.replyBuffer, 0, num, SocketFlags.None, _remoteEP);
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("[{0}] could not send reply to {1}", "LANMasterServerAnnouncer", _remoteEP));
				Log.Exception(ex);
			}
		}

		// Token: 0x0600DAF9 RID: 56057 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DAFA RID: 56058 RVA: 0x004E406C File Offset: 0x004E226C
		public void StopServer()
		{
			this.shouldAnnounce = false;
			if (this.replyCoroutine != null)
			{
				ThreadManager.StopCoroutine(this.replyCoroutine);
			}
			UdpClient udpClient = this.udpClient;
			if (udpClient != null)
			{
				udpClient.Dispose();
			}
			this.udpClient = null;
			this.receiveHandler = null;
		}

		// Token: 0x0400A580 RID: 42368
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldAnnounce = true;

		// Token: 0x0400A581 RID: 42369
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A582 RID: 42370
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A583 RID: 42371
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine replyCoroutine;

		// Token: 0x0400A584 RID: 42372
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];

		// Token: 0x0400A585 RID: 42373
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] replyBuffer = new byte[4];
	}
}
