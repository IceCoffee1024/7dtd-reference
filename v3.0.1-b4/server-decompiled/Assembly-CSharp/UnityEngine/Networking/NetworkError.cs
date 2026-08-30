using System;

namespace UnityEngine.Networking
{
	// Token: 0x020018C3 RID: 6339
	public enum NetworkError
	{
		// Token: 0x0400936A RID: 37738
		Ok,
		// Token: 0x0400936B RID: 37739
		WrongHost,
		// Token: 0x0400936C RID: 37740
		WrongConnection,
		// Token: 0x0400936D RID: 37741
		WrongChannel,
		// Token: 0x0400936E RID: 37742
		NoResources,
		// Token: 0x0400936F RID: 37743
		BadMessage,
		// Token: 0x04009370 RID: 37744
		Timeout,
		// Token: 0x04009371 RID: 37745
		MessageToLong,
		// Token: 0x04009372 RID: 37746
		WrongOperation,
		// Token: 0x04009373 RID: 37747
		VersionMismatch,
		// Token: 0x04009374 RID: 37748
		CRCMismatch,
		// Token: 0x04009375 RID: 37749
		DNSFailure,
		// Token: 0x04009376 RID: 37750
		UsageError
	}
}
