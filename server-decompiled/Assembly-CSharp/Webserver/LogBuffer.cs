using System;
using System.Collections.Generic;
using UnityEngine;

namespace Webserver
{
	// Token: 0x02001ABA RID: 6842
	public class LogBuffer
	{
		// Token: 0x1400011A RID: 282
		// (add) Token: 0x0600CE4E RID: 52814 RVA: 0x004AF708 File Offset: 0x004AD908
		// (remove) Token: 0x0600CE4F RID: 52815 RVA: 0x004AF73C File Offset: 0x004AD93C
		public static event Action<LogBuffer.LogEntry> EntryAdded;

		// Token: 0x0600CE50 RID: 52816 RVA: 0x004AF76F File Offset: 0x004AD96F
		public static void Init()
		{
			if (LogBuffer.instance == null)
			{
				LogBuffer.instance = new LogBuffer();
			}
		}

		// Token: 0x0600CE51 RID: 52817 RVA: 0x004AF782 File Offset: 0x004AD982
		[PublicizedFrom(EAccessModifier.Private)]
		public LogBuffer()
		{
			Log.LogCallbacksExtended += this.LogCallback;
		}

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x0600CE52 RID: 52818 RVA: 0x004AF7B1 File Offset: 0x004AD9B1
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

		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x0600CE53 RID: 52819 RVA: 0x004AF7C8 File Offset: 0x004AD9C8
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

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x0600CE54 RID: 52820 RVA: 0x004AF80C File Offset: 0x004ADA0C
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

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x0600CE55 RID: 52821 RVA: 0x004AF85C File Offset: 0x004ADA5C
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

		// Token: 0x17001971 RID: 6513
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

		// Token: 0x0600CE57 RID: 52823 RVA: 0x004AF910 File Offset: 0x004ADB10
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

		// Token: 0x0600CE58 RID: 52824 RVA: 0x004AF9D8 File Offset: 0x004ADBD8
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

		// Token: 0x04009C41 RID: 40001
		[PublicizedFrom(EAccessModifier.Private)]
		public const int maxEntries = 3000;

		// Token: 0x04009C42 RID: 40002
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogBuffer instance;

		// Token: 0x04009C43 RID: 40003
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> logEntries = new List<LogBuffer.LogEntry>();

		// Token: 0x04009C44 RID: 40004
		[PublicizedFrom(EAccessModifier.Private)]
		public int listOffset;

		// Token: 0x04009C46 RID: 40006
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<LogBuffer.LogEntry> emptyList = new List<LogBuffer.LogEntry>();

		// Token: 0x02001ABB RID: 6843
		public class LogEntry
		{
			// Token: 0x0600CE59 RID: 52825 RVA: 0x004AFB08 File Offset: 0x004ADD08
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

			// Token: 0x04009C47 RID: 40007
			public readonly int MessageId;

			// Token: 0x04009C48 RID: 40008
			public readonly DateTime Timestamp;

			// Token: 0x04009C49 RID: 40009
			public readonly string IsoTime;

			// Token: 0x04009C4A RID: 40010
			public readonly string Message;

			// Token: 0x04009C4B RID: 40011
			public readonly string Trace;

			// Token: 0x04009C4C RID: 40012
			public readonly LogType Type;

			// Token: 0x04009C4D RID: 40013
			public readonly long Uptime;
		}
	}
}
