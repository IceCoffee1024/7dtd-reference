using System;
using System.Net;
using System.Net.Sockets;

namespace Platform.LAN
{
	// Token: 0x02001CD7 RID: 7383
	public class UdpClientSendHandler
	{
		// Token: 0x0600DB25 RID: 56101 RVA: 0x004E4787 File Offset: 0x004E2987
		public UdpClientSendHandler(UdpClient _udpClient)
		{
			this.udpClient = _udpClient;
		}

		// Token: 0x0600DB26 RID: 56102 RVA: 0x004E4798 File Offset: 0x004E2998
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

		// Token: 0x0600DB27 RID: 56103 RVA: 0x004E481C File Offset: 0x004E2A1C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CompleteSendAsync(IAsyncResult _result)
		{
			((UdpClientSendHandler)_result.AsyncState).CompleteSend(_result);
		}

		// Token: 0x0600DB28 RID: 56104 RVA: 0x004E4830 File Offset: 0x004E2A30
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

		// Token: 0x0400A5A7 RID: 42407
		public readonly UdpClient udpClient;

		// Token: 0x0400A5A8 RID: 42408
		public bool isComplete;
	}
}
