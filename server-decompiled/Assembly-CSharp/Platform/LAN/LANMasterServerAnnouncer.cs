using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001D31 RID: 7473
	public class LANMasterServerAnnouncer : IMasterServerAnnouncer
	{
		// Token: 0x17001C4A RID: 7242
		// (get) Token: 0x0600DD82 RID: 56706 RVA: 0x00020119 File Offset: 0x0001E319
		public bool GameServerInitialized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600DD83 RID: 56707 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DD84 RID: 56708 RVA: 0x0004E500 File Offset: 0x0004C700
		public string GetServerPorts()
		{
			return string.Empty;
		}

		// Token: 0x0600DD85 RID: 56709 RVA: 0x004EC5E8 File Offset: 0x004EA7E8
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

		// Token: 0x0600DD86 RID: 56710 RVA: 0x004EC6CC File Offset: 0x004EA8CC
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

		// Token: 0x0600DD87 RID: 56711 RVA: 0x004EC6DC File Offset: 0x004EA8DC
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

		// Token: 0x0600DD88 RID: 56712 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DD89 RID: 56713 RVA: 0x004EC754 File Offset: 0x004EA954
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

		// Token: 0x0400A73E RID: 42814
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldAnnounce = true;

		// Token: 0x0400A73F RID: 42815
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A740 RID: 42816
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A741 RID: 42817
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine replyCoroutine;

		// Token: 0x0400A742 RID: 42818
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];

		// Token: 0x0400A743 RID: 42819
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] replyBuffer = new byte[4];
	}
}
