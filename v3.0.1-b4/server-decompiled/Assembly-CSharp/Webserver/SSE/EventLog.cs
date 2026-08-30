using System;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.UrlHandlers;
using Webserver.WebAPI.APIs;

namespace Webserver.SSE
{
	// Token: 0x02001AD4 RID: 6868
	[Preserve]
	public class EventLog : AbsEvent
	{
		// Token: 0x0600CE2B RID: 52779 RVA: 0x004B3D26 File Offset: 0x004B1F26
		public EventLog(SseHandler _parent)
			: base(_parent, true, "log")
		{
			LogBuffer.EntryAdded += this.LogCallback;
		}

		// Token: 0x0600CE2C RID: 52780 RVA: 0x004B3D48 File Offset: 0x004B1F48
		[PublicizedFrom(EAccessModifier.Private)]
		public void LogCallback(LogBuffer.LogEntry _logEntry)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			LogApi.WriteLogMessageObject(ref jsonWriter, _logEntry);
			base.SendData("logLine", jsonWriter.ToString());
		}
	}
}
