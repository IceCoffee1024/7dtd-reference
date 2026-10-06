using System;

namespace UnityEngine.Networking
{
	// Token: 0x02001915 RID: 6421
	public enum NetworkError
	{
		// Token: 0x04009559 RID: 38233
		Ok,
		// Token: 0x0400955A RID: 38234
		WrongHost,
		// Token: 0x0400955B RID: 38235
		WrongConnection,
		// Token: 0x0400955C RID: 38236
		WrongChannel,
		// Token: 0x0400955D RID: 38237
		NoResources,
		// Token: 0x0400955E RID: 38238
		BadMessage,
		// Token: 0x0400955F RID: 38239
		Timeout,
		// Token: 0x04009560 RID: 38240
		MessageToLong,
		// Token: 0x04009561 RID: 38241
		WrongOperation,
		// Token: 0x04009562 RID: 38242
		VersionMismatch,
		// Token: 0x04009563 RID: 38243
		CRCMismatch,
		// Token: 0x04009564 RID: 38244
		DNSFailure,
		// Token: 0x04009565 RID: 38245
		UsageError
	}
}
