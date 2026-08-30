using System;

namespace Platform
{
	// Token: 0x02001B3C RID: 6972
	public enum EBeginUserAuthenticationResult
	{
		// Token: 0x04009F4E RID: 40782
		Ok,
		// Token: 0x04009F4F RID: 40783
		InvalidTicket,
		// Token: 0x04009F50 RID: 40784
		DuplicateRequest,
		// Token: 0x04009F51 RID: 40785
		InvalidVersion,
		// Token: 0x04009F52 RID: 40786
		GameMismatch,
		// Token: 0x04009F53 RID: 40787
		ExpiredTicket
	}
}
