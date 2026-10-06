using System;

namespace Platform
{
	// Token: 0x02001C1B RID: 7195
	public interface IPlatformUserBlockedData
	{
		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x0600D609 RID: 54793
		EBlockType Type { get; }

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x0600D60A RID: 54794
		EUserBlockState State { get; }

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x0600D60B RID: 54795
		// (set) Token: 0x0600D60C RID: 54796
		bool Locally { get; set; }
	}
}
