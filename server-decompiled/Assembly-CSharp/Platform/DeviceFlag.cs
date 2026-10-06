using System;

namespace Platform
{
	// Token: 0x02001B73 RID: 7027
	[Flags]
	public enum DeviceFlag
	{
		// Token: 0x0400A0E6 RID: 41190
		None = 0,
		// Token: 0x0400A0E7 RID: 41191
		StandaloneWindows = 1,
		// Token: 0x0400A0E8 RID: 41192
		StandaloneLinux = 2,
		// Token: 0x0400A0E9 RID: 41193
		StandaloneOSX = 4,
		// Token: 0x0400A0EA RID: 41194
		XBoxSeriesS = 8,
		// Token: 0x0400A0EB RID: 41195
		XBoxSeriesX = 16,
		// Token: 0x0400A0EC RID: 41196
		PS5 = 32
	}
}
