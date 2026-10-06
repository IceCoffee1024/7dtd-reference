using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SpaceWizards.HttpListener;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001B29 RID: 6953
	public class SseClient
	{
		// Token: 0x0600D10E RID: 53518 RVA: 0x004BE640 File Offset: 0x004BC840
		public SseClient(SseHandler _parent, RequestContext _context)
		{
			this.parent = _parent;
			this.response = _context.Response;
			this.RemoteEndpoint = _context.Request.RemoteEndPoint;
			this.response.SendChunked = true;
			this.response.AddHeader("Content-Type", "text/event-stream");
			this.response.OutputStream.Flush();
		}

		// Token: 0x0600D10F RID: 53519 RVA: 0x004BE6B4 File Offset: 0x004BC8B4
		public ESseClientWriteResult Write(byte[] _bytes, int _bytesToSend)
		{
			SpaceWizards.HttpListener.HttpListenerResponse httpListenerResponse = this.response;
			ESseClientWriteResult esseClientWriteResult;
			try
			{
				if (!httpListenerResponse.OutputStream.CanWrite)
				{
					this.parent.ClientClosed(this);
					httpListenerResponse.Close();
					esseClientWriteResult = ESseClientWriteResult.Closed;
				}
				else
				{
					httpListenerResponse.OutputStream.Write(_bytes, 0, _bytesToSend);
					httpListenerResponse.OutputStream.Flush();
					this.lastMessageSent = DateTime.Now;
					esseClientWriteResult = ESseClientWriteResult.Ok;
				}
			}
			catch (IOException ex)
			{
				this.parent.ClientClosed(this);
				SocketException ex2 = ex.InnerException as SocketException;
				if (ex2 != null)
				{
					if (ex2.SocketErrorCode == SocketError.ConnectionAborted || ex2.SocketErrorCode == SocketError.Shutdown)
					{
						esseClientWriteResult = ESseClientWriteResult.Closed;
					}
					else
					{
						Log.Error("[Web] [SSE] SocketError (" + ex2.SocketErrorCode.ToStringCached<SocketError>() + ") while trying to write", new object[] { true });
						esseClientWriteResult = ESseClientWriteResult.Error;
					}
				}
				else
				{
					Log.Error("[Web] [SSE] IOException while trying to write:", new object[] { true });
					Log.Exception(ex);
					esseClientWriteResult = ESseClientWriteResult.Error;
				}
			}
			catch (Exception ex3)
			{
				this.parent.ClientClosed(this);
				httpListenerResponse.Close();
				Log.Error("[Web] [SSE] Exception while trying to write:", new object[] { true });
				Log.Exception(ex3);
				esseClientWriteResult = ESseClientWriteResult.Error;
			}
			return esseClientWriteResult;
		}

		// Token: 0x0600D110 RID: 53520 RVA: 0x004BE800 File Offset: 0x004BCA00
		public void HandleKeepAlive()
		{
			DateTime now = DateTime.Now;
			if ((now - this.lastMessageSent).TotalSeconds < 10.0)
			{
				return;
			}
			this.Write(SseClient.keepAliveData, SseClient.keepAliveData.Length);
			this.lastMessageSent = now;
		}

		// Token: 0x04009E95 RID: 40597
		[PublicizedFrom(EAccessModifier.Private)]
		public const int keepAliveIntervalSeconds = 10;

		// Token: 0x04009E96 RID: 40598
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keepAliveData = Encoding.UTF8.GetBytes(": KeepAlive\n\n");

		// Token: 0x04009E97 RID: 40599
		public readonly IPEndPoint RemoteEndpoint;

		// Token: 0x04009E98 RID: 40600
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009E99 RID: 40601
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SpaceWizards.HttpListener.HttpListenerResponse response;

		// Token: 0x04009E9A RID: 40602
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastMessageSent = DateTime.Now;
	}
}
