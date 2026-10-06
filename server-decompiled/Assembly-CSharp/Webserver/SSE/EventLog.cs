using System;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.UrlHandlers;
using Webserver.WebAPI.APIs;

namespace Webserver.SSE
{
	// Token: 0x02001B27 RID: 6951
	[Preserve]
	public class EventLog : AbsEvent
	{
		// Token: 0x0600D10C RID: 53516 RVA: 0x004BE5E6 File Offset: 0x004BC7E6
		public EventLog(SseHandler _parent)
			: base(_parent, true, "log")
		{
			LogBuffer.EntryAdded += this.LogCallback;
		}

		// Token: 0x0600D10D RID: 53517 RVA: 0x004BE608 File Offset: 0x004BC808
		[PublicizedFrom(EAccessModifier.Private)]
		public void LogCallback(LogBuffer.LogEntry _logEntry)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			LogApi.WriteLogMessageObject(ref jsonWriter, _logEntry);
			base.SendData("logLine", jsonWriter.ToString());
		}
	}
}
