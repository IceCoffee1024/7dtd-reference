using System;

namespace Platform
{
	// Token: 0x02001C1C RID: 7196
	public static class IPlatformUserBlockedDataExtensions
	{
		// Token: 0x0600D60D RID: 54797 RVA: 0x004CFBD3 File Offset: 0x004CDDD3
		public static bool IsBlocked(this IPlatformUserBlockedData blockedData)
		{
			return blockedData.State.IsBlocked();
		}
	}
}
