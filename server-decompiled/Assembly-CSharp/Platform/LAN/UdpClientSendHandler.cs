using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001D09 RID: 7433
	public class UdpClientSendHandler
	{
		// Token: 0x0600DCA0 RID: 56480 RVA: 0x004E92CF File Offset: 0x004E74CF
		public UdpClientSendHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600DCA1 RID: 56481 RVA: 0x004E92E0 File Offset: 0x004E74E0
		public bool BeginSend(byte[] _message, int _length, IPEndPoint _endPoint)
		{
			try
			{
				this.isComplete = false;
				this.udpClient.BeginSend(_message, _length, _endPoint, new AsyncCallback(UdpClientSendHandler.CompleteSendAsync), this);
				return true;
			}
			catch (SocketException ex)
			{
				Log.Warning(string.Format("LAN send handler unable to start send. {0} ErrorCode: {1}", "SocketException", ex.ErrorCode));
			}
			catch (Exception ex2)
			{
				Log.Exception(ex2);
			}
			this.isComplete = true;
			return false;
		}

		// Token: 0x0600DCA2 RID: 56482 RVA: 0x004E9364 File Offset: 0x004E7564
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteSendAsync(IAsyncResult _result)
		{
			((UdpClientSendHandler)_result.AsyncState).CompleteSend(_result);
		}

		// Token: 0x0600DCA3 RID: 56483 RVA: 0x004E9378 File Offset: 0x004E7578
		[PublicizedFrom(EAccessModifier.Private)]
		public void CompleteSend(IAsyncResult _result)
		{
			try
			{
				this.udpClient.EndSend(_result);
			}
			catch (ObjectDisposedException)
			{
			}
			catch (SocketException ex)
			{
				Log.Warning(string.Format("LAN send handler unable to complete send. {0} ErrorCode: {1}", "SocketException", ex.ErrorCode));
			}
			catch (Exception ex2)
			{
				Log.Exception(ex2);
			}
			this.isComplete = true;
		}

		// Token: 0x0400A67C RID: 42620
		public readonly UdpClient udpClient;

		// Token: 0x0400A67D RID: 42621
		public bool isComplete;
	}
}
