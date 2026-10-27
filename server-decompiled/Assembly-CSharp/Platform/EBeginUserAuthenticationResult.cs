using System;

namespace Platform
{
	// Token: 0x02001BC6 RID: 7110
	public enum EBeginUserAuthenticationResult
	{
		// Token: 0x0400A23D RID: 41533
		Ok,
		// Token: 0x0400A23E RID: 41534
		InvalidTicket,
		// Token: 0x0400A23F RID: 41535
		DuplicateRequest,
		// Token: 0x0400A240 RID: 41536
		InvalidVersion,
		// Token: 0x0400A241 RID: 41537
		GameMismatch,
		// Token: 0x0400A242 RID: 41538
		ExpiredTicket
	}
}
