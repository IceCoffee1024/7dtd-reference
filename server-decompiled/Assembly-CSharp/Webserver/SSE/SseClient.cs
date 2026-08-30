using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SpaceWizards.HttpListener;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001AD6 RID: 6870
	public class SseClient
	{
		// Token: 0x0600CE2D RID: 52781 RVA: 0x004B3D80 File Offset: 0x004B1F80
		public SseClient(SseHandler _parent, RequestContext _context)
		{
			this.parent = _parent;
			this.response = _context.Response;
			this.RemoteEndpoint = _context.Request.RemoteEndPoint;
			this.response.SendChunked = true;
			this.response.AddHeader("Content-Type", "text/event-stream");
			this.response.OutputStream.Flush();
		}

		// Token: 0x0600CE2E RID: 52782 RVA: 0x004B3DF4 File Offset: 0x004B1FF4
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

		// Token: 0x0600CE2F RID: 52783 RVA: 0x004B3F40 File Offset: 0x004B2140
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

		// Token: 0x04009C9E RID: 40094
		[PublicizedFrom(EAccessModifier.Private)]
		public const int keepAliveIntervalSeconds = 10;

		// Token: 0x04009C9F RID: 40095
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keepAliveData = Encoding.UTF8.GetBytes(": KeepAlive\n\n");

		// Token: 0x04009CA0 RID: 40096
		public readonly IPEndPoint RemoteEndpoint;

		// Token: 0x04009CA1 RID: 40097
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009CA2 RID: 40098
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SpaceWizards.HttpListener.HttpListenerResponse response;

		// Token: 0x04009CA3 RID: 40099
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime lastMessageSent = DateTime.Now;
	}
}
