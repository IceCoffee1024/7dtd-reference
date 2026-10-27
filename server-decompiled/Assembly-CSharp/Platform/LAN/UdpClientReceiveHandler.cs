using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001D3B RID: 7483
	public class UdpClientReceiveHandler
	{
		// Token: 0x0600DDB8 RID: 56760 RVA: 0x004ECF90 File Offset: 0x004EB190
		public UdpClientReceiveHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600DDB9 RID: 56761 RVA: 0x004ECFA0 File Offset: 0x004EB1A0
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

		// Token: 0x0600DDBA RID: 56762 RVA: 0x004ED030 File Offset: 0x004EB230
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteReceiveAsync(IAsyncResult _result)
		{
			((UdpClientReceiveHandler)_result.AsyncState).CompleteReceive(_result);
		}

		// Token: 0x0600DDBB RID: 56763 RVA: 0x004ED044 File Offset: 0x004EB244
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

		// Token: 0x0400A767 RID: 42855
		public readonly UdpClient udpClient;

		// Token: 0x0400A768 RID: 42856
		public IPEndPoint remoteEP;

		// Token: 0x0400A769 RID: 42857
		public byte[] message;

		// Token: 0x0400A76A RID: 42858
		public int length;

		// Token: 0x0400A76B RID: 42859
		public bool isComplete;
	}
}
