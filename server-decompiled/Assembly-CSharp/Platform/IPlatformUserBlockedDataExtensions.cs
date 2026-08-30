using System;

namespace Platform
{
	// Token: 0x02001BC3 RID: 7107
	public static class IPlatformUserBlockedDataExtensions
	{
		// Token: 0x0600D307 RID: 54023 RVA: 0x004C4CF7 File Offset: 0x004C2EF7
		public static bool IsBlocked(this IPlatformUserBlockedData blockedData)
		{
			return blockedData.State.IsBlocked();
		}
	}
}
