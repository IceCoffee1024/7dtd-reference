using System;
using System.Diagnostics;

namespace Platform.XBL.Save
{
	// Token: 0x02001C6F RID: 7279
	public static class GameCoreSaveHelpers
	{
		// Token: 0x0600D7D7 RID: 55255 RVA: 0x004D6E6C File Offset: 0x004D506C
		[Conditional("NEVER_DEFINED")]
		public static void TraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}

		// Token: 0x0600D7D8 RID: 55256 RVA: 0x004D6E6C File Offset: 0x004D506C
		public static void NonTraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}
	}
}
