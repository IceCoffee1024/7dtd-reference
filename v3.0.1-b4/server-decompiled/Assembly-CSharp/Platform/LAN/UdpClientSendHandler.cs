using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001CB0 RID: 7344
	public class UdpClientSendHandler
	{
		// Token: 0x0600D98A RID: 55690 RVA: 0x004DE173 File Offset: 0x004DC373
		public UdpClientSendHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600D98B RID: 55691 RVA: 0x004DE184 File Offset: 0x004DC384
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

		// Token: 0x0600D98C RID: 55692 RVA: 0x004DE208 File Offset: 0x004DC408
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteSendAsync(IAsyncResult _result)
		{
			((UdpClientSendHandler)_result.AsyncState).CompleteSend(_result);
		}

		// Token: 0x0600D98D RID: 55693 RVA: 0x004DE21C File Offset: 0x004DC41C
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

		// Token: 0x0400A474 RID: 42100
		public readonly UdpClient udpClient;

		// Token: 0x0400A475 RID: 42101
		public bool isComplete;
	}
}
