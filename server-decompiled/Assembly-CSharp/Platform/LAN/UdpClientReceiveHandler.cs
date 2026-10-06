using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001D0A RID: 7434
	public class UdpClientReceiveHandler
	{
		// Token: 0x0600DCA4 RID: 56484 RVA: 0x004E93F0 File Offset: 0x004E75F0
		public UdpClientReceiveHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600DCA5 RID: 56485 RVA: 0x004E9400 File Offset: 0x004E7600
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

		// Token: 0x0600DCA6 RID: 56486 RVA: 0x004E9490 File Offset: 0x004E7690
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteReceiveAsync(IAsyncResult _result)
		{
			((UdpClientReceiveHandler)_result.AsyncState).CompleteReceive(_result);
		}

		// Token: 0x0600DCA7 RID: 56487 RVA: 0x004E94A4 File Offset: 0x004E76A4
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

		// Token: 0x0400A67E RID: 42622
		public readonly UdpClient udpClient;

		// Token: 0x0400A67F RID: 42623
		public IPEndPoint remoteEP;

		// Token: 0x0400A680 RID: 42624
		public byte[] message;

		// Token: 0x0400A681 RID: 42625
		public int length;

		// Token: 0x0400A682 RID: 42626
		public bool isComplete;
	}
}
