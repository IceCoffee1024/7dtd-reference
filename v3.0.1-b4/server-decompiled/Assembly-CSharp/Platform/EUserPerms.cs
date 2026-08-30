using System;

namespace Platform
{
	// Token: 0x02001B86 RID: 7046
	[Flags]
	public enum EUserPerms
	{
		// Token: 0x04009FD2 RID: 40914
		Multiplayer = 1,
		// Token: 0x04009FD3 RID: 40915
		Communication = 2,
		// Token: 0x04009FD4 RID: 40916
		Crossplay = 4,
		// Token: 0x04009FD5 RID: 40917
		HostMultiplayer = 8,
		// Token: 0x04009FD6 RID: 40918
		All = 15
	}
}
