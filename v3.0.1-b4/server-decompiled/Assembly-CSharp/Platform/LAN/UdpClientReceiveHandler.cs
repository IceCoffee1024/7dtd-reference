using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001CB1 RID: 7345
	public class UdpClientReceiveHandler
	{
		// Token: 0x0600D98E RID: 55694 RVA: 0x004DE294 File Offset: 0x004DC494
		public UdpClientReceiveHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600D98F RID: 55695 RVA: 0x004DE2A4 File Offset: 0x004DC4A4
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

		// Token: 0x0600D990 RID: 55696 RVA: 0x004DE334 File Offset: 0x004DC534
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteReceiveAsync(IAsyncResult _result)
		{
			((UdpClientReceiveHandler)_result.AsyncState).CompleteReceive(_result);
		}

		// Token: 0x0600D991 RID: 55697 RVA: 0x004DE348 File Offset: 0x004DC548
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

		// Token: 0x0400A476 RID: 42102
		public readonly UdpClient udpClient;

		// Token: 0x0400A477 RID: 42103
		public IPEndPoint remoteEP;

		// Token: 0x0400A478 RID: 42104
		public byte[] message;

		// Token: 0x0400A479 RID: 42105
		public int length;

		// Token: 0x0400A47A RID: 42106
		public bool isComplete;
	}
}
