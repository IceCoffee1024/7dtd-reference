using System;
using System.Collections.Generic;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001A93 RID: 6803
	public class LogBuffer
	{
		// Token: 0x14000119 RID: 281
		// (add) Token: 0x0600CCC5 RID: 52421 RVA: 0x004A9314 File Offset: 0x004A7514
		// (remove) Token: 0x0600CCC6 RID: 52422 RVA: 0x004A9348 File Offset: 0x004A7548
		public static event Action<LogBuffer.LogEntry> EntryAdded;

		// Token: 0x0600CCC7 RID: 52423 RVA: 0x004A937B File Offset: 0x004A757B
		public static void Init()
		{
			if (LogBuffer.instance == null)
			{
				LogBuffer.instance = new LogBuffer();
			}
		}

		// Token: 0x0600CCC8 RID: 52424 RVA: 0x004A938E File Offset: 0x004A758E
		[PublicizedFrom(EAccessModifier.Private)]
		public LogBuffer()
		{
			Log.LogCallbacksExtended += this.LogCallback;
		}

		// Token: 0x170018FB RID: 6395
		// (get) Token: 0x0600CCC9 RID: 52425 RVA: 0x004A93BD File Offset: 0x004A75BD
		public static LogBuffer Instance
		{
			get
			{
				LogBuffer logBuffer;
				if ((logBuffer = LogBuffer.instance) == null)
				{
					logBuffer = (LogBuffer.instance = new LogBuffer());
				}
				return logBuffer;
			}
		}

		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x0600CCCA RID: 52426 RVA: 0x004A93D4 File Offset: 0x004A75D4
		public int OldestLine
		{
			get
			{
				List<LogBuffer.LogEntry> list = this.logEntries;
				int num;
				lock (list)
				{
					num = this.listOffset;
				}
				return num;
			}
		}

		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x0600CCCB RID: 52427 RVA: 0x004A9418 File Offset: 0x004A7618
		public int LatestLine
		{
			get
			{
				List<LogBuffer.LogEntry> list = this.logEntries;
				int num;
				lock (list)
				{
					num = this.listOffset + this.logEntries.Count - 1;
				}
				return num;
			}
		}

		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x0600CCCC RID: 52428 RVA: 0x004A9468 File Offset: 0x004A7668
		public int StoredLines
		{
			get
			{
				List<LogBuffer.LogEntry> list = this.logEntries;
				int count;
				lock (list)
				{
					count = this.logEntries.Count;
				}
				return count;
			}
		}

		// Token: 0x170018FF RID: 6399
		public LogBuffer.LogEntry this[int _index]
		{
			get
			{
				List<LogBuffer.LogEntry> list = this.logEntries;
				lock (list)
				{
					if (_index >= this.listOffset && _index < this.listOffset + this.logEntries.Count)
					{
						return this.logEntries[_index];
					}
				}
				return null;
			}
		}

		// Token: 0x0600CCCE RID: 52430 RVA: 0x004A951C File Offset: 0x004A771C
		[PublicizedFrom(EAccessModifier.Private)]
		public void LogCallback(string _formattedMsg, string _plainMsg, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
			List<LogBuffer.LogEntry> list = this.logEntries;
			lock (list)
			{
				LogBuffer.LogEntry logEntry = new LogBuffer.LogEntry(this.listOffset + this.logEntries.Count, _timestamp, _plainMsg, _trace, _type, _uptime);
				this.logEntries.Add(logEntry);
				Action<LogBuffer.LogEntry> entryAdded = LogBuffer.EntryAdded;
				if (entryAdded != null)
				{
					entryAdded(logEntry);
				}
				if (this.logEntries.Count > 3000)
				{
					this.listOffset += this.logEntries.Count - 3000;
					this.logEntries.RemoveRange(0, this.logEntries.Count - 3000);
				}
			}
		}

		// Token: 0x0600CCCF RID: 52431 RVA: 0x004A95E4 File Offset: 0x004A77E4
		public List<LogBuffer.LogEntry> GetRange(ref int _start, int _count, out int _end)
		{
			List<LogBuffer.LogEntry> list = this.logEntries;
			List<LogBuffer.LogEntry> range;
			lock (list)
			{
				int num;
				if (_count < 0)
				{
					_count = -_count;
					if (_start >= this.listOffset + this.logEntries.Count)
					{
						_start = this.listOffset + this.logEntries.Count - 1;
					}
					_end = _start;
					if (_start < this.listOffset)
					{
						return this.emptyList;
					}
					_start -= _count - 1;
					if (_start < this.listOffset)
					{
						_start = this.listOffset;
					}
					num = _start - this.listOffset;
					_end++;
					_count = _end - _start;
				}
				else
				{
					if (_start < this.listOffset)
					{
						_start = this.listOffset;
					}
					if (_start >= this.listOffset + this.logEntries.Count)
					{
						_end = _start;
						return this.emptyList;
					}
					num = _start - this.listOffset;
					if (num + _count > this.logEntries.Count)
					{
						_count = this.logEntries.Count - num;
					}
					_end = _start + _count;
				}
				range = this.logEntries.GetRange(num, _count);
			}
			return range;
		}

		// Token: 0x04009B10 RID: 39696
		[PublicizedFrom(EAccessModifier.Private)]
		public const int maxEntries = 3000;

		// Token: 0x04009B11 RID: 39697
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogBuffer instance;

		// Token: 0x04009B12 RID: 39698
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> logEntries = new List<LogBuffer.LogEntry>();

		// Token: 0x04009B13 RID: 39699
		[PublicizedFrom(EAccessModifier.Private)]
		public int listOffset;

		// Token: 0x04009B15 RID: 39701
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> emptyList = new List<LogBuffer.LogEntry>();

		// Token: 0x02001A94 RID: 6804
		public class LogEntry
		{
			// Token: 0x0600CCD0 RID: 52432 RVA: 0x004A9714 File Offset: 0x004A7914
			public LogEntry(int _messageId, DateTime _timestamp, string _message, string _trace, LogType _type, long _uptime)
			{
				this.MessageId = _messageId;
				this.Timestamp = _timestamp;
				this.IsoTime = _timestamp.ToString("o");
				this.Message = _message;
				this.Trace = _trace;
				this.Type = _type;
				this.Uptime = _uptime;
			}

			// Token: 0x04009B16 RID: 39702
			public readonly int MessageId;

			// Token: 0x04009B17 RID: 39703
			public readonly DateTime Timestamp;

			// Token: 0x04009B18 RID: 39704
			public readonly string IsoTime;

			// Token: 0x04009B19 RID: 39705
			public readonly string Message;

			// Token: 0x04009B1A RID: 39706
			public readonly string Trace;

			// Token: 0x04009B1B RID: 39707
			public readonly LogType Type;

			// Token: 0x04009B1C RID: 39708
			public readonly long Uptime;
		}
	}
}
