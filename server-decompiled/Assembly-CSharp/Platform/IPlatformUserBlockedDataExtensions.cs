using System;

namespace Platform
{
	// Token: 0x02001C4D RID: 7245
	public static class IPlatformUserBlockedDataExtensions
	{
		// Token: 0x0600D721 RID: 55073 RVA: 0x004D36FB File Offset: 0x004D18FB
		public static bool IsBlocked(this IPlatformUserBlockedData blockedData)
		{
			return blockedData.State.IsBlocked();
		}
	}
}
