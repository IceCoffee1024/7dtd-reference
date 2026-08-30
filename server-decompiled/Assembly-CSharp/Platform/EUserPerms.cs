using System;

namespace Platform
{
	// Token: 0x02001BAD RID: 7085
	[Flags]
	public enum EUserPerms
	{
		// Token: 0x0400A103 RID: 41219
		Multiplayer = 1,
		// Token: 0x0400A104 RID: 41220
		Communication = 2,
		// Token: 0x0400A105 RID: 41221
		Crossplay = 4,
		// Token: 0x0400A106 RID: 41222
		HostMultiplayer = 8,
		// Token: 0x0400A107 RID: 41223
		All = 15
	}
}
