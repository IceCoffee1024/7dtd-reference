using System;

namespace Platform
{
	// Token: 0x02001C4C RID: 7244
	public interface IPlatformUserBlockedData
	{
		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x0600D71D RID: 55069
		EBlockType Type { get; }

		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x0600D71E RID: 55070
		EUserBlockState State { get; }

		// Token: 0x17001B50 RID: 6992
		// (get) Token: 0x0600D71F RID: 55071
		// (set) Token: 0x0600D720 RID: 55072
		bool Locally { get; set; }
	}
}
