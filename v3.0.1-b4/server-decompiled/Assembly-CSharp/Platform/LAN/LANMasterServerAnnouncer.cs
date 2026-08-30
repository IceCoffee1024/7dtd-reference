using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Platform.LAN
{
	// Token: 0x02001CA7 RID: 7335
	public class LANMasterServerAnnouncer : IMasterServerAnnouncer
	{
		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x0600D958 RID: 55640 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool GameServerInitialized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D959 RID: 55641 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D95A RID: 55642 RVA: 0x0004E124 File Offset: 0x0004C324
		public string GetServerPorts()
		{
			return string.Empty;
		}

		// Token: 0x0600D95B RID: 55643 RVA: 0x004DD8EC File Offset: 0x004DBAEC
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

		// Token: 0x0600D95C RID: 55644 RVA: 0x004DD9D0 File Offset: 0x004DBBD0
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

		// Token: 0x0600D95D RID: 55645 RVA: 0x004DD9E0 File Offset: 0x004DBBE0
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

		// Token: 0x0600D95E RID: 55646 RVA: 0x000027FC File Offset: 0x000009FC
		public void Update()
		{
		}

		// Token: 0x0600D95F RID: 55647 RVA: 0x004DDA58 File Offset: 0x004DBC58
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

		// Token: 0x0400A44D RID: 42061
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldAnnounce = true;

		// Token: 0x0400A44E RID: 42062
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClient udpClient;

		// Token: 0x0400A44F RID: 42063
		[PublicizedFrom(EAccessModifier.Private)]
		public UdpClientReceiveHandler receiveHandler;

		// Token: 0x0400A450 RID: 42064
		[PublicizedFrom(EAccessModifier.Private)]
		public Coroutine replyCoroutine;

		// Token: 0x0400A451 RID: 42065
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] emptyMessage = new byte[0];

		// Token: 0x0400A452 RID: 42066
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] replyBuffer = new byte[4];
	}
}
