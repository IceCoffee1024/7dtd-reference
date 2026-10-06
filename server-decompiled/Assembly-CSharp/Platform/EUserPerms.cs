using System;

namespace Platform
{
	// Token: 0x02001BDF RID: 7135
	[Flags]
	public enum EUserPerms
	{
		// Token: 0x0400A1D8 RID: 41432
		Multiplayer = 1,
		// Token: 0x0400A1D9 RID: 41433
		Communication = 2,
		// Token: 0x0400A1DA RID: 41434
		Crossplay = 4,
		// Token: 0x0400A1DB RID: 41435
		HostMultiplayer = 8,
		// Token: 0x0400A1DC RID: 41436
		All = 15
	}
}
