using System;
using System.Collections.Generic;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001B17 RID: 6935
	public class LogBuffer
	{
		// Token: 0x14000119 RID: 281
		// (add) Token: 0x0600D0BA RID: 53434 RVA: 0x004B76D0 File Offset: 0x004B58D0
		// (remove) Token: 0x0600D0BB RID: 53435 RVA: 0x004B7704 File Offset: 0x004B5904
		public static event Action<LogBuffer.LogEntry> EntryAdded;

		// Token: 0x0600D0BC RID: 53436 RVA: 0x004B7737 File Offset: 0x004B5937
		public static void Init()
		{
			if (LogBuffer.instance == null)
			{
				LogBuffer.instance = new LogBuffer();
			}
		}

		// Token: 0x0600D0BD RID: 53437 RVA: 0x004B774A File Offset: 0x004B594A
		[PublicizedFrom(EAccessModifier.Private)]
		public LogBuffer()
		{
			Log.LogCallbacksExtended += this.LogCallback;
		}

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x0600D0BE RID: 53438 RVA: 0x004B7779 File Offset: 0x004B5979
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

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x0600D0BF RID: 53439 RVA: 0x004B7790 File Offset: 0x004B5990
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

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x0600D0C0 RID: 53440 RVA: 0x004B77D4 File Offset: 0x004B59D4
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

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x0600D0C1 RID: 53441 RVA: 0x004B7824 File Offset: 0x004B5A24
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

		// Token: 0x17001A5B RID: 6747
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

		// Token: 0x0600D0C3 RID: 53443 RVA: 0x004B78D8 File Offset: 0x004B5AD8
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

		// Token: 0x0600D0C4 RID: 53444 RVA: 0x004B79A0 File Offset: 0x004B5BA0
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

		// Token: 0x04009DF0 RID: 40432
		[PublicizedFrom(EAccessModifier.Private)]
		public const int maxEntries = 3000;

		// Token: 0x04009DF1 RID: 40433
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogBuffer instance;

		// Token: 0x04009DF2 RID: 40434
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> logEntries = new List<LogBuffer.LogEntry>();

		// Token: 0x04009DF3 RID: 40435
		[PublicizedFrom(EAccessModifier.Private)]
		public int listOffset;

		// Token: 0x04009DF5 RID: 40437
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> emptyList = new List<LogBuffer.LogEntry>();

		// Token: 0x02001B18 RID: 6936
		public class LogEntry
		{
			// Token: 0x0600D0C5 RID: 53445 RVA: 0x004B7AD0 File Offset: 0x004B5CD0
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

			// Token: 0x04009DF6 RID: 40438
			public readonly int MessageId;

			// Token: 0x04009DF7 RID: 40439
			public readonly DateTime Timestamp;

			// Token: 0x04009DF8 RID: 40440
			public readonly string IsoTime;

			// Token: 0x04009DF9 RID: 40441
			public readonly string Message;

			// Token: 0x04009DFA RID: 40442
			public readonly string Trace;

			// Token: 0x04009DFB RID: 40443
			public readonly LogType Type;

			// Token: 0x04009DFC RID: 40444
			public readonly long Uptime;
		}
	}
}
