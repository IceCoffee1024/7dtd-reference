using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001B57 RID: 6999
	public abstract class AbsEvent
	{
		// Token: 0x0600D218 RID: 53784 RVA: 0x004C1E3C File Offset: 0x004C003C
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

		// Token: 0x0600D219 RID: 53785 RVA: 0x004C1EA0 File Offset: 0x004C00A0
		public void AddListener(SseClient _client)
		{
			this.totalOpened++;
			this.currentlyOpen++;
			this.openClients.Add(_client);
			this.logConnectionState("Connection opened", _client);
		}

		// Token: 0x0600D21A RID: 53786 RVA: 0x004C1ED6 File Offset: 0x004C00D6
		[PublicizedFrom(EAccessModifier.Protected)]
		public void SendData(string _eventName, string _data)
		{
			this.sendQueue.Enqueue(new ValueTuple<string, string>(_eventName, _data));
			this.parent.SignalSendQueue();
		}

		// Token: 0x0600D21B RID: 53787 RVA: 0x004C1EF8 File Offset: 0x004C00F8
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

		// Token: 0x0600D21C RID: 53788 RVA: 0x004C200C File Offset: 0x004C020C
		[PublicizedFrom(EAccessModifier.Private)]
		public void sendBufToListeners(byte[] _bytes, int _bytesToSend)
		{
			for (int i = this.openClients.Count - 1; i >= 0; i--)
			{
				this.openClients[i].Write(_bytes, _bytesToSend);
			}
		}

		// Token: 0x0600D21D RID: 53789 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x0600D21E RID: 53790 RVA: 0x004C2048 File Offset: 0x004C0248
		[PublicizedFrom(EAccessModifier.Private)]
		public void logConnectionState(string _message, SseClient _client)
		{
			Log.Out(string.Format("[Web] [SSE] '{0}': {1} from {2} (Left open: {3}, total opened: {4}, closed: {5})", new object[] { this.Name, _message, _client.RemoteEndpoint, this.currentlyOpen, this.totalOpened, this.totalClosed }));
		}

		// Token: 0x0600D21F RID: 53791 RVA: 0x004C20AA File Offset: 0x004C02AA
		public void ClientClosed(SseClient _client)
		{
			if (this.openClients.Remove(_client))
			{
				this.currentlyOpen--;
				this.totalClosed++;
				this.logConnectionState("Closed connection", _client);
			}
		}

		// Token: 0x04009F70 RID: 40816
		[PublicizedFrom(EAccessModifier.Private)]
		public const int encodingBufferSize = 1048576;

		// Token: 0x04009F71 RID: 40817
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009F72 RID: 40818
		public readonly string Name;

		// Token: 0x04009F73 RID: 40819
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] encodingBuffer;

		// Token: 0x04009F74 RID: 40820
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringBuilder stringBuilder = new StringBuilder();

		// Token: 0x04009F75 RID: 40821
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> openClients = new List<SseClient>();

		// Token: 0x04009F76 RID: 40822
		[TupleElementNames(new string[] { "_eventName", "_data" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<ValueTuple<string, string>> sendQueue = new BlockingQueue<ValueTuple<string, string>>();

		// Token: 0x04009F77 RID: 40823
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentlyOpen;

		// Token: 0x04009F78 RID: 40824
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalOpened;

		// Token: 0x04009F79 RID: 40825
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalClosed;
	}
}
