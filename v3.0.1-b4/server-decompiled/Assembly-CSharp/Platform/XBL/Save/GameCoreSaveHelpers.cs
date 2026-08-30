using System;
using System.Diagnostics;

namespace Platform.XBL.Save
{
	// Token: 0x02001C16 RID: 7190
	public static class GameCoreSaveHelpers
	{
		// Token: 0x0600D4CE RID: 54478 RVA: 0x004CBF0C File Offset: 0x004CA10C
		[Conditional("NEVER_DEFINED")]
		public static void TraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}

		// Token: 0x0600D4CF RID: 54479 RVA: 0x004CBF0C File Offset: 0x004CA10C
		public static void NonTraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}
	}
}
