using System;
using System.Diagnostics;

namespace Platform.XBL.Save
{
	// Token: 0x02001C3D RID: 7229
	public static class GameCoreSaveHelpers
	{
		// Token: 0x0600D65E RID: 54878 RVA: 0x004D2330 File Offset: 0x004D0530
		[Conditional("NEVER_DEFINED")]
		public static void TraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}

		// Token: 0x0600D65F RID: 54879 RVA: 0x004D2330 File Offset: 0x004D0530
		public static void NonTraceLogHR(int hr, string identifier)
		{
			XblHelpers.LogHR(hr, identifier, false);
		}
	}
}
