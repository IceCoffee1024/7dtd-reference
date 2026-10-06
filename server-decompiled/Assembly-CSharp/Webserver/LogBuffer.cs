using System;
using System.Collections.Generic;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001AE6 RID: 6886
	public class LogBuffer
	{
		// Token: 0x14000119 RID: 281
		// (add) Token: 0x0600CFA6 RID: 53158 RVA: 0x004B3BD4 File Offset: 0x004B1DD4
		// (remove) Token: 0x0600CFA7 RID: 53159 RVA: 0x004B3C08 File Offset: 0x004B1E08
		public static event Action<LogBuffer.LogEntry> EntryAdded;

		// Token: 0x0600CFA8 RID: 53160 RVA: 0x004B3C3B File Offset: 0x004B1E3B
		public static void Init()
		{
			if (LogBuffer.instance == null)
			{
				LogBuffer.instance = new LogBuffer();
			}
		}

		// Token: 0x0600CFA9 RID: 53161 RVA: 0x004B3C4E File Offset: 0x004B1E4E
		[PublicizedFrom(EAccessModifier.Private)]
		public LogBuffer()
		{
			Log.LogCallbacksExtended += this.LogCallback;
		}

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x0600CFAA RID: 53162 RVA: 0x004B3C7D File Offset: 0x004B1E7D
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

		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x0600CFAB RID: 53163 RVA: 0x004B3C94 File Offset: 0x004B1E94
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

		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x0600CFAC RID: 53164 RVA: 0x004B3CD8 File Offset: 0x004B1ED8
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

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x0600CFAD RID: 53165 RVA: 0x004B3D28 File Offset: 0x004B1F28
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

		// Token: 0x17001A13 RID: 6675
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

		// Token: 0x0600CFAF RID: 53167 RVA: 0x004B3DDC File Offset: 0x004B1FDC
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

		// Token: 0x0600CFB0 RID: 53168 RVA: 0x004B3EA4 File Offset: 0x004B20A4
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

		// Token: 0x04009D07 RID: 40199
		[PublicizedFrom(EAccessModifier.Private)]
		public const int maxEntries = 3000;

		// Token: 0x04009D08 RID: 40200
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogBuffer instance;

		// Token: 0x04009D09 RID: 40201
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> logEntries = new List<LogBuffer.LogEntry>();

		// Token: 0x04009D0A RID: 40202
		[PublicizedFrom(EAccessModifier.Private)]
		public int listOffset;

		// Token: 0x04009D0C RID: 40204
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> emptyList = new List<LogBuffer.LogEntry>();

		// Token: 0x02001AE7 RID: 6887
		public class LogEntry
		{
			// Token: 0x0600CFB1 RID: 53169 RVA: 0x004B3FD4 File Offset: 0x004B21D4
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

			// Token: 0x04009D0D RID: 40205
			public readonly int MessageId;

			// Token: 0x04009D0E RID: 40206
			public readonly DateTime Timestamp;

			// Token: 0x04009D0F RID: 40207
			public readonly string IsoTime;

			// Token: 0x04009D10 RID: 40208
			public readonly string Message;

			// Token: 0x04009D11 RID: 40209
			public readonly string Trace;

			// Token: 0x04009D12 RID: 40210
			public readonly LogType Type;

			// Token: 0x04009D13 RID: 40211
			public readonly long Uptime;
		}
	}
}
