using System;

namespace Platform
{
	// Token: 0x02001BA4 RID: 7076
	[Flags]
	public enum DeviceFlag
	{
		// Token: 0x0400A1CF RID: 41423
		None = 0,
		// Token: 0x0400A1D0 RID: 41424
		StandaloneWindows = 1,
		// Token: 0x0400A1D1 RID: 41425
		StandaloneLinux = 2,
		// Token: 0x0400A1D2 RID: 41426
		StandaloneOSX = 4,
		// Token: 0x0400A1D3 RID: 41427
		XBoxSeriesS = 8,
		// Token: 0x0400A1D4 RID: 41428
		XBoxSeriesX = 16,
		// Token: 0x0400A1D5 RID: 41429
		PS5 = 32
	}
}
