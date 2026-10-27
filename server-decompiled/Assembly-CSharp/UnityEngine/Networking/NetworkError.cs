using System;

namespace UnityEngine.Networking
{
	// Token: 0x02001946 RID: 6470
	public enum NetworkError
	{
		// Token: 0x04009642 RID: 38466
		Ok,
		// Token: 0x04009643 RID: 38467
		WrongHost,
		// Token: 0x04009644 RID: 38468
		WrongConnection,
		// Token: 0x04009645 RID: 38469
		WrongChannel,
		// Token: 0x04009646 RID: 38470
		NoResources,
		// Token: 0x04009647 RID: 38471
		BadMessage,
		// Token: 0x04009648 RID: 38472
		Timeout,
		// Token: 0x04009649 RID: 38473
		MessageToLong,
		// Token: 0x0400964A RID: 38474
		WrongOperation,
		// Token: 0x0400964B RID: 38475
		VersionMismatch,
		// Token: 0x0400964C RID: 38476
		CRCMismatch,
		// Token: 0x0400964D RID: 38477
		DNSFailure,
		// Token: 0x0400964E RID: 38478
		UsageError
	}
}
