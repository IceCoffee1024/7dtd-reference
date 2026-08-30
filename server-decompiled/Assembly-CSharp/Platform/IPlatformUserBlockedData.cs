using System;

namespace Platform
{
	// Token: 0x02001BE9 RID: 7145
	public interface IPlatformUserBlockedData
	{
		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x0600D491 RID: 54417
		EBlockType Type { get; }

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x0600D492 RID: 54418
		EUserBlockState State { get; }

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x0600D493 RID: 54419
		// (set) Token: 0x0600D494 RID: 54420
		bool Locally { get; set; }
	}
}
