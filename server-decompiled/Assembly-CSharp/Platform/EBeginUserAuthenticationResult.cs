using System;

namespace Platform
{
	// Token: 0x02001B63 RID: 7011
	public enum EBeginUserAuthenticationResult
	{
		// Token: 0x0400A07F RID: 41087
		Ok,
		// Token: 0x0400A080 RID: 41088
		InvalidTicket,
		// Token: 0x0400A081 RID: 41089
		DuplicateRequest,
		// Token: 0x0400A082 RID: 41090
		InvalidVersion,
		// Token: 0x0400A083 RID: 41091
		GameMismatch,
		// Token: 0x0400A084 RID: 41092
		ExpiredTicket
	}
}
