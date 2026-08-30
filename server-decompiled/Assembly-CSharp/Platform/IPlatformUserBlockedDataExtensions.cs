using System;

namespace Platform
{
	// Token: 0x02001BEA RID: 7146
	public static class IPlatformUserBlockedDataExtensions
	{
		// Token: 0x0600D495 RID: 54421 RVA: 0x004CB0EB File Offset: 0x004C92EB
		public static bool IsBlocked(this IPlatformUserBlockedData blockedData)
		{
			return blockedData.State.IsBlocked();
		}
	}
}
