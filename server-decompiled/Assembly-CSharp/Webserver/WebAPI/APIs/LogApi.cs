using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs
{
	// Token: 0x02001B30 RID: 6960
	[Preserve]
	public class LogApi : AbsRestApi
	{
		// Token: 0x0600D144 RID: 53572 RVA: 0x004BCC1F File Offset: 0x004BAE1F
		public LogApi()
			: base("Log")
		{
		}

		// Token: 0x0600D145 RID: 53573 RVA: 0x004BCC2C File Offset: 0x004BAE2C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			int num;
			if (_context.QueryParameters["count"] == null || !int.TryParse(_context.QueryParameters["count"], out num))
			{
				num = 50;
			}
			if (num == 0)
			{
				num = 1;
			}
			if (num > 1000)
			{
				num = 1000;
			}
			if (num < -1000)
			{
				num = -1000;
			}
			int num2;
			if (_context.QueryParameters["firstLine"] == null || !int.TryParse(_context.QueryParameters["firstLine"], out num2))
			{
				num2 = ((num > 0) ? LogBuffer.Instance.OldestLine : LogBuffer.Instance.LatestLine);
			}
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(LogApi.jsonKeyEntries);
			int num3;
			List<LogBuffer.LogEntry> range = LogBuffer.Instance.GetRange(ref num2, num, out num3);
			jsonWriter.WriteBeginArray();
			for (int i = 0; i < range.Count; i++)
			{
				LogBuffer.LogEntry logEntry = range[i];
				if (i > 0)
				{
					jsonWriter.WriteValueSeparator();
				}
				LogApi.WriteLogMessageObject(ref jsonWriter, logEntry);
			}
			jsonWriter.WriteEndArray();
			jsonWriter.WriteRaw(LogApi.jsonKeyFirstLine);
			jsonWriter.WriteInt32(num2);
			jsonWriter.WriteRaw(LogApi.jsonKeyLastLine);
			jsonWriter.WriteInt32(num3);
			jsonWriter.WriteEndObject();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D146 RID: 53574 RVA: 0x004BCD74 File Offset: 0x004BAF74
		public static void WriteLogMessageObject(ref JsonWriter _writer, LogBuffer.LogEntry _logEntry)
		{
			_writer.WriteRaw(LogApi.jsonIdKey);
			_writer.WriteInt32(_logEntry.MessageId);
			_writer.WriteRaw(LogApi.jsonMsgKey);
			_writer.WriteString(_logEntry.Message);
			_writer.WriteRaw(LogApi.jsonTypeKey);
			_writer.WriteString(_logEntry.Type.ToStringCached<LogType>());
			_writer.WriteRaw(LogApi.jsonTraceKey);
			_writer.WriteString(_logEntry.Trace);
			_writer.WriteRaw(LogApi.jsonIsotimeKey);
			_writer.WriteString(_logEntry.IsoTime);
			_writer.WriteRaw(LogApi.jsonUptimeKey);
			_writer.WriteString(_logEntry.Uptime.ToString());
			_writer.WriteEndObject();
		}

		// Token: 0x04009E8C RID: 40588
		[PublicizedFrom(EAccessModifier.Private)]
		public const int maxCount = 1000;

		// Token: 0x04009E8D RID: 40589
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyEntries = JsonWriter.GetEncodedPropertyNameWithBeginObject("entries");

		// Token: 0x04009E8E RID: 40590
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyFirstLine = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("firstLine");

		// Token: 0x04009E8F RID: 40591
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyLastLine = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastLine");

		// Token: 0x04009E90 RID: 40592
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonIdKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("id");

		// Token: 0x04009E91 RID: 40593
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonMsgKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("msg");

		// Token: 0x04009E92 RID: 40594
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonTypeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009E93 RID: 40595
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonTraceKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("trace");

		// Token: 0x04009E94 RID: 40596
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonIsotimeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isotime");

		// Token: 0x04009E95 RID: 40597
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonUptimeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("uptime");
	}
}
