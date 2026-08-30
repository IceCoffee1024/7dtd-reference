using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001AD3 RID: 6867
	public abstract class AbsEvent
	{
		// Token: 0x0600CE23 RID: 52771 RVA: 0x004B3A80 File Offset: 0x004B1C80
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

		// Token: 0x0600CE24 RID: 52772 RVA: 0x004B3AE4 File Offset: 0x004B1CE4
		public void AddListener(SseClient _client)
		{
			this.totalOpened++;
			this.currentlyOpen++;
			this.openClients.Add(_client);
			this.logConnectionState("Connection opened", _client);
		}

		// Token: 0x0600CE25 RID: 52773 RVA: 0x004B3B1A File Offset: 0x004B1D1A
		[PublicizedFrom(EAccessModifier.Protected)]
		public void SendData(string _eventName, string _data)
		{
			this.sendQueue.Enqueue(new ValueTuple<string, string>(_eventName, _data));
			this.parent.SignalSendQueue();
		}

		// Token: 0x0600CE26 RID: 52774 RVA: 0x004B3B3C File Offset: 0x004B1D3C
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

		// Token: 0x0600CE27 RID: 52775 RVA: 0x004B3C50 File Offset: 0x004B1E50
		[PublicizedFrom(EAccessModifier.Private)]
		public void sendBufToListeners(byte[] _bytes, int _bytesToSend)
		{
			for (int i = this.openClients.Count - 1; i >= 0; i--)
			{
				this.openClients[i].Write(_bytes, _bytesToSend);
			}
		}

		// Token: 0x0600CE28 RID: 52776 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x0600CE29 RID: 52777 RVA: 0x004B3C8C File Offset: 0x004B1E8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void logConnectionState(string _message, SseClient _client)
		{
			Log.Out(string.Format("[Web] [SSE] '{0}': {1} from {2} (Left open: {3}, total opened: {4}, closed: {5})", new object[] { this.Name, _message, _client.RemoteEndpoint, this.currentlyOpen, this.totalOpened, this.totalClosed }));
		}

		// Token: 0x0600CE2A RID: 52778 RVA: 0x004B3CEE File Offset: 0x004B1EEE
		public void ClientClosed(SseClient _client)
		{
			if (this.openClients.Remove(_client))
			{
				this.currentlyOpen--;
				this.totalClosed++;
				this.logConnectionState("Closed connection", _client);
			}
		}

		// Token: 0x04009C90 RID: 40080
		[PublicizedFrom(EAccessModifier.Private)]
		public const int encodingBufferSize = 1048576;

		// Token: 0x04009C91 RID: 40081
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009C92 RID: 40082
		public readonly string Name;

		// Token: 0x04009C93 RID: 40083
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] encodingBuffer;

		// Token: 0x04009C94 RID: 40084
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringBuilder stringBuilder = new StringBuilder();

		// Token: 0x04009C95 RID: 40085
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> openClients = new List<SseClient>();

		// Token: 0x04009C96 RID: 40086
		[TupleElementNames(new string[] { "_eventName", "_data" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<ValueTuple<string, string>> sendQueue = new BlockingQueue<ValueTuple<string, string>>();

		// Token: 0x04009C97 RID: 40087
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentlyOpen;

		// Token: 0x04009C98 RID: 40088
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalOpened;

		// Token: 0x04009C99 RID: 40089
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalClosed;
	}
}
