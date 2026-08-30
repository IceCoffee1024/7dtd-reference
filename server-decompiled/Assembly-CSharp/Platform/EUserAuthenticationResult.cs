using System;

namespace Platform
{
	// Token: 0x02001B64 RID: 7012
	public enum EUserAuthenticationResult
	{
		// Token: 0x0400A086 RID: 41094
		Ok,
		// Token: 0x0400A087 RID: 41095
		UserNotConnectedToPlatform,
		// Token: 0x0400A088 RID: 41096
		NoLicenseOrExpired,
		// Token: 0x0400A089 RID: 41097
		PlatformBanned,
		// Token: 0x0400A08A RID: 41098
		LoggedInElseWhere,
		// Token: 0x0400A08B RID: 41099
		PlatformBanCheckTimedOut,
		// Token: 0x0400A08C RID: 41100
		AuthTicketCanceled,
		// Token: 0x0400A08D RID: 41101
		AuthTicketInvalidAlreadyUsed,
		// Token: 0x0400A08E RID: 41102
		AuthTicketInvalid,
		// Token: 0x0400A08F RID: 41103
		PublisherIssuedBan,
		// Token: 0x0400A090 RID: 41104
		EosTicketFailed = 50
	}
}
