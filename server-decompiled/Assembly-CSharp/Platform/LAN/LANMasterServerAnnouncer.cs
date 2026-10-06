using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001D00 RID: 7424
	public class LANMasterServerAnnouncer : IMasterServerAnnouncer
	{
		// Token: 0x17001C02 RID: 7170
		// (get) Token: 0x0600DC6E RID: 56430 RVA: 0x00020119 File Offset: 0x0001E319
		public bool GameServerInitialized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600DC6F RID: 56431 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DC70 RID: 56432 RVA: 0x0004E540 File Offset: 0x0004C740
		public string GetServerPorts()
		{
			return string.Empty;
		}

		// Token: 0x0600DC71 RID: 56433 RVA: 0x004E8A48 File Offset: 0x004E6C48
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

		// Token: 0x0600DC72 RID: 56434 RVA: 0x004E8B2C File Offset: 0x004E6D2C
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

		// Token: 0x0600DC73 RID: 56435 RVA: 0x004E8B3C File Offset: 0x004E6D3C
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

		// Token: 0x0600DC74 RID: 56436 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600DC75 RID: 56437 RVA: 0x004E8BB4 File Offset: 0x004E6DB4
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

		// Token: 0x0400A655 RID: 42581
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldAnnounce = true;

		// Token: 0x0400A656 RID: 42582
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A657 RID: 42583
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A658 RID: 42584
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine replyCoroutine;

		// Token: 0x0400A659 RID: 42585
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];

		// Token: 0x0400A65A RID: 42586
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] replyBuffer = new byte[4];
	}
}
