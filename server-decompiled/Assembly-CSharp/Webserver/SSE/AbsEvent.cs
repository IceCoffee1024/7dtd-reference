using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Webserver.UrlHandlers;

namespace Webserver.SSE
{
	// Token: 0x02001B26 RID: 6950
	public abstract class AbsEvent
	{
		// Token: 0x0600D104 RID: 53508 RVA: 0x004BE340 File Offset: 0x004BC540
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

		// Token: 0x0600D105 RID: 53509 RVA: 0x004BE3A4 File Offset: 0x004BC5A4
		public void AddListener(SseClient _client)
		{
			this.totalOpened++;
			this.currentlyOpen++;
			this.openClients.Add(_client);
			this.logConnectionState("Connection opened", _client);
		}

		// Token: 0x0600D106 RID: 53510 RVA: 0x004BE3DA File Offset: 0x004BC5DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public void SendData(string _eventName, string _data)
		{
			this.sendQueue.Enqueue(new ValueTuple<string, string>(_eventName, _data));
			this.parent.SignalSendQueue();
		}

		// Token: 0x0600D107 RID: 53511 RVA: 0x004BE3FC File Offset: 0x004BC5FC
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

		// Token: 0x0600D108 RID: 53512 RVA: 0x004BE510 File Offset: 0x004BC710
		[PublicizedFrom(EAccessModifier.Private)]
		public void sendBufToListeners(byte[] _bytes, int _bytesToSend)
		{
			for (int i = this.openClients.Count - 1; i >= 0; i--)
			{
				this.openClients[i].Write(_bytes, _bytesToSend);
			}
		}

		// Token: 0x0600D109 RID: 53513 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x0600D10A RID: 53514 RVA: 0x004BE54C File Offset: 0x004BC74C
		[PublicizedFrom(EAccessModifier.Private)]
		public void logConnectionState(string _message, SseClient _client)
		{
			Log.Out(string.Format("[Web] [SSE] '{0}': {1} from {2} (Left open: {3}, total opened: {4}, closed: {5})", new object[] { this.Name, _message, _client.RemoteEndpoint, this.currentlyOpen, this.totalOpened, this.totalClosed }));
		}

		// Token: 0x0600D10B RID: 53515 RVA: 0x004BE5AE File Offset: 0x004BC7AE
		public void ClientClosed(SseClient _client)
		{
			if (this.openClients.Remove(_client))
			{
				this.currentlyOpen--;
				this.totalClosed++;
				this.logConnectionState("Closed connection", _client);
			}
		}

		// Token: 0x04009E87 RID: 40583
		[PublicizedFrom(EAccessModifier.Private)]
		public const int encodingBufferSize = 1048576;

		// Token: 0x04009E88 RID: 40584
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SseHandler parent;

		// Token: 0x04009E89 RID: 40585
		public readonly string Name;

		// Token: 0x04009E8A RID: 40586
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] encodingBuffer;

		// Token: 0x04009E8B RID: 40587
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringBuilder stringBuilder = new StringBuilder();

		// Token: 0x04009E8C RID: 40588
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> openClients = new List<SseClient>();

		// Token: 0x04009E8D RID: 40589
		[TupleElementNames(new string[] { "_eventName", "_data" })]
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly BlockingQueue<ValueTuple<string, string>> sendQueue = new BlockingQueue<ValueTuple<string, string>>();

		// Token: 0x04009E8E RID: 40590
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentlyOpen;

		// Token: 0x04009E8F RID: 40591
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalOpened;

		// Token: 0x04009E90 RID: 40592
		[PublicizedFrom(EAccessModifier.Private)]
		public int totalClosed;
	}
}
