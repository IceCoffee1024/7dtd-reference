using System;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.UrlHandlers;
using Webserver.WebAPI.APIs;

namespace Webserver.SSE
{
	// Token: 0x02001AFB RID: 6907
	[Preserve]
	public class EventLog : AbsEvent
	{
		// Token: 0x0600CFB4 RID: 53172 RVA: 0x004BA11A File Offset: 0x004B831A
		public EventLog(SseHandler _parent)
			: base(_parent, true, "log")
		{
			LogBuffer.EntryAdded += this.LogCallback;
		}

		// Token: 0x0600CFB5 RID: 53173 RVA: 0x004BA13C File Offset: 0x004B833C
		[PublicizedFrom(EAccessModifier.Private)]
		public void LogCallback(LogBuffer.LogEntry _logEntry)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			LogApi.WriteLogMessageObject(ref jsonWriter, _logEntry);
			base.SendData("logLine", jsonWriter.ToString());
		}
	}
}
