using System;

namespace Platform
{
	// Token: 0x02001B1A RID: 6938
	[Flags]
	public enum DeviceFlag
	{
		// Token: 0x04009EE0 RID: 40672
		None = 0,
		// Token: 0x04009EE1 RID: 40673
		StandaloneWindows = 1,
		// Token: 0x04009EE2 RID: 40674
		StandaloneLinux = 2,
		// Token: 0x04009EE3 RID: 40675
		StandaloneOSX = 4,
		// Token: 0x04009EE4 RID: 40676
		XBoxSeriesS = 8,
		// Token: 0x04009EE5 RID: 40677
		XBoxSeriesX = 16,
		// Token: 0x04009EE6 RID: 40678
		PS5 = 32
	}
}
