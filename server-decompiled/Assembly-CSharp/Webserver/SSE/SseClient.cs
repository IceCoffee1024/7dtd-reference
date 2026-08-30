using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SpaceWizards.HttpListener;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001AFD RID: 6909
	public class SseClient
	{
		// Token: 0x0600CFB6 RID: 53174 RVA: 0x004BA174 File Offset: 0x004B8374
		public SseClient(SseHandler _parent, RequestContext _context)
		{
			this.parent = _parent;
			this.response = _context.Response;
			this.RemoteEndpoint = _context.Request.RemoteEndPoint;
			this.response.SendChunked = true;
			this.response.AddHeader("Content-Type", "text/event-stream");
			this.response.OutputStream.Flush();
		}

		// Token: 0x0600CFB7 RID: 53175 RVA: 0x004BA1E8 File Offset: 0x004B83E8
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

		// Token: 0x0600CFB8 RID: 53176 RVA: 0x004BA334 File Offset: 0x004B8534
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

		// Token: 0x04009DCF RID: 40399
		[PublicizedFrom(EAccessModifier.Private)]
		public const int keepAliveIntervalSeconds = 10;

		// Token: 0x04009DD0 RID: 40400
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keepAliveData = Encoding.UTF8.GetBytes(": KeepAlive\n\n");

		// Token: 0x04009DD1 RID: 40401
		public readonly IPEndPoint RemoteEndpoint;

		// Token: 0x04009DD2 RID: 40402
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009DD3 RID: 40403
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SpaceWizards.HttpListener.HttpListenerResponse response;

		// Token: 0x04009DD4 RID: 40404
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastMessageSent = DateTime.Now;
	}
}
