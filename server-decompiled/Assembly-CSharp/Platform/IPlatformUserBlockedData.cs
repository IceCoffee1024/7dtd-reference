using System;

namespace Platform
{
	// Token: 0x02001BC2 RID: 7106
	public interface IPlatformUserBlockedData
	{
		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x0600D303 RID: 54019
		EBlockType Type { get; }

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x0600D304 RID: 54020
		EUserBlockState State { get; }

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x0600D305 RID: 54021
		// (set) Token: 0x0600D306 RID: 54022
		bool Locally { get; set; }
	}
}
