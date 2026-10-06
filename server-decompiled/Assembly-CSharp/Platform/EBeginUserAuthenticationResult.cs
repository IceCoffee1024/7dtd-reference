using System;

namespace Platform
{
	// Token: 0x02001B95 RID: 7061
	public enum EBeginUserAuthenticationResult
	{
		// Token: 0x0400A154 RID: 41300
		Ok,
		// Token: 0x0400A155 RID: 41301
		InvalidTicket,
		// Token: 0x0400A156 RID: 41302
		DuplicateRequest,
		// Token: 0x0400A157 RID: 41303
		InvalidVersion,
		// Token: 0x0400A158 RID: 41304
		GameMismatch,
		// Token: 0x0400A159 RID: 41305
		ExpiredTicket
	}
}
