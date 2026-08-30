using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001CD8 RID: 7384
	public class UdpClientReceiveHandler
	{
		// Token: 0x0600DB29 RID: 56105 RVA: 0x004E48A8 File Offset: 0x004E2AA8
		public UdpClientReceiveHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600DB2A RID: 56106 RVA: 0x004E48B8 File Offset: 0x004E2AB8
		public bool BeginReceive()
		{
			this.remoteEP = null;
			this.message = null;
			try
			{
				this.isComplete = false;
				this.udpClient.BeginReceive(new AsyncCallback(UdpClientReceiveHandler.CompleteReceiveAsync), this);
				return true;
			}
			catch (SocketException ex)
			{
				Log.Warning(string.Format("LAN receive handler unable to start receive. {0} ErrorCode: {1}", "SocketException", ex.ErrorCode));
			}
			catch (Exception ex2)
			{
				Log.Exception(ex2);
			}
			this.isComplete = true;
			return false;
		}

		// Token: 0x0600DB2B RID: 56107 RVA: 0x004E4948 File Offset: 0x004E2B48
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteReceiveAsync(IAsyncResult _result)
		{
			((UdpClientReceiveHandler)_result.AsyncState).CompleteReceive(_result);
		}

		// Token: 0x0600DB2C RID: 56108 RVA: 0x004E495C File Offset: 0x004E2B5C
		[PublicizedFrom(EAccessModifier.Private)]
		public void CompleteReceive(IAsyncResult _result)
		{
			try
			{
				this.message = this.udpClient.EndReceive(_result, ref this.remoteEP);
				this.length = this.message.Length;
				this.isComplete = true;
				return;
			}
			catch (ObjectDisposedException)
			{
			}
			catch (SocketException ex)
			{
				Log.Warning(string.Format("LAN receive handler unable to complete receive. {0} ErrorCode: {1}", "SocketException", ex.ErrorCode));
			}
			catch (Exception ex2)
			{
				Log.Exception(ex2);
			}
			this.remoteEP = null;
			this.message = null;
			this.length = 0;
			this.isComplete = true;
		}

		// Token: 0x0400A5A9 RID: 42409
		public readonly UdpClient udpClient;

		// Token: 0x0400A5AA RID: 42410
		public IPEndPoint remoteEP;

		// Token: 0x0400A5AB RID: 42411
		public byte[] message;

		// Token: 0x0400A5AC RID: 42412
		public int length;

		// Token: 0x0400A5AD RID: 42413
		public bool isComplete;
	}
}
