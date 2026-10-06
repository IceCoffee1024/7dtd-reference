using System;

namespace Platform
{
	// Token: 0x02001B96 RID: 7062
	public enum EUserAuthenticationResult
	{
		// Token: 0x0400A15B RID: 41307
		Ok,
		// Token: 0x0400A15C RID: 41308
		UserNotConnectedToPlatform,
		// Token: 0x0400A15D RID: 41309
		NoLicenseOrExpired,
		// Token: 0x0400A15E RID: 41310
		PlatformBanned,
		// Token: 0x0400A15F RID: 41311
		LoggedInElseWhere,
		// Token: 0x0400A160 RID: 41312
		PlatformBanCheckTimedOut,
		// Token: 0x0400A161 RID: 41313
		AuthTicketCanceled,
		// Token: 0x0400A162 RID: 41314
		AuthTicketInvalidAlreadyUsed,
		// Token: 0x0400A163 RID: 41315
		AuthTicketInvalid,
		// Token: 0x0400A164 RID: 41316
		PublisherIssuedBan,
		// Token: 0x0400A165 RID: 41317
		EosTicketFailed = 50
	}
}
