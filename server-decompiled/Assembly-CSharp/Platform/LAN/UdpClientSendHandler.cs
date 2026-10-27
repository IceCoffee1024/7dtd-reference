using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001D3A RID: 7482
	public class UdpClientSendHandler
	{
		// Token: 0x0600DDB4 RID: 56756 RVA: 0x004ECE6F File Offset: 0x004EB06F
		public UdpClientSendHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600DDB5 RID: 56757 RVA: 0x004ECE80 File Offset: 0x004EB080
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

		// Token: 0x0600DDB6 RID: 56758 RVA: 0x004ECF04 File Offset: 0x004EB104
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteSendAsync(IAsyncResult _result)
		{
			((UdpClientSendHandler)_result.AsyncState).CompleteSend(_result);
		}

		// Token: 0x0600DDB7 RID: 56759 RVA: 0x004ECF18 File Offset: 0x004EB118
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

		// Token: 0x0400A765 RID: 42853
		public readonly UdpClient udpClient;

		// Token: 0x0400A766 RID: 42854
		public bool isComplete;
	}
}
