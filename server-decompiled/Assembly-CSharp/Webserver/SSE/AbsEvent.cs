using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001AFA RID: 6906
	public abstract class AbsEvent
	{
		// Token: 0x0600CFAC RID: 53164 RVA: 0x004B9E74 File Offset: 0x004B8074
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsEvent(SseHandler _parent, bool _reuseEncodingBuffer = true, string _name = null)
		{
			this.Name = _name ?? base.GetType().Name;
			this.parent = _parent;
			if (_reuseEncodingBuffer)
			{
				this.encodingBuffer = new byte[1048576];
			}
		}

		// Token: 0x0600CFAD RID: 53165 RVA: 0x004B9ED8 File Offset: 0x004B80D8
		public void AddListener(SseClient _client)
		{
			this.totalOpened++;
			this.currentlyOpen++;
			this.openClients.Add(_client);
			this.logConnectionState("Connection opened", _client);
		}

		// Token: 0x0600CFAE RID: 53166 RVA: 0x004B9F0E File Offset: 0x004B810E
		[PublicizedFrom(EAccessModifier.Protected)]
		public void SendData(string _eventName, string _data)
		{
			this.sendQueue.Enqueue(new ValueTuple<string, string>(_eventName, _data));
			this.parent.SignalSendQueue();
		}

		// Token: 0x0600CFAF RID: 53167 RVA: 0x004B9F30 File Offset: 0x004B8130
		public void ProcessSendQueue()
		{
			while (this.sendQueue.HasData())
			{
				ValueTuple<string, string> valueTuple = this.sendQueue.Dequeue();
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				this.stringBuilder.Append("event: ");
				this.stringBuilder.AppendLine(item);
				this.stringBuilder.Append("data: ");
				this.stringBuilder.AppendLine(item2);
				this.stringBuilder.AppendLine("");
				string text = this.stringBuilder.ToString();
				this.stringBuilder.Clear();
				byte[] bytes;
				int num;
				if (this.encodingBuffer != null)
				{
					bytes = this.encodingBuffer;
					try
					{
						num = Encoding.UTF8.GetBytes(text, 0, text.Length, bytes, 0);
						goto IL_00DB;
					}
					catch (ArgumentException ex)
					{
						Log.Error("[Web] [SSE] '" + this.Name + "': Exception while encoding data for output, most likely exceeding buffer size:");
						Log.Exception(ex);
						break;
					}
					goto IL_00CA;
				}
				goto IL_00CA;
				IL_00DB:
				this.sendBufToListeners(bytes, num);
				continue;
				IL_00CA:
				bytes = Encoding.UTF8.GetBytes(text);
				num = bytes.Length;
				goto IL_00DB;
			}
		}

		// Token: 0x0600CFB0 RID: 53168 RVA: 0x004BA044 File Offset: 0x004B8244
		[PublicizedFrom(EAccessModifier.Private)]
		public void sendBufToListeners(byte[] _bytes, int _bytesToSend)
		{
			for (int i = this.openClients.Count - 1; i >= 0; i--)
			{
				this.openClients[i].Write(_bytes, _bytesToSend);
			}
		}

		// Token: 0x0600CFB1 RID: 53169 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x0600CFB2 RID: 53170 RVA: 0x004BA080 File Offset: 0x004B8280
		[PublicizedFrom(EAccessModifier.Private)]
		public void logConnectionState(string _message, SseClient _client)
		{
			Log.Out(string.Format("[Web] [SSE] '{0}': {1} from {2} (Left open: {3}, total opened: {4}, closed: {5})", new object[] { this.Name, _message, _client.RemoteEndpoint, this.currentlyOpen, this.totalOpened, this.totalClosed }));
		}

		// Token: 0x0600CFB3 RID: 53171 RVA: 0x004BA0E2 File Offset: 0x004B82E2
		public void ClientClosed(SseClient _client)
		{
			if (this.openClients.Remove(_client))
			{
				this.currentlyOpen--;
				this.totalClosed++;
				this.logConnectionState("Closed connection", _client);
			}
		}

		// Token: 0x04009DC1 RID: 40385
		[PublicizedFrom(EAccessModifier.Private)]
		public const int encodingBufferSize = 1048576;

		// Token: 0x04009DC2 RID: 40386
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009DC3 RID: 40387
		public readonly string Name;

		// Token: 0x04009DC4 RID: 40388
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] encodingBuffer;

		// Token: 0x04009DC5 RID: 40389
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringBuilder stringBuilder = new StringBuilder();

		// Token: 0x04009DC6 RID: 40390
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> openClients = new List<SseClient>();

		// Token: 0x04009DC7 RID: 40391
		[TupleElementNames(new string[] { "_eventName", "_data" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<ValueTuple<string, string>> sendQueue = new BlockingQueue<ValueTuple<string, string>>();

		// Token: 0x04009DC8 RID: 40392
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentlyOpen;

		// Token: 0x04009DC9 RID: 40393
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalOpened;

		// Token: 0x04009DCA RID: 40394
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalClosed;
	}
}
