using System;

namespace Platform
{
	// Token: 0x02001C10 RID: 7184
	[Flags]
	public enum EUserPerms
	{
		// Token: 0x0400A2C1 RID: 41665
		Multiplayer = 1,
		// Token: 0x0400A2C2 RID: 41666
		Communication = 2,
		// Token: 0x0400A2C3 RID: 41667
		Crossplay = 4,
		// Token: 0x0400A2C4 RID: 41668
		HostMultiplayer = 8,
		// Token: 0x0400A2C5 RID: 41669
		All = 15
	}
}
