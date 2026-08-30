using System;

namespace Platform
{
	// Token: 0x02001B41 RID: 6977
	[Flags]
	public enum DeviceFlag
	{
		// Token: 0x0400A011 RID: 40977
		None = 0,
		// Token: 0x0400A012 RID: 40978
		StandaloneWindows = 1,
		// Token: 0x0400A013 RID: 40979
		StandaloneLinux = 2,
		// Token: 0x0400A014 RID: 40980
		StandaloneOSX = 4,
		// Token: 0x0400A015 RID: 40981
		XBoxSeriesS = 8,
		// Token: 0x0400A016 RID: 40982
		XBoxSeriesX = 16,
		// Token: 0x0400A017 RID: 40983
		PS5 = 32
	}
}
