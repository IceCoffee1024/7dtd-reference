using System;

namespace Platform
{
	// Token: 0x02001B3D RID: 6973
	public enum EUserAuthenticationResult
	{
		// Token: 0x04009F55 RID: 40789
		Ok,
		// Token: 0x04009F56 RID: 40790
		UserNotConnectedToPlatform,
		// Token: 0x04009F57 RID: 40791
		NoLicenseOrExpired,
		// Token: 0x04009F58 RID: 40792
		PlatformBanned,
		// Token: 0x04009F59 RID: 40793
		LoggedInElseWhere,
		// Token: 0x04009F5A RID: 40794
		PlatformBanCheckTimedOut,
		// Token: 0x04009F5B RID: 40795
		AuthTicketCanceled,
		// Token: 0x04009F5C RID: 40796
		AuthTicketInvalidAlreadyUsed,
		// Token: 0x04009F5D RID: 40797
		AuthTicketInvalid,
		// Token: 0x04009F5E RID: 40798
		PublisherIssuedBan,
		// Token: 0x04009F5F RID: 40799
		EosTicketFailed = 50
	}
}
