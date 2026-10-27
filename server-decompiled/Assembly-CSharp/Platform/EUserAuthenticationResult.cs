using System;

namespace Platform
{
	// Token: 0x02001BC7 RID: 7111
	public enum EUserAuthenticationResult
	{
		// Token: 0x0400A244 RID: 41540
		Ok,
		// Token: 0x0400A245 RID: 41541
		UserNotConnectedToPlatform,
		// Token: 0x0400A246 RID: 41542
		NoLicenseOrExpired,
		// Token: 0x0400A247 RID: 41543
		PlatformBanned,
		// Token: 0x0400A248 RID: 41544
		LoggedInElseWhere,
		// Token: 0x0400A249 RID: 41545
		PlatformBanCheckTimedOut,
		// Token: 0x0400A24A RID: 41546
		AuthTicketCanceled,
		// Token: 0x0400A24B RID: 41547
		AuthTicketInvalidAlreadyUsed,
		// Token: 0x0400A24C RID: 41548
		AuthTicketInvalid,
		// Token: 0x0400A24D RID: 41549
		PublisherIssuedBan,
		// Token: 0x0400A24E RID: 41550
		EosTicketFailed = 50
	}
}
