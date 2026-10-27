using System;
using System.Diagnostics;

namespace Platform.XBL.Save
{
	// Token: 0x02001CA0 RID: 7328
	public static class GameCoreSaveHelpers
	{
		// Token: 0x0600D8EB RID: 55531 RVA: 0x004DA9BC File Offset: 0x004D8BBC
		[Conditional("NEVER_DEFINED")]
		public static void TraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}

		// Token: 0x0600D8EC RID: 55532 RVA: 0x004DA9BC File Offset: 0x004D8BBC
		public static void NonTraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}
	}
}
