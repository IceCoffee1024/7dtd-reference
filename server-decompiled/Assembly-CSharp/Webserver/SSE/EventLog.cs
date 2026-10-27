using System;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.UrlHandlers;
using Webserver.WebAPI.APIs;

namespace Webserver.SSE
{
	// Token: 0x02001B58 RID: 7000
	[Preserve]
	public class EventLog : AbsEvent
	{
		// Token: 0x0600D220 RID: 53792 RVA: 0x004C20E2 File Offset: 0x004C02E2
		public EventLog(SseHandler _parent)
			: base(_parent, true, "log")
		{
			LogBuffer.EntryAdded += this.LogCallback;
		}

		// Token: 0x0600D221 RID: 53793 RVA: 0x004C2104 File Offset: 0x004C0304
		[PublicizedFrom(EAccessModifier.Private)]
		public void LogCallback(LogBuffer.LogEntry _logEntry)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			LogApi.WriteLogMessageObject(ref jsonWriter, _logEntry);
			base.SendData("logLine", jsonWriter.ToString());
		}
	}
}
