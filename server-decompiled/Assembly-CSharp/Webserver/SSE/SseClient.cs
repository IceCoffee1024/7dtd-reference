using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SpaceWizards.HttpListener;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001B5A RID: 7002
	public class SseClient
	{
		// Token: 0x0600D222 RID: 53794 RVA: 0x004C213C File Offset: 0x004C033C
		public SseClient(SseHandler _parent, RequestContext _context)
		{
			this.parent = _parent;
			this.response = _context.Response;
			this.RemoteEndpoint = _context.Request.RemoteEndPoint;
			this.response.SendChunked = true;
			this.response.AddHeader("Content-Type", "text/event-stream");
			this.response.OutputStream.Flush();
		}

		// Token: 0x0600D223 RID: 53795 RVA: 0x004C21B0 File Offset: 0x004C03B0
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

		// Token: 0x0600D224 RID: 53796 RVA: 0x004C22FC File Offset: 0x004C04FC
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

		// Token: 0x04009F7E RID: 40830
		[PublicizedFrom(EAccessModifier.Private)]
		public const int keepAliveIntervalSeconds = 10;

		// Token: 0x04009F7F RID: 40831
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keepAliveData = Encoding.UTF8.GetBytes(": KeepAlive\n\n");

		// Token: 0x04009F80 RID: 40832
		public readonly IPEndPoint RemoteEndpoint;

		// Token: 0x04009F81 RID: 40833
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009F82 RID: 40834
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SpaceWizards.HttpListener.HttpListenerResponse response;

		// Token: 0x04009F83 RID: 40835
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastMessageSent = DateTime.Now;
	}
}
